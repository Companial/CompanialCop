using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0005EnumExtensionFieldsNumberedInDedicatedRange : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0005EnumExtensionsValuesNumberedInDedicatedRange);

        public override void Initialize(AnalysisContext context)
        {           
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeEnumExtValuesID), SyntaxKind.EnumExtensionType);
        }

        private static void AnalyzeEnumExtValuesID(SyntaxNodeAnalysisContext context)
        {
            EnumExtensionTypeSyntax enumExtensionTypeSyntax = context.Node as EnumExtensionTypeSyntax;
            string appAllowedRanges = GetAppObjectRange(context);
            AnalyzeEnumExtValuesIDs(context, enumExtensionTypeSyntax, appAllowedRanges);

        }

        public static void AnalyzeEnumExtValuesIDs(SyntaxNodeAnalysisContext context, EnumExtensionTypeSyntax enumExtensionTypeSyntax, string appAllowedRanges)
        {
            if (appAllowedRanges == null || enumExtensionTypeSyntax == null)
                return;

            if (enumExtensionTypeSyntax.Values != null)
            {
                SyntaxList<EnumValueSyntax> values = enumExtensionTypeSyntax.Values;
                foreach (EnumValueSyntax enumValueSyntax in values)
                {
                    SyntaxToken fieldNo = enumValueSyntax.Id;
                    if (!CheckNoInAllowedRange(int.Parse(fieldNo.Value.ToString()), appAllowedRanges))
                        ReportEnumValuesInAllowedRange(context, fieldNo.GetLocation(), fieldNo.Value.ToString(), appAllowedRanges, enumValueSyntax.Name);
                }
            }
        }

        private static string GetAppObjectRange(SyntaxNodeAnalysisContext context)
        {
            string allowedRanges = context.SemanticModel.Compilation.Options.IdSpaces.AllowedRanges;
            return allowedRanges;
        }

        private static bool CheckNoInAllowedRange(int FieldNo, string AllowedRange)
        {
            int fromID;
            int toID;
            MatchCollection matches = Regex.Matches(AllowedRange, @"\[(\d+)\.\.(\d+)\]");

            foreach (Match match in matches)
            {
                fromID = int.Parse(match.Groups[1].Value);
                toID = int.Parse(match.Groups[2].Value);
                if (Enumerable.Range(fromID, toID).Contains(FieldNo))
                    return true;
            }
            return false;
        }

        private static void ReportEnumValuesInAllowedRange(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location, string valueText, string allowedRange, IdentifierNameSyntax syntaxName)
        {
            if (syntaxNodeAnalysisContext.Node != null)
                syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0005EnumExtensionsValuesNumberedInDedicatedRange, location, (object)valueText, (object)allowedRange, (object)syntaxName));
        }
    }
}
