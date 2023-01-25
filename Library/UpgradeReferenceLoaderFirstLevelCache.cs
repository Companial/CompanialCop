using System;
using System.Collections.Concurrent;
using Microsoft.Dynamics.Nav.Analyzers.Common.AppSourceCopConfiguration;
using Microsoft.Dynamics.Nav.CodeAnalysis;


#nullable enable
namespace CompanialCopAnalyzer.Design.Helper
{
    internal class UpgradeReferenceLoaderFirstLevelCache
    {
        private const int maxCacheSize = 30;
        private readonly ConcurrentDictionary<Compilation, ISymbolReferenceLoader> cache;

        public static UpgradeReferenceLoaderFirstLevelCache Instance { get; } = new UpgradeReferenceLoaderFirstLevelCache();

        private UpgradeReferenceLoaderFirstLevelCache() => this.cache = new ConcurrentDictionary<Compilation, ISymbolReferenceLoader>();

        public ISymbolReferenceLoader Get(
          Compilation compilation,
          AppSourceCopPreviousModuleSpecifications specification)
        {
            this.ScavengeIfNeeded();
            return this.cache.GetOrAdd(compilation, (Func<Compilation, ISymbolReferenceLoader>)(_ => UpgradeReferenceLoaderFirstLevelCache.CreateReferenceLoader(specification, compilation)));
        }

        private static ISymbolReferenceLoader CreateReferenceLoader(
          AppSourceCopPreviousModuleSpecifications specification,
          Compilation compilation)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            if (string.IsNullOrEmpty(specification.PackageCachePath))
                throw new InvalidOperationException("The specified package cache path should not be null for '" + specification.Specification?.ToDisplayString() + "'.");
            return UpgradeReferenceLoaderSecondLevelCache.Instance.Get(specification, compilation);
        }

        private bool ScavengeIfNeeded()
        {
            if (this.cache.Count < 30)
                return false;
            this.cache.Clear();
            return true;
        }
    }
}
