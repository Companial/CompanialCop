using CompanialCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Runtime.Remoting.Contexts;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0038MissingParenthesis : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0038MissingParenthesis);

        public override void Initialize(AnalysisContext context) => context.RegisterOperationAction(new Action<OperationAnalysisContext>(this.AnalyzeParenthesis), OperationKind.InvocationExpression);

        private void AnalyzeParenthesis(OperationAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoleteRemoved || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) { return; }

            IInvocationExpression operation = (IInvocationExpression)ctx.Operation;
            if (operation.Arguments.Length != 0)
                return;

            IMethodSymbol method = operation.TargetMethod;
            if ((method != null ? (method.MethodKind == MethodKind.Property ? 1 : 0) : 0) != 0)
                return;

            if ((method != null ? (method.MethodKind == MethodKind.BuiltInMethod ? 1 : 0) : 0) != 0 && !(operation.Syntax.GetLastToken().IsKind(SyntaxKind.CloseParenToken)))
            {
                if ((method.Name == "Count") || (method.Name == "IsEmpty"))
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0038MissingParenthesis, ctx.Operation.Syntax.GetLocation(), (object)method.Name));
                }
            }
        }
    }
}
