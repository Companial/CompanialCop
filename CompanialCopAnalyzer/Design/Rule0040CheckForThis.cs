using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CompanialCopAnalyzer.Design;
[DiagnosticAnalyzer]
public class Rule0040CheckForThis : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0040AddThisQualificationToImproveReadability);

    public override void Initialize(AnalysisContext context) 
        => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(AnalyzeNode), SyntaxKind.IdentifierName);


    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var identifierNode = (IdentifierNameSyntax)context.Node;

        if (IsPartOfDeclaration(identifierNode))
            return;

        var symbol = context.SemanticModel.GetSymbolInfo(identifierNode).Symbol;

        if (symbol is IVariableSymbol variableSymbol)
        {
            // Check if this is a global variable (declared at codeunit level)
            if (variableSymbol.ContainingSymbol is ICodeunitTypeSymbol && !IsAccessedWithThis(identifierNode))
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0040AddThisQualificationToImproveReadability, identifierNode.GetLocation(), identifierNode.Identifier.Text));
            }
        }
    }
    private bool IsPartOfDeclaration(SyntaxNode node)
    {
        return node.Parent is VariableDeclarationSyntax;
    }

    private bool IsAccessedWithThis(SyntaxNode node)
    {
        return node.Parent is MemberAccessExpressionSyntax memberAccess &&
               memberAccess.Expression is ThisExpressionSyntax;
    }
}