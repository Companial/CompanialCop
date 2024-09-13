using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0012InternalProcedures : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0012InternalProcedures);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeInternalProcedures), SyntaxKind.MethodDeclaration);

        private void AnalyzeInternalProcedures(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().NavTypeKind == NavTypeKind.Interface) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            MethodDeclarationSyntax syntax = ctx.Node as MethodDeclarationSyntax;
            SyntaxNodeOrToken firstToken = syntax.ProcedureKeyword.GetPreviousToken();

            if (firstToken.Kind != SyntaxKind.LocalKeyword && firstToken.Kind != SyntaxKind.InternalKeyword)
            {
                var leadingTrivia = syntax.GetLeadingTrivia();
                var documentationComments = leadingTrivia.Where(x => x.Kind == SyntaxKind.SingleLineDocumentationCommentTrivia);

                if (documentationComments.Any())
                {
                    return;
                }

                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0012InternalProcedures, syntax.ProcedureKeyword.GetLocation()));
            }
        }
    }
}