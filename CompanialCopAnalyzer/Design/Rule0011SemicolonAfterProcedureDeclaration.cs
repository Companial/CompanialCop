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
    public class Rule0011SemicolonAfterProcedureDeclaration : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0011SemicolonAfterProcedureDeclaration);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeSemicolonAfterProcedureDeclaration), SyntaxKind.MethodDeclaration);

        private void AnalyzeSemicolonAfterProcedureDeclaration(SyntaxNodeAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            MethodDeclarationSyntax syntax = ctx.Node as MethodDeclarationSyntax;

            if (syntax.SemicolonToken.Kind != SyntaxKind.None)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0011SemicolonAfterProcedureDeclaration, syntax.SemicolonToken.GetLocation()));
            }
        }
    }
}