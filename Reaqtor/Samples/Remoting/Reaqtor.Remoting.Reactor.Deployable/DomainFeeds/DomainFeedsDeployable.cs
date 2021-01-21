// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.Reactor.DomainFeeds
{
    using DataModels.FlightStatus;
    using DataModels.News;
    using DataModels.Traffic;
    using DataModels.Weather;
    using Observables;

    /// <summary>
    /// Deployable for domain feeds component
    /// </summary>
    public class DomainFeedsDeployable : IDeployable
    {
        /// <summary>
        /// The trace source for logging events during deployment
        /// </summary>
        private readonly TraceSource _traceSource = new("PlatformObservables");

        /// <summary>
        /// Executes the definitions of observables defined in this assembly.
        /// </summary>
        public void Execute(ReactiveClientContext context)
        {
            RegisterFlightStatusFeed(context);
            RegisterNewsFeed(context);
            RegisterTrafficFeed(context);
            RegisterWeatherAlertFeed(context);
        }

        #region Private Methods

        /// <summary>
        /// Registers the flight status feed definition
        /// </summary>
        private void RegisterFlightStatusFeed(ReactiveClientContext context)
        {
            // synthetic
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Synthetic.FlightStatus.Uri).Wait();
#endif
            DefineObservable<FlightStatusParameters, FlightStatus>(
                context,
                MetadataRegistry.Observable.Synthetic.FlightStatus.Uri,
                (flightStatusParameters) => new MockFlightStatusObservable(flightStatusParameters).AsQbservable()).Wait();

            // configure real data
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Real.FlightStatus.Uri).Wait();
#endif
            DefineObservable<FlightStatusParameters, FlightStatus>(
                context,
                MetadataRegistry.Observable.Real.FlightStatus.Uri,
                (flightStatusParameters) => new FlightStatusObservable(flightStatusParameters).AsQbservable()).Wait();
        }

        /// <summary>
        /// Registers the news feed definition
        /// </summary>
        private void RegisterNewsFeed(ReactiveClientContext context)
        {
            // synthetic
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Synthetic.NewsInfo.Uri).Wait();
#endif
            Expression<Func<NewsParameters, IAsyncReactiveQbservable<NewsInfo>>> syntheticDefinition
                = parameters => new MockNewsObservable(parameters).AsQbservable();
            DefineObservable(context, MetadataRegistry.Observable.Synthetic.NewsInfo.Uri, syntheticDefinition).Wait();

            // real data
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Real.NewsInfo.Uri).Wait();
#endif
            Expression<Func<NewsParameters, IAsyncReactiveQbservable<NewsInfo>>> realDefinition
                = (parameters) => new NewsObservable(parameters).AsQbservable();
            DefineObservable(context, MetadataRegistry.Observable.Real.NewsInfo.Uri, realDefinition).Wait();
        }

        /// <summary>
        /// Registers the traffic feed definition
        /// </summary>
        private void RegisterTrafficFeed(ReactiveClientContext context)
        {
            // synthetic
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Synthetic.TrafficInfo.Uri).Wait();
#endif
            Expression<Func<TrafficParameters, IAsyncReactiveQbservable<TrafficInfo>>> syntheticDefinition
                = parameters => new SyntheticTrafficObservable(parameters).AsQbservable();
            DefineObservable(context, MetadataRegistry.Observable.Synthetic.TrafficInfo.Uri, syntheticDefinition).Wait();

            // configurable real data
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Real.TrafficInfo.ConfigurableUri).Wait();
#endif
            Expression<Func<TrafficParameters, TrafficConfiguration, IAsyncReactiveQbservable<TrafficInfo>>> configurableRealDefinition
                = (parameters, configuration) => new TrafficObservable(parameters, configuration).AsQbservable();
            DefineObservable(context, MetadataRegistry.Observable.Real.TrafficInfo.ConfigurableUri, configurableRealDefinition).Wait();

            // default configuration real data by defining it in terms of the configurable one
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Real.TrafficInfo.Uri).Wait();
#endif
            var configurableDef =
                context.GetObservable<TrafficParameters, TrafficConfiguration, TrafficInfo>(
                    MetadataRegistry.Observable.Real.TrafficInfo.ConfigurableUri);
            Expression<Func<TrafficParameters, IAsyncReactiveQbservable<TrafficInfo>>> defaultRealDefinition =
                parameters => configurableDef(parameters, new TrafficConfiguration());
            DefineObservable(context, MetadataRegistry.Observable.Real.TrafficInfo.Uri, defaultRealDefinition).Wait();
        }

        /// <summary>
        /// Registers the weather alert feed definition
        /// </summary>
        private void RegisterWeatherAlertFeed(ReactiveClientContext context)
        {
            // synthetic
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Synthetic.WeatherAlert.Uri).Wait();
#endif
            DefineObservable<WeatherAlertParameters, WeatherAlert>(
                context,
                MetadataRegistry.Observable.Synthetic.WeatherAlert.Uri,
                (weatherAlertParameters) => new MockWeatherAlertObservable(weatherAlertParameters).AsQbservable()).Wait();

            // configure real data
