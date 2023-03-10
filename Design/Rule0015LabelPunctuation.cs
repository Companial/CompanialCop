using System;
using System.Collections.Immutable;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0015LabelPunctuation : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0015LabelPunctuation);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeLabelPunctuation), SyntaxKind.VariableDeclaration);

        private void AnalyzeLabelPunctuation(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            VariableDeclarationSyntax syntax = (VariableDeclarationSyntax)ctx.Node;
            if (syntax != null)
            {
                string variableName = syntax.GetNameStringValue();
                if (syntax.Type.DataType.Kind != SyntaxKind.LabelDataType) return;

                dynamic labelDataType = syntax.Type.DataType;
                string labelText = labelDataType.Label.LabelText.Value.Text;

                if (variableName.EndsWith("Msg") || variableName.EndsWith("Err"))
                    if (!labelText.EndsWith(".'"))
                        ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0015LabelPunctuation, syntax.Name.GetLocation()));

                if (variableName.EndsWith("Qst"))
                    if (!labelText.Contains("?"))
                        ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0015LabelPunctuation, syntax.Name.GetLocation()));
            }
        }
    }
}