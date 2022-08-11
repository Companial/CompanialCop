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
    internal class Rule0013ValidateEmptyParts : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0013EmptyOnRunTrigger);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeOnRunTrigger), SyntaxKind.CodeunitObject);
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

        private static void ReportEmptyOnRunTrigger(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0013EmptyOnRunTrigger, location));
        }
    }
}
