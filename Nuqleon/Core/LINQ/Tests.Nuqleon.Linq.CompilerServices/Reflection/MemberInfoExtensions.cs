// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class MemberInfoExtensions
    {
        [TestMethod]
        public void MemberInfoExtensions_NonSupported()
        {
            Assert.ThrowsException<NotSupportedException>(() => _ = new MyMemberInfo().ToCSharpString());
        }

        private sealed class MyMemberInfo : MemberInfo
        {
            public override Type DeclaringType => throw new NotImplementedException();

            public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotImplementedException();

            public override object[] GetCustomAttributes(bool inherit) => throw new NotImplementedException();

            public override bool IsDefined(Type attributeType, bool inherit) => throw new NotImplementedException();

            public override MemberTypes MemberType => MemberTypes.Custom;

            public override string Name => throw new NotImplementedException();

            public override Type ReflectedType => throw new NotImplementedException();
        }
    }
}
