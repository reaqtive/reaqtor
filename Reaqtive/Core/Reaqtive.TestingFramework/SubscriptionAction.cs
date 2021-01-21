// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.TestingFramework
{
    public enum SubscriptionActionKind
    {
        LoadState,
        SaveState,
        Crash,
        Recover,
    }

    public abstract class SubscriptionAction : IEquatable<SubscriptionAction>
    {
        public SubscriptionActionKind Type { get; protected set; }

        public override bool Equals(object obj) => obj is SubscriptionAction a && Equals(a);

        public override int GetHashCode() => (int)Type;

        public bool Equals(SubscriptionAction other)
        {
            if (other == null)
            {
                return false;
            }

            return Type == other.Type;
        }

        public virtual void Accept(ISubscription subscription)
        {
        }
    }

    internal class LoadState : SubscriptionAction
    {
        private readonly IOperatorStateContainer _state;

        public LoadState(IOperatorStateContainer state)
        {
            Type = SubscriptionActionKind.LoadState;
            _state = state;
        }

        public override void Accept(ISubscription subscription)
        {
            var visitor = new SubscriptionStateVisitor(subscription);
            visitor.LoadState(_state.CreateReader());
        }
    }

    internal class SaveState : SubscriptionAction
    {
        private readonly IOperatorStateContainer _state;

        public SaveState(IOperatorStateContainer state)
        {
            Type = SubscriptionActionKind.SaveState;
            _state = state;
        }

        public override void Accept(ISubscription subscription)
        {
            var visitor = new SubscriptionStateVisitor(subscription);
            visitor.SaveState(_state.CreateWriter());
        }
    }

    internal class Crash : SubscriptionAction
    {
        public Crash()
        {
            Type = SubscriptionActionKind.Crash;
        }
    }

    internal class Recover : SubscriptionAction
    {
        public Recover()
        {
            Type = SubscriptionActionKind.Recover;
        }
    }
}
