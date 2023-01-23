using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0017EmptyObjectSections : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0017EmptyObjectSections);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeOnRunTrigger), SyntaxKind.CodeunitObject);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzePageActions), SyntaxKind.PageActionList);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzePageExtensionActions), SyntaxKind.PageExtensionActionList);
        }
        private static void AnalyzeOnRunTrigger(SyntaxNodeAnalysisContext context)
        {
            CodeunitSyntax codeunitSyntax = context.Node as CodeunitSyntax;
            SyntaxList<MemberSyntax> members = codeunitSyntax.Members;
            foreach (dynamic member in codeunitSyntax.Members)
            {
                if (member.IsKind(SyntaxKind.TriggerDeclaration))
                {
                    if (member.Body.Statements.Count == 0)
                    {
                        ReportEmptyObjectSection(context, member.GetLocation());
                    }
                }
            }
        }

        private static void AnalyzePageActions(SyntaxNodeAnalysisContext context)
        {

            PageActionListSyntax actionListSyntax = context.Node as PageActionListSyntax;

            if (actionListSyntax == null) return;

            if (actionListSyntax.Areas.Count == 0)
            {
                ReportEmptyObjectSection(context, actionListSyntax.GetLocation());
            }
            else
            {
                foreach (PageActionAreaSyntax pageActionArea in actionListSyntax.Areas)
                {
                    if (pageActionArea.Actions.Count == 0)
                        ReportEmptyObjectSection(context, actionListSyntax.GetLocation());
                }
            }
        }

        private static void AnalyzePageExtensionActions(SyntaxNodeAnalysisContext context)
        {

            PageExtensionActionListSyntax actionListSyntax = context.Node as PageExtensionActionListSyntax;

            if (actionListSyntax == null) return;
        }

        private static void ReportEmptyObjectSection(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0017EmptyObjectSections, location));
        }
    }
}
