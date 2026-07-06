// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public static class Helpers
    {
        public static IEnumerable<Recorded<INotification<T>>> DeserializeObserverMessages<T>(IList<Recorded<INotification<string>>> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                switch (message.Value.Kind)
                {
                    case Protocol.NotificationKind.OnCompleted:
                        yield return ObserverMessage.OnCompleted<T>(message.Time);
                        break;
                    case Protocol.NotificationKind.OnError:
                        yield return ObserverMessage.OnError<T>(message.Time, message.Value.Exception);
                        break;
                    case Protocol.NotificationKind.OnNext:
                        var deserialized = new SerializationHelpers().Deserialize<T>(message.Value.Value);
                        yield return ObserverMessage.OnNext(message.Time, deserialized);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected notification kind '{0}'.", message.Value.Kind));
                }
            }
        }

        public static IEnumerable<Recorded<INotification<string>>> SerializeObserverMessages<T>(IList<Recorded<INotification<T>>> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                switch (message.Value.Kind)
                {
                    case Protocol.NotificationKind.OnCompleted:
                        yield return ObserverMessage.OnCompleted<string>(message.Time);
                        break;
                    case Protocol.NotificationKind.OnError:
                        yield return ObserverMessage.OnError<string>(message.Time, message.Value.Exception);
                        break;
                    case Protocol.NotificationKind.OnNext:
                        var serialized = Serialize<T>(message.Value.Value);
                        yield return ObserverMessage.OnNext(message.Time, serialized);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected notification kind '{0}'.", message.Value.Kind));
                }
            }
        }

        private static string Serialize<T>(T value)
        {
            if (typeof(T).FindGenericType(typeof(IAsyncReactiveQbservable<>)) != null)
            {
                var expressionProperty = (PropertyInfo)ReflectionHelpers.InfoOf((IAsyncReactiveQbservable<int> o) => o.Expression);
                var expression = (Expression)expressionProperty.GetValue(value, null);
                return new SerializationHelpers().Serialize<Expression>(expression);
            }
            else
            {
                return new SerializationHelpers().Serialize<T>(value);
            }
        }

        public static Func<TContext, ITestScheduler, Task> DoScheduling<TContext>(VirtualTimeAgenda<TContext> schedule)
            where TContext : ReactiveClientContext
        {
            return (ctx, scheduler) =>
            {
                foreach (var scheduledEvent in schedule)
                {
                    if (scheduledEvent.IsAsync)
                    {
                        scheduler.ScheduleAbsolute(scheduledEvent.Time, () => scheduledEvent.AsyncEvent(ctx));
                    }
                    else
                    {
                        scheduler.ScheduleAbsolute(scheduledEvent.Time, () => scheduledEvent.Event(ctx));
                    }
                }

                return Task.FromResult(true);
            };
        }

        public static Uri NextUri(string suffix)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, "reactor://test/remoting/{0}", string.Format(CultureInfo.InvariantCulture, "{0}/{1}", suffix, Guid.NewGuid())));
        }
    }
}
