// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable
{
    /// <summary>
    /// Manager for the firehose observable instances
    /// </summary>
    public static class FirehoseObservableManager
    {
        /// <summary>
        /// Holds the firehose observable instances mapped per Stream Uri
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> FirehosePerTopic = new();

        /// <summary>
        /// Returns an instance of <see cref="FirehoseObservable{TResult}"/> for the specified topic message topic
        /// </summary>
        /// <typeparam name="TResult">the types of objects pumping out of the firehose</typeparam>
        /// <param name="messageTopic">the topic of the firehose messages</param>
        /// <returns>an <see cref="IObservable{TResult}"/> instance</returns>
        public static IObservable<TResult> GetFirehoseObservable<TResult>(Uri messageTopic)
        {
            if (messageTopic == null)
            {
                throw new ArgumentNullException(nameof(messageTopic));
            }

            var canonicalMessageTopic = messageTopic.ToCanonicalString();
            return (IObservable<TResult>)FirehosePerTopic.GetOrAdd(
                canonicalMessageTopic,
                _ =>
                {
                    Tracer.TraceSource.TraceInformation(
                        "Creating a firehose instance for the ({0}) stream", canonicalMessageTopic);
                    return
                        new FirehoseObservable<TResult>(messageTopic).Finally(
                            () => RemoveFirehose(canonicalMessageTopic)).Publish().RefCount();
                });
        }

        /// <summary>
        /// Cleans up the firehose per topic map when the last firehose instance of this topic was disposed
        /// </summary>
        /// <param name="messageTopic">the topic used for all the messages going through that firehose instance that just got disposed</param>
        private static void RemoveFirehose(string messageTopic)
        {
            Tracer.TraceSource.TraceInformation(
                "Removing the last firehose reference for the ({0}) stream", messageTopic);

            bool removalSuccess = FirehosePerTopic.TryRemove(messageTopic, out _);

            Tracer.TraceSource.TraceEvent(
                System.Diagnostics.TraceEventType.Verbose,
                0,
                removalSuccess
                    ? "Successfully removed the last firehose reference for the ({0}) stream"
                    : "Failed to remove the last firehose reference for the ({0}) stream",
                messageTopic);
        }
    }
}
