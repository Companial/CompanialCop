using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0011EnumExtensionFieldsNumberedInDedicatedRange : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0011EnumExtensionsValuesNumberedInDedicatedRange);

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
                        ReportEnumValuesInAllowedRange(context, fieldNo.GetLocation(), enumValueSyntax.Name.Identifier.Value.ToString(), enumValueSyntax.Name);

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

        private static void ReportEnumValuesInAllowedRange(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location, string valueText, IdentifierNameSyntax syntaxName)
        {
            if (syntaxNodeAnalysisContext.Node != null)
                syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0011EnumExtensionsValuesNumberedInDedicatedRange, location, (object)valueText, (object)syntaxName));
        }
    }
}
