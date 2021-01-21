// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Constant definitions that must be exactly the same on both the client 
    /// and the service. These are just for the Rx operators.
    /// </summary>
    public static partial class Constants
    {
        public static partial class Observable
        {
            public static partial class Sum
            {
                public static class NoSelector
                {
                    public const string Int32 = "rx://operators/sum/Int32";
                    public const string NullableInt32 = "rx://operators/sum/NullableInt32";
                    public const string Int64 = "rx://operators/sum/Int64";
                    public const string NullableInt64 = "rx://operators/sum/NullableInt64";
                    public const string Single = "rx://operators/sum/Single";
                    public const string NullableSingle = "rx://operators/sum/NullableSingle";
                    public const string Double = "rx://operators/sum/Double";
                    public const string NullableDouble = "rx://operators/sum/NullableDouble";
                    public const string Decimal = "rx://operators/sum/Decimal";
                    public const string NullableDecimal = "rx://operators/sum/NullableDecimal";
                }

                public static class Selector
                {
                    public const string Int32 = "rx://operators/sum/selector/Int32";
                    public const string NullableInt32 = "rx://operators/sum/selector/NullableInt32";
                    public const string Int64 = "rx://operators/sum/selector/Int64";
                    public const string NullableInt64 = "rx://operators/sum/selector/NullableInt64";
                    public const string Single = "rx://operators/sum/selector/Single";
                    public const string NullableSingle = "rx://operators/sum/selector/NullableSingle";
                    public const string Double = "rx://operators/sum/selector/Double";
                    public const string NullableDouble = "rx://operators/sum/selector/NullableDouble";
                    public const string Decimal = "rx://operators/sum/selector/Decimal";
                    public const string NullableDecimal = "rx://operators/sum/selector/NullableDecimal";
                }
            }

            public static partial class Min
            {
                public static class NoSelector
                {
                    public const string Int32 = "rx://operators/min/Int32";
                    public const string NullableInt32 = "rx://operators/min/NullableInt32";
                    public const string Int64 = "rx://operators/min/Int64";
                    public const string NullableInt64 = "rx://operators/min/NullableInt64";
                    public const string Single = "rx://operators/min/Single";
                    public const string NullableSingle = "rx://operators/min/NullableSingle";
                    public const string Double = "rx://operators/min/Double";
                    public const string NullableDouble = "rx://operators/min/NullableDouble";
                    public const string Decimal = "rx://operators/min/Decimal";
                    public const string NullableDecimal = "rx://operators/min/NullableDecimal";
                }

                public static class Selector
                {
                    public const string Int32 = "rx://operators/min/selector/Int32";
                    public const string NullableInt32 = "rx://operators/min/selector/NullableInt32";
                    public const string Int64 = "rx://operators/min/selector/Int64";
                    public const string NullableInt64 = "rx://operators/min/selector/NullableInt64";
                    public const string Single = "rx://operators/min/selector/Single";
                    public const string NullableSingle = "rx://operators/min/selector/NullableSingle";
                    public const string Double = "rx://operators/min/selector/Double";
                    public const string NullableDouble = "rx://operators/min/selector/NullableDouble";
                    public const string Decimal = "rx://operators/min/selector/Decimal";
                    public const string NullableDecimal = "rx://operators/min/selector/NullableDecimal";
                }
            }

            public static partial class Max
            {
                public static class NoSelector
                {
                    public const string Int32 = "rx://operators/max/Int32";
                    public const string NullableInt32 = "rx://operators/max/NullableInt32";
                    public const string Int64 = "rx://operators/max/Int64";
                    public const string NullableInt64 = "rx://operators/max/NullableInt64";
                    public const string Single = "rx://operators/max/Single";
                    public const string NullableSingle = "rx://operators/max/NullableSingle";
                    public const string Double = "rx://operators/max/Double";
                    public const string NullableDouble = "rx://operators/max/NullableDouble";
                    public const string Decimal = "rx://operators/max/Decimal";
                    public const string NullableDecimal = "rx://operators/max/NullableDecimal";
                }

                public static class Selector
                {
                    public const string Int32 = "rx://operators/max/selector/Int32";
                    public const string NullableInt32 = "rx://operators/max/selector/NullableInt32";
                    public const string Int64 = "rx://operators/max/selector/Int64";
                    public const string NullableInt64 = "rx://operators/max/selector/NullableInt64";
                    public const string Single = "rx://operators/max/selector/Single";
                    public const string NullableSingle = "rx://operators/max/selector/NullableSingle";
                    public const string Double = "rx://operators/max/selector/Double";
                    public const string NullableDouble = "rx://operators/max/selector/NullableDouble";
                    public const string Decimal = "rx://operators/max/selector/Decimal";
                    public const string NullableDecimal = "rx://operators/max/selector/NullableDecimal";
                }
            }

            public static partial class Average
            {
                public static class NoSelector
                {
                    public const string Int32 = "rx://operators/average/Int32";
                    public const string NullableInt32 = "rx://operators/average/NullableInt32";
                    public const string Int64 = "rx://operators/average/Int64";
                    public const string NullableInt64 = "rx://operators/average/NullableInt64";
                    public const string Single = "rx://operators/average/Single";
                    public const string NullableSingle = "rx://operators/average/NullableSingle";
                    public const string Double = "rx://operators/average/Double";
                    public const string NullableDouble = "rx://operators/average/NullableDouble";
                    public const string Decimal = "rx://operators/average/Decimal";
                    public const string NullableDecimal = "rx://operators/average/NullableDecimal";
                }

                public static class Selector
                {
                    public const string Int32 = "rx://operators/average/selector/Int32";
                    public const string NullableInt32 = "rx://operators/average/selector/NullableInt32";
                    public const string Int64 = "rx://operators/average/selector/Int64";
                    public const string NullableInt64 = "rx://operators/average/selector/NullableInt64";
                    public const string Single = "rx://operators/average/selector/Single";
                    public const string NullableSingle = "rx://operators/average/selector/NullableSingle";
                    public const string Double = "rx://operators/average/selector/Double";
                    public const string NullableDouble = "rx://operators/average/selector/NullableDouble";
                    public const string Decimal = "rx://operators/average/selector/Decimal";
                    public const string NullableDecimal = "rx://operators/average/selector/NullableDecimal";
                }
            }

        }
    }
}