#if DO_UNDEFINE
            UndefineObservable(context, MetadataRegistry.Observable.Real.WeatherAlert.Uri).Wait();
#endif
            DefineObservable<WeatherAlertParameters, WeatherAlert>(
                context,
                MetadataRegistry.Observable.Real.WeatherAlert.Uri,
                (weatherAlertParameters) => new WeatherAlertObservable(weatherAlertParameters).AsQbservable()).Wait();
        }

#if DO_UNDEFINE
        /// <summary>
        /// Undefines any previous definition of the observable
        /// </summary>
        /// <param name="id">the Id of the observable definition</param>
        private async Task UndefineObservable(ReactiveClientContext context, Uri id)
        {
            try
            {
                await context.UndefineObservableAsync(id, CancellationToken.None);
                _traceSource.TraceInformation(string.Format("Successfully Undefined observable ({0})", id.OriginalString));
            }
            catch (Exception)
            {
                _traceSource.TraceInformation(string.Format("There is no existing definition for observable ({0}) ", id.OriginalString));
            }
        }
#endif

        /// <summary>
        /// Registers the definition of a two parameters observable
        /// </summary>
        /// <typeparam name="TParam">publicly visible observable parameter</typeparam>
        /// <typeparam name="TConfig">configuration parameter for the observable</typeparam>
        /// <typeparam name="TResult">The type of notifications that the observable is going to publish</typeparam>
        /// <param name="id">the Id of the observable</param>
        /// <param name="definition">the expression that defines the observable</param>
        /// <returns>an async task</returns>
        private async Task DefineObservable<TParam, TConfig, TResult>(ReactiveClientContext context, Uri id, Expression<Func<TParam, TConfig, IAsyncReactiveQbservable<TResult>>> definition)
        {
            _traceSource.TraceInformation("Registering definition for observable ({0})", id.OriginalString);
            await context.DefineObservableAsync(id, definition, null, CancellationToken.None);
            _traceSource.TraceInformation("Sucessfully Defined observable ({0})", id.OriginalString);
        }

        /// <summary>
        /// Registers the definition of a one parameter observable
        /// </summary>
        /// <typeparam name="TParam">publicly visible observable parameter</typeparam>
        /// <typeparam name="TResult">The type of notifications that the observable is going to publish</typeparam>
        /// <param name="id">the Id of the observable</param>
        /// <param name="definition">the expression that defines the observable</param>
        /// <returns>an async task</returns>
        private async Task DefineObservable<TParam, TResult>(ReactiveClientContext context, Uri id, Expression<Func<TParam, IAsyncReactiveQbservable<TResult>>> definition)
        {
            _traceSource.TraceInformation("Registering definition for observable ({0})", id.OriginalString);
            await context.DefineObservableAsync(id, definition, null, CancellationToken.None);
            _traceSource.TraceInformation("Sucessfully registered definition for observable ({0})", id.OriginalString);
        }

        #endregion
    }
}
