using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0036LocalEventPublisher : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0036LocalEventPublisher);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(AnalyzeEventPublisher), SymbolKind.Method);

        private void AnalyzeEventPublisher(SymbolAnalysisContext ctx)
        {                     
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.Symbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.Symbol.GetContainingObjectTypeSymbol())) return;

            IMethodSymbol? symbol = ctx.Symbol as IMethodSymbol;
            if (symbol is null) return;
            if (!symbol.IsEvent) return;

            if (!symbol.IsLocal)
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0036LocalEventPublisher, symbol.GetLocation()));            
        }
    }
}