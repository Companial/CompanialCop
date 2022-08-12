﻿using BCCodeCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0016CommitMustBeExplainedByComment : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0016CommitMustBeExplainedByComment);

        public override void Initialize(AnalysisContext context) => context.RegisterOperationAction(new Action<OperationAnalysisContext>(this.CheckCommitForExplainingComment), OperationKind.InvocationExpression);

        private void CheckCommitForExplainingComment(OperationAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;

            IInvocationExpression operation = (IInvocationExpression)ctx.Operation;
            if (operation.TargetMethod.Name.ToUpper() == "COMMIT")
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

                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0016CommitMustBeExplainedByComment, ctx.Operation.Syntax.GetLocation()));
            }
        }
    }
}