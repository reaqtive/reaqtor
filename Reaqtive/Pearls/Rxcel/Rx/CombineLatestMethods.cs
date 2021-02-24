// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Reactive Excel implementation using classic Rx to demonstrate the concepts around
// building reactive computational graphs.
//

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

internal static class CombineLatestMethods
{
    public static readonly MethodInfo[] s_combineLatest = new[]
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (documents signature)
        InfoOf(() => Observable.Select(default(IObservable<double?>), default(Func<double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
        InfoOf(() => Observable.CombineLatest(default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(IObservable<double?>), default(Func<double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?, double?>))),
#pragma warning restore IDE0034 // Simplify 'default' expression
    };

    private static MethodInfo InfoOf<T>(Expression<Func<T>> f)
    {
        return ((MethodCallExpression)f.Body).Method;
    }
}
