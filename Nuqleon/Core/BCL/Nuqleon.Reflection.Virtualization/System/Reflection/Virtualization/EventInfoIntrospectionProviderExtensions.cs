// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="IEventInfoIntrospectionProvider"/>.
    /// </summary>
    public static class EventInfoIntrospectionProviderExtensions
    {
        /// <summary>
        /// Retrieves the <see cref="MethodInfo"/> object for the <see cref="EventInfo.AddEventHandler(object, Delegate)" /> method of the event.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="event">The event to get the add method for.</param>
        /// <returns>A <see cref="MethodInfo" /> object representing the method used to add an event handler delegate from the event source.</returns>
        public static MethodInfo GetAddMethod(this IEventInfoIntrospectionProvider provider, EventInfo @event) => NotNull(provider).GetAddMethod(@event, nonPublic: false);

        /// <summary>
        /// Retrieves the <see cref="MethodInfo"/> object for the <see cref="EventInfo.RemoveEventHandler(object, Delegate)" /> method of the event.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="event">The event to get the remove method for.</param>
        /// <returns>A <see cref="MethodInfo" /> object representing the method used to remove an event handler delegate from the event source.</returns>
        public static MethodInfo GetRemoveMethod(this IEventInfoIntrospectionProvider provider, EventInfo @event) => NotNull(provider).GetRemoveMethod(@event, nonPublic: false);

        /// <summary>
        /// Returns the method that is called when the event is raised.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="event">The event to get the raise method for.</param>
        /// <returns>A <see cref="MethodInfo"/> object that was called when the event was raised.</returns>
        public static MethodInfo GetRaiseMethod(this IEventInfoIntrospectionProvider provider, EventInfo @event) => NotNull(provider).GetRaiseMethod(@event, nonPublic: false);

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the .other directive.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="event">The event to get the associated methods for.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing the public methods that have been associated with the event in metadata by using the .other directive. If there are no such public methods, an empty array is returned.</returns>
        public static IReadOnlyList<MethodInfo> GetOtherMethods(this IEventInfoIntrospectionProvider provider, EventInfo @event) => NotNull(provider).GetOtherMethods(@event, nonPublic: false);

        /// <summary>
        /// Gets a value indicating whether the event has the SpecialName attribute.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="event">The event to inspect.</param>
        /// <returns>true if the event has the SpecialName attribute set; otherwise, false.</returns>
        public static bool IsSpecialName(this IEventInfoIntrospectionProvider provider, EventInfo @event) => (NotNull(provider).GetAttributes(@event) & EventAttributes.SpecialName) > EventAttributes.None;
    }
}
