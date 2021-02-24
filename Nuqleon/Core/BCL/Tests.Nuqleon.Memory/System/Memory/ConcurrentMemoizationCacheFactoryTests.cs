// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ConcurrentMemoizationCacheFactoryTests : MemoizationCacheFactoryTestsBase
    {
        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Nop_ArgumentChecking()
        {
            Nop_ArgumentChecking(ConcurrentMemoizationCacheFactory.Nop);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Nop_Simple()
        {
            Nop_Simple(ConcurrentMemoizationCacheFactory.Nop);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Unbounded_ArgumentChecking()
        {
            Unbounded_ArgumentChecking(ConcurrentMemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Unbounded_Simple()
        {
            Unbounded_Simple(ConcurrentMemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Unbounded_NoErrorCaching()
        {
            Unbounded_NoErrorCaching(ConcurrentMemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Unbounded_ErrorCaching()
        {
            Unbounded_ErrorCaching(ConcurrentMemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Unbounded_Trim1()
        {
            Unbounded_Trim1(ConcurrentMemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Unbounded_Trim2()
        {
            Unbounded_Trim2(ConcurrentMemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_ArgumentChecking()
        {
            Lru_ArgumentChecking(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_NoError_Simple()
        {
            Lru_NoError_Simple(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_NoError_Pattern1()
        {
            Lru_NoError_Pattern1(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_NoError_Pattern2()
        {
            Lru_NoError_Pattern2(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_NoError_Random()
        {
            Lru_NoError_Random(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_NoError_Error()
        {
            Lru_NoError_Error(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_CacheError_Error()
        {
            Lru_CacheError_Error(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_Trim1()
        {
            Lru_Trim1(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_Trim2()
        {
            Lru_Trim2(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_Trim3()
        {
            Lru_Trim3(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_Trim4()
        {
            Lru_Trim4(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Lru_Trim5()
        {
            Lru_Trim5(ConcurrentMemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_ArgumentChecking()
        {
            Evict_ArgumentChecking(ConcurrentMemoizationCacheFactory.CreateEvictedByHighest, ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Lowest_HitCount()
        {
            Evict_Lowest_HitCount(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Highest_HitCount()
        {
            Evict_Highest_HitCount(ConcurrentMemoizationCacheFactory.CreateEvictedByHighest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Lowest_InvokeDuration()
        {
            Evict_Lowest_InvokeDuration(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Lowest_LastAccessTime()
        {
            Evict_Lowest_LastAccessTime(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Error_NoCaching()
        {
            Evict_Error_NoCaching(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Error_Caching()
        {
            Evict_Error_Caching(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Trim1()
        {
            Evict_Trim1(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Trim2()
        {
            Evict_Trim2(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void ConcurrentMemoizationCacheFactory_Evict_Trim3()
        {
            Evict_Trim3(ConcurrentMemoizationCacheFactory.CreateEvictedByLowest);
        }
    }
}
