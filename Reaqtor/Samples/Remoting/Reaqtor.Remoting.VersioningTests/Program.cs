// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.TestingFramework.TestRunner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace Reaqtor.Remoting.VersioningTests
{
    class Program
    {
        const string VersionTestsFileNameDefault = "TestCases.zip";
        const string VersioningTestsGatesFileNameDefault = "VersioningTestsGates.xml";

        const string VersioningTestsBaseDirectoryKey = "VersioningTestsBaseDirectory";
        const string VersioningTestsRunKey = "VersioningTestsRun";
        const string VersioningTestsSaveKey = "VersioningTestsSave";
        const string TemplatizeExpressionsKey = "TemplatizeExpressions";
        const string SerializationPolicyVersionKey = "SerializationPolicyVersion";
        const string VersioningTestsOptionsKey = "VersioningTestsOptions";

        static void Main(string[] args)
        {
            Console.Title = "Reaqtor.Remoting.VersioningTests";

            var results = new List<Tuple<Configuration, int, int>>();

            // Get settings from command line or App.config
            foreach (var configuration in GetConfigurations(args))
            {
                var directory = Path.Combine(Path.GetTempPath(), "Reaqtor.Remoting.VersioningTests", Guid.NewGuid().ToString());
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                try
                {
                    // Extract test cases from directory
                    if (configuration.Run)
                    {
                        if (!File.Exists(configuration.FileName))
                        {
                            var originalColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Could not find test cases at '{0}'.", configuration.FileName);
                            Console.ForegroundColor = originalColor;
                            continue;
                        }

                        Console.Write("Extracting test cases from {0}... ", configuration.FileName);
                        Extract(directory, configuration.FileName);
                        Console.WriteLine("Done.");
                    }

                    // Run tests
                    var path = GetAssemblyPath("Tests.Reactor.Remoting.Glitching");
                    var domain = AppDomain.CreateDomain("TestRunner", null, new AppDomainSetup { ApplicationBase = Path.GetDirectoryName(path) });
                    var instance = (TestAssemblyRunner)domain.CreateInstanceAndUnwrap(typeof(TestAssemblyRunner).Assembly.FullName, typeof(TestAssemblyRunner).FullName);
                    ApplySettings(configuration, directory, domain);
                    instance.Configure(path);
                    instance.Run();
                    results.Add(Tuple.Create(configuration, instance.TotalRuns, instance.TotalFailures));
                    instance.Dispose();

                    // Zip test cases in directory
                    if (configuration.Save)
                    {
                        Console.Write("Compressing test cases to {0}... ", configuration.FileName);
                        Compress(directory, configuration.FileName);
                        Console.WriteLine("Done.");
                    }
                }
                finally
                {
                    Console.Write("Cleaning up directory... ");
                    Directory.Delete(directory, true);
                    Console.WriteLine("Done.");
                }
            }

            using (var gateResults = new GateResults(VersioningTestsGatesFileNameDefault))
            {
                foreach (var result in results)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = result.Item3 > 0 ? ConsoleColor.Red : ConsoleColor.Green;
                    var message = string.Format(CultureInfo.InvariantCulture, "{0} - Total: {1} Passed: {2} Failed: {3}", result.Item1, result.Item2, result.Item2 - result.Item3, result.Item3);
                    Console.WriteLine(message);
                    Console.ForegroundColor = originalColor;
                    gateResults.Add(new GateResult { Name = result.Item1.ToString(), Message = message, Success = result.Item3 == 0 });
                }
            }
        }

        static string GetAssemblyPath(string name)
        {
            var assemblyPath = name + ".dll";
            if (!File.Exists(assemblyPath))
            {
#if DEBUG
                assemblyPath = string.Format(CultureInfo.InvariantCulture, @"..\..\..\{0}\bin\Debug\{0}.dll", name);
#else
                assemblyPath = string.Format(CultureInfo.InvariantCulture, @"..\..\..\{0}\bin\Release\{0}.dll", name);
#endif

                if (!File.Exists(assemblyPath))
                {
                    throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find assembly at '{0}'.", assemblyPath));
                }
            }

            return assemblyPath;
        }

        static void Extract(string directory, string fileName)
        {
            ZipFile.ExtractToDirectory(fileName, directory);
        }

        static void Compress(string directory, string path)
        {
            if (File.Exists(path)) { File.Delete(path); }
            using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                Directory.EnumerateFiles(directory).Where(p => !p.EndsWith(".zip")).ToList().ForEach(f => archive.CreateEntryFromFile(f, Path.GetFileName(f)));
            }
        }

        static IEnumerable<Configuration> GetConfigurations(string[] args)
        {
            var config = args.Length > 0
                ? XDocument.Load(args[0])
                : XDocument.Load("TestSettings.xml");

            var root = config.Element("Configurations");
            if (root != null)
            {
                foreach (var child in root.Elements())
                {
                    var settings = new Configuration();
                    switch (child.Name.LocalName)
	                {
                        case "Run":
                            ParseAttributes(child, settings);
                            settings.Run = true;
                            yield return settings;
                            break;
                        case "Save":
                            ParseAttributes(child, settings);
                            settings.Save = true;
                            yield return settings;
                            break;
		                default:
                            Console.WriteLine("Unexpected configuration node '{0}'.", child);
                            break;
	                }
                }
            }
        }

        static void ParseAttributes(XElement element, Configuration settings)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.LocalName)
	            {
                    case "FileName":
                        settings.FileName = attribute.Value;
                        break;
		            default:
                        settings.Options.Add(attribute.Name.LocalName, attribute.Value);
                        break;
	            }
            }
        }

        static void ApplySettings(Configuration settings, string directory, AppDomain domain)
        {
            domain.SetData(VersioningTestsBaseDirectoryKey, directory);
            domain.SetData(VersioningTestsRunKey, settings.Run);
            domain.SetData(VersioningTestsSaveKey, settings.Save);
            domain.SetData(VersioningTestsOptionsKey, settings.Options);
        }

        private sealed class Configuration
        {
            public string FileName = VersionTestsFileNameDefault;
            public bool Run = false;
            public bool Save = false;
            public IDictionary<string, string> Options = new Dictionary<string, string>();

            public override string ToString()
            {
                var type = Run ? "Run" : "Save";
                var options = string.Join(", ", Options.Select(kv => kv.Key + " = " + kv.Value));
                options = options != "" ? ", " + options : "";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{{ Type = {0}, FileName = {1}{2} }}",
                    type,
                    FileName,
                    options);
            }
        }

        private sealed class GateResults : List<GateResult>, IDisposable
        {
            private readonly string _outputPath;

            public GateResults(string outputPath)
            {
                _outputPath = outputPath;
            }

            public void Dispose()
            {
                var xml = new XDocument();
                var gates = new XElement("Gates");
                foreach (var result in this)
                {
                    var child = result.Success
                        ? new XElement("GatePassed")
                        : new XElement("GateFailed");
                    child.Add(new XElement("Name", new XCData(result.Name)));
                    child.Add(new XElement("Message", new XCData(result.Message)));
                    gates.Add(child);
                }
                xml.Add(gates);
                xml.Save(_outputPath);
            }
        }

        private struct GateResult
        {
            public bool Success;
            public string Name;
            public string Message;
        }
    }
}
