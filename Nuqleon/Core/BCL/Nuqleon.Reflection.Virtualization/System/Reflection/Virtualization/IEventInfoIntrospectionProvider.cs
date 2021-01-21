// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to introspect <see cref="EventInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IEventInfoIntrospectionProvider : IMemberInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the attributes associated with the specified event.
        /// </summary>
        /// <param name="event">The event to get the attributes for.</param>
        /// <returns>The attributes associated with the specified event.</returns>
        EventAttributes GetAttributes(EventInfo @event);

        /// <summary>
        /// Gets the type of the event handler.
        /// </summary>
        /// <param name="event">The event to get the event handler type for.</param>
        /// <returns>The type of the event handler.</returns>
        Type GetEventHandlerType(EventInfo @event);

        /// <summary>
        /// Gets a value indicating whether the event is multicast.
        /// </summary>
        /// <param name="event">The event to check for multicast.</param>
        /// <returns>true if the delegate is an instance of a multicast delegate; otherwise, false.</returns>
        bool IsMulticast(EventInfo @event);

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the .other directive.
        /// </summary>
        /// <param name="event">The event to get the associated methods for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing the public methods that have been associated with the event in metadata by using the .other directive. If there are no such public methods, an empty array is returned.</returns>
        IReadOnlyList<MethodInfo> GetOtherMethods(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Retrieves the <see cref="MethodInfo"/> object for the <see cref="EventInfo.AddEventHandler(object, Delegate)" /> method of the event, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="event">The event to get the add method for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo" /> object representing the method used to add an event handler delegate from the event source.</returns>
        MethodInfo GetAddMethod(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Returns the method that is called when the event is raised, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="event">The event to get the raise method for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo"/> object that was called when the event was raised.</returns>
        MethodInfo GetRaiseMethod(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Retrieves the <see cref="MethodInfo"/> object for the <see cref="EventInfo.RemoveEventHandler(object, Delegate)" /> method of the event, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="event">The event to get the remove method for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo" /> object representing the method used to remove an event handler delegate from the event source.</returns>
        MethodInfo GetRemoveMethod(EventInfo @event, bool nonPublic);
    }
}
