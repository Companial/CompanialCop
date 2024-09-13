using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0029OptionDataTypeNotAllowed : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0029OptionDataTypeNotAllowed);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.OptionDataType);

        public void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            if(context.ContainingSymbol.IsObsoleteRemoved || context.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
            {
                return;
            }

            var optionSymbol = (OptionDataTypeSyntax)context.Node;

            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0029OptionDataTypeNotAllowed, optionSymbol.GetLocation()));
        }
    }
}
