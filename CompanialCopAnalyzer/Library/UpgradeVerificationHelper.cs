using Microsoft.Dynamics.Nav.Analyzers.Common.AppSourceCopConfiguration;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols.BuiltIn;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;


#nullable enable
namespace CompanialCopAnalyzer.Design.Helper
{
    internal static class UpgradeVerificationHelper
    {
        private const string MissingPackageDiagnosticId = "AL1022";
        private const string MissingPackageCacheDiagnosticId = "AL1045";
        public static readonly HashSet<string> ExpectedReferenceManagerDiagnostics = new HashSet<string>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase)
    {
      "AL1045",
      "AL1022"
    };

        /*public static UpgradeBaselineInfo? LoadPreviousModule(
          Compilation compilation,
          AppSourceCopPreviousModuleSpecifications? config)
        {
            if (config == null || config.Specification == null)
                return (UpgradeBaselineInfo)null;
            ISymbolReferenceLoader referenceLoader = UpgradeVerificationHelper.GetReferenceLoader(compilation, config);
            IReferenceManager referenceManager = ReferenceManagerFactory.Create((IEnumerable<SymbolReferenceSpecification>)UpgradeVerificationHelper.GetModulesToLoad(config.Specification, referenceLoader), compilation, referenceLoader);
            ImmutableArray<IModuleSymbol> loadedModules = referenceManager.GetLoadedModules();
            ImmutableArray<Diagnostic> diagnostics = referenceManager.GetDiagnostics(CancellationToken.None);
            UpgradeVerificationHelper.ValidateReferenceManagerDiagnostics(diagnostics);
            IModuleSymbol module = loadedModules.FirstOrDefault<IModuleSymbol>((Func<IModuleSymbol, bool>)(m => m.AppId == compilation.ModuleInfo.AppId && m.Version == config.Specification.Version));
            return new UpgradeBaselineInfo(config.Specification, module, new ImmutableArray<Diagnostic>?(diagnostics));
        }

        private static ISymbolReferenceLoader GetReferenceLoader(
          Compilation compilation,
          AppSourceCopPreviousModuleSpecifications config)
        {
            if (string.IsNullOrEmpty(config.PackageCachePath))
                return ReferenceLoaderFactory.GetReferenceLoader(compilation);
            return config.DisableCrossCompilationBaselineCache ? UpgradeReferenceLoaderSimpleCache.Instance.Get(compilation, config) : UpgradeReferenceLoaderFirstLevelCache.Instance.Get(compilation, config);
        }

        private static ImmutableArray<SymbolReferenceSpecification> GetModulesToLoad(
          SymbolReferenceSpecification previousSymbolSpecification,
          ISymbolReferenceLoader referenceLoader)
        {
            IEnumerable<SymbolReferenceSpecification> dependencies = UpgradeVerificationHelper.GetDependencies(previousSymbolSpecification, referenceLoader);
            PooledList<SymbolReferenceSpecification> instance = PooledList<SymbolReferenceSpecification>.GetInstance();
            try
            {
                instance.Add(previousSymbolSpecification);
                instance.AddRange(dependencies);
                return instance.ToImmutableArray<SymbolReferenceSpecification>();
            }
            finally
            {
                instance.Free();
            }
        }

        private static IEnumerable<SymbolReferenceSpecification> GetDependencies(
          SymbolReferenceSpecification baseline,
          ISymbolReferenceLoader referenceLoader)
        {
            List<Diagnostic> diagnosticList = new List<Diagnostic>();
            IEnumerable<SymbolReferenceSpecification> dependencies = referenceLoader.GetDependencies(baseline, (IList<Diagnostic>)diagnosticList);
            if (!diagnosticList.Any<Diagnostic>((Func<Diagnostic, bool>)(x => x.Severity == DiagnosticSeverity.Error && !UpgradeVerificationHelper.IsBaselineMissingPackageDiagnostic(x, baseline))))
                return dependencies;
            throw new InvalidOperationException("Errors were reported while getting the baseline dependencies. Details: " + string.Join(Environment.NewLine, diagnosticList.Select<Diagnostic, string>((Func<Diagnostic, string>)(x => x.ToString()))));
        }

        private static void ValidateReferenceManagerDiagnostics(ImmutableArray<Diagnostic> diagnostics)
        {
            if (diagnostics.Any<Diagnostic>((Func<Diagnostic, bool>)(x => x.Severity == DiagnosticSeverity.Error && !UpgradeVerificationHelper.ExpectedReferenceManagerDiagnostics.Contains(x.Id))))
                throw new InvalidOperationException("Errors were reported while loading the reference symbols. Details: " + string.Join(Environment.NewLine, diagnostics.Select<Diagnostic, string>((Func<Diagnostic, string>)(x => x.ToString()))));
        }

        public static bool IsBaselineMissingPackageDiagnostic(
          Diagnostic diagnostic,
          SymbolReferenceSpecification reference)
        {
            if (string.Equals(diagnostic.Id, "AL1045", StringComparison.OrdinalIgnoreCase))
                return true;
            return string.Equals(diagnostic.Id, "AL1022", StringComparison.OrdinalIgnoreCase) && diagnostic.Arguments.Any<object>((Func<object, bool>)(x => string.Equals(x.ToString(), reference.Name, StringComparison.OrdinalIgnoreCase))) && diagnostic.Arguments.Any<object>((Func<object, bool>)(x => string.Equals(x.ToString(), reference.Publisher, StringComparison.OrdinalIgnoreCase)));
        }

        public static ISymbol? GetObjectSymbolFromModule(
          IModuleSymbol? moduleSymbol,
          ITypeSymbol objectSymbol,
          bool includePropagatedDependencies = true)
        {
            if (moduleSymbol == null)
                return (ISymbol)null;
            if (SemanticFacts.IsApplicationObjectWithoutId(objectSymbol.Kind))
                return !includePropagatedDependencies ? moduleSymbol.GetObjectSymbolByName(objectSymbol.Kind, objectSymbol.Name) : moduleSymbol.GetObjectSymbolByNameWithPropagatedDependencies(objectSymbol.Kind, objectSymbol.Name).SingleOrDefault<ISymbol>();
            if (!(objectSymbol is IApplicationObjectTypeSymbol objectTypeSymbol))
                return (ISymbol)null;
            return !includePropagatedDependencies ? (ISymbol)moduleSymbol.GetObjectSymbolById(objectTypeSymbol.Kind, objectTypeSymbol.Id) : (ISymbol)moduleSymbol.GetObjectSymbolByIdWithPropagatedDependencies(objectTypeSymbol.Kind, objectTypeSymbol.Id).SingleOrDefault<ISymbolWithId>();
        }

        public static ISymbol? GetObjectSymbolFromModuleOrDependencies(
          IModuleSymbol? moduleSymbol,
          ITypeSymbol objectSymbol)
        {
            if (moduleSymbol == null)
                return (ISymbol)null;
            if (SemanticFacts.IsApplicationObjectWithoutId(objectSymbol.Kind))
                return moduleSymbol.GetObjectSymbolsByNameAcrossModules(objectSymbol.Kind, objectSymbol.Name).SingleOrDefault<ISymbol>();
            return !(objectSymbol is IApplicationObjectTypeSymbol objectTypeSymbol) ? (ISymbol)null : (ISymbol)moduleSymbol.GetObjectSymbolsByIdAcrossModules(objectTypeSymbol.Kind, objectTypeSymbol.Id).SingleOrDefault<ISymbolWithId>();
        }

        public static Location GetSymbolLocation(ISymbol? symbol) => symbol == null || !symbol.IsSourceSymbol() ? Location.None : symbol.GetLocation();

        public static TableTypeKind GetTableTypeKindFromTableApplicationTypeSymbol(
          ISymbol tableApplicationObjectTypeSymbol)
        {
            if (tableApplicationObjectTypeSymbol.Kind == SymbolKind.Table)
                return ((ITableTypeSymbol)tableApplicationObjectTypeSymbol).TableType;
            if (tableApplicationObjectTypeSymbol.Kind != SymbolKind.TableExtension)
                throw new InvalidOperationException(string.Format("Cannot resolve the '{0}' of a '{1} symbol.'", (object)"TableTypeKind", (object)tableApplicationObjectTypeSymbol.Kind));
            return !(((IApplicationObjectExtensionTypeSymbol)tableApplicationObjectTypeSymbol).Target is ITableTypeSymbol target) ? TableTypeKind.Normal : target.TableType;
        }

        public static bool IsOptionEnumFieldConversion(IFieldSymbol oldSymbol, IFieldSymbol newSymbol) => UpgradeVerificationHelper.IsOptionEnumConversion(oldSymbol.GetTypeSymbol().NavTypeKind, newSymbol.GetTypeSymbol().NavTypeKind);

        public static bool IsOptionEnumConversion(NavTypeKind oldType, NavTypeKind newType) => oldType == NavTypeKind.Option && newType == NavTypeKind.Enum;

        public static bool IsIntegerEnumConversion(NavTypeKind oldType, NavTypeKind newType) => oldType == NavTypeKind.Integer && newType == NavTypeKind.Enum;*/

