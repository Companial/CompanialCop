using CompanialCopAnalyzer;
using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0013ToolTipPunctuation : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0013ToolTipPunctuation);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeToolTipPunctuation), SyntaxKind.PageField, SyntaxKind.PageAction);

        private void AnalyzeToolTipPunctuation(SyntaxNodeAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            LabelPropertyValueSyntax? toolTipProperty = ctx.Node?.GetProperty("ToolTip")?.Value as LabelPropertyValueSyntax;

            bool showWarning = false;

            if (toolTipProperty != null)
            {
                string tooltipValue = toolTipProperty.Value.GetText().ToString();
                if (!tooltipValue.EndsWith(".'") && !tooltipValue.EndsWith(".)'"))
                {
                    showWarning = true;
                    if (toolTipProperty.Value.Properties != null)
                        foreach (IdentifierEqualsLiteralSyntax property in toolTipProperty.Value.Properties.Values)
                        {
                            if (property.Identifier.Text.ToLower() == "locked")
                            {
                                showWarning = false;
                                break;
                            }
                        }
                }
                    
                if (showWarning)
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0013ToolTipPunctuation, toolTipProperty.GetLocation()));
            }
        }
    }
}