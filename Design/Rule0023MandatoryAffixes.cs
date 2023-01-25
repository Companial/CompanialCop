using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.Analyzers.Common.AppSourceCopConfiguration;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;


namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class RuleIdentifiersMustHaveValidAffixes : DiagnosticAnalyzer
    {
        private string[] affixes;
        private string affixesDisplayString;
        private static readonly ImmutableArray<SymbolKind> ObjectSymbolKindsToAnalyze = SymbolKindExtensions.AllRootObjectSymbolKinds.Where<SymbolKind>((Func<SymbolKind, bool>)(x => x != SymbolKind.DotNetPackage)).ToImmutableArray<SymbolKind>();
        private static readonly ImmutableArray<SymbolKind> ObjectSymbolKindsToAnalyzeForWarnings = ImmutableArray.Create<SymbolKind>(SymbolKind.ReportExtension);
        private static readonly ImmutableArray<SymbolKind> SymbolKindsThatShouldReportWarning = ImmutableArray.Create<SymbolKind>(SymbolKind.ReportDataItem, SymbolKind.ReportColumn, SymbolKind.ReportLabel);
        private static readonly Dictionary<SymbolKind, char> UnsupportedNameCharacterPerSymbolKind = new Dictionary<SymbolKind, char>()
    {
      {
        SymbolKind.ReportColumn,
        ' '
      }
    };
        private static readonly ImmutableArray<SymbolKind> TopMostObjectSymbolKinds = RuleIdentifiersMustHaveValidAffixes.ObjectSymbolKindsToAnalyze.Where<SymbolKind>((Func<SymbolKind, bool>)(x => !x.IsExtensionOrCustomizationObject())).ToImmutableArray<SymbolKind>();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0023MandatoryAffixes);

        public override void Initialize(AnalysisContext context) => context.RegisterCompilationStartAction(new Action<CompilationStartAnalysisContext>(this.CompilationStart));

        private void CompilationStart(CompilationStartAnalysisContext context)
        {
            this.affixes = AppSourceCopConfigurationProvider.GetMandatoryNameAffixes(AppSourceCopConfigurationProvider.GetAppSourceCopConfiguration(context.Compilation));
            this.affixesDisplayString = string.Join(", ", this.affixes);
            if (this.affixes.Length == 0)
                context.RegisterCompilationEndAction(new Action<CompilationAnalysisContext>(this.ReportMissingAffixes));
            else
                context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeObjectSymbolsInModule), RuleIdentifiersMustHaveValidAffixes.ObjectSymbolKindsToAnalyze);
        }

        private void ReportMissingAffixes(CompilationAnalysisContext context)
        {
            Location propertyLocation = AppSourceCopConfigurationProvider.GetAppSourceCopConfigurationPropertyLocation(context.Compilation, "MandatoryAffixes");
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, propertyLocation));
        }

        private void AnalyzeObjectSymbolsInModule(SymbolAnalysisContext ctx)
        {
            if (UpgradeVerificationHelper.IsObsoleteOrDeprecated(ctx.Symbol))
                return;
            this.AnalyzeObjectSymbols(ctx.Symbol, ctx.Symbol, ctx);
        }

        private void AnalyzeObjectSymbols(
          ISymbol symbol,
          ISymbol topMostSymbol,
          SymbolAnalysisContext ctx)
        {
            if (RuleIdentifiersMustHaveValidAffixes.ShouldSymbolBePrefixed(symbol, topMostSymbol))
                this.VerifyAffixesOfSymbol(symbol, ctx);
            if (!RuleIdentifiersMustHaveValidAffixes.IsSymbolTraversable(symbol))
                return;
            foreach (ISymbol member in ((IContainerSymbol)symbol).GetMembers())
                this.AnalyzeObjectSymbols(member, topMostSymbol, ctx);
        }

        private static bool IsSymbolTraversable(ISymbol symbol)
        {
            if (!(symbol is IContainerSymbol))
                return false;
            return symbol.Kind == SymbolKind.Change ? ((IChangeSymbol)symbol).ChangeKind.IsAdd() : !RuleIdentifiersMustHaveValidAffixes.TopMostObjectSymbolKinds.Contains(symbol.Kind);
        }

        private static bool ShouldSymbolBePrefixed(ISymbol symbol, ISymbol topMostSymbol)
        {
            if (symbol.IsSynthesized)
                return false;
            switch (symbol.Kind)
            {
                case SymbolKind.Method:
                    return ProcedureVerificationHelper.IsMethodExposedInTheCloud((IMethodSymbol)symbol);
                case SymbolKind.Field:
                    return true;
                case SymbolKind.Key:
                    return RuleIdentifiersMustHaveValidAffixes.ShouldKeyBePrefixed((IKeySymbol)symbol, topMostSymbol);
                case SymbolKind.Control:
                    return ((IControlSymbol)symbol).ControlKind != 0;
                case SymbolKind.Action:
                    return ((IActionSymbol)symbol).ActionKind != 0;
                case SymbolKind.EnumValue:
                    return true;
                case SymbolKind.ReportDataItem:
                case SymbolKind.ReportColumn:
                case SymbolKind.ReportLabel:
                    return true;
                case SymbolKind.ReportLayout:
                    return true;
                default:
                    return RuleIdentifiersMustHaveValidAffixes.ObjectSymbolKindsToAnalyze.Contains(symbol.Kind);
            }
        }

        private static bool ShouldKeyBePrefixed(IKeySymbol keySymbol, ISymbol topMostSymbol)
        {
            if (topMostSymbol.Kind != SymbolKind.TableExtension)
                return false;
            foreach (IFieldSymbol field in keySymbol.Fields)
            {
                if (field.ContainingType != null && field.ContainingType.NavTypeKind != NavTypeKind.TableExtension)
                    return true;
            }
            return false;
        }

        private void VerifyAffixesOfSymbol(ISymbol symbol, SymbolAnalysisContext ctx)
        {
            if (RuleIdentifiersMustHaveValidAffixes.VerifyAffixIsUsed(symbol, this.affixes) || UpgradeVerificationHelper.IsObsoleteOrDeprecated(symbol))
                return;
            if (symbol.Kind == SymbolKind.Method)
            {
                IApplicationObjectTypeSymbol objectTypeSymbol = symbol.GetContainingApplicationObjectTypeSymbol();
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, symbol.GetLocation(), (object)symbol.Name, (object)objectTypeSymbol.Kind, (object)objectTypeSymbol.Name, (object)this.affixesDisplayString));
            }
            else if (RuleIdentifiersMustHaveValidAffixes.ShouldItReportWarning(symbol))
                RuleIdentifiersMustHaveValidAffixes.ReportMissingAffixesWarningDiagnostic(ctx, symbol, this.affixesDisplayString);
            else
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, symbol.GetLocation(), (object)symbol.Name, (object)this.affixesDisplayString));
        }

        internal static void ReportMissingAffixesWarningDiagnostic(
          SymbolAnalysisContext ctx,
          ISymbol symbol,
          string affixesDisplayString)
        {
            ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, symbol.GetLocation(), (object)symbol.Name, (object)affixesDisplayString));
        }

        internal static bool VerifyAffixIsUsed(ISymbol symbol, string[] affixes)
        {
            if (RuleIdentifiersMustHaveValidAffixes.VerifyAffixIsUsed(symbol.Kind, symbol.Name, affixes))
                return true;
            if (symbol.Kind == SymbolKind.Action)
            {
                IActionSymbol actionSymbol = (IActionSymbol)symbol;
                if (actionSymbol.ActionKind == Microsoft.Dynamics.Nav.CodeAnalysis.ActionKind.ActionRef && actionSymbol.Name.EndsWith("_Promoted"))
                    return RuleIdentifiersMustHaveValidAffixes.VerifyAffixIsUsed(SymbolKind.Action, actionSymbol.Name.Substring(0, actionSymbol.Name.Length - "_Promoted".Length), affixes);
            }
            return false;
        }

        private static bool VerifyAffixIsUsed(
          SymbolKind symbolKind,
          string symbolName,
          string[] affixes)
        {
            if (string.IsNullOrEmpty(symbolName))
                return false;
            foreach (string affix in affixes)
            {
                char joker;
                if (RuleIdentifiersMustHaveValidAffixes.UnsupportedNameCharacterPerSymbolKind.TryGetValue(symbolKind, out joker) && affix.Contains<char>(joker))
                {
                    if (RuleIdentifiersMustHaveValidAffixes.ValidateAffixWithWhitespace(symbolName, affix, joker))
                        return true;
                }
                else if (symbolName.StartsWith(affix, StringComparison.OrdinalIgnoreCase) || symbolName.EndsWith(affix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static bool ShouldItReportWarning(ISymbol symbol) => RuleIdentifiersMustHaveValidAffixes.SymbolKindsThatShouldReportWarning.Contains(symbol.Kind) && RuleIdentifiersMustHaveValidAffixes.ObjectSymbolKindsToAnalyzeForWarnings.Contains(symbol.ContainingType.Kind);

        private static bool ValidateAffixWithWhitespace(string symbolName, string affix, char joker) => RuleIdentifiersMustHaveValidAffixes.ValidatePrefixWithWhitespace(symbolName, affix, joker) || RuleIdentifiersMustHaveValidAffixes.ValidateSuffixWithWhitespace(symbolName, affix, joker);

        private static bool ValidatePrefixWithWhitespace(string symbolName, string affix, char joker)
        {
            int index1 = 0;
            for (int index2 = 0; index2 < affix.Length; ++index2)
            {
                if ((int)affix[index2] != (int)joker && (int)affix[index2] != (int)symbolName[index1])
                    return false;
                ++index1;
            }
            return true;
        }

        private static bool ValidateSuffixWithWhitespace(string symbolName, string affix, char joker)
        {
            int index1 = symbolName.Length - 1;
            for (int index2 = affix.Length - 1; index2 >= 0; --index2)
            {
                if ((int)affix[index2] != (int)joker && (int)affix[index2] != (int)symbolName[index1])
                    return false;
                --index1;
            }
            return true;
        }
    }
}
