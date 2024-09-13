using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    class Rule0010CheckForMissingCaptions : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0010CheckForMissingCaptions);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.CheckForMissingCaptions),
                SymbolKind.Page,
                SymbolKind.Query,
                SymbolKind.Table,
                SymbolKind.Field,
                SymbolKind.Action,
                SymbolKind.EnumValue,
                SymbolKind.Control
            );

        private void CheckForMissingCaptions(SymbolAnalysisContext context)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(context.Symbol)) return;
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(context.Symbol.GetContainingObjectTypeSymbol())) return;

            if (context.Symbol.Kind == SymbolKind.Control)
            {
                var Control = ((IControlSymbol)context.Symbol);
                switch (Control.ControlKind)
                {
                    case ControlKind.Field:
                        if (CaptionIsMissing(context.Symbol, context))
                            if (Control.RelatedFieldSymbol != null)
                            {
                                if (CaptionIsMissing(Control.RelatedFieldSymbol, context))
                                    RaiseCaptionWarning(context, context.Symbol.Name);
                            }
                            else
                            {
                                RaiseCaptionWarning(context, context.Symbol.Name);
                            }
                        break;

                    case ControlKind.Area:
                        break;

                    case ControlKind.Repeater:
                        break;

                    case ControlKind.Part:
                        if (CaptionIsMissing(context.Symbol, context))
                            if (Control.RelatedPartSymbol != null)
                                if (CaptionIsMissing(Control.RelatedPartSymbol, context))
                                    RaiseCaptionWarning(context, context.Symbol.Name);
                        break;

                    case ControlKind.UserControl:
                        break;

                    case ControlKind.SystemPart:
                        break;

                    default:
                        if (CaptionIsMissing(context.Symbol, context))
                            RaiseCaptionWarning(context, context.Symbol.Name);
                        break;
                }
            }
            else if (context.Symbol.Kind == SymbolKind.Action)
            {
                switch (((IActionSymbol)context.Symbol).ActionKind)
                {
                    case ActionKind.Action:
                        if (CaptionIsMissing(context.Symbol, context))
                            RaiseCaptionWarning(context, context.Symbol.Name);
                        break;

                    case ActionKind.Group:
                        if (CaptionIsMissing(context.Symbol, context))
                            RaiseCaptionWarning(context, context.Symbol.Name);
                        break;
                }
            }
            else if (context.Symbol.Kind == SymbolKind.Page)
            {
                if (((IPageTypeSymbol)context.Symbol).PageType != PageTypeKind.API)
                    if (CaptionIsMissing(context.Symbol, context))
                        RaiseCaptionWarning(context, context.Symbol.Name);
            }
            else
            {
                if (CaptionIsMissing(context.Symbol, context))
                    RaiseCaptionWarning(context, context.Symbol.Name);
            }
        }

        private bool CaptionIsMissing(ISymbol Symbol, SymbolAnalysisContext context)
        {
            try
            {
                if (Symbol.ContainingType.Kind == SymbolKind.Table)
                {
                    if (((ITableTypeSymbol)Symbol.ContainingType).Id >= 2000000000)
                        return false;
                    if (((IFieldSymbol)Symbol).Id >= 2000000000)
                        return false;
                }
            }
            catch (NullReferenceException)
            { }

            if (Symbol.GetBooleanPropertyValue(PropertyKind.ShowCaption) != false)
                if (Symbol.GetProperty(PropertyKind.Caption) == null && Symbol.GetProperty(PropertyKind.CaptionClass) == null)
                    return true;
            return false;

        }
        private void RaiseCaptionWarning(SymbolAnalysisContext context, string valueText)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0010CheckForMissingCaptions, context.Symbol.GetLocation(), valueText));
        }
    }
}