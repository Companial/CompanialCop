using CompanialCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0037GlobalVariablePrefix : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0037GlobalVariablePrefix);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeGlobalVariableNaming), SyntaxKind.GlobalVarSection);

        private void AnalyzeGlobalVariableNaming(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) 
                return;

            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) 
                return;

            GlobalVarSectionSyntax syntax = ctx.Node as GlobalVarSectionSyntax;
            if (syntax is null) 
                return;

            foreach(VariableDeclarationSyntax variableSyntax in syntax.Variables) 
            {
                string variableName = variableSyntax.GetNameStringValue();

                if (!variableName.StartsWith("g"))
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0037GlobalVariablePrefix, variableSyntax.Name.GetLocation()));
                }
            }
        }
    }
}