using CompanialCopAnalyzer.Library;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CompanialCopAnalyzer.Design;

[DiagnosticAnalyzer]
public class Rule0041FindDuplicateKeys : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.Rule0041DuplicateKeyFound);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeTable), SyntaxKind.TableObject);
        context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeTableExtension), SyntaxKind.TableExtensionObject);
    }

    private void AnalyzeTable(SyntaxNodeAnalysisContext context)
    {
        var tableNode = (TableSyntax)context.Node;
        var keyDeclarations = tableNode?.Keys;
        if (keyDeclarations == null || keyDeclarations.Keys == null || keyDeclarations.Keys.Count == 0)
        {
            return;
        }

        SyntaxList<KeySyntax> keysList = keyDeclarations.Keys;
        AnalyzeKeys(context, keysList);
    }

    private void AnalyzeTableExtension(SyntaxNodeAnalysisContext context)
    {
        var tableExtensionSyntax = (TableExtensionSyntax)context.Node;

        string? baseObject = RulesTablesHelper.GetIdentifierName(tableExtensionSyntax.BaseObject.Identifier);
        if (baseObject == null)
        { 
            return; 
        }

        var baseTableSymbol = context.SemanticModel.Compilation
            .GetApplicationObjectTypeSymbolsByNameAcrossModules(SymbolKind.Table, baseObject)
            .FirstOrDefault();

        var baseTableKeys = GetBaseTableKeys(baseTableSymbol);

        var tableExtension = RulesTablesHelper
            .GetTableExtensions(context.SemanticModel.Compilation)
            .Where(x => x.Key == baseObject)
            .FirstOrDefault()
            .Value;

        if (tableExtension != null && tableExtension.Keys != null && tableExtension.Keys.Keys != null && tableExtension.Keys.Keys.Count > 0)
        {
            //compare each of extension's keys with baseTable
            AnalyzeKeys(context, tableExtension.Keys.Keys, baseTableKeys);
        }
    }

    #region HelperMethods

    private void AnalyzeKeys(
        SyntaxNodeAnalysisContext context,
        SyntaxList<KeySyntax> keysList,
        List<(string, List<string>, bool)> existingKeys = null)
    {
        existingKeys ??= [];

        foreach (var key in keysList)
        {
            var keyName = key.Name;

            // key.Fields can be null
            var fieldNames = (key.Fields != null)
                ? [.. key.Fields.Select(f => f.Identifier.ValueText.Trim('"'))]
                : new List<string>();

            var isEnabled = IsKeyEnabled(key);

            if (!isEnabled)
            {
                existingKeys.Add((keyName.ToString(), fieldNames, false));
                continue;
            }

            foreach (var (existingName, existingFields, existingEnabled) in existingKeys)
            {
                if (!existingEnabled)
                {
                    continue;
                }

                if (IsPrefixMatch(fieldNames, existingFields))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        DiagnosticDescriptors.Rule0041DuplicateKeyFound,
                        key.GetLocation(),
                        keyName,
                        existingName));
                    break;
                }
            }

            existingKeys.Add((keyName.ToString(), fieldNames, isEnabled));
        }
    }

    private bool IsPrefixMatch(List<string> candidateFields, List<string> existingFields)
    {
        if (candidateFields.Count > existingFields.Count)
        { 
            return false; 
        }

        for (int i = 0; i < candidateFields.Count; i++)
        {
            if (!string.Equals(candidateFields[i], existingFields[i], StringComparison.OrdinalIgnoreCase))
                { 
                return false;
            }
        }
        return true;
    }

    private List<(string, List<string>, bool)> GetBaseTableKeys(IApplicationObjectTypeSymbol baseTableSymbol)
    {
        var baseTableKeys = new List<(string Name, List<string> Fields, bool Enabled)>();

        if (baseTableSymbol is ITableTypeSymbol tableSymbol && tableSymbol.Keys != null)
        {
            foreach (var baseKey in tableSymbol.Keys)
            {
                var baseKeyName = baseKey.Name;

                var baseKeyFieldNames = (baseKey.Fields != null)
                    ? [.. baseKey.Fields.Select(f => f.Name)]
                    : new List<string>();

                var isEnabled = IsKeyEnabled(baseKey); // default true unless explicitly set to false
                baseTableKeys.Add((baseKeyName, baseKeyFieldNames, isEnabled));
            }
        }
        return baseTableKeys;
    }

    private bool IsKeyEnabled(KeySyntax key)
    {
        if (key?.PropertyList?.Properties == null)
        {
            return true;
        }

        foreach (var property in key.PropertyList.Properties.OfType<PropertySyntax>())
        {
            if (property.Name?.Identifier.ToString()
                  .Equals("Enabled", StringComparison.OrdinalIgnoreCase) == true
                && property.Value is BooleanPropertyValueSyntax booleanLiteral)
            {
                // "true"/"false" token text
                return !booleanLiteral.Value.Value.ToString()
                    .Equals("false", StringComparison.OrdinalIgnoreCase);
            }
        }
        return true;
    }


    private bool IsKeyEnabled(IKeySymbol key)
    {
        if (key == null || key.Properties == null)
        {
            return true;
        }

        foreach (var property in key.Properties)
        {
            if (property.Name.Equals("Enabled", StringComparison.OrdinalIgnoreCase) &&
                property.Value is bool boolValue)
            {
                return boolValue;
            }
        }
        return true; // Default to enabled if not explicitly set
    }

    #endregion
}
