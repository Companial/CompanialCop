﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0016InternalMethodsMustHaveExplicitParameters : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0016InternalMethodsMustHaveExplicitParameters);

        public override void Initialize(AnalysisContext context) => context.RegisterOperationAction(new Action<OperationAnalysisContext>(this.CheckCommitForExplainingComment), OperationKind.InvocationExpression);

        private void CheckCommitForExplainingComment(OperationAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;

            List<string> methodsToCheck = new List<string> { "INSERT", "MODIFY", "DELETE", "UPDATE", "DELETEALL" };

            IInvocationExpression operation = (IInvocationExpression)ctx.Operation;
            IMethodSymbol targetMethod = operation.TargetMethod;

            if (targetMethod.MethodKind != MethodKind.BuiltInMethod) return;

            if (methodsToCheck.Contains(targetMethod.Name.ToUpper()))
                if (operation.Arguments.Length == 0)
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0016InternalMethodsMustHaveExplicitParameters, ctx.Operation.Syntax.GetLocation()));
        }
    }
}