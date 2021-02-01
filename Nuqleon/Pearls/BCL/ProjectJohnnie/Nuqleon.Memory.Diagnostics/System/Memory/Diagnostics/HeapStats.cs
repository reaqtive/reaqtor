// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Representation of a collection of heap usage statistics.
    /// </summary>
    public sealed class HeapStats
    {
        /// <summary>
        /// Gets the number of instances per type.
        /// </summary>
        public IReadOnlyDictionary<Type, long> InstanceCountPerType { get; set; }

        /// <summary>
        /// Gets the total number of elements across all instances per array type.
        /// </summary>
        public IReadOnlyDictionary<Type, long> ElementCountPerArrayType { get; set; }

        /// <summary>
        /// Gets the total number of boxed values.
        /// </summary>
        public long BoxedValueCount { get; set; }

        /// <summary>
        /// Gets the total number of strings.
        /// </summary>
        public long StringCount { get; set; }

        /// <summary>
        /// Gets the total number of characters across all string instances.
        /// </summary>
        public long TotalStringCharacterCount { get; set; }

        /// <summary>
        /// Gets the total number of bytes used by boxed values.
        /// </summary>
        public long TotalBoxedValueByteCount { get; set; }

        /// <summary>
        /// Gets the total number of bytes used.
        /// </summary>
        /// <remarks>
        /// This number excludes any free space on the heap and does not take padding or alignment
        /// across consecutive objects on the heap into account.
        /// </remarks>
        public long TotalByteCount { get; set; }

        /// <summary>
        /// Gets a string representation of the heap statistics, usable for printing a report.
        /// </summary>
        /// <returns>String representation of the heap statistics.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(); // PERF: Consider pooling.

            ToString(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Appends a string representation of the heap statistics to the specified string <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        public void ToString(StringBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            ToStringInstanceCountPerType(builder);
            ToStringElementCountPerArrayType(builder);
            ToStringStringStats(builder);
            ToStringGlobalStats(builder);
        }

        /// <summary>
        /// Appends information about <see cref="InstanceCountPerType"/>.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        private void ToStringInstanceCountPerType(StringBuilder builder)
        {
            builder.AppendLine("Objects per type");
            builder.AppendLine("================");
            builder.AppendLine();

            builder.AppendLine("Count\tSize\tTotal\tType");
            builder.AppendLine("-------\t-------\t-------\t--------------------------------");

            var sumCount = 0L;
            var sumSize = 0L;

            foreach (var kv in InstanceCountPerType)
            {
                var type = kv.Key;
                var count = kv.Value;

                var size = TypeHelpers.SizeOf(type);

                if (type.IsValueType) // Boxing cost.
                {
                    size += TypeHelpers.IntPtrSize * 2; // SyncBlock + MethodTable
                }

                var totalSize = count * size;

                builder
                    .Append(count).Append('\t')
                    .Append(size).Append('\t')
                    .Append(totalSize).Append('\t')
                    .Append(type).AppendLine();

                sumCount += count;
                sumSize += totalSize;
            }

            builder.AppendLine("-------\t-------\t-------\t--------------------------------");

            builder
                .Append(sumCount).Append('\t')
                .Append("").Append('\t')
                .Append(sumSize).Append('\t')
                .Append("TOTAL").AppendLine();

            builder.AppendLine();
        }

        /// <summary>
        /// Appends information about <see cref="ElementCountPerArrayType"/>.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        private void ToStringElementCountPerArrayType(StringBuilder builder)
        {
            builder.AppendLine("Elements per array type");
            builder.AppendLine("=======================");
            builder.AppendLine();

            builder.AppendLine("Elems\tSize\tTotal\tArray type");
            builder.AppendLine("-------\t-------\t-------\t--------------------------------");

            var sumCount = 0L;
            var sumSize = 0L;

            foreach (var kv in ElementCountPerArrayType)
            {
                var type = kv.Key;
                var count = kv.Value;

                var elemType = type.GetElementType();

                var elemSize =
                    elemType.IsValueType ?
                    TypeHelpers.SizeOf(elemType) : // NB: This ignores alignment and padding.
                    TypeHelpers.IntPtrSize;

                var totalSize = elemSize * count;

                builder
                    .Append(count).Append('\t')
                    .Append(elemSize).Append('\t')
                    .Append(totalSize).Append('\t')
                    .Append(type).AppendLine();

                sumCount += count;
                sumSize += totalSize;
            }

            builder.AppendLine("-------\t-------\t-------\t--------------------------------");

            builder
                .Append(sumCount).Append('\t')
                .Append("").Append('\t')
                .Append(sumSize).Append('\t')
                .Append("TOTAL").AppendLine();

            builder.AppendLine();
        }

        /// <summary>
        /// Appends information about <see cref="TotalStringCharacterCount"/>.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        private void ToStringStringStats(StringBuilder builder)
        {
            builder.AppendLine("Strings");
            builder.AppendLine("=======");
            builder.AppendLine();

            builder.Append("Total string character count: ").Append(TotalStringCharacterCount).AppendLine();
            builder.AppendLine();
        }

        /// <summary>
        /// Appends information about <see cref="TotalBoxedValueByteCount"/> and <see cref="TotalByteCount"/>.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        private void ToStringGlobalStats(StringBuilder builder)
        {
            builder.AppendLine("Summary");
            builder.AppendLine("=======");
            builder.AppendLine();

            var count = InstanceCountPerType.Sum(kv => kv.Value);

            builder.Append("Total object count: ").Append(count).AppendLine();
            builder.Append("Total boxed value bytes: ").Append(TotalBoxedValueByteCount).AppendLine();
            builder.Append("Total bytes: ").Append(TotalByteCount).AppendLine();
            builder.AppendLine();
        }
    }
}
