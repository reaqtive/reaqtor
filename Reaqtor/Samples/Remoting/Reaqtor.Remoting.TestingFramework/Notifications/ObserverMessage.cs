// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Reaqtive.Testing;

using Reaqtor.Remoting.Protocol;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.Remoting.TestingFramework
{
    public static class ObserverMessage
    {
        public static Recorded<INotification<T>> OnNext<T>(T value)
        {
            return new Recorded<INotification<T>>(-1, ObserverNotification.CreateOnNext<T>(value));
        }

        public static Recorded<INotification<T>> OnNext<T>(long ticks, T value)
        {
            return new Recorded<INotification<T>>(ticks, ObserverNotification.CreateOnNext<T>(value));
        }

        public static Recorded<INotification<T>> OnNext<T>(long ticks, Func<T, bool> predicate)
        {
            return new Recorded<INotification<T>>(ticks, ObserverNotification.CreateOnNext<T>(predicate));
        }

        public static Recorded<INotification<T>> OnNext<T>(DateTimeOffset dueTime, T value)
        {
            return OnNext<T>(dueTime.Ticks, value);
        }

        public static Recorded<INotification<T>> OnNext<T>(TimeSpan dueTime, T value)
        {
            return OnNext<T>(dueTime.Ticks, value);
        }

        public static Recorded<INotification<T>> OnError<T>(Exception error)
        {
            return new Recorded<INotification<T>>(-1, ObserverNotification.CreateOnError<T>(error));
        }

        public static Recorded<INotification<T>> OnError<T>(long ticks, Exception error)
        {
            return new Recorded<INotification<T>>(ticks, ObserverNotification.CreateOnError<T>(error));
        }

        public static Recorded<INotification<T>> OnError<T>(long ticks, Func<Exception, bool> predicate)
        {
            return new Recorded<INotification<T>>(ticks, ObserverNotification.CreateOnError<T>(predicate));
        }

        public static Recorded<INotification<T>> OnError<T>(DateTimeOffset dueTime, Exception error)
        {
            return OnError<T>(dueTime.Ticks, error);
        }

        public static Recorded<INotification<T>> OnError<T>(TimeSpan dueTime, Exception error)
        {
            return OnError<T>(dueTime.Ticks, error);
        }

        public static Recorded<INotification<T>> OnCompleted<T>()
        {
            return new Recorded<INotification<T>>(-1, ObserverNotification.CreateOnCompleted<T>());
        }

        public static Recorded<INotification<T>> OnCompleted<T>(long ticks)
        {
            return new Recorded<INotification<T>>(ticks, ObserverNotification.CreateOnCompleted<T>());
        }

        public static Recorded<INotification<T>> OnCompleted<T>(DateTimeOffset dueTime)
        {
            return OnCompleted<T>(dueTime.Ticks);
        }

        public static Recorded<INotification<T>> OnCompleted<T>(TimeSpan dueTime)
        {
            return OnCompleted<T>(dueTime.Ticks);
        }

        public static void AssertEqual<T>(this IList<Recorded<INotification<T>>> messages, params Recorded<INotification<T>>[] other)
        {
            if (!messages.SequenceEqual(other, RecordedNotificationEqualityComparer<T>.Default))
            {
                Assert.Fail(Message(messages, other));
            }
        }

        private static string Message<T>(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("Expected: [");
            sb.Append(string.Join(", ", expected.Select(x => x.ToString()).ToArray()));
            sb.Append(']');
            sb.AppendLine();
            sb.Append("Actual..: [");
            sb.Append(string.Join(", ", actual.Select(x => x.ToString()).ToArray()));
            sb.Append(']');
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
