// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Representation of a report for a heap partition.
    /// </summary>
    /// <remarks>
    /// Don't keep instances of this type alive for a prolonged duration. Instances of this type contain a set of
    /// reachable objects, which can't be garbage collected as long as the instance of the heap report is rooted.
    /// Consider calling <see cref="GetStats"/> and discarding the reference to the heap report as soon as possible.
    /// </remarks>
    public class HeapReport
    {
        /// <summary>
        /// Creates a new heap report with an empty <see cref="Objects"/> set.
        /// </summary>
        public HeapReport()
        {
            Objects = new ObjectSet();
        }

        /// <summary>
        /// Creates a new heap report with the specified <paramref name="objects"/> set.
        /// </summary>
        /// <param name="objects">The objects set to assign to <see cref="Objects"/>.</param>
        public HeapReport(ISet<object> objects)
        {
            Objects = objects;
        }

        /// <summary>
        /// Gets the set of all objects that are part of this report.
        /// </summary>
        public ISet<object> Objects { get; }

        /// <summary>
        /// Performs a deep clone of the current report instance.
        /// </summary>
        /// <returns>A deep clone of the current report instance.</returns>
        public HeapReport Clone()
        {
            return new HeapReport(new ObjectSet(Objects));
        }

        /// <summary>
        /// Computes and returns heap statistics.
        /// </summary>
        /// <returns>A heap statistics instance.</returns>
        public HeapStats GetStats()
        {
            var instanceCountPerType = new Dictionary<Type, long>();
            var elementCountPerArrayType = new Dictionary<Type, long>();
            var boxedValueCount = 0L;
            var stringCount = 0L;
            var totalStringCharacterCount = 0L;
            var totalBoxedValueByteCount = 0L;
            var totalByteCount = 0L;

            foreach (var obj in Objects)
            {
                var type = obj.GetType();

                if (!instanceCountPerType.TryGetValue(type, out var count))
                {
                    count = 0;
                }

                instanceCountPerType[type] = count + 1;

                if (type.IsArray)
                {
                    var array = (Array)obj;

                    if (!elementCountPerArrayType.TryGetValue(type, out var elemCount))
                    {
                        elemCount = 0;
                    }

                    elementCountPerArrayType[type] = elemCount + array.Length;
                }
                else if (type.IsValueType)
                {
                    boxedValueCount++;
                }

                if (obj is string s)
                {
                    stringCount++;
                    totalStringCharacterCount += s.Length;
                }
            }

            foreach (var kv in instanceCountPerType)
            {
                var type = kv.Key;
                var count = kv.Value;

                var size = TypeHelpers.SizeOf(type);

                if (type.IsValueType) // Boxing cost.
                {
                    size += TypeHelpers.IntPtrSize * 2; // SyncBlock + MethodTable
                }

                var totalSize = size * count;

                if (type.IsValueType)
                {
                    totalBoxedValueByteCount += totalSize;
                }

                totalByteCount += totalSize;
            }

            foreach (var kv in elementCountPerArrayType)
            {
                var type = kv.Key;
                var count = kv.Value;

                var elemType = type.GetElementType();

                var elemSize =
                    elemType.IsValueType ?
                    TypeHelpers.SizeOf(elemType) : // NB: This ignores alignment and padding.
                    TypeHelpers.IntPtrSize;

                totalByteCount += elemSize * count;
            }

            totalByteCount += totalStringCharacterCount * 2;

            return new HeapStats
            {
                InstanceCountPerType = instanceCountPerType,
                ElementCountPerArrayType = elementCountPerArrayType,
                BoxedValueCount = boxedValueCount,
                StringCount = stringCount,
                TotalStringCharacterCount = totalStringCharacterCount,
                TotalByteCount = totalByteCount,
                TotalBoxedValueByteCount = totalBoxedValueByteCount,
            };
        }

        /// <summary>
        /// Splits the heap report by GC generation.
        /// </summary>
        /// <returns>An array containing a heap report per GC generation. The index of the an array element denotes the GC generation.</returns>
        public HeapReport[] SplitByGeneration()
        {
            var maxGen = GC.MaxGeneration;

            //
            // Keep the hash sets and heap reports in separate arrays, so the core algorithm can do away
            // with traversing into the Objects property of the heap reports for each object.
            //

            var res = new HeapReport[maxGen + 1];
            var gens = new ObjectSet[maxGen + 1];

            for (var i = 0; i <= maxGen; i++)
            {
                var set = new ObjectSet();
                gens[i] = set;
                res[i] = new HeapReport(set);
            }

            //
            // Simply iterate over all objects and copy them to the set for the corresponding generation.
            //

            foreach (var obj in Objects)
            {
                gens[GC.GetGeneration(obj)].Add(obj);
            }

            return res;
        }
    }
}
