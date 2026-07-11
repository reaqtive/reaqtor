// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices;

[TestClass]
public class UnboundParameterExceptionTests
{
    [TestMethod]
    public void UnboundParameterException_ArgumentChecking()
    {
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => new UnboundParameterException("", expression: null, []));
        Assert.AreEqual("expression", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => new UnboundParameterException("", Expression.Constant(42), parameters: null));
        Assert.AreEqual("parameters", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => UnboundParameterException.ThrowIfOpen(expression: null, ""));
        Assert.AreEqual("expression", ex3.ParamName);
    }

    [TestMethod]
    public void UnboundParameterException_Simple()
    {
        var e = Expression.Constant(42);
        var p = new[] { Expression.Parameter(typeof(int)) };
        var ex = new UnboundParameterException("Oops", e, p);
        Assert.StartsWith("Oops", ex.Message);
        Assert.AreSame(e, ex.Expression);
        Assert.IsTrue(p.SequenceEqual(ex.Parameters));
    }


    [TestMethod]
    public void UnboundParameterException_ThrowIfOpen_Positive()
    {
        var f = (Expression<Func<int, int>>)(x => x + 1);

        var ex = Assert.ThrowsExactly<UnboundParameterException>(() => UnboundParameterException.ThrowIfOpen(f.Body, "Oops"));
        Assert.AreSame(f.Body, ex.Expression);

        Assert.IsTrue(f.Parameters.SequenceEqual(ex.Parameters));
    }

    [TestMethod]
    public void UnboundParameterException_ThrowIfOpen_Negative()
    {
        var f = (Expression<Func<int, int>>)(x => x + 1);

        UnboundParameterException.ThrowIfOpen(f, "Oops"); // does not throw
    }
}
