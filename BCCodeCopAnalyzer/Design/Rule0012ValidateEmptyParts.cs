using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0012ValidateEmptyParts : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0012EmptyActionsSection, DiagnosticDescriptors.Rule0013EmptyOnRunTrigger);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeOnRunTrigger), SyntaxKind.CodeunitObject);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeActionsSection), SyntaxKind.PageObject);
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
                        ReportEmptyOnRunTrigger(context, member.GetLocation());
                }
            }
        }

        private static void AnalyzeActionsSection(SyntaxNodeAnalysisContext context)
        {
            dynamic objectSyntax = null;
            switch (context.Node.Kind)
            {
                case SyntaxKind.PageObject:
                    objectSyntax = context.Node as PageSyntax;
                    break;
                case SyntaxKind.PageExtensionObject:
                    objectSyntax = context.Node as PageExtensionSyntax;
                    break;
            }
            PageActionListSyntax actionListSyntax = objectSyntax.Actions;

            if (actionListSyntax == null)
                return;
            if (actionListSyntax.Areas.Count == 0)
            {
                ReportEmptyActionSection(context, actionListSyntax.GetLocation());
            }
            else
            {
                foreach (PageActionAreaSyntax pageActionArea in actionListSyntax.Areas)
                {
                    if (pageActionArea.Actions.Count == 0)
                        ReportEmptyActionSection(context, actionListSyntax.GetLocation());
                }
            }


        }

        private static void ReportEmptyOnRunTrigger(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0013EmptyOnRunTrigger, location));
        }

        private static void ReportEmptyActionSection(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0012EmptyActionsSection, location));
        }
    }
}
