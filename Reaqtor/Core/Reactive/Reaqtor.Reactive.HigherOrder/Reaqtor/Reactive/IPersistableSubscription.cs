// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;

namespace Reaqtor.Reactive
{
    public partial class HigherOrderExecutionEnvironment
    {
        private interface IPersistable
        {
            void Save(IOperatorStateWriter writer);
            void Load(IOperatorStateReader reader);
        }

        private interface IPersistableSubscription : ISubscription, IPersistable
        {
        }
    }
}
