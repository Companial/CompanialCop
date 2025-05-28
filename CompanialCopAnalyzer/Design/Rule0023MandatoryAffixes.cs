using Microsoft.Dynamics.Nav.Analyzers.Common.AppSourceCopConfiguration;
using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class RuleIdentifiersMustHaveValidAffixes : DiagnosticAnalyzer
    {
        private string[] _affixes;
        private string _affixesDisplayString;

        private static readonly ImmutableArray<SymbolKind> ObjectSymbolKindsToAnalyze = SymbolKindExtensions.AllRootObjectSymbolKinds.Where(x => x != SymbolKind.DotNetPackage).ToImmutableArray();
        private static readonly ImmutableArray<SymbolKind> ObjectSymbolKindsToAnalyzeForWarnings = ImmutableArray.Create(SymbolKind.ReportExtension);
        private static readonly ImmutableArray<SymbolKind> SymbolKindsThatShouldReportWarning = ImmutableArray.Create(SymbolKind.ReportDataItem, SymbolKind.ReportColumn, SymbolKind.ReportLabel);
        private static readonly Dictionary<SymbolKind, char> UnsupportedNameCharacterPerSymbolKind = new(){ { SymbolKind.ReportColumn, ' ' } };
        private static readonly ImmutableArray<SymbolKind> TopMostObjectSymbolKinds = ObjectSymbolKindsToAnalyze.Where(x => !x.IsExtensionOrCustomizationObject()).ToImmutableArray();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, DiagnosticDescriptors.Rule0023MissingConfiguration);

        public override void Initialize(AnalysisContext context) 
            => context.RegisterCompilationStartAction(new Action<CompilationStartAnalysisContext>(this.CompilationStart));

        private void CompilationStart(CompilationStartAnalysisContext context)
        {
            AppSourceCopConfiguration configuration = AppSourceCopConfigurationProvider.GetConfiguration(context.Compilation);
            AppSourceCopConfiguration appSourceCopConfiguration = AppSourceCopConfigurationProvider.GetAppSourceCopConfiguration(context.Compilation);
            if (configuration == null && appSourceCopConfiguration == null)
            {
                context.RegisterCompilationEndAction(new Action<CompilationAnalysisContext>(ReportConfigurationMissing));
                return;
            }
            List<string> affixes = new();
            affixes.AddRange(AppSourceCopConfigurationProvider.GetMandatoryNameAffixes(configuration));
            affixes.AddRange(AppSourceCopConfigurationProvider.GetMandatoryNameAffixes(appSourceCopConfiguration));

            _affixes = affixes.Distinct().ToArray();
            _affixesDisplayString = string.Join(", ", _affixes);
            if (_affixes.Length == 0)
                context.RegisterCompilationEndAction(new Action<CompilationAnalysisContext>(ReportMissingAffixes));
            else
                context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(AnalyzeObjectSymbolsInModule), ObjectSymbolKindsToAnalyze);
        }

        private void ReportConfigurationMissing(CompilationAnalysisContext context)
        {
            Location propertyLocation = AppSourceCopConfigurationProvider.GetAppSourceCopConfigurationPropertyLocation(context.Compilation, "MandatoryAffixes");
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MissingConfiguration, propertyLocation));
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
            AnalyzeObjectSymbols(ctx.Symbol, ctx.Symbol, ctx);
        }

        private void AnalyzeObjectSymbols(ISymbol symbol, ISymbol topMostSymbol, SymbolAnalysisContext ctx)
        {
            if (ShouldSymbolBePrefixed(symbol, topMostSymbol))
                VerifyAffixesOfSymbol(symbol, ctx);
            if (!IsSymbolTraversable(symbol))
                return;
            foreach (ISymbol member in ((IContainerSymbol)symbol).GetMembers())
                AnalyzeObjectSymbols(member, topMostSymbol, ctx);
        }

        private static bool IsSymbolTraversable(ISymbol symbol)
        {
            if (symbol is not IContainerSymbol)
                return false;
            return symbol.Kind == SymbolKind.Change ? 
                ((IChangeSymbol)symbol).ChangeKind.IsAdd() 
                : !TopMostObjectSymbolKinds.Contains(symbol.Kind);
        }

        private static bool ShouldSymbolBePrefixed(ISymbol symbol, ISymbol topMostSymbol)
        {
            if (symbol.IsSynthesized)
                return false;
            return symbol.Kind switch
            {
                SymbolKind.Method => ProcedureVerificationHelper.IsMethodExposedInTheCloud((IMethodSymbol)symbol),
                SymbolKind.Field => true,
                SymbolKind.Key => ShouldKeyBePrefixed((IKeySymbol)symbol, topMostSymbol),
                SymbolKind.Control => ((IControlSymbol)symbol).ControlKind != 0,
                SymbolKind.Action => ((IActionSymbol)symbol).ActionKind != 0,
                SymbolKind.EnumValue or SymbolKind.ReportDataItem or SymbolKind.ReportColumn or SymbolKind.ReportLabel or SymbolKind.ReportLayout => true,
                SymbolKind.GlobalVariable => symbol.DeclaredAccessibility == Accessibility.Protected,
                _ => ObjectSymbolKindsToAnalyze.Contains(symbol.Kind),
            };
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
            if (VerifyAffixIsUsed(symbol, _affixes) || UpgradeVerificationHelper.IsObsoleteOrDeprecated(symbol))
                return;
            if (symbol.Kind == SymbolKind.Method)
            {
                IApplicationObjectTypeSymbol objectTypeSymbol = symbol.GetContainingApplicationObjectTypeSymbol();
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, symbol.GetLocation(), symbol.Name, objectTypeSymbol.Kind, objectTypeSymbol.Name, _affixesDisplayString));
            }
            else if (ShouldItReportWarning(symbol))
                ReportMissingAffixesWarningDiagnostic(ctx, symbol, _affixesDisplayString);
            else
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, symbol.GetLocation(), symbol.Name, _affixesDisplayString));
        }

        internal static void ReportMissingAffixesWarningDiagnostic(SymbolAnalysisContext ctx, ISymbol symbol, string affixesDisplayString)
        {
            ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0023MandatoryAffixes, symbol.GetLocation(), (object)symbol.Name, (object)affixesDisplayString));
        }

        internal static bool VerifyAffixIsUsed(ISymbol symbol, string[] affixes)
        {
            if (VerifyAffixIsUsed(symbol.Kind, symbol.Name, affixes))
                return true;
            if (symbol.Kind == SymbolKind.Action)
            {
                IActionSymbol actionSymbol = (IActionSymbol)symbol;
                if (actionSymbol.ActionKind == ActionKind.ActionRef && actionSymbol.Name.EndsWith("_Promoted"))
                    return VerifyAffixIsUsed(SymbolKind.Action, actionSymbol.Name.Substring(0, actionSymbol.Name.Length - "_Promoted".Length), affixes);
            }
            return false;
        }

        private static bool VerifyAffixIsUsed(SymbolKind symbolKind, string symbolName, string[] affixes)
        {
            if (string.IsNullOrEmpty(symbolName))
                return false;
            foreach (string affix in affixes)
            {
                if (UnsupportedNameCharacterPerSymbolKind.TryGetValue(symbolKind, out char joker) && affix.Contains<char>(joker))
                {
                    if (ValidateAffixWithWhitespace(symbolName, affix, joker))
                        return true;
                }
                else if (symbolName.StartsWith(affix, StringComparison.OrdinalIgnoreCase) || symbolName.EndsWith(affix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static bool ShouldItReportWarning(ISymbol symbol) 
            => SymbolKindsThatShouldReportWarning.Contains(symbol.Kind) 
            && ObjectSymbolKindsToAnalyzeForWarnings.Contains(symbol.ContainingType.Kind);

        #region Validation
        private static bool ValidateAffixWithWhitespace(string symbolName, string affix, char joker) 
            => ValidatePrefixWithWhitespace(symbolName, affix, joker) || ValidateSuffixWithWhitespace(symbolName, affix, joker);

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
        #endregion
    }
}
