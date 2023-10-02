using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using System.Runtime.Remoting.Contexts;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0034PageExtensionEditable : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0034PageExtensionEditableField);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        public void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (context.Symbol.IsObsoleteRemoved || context.Symbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
            {
                return;
            }

            IPropertySymbol? propertySymbol = (context.Symbol as IPropertySymbol);
            if (propertySymbol == null || propertySymbol.PropertyKind != PropertyKind.Editable)
            {
                return;
            }

            IPageTypeSymbol? pageTypeSymbol = GetBasePageSymbol(context.Symbol.GetContainingObjectTypeSymbol() as IPageExtensionTypeSymbol, context.Compilation);
            if (pageTypeSymbol == null)
            {
                return;
            }

            string? fieldName = (propertySymbol.ContainingSymbol as IChangeSymbol)?.Name;

            if(fieldName == null)
            {
                return;
            }

            IEnumerable<IControlSymbol> fields = pageTypeSymbol.FlattenedControls.Where(x => x.ControlKind == ControlKind.Field && x.Name == fieldName);

            if(fields.Count() > 1)
            {
                return;
            }

            IPropertySymbol? editablePropertySymbol = fields.First().GetProperty(PropertyKind.Editable);
            
            if(editablePropertySymbol == null)
            {
                return;
            }

            if(!bool.TryParse(editablePropertySymbol.ValueText, out bool _))
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0034PageExtensionEditableField, propertySymbol.GetLocation(), fieldName));
            }
        }

        private IPageTypeSymbol? GetBasePageSymbol(IPageExtensionTypeSymbol? pageExtensionTypeSymbol, Compilation compilation)
        {
            if (pageExtensionTypeSymbol == null)
            {
                return null;
            }

            PageExtensionSyntax? pageExtensionSyntax = pageExtensionTypeSymbol.DeclaringSyntaxReference?.GetSyntax() as PageExtensionSyntax;

            if (pageExtensionSyntax == null)
            {
                return null;
            }

            return GetObjectTypeSymbol(pageExtensionSyntax.BaseObject.Identifier as IdentifierNameSyntax, SymbolKind.Page, compilation) as IPageTypeSymbol;
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