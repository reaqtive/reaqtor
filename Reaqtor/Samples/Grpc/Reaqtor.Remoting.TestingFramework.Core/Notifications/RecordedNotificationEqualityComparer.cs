// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;

using Nuqleon.DataModel.TypeSystem;

using Reaqtive.Testing;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework;

public class RecordedNotificationEqualityComparer<T> : IEqualityComparer<Recorded<INotification<T>>>
{
    private const int Prime = 17;
    private readonly IEqualityComparer<INotification<T>> _notificationComparer;

    public RecordedNotificationEqualityComparer(IEqualityComparer<object> valueComparer)
    {
        _notificationComparer = new NotificationEqualityComparer<T>(valueComparer);
    }

#if !NET472_OR_GREATER
#pragma warning disable CA1000 // Do not declare static members on generic types. (By design.)
#endif
    public static RecordedNotificationEqualityComparer<T> Default
    {
        get
        {
            field ??= new RecordedNotificationEqualityComparer<T>(DataTypeObjectEqualityComparer.Default);
            return field;
        }
    }
#if !NET472_OR_GREATER
#pragma warning restore CA1000
#endif

    public bool Equals(Recorded<INotification<T>> x, Recorded<INotification<T>> y)
    {
        var ignoreTime = x.Time < 0 || y.Time < 0;
        var isTimeEqual = ignoreTime || x.Time == y.Time;
        return isTimeEqual && _notificationComparer.Equals(x.Value, y.Value);
    }

    public int GetHashCode(Recorded<INotification<T>> obj)
    {
        unchecked
        {
            return obj.Time.GetHashCode() * Prime + _notificationComparer.GetHashCode(obj.Value);
        }
    }
}

internal sealed class NotificationEqualityComparer<T> : IEqualityComparer<INotification<T>>
{
    private const int Prime = 17;

    private readonly IEqualityComparer<object> _valueComparer;

    public NotificationEqualityComparer(IEqualityComparer<object> valueComparer)
    {
        _valueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
    }

    // NB: on net10.0 AnalysisMode=All flags the defensive throwing default arm below (CA1065). The throw is
    //     faithfully ported behavior over a closed NotificationKind set (OnCompleted/OnError/OnNext) and is
    //     unreachable in practice; suppressed in the same #if !NET472_OR_GREATER style this file already uses
    //     for CA1000, matching the repo's CA1065 convention on defensive comparer default arms.
#if !NET472_OR_GREATER
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations. (Defensive default arm over a closed enum set; unreachable in practice.)
#endif
    public bool Equals(INotification<T> x, INotification<T> y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.Kind != y.Kind)
        {
            return false;
        }
        if (x.HasPredicate)
        {
            return x.Equals(y);
        }
        else if (y.HasPredicate)
        {
            return y.Equals(x);
        }
        else
        {
            return x.Kind switch
            {
                Protocol.NotificationKind.OnCompleted => x.Equals(y),
                Protocol.NotificationKind.OnError => x.Equals(y),
                Protocol.NotificationKind.OnNext => x.HasValue && y.HasValue && _valueComparer.Equals(x.Value, y.Value),
                _ => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected notification kind '{0}'.", x.Kind)),
            };
        }
    }
#if !NET472_OR_GREATER
#pragma warning restore CA1065
#endif

    public int GetHashCode(INotification<T> obj)
    {
        if (obj.HasPredicate)
        {
            return EqualityComparer<INotification<T>>.Default.GetHashCode(obj);
        }
        else
        {
            var hash = obj.Exception.GetHashCode();

            unchecked
            {
                hash = hash * Prime + obj.HasValue.GetHashCode();
                hash = hash * Prime + obj.Kind.GetHashCode();
                return hash * Prime + _valueComparer.GetHashCode(obj.Value);
            }
        }
    }
}
