// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Hosting.Shared.Serialization;

namespace Reaqtor.Remoting.Protocol
{
    public class CommandTextParser<TExpression> : ICommandTextParser<TExpression>, ICommandResponseFactory
    {
        private readonly SerializationHelpers _serializer;

        public CommandTextParser()
            : this(new SerializationHelpers())
        {
        }

        public CommandTextParser(SerializationHelpers serializer)
        {
            _serializer = serializer;
        }

        public NewCommandData<TExpression> ParseNewText(string commandText)
        {
            var data = _serializer.Deserialize<DataModelNewCommandData<TExpression>>(commandText);
            return new NewCommandData<TExpression>(data.Uri, data.Expression, data.State);
        }

        public Uri ParseRemoveText(string commandText)
        {
            return _serializer.Deserialize<Uri>(commandText);
        }

        public Expression ParseGetText(string commandText)
        {
            return _serializer.Deserialize<Expression>(commandText);
        }

        public async Task<string> CreateResponseAsync<T>(IReactiveServiceCommand command, Task<T> task, CancellationToken token)
        {
            var result = await task;
            return _serializer.Serialize(result);
        }

        public async Task<string> CreateResponseAsync(IReactiveServiceCommand command, Task task, CancellationToken token)
        {
            await task;
            return "OK";
        }
    }
}
