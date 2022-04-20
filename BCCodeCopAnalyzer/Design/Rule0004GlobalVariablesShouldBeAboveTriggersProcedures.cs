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
    internal class Rule0004GlobalVariablesShouldBeAboveTriggersProcedures : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0004GlobalVariablesMayBeAboveTriggersProcedures);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeGlobalVariables), SyntaxKind.GlobalVarSection);
        }

        public static void AnalyzeGlobalVariables(SyntaxNodeAnalysisContext context)
        {
            GlobalVarSectionSyntax? syntax = context.Node as GlobalVarSectionSyntax;
            AnalyzeGlobalVariablesPlacement(context, syntax);

        }

        public static void AnalyzeGlobalVariablesPlacement(SyntaxNodeAnalysisContext context, GlobalVarSectionSyntax? syntax)
        {
            if (syntax != null)
            {
                int globalVarSectionPosition = 0;
                int triggersSectionPosition = 0;
                int MethodSectionPosition = 0;
                SyntaxNode parent = syntax.Parent;
                dynamic parentSyntax = parent;
                if (parentSyntax == null)
                    return;

                    foreach (MemberSyntax member in parentSyntax.Members)
                    {
                        if (member.Kind == SyntaxKind.GlobalVarSection)
                        {
                            globalVarSectionPosition = member.Position;
                        }
                        if (member.Kind == SyntaxKind.TriggerDeclaration && triggersSectionPosition == 0)
                        {
                            triggersSectionPosition = member.Position;
                        }
                        if (member.Kind == SyntaxKind.MethodDeclaration && MethodSectionPosition == 0)
                        {
                            MethodSectionPosition = member.Position;
                        }
                    }
                if ((globalVarSectionPosition > triggersSectionPosition) || globalVarSectionPosition > MethodSectionPosition)
                {
                    if (context.Node != null)
                        ReportGlobalVariablesShouldbeBeforeTriggersProcedures(context, context.Node.GetLocation(), syntax.Kind);
                }
            }
        }

        private static void ReportGlobalVariablesShouldbeBeforeTriggersProcedures(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            SyntaxKind syntaxKind)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0004GlobalVariablesMayBeAboveTriggersProcedures, location, "", (object)syntaxKind));
        }
    }
}
