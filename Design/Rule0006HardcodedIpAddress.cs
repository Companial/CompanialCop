using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace CompanialCopAnalyzer.Design
{
    [DiagnosticAnalyzer]
    internal class Rule0006HardcodedIpAddress : DiagnosticAnalyzer
    {
        private const string IPv4Broadcast = "255.255.255.255";
        private const int IPv4AddressParts = 4;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0006HardcodedIpAddress);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(new Action<SyntaxNodeAnalysisContext>(this.CheckForHardcodedIpAddresses), SyntaxKind.VariableDeclaration);

        private void CheckForHardcodedIpAddresses(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.ContainingSymbol.IsObsoletePending || ctx.ContainingSymbol.IsObsoleteRemoved) return;
            if (ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoletePending || ctx.ContainingSymbol.GetContainingObjectTypeSymbol().IsObsoleteRemoved) return;

            VariableDeclarationSyntax syntax = ctx.Node as VariableDeclarationSyntax;
            if (syntax != null)
            {
                if (syntax.Type.DataType.Kind != SyntaxKind.LabelDataType) return;

                dynamic labelDataType = syntax.Type.DataType;
                string labelText = labelDataType.Label.LabelText.Value.Value;
                StringLiteralValueSyntax xlabelText = labelDataType.Label.LabelText;

                if (labelText != null)
                    if (!IPAddress.TryParse(labelText, out var address)
                        || IPAddress.IsLoopback(address)
                        || address.GetAddressBytes().All(x => x == 0)                       // Nonroutable 0.0.0.0 or 0::0
                        || labelText == IPv4Broadcast
                        || labelText.StartsWith("2.5.")                                  // Looks like OID
                        || (address.AddressFamily == AddressFamily.InterNetwork
                            && labelText.Count(x => x == '.') != IPv4AddressParts - 1))
                    {
                        return;
                    }

                if (labelText != null)
                    ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0006HardcodedIpAddress, xlabelText.GetLocation(), syntax.Name));
            }
        }
    }
}
