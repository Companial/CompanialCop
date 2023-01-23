﻿using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0024EmptyCaptionLocked : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0024EmptyCaptionLocked);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeCaptionProperty), SyntaxKind.EnumValue);

        private void AnalyzeCaptionProperty(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            LabelPropertyValueSyntax captionProperty = ctx.Node.GetProperty("Caption").Value as LabelPropertyValueSyntax;

            if (captionProperty == null || captionProperty.Value.LabelText.Value.Value.ToString().Trim() != "" || ctx.ContainingSymbol.Kind != SymbolKind.Enum) return;

            bool labelLocked = false;

            if (captionProperty.Value.Properties != null)
                foreach (IdentifierEqualsLiteralSyntax property in captionProperty.Value.Properties.Values)
                {
                    if (property.Identifier.Text.ToLower() == "locked")
                    {
                        labelLocked = true;
                        break;
                    }
                }

            if (!labelLocked)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0024EmptyCaptionLocked, captionProperty.GetLocation()));
            }
        }
    }
}