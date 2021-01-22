// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Json = Nuqleon.Json;

namespace Tests.Reaqtor.Remoting.Glitching.Versioning
{
    public class StateVersionTestCase
    {
        private const string TestTypeKey = "TestType";
        private const string TestMethodKey = "TestMethod";
        private const string SavedAtKey = "SavedAt";

        private static readonly Type[] TestArgs = Type.EmptyTypes;

        public StateVersionTestCase(MemberInfo testMethod, long savedAt)
        {
            TestMethod = testMethod;
            SavedAt = savedAt;
        }

        public MemberInfo TestMethod
        {
            get;
            private set;
        }

        public long SavedAt
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Json.Expressions.Expression.Object(new Dictionary<string, Json.Expressions.Expression>
            {
                { TestTypeKey, Json.Expressions.Expression.String(TestMethod.DeclaringType.AssemblyQualifiedName) },
                { TestMethodKey, Json.Expressions.Expression.String(TestMethod.Name) },
                { SavedAtKey, Json.Expressions.Expression.String(SavedAt.ToString()) },
            }).ToString();
        }

        public static bool TryParse(string data, out StateVersionTestCase testCase)
        {
            Json.Expressions.Expression json;
            try
            {
                json = Json.Expressions.Expression.Parse(data);
            }
            catch (Json.Parser.ParseException)
            {
                testCase = default;
                return false;
            }

            var obj = json as Json.Expressions.ObjectExpression;
            if (json != null && obj == null)
            {
                testCase = default;
                return false;
            }

            return TryExtract(obj, out testCase);
        }

        private static bool TryExtract(Json.Expressions.ObjectExpression json, out StateVersionTestCase testCase)
        {
            if (json.Members[TestTypeKey] is not Json.Expressions.ConstantExpression testTypeExpr)
            {
                testCase = default;
                return false;
            }
            var testType = Type.GetType(testTypeExpr.Value.ToString());
            if (testType == null)
            {
                testCase = default;
                return false;
            }

            var testMethodExpr = json.Members[TestMethodKey] as Json.Expressions.ConstantExpression;
            var testMethodName = testMethodExpr.Value.ToString();
            var testMethod = testType.GetMethod(testMethodName, TestArgs);
            if (testMethod == null)
            {
                testCase = default;
                return false;
            }

            if (json.Members[SavedAtKey] is not Json.Expressions.ConstantExpression savedAtExpr || !long.TryParse(savedAtExpr.Value.ToString(), out var savedAt))
            {
                testCase = default;
                return false;
            }

            testCase = new StateVersionTestCase(testMethod, savedAt);
            return true;
        }
    }
}
