// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1034 // Do not nest types. (Mirrors structure of namespaces.)
#pragma warning disable CA1720 // Identifier X contains type name.
#pragma warning disable CA1724 // Using System.* namespaces by design here.

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a catalog of immutable types in the Base Class Library.
    /// </summary>
    public static class ImmutableTypeCatalog
    {
        /// <summary>
        /// Immutable types in the System namespace.
        /// </summary>
        public static class System
        {
            /// <summary>
            /// Immutable types in the System.Collections namespace.
            /// </summary>
            public static class Collections
            {
                /// <summary>
                /// Immutable types in the System.Collections.Generic namespace.
                /// </summary>
                public static class Generic
                {
                    /// <summary>
                    /// Gets a table of immutable types in the System.Collections.Generic namespace.
                    /// </summary>
                    public static TypeTable AllThisNamespaceOnly { get; } = new TypeTable
                    {
                        typeof(global::System.Collections.Generic.KeyValuePair<,>),
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of immutable types in the System.Collections.Generic namespace and any child namespace.
                    /// </summary>
                    public static TypeTable AllThisAndChildNamespaces { get; } = new TypeTable
                    {
                        AllThisNamespaceOnly,
                    }.ToReadOnly();
                }

                /// <summary>
                /// Gets a table of immutable types in the System.Collections namespace.
                /// </summary>
                public static TypeTable AllThisNamespaceOnly { get; } = new TypeTable
                {
                }.ToReadOnly();

                /// <summary>
                /// Gets a table of immutable types in the System.Collections namespace.
                /// </summary>
                public static TypeTable AllThisAndChildNamespaces { get; } = new TypeTable
                {
                    AllThisNamespaceOnly,

                    Generic.AllThisAndChildNamespaces,
                }.ToReadOnly();
            }

            /// <summary>
            /// Immutable types in the System.Text namespace.
            /// </summary>
            public static class Text
            {
                /// <summary>
                /// Immutable types in the System.Text.RegularExpressions namespace.
                /// </summary>
                public static class RegularExpressions
                {
                    /// <summary>
                    /// Gets a table of immutable types in the System.Text.RegularExpressions namespace.
                    /// </summary>
                    public static TypeTable AllThisNamespaceOnly { get; } = new TypeTable
                    {
                        typeof(global::System.Text.RegularExpressions.Regex),
                        typeof(global::System.Text.RegularExpressions.MatchCollection),
                        typeof(global::System.Text.RegularExpressions.Match),
                        typeof(global::System.Text.RegularExpressions.GroupCollection),
                        typeof(global::System.Text.RegularExpressions.Group),
                        typeof(global::System.Text.RegularExpressions.CaptureCollection),
                        typeof(global::System.Text.RegularExpressions.Capture),
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of immutable types in the System.Text.RegularExpressions namespace and any child namespace.
                    /// </summary>
                    public static TypeTable AllThisAndChildNamespaces { get; } = new TypeTable
                    {
                        AllThisNamespaceOnly,
                    }.ToReadOnly();
                }


                /// <summary>
                /// Gets a table of immutable types in the System.Text namespace.
                /// </summary>
                public static TypeTable AllThisNamespaceOnly { get; } = new TypeTable
                {
                }.ToReadOnly();

                /// <summary>
                /// Gets a table of immutable types in the System.Text namespace and any child namespace.
                /// </summary>
                public static TypeTable AllThisAndChildNamespaces { get; } = new TypeTable
                {
                    AllThisNamespaceOnly,

                    RegularExpressions.AllThisAndChildNamespaces,
                }.ToReadOnly();
            }

            /// <summary>
            /// Gets a table of immutable types in the System namespace.
            /// </summary>
            public static TypeTable AllThisNamespaceOnly { get; } = new TypeTable
            {
                typeof(bool),

                typeof(byte),
                typeof(sbyte),
                typeof(ushort),
                typeof(short),
                typeof(uint),
                typeof(int),
                typeof(ulong),
                typeof(long),

                typeof(float),
                typeof(double),
                typeof(decimal),

                typeof(char),
                typeof(string),

                typeof(global::System.DateTime),
                typeof(global::System.DateTimeOffset),
                typeof(global::System.TimeSpan),

                typeof(global::System.Guid),
                typeof(global::System.Uri),
                typeof(global::System.Version),

                typeof(global::System.Nullable<>),

                typeof(global::System.Tuple<>),
                typeof(global::System.Tuple<,>),
                typeof(global::System.Tuple<,,>),
                typeof(global::System.Tuple<,,,>),
                typeof(global::System.Tuple<,,,,>),
                typeof(global::System.Tuple<,,,,,>),
                typeof(global::System.Tuple<,,,,,,>),
                typeof(global::System.Tuple<,,,,,,,>),

                // NB: ValueTuple types are mutable.
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of immutable types in the System namespace and any child namespace.
            /// </summary>
            public static TypeTable AllThisAndChildNamespaces { get; } = new TypeTable
            {
                AllThisNamespaceOnly,

                Collections.AllThisAndChildNamespaces,
                Text.AllThisAndChildNamespaces,
            }.ToReadOnly();
        }

        /// <summary>
        /// Gets a table of immutable types in the Base Class Library.
        /// </summary>
        public static TypeTable All { get; } = new TypeTable
        {
            System.AllThisAndChildNamespaces,
        }.ToReadOnly();
    }
}
