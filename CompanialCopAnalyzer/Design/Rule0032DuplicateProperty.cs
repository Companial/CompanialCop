using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0032DuplicateProperty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0032DuplicateProperty);
        private static readonly List<PropertyKind> SupportedPropertyKinds = new List<PropertyKind>
        {
            PropertyKind.DataClassification,
            PropertyKind.ApplicationArea
        };

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);

        public void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (context.Symbol.IsObsoleteRemoved || context.Symbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
            {
                return;
            }

            if(context.Symbol.ContainingSymbol is IObjectTypeSymbol)
            {
                return;
            }

            IPropertySymbol? propertySymbol = context.Symbol as IPropertySymbol;

            if(propertySymbol == null || !SupportedPropertyKinds.Contains(propertySymbol.PropertyKind))
            {
                return;
            }

            IObjectTypeSymbol objectTypeSymbol = context.Symbol.GetContainingObjectTypeSymbol();
            IPropertySymbol? objectPropertySymbol = objectTypeSymbol.GetProperty(propertySymbol.PropertyKind);


            if(objectPropertySymbol != null && 
                propertySymbol.GetPropertyValueSyntax<PropertyValueSyntax>().IsEquivalentTo(objectPropertySymbol.GetPropertyValueSyntax<PropertyValueSyntax>()))
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0032DuplicateProperty, propertySymbol.GetLocation(), propertySymbol.Name));
            }
        }
    }
}

