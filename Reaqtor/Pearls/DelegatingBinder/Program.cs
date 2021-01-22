// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;

namespace DelegatingBinder
{
    public class Program
    {
        public static void Main()
        {
            Demo();
            Perf();
        }

        private static void Demo()
        {
            var service = new Service();
            var client = new Client(service);

            client.GetSubjectFactory<Person>("subject/partitioned").Create("people");

            var sub = client.GetObservable<Person>("people").Where(x => x.Name == "Bart").Select(x => x.ToString()).Subscribe("sub", client.GetObserver<string>("cout"));

            var xs = client.GetObserver<Person>("people");
            xs.OnNext(new Person { Name = "Homer", Age = 38 });
            xs.OnNext(new Person { Name = "Bart", Age = 10 });
            xs.OnNext(new Person { Name = "Lisa", Age = 8 });

            sub.Dispose();
        }

        private static void Perf()
        {
            var I = 2;     // number of interesting keys
            var K = 10000; // number of subscriptions per interesting key (I)

            var J = 3;     // number of keys with events
            var N = 10000; // number of events sent per key (J)

            var r = new Random();
            var keys = new[] { "Homer", "Bart", "Lisa" }.Take(I).SelectMany(n => Enumerable.Repeat(n, K)).OrderBy(_ => r.Next()).ToArray();
            var events = new[] { new Person { Name = "Homer", Age = 38 }, new Person { Name = "Bart", Age = 10 }, new Person { Name = "Lisa", Age = 8 } }.Take(J).SelectMany(e => Enumerable.Repeat(e, N)).OrderBy(_ => r.Next()).ToArray();

            Perf("subject", keys, events);
            Perf("subject/partitioned", keys, events);
        }

        private static void Perf(string streamFactory, IEnumerable<string> keys, IEnumerable<Person> events)
        {
            var service = new Service();
            var client = new Client(service);

            client.GetSubjectFactory<Person>(streamFactory).Create("people");

            var people = client.GetObservable<Person>("people");
            var nop = client.GetObserver<string>("nop");

            Console.Write("Subscribing... ");

            var sw = Stopwatch.StartNew();

            var i = 0;
            foreach (var key in keys)
            {
                people.Where(x => x.Name == key).Select(x => x.ToString()).Subscribe("sub" + i, nop); // NOTE: This path is not optimized (cf. compiled delegate caching in IRP)
                i++;
            }

            Console.WriteLine("Done. " + sw.Elapsed);

            var xs = client.GetObserver<Person>("people");

            Console.Write("Publishing... ");

            sw.Restart();

            foreach (var person in events)
            {
                xs.OnNext(person);
            }

            Console.WriteLine("Done. " + sw.Elapsed);
        }
    }

    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString() => Name + " is " + Age;
    }
}
