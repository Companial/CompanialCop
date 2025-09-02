using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0034RedudantEditableProperty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(DiagnosticDescriptors.Rule0034TableRelationTooLong);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.Field);

        public void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not IFieldSymbol currentFieldSymbol)
            {
                return;
            }

            var containing = currentFieldSymbol.GetContainingObjectTypeSymbol();
            if (currentFieldSymbol.IsObsoleteRemoved || (containing != null && containing.IsObsoleteRemoved))
            {
                return;
            }

            var tableRelation = currentFieldSymbol.GetProperty(PropertyKind.TableRelation);
            if (tableRelation == null)
            {
                return;
            }

            var tableRelationValueSyntax = tableRelation.GetPropertyValueSyntax<TableRelationPropertyValueSyntax>();

            IFieldSymbol fieldSymbol;
            while (tableRelationValueSyntax != null)
            {
                string tableRelationsTargetFieldName;

                if (tableRelationValueSyntax.RelatedTableField is QualifiedNameSyntax relatedTableField)
                {
                    var left = relatedTableField.Left as IdentifierNameSyntax;
                    var right = relatedTableField.Right as IdentifierNameSyntax;

                    fieldSymbol = GetRelatedFieldSymbol(left, right, context.SemanticModel.Compilation);
                    tableRelationsTargetFieldName = relatedTableField.ToString();
                }
                // for a case where table field is not mentioned, use table's primary key
                else if (tableRelationValueSyntax.RelatedTableField is IdentifierNameSyntax identifierTableField)
                {
                    fieldSymbol = GetRelatedFieldSymbol(identifierTableField, null, context.SemanticModel.Compilation);
                    tableRelationsTargetFieldName = $"{identifierTableField.GetText()} {fieldSymbol}";
                }
                else
                {
                    goto proceed;
                }

                if (fieldSymbol != null && currentFieldSymbol.HasLength && fieldSymbol.HasLength)
                {
                    if (currentFieldSymbol.Length < fieldSymbol.Length)
                    {
                        var loc = (context.Node is FieldSyntax fieldSyntax && fieldSyntax.Type != null)
                            ? fieldSyntax.Type.GetLocation()
                            : context.Node.GetLocation();

                        context.ReportDiagnostic(Diagnostic.Create(
                            DiagnosticDescriptors.Rule0034TableRelationTooLong,
                            loc,
                            fieldSymbol.Length,
                            tableRelationsTargetFieldName,
                            currentFieldSymbol.Length,
                            currentFieldSymbol.Name));
                    }
                }

            proceed:
                tableRelationValueSyntax = tableRelationValueSyntax.ElseExpression?.ElseTableRelationCondition;
            }
        }

        private IFieldSymbol GetRelatedFieldSymbol(IdentifierNameSyntax table, IdentifierNameSyntax field, Compilation compilation)
        {
            if (table == null || compilation == null)
            {
                return null;
            }

            var tableName = table.GetIdentifierOrLiteralValue();
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            ITableTypeSymbol tableTypeSymbol = null;
            var applicationObjects = compilation.GetApplicationObjectTypeSymbolsByNameAcrossModules(SymbolKind.Table, tableName);
            if (applicationObjects != null)
            {
                int candidateCount = 0;
                ITableTypeSymbol first = null;

                foreach (var s in applicationObjects)
                {
                    if (s is ITableTypeSymbol t)
                    {
                        candidateCount++;
                        first ??= t;
                    }
                }

                if (candidateCount <= 1)
                {
                    tableTypeSymbol = first;
                }
            }


            string fieldName = null;
            if (field != null)
            {
                fieldName = field.GetIdentifierOrLiteralValue();
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                if (tableTypeSymbol != null && tableTypeSymbol.Fields != null)
                {
                    foreach (var f in tableTypeSymbol.Fields)
                    {
                        if (f != null && string.Equals(f.Name, fieldName, StringComparison.Ordinal))
                        {
                            return f;
                        }
                    }
                }
            }
            else
            {
                if (tableTypeSymbol != null &&
                    tableTypeSymbol.PrimaryKey != null &&
                    tableTypeSymbol.PrimaryKey.Fields != null)
                {
                    foreach (var f in tableTypeSymbol.PrimaryKey.Fields)
                    {
                        return f;
                    }
                }
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                var declared = compilation.GetDeclaredApplicationObjectSymbols();
                if (declared != null)
                {
                    foreach (var obj in declared)
                    {
                        if (obj == null || obj.Kind != SymbolKind.TableExtension)
                        {
                            continue;
                        }

                        if (obj is not ITableExtensionTypeSymbol tableExt)
                        {
                            continue;
                        }

                        if (tableExt.Target == null || !string.Equals(tableExt.Target.Name, tableName, StringComparison.Ordinal))
                        {
                            continue;
                        }

                        if (tableExt.AddedFields != null)
                        {
                            foreach (var added in tableExt.AddedFields)
                            {
                                if (added != null && string.Equals(added.Name, fieldName, StringComparison.Ordinal))
                                {
                                    return added;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
