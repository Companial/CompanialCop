﻿using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0021ParameterNaming : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0021ParameterNameShouldNotContainWhiteSpace, DiagnosticDescriptors.Rule0021ParameterNameShouldNotContainWildcardSymbols);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeParameterNaming), SyntaxKind.Parameter);

        private void AnalyzeParameterNaming(SyntaxNodeAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.ContainingSymbol.GetContainingObjectTypeSymbol())) return;

            ParameterSyntax syntax = ctx.Node as ParameterSyntax;
            if (syntax.Parent.Parent.Kind == SyntaxKind.TriggerDeclaration) return;

            if (syntax != null)
            {
                string parameterName = syntax.GetNameStringValue();

                foreach (SyntaxNodeOrToken token in syntax.Parent.Parent.GetAnnotatedNodesAndTokens(AnnotationKind.AttributeType))
                {
                    if (token.ToString().Contains("EventSubscriber")) return;
                }
                if (Extensions.ContainsWhiteSpace(parameterName))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0021ParameterNameShouldNotContainWhiteSpace, syntax.Name.GetLocation(), syntax.Name));
                if (Extensions.ContainsWildCardSymbols(parameterName))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0021ParameterNameShouldNotContainWildcardSymbols, syntax.Name.GetLocation(), syntax.Name));
            }
        }
    }
}