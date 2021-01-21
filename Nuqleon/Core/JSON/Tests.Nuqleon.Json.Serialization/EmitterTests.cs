// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nuqleon.Json.Serialization;
using System;
using System.Text;

namespace Tests
{
    [TestClass]
    public partial class EmitterTests
    {
        private static void AssertEmit<T>(EmitStringAction<T> action, T value, string expected)
        {
            var sb = new StringBuilder();
            action(sb, value, GetEmitterContext());
            Assert.AreEqual(expected, sb.ToString());

#if !NO_IO
            var sw = new System.IO.StringWriter();

            var emitWriterMtd = action.Method.DeclaringType.GetMethod(action.Method.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, binder: null, new[] { typeof(System.IO.TextWriter), typeof(T), typeof(EmitterContext) }, modifiers: null);
            var emitWriterFnc = (EmitWriterAction<T>)Delegate.CreateDelegate(typeof(EmitWriterAction<T>), emitWriterMtd);
            emitWriterFnc(sw, value, GetEmitterContext());

            Assert.AreEqual(expected, sw.ToString());
#endif
        }

        private static EmitterContext GetEmitterContext()
        {
#if !NO_IO
            return new EmitterContext(builderString: null, builderWriter: null);
#else
            return new EmitterContext(builderString: null);
#endif
        }
    }
}
