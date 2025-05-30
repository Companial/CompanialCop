using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design;

[DiagnosticAnalyzer]
public class Rule0042EnumsNeedToHaveInitValue : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0042EnumsNeedToHaveInitValue);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSymbolAction(AnalyzeField, SymbolKind.Field);
    }

    private void AnalyzeField(SymbolAnalysisContext context)
    {
        var fieldSymbol = (IFieldSymbol)context.Symbol;

        if (fieldSymbol.Type is not IEnumTypeSymbol enumType)
            return;

        var hasZeroValue = enumType.Values.Any(v => v.Ordinal == 0);
        var hasInitValue = fieldSymbol.Properties.Any(p => p.Name == "InitValue");

        if (!hasZeroValue && !hasInitValue)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0042EnumsNeedToHaveInitValue,
                fieldSymbol.GetLocation(), fieldSymbol.Name, enumType.Name));
        }
    }
}