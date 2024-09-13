using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0003MethodNameShouldNotContainWhiteSpace : DiagnosticAnalyzer
    {

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0003MethodsNameShouldNotContainWhiteSpace);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeMethodNameSyntax), SyntaxKind.MethodDeclaration);
        }

        public static void AnalyzeMethodNameSyntax(SyntaxNodeAnalysisContext context)
        {
            if (context.Node != null)
            {
                MethodDeclarationSyntax syntax = GetMethodDeclarationSyntax(context);
                AnalyzeMethodName(context, syntax);
            }
        }

        public static MethodDeclarationSyntax GetMethodDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            MethodDeclarationSyntax? syntax = context.Node as MethodDeclarationSyntax;
            return syntax;
        }

        public static void AnalyzeMethodName(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax syntax)
        {
            if (syntax == null)
                return;
            IdentifierNameSyntax syntaxName = syntax.Name;

            SyntaxKind syntaxKind = syntax.Kind;
            string? name = syntax.GetNameStringValue();

            if (Extensions.ContainsWhiteSpace(name))
                ReportMethodNameMayNotContainWhiteSpace(context, syntaxName.GetLocation(), name, syntaxKind, syntaxName);
        }

        private static void ReportMethodNameMayNotContainWhiteSpace(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            string valueText,
            SyntaxKind syntaxKind,
            IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0003MethodsNameShouldNotContainWhiteSpace, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }
    }
}