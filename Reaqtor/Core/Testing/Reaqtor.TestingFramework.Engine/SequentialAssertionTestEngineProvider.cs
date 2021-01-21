// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.TestingFramework
{
    public sealed class SequentialAssertionTestEngineProvider : AssertionTestEngineProvider, IDisposable
    {
        private readonly IEnumerator<ServiceOperation> _operations;
        private readonly IEqualityComparer<ServiceOperation> _comparer;

        public SequentialAssertionTestEngineProvider(IEqualityComparer<ServiceOperation> comparer, params ServiceOperation[] operations)
        {
            _comparer = comparer;
            _operations = ((IEnumerable<ServiceOperation>)operations).GetEnumerator();
        }

        protected override void AssertCore(ServiceOperation operation)
        {
            if (!_operations.MoveNext())
            {
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Did not expect operation '{0}'. The expected operations collection has no more expected operations.", operation));
                return;
            }

            var op = _operations.Current;
            if (!_comparer.Equals(op, operation))
            {
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The service operation doesn't match the expected operation.\r\n\r\nExpected: '{0}'\r\nActual: '{1}'", op, operation));
            }
        }

        public void Dispose()
        {
            if (_operations.MoveNext())
            {
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "The expected operations collection has remaining expected operations. First remaining operation: '{0}'.", _operations.Current));
            }

            _operations.Dispose();
        }
    }
}
