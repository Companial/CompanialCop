﻿using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0026ZeroEnumValueReservedForEmpty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0026ZeroEnumValueReservedForEmpty);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeReservedEnum), SyntaxKind.EnumValue);

        private void AnalyzeReservedEnum(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            LabelPropertyValueSyntax? captionProperty = ctx.Node?.GetProperty("Caption")?.Value as LabelPropertyValueSyntax;
            EnumValueSyntax? enumValue = ctx.Node as EnumValueSyntax;

            if (enumValue == null) return;

            if (enumValue.Id.ValueText != "0" || ctx.ContainingSymbol.Kind != SymbolKind.Enum) return;

            if (enumValue.GetNameStringValue().Trim() != "")
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0026ZeroEnumValueReservedForEmpty, enumValue.Name.GetLocation()));
            
            if (captionProperty != null && captionProperty.Value.LabelText.Value.Value.ToString().Trim() != "")
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0026ZeroEnumValueReservedForEmpty, captionProperty.GetLocation()));
        }
    }
}