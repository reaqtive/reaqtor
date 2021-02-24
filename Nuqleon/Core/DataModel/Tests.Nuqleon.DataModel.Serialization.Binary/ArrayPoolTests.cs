// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;

using Nuqleon.DataModel.Serialization.Binary;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    [TestClass]
    public class ArrayPoolTests
    {
        [TestMethod]
        public void ArrayPool_Concurrency()
        {
            var pool = new ArrayPool<int>(1);
            Parallel.ForEach(
                Enumerable.Range(0, 1000),
                _ =>
                {
                    var arr = pool.Get();
                    try
                    {
                        Assert.IsNotNull(arr);
                    }
                    finally
                    {
                        pool.Release(arr);
                    }
                }
            );
        }
    }
}
