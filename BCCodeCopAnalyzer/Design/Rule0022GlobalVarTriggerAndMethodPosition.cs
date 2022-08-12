using BCCodeCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0022GlobalVarTriggerAndMethodPosition : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0022GlobalVarTriggerAndMethodPosition);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeGlobalVariablesPlacement), SyntaxKind.TableObject, SyntaxKind.PageObject,
                                                                                                                                                                             SyntaxKind.CodeunitObject, SyntaxKind.ReportObject, SyntaxKind.QueryObject);

        private void AnalyzeGlobalVariablesPlacement(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            dynamic parentSyntax = ctx.Node;
            int globalVarSectionPosition = 0;
            int triggersSectionPosition = 0;
            int methodSectionPosition = 0;
            Location globalVarLocation = Location.None;
            Location triggerLocation = Location.None;

            if (parentSyntax != null)
                foreach (MemberSyntax member in parentSyntax.Members)
                {
                    if (member.Kind == SyntaxKind.GlobalVarSection)
                    {
                        globalVarSectionPosition = member.Position;
                        globalVarLocation = member.GetLocation();
                    }
                    if (member.Kind == SyntaxKind.TriggerDeclaration && triggersSectionPosition == 0)
                    {
                        triggersSectionPosition = member.Position;
                        triggerLocation = member.GetLocation();
                    }
                    if (member.Kind == SyntaxKind.MethodDeclaration && methodSectionPosition == 0)
                    {
                        methodSectionPosition = member.Position;
                    }

                    if (globalVarSectionPosition != 0 & triggersSectionPosition != 0 & methodSectionPosition != 0)
                        break;
                }

            if ((globalVarSectionPosition > triggersSectionPosition && triggersSectionPosition != 0) || (globalVarSectionPosition > methodSectionPosition && methodSectionPosition != 0))
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0022GlobalVarTriggerAndMethodPosition, globalVarLocation));

            if (triggersSectionPosition > methodSectionPosition && methodSectionPosition != 0)
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0022GlobalVarTriggerAndMethodPosition, triggerLocation));
        }
    }
}
