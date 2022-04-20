using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer.Design
{

    [DiagnosticAnalyzer]
    public class Rule0001VariableNameShouldNotContainWhiteSpace : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule001VariableNameShouldNotContainWhiteSpace, DiagnosticDescriptors.Rule0002VariableShouldNotContainWildcardSymbols);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeVariableSyntax, SyntaxKind.VariableDeclaration);
        }

        public static void AnalyzeVariableSyntax(SyntaxNodeAnalysisContext context)
        {
            if (context.Node == null)
                return;

            VariableDeclarationSyntax syntax = GetVariableDeclarationSyntax(context);
            AnalyzeVariable(context, syntax);

        }
        public static VariableDeclarationSyntax GetVariableDeclarationSyntax(SyntaxNodeAnalysisContext context)
        {
            VariableDeclarationSyntax? syntax = context.Node as VariableDeclarationSyntax;
            if (syntax != null)
                return syntax;
            else
                return null;
        }

        public static void AnalyzeVariable(SyntaxNodeAnalysisContext context, VariableDeclarationSyntax syntax)
        {
            if (syntax == null)
                return;

            IdentifierNameSyntax syntaxName = syntax.Name;
            string? name = syntax.GetNameStringValue();
            if (name == null)
                return;

            if (Extensions.ContainsWhiteSpace(name))
                ReportVariableNameShouldNotContainWhiteSpace(context, syntaxName.GetLocation(), name, syntax.Kind, syntaxName);
            if (Extensions.ContainsWildCardSymbols(name))
                ReportVariableNameMayNotContainWildcardSymbols(context, syntaxName.GetLocation(), name, syntax.Kind, syntaxName);
        }

        private static void ReportVariableNameShouldNotContainWhiteSpace(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            string valueText,
            SyntaxKind syntaxKind,
            IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule001VariableNameShouldNotContainWhiteSpace, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }

        private static void ReportVariableNameMayNotContainWildcardSymbols(
            SyntaxNodeAnalysisContext syntaxNodeAnalysisContext,
            Location location,
            string valueText,
            SyntaxKind syntaxKind,
            IdentifierNameSyntax syntaxName)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0002VariableShouldNotContainWildcardSymbols, location, (object)valueText, (object)syntaxKind, (object)syntaxName));
        }
    }
}