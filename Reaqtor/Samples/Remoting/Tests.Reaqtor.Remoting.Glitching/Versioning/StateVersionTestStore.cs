// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1822 // Mark as static. (Keeping instances for back-compat and addition of functionality later.)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tests.Reaqtor.Remoting.Glitching.Versioning
{
    public class StateVersionTestStore
    {
        private const string VersioningTestsBaseDirectoryKey = "VersioningTestsBaseDirectory";

        private static string BaseDirectory => AppDomain.CurrentDomain.GetData(VersioningTestsBaseDirectoryKey) as string ?? "";

        public void Add(MemberInfo testMethod, long savedAt, byte[] state)
        {
            EnsureDirectory();
            var fileNameBase = GetFileName(testMethod, savedAt);
            var stateFileName = fileNameBase + ".state";
            File.WriteAllBytes(GetPath(stateFileName), state);
            var testCaseFileName = fileNameBase + ".json";
            var testCase = new StateVersionTestCase(testMethod, savedAt);
            File.WriteAllText(GetPath(testCaseFileName), testCase.ToString());
        }

        public byte[] GetState(StateVersionTestCase testCase)
        {
            var stateFileName = GetFileName(testCase.TestMethod, testCase.SavedAt) + ".state";
            return File.ReadAllBytes(GetPath(stateFileName));
        }

        public IEnumerable<StateVersionTestCase> GetTestCasesByMember(MemberInfo member)
        {
            var fileNameBase = GetFileNameBase(member);
            var regex = new Regex("^" + fileNameBase + @"_\d+\.json$", RegexOptions.Compiled);
            foreach (var f in Directory.GetFiles(BaseDirectory).Where(f => regex.IsMatch(Path.GetFileName(f))))
            {
                if (StateVersionTestCase.TryParse(File.ReadAllText(f), out var testCase))
                {
                    Debug.Assert(testCase.TestMethod == member);
                    yield return testCase;
                }
            }
        }

        public IEnumerable<StateVersionTestCase> TestCases
        {
            get
            {
                foreach (var f in Directory.GetFiles(BaseDirectory).Where(f => f.EndsWith(".json")))
                {
                    if (StateVersionTestCase.TryParse(File.ReadAllText(f), out var testCase))
                    {
                        yield return testCase;
                    }
                }
            }
        }

        private static string GetFileNameBase(MemberInfo testMethod)
        {
            return testMethod.DeclaringType.Name + "_" + testMethod.Name;
        }

        private static string GetFileName(MemberInfo testMethod, long savedAt)
        {
            return GetFileNameBase(testMethod) + "_" + savedAt;
        }

        private static string GetPath(string filename)
        {
            return Path.Combine(BaseDirectory, filename);
        }

        private static void EnsureDirectory()
        {
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }
        }
    }
}
