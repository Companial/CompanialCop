using BCCodeCopAnalyzer;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;


namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    public class Rule0018ObjectAccessAndExtensibleProperty : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0018ObjectExtensibleProperty, DiagnosticDescriptors.Rule0018ObjectAccessProperty);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeAccessProperty), SyntaxKind.CodeunitObject, SyntaxKind.QueryObject, SyntaxKind.TableObject, SyntaxKind.Interface, SyntaxKind.EnumType, SyntaxKind.PermissionSet);
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.AnalyzeExtensibleProperty), SyntaxKind.PageObject, SyntaxKind.TableObject, SyntaxKind.ReportObject, SyntaxKind.EnumType);
        }

        private void AnalyzeAccessProperty(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            PropertySyntax tooltipProperty = ctx.Node.GetProperty("Access");
            dynamic currObject = ctx.Node;

            if (tooltipProperty == null)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0018ObjectAccessProperty, currObject.Name.GetLocation()));
            }

        }

        private void AnalyzeExtensibleProperty(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            dynamic currObject = ctx.Node;
            PropertySyntax tooltipProperty = ctx.Node.GetProperty("Extensible");

            if (tooltipProperty == null)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0018ObjectExtensibleProperty, currObject.Name.GetLocation()));
            }

        }
    }
}