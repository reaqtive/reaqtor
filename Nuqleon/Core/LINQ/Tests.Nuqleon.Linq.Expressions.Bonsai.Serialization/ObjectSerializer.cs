// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions.Bonsai.Serialization;

namespace Tests
{
    [TestClass]
    public class ObjectSerializerTests
    {
        [TestMethod]
        public void ObjectSerializer_SimpleTypes()
        {
            var ser = new ObjectSerializer();

            var lst = new Dictionary<Type, object[]>
            {
                { typeof(int), new object[] { int.MinValue, -123, 0, 42, int.MaxValue }  },
                { typeof(double), new object[] { -123.45, 0.0, 42.89 }  },
                { typeof(bool), new object[] { true, false }  },

                { typeof(int?), new object[] { int.MinValue, -123, 0, 42, int.MaxValue, default(int?) }  },
                { typeof(double?), new object[] { -123.45, 0.0, 42.89, default(double?) }  },
                { typeof(bool?), new object[] { true, false, default(bool?) }  },

                { typeof(string), new object[] { "", "bar", default(string) }  },

                { typeof(Guid), new object[] { new Guid(), Guid.NewGuid() }  },

                { typeof(DateTime), new object[] { DateTime.Now }  },
                { typeof(TimeSpan), new object[] { TimeSpan.Zero, new TimeSpan(7, 42, 35, 12) }  },
                { typeof(DateTimeOffset), new object[] { DateTimeOffset.Now }  },
            };

            foreach (var e in lst)
            {
                var s = ser.GetSerializer(e.Key);
                var d = ser.GetDeserializer(e.Key);

                foreach (var x in e.Value)
                {
                    var y = d(s(x));

                    Assert.AreEqual(x, y);
                }
            }
        }
    }
}
