using System;
using System.Collections.Immutable;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0001KeyNaming : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0001KeyNaming);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeKeyNaming), SyntaxKind.TableObject);

        private void AnalyzeKeyNaming(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            TableSyntax syntax = ctx.Node as TableSyntax;
            KeyListSyntax keyList = syntax.Keys;

            if (keyList == null) return;

            KeySyntax key = keyList.Keys[0];
            if (key.GetNameStringValue() != "PK")
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0001KeyNaming, key.Name.GetLocation(), key.Name));
        }
    }
}