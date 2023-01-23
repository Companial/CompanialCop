using Microsoft.Dynamics.Nav.Analyzers.Common.AppSourceCopConfiguration;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using System;
using System.Collections.Concurrent;


#nullable enable
namespace CompanialCopAnalyzer.Design.Helper
{
    internal class UpgradeReferenceLoaderSimpleCache
    {
        private const int maxCacheSize = 30;
        private readonly ConcurrentDictionary<Compilation, ISymbolReferenceLoader> cache;

        public static UpgradeReferenceLoaderSimpleCache Instance { get; } = new UpgradeReferenceLoaderSimpleCache();

        private UpgradeReferenceLoaderSimpleCache() => this.cache = new ConcurrentDictionary<Compilation, ISymbolReferenceLoader>();

        public ISymbolReferenceLoader Get(
          Compilation compilation,
          AppSourceCopPreviousModuleSpecifications specification)
        {
            this.ScavengeIfNeeded();
            return this.cache.GetOrAdd(compilation, (Func<Compilation, ISymbolReferenceLoader>)(_ => UpgradeReferenceLoaderSimpleCache.CreateReferenceLoader(specification, compilation)));
        }

        public void Remove(Compilation compilation)
        {
            if (this.ScavengeIfNeeded())
                return;
            this.cache.TryRemove(compilation, out ISymbolReferenceLoader _);
        }

        private static ISymbolReferenceLoader CreateReferenceLoader(
          AppSourceCopPreviousModuleSpecifications specification,
          Compilation compilation)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            if (compilation.FileSystem == null)
                throw new InvalidOperationException("The specified compilation does not have a file system.");
            return ReferenceLoaderFactory.CreateReferenceLoader(compilation.FileSystem, specification.PackageCachePath);
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
