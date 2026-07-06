// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public interface IPartitionedMultiSubject : IMultiSubject
    {
        IPartitionableSubscribable<T> CreatePartitionableSubscribable<T>();
    }
}
