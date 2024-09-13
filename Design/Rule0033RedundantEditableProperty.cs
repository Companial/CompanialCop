using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0033RedudantEditableProperty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0033RedundantEditableProperty);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        public void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (context.Symbol.IsObsoleteRemoved || context.Symbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
            {
                return;
            }

            if (context.Symbol.ContainingSymbol is IObjectTypeSymbol)
            {
                return;
            }

            IPropertySymbol? propertySymbol = (context.Symbol as IPropertySymbol);
            if (propertySymbol == null || propertySymbol.PropertyKind != PropertyKind.Editable)
            {
                return;
            }

            IObjectTypeSymbol objectTypeSymbol = context.Symbol.GetContainingObjectTypeSymbol();
            IPageTypeSymbol? pageTypeSymbol = null;
            IPageExtensionTypeSymbol? pageExtensionTypeSymbol = objectTypeSymbol as IPageExtensionTypeSymbol;

            if(pageExtensionTypeSymbol != null)
            {
                PageExtensionSyntax? pageExtensionSyntax = pageExtensionTypeSymbol.DeclaringSyntaxReference?.GetSyntax() as PageExtensionSyntax;

                if(pageExtensionSyntax == null)
                {
                    return;
                }

                pageTypeSymbol = GetObjectTypeSymbol(pageExtensionSyntax.BaseObject.Identifier as IdentifierNameSyntax, SymbolKind.Page, context.Compilation) as IPageTypeSymbol;
            }
            else
            {
                pageTypeSymbol = objectTypeSymbol as IPageTypeSymbol;
            }

            if(pageTypeSymbol == null)
            {
                return;
            }

            bool? pageExtensionValue = pageExtensionValue = pageExtensionTypeSymbol.GetSimplePropertyValue<bool>(PropertyKind.Editable);
            bool? pageValue = pageTypeSymbol.GetSimplePropertyValue<bool>(PropertyKind.Editable);

            if ((pageValue.HasValue && !pageValue.Value) || (pageExtensionValue != null && pageExtensionValue.HasValue && !pageExtensionValue.Value))
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0033RedundantEditableProperty, propertySymbol.GetLocation()));
            }
        }

        private IObjectTypeSymbol? GetObjectTypeSymbol(IdentifierNameSyntax? identifier, SymbolKind kind, Compilation compilation)
        {
            if (identifier == null) return null;
            string applicationObjectIdentifier = identifier.GetIdentifierOrLiteralValue() ?? string.Empty;
            IEnumerable<ISymbol> applicationObjects = compilation.GetApplicationObjectTypeSymbolsByNameAcrossModules(kind, applicationObjectIdentifier);
            if (applicationObjects.Count() > 1)
            {
                return null;
            }
            return applicationObjects.FirstOrDefault() as IObjectTypeSymbol;
        }
    }
}