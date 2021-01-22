// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Reflection;

namespace System.Linq.CompilerServices.Reflection
{
    internal static class EventInfoExtensions
    {
        internal static bool IsStatic(this EventInfo @event) => @event.GetAddMethod().IsStatic;

        internal static string ToCSharpString(this EventInfo @event)
        {
            // NOTE: This produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var dot = IsStatic(@event) ? "::" : ".";

            return "event " + @event.EventHandlerType.ToCSharpString() + " " + @event.DeclaringType.ToCSharpString() + dot + @event.Name;
        }
    }
}
