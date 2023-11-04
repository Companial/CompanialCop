//using Microsoft.Dynamics.Nav.Analyzers.Common.AppSourceCopConfiguration;
//using Microsoft.Dynamics.Nav.CodeAnalysis;
//using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
//using System;
//using System.Collections.Concurrent;


//#nullable enable
//namespace CompanialCopAnalyzer.Design.Helper
//{
//    internal class UpgradeReferenceLoaderSecondLevelCache
//    {
//        private const int maxCacheSize = 30;
//        private readonly ConcurrentDictionary<AppSourceCopPreviousModuleSpecifications, ISymbolReferenceLoader> cache;

//        public static UpgradeReferenceLoaderSecondLevelCache Instance { get; } = new UpgradeReferenceLoaderSecondLevelCache();

//        private UpgradeReferenceLoaderSecondLevelCache() => this.cache = new ConcurrentDictionary<AppSourceCopPreviousModuleSpecifications, ISymbolReferenceLoader>();

//        public ISymbolReferenceLoader Get(
//          AppSourceCopPreviousModuleSpecifications specification,
//          Compilation compilation)
//        {
//            this.ScavengeIfNeeded();
//            return this.cache.GetOrAdd(specification, (Func<AppSourceCopPreviousModuleSpecifications, ISymbolReferenceLoader>)(_ => UpgradeReferenceLoaderSecondLevelCache.CreateReferenceLoader(specification, compilation)));
//        }

//        private static ISymbolReferenceLoader CreateReferenceLoader(
//          AppSourceCopPreviousModuleSpecifications specification,
//          Compilation compilation)
//        {
//            if (specification == null)
//                throw new ArgumentNullException(nameof(specification));
//            if (string.IsNullOrEmpty(specification?.PackageCachePath))
//                throw new InvalidOperationException("The specified package cache path should not be null for '" + specification?.Specification?.ToDisplayString() + "'.");
//            if (compilation.FileSystem == null)
//                throw new InvalidOperationException("The specified compilation does not have a file system.");
//            return ReferenceLoaderFactory.CreateReferenceLoader(compilation.FileSystem, specification.PackageCachePath);
//        }

//        private bool ScavengeIfNeeded()
//        {
//            if (this.cache.Count < 30)
//                return false;
//            this.cache.Clear();
//            return true;
//        }
//    }
//}
