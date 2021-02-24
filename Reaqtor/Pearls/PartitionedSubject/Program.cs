// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - October 2014
//

//
// Design notes:
//
//   The goal is to optimize key-based lookup patterns a la
//
//     weather.Where(w => w.TileId == "someId")
//
//   Effectively, this filters weather entities by a key using an equality check. When
//   implemented naively, many such subscriptions will lead to an evaluation of these
//   predicates for each incoming event. When having N such subscriptions and M events
//   there will be N x M evaluations taking place.
//
//   In order to optimize this, a few strategies can be followed. First, the use of
//   common subexpression elimination could help by coalescing filters that have the
//   same condition. However, if the domain is sparse, effectiveness of this technique
//   would be very low. In addition, the resulting common Where nodes in the evaluation
//   graph would still need to be executed for each event. If there's P unique keys in
//   the domain, there's be an upper bound of P x M evaluations.
//
//   The more effective technique described in this sample uses partitioning by a key
//   value for each event. In the sample above, weather is substituted for a subject
//   with a partition key specified:
//
//     var weather = new PartitionedSubject<WeatherInfo, string>(w => w.TileId);
//
//   In IRP terms, this would be equivalent to:
//
//     var sf = ctx.GetStreamFactory<string, WeatherInfo, WeatherInfo>(partSubjFactUri);
//     var weather = await sf.CreateAsync(weatherUri, w => w.TileId);
//
//   where the key selector function is specified as an expression tree.
//
//   Each subscription that filters by a partition key value can specify this value as
//   a parameter. In this prototype, we specify the key as a parameter to the Subscribe
//   method, i.e.:
//
//     var d = weather.Subscribe("someId", observer);
//
//   In IRP terms, this would map to a parameterized observable:
//
//     var weatherByTile = ctx.GetObservable<string, WeatherInfo>(weatherUri);
//     var subscription = await weatherByTile("someId").SubscribeAsync(observer, ...);
//
//   As an additional layer on top of this - but not demonstrated in this example - we
//   could provide key-based filtering as a recognized pattern that translates to the
//   parameterized approach shown above:
//
//     var weather = ctx.GetObservable<WeatherInfo>(weatherUri);
//     var subscription = await weather.Where(w => w.TileId == "someId")
//                                     .SubscribeAsync(observer, ...);
//
//   In order to achieve this, a few options can be considered:
//
//     1. Adding intelligence to the binder.
//     2. Truly implementing a delegation pattern of operators into subjects.
//
//   It goes without saying the latter will be the preferred option. This brings up the
//   point that the recognized patterns may need to be parameterized given that keys may
//   be of types where equality checks are domain-specific. One could imagine a stream
//   factory for this type of stream that supports delegation of filters to take more
//   parameters to specify matching patterns:
//
//     var sf = ctx.GetStreamFactory<string, WeatherInfo, WeatherInfo>(partSubjFactUri);
//     var weather = await sf.CreateAsync(weatherUri, w => w.TileId,
//                                        (key, value) => key == value,
//                                        (key, value) => key.Equals(value),
//                                        ...);
//
//   In here, the key and value parameters would be of the TKey type (string in this
//   example), and the body of the lambda would provide a matching pattern. Aspects of
//   commutativity could be considered given we're talking about equality checks here.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace PartitionedSubject
{
    public class Program
    {
        public static void Main()
        {
            Naive();
            Optimized();
        }

        private static void Naive()
        {
            var M = 1000;

            var sub = new UnorderedFastSubject<Weather>();

            var c = new Dictionary<string, int>();

            var rnd1 = new Random();

            var sw = Stopwatch.StartNew();

            Console.Write("Subscribing " + M + "... ");

            for (var m = 0; m < M; m++)
            {
                if (m % 1000 == 0)
                    Console.Title = m.ToString();

                var tile = GetTile(rnd1);

                sub.Where(w => w.Tile == tile).Subscribe(Observer.Create<Weather>(_ =>
                {
                    c.TryGetValue(tile, out var d);
                    c[tile] = d + 1;
                }));
            }

            Console.WriteLine("Done " + sw.Elapsed);

            Publish(sub, 10000);

            Console.WriteLine("{0} tiles, avg {1}/tile, max {2}/tile", c.Count, c.Values.Average(), c.Values.Max());
        }

        private static void Optimized()
        {
            var M = 1000000;

            var sub = new PartitionedSubject<Weather, string>(w => w.Tile, EqualityComparer<string>.Default);

            var c = new Dictionary<string, int>();

            var rnd1 = new Random();

            var sw = Stopwatch.StartNew();

            Console.Write("Subscribing " + M + "... ");

            for (var m = 0; m < M; m++)
            {
                if (m % 1000 == 0)
                    Console.Title = m.ToString();

                var tile = GetTile(rnd1);

                sub.Subscribe(tile, Observer.Create<Weather>(_ =>
                {
                    c.TryGetValue(tile, out var d);
                    c[tile] = d + 1;
                }));
            }

            Console.WriteLine("Done " + sw.Elapsed);

            Publish(sub);

            Console.WriteLine("{0} tiles, avg {1}/tile, max {2}/tile", c.Count, c.Values.Average(), c.Values.Max());
        }

        private static void Publish(IObserver<Weather> sub, int N = 1000000)
        {
            var sw = Stopwatch.StartNew();

            Console.Write("Publishing " + N + "... ");

            var pub = Task.Run(() =>
            {
                var rnd2 = new Random();

                for (var n = 0; n < N; n++)
                {
                    if (n % 1000 == 0)
                        Console.Title = n.ToString();

                    var tile = GetTile(rnd2);

                    sub.OnNext(new Weather { Tile = tile });
                }
            });

            pub.Wait();

            Console.WriteLine("Done " + sw.Elapsed);
        }

        private static string GetTile(Random rnd)
        {
            //var i = rnd.Next(0, 99999);
            //var j = rnd.Next(0, 99999);

            //var x = i.ToString().PadLeft(5, '0');
            //var y = j.ToString().PadLeft(5, '0');

            var i = rnd.Next(0, 999);
            var j = rnd.Next(0, 999);

            var x = i.ToString().PadLeft(3, '0');
            var y = j.ToString().PadLeft(3, '0');

            var tile = "t000x" + x + "y" + y;
            return tile;
        }
    }

    internal class Weather
    {
        public string Tile { get; set; }
    }
}
