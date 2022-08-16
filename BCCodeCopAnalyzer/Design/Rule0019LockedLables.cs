using BCCodeCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0019LockedLables : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0019LockedVariableLables, DiagnosticDescriptors.Rule0019LockedVariableTokLables);
        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeLockedLabel), SymbolKind.GlobalVariable, SymbolKind.LocalVariable);

        private void AnalyzeLockedLabel(SymbolAnalysisContext ctx)
        {
            IVariableSymbol symbol = (IVariableSymbol)ctx.Symbol;

            ITypeSymbol type1 = symbol.Type;
            if ((type1 != null ? (type1.NavTypeKind != NavTypeKind.Label ? 1 : 0) : 1) != 0)
                return;

            ILabelTypeSymbol type = symbol.Type as ILabelTypeSymbol;
            if (type.Locked)
                if (!type.Name.EndsWith("Tok"))
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0019LockedVariableTokLables, symbol.GetLocation(), symbol.Name));

            if (type.Name.EndsWith("Tok"))
                if (!type.Locked)
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0019LockedVariableLables, symbol.GetLocation(), symbol.Name));

        }
    }
}