using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0022GridLayoutMustNotBeRows : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0022GridLayoutMustNotBeRows);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeGridLayoutValue), SyntaxKind.PageGroup);

        private void AnalyzeGridLayoutValue(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            PropertySyntax gridLayoutProperty = ctx.Node.GetProperty("GridLayout");

            if (gridLayoutProperty != null)
            {
                string propertyValue = gridLayoutProperty.Value.GetText().ToString();
                if (propertyValue.Equals("Rows"))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0022GridLayoutMustNotBeRows, gridLayoutProperty.GetLocation(), gridLayoutProperty.Name));
            }
        }
    }
}