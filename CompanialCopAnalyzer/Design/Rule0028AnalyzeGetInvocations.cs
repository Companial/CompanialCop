using CompanialCopAnalyzer;
using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Build.Framework.XamlTypes;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Contexts;
using System.Xml;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule28AnalyzeGetInvocations : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0028_1IncorrectArgumentCount, DiagnosticDescriptors.Rule0028_2InvalidArgumentTypeInGetCall,DiagnosticDescriptors.Rule0028_3ArgumentLengthExceedsMaxLength);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeGetCall, SyntaxKind.InvocationExpression);
        }

        public void AnalyzeGetCall(SyntaxNodeAnalysisContext context)
        {
            var objectTypeSymbol = context.ContainingSymbol.GetContainingObjectTypeSymbol();
            if(context.ContainingSymbol.IsObsoleteRemoved || objectTypeSymbol.IsObsoleteRemoved)
            {
                return;
            }

            var invocationExpression = (InvocationExpressionSyntax) context.Node;

            if(invocationExpression.ArgumentList.Arguments.Count == 0)
            {
                return;
            }

            var baseTable = GetBaseTable(context);
            if (baseTable == null)
            {
                return;
            }

            if (invocationExpression.ArgumentList.Arguments.Count == 1
                && context.SemanticModel.GetSymbolInfo(invocationExpression.ArgumentList.Arguments[0]).Symbol?.GetTypeSymbol().NavTypeKind == NavTypeKind.RecordId)
            {
                return;
            }

            var primaryKeyFields = baseTable.PrimaryKey.Fields;
            var arguments = invocationExpression.ArgumentList.Arguments;

            if (arguments.Count != primaryKeyFields.Length)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0028_1IncorrectArgumentCount, invocationExpression.GetLocation(), baseTable.Name, primaryKeyFields.Length));
                return;
            }

            CompareFieldsToArguments(context, primaryKeyFields, arguments, baseTable.Name);
        }
        
        private ITableTypeSymbol? GetBaseTable(SyntaxNodeAnalysisContext context)
        {
            if(context.Node is not InvocationExpressionSyntax invocationExpression)
            {
                return null;
            }

            if (invocationExpression.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                if (memberAccess.Name.Identifier.ValueText != "Get")
                {
                    return null;
                }


                var recordSymbol = context.SemanticModel.GetSymbolInfo(memberAccess.Expression).Symbol.GetTypeSymbol();

                if (recordSymbol == null || recordSymbol is not IRecordTypeSymbol record)
                {
                    return null;
                }

                return record.BaseTable;
            }
            else
            {
                if (invocationExpression.Expression is IdentifierNameSyntax identifierName && identifierName.Identifier.ValueText != "Get")
                {
                    return null;
                }

                if (context.ContainingSymbol.GetContainingObjectTypeSymbol() is not ITableTypeSymbol sourceTable)
                {
                    return null;
                }

                return sourceTable;
            }
        }

        private void CompareFieldsToArguments(SyntaxNodeAnalysisContext context, ImmutableArray<IFieldSymbol> fields, SeparatedSyntaxList<CodeExpressionSyntax> arguments, string tableName)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                var argumentType = context.SemanticModel.GetSymbolInfo(arguments[i]).Symbol.GetTypeSymbol();
                var fieldType = fields[i].GetTypeSymbol();

                if (fieldType.IsTextType() && argumentType.IsTextType())
                {
                    int argumentLength = argumentType.Length;

                    if (arguments[i] is LiteralExpressionSyntax literalExpression && literalExpression.Literal is StringLiteralValueSyntax stringLiteral)
                    {
                        argumentLength = stringLiteral.Value.ValueText.Length;
                    }

                    if (argumentLength > fieldType.Length)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0028_3ArgumentLengthExceedsMaxLength, arguments[i].GetLocation(), tableName, fieldType.Length));
                    }
                }
                else if (fieldType != argumentType)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0028_2InvalidArgumentTypeInGetCall, arguments[i].GetLocation(), tableName));
                }
            }
        }
    }
}