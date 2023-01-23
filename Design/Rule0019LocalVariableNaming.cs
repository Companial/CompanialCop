using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0019LocalVariableNaming : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0019LocalVariableNameShouldNotContainWhiteSpace, DiagnosticDescriptors.Rule0019LocalVariableNameShouldNotContainWildcardSymbols);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeLocalVariableNaming), SyntaxKind.VariableDeclaration);

        private void AnalyzeLocalVariableNaming(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;
            VariableDeclarationSyntax syntax = ctx.Node as VariableDeclarationSyntax;
            if (syntax != null)
            {
                if (syntax.Parent.IsKind(SyntaxKind.GlobalVarSection)) return;

                string variableName = syntax.GetNameStringValue();

                if (Extensions.ContainsWhiteSpace(variableName))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0019LocalVariableNameShouldNotContainWhiteSpace, syntax.Name.GetLocation(), syntax.Name));
                if (Extensions.ContainsWildCardSymbols(variableName))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0019LocalVariableNameShouldNotContainWildcardSymbols, syntax.Name.GetLocation(), syntax.Name));
            }
        }
    }
}