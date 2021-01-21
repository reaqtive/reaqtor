// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ConfigurationOptionsTests
    {
        [TestMethod]
        public void ConfigurationOptions_Defaults()
        {
            var options = new ConfigurationOptions();

            Assert.AreEqual(1, options.CheckpointDegreeOfParallelism);
            Assert.IsFalse(options.DumpRecoveryStateBlobs);
            Assert.IsNull(options.DumpRecoveryStatePath);
            Assert.AreEqual(Environment.ProcessorCount / 2, options.RecoveryDegreeOfParallelism);
            //Assert.IsNull(options.SerializationPolicyVersion);
            Assert.IsFalse(options.TemplatizeExpressions);
        }

        [TestMethod]
        public void ConfigurationOptions_FromUserSettings()
        {
            var options = new ConfigurationOptions
            {
                CheckpointDegreeOfParallelism = 2,
                DumpRecoveryStateBlobs = true,
                DumpRecoveryStatePath = "foo",
                RecoveryDegreeOfParallelism = 1,
                //SerializationPolicyVersion = new Version(3, 3, 3, 0),
                TemplatizeExpressions = true
            };

            Assert.AreEqual(2, options.CheckpointDegreeOfParallelism);
            Assert.IsTrue(options.DumpRecoveryStateBlobs);
            Assert.AreEqual("foo", options.DumpRecoveryStatePath);
            Assert.AreEqual(1, options.RecoveryDegreeOfParallelism);
            //Assert.AreEqual(new Version(3, 3, 3, 0), options.SerializationPolicy.DefaultVersion);
            Assert.IsTrue(options.TemplatizeExpressions);

            options.Clear();
        }
    }
}
