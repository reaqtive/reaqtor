// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Hosting.Shared.Serialization;

namespace Reaqtor.Remoting.Protocol
{
    public class CommandTextFactory<TExpression> : ICommandTextFactory<TExpression>, ICommandResponseParser
    {
        private readonly SerializationHelpers _serializer;

        public CommandTextFactory()
            : this(new SerializationHelpers())
        {
        }

        public CommandTextFactory(SerializationHelpers serializer)
        {
            _serializer = serializer;
        }

        public string CreateNewText(NewCommandData<TExpression> data)
        {
            var mappedData = new DataModelNewCommandData<TExpression>
            {
                Expression = data.Expression,
                State = data.State,
                Uri = data.Uri,
            };

            return _serializer.Serialize(mappedData);
        }

        public string CreateRemoveText(Uri uri)
        {
            return _serializer.Serialize(uri);
        }

        public string CreateGetText(Expression expression)
        {
            return _serializer.Serialize(expression);
        }

        public async Task<T> ParseResponseAsync<T>(IReactiveServiceCommand command, Task<string> request, CancellationToken token)
        {
            var response = await request;
            return _serializer.Deserialize<T>(response);
        }

        public async Task ParseResponseAsync(IReactiveServiceCommand command, Task<string> request, CancellationToken token)
        {
            var response = await request;
            Debug.Assert(response == "OK");
        }
    }
}
