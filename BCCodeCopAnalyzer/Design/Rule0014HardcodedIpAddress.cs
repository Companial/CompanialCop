using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0014HardcodedIpAddress : DiagnosticAnalyzer
    {
        private readonly string[] ignoredVariableNames =
            {
                "VERSION",
                "ASSEMBLY",
            };
        private const string IPv4Broadcast = "255.255.255.255";
        private const int IPv4AddressParts = 4;

        protected string GetValueText(LiteralExpressionSyntax literalExpression) => literalExpression.Literal.GetLiteralValue().ToString();
        protected string GetValueText(StringLiteralValueSyntax literalExpression) => literalExpression.Value.Value.ToString();
        protected string? GetAssignedVariableName(LiteralExpressionSyntax stringLiteral) => stringLiteral.FirstAncestorOrSelf<SyntaxNode>(IsVariableIdentifier)?.ToString();
        protected string? GetAssignedVariableName(StringLiteralValueSyntax stringLiteral) => stringLiteral.FirstAncestorOrSelf<SyntaxNode>(IsVariableIdentifier)?.ToString();
        private static bool IsVariableIdentifier(SyntaxNode syntaxNode) =>
            syntaxNode is StatementSyntax
            || syntaxNode is VariableDeclarationSyntax
            || syntaxNode is LabelSyntax
            || syntaxNode is ParameterSyntax;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0014HardcodedIpAddress);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.CheckForHardcodedIpAddresses), SyntaxKind.StringLiteralValue);
        }


        private void CheckForHardcodedIpAddresses(SyntaxNodeAnalysisContext context)
        {
            StringLiteralValueSyntax? stringLiteralValue = context.Node as StringLiteralValueSyntax;

            string literalValue = null;
            if (stringLiteralValue != null)
            {
                var variableName = GetAssignedVariableName(stringLiteralValue);
                if (variableName != null && ignoredVariableNames.Any(x => variableName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    return;
                }
                literalValue = GetValueText(stringLiteralValue);
            }

            if (literalValue != null)
                if (!IPAddress.TryParse(literalValue, out var address)
                    || IPAddress.IsLoopback(address)
                    || address.GetAddressBytes().All(x => x == 0)                       // Nonroutable 0.0.0.0 or 0::0
                    || literalValue == IPv4Broadcast
                    || literalValue.StartsWith("2.5.")                                  // Looks like OID
                    || (address.AddressFamily == AddressFamily.InterNetwork
                        && literalValue.Count(x => x == '.') != IPv4AddressParts - 1))
                {
                    return;
                }
            if (stringLiteralValue != null)
                ReportHardcodedIpAddress(context, stringLiteralValue.GetLocation(), literalValue);
        }

        private static void ReportHardcodedIpAddress(SyntaxNodeAnalysisContext syntaxNodeAnalysisContext, Location location, string valueText)
        {
            syntaxNodeAnalysisContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0014HardcodedIpAddress, location, (object)valueText));
        }
    }
}
