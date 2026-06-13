// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System.Threading;

namespace System.Runtime.Remoting.Tasks
{
    /// <summary>
    /// A marshalable class used to cancel remote asynchronous operations.
    /// </summary>
    public sealed class RemoteCancellationDisposable : MarshalByRefObject, IDisposable
    {
        private ICancellationProvider _provider;
        private Guid _guid;

        /// <summary>
        /// Instantiates the disposable.
        /// </summary>
        /// <param name="provider">The cancellation provider.</param>
        /// <param name="identifier">The GUID of the operation to cancel upon disposal.</param>
        public RemoteCancellationDisposable(ICancellationProvider provider, Guid identifier)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _guid = identifier;
        }

        /// <summary>
        /// Used to indicate the lifetime of the remote service.
        /// </summary>
        /// <returns>Always returns null.</returns>
        public override object InitializeLifetimeService() => null;

        /// <summary>
        /// Dispose the remote operation using the given used to construct this instance.
        /// </summary>
        public void Dispose()
        {
            var provider = Interlocked.Exchange(ref _provider, null);
            provider?.Cancel(_guid);
        }
    }
}
