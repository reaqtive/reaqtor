// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2013
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

using Nuqleon.Linq.Expressions.Serialization;

namespace Tests
{
    [TestClass]
    public class ExpressionJsonSerializerTests : TestBase
    {
        [TestMethod]
        public void ExpressionJsonSerializer_Serialize_WithoutMatchingRule()
        {
            var ser = new ExpressionJsonSerializer
            {
                Rules =
                {
                    (Person p) => new Person { Name = p.Name, Age = p.Age }
                }
            };

            Assert.ThrowsException<InvalidOperationException>(() => ser.Serialize(new Book { Title = "War and Peace", YearPublished = 1869 }));
        }

        [TestMethod]
        public void ExpressionJsonSerializer_ReadOnlyRuleTable_AddCore_ThrowsNotSupported()
        {
            var ser = new ExpressionJsonSerializer
            {
                Rules =
                {
                    (Person p) => new Person { Name = p.Name, Age = p.Age }
                }
            };

            Assert.ThrowsException<NotSupportedException>(() => ser.Rules.AsReadOnly().Add((Book b) => new Book { Title = b.Title, YearPublished = b.YearPublished }));
        }

        [TestMethod]
        public void ExpressionJsonSerializer_AddT_ThrowsArgumentNull()
        {
            var ser = new ExpressionJsonSerializer
            {
                Rules =
                {
                    (Person p) => new Person { Name = p.Name, Age = p.Age }
                }
            };

            Expression<Func<Book, Book>> roundtrip = (Book b) => new Book { Title = b.Title };
            Action<ArgumentNullException> argNull(string paramName) => ex => Assert.AreEqual(paramName, ex.ParamName);

            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add(rule: null), argNull("rule"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(roundtrip: null), argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>("foo", roundtrip: null), argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(name: null, roundtrip), argNull("name"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(new Book(), roundtrip: null), ex => argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>("foo", new Book(), roundtrip: null), argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(name: null, new Book(), roundtrip), argNull("name"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(_ => true, roundtrip: null), argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(filter: null, roundtrip), argNull("filter"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(new Book(), _ => true, roundtrip: null), argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(new Book(), filter: null, roundtrip), argNull("filter"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>("foo", new Book(), _ => true, roundtrip: null), argNull("roundtrip"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>("foo", new Book(), filter: null, roundtrip), argNull("filter"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ser.Rules.Add<Book>(name: null, new Book(), _ => true, roundtrip), argNull("name"));
        }

        [TestMethod]
        public void ExpressionJsonSerializer_AddT_Success()
        {
            var ser = new ExpressionJsonSerializer();
            Expression<Func<Book, Book>> bookRoundtrip = (Book b) => new Book { Title = b.Title };
            Expression<Func<Person, Person>> personRoundtrip = (Person p) => new Person { Name = p.Name };
            ser.Rules.Add<Book>("foo1", new Book(), bookRoundtrip);
            Assert.IsNotNull(ser.Rules["foo1"]);
            ser.Rules.Add<Book>(new Book(), bookRoundtrip);
            Assert.IsNotNull(ser.Rules[nameof(Book)]);
            ser.Rules.Add<Person>(_ => true, personRoundtrip);
            Assert.IsNotNull(ser.Rules[nameof(Person)]);
            ser.Rules.Add<Book>("foo2", new Book(), bookRoundtrip);
            Assert.IsNotNull(ser.Rules["foo2"]);
        }

        [TestMethod]
        public void ExpressionJsonSerializer_GetRuleByName_ThrowsInvalidOperation()
        {
            var ser = new ExpressionJsonSerializer
            {
                Rules =
                {
                    (Person p) => new Person { Name = p.Name, Age = p.Age }
                }
            };

            void action()
            {
                var rule = ser.Rules["foo"];
            }

            Assert.ThrowsException<InvalidOperationException>(action);
        }
    }
}
