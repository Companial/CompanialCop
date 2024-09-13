using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0007FlowFieldsShouldNotBeEditable : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0007FlowFieldsShouldNotBeEditable);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeFlowFieldEditable), SymbolKind.Field);

        private void AnalyzeFlowFieldEditable(SymbolAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.Symbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.Symbol.GetContainingObjectTypeSymbol())) return;

            IFieldSymbol field = (IFieldSymbol)ctx.Symbol;
            if (field.FieldClass == FieldClassKind.FlowField && field.GetBooleanPropertyValue(PropertyKind.Editable).Value)
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0007FlowFieldsShouldNotBeEditable, field.Location, field.Name));
        }
    }
}