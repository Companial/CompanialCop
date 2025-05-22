using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;
using System.Collections;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0035UnusedGlobalProcedures : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(DiagnosticDescriptors.Rule0035UnusedGlobalProcedure);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(delegate (CompilationStartAnalysisContext compilationStartAnalysisContext)
            {
                ImmutableArray<IApplicationObjectTypeSymbol> applicationObjects = compilationStartAnalysisContext.Compilation.GetDeclaredApplicationObjectSymbols();
                UnusedMethodAnalyzer unusedObjectAnalyzer = new UnusedMethodAnalyzer(applicationObjects, compilationStartAnalysisContext.CancellationToken);

                compilationStartAnalysisContext.RegisterSyntaxNodeAction(unusedObjectAnalyzer.AnalyzeInvocationExpression, SyntaxKind.InvocationExpression);
                compilationStartAnalysisContext.RegisterCompilationEndAction(unusedObjectAnalyzer.CompilationEndAction);
            });
        }

        private static void ReportUnusedObjects(Action<Diagnostic> action, IEnumerable unusedMethods)
        {
            foreach (IMethodSymbol method in unusedMethods)
            {
                Diagnostic diagnostic = Diagnostic.Create(DiagnosticDescriptors.Rule0035UnusedGlobalProcedure, method.GetLocation(), method.Name);
                action(diagnostic);
            }
        }

        private class UnusedMethodAnalyzer
        {
            private readonly HashSet<ISymbol> unusedMethods;

            public UnusedMethodAnalyzer(ImmutableArray<IApplicationObjectTypeSymbol> applicationObjects, CancellationToken cancellationToken)
            {
                unusedMethods = new HashSet<ISymbol>();

                ImmutableArray<IApplicationObjectTypeSymbol>.Enumerator enumerator = applicationObjects.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IApplicationObjectTypeSymbol applicationObject = enumerator.Current;
                    var globalMethods = applicationObject.GetMembers().Where(x => x is IMethodSymbol methodSymbol 
                                                                                        && !methodSymbol.IsLocal 
                                                                                        && methodSymbol.MethodKind != MethodKind.BuiltInMethod);

                    foreach(var method in globalMethods)
                    {
                        if (IsUnused(applicationObject, (IMethodSymbol) method))
                        {
                            unusedMethods.Add(method);
                        }
                    }
                }
            }

            public void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
            {
                if (unusedMethods.Count == 0)
                {
                    return;
                }

                if (context.ContainingSymbol.IsObsoleteRemoved || context.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved)
                {
                    return;
                }

                IMethodSymbol? method = context.SemanticModel.GetSymbolInfo(context.Node).Symbol as IMethodSymbol;

                if (method == null)
                {
                    return;
                }

                if (unusedMethods.Contains(method))
                {
                    unusedMethods.Remove(method);
                    return;
                }
            }

            private bool IsUnused(IApplicationObjectTypeSymbol applicationObject, IMethodSymbol methodSymbol)
            {
                string[] attributes =
                {
                    "Test",
                    "PageHandler",
                    "ReportHandler",
                    "ConfirmHandler",
                    "MessageHandler",
                    "StrMenuHandler",
                    "HyperlinkHandler",
                    "ModalPageHandler",
                    "FilterPageHandler",
                    "RequestPageHandler",
                    "SessionSettingsHandler",
                    "SendNotificationHandler",
                    "RecallNotificationHandler",
                    "ServiceEnabled"
                };

                MethodDeclarationSyntax? methodDeclarationSyntax = methodSymbol.DeclaringSyntaxReference?.GetSyntax() as MethodDeclarationSyntax;

                if (methodDeclarationSyntax == null)
                {
                    return false;
                }

                List<SyntaxTrivia> syntaxTrivias = new List<SyntaxTrivia>();
                syntaxTrivias.AddRange(methodDeclarationSyntax.GetLeadingTrivia());
                syntaxTrivias.AddRange(methodDeclarationSyntax.GetTrailingTrivia());

                if (syntaxTrivias.Any(x => x.IsKind(SyntaxKind.LineCommentTrivia))) return false;

                if (methodSymbol.Attributes.Any(x => attributes.Contains(x.Name)))
                {
                    return false;
                }

                return IsUsedByInterface((ICodeunitTypeSymbol)applicationObject, methodSymbol);
            }
            private bool IsUsedByInterface(ICodeunitTypeSymbol applicationObject, IMethodSymbol methodSymbol)
            {
                IEnumerable<ImmutableArray<ISymbol>> members = applicationObject.ImplementedInterfaces.Select(x => x.GetMembers()).ToList();
                return !members.Any(x => x.Any(z => ((IMethodSymbol)z).Id == methodSymbol.Id));
            }

            public void CompilationEndAction(CompilationAnalysisContext context)
            {
                ReportUnusedObjects(new Action<Diagnostic>(context.ReportDiagnostic), unusedMethods);
            }
        }
    }
}
