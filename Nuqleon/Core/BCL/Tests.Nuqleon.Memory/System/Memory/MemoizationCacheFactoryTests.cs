// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Memory;

namespace Tests
{
    [TestClass]
    public class MemoizationCacheFactoryTests : MemoizationCacheFactoryTestsBase
    {
        [TestMethod]
        public void MemoizationCacheFactory_Nop_ArgumentChecking()
        {
            Nop_ArgumentChecking(MemoizationCacheFactory.Nop);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Nop_Simple()
        {
            Nop_Simple(MemoizationCacheFactory.Nop);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Unbounded_ArgumentChecking()
        {
            Unbounded_ArgumentChecking(MemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Unbounded_Simple()
        {
            Unbounded_Simple(MemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Unbounded_NoErrorCaching()
        {
            Unbounded_NoErrorCaching(MemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Unbounded_ErrorCaching()
        {
            Unbounded_ErrorCaching(MemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Unbounded_Trim1()
        {
            Unbounded_Trim1(MemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Unbounded_Trim2()
        {
            Unbounded_Trim2(MemoizationCacheFactory.Unbounded);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_ArgumentChecking()
        {
            Lru_ArgumentChecking(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_NoError_Simple()
        {
            Lru_NoError_Simple(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_NoError_Pattern1()
        {
            Lru_NoError_Pattern1(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_NoError_Pattern2()
        {
            Lru_NoError_Pattern2(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_NoError_Random()
        {
            Lru_NoError_Random(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_NoError_Error()
        {
            Lru_NoError_Error(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_CacheError_Error()
        {
            Lru_CacheError_Error(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_Trim1()
        {
            Lru_Trim1(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_Trim2()
        {
            Lru_Trim2(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_Trim3()
        {
            Lru_Trim3(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_Trim4()
        {
            Lru_Trim4(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Lru_Trim5()
        {
            Lru_Trim5(MemoizationCacheFactory.CreateLru);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_ArgumentChecking()
        {
            Evict_ArgumentChecking(MemoizationCacheFactory.CreateEvictedByHighest, MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Lowest_HitCount()
        {
            Evict_Lowest_HitCount(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Highest_HitCount()
        {
            Evict_Highest_HitCount(MemoizationCacheFactory.CreateEvictedByHighest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Lowest_InvokeDuration()
        {
            Evict_Lowest_InvokeDuration(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Lowest_LastAccessTime()
        {
            Evict_Lowest_LastAccessTime(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Error_NoCaching()
        {
            Evict_Error_NoCaching(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Error_Caching()
        {
            Evict_Error_Caching(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Trim1()
        {
            Evict_Trim1(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Trim2()
        {
            Evict_Trim2(MemoizationCacheFactory.CreateEvictedByLowest);
        }

        [TestMethod]
        public void MemoizationCacheFactory_Evict_Trim3()
        {
            Evict_Trim3(MemoizationCacheFactory.CreateEvictedByLowest);
        }
    }
}
