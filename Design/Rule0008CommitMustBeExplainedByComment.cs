﻿using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0008CommitMustBeExplainedByComment : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0008CommitMustBeExplainedByComment);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeCommitExpression, SyntaxKind.ExpressionStatement);

        public void AnalyzeCommitExpression(SyntaxNodeAnalysisContext context)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(context.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(context.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            ExpressionStatementSyntax? expressionStatementSyntax = context.Node as ExpressionStatementSyntax;

            if(expressionStatementSyntax == null) return;

            IBuiltInMethodTypeSymbol? builtInMethodTypeSymbol = context.SemanticModel.GetSymbolInfo(expressionStatementSyntax.Expression).Symbol as IBuiltInMethodTypeSymbol;
            if(builtInMethodTypeSymbol?.Name != "Commit" || builtInMethodTypeSymbol?.ContainingSymbol?.Name != "Database")
            {
                return;
            }

            if (context.Node.GetLeadingTrivia().Any(x => x.IsKind(SyntaxKind.LineCommentTrivia))) return;
            if (context.Node.GetTrailingTrivia().Any(x => x.IsKind(SyntaxKind.LineCommentTrivia))) return;

            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0008CommitMustBeExplainedByComment, context.Node.GetLocation()));
        }
    }
}