        public static bool IsObsoleteOrDeprecated(ISymbol symbol) => symbol.IsObsoleteRemoved || symbol.IsObsoletePending;

        //public static bool IsSameObsoleteState(ISymbol oldSymbol, ISymbol newSymbol) => oldSymbol.IsObsoleteRemoved == newSymbol.IsObsoleteRemoved && oldSymbol.IsObsoletePending == newSymbol.IsObsoletePending;

        public static bool IsSymbolExposedInTheCloud(ISymbol symbol)
        {
            switch (symbol.DeclaredAccessibility)
            {
                case Accessibility.Local:
                case Accessibility.ProtectedAndInternal:
                case Accessibility.Internal:
                case Accessibility.ProtectedOrInternal:
                    return false;
                default:
                    return symbol.CompilationScope.IsAllowedInSaaS(true);
            }
        }

        /*public static bool ShouldValidateBreakingChanges(ISymbol oldSymbol, ISymbol? newSymbol) => UpgradeVerificationHelper.IsSymbolExposedInTheCloud(oldSymbol) && (!UpgradeVerificationHelper.IsObsoleteOrDeprecated(oldSymbol) || newSymbol != null && UpgradeVerificationHelper.IsObsoleteOrDeprecated(newSymbol));

        public static bool ShouldValidateTableBreakingChanges(
          ITableTypeSymbol oldTable,
          ITableTypeSymbol newTable)
        {
            return UpgradeVerificationHelper.IsTableTypeWithSqlSchema(oldTable.TableType) || UpgradeVerificationHelper.ShouldValidateBreakingChanges((ISymbol)oldTable, (ISymbol)newTable);
        }

        public static bool ShouldValidateTableBreakingChanges(
          ITableExtensionTypeSymbol oldTableExtension,
          ITableExtensionTypeSymbol newTableExtension)
        {
            return UpgradeVerificationHelper.IsTableTypeWithSqlSchema(UpgradeVerificationHelper.GetTableTypeKindFromTableApplicationTypeSymbol((ISymbol)oldTableExtension)) || UpgradeVerificationHelper.ShouldValidateBreakingChanges((ISymbol)oldTableExtension, (ISymbol)newTableExtension);
        }

        public static bool IsFieldWithSqlSchema(TableTypeKind tableType, Microsoft.Dynamics.Nav.CodeAnalysis.FieldClassKind fieldClass)
        {
            if (!UpgradeVerificationHelper.IsTableTypeWithSqlSchema(tableType))
                return false;
            switch (fieldClass)
            {
                case Microsoft.Dynamics.Nav.CodeAnalysis.FieldClassKind.Normal:
                    return true;
                case Microsoft.Dynamics.Nav.CodeAnalysis.FieldClassKind.FlowField:
                case Microsoft.Dynamics.Nav.CodeAnalysis.FieldClassKind.FlowFilter:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsTableTypeWithSqlSchema(TableTypeKind tableType)
        {
            switch (tableType)
            {
                case TableTypeKind.Normal:
                    return true;
                case TableTypeKind.CRM:
                case TableTypeKind.ExternalSQL:
                case TableTypeKind.Exchange:
                case TableTypeKind.MicrosoftGraph:
                case TableTypeKind.CDS:
                case TableTypeKind.Temporary:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsNameChangeAllowed(ISymbol oldObject, ISymbol newObject) => !UpgradeVerificationHelper.ShouldValidateBreakingChanges(oldObject, newObject) || SemanticFacts.IsSameName(newObject.Name, oldObject.Name);

        public static bool IsExtensionTargetRemovedObjectFromDependency(
          IModuleSymbol moduleSymbol,
          ISymbol oldObject,
          ISymbol? newObject)
        {
            if (!oldObject.Kind.IsExtensionOrCustomizationObject())
                return false;
            IApplicationObjectTypeSymbol objectTypeSymbol = oldObject is IApplicationObjectExtensionTypeSymbol extensionTypeSymbol ? extensionTypeSymbol.Target : (IApplicationObjectTypeSymbol)null;
            return objectTypeSymbol != null && newObject == null && UpgradeVerificationHelper.IsRemovedApplicationObjectFromDependency(moduleSymbol, (ITypeSymbol)objectTypeSymbol);
        }

        public static bool IsRemovedApplicationObjectFromDependency(
          IModuleSymbol moduleSymbol,
          ITypeSymbol typeSymbol)
        {
            if (typeSymbol != null && typeSymbol.Kind == SymbolKind.Undefined)
                return true;
            return (typeSymbol == null || typeSymbol.Kind != SymbolKind.Table) && typeSymbol is IApplicationObjectTypeSymbol objectTypeSymbol && !UpgradeVerificationHelper.IsDeclaredInSameModule(moduleSymbol, objectTypeSymbol) && !UpgradeVerificationHelper.IsDeclaredInModuleOrDependency(moduleSymbol, objectTypeSymbol);
        }

        private static bool IsDeclaredInModuleOrDependency(
          IModuleSymbol moduleSymbol,
          IApplicationObjectTypeSymbol oldApplicationObject)
        {
            return UpgradeVerificationHelper.GetObjectSymbolFromModuleOrDependencies(moduleSymbol, (ITypeSymbol)oldApplicationObject) != null;
        }

        public static bool IsDeclaredInSameModule(
          IModuleSymbol moduleSymbol,
          IApplicationObjectTypeSymbol applicationObject)
        {
            IModuleSymbol containingSymbolOfKind = applicationObject.GetContainingSymbolOfKind<IModuleSymbol>(SymbolKind.Module);
            return containingSymbolOfKind == null || containingSymbolOfKind.AppId == moduleSymbol.AppId;
        }*/
    }
}
