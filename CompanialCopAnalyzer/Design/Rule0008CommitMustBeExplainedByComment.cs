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
    public class Rule0008CommitMustBeExplainedByComment : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0008CommitMustBeExplainedByComment);

        public override void Initialize(AnalysisContext context) => context.RegisterOperationAction(new Action<OperationAnalysisContext>(this.CheckCommitForExplainingComment), OperationKind.InvocationExpression);

        private void CheckCommitForExplainingComment(OperationAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            IInvocationExpression operation = (IInvocationExpression)ctx.Operation;
            if (operation.TargetMethod.Name.ToUpper() == "COMMIT" && operation.TargetMethod.MethodKind == MethodKind.BuiltInMethod)
            {
                foreach (SyntaxTrivia trivia in operation.Syntax.Parent.GetLeadingTrivia())
                {
                    if (trivia.IsKind(SyntaxKind.LineCommentTrivia))
                    {
                        return;
                    }
                }
                foreach (SyntaxTrivia trivia in operation.Syntax.Parent.GetTrailingTrivia())
                {
                    if (trivia.IsKind(SyntaxKind.LineCommentTrivia))
                    {
                        return;
                    }
                }

                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0008CommitMustBeExplainedByComment, ctx.Operation.Syntax.GetLocation()));
            }
        }
    }
}