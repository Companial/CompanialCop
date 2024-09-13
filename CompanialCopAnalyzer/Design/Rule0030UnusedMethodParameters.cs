using CompanialCopAnalyzer.Design.Helper;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0030UnusedMethodParameters : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0030UnusedMethodParameters);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCodeBlockStartAction(delegate (CodeBlockStartAnalysisContext startCodeBlockContext)
            {
                if (startCodeBlockContext.OwningSymbol.Kind == SymbolKind.Method)
                {
                    IMethodSymbol methodSymbol = (IMethodSymbol)startCodeBlockContext.OwningSymbol;
                    if (methodSymbol.GetContainingObjectTypeSymbol() is ICodeunitTypeSymbol codeunitTypeSymbol)
                    {
                        IEnumerable<ImmutableArray<ISymbol>> members = codeunitTypeSymbol.ImplementedInterfaces.Select(x => x.GetMembers());

                        if (members.Any(x => x.Any(z => ((IMethodSymbol)z).Id == methodSymbol.Id)))
                        {
                            return;
                        }
                    }

                    if (!methodSymbol.IsEvent && 
                        !methodSymbol.IsObsoleteRemoved && 
                        !methodSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved && 
                        !methodSymbol.Parameters.IsEmpty)
                    {
                        UnusedVariableAnalyzer @object = new UnusedVariableAnalyzer(methodSymbol, startCodeBlockContext.CancellationToken);
                        startCodeBlockContext.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(@object.AnalyzeSyntaxNode), SyntaxKind.IdentifierName, SyntaxKind.IdentifierNameOrEmpty);
                        startCodeBlockContext.RegisterCodeBlockEndAction(new Action<CodeBlockAnalysisContext>(@object.CodeBlockEndAction));
                    }
                }
            });
        }

        private static void ReportUnusedLocalVariables(Action<Diagnostic> action, IEnumerable unusedVariables)
        {
            foreach (ISymbol variable in unusedVariables)
            {
                Diagnostic diagnostic = Diagnostic.Create(DiagnosticDescriptors.Rule0030UnusedMethodParameters, variable.GetLocation(), variable.Name, variable.ContainingSymbol!.Name);
                action(diagnostic);
            }
        }

        private class UnusedVariableAnalyzer
        {
            private readonly HashSet<ISymbol> unusedVariables;

            private readonly HashSet<string> unusedVariableNames;

            public UnusedVariableAnalyzer(IMethodSymbol method, CancellationToken cancellationToken)
            {
                unusedVariables = new HashSet<ISymbol>();
                unusedVariableNames = new HashSet<string>(SemanticFacts.NameEqualityComparer);
                cancellationToken.ThrowIfCancellationRequested();
                if (method.IsLocal)
                {
                    return;
                }
                if (method.MethodKind != MethodKind.Trigger && !method.IsEventSubscriber())
                {
                    ImmutableArray<IParameterSymbol>.Enumerator enumerator2 = method.Parameters.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        IParameterSymbol parameter = enumerator2.Current;
                        cancellationToken.ThrowIfCancellationRequested();
                        if (!parameter.IsSynthesized)
                        {
                            unusedVariables.Add(parameter);
                            unusedVariableNames.Add(parameter.Name);
                        }
                    }
                }
            }

            public void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
            {
                if (unusedVariables.Count == 0 || context.Node.Parent.IsKind(SyntaxKind.Parameter, SyntaxKind.ReturnValue))
                {
                    return;
                }
                var identifier = context.Node as IdentifierNameSyntax;
                if (identifier != null && unusedVariableNames.Contains(identifier.Unquoted()))
                {
                    ISymbol? variable = context.SemanticModel.GetSymbolInfo(identifier, context.CancellationToken).Symbol;
                    if (variable != null && unusedVariables.Contains(variable))
                    {
                        unusedVariables.Remove(variable);
                        unusedVariableNames.Remove(variable.Name);
                    }
                }
            }

            public void CodeBlockEndAction(CodeBlockAnalysisContext context)
            {
                ReportUnusedLocalVariables(new Action<Diagnostic>(context.ReportDiagnostic), unusedVariables);
            }
        }
    }
}
