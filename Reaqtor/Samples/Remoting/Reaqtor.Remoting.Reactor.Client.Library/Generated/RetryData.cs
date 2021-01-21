// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// The set of supported retry variations.
    /// </summary>
    public enum Variant
    {
        /// <summary>Do not retry</summary>
        None,

        /// <summary>Retry with a fixed interval between retries.</summary>
        Fixed,

        /// <summary>Retry with an exponential back off between retries.</summary>
        Exponential,

        /// <summary>Retry with a progressive back off between retries.</summary>
        Progressive
    }

    /// <summary>
    /// Serializable format of Retry delegate data. We cannot simply embed the 
    /// delegate into the subscription expression because that would require
    /// installing the client's assembly into the service. Instead, we have the
    /// client create instances of this RetryData type which can be serialized.
    /// </summary>
    [KnownType]
    public class RetryData
    {
        /// <summary>
        /// Gets or sets the retry delegate variant.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/Variant")]
        public int Variant
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry delegate's retry count.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/RetryCount")]
        public int RetryCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry delegate's retry interval.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/RetryInterval")]
        public TimeSpan RetryInterval
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry initial interval.
        /// </summary>
        /// <value>
        /// The initial interval.
        /// </value>
        [Mapping("reactor://platform.bing.com/entities/RetryData/InitialInterval")]
        public TimeSpan InitialInterval
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry increment.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/Increment")]
        public TimeSpan Increment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry delegate minBackOff.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/MinBackOff")]
        public TimeSpan MinBackOff
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry delegate maxBackOff.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/MaxBackOff")]
        public TimeSpan MaxBackOff
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry delegate deltaBackOff.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/RetryData/DeltaBackOff")]
        public TimeSpan DeltaBackOff
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a RetryData instance that represents the exponential Retry policy.
        /// </summary>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="minBackOff">The min back off.</param>
        /// <param name="maxBackOff">The max back off.</param>
        /// <param name="deltaBackOff">The delta back off.</param>
        /// <returns>The appropriate RetryData instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>retryCount</c> The retry count must be non-negative
        /// or
        /// <c>minBackOff</c> The minimum back-off between retries must be non-negative
        /// or
        /// <c>maxBackOff</c> The maximum back-off between retries must be non-negative
        /// or
        /// <c>deltaBackOff</c> The delta back-off between retries must be non-negative
        /// </exception>
        public static RetryData Exponential(
            int retryCount, TimeSpan minBackOff, TimeSpan maxBackOff, TimeSpan deltaBackOff)
        {
            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "The retry count must be non-negative");
            }

            if (minBackOff < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minBackOff), minBackOff, "The minimum back-off between retries must be non-negative");
            }

            if (maxBackOff < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxBackOff), maxBackOff, "The maximum back-off between retries must be non-negative");
            }

            if (deltaBackOff < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(deltaBackOff), deltaBackOff, "The delta back-off between retries must be non-negative");
            }

            return new RetryData
            {
                Variant = 3,
                RetryCount = retryCount,
                MinBackOff = minBackOff,
                MaxBackOff = maxBackOff,
                DeltaBackOff = deltaBackOff
            };
        }

        /// <summary>
        /// Gets a RetryData instance that represents the fixed Retry policy.
        /// </summary>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="retryInterval">The interval between retries.</param>
        /// <returns>The appropriate RetryData instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>retryCount</c> The retry count must be non-negative
        /// or
        /// <c>intervalBetweenRetries</c> The interval between retries must be non-negative
        /// </exception>
        public static RetryData Fixed(int retryCount, TimeSpan retryInterval)
        {
            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(retryCount), retryCount, "The retry count must be non-negative");
            }

            if (retryInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(retryInterval), retryInterval, "The retry interval must be non-negative");
            }

            return new RetryData
            {
                Variant = 1,
                RetryCount = retryCount,
                RetryInterval = retryInterval
            };
        }

        /// <summary>
        /// Gets a RetryData instance that represents the NoRetry policy.
        /// </summary>
        /// <returns>The appropriate RetryData instance.</returns>
        public static RetryData NoRetry()
        {
            return new RetryData { Variant = 0 };
        }

        /// <summary>
        /// Gets a RetryData instance that represents the fixed Retry policy.
        /// </summary>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="initialInterval">The initial interval.</param>
        /// <param name="increment">The increment.</param>
        /// <returns>
        /// The appropriate RetryData instance.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><c>retryCount</c> The retry count must be non-negative
        /// or
        /// <c>intervalBetweenRetries</c> The interval between retries must be non-negative</exception>
        public static RetryData Progressive(int retryCount, TimeSpan initialInterval, TimeSpan increment)
        {
            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(retryCount), retryCount, "The retry count must be non-negative");
            }

            if (initialInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(initialInterval), initialInterval, "The retry initial interval must be non-negative");
            }

            if (increment < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(increment), increment, "The retry increment must be non-negative");
            }

            return new RetryData
            {
                Variant = 2,
                RetryCount = retryCount,
                InitialInterval = initialInterval,
                Increment = increment,
            };
        }
    }
}
