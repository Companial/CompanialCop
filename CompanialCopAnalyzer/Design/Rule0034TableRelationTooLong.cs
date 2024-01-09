using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Drawing;
using System.Runtime.Remoting.Contexts;
namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0034RedudantEditableProperty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0034TableRelationTooLong);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);

        public void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (context.Symbol.IsObsoleteRemoved || context.Symbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
            {
                return;
            }

            IFieldSymbol currentFieldSymbol = (IFieldSymbol)context.Symbol;
            IPropertySymbol? tableRelation = context.Symbol.GetProperty(PropertyKind.TableRelation);

            if (tableRelation == null)
            {
                return;
            }

            TableRelationPropertyValueSyntax? tableRelationValueSyntax = tableRelation.GetPropertyValueSyntax<TableRelationPropertyValueSyntax>();

            while(tableRelationValueSyntax != null)
            {
                QualifiedNameSyntax? relatedTableField = tableRelationValueSyntax.RelatedTableField as QualifiedNameSyntax;

                if (relatedTableField == null)
                {
                    goto proceed;
                }

                IFieldSymbol? fieldSymbol = GetRelatedFieldSymbol(relatedTableField.Left as IdentifierNameSyntax, relatedTableField.Right as IdentifierNameSyntax, context.Compilation);

                if (fieldSymbol == null || !fieldSymbol.HasLength || !currentFieldSymbol.HasLength)
                {
                    goto proceed;
                }

                if (currentFieldSymbol.Length < fieldSymbol.Length)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0034TableRelationTooLong, relatedTableField.GetLocation()));
                }

                proceed:
                    tableRelationValueSyntax = tableRelationValueSyntax.ElseExpression?.ElseTableRelationCondition;
            }
        }

        private IFieldSymbol? GetRelatedFieldSymbol(IdentifierNameSyntax? table, IdentifierNameSyntax? field, Compilation compilation)
        {
            if (table == null || field == null)
            {
                return null;
            }

            string applicationObjectIdentifier = table.GetIdentifierOrLiteralValue() ?? string.Empty;
            IEnumerable<ISymbol> applicationObjects = compilation.GetApplicationObjectTypeSymbolsByNameAcrossModules(SymbolKind.Table, applicationObjectIdentifier);
            ITableTypeSymbol? tableTypeSymbol = null;
            if (applicationObjects.Count() <= 1)
            {
                tableTypeSymbol = applicationObjects.FirstOrDefault() as ITableTypeSymbol;
            }

            IFieldSymbol? fieldSymbol = tableTypeSymbol?.Fields.Where(x => x.Name == field.Identifier.ValueText).FirstOrDefault();

            if(fieldSymbol != null)
            {
                return fieldSymbol;
            }

            applicationObjects = compilation.GetDeclaredApplicationObjectSymbols().Where(x => x.Kind == SymbolKind.TableExtension);

            foreach(var applicationObject in applicationObjects)
            {
                ITableExtensionTypeSymbol tableExtension = (ITableExtensionTypeSymbol)applicationObject;

                if(tableExtension.Target?.Name == applicationObjectIdentifier)
                {
                    fieldSymbol = tableExtension.AddedFields.Where(x => x.Name == field.Identifier.ValueText).FirstOrDefault();

                    if (fieldSymbol != null)
                    {
                        return fieldSymbol;
                    }
                }
            }

            return null;
        }
    }
}