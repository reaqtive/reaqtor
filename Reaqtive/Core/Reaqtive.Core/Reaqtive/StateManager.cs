// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitting null checks for these. Will be non-null when moving to #nullable enable.

using System;
using System.Globalization;

namespace Reaqtive
{
    /// <summary>
    /// Provides a set of methods to perform state management operations.
    /// </summary>
    public static class StateManager
    {
        /// <summary>
        /// Loads the state of the specified stateful operator from the specified state reader.
        /// </summary>
        /// <param name="reader">Reader to read operator state from.</param>
        /// <param name="node">Operator whose state to read.</param>
        public static void LoadState(this IOperatorStateReader reader, IStatefulOperator node)
        {
            var version = LoadVersionInfo(reader, node);

            if (version != null)
            {
                node.LoadState(reader, version);
            }
            else
            {
                reader.Reset();
            }
        }

        /// <summary>
        /// Loads the state of the specified stateful operator using a reader obtained from the specified state reader factory.
        /// </summary>
        /// <param name="factory">Factory to create an operator state reader to read operator state from.</param>
        /// <param name="node">Operator whose state to read.</param>
        public static void LoadState(this IOperatorStateReaderFactory factory, IStatefulOperator node)
        {
            using var reader = factory.Create(node);

            LoadState(reader, node);
        }

        /// <summary>
        /// Saves the state of the specified stateful operator to the specified state writer.
        /// </summary>
        /// <param name="writer">Writer to write operator state to.</param>
        /// <param name="node">Operator whose state to write.</param>
        public static void SaveState(this IOperatorStateWriter writer, IStatefulOperator node)
        {
            SaveVersionInfo(writer, node);

            node.SaveState(writer, node.Version);
        }

        /// <summary>
        /// Saves the state of the specified stateful operator using a writer obtained from the specified state writer factory.
        /// </summary>
        /// <param name="factory">Factory to create an operator state writer to write operator state to.</param>
        /// <param name="node">Operator whose state to write.</param>
        public static void SaveState(this IOperatorStateWriterFactory factory, IStatefulOperator node)
        {
            using var writer = factory.Create(node);

            SaveState(writer, node);
        }

        /// <summary>
        /// Loads version information from the specified state reader and asserts that the version name tag matches with the specified versioned artifact's.
        /// </summary>
        /// <param name="reader">Reader to read version state from.</param>
        /// <param name="versioned">Version artifact whose version name tag to compart to the version information read from the reader.</param>
        /// <returns>The version number read from the state reader.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the state reader read a version name tag that doesn't match the name tag in the specified version artifact.</exception>
        public static Version LoadVersionInfo(this IOperatorStateReader reader, IVersioned versioned)
        {
            // The call to TryRead will only ever return false if the operator is transitioning.
            // Otherwise, the constructor call for the reader would have thrown.
            if (!reader.TryRead<string>(out string name) || (versioned is ITransitioningOperator && versioned.Name != name))
            {
                return null;
            }

            if (versioned.Name != name)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Failed to restore state for artifact '{0}' with expected tag '{1}'. State tag is '{2}'.", versioned.ToString(), versioned.Name, name));
            }

            var major = reader.Read<int>();
            var minor = reader.Read<int>();
            var build = reader.Read<int>();
            var revision = reader.Read<int>();

            return Versioning.GetVersion(major, minor, build, revision);
        }

        /// <summary>
        /// Saves the version information of the specified versioned artifact to the specified operator state writer.
        /// </summary>
        /// <param name="writer">Write to write version information to.</param>
        /// <param name="versioned">Versioned artifact whose version information to write to the state writer.</param>
        public static void SaveVersionInfo(this IOperatorStateWriter writer, IVersioned versioned)
        {
            writer.Write<string>(versioned.Name);

            writer.Write<int>(versioned.Version.Major);
            writer.Write<int>(versioned.Version.Minor);
            writer.Write<int>(versioned.Version.Build);
            writer.Write<int>(versioned.Version.Revision);
        }
    }
}
