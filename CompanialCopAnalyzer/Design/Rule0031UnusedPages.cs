using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Diagnostics.Tracing;
using System.Collections;
using System.Runtime.Remoting.Contexts;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0031UnusedPages : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0031ObjectIsUnused);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(delegate (CompilationStartAnalysisContext compilationStartAnalysisContext)
            {
                ImmutableArray<IApplicationObjectTypeSymbol> applicationObjects = compilationStartAnalysisContext.Compilation.GetDeclaredApplicationObjectSymbols();
                UnusedObjectAnalyzer unusedObjectAnalyzer = new UnusedObjectAnalyzer(applicationObjects, compilationStartAnalysisContext.CancellationToken);

                compilationStartAnalysisContext.RegisterSyntaxNodeAction(unusedObjectAnalyzer.AnalyzeObjectReference, SyntaxKind.ObjectReference, SyntaxKind.ObjectNameReference, SyntaxKind.ObjectReferencePropertyValue);
                compilationStartAnalysisContext.RegisterSyntaxNodeAction(unusedObjectAnalyzer.AnalyzeObjectOptionAccess, SyntaxKind.OptionAccessExpression);
                compilationStartAnalysisContext.RegisterCompilationEndAction(unusedObjectAnalyzer.CompilationEndAction);
            });
        }

        private static void ReportUnusedObjects(Action<Diagnostic> action, IEnumerable unusedObjects)
        {
            foreach (IObjectTypeSymbol @object in unusedObjects)
            {
                Diagnostic diagnostic = Diagnostic.Create(DiagnosticDescriptors.Rule0031ObjectIsUnused, @object.GetLocation(), @object.Kind, @object.Name);
                action(diagnostic);
            }
        }

        private class UnusedObjectAnalyzer
        {
            private readonly Dictionary<SymbolKind, HashSet<ISymbol>> unusedObjects;

            public UnusedObjectAnalyzer(ImmutableArray<IApplicationObjectTypeSymbol> applicationObjects, CancellationToken cancellationToken)
            {
                unusedObjects = new Dictionary<SymbolKind, HashSet<ISymbol>>();

                ImmutableArray<IApplicationObjectTypeSymbol>.Enumerator enumerator = applicationObjects.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IApplicationObjectTypeSymbol applicationObject = enumerator.Current;

                    if(IsUnused(applicationObject))
                    {
                        var typeKind = applicationObject.Kind;
                        if(unusedObjects.ContainsKey(typeKind))
                        {
                            unusedObjects[typeKind].Add(applicationObject);
                        }
                        else
                        {
                            unusedObjects.Add(typeKind, new HashSet<ISymbol> { applicationObject });
                        }
                    }
                }
            }

            private bool IsUnused(IApplicationObjectTypeSymbol applicationObject)
            {
                if (applicationObject.IsObsoleteRemoved)
                    return false;

                switch (applicationObject)
                {
                    case IPageTypeSymbol page:
                        UsageCategoryKind? usageCategory = page.GetEnumPropertyValue<UsageCategoryKind>(PropertyKind.UsageCategory);
                        PageTypeKind? pageType = page.GetEnumPropertyValue<PageTypeKind>(PropertyKind.PageType);

                        if((pageType != null && pageType.Value == PageTypeKind.API) || (usageCategory != null && usageCategory != UsageCategoryKind.None))
                        {
                            return false;
                        }

                        return true;
                    case IEnumTypeSymbol:
                        return true;

                    default:
                        return false;
                }
            }

            public void AnalyzeObjectOptionAccess(SyntaxNodeAnalysisContext context)
            {
                if (unusedObjects.Count == 0)
                {
                    return;
                }

                if (context.ContainingSymbol.IsObsoleteRemoved || context.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
                {
                    return;
                }

                SymbolKind kind = SymbolKind.Undefined;
                OptionAccessExpressionSyntax? optionAccessExpressionSyntax = context.Node as OptionAccessExpressionSyntax;

                if(optionAccessExpressionSyntax?.Expression is IdentifierNameSyntax identifierNameSyntax)
                {
                    string Expression = identifierNameSyntax.GetIdentifierOrLiteralValue() ?? string.Empty;
                    kind = StringToSymbolKind(Expression);

                    if(!unusedObjects.ContainsKey(kind))
                    {
                        return;
                    }
                }

                if (kind == SymbolKind.Undefined)
                {
                    return;
                }

                IObjectTypeSymbol? objectTypeSymbol = GetObjectTypeSymbol(optionAccessExpressionSyntax?.Name, kind, context.SemanticModel.Compilation);

                RemoveObjectTypeSymbol(objectTypeSymbol);
            }

            public void AnalyzeObjectReference(SyntaxNodeAnalysisContext context)
            {
                if (unusedObjects.Count == 0)
                {
                    return;
                }

                if (context.ContainingSymbol.IsObsoleteRemoved || context.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
                {
                    return;
                }

                IObjectTypeSymbol? objectTypeSymbol;

                if (context.Node.Parent is SubtypedDataTypeSyntax dataTypeSyntax)
                {
                    objectTypeSymbol = GetSubTypedObjectTypeSymbol(dataTypeSyntax, context.SemanticModel.Compilation);
                }
                else if (context.ContainingSymbol is IPropertySymbol propertySymbol)
                {
                    if(propertySymbol.Value is IObjectTypeSymbol objectType)
                    {
                        objectTypeSymbol = objectType;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    objectTypeSymbol = GetExtensionObjectTypeSymbol(context);
                }

                RemoveObjectTypeSymbol(objectTypeSymbol);
            }

            private void RemoveObjectTypeSymbol(IObjectTypeSymbol? objectTypeSymbol)
            {
                if(objectTypeSymbol == null)
                {
                    return;
                }

                if (unusedObjects.TryGetValue(objectTypeSymbol.Kind, out HashSet<ISymbol> references))
                {
                    if (references.Contains(objectTypeSymbol))
                    {
                        references.Remove(objectTypeSymbol);

                        if (references.Count == 0)
                        {
                            unusedObjects.Remove(objectTypeSymbol.Kind);
                        }
                    }
                }
            }

            private IObjectTypeSymbol? GetExtensionObjectTypeSymbol(SyntaxNodeAnalysisContext context)
            {
                SymbolKind kind;

                switch (context.Node.Parent)
                {
                    case EnumDataTypeSyntax:
                    case EnumExtensionTypeSyntax:
                        kind = SymbolKind.Enum;
                        break;
                    case PageExtensionSyntax:
                        kind = SymbolKind.PageExtension;
                        break;

                    default:
                        return null;
                }

                IdentifierNameSyntax? identifier;
                if (context.Node is ObjectNameOrIdSyntax objectNameOrIdSyntax)
                {
                    identifier = objectNameOrIdSyntax.Identifier as IdentifierNameSyntax;
                }
                else if (context.Node is ObjectNameReferenceSyntax objectNameReferenceSyntax)
                {
                    identifier = objectNameReferenceSyntax.Identifier as IdentifierNameSyntax;
                }
                else
                {
                    return null;
                }

                return GetObjectTypeSymbol(identifier, kind, context.SemanticModel.Compilation);
            }

            private IObjectTypeSymbol? GetSubTypedObjectTypeSymbol(SubtypedDataTypeSyntax? syntax, Compilation compilation)
            {
                if(syntax == null)
                {
                    return null;
                }

                SymbolKind kind = StringToSymbolKind(syntax.TypeName.ValueText);

                if (kind == SymbolKind.Undefined)
                    return null;

                return GetObjectTypeSymbol(syntax.Subtype.Identifier as IdentifierNameSyntax, kind, compilation);
            }

            private SymbolKind StringToSymbolKind(string value)
            {
                NavTypeKind navTypeKind = NavTypeExtensions.GetNavTypeKind(value, false);
                return navTypeKind switch
                {
                    NavTypeKind.TestPage or NavTypeKind.Page => SymbolKind.Page,
                    NavTypeKind.Record => SymbolKind.Table,
                    NavTypeKind.TestRequestPage or NavTypeKind.Report => SymbolKind.Report,
                    NavTypeKind.Query => SymbolKind.Query,
                    NavTypeKind.XmlPort => SymbolKind.XmlPort,
                    NavTypeKind.Enum => SymbolKind.Enum,
                    NavTypeKind.ControlAddIn => SymbolKind.ControlAddIn,
                    NavTypeKind.DotNet => SymbolKind.DotNet,
                    NavTypeKind.Interface => SymbolKind.Interface,
                    NavTypeKind.Codeunit => SymbolKind.Codeunit,
                    _ => SymbolKind.Undefined
                };
            }

            private IObjectTypeSymbol? GetObjectTypeSymbol(IdentifierNameSyntax? identifier, SymbolKind kind, Compilation compilation)
            {
                if(identifier == null) return null;

                string applicationObjectIdentifier = identifier.GetIdentifierOrLiteralValue() ?? string.Empty;
                IEnumerable<ISymbol> applicationObjects = compilation.GetApplicationObjectTypeSymbolsByNameAcrossModules(kind, applicationObjectIdentifier);

                if (applicationObjects.Count() > 1)
                {
                    return null;
                }
                return applicationObjects.FirstOrDefault() as IObjectTypeSymbol;
            }

            public void CompilationEndAction(CompilationAnalysisContext context)
            {
                ReportUnusedObjects(new Action<Diagnostic>(context.ReportDiagnostic), unusedObjects.SelectMany(x => x.Value));
            }
        }
    }
}
