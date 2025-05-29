using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0013ToolTipPunctuation : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0013ToolTipPunctuation);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeToolTipPunctuation), SyntaxKind.PageField, SyntaxKind.PageAction, SyntaxKind.Field);

        private void AnalyzeToolTipPunctuation(SyntaxNodeAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol))
                return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol()))
                return;

            if (ctx.Node?.GetProperty("ToolTip")?.Value is LabelPropertyValueSyntax toolTipProperty)
            {
                string tooltipValue = toolTipProperty.Value.LabelText.ToString();

                if (!tooltipValue.EndsWith(".'") && !tooltipValue.EndsWith(".)'"))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0013ToolTipPunctuation, toolTipProperty.GetLocation()));
            }
        }
    }
}