using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0004First19TableFieldsIDReservedToPrimaryKey : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0004First19TableFieldsIDReservedToPrimaryKey);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeTableFieldsIDs), SyntaxKind.TableObject);
        }

        private static void AnalyzeTableFieldsIDs(SyntaxNodeAnalysisContext context)
        {
            TableSyntax? tableSyntax = context.Node as TableSyntax;
            KeyListSyntax? keys = tableSyntax.Keys;
            List<String> keysList = GetPrimaryKeyFieldsNames(keys);
            if (keysList == null)
                return;
            FieldListSyntax fieldList = tableSyntax.Fields;
            SyntaxList<FieldSyntax> fields = fieldList.Fields;
            foreach (FieldSyntax fieldSyntax in fields)
            {
                IdentifierNameSyntax fieldName = fieldSyntax.Name;
                SyntaxToken FieldNo = fieldSyntax.No;
                if (FieldNo.Value as int? < 20 && !keysList.Contains(fieldName.Identifier.Value.ToString()))
                {
                    ReportFieldsReservedForPrimaryKeyFields(context, FieldNo.GetLocation(), FieldNo.ValueText, fieldName);
                }
            }
        }

        private static List<String> GetPrimaryKeyFieldsNames(KeyListSyntax keyList)
        {
            if (keyList == null)
                return null;
            List<string> keys = new List<string>();
            if (keyList.Keys == null)
            {
                return null;
            }

            SyntaxList<KeySyntax> keysList = keyList.Keys;
            if (keysList != null)
            {
                KeySyntax keySyntax = keysList.First();
                SeparatedSyntaxList<IdentifierNameSyntax> fields = keySyntax.Fields;
                foreach (IdentifierNameSyntax identifierNameSyntax in fields)
                {
                    keys.Add(identifierNameSyntax.Identifier.Value.ToString());
                }
            }
            return keys;
        }

        private static void ReportFieldsReservedForPrimaryKeyFields(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location, string valueText, IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0004First19TableFieldsIDReservedToPrimaryKey, location, (object)valueText, (object)syntaxName));
        }
    }
}
