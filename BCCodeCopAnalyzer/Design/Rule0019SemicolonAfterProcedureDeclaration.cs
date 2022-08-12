﻿using BCCodeCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0019SemicolonAfterProcedureDeclaration : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0019SemicolonAfterProcedureDeclaration);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeSemicolonAfterProcedureDeclaration), SyntaxKind.MethodDeclaration);

        private void AnalyzeSemicolonAfterProcedureDeclaration(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            MethodDeclarationSyntax syntax = ctx.Node as MethodDeclarationSyntax;

            if (syntax.SemicolonToken.Kind != SyntaxKind.None)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0019SemicolonAfterProcedureDeclaration, syntax.SemicolonToken.GetLocation()));
            }
        }
    }
}