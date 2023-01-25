using System.Collections.Immutable;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;


#nullable enable
namespace CompanialCopAnalyzer.Design.Helper
{
    internal class UpgradeBaselineInfo
    {
        public IModuleSymbol? ModuleSymbol { get; }

        public ImmutableArray<Diagnostic>? LoadingDiagnostics { get; }

        public UpgradeBaselineInfo(
          SymbolReferenceSpecification? specification,
          IModuleSymbol? module,
          ImmutableArray<Diagnostic>? loadingDiagnostics)
        {
            this.ModuleSymbol = module;
            this.LoadingDiagnostics = loadingDiagnostics;
        }
    }
}
