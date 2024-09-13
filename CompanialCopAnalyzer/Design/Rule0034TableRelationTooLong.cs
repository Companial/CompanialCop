using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;
namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0034RedudantEditableProperty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0034TableRelationTooLong);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.Field);

        public void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            IFieldSymbol? currentFieldSymbol = context.SemanticModel.GetDeclaredSymbol(context.Node) as IFieldSymbol;
            
            if (currentFieldSymbol == null || currentFieldSymbol.IsObsoleteRemoved || currentFieldSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
            {
                return;
            }

            IPropertySymbol? tableRelation = currentFieldSymbol.GetProperty(PropertyKind.TableRelation);

            if (tableRelation == null)
            {
                return;
            }

            TableRelationPropertyValueSyntax? tableRelationValueSyntax = tableRelation.GetPropertyValueSyntax<TableRelationPropertyValueSyntax>();

            while (tableRelationValueSyntax != null)
            {
                QualifiedNameSyntax? relatedTableField = tableRelationValueSyntax.RelatedTableField as QualifiedNameSyntax;

                if (relatedTableField == null)
                {
                    goto proceed;
                }

                IFieldSymbol? fieldSymbol = GetRelatedFieldSymbol(relatedTableField.Left as IdentifierNameSyntax, relatedTableField.Right as IdentifierNameSyntax, context.SemanticModel.Compilation);

                if (fieldSymbol == null || !fieldSymbol.HasLength || !currentFieldSymbol.HasLength)
                {
                    goto proceed;
                }

                if (currentFieldSymbol.Length < fieldSymbol.Length)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0034TableRelationTooLong, ((FieldSyntax)context.Node).Type.GetLocation(), fieldSymbol.Length, relatedTableField.ToString(), currentFieldSymbol.Length, currentFieldSymbol.Name));
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
            string fieldName = field.GetIdentifierOrLiteralValue() ?? string.Empty;

            IEnumerable<ISymbol> applicationObjects = compilation.GetApplicationObjectTypeSymbolsByNameAcrossModules(SymbolKind.Table, applicationObjectIdentifier);
            ITableTypeSymbol? tableTypeSymbol = null;
            if (applicationObjects.Count() <= 1)
            {
                tableTypeSymbol = applicationObjects.FirstOrDefault() as ITableTypeSymbol;
            }

            IFieldSymbol? fieldSymbol = tableTypeSymbol?.Fields.Where(x => x.Name == fieldName).FirstOrDefault();

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