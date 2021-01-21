// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using Newtonsoft.Json;

using Nuqleon.Json.Serialization;

namespace Perf.Nuqleon.Json.Serialization
{
    public class Serialize
    {
        private readonly IFastJsonSerializer<Person> _nucleusSerializer;
        private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer;
        private readonly Person _person;

        public Serialize()
        {
            _nucleusSerializer = FastJsonSerializerFactory.CreateSerializer<Person>(DefaultNameProvider.Instance, FastJsonConcurrencyMode.SingleThreaded);
            _jsonSerializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
            _person = new Person { Name = "Bart", Age = 21 };
        }

        [Benchmark]
        public void Nuqleon()
        {
            _ = _nucleusSerializer.Serialize(_person);
        }

        [Benchmark]
        public void Newtonsoft_JsonConvert()
        {
            _ = JsonConvert.SerializeObject(_person);
        }

        [Benchmark]
        public void Newtonsoft_JsonSerializer()
        {
            var sb = new StringBuilder();

            using var sw = new StringWriter(sb);
            using JsonWriter writer = new JsonTextWriter(sw);

            _jsonSerializer.Serialize(writer, _person);

            _ = sb.ToString();
        }

        [Benchmark]
        public void SystemText()
        {
            _ = System.Text.Json.JsonSerializer.Serialize(_person);
        }
    }

    public class Deserialize
    {
        private readonly IFastJsonDeserializer<Person> _nucleusDeserializer;
        private readonly Newtonsoft.Json.JsonSerializer _jsonDeserializer;
        private readonly string _json;

        public Deserialize()
        {
            _nucleusDeserializer = FastJsonSerializerFactory.CreateDeserializer<Person>(DefaultNameResolver.Instance, FastJsonConcurrencyMode.SingleThreaded);
            _jsonDeserializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
            _json = "{ \"Name\": \"Bart\", \"Age\": 21 }";
        }

        [Benchmark]
        public void Nuqleon()
        {
            _ = _nucleusDeserializer.Deserialize(_json);
        }

        [Benchmark]
        public void Newtonsoft_JsonConvert()
        {
            _ = JsonConvert.DeserializeObject<Person>(_json);
        }

        [Benchmark]
        public void Newtonsoft_JsonSerializer()
        {
            using var sr = new StringReader(_json);
            using JsonReader reader = new JsonTextReader(sr);

            _ = _jsonDeserializer.Deserialize<Person>(reader);
        }

        [Benchmark]
        public void SystemText()
        {
            _ = System.Text.Json.JsonSerializer.Deserialize<Person>(_json);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
