// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Base class for subscription visitors performing actions on each operator node in a subscription tree.
    /// </summary>
    public class SubscriptionVisitor : ISubscriptionVisitor
    {
        /// <summary>
        /// Creates a new subscription visitor that applies the specified action to each operator node of the specified type in a subscription tree.
        /// </summary>
        /// <typeparam name="T">Type of the operators processed by the specified action. This type should implement IOperator.</typeparam>
        /// <param name="visit">Action applied to each operator node of the specified type.</param>
        /// <returns>Subscription visitor that will apply the specified action to each matching operator node.</returns>
        public static SubscriptionVisitorBuilder<T> Do<T>(Action<T> visit)
            where T : class, IOperator
        {
            return new SubscriptionVisitorBuilder<T>(visit);
        }

        /// <summary>
        /// Visits the specified operator node.
        /// </summary>
        /// <param name="node">Node to apply the visitor to.</param>
        public virtual void Visit(IOperator node)
        {
            if (node == null)
            {
                return;
            }

            VisitCore(node);
            VisitChildren(node);
        }

        /// <summary>
        /// Core implementation of the visitor functionality.
        /// </summary>
        /// <param name="node">Node to apply the visitor to.</param>
        protected virtual void VisitCore(IOperator node)
        {
        }

        /// <summary>
        /// Visits the child nodes of the current operator.
        /// </summary>
        /// <param name="node">Node whose child nodes to visit.</param>
        private void VisitChildren(IOperator node)
        {
            if (node is IUnaryOperator unaryNode)
            {
                unaryNode.Input.Accept(this);
            }
            else if (node.Inputs is ISubscription[] inputs)
            {
                var len = inputs.Length;
                for (var i = 0; i < len; ++i)
                {
                    inputs[i].Accept(this);
                }
            }
            else
            {
                foreach (var child in node.Inputs)
                {
                    child.Accept(this);
                }
            }
        }
    }

    /// <summary>
    /// Subscription visitor performing an action on each operator node in a subscription tree.
    /// </summary>
    /// <typeparam name="T1">Type of the operators processed by the visitor's action. This type should implement IOperator.</typeparam>
    public class SubscriptionVisitor<T1> : SubscriptionVisitor
        where T1 : class, IOperator
    {
        private readonly Action<T1> _visitor;

        /// <summary>
        /// Creates a new subscription visitor with the specified action to visit operator nodes.
        /// </summary>
        /// <param name="visitor">Action applied to each operator node of type <typeparamref name="T1"/>.</param>
        public SubscriptionVisitor(Action<T1> visitor)
        {
            _visitor = visitor;
        }

        /// <summary>
        /// Core implementation of the visitor functionality applied to operators of type <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="node">Node to apply the visitor to.</param>
        protected override void VisitCore(IOperator node)
        {
            if (node is T1 t)
            {
                _visitor(t);
            }
        }
    }

    /// <summary>
    /// Subscription visitor performing actions on each operator node in a subscription tree.
    /// </summary>
    /// <typeparam name="T1">Type of the operators processed by the visitor's first action. This type should implement IOperator.</typeparam>
    /// <typeparam name="T2">Type of the operators processed by the visitor's second action. This type should implement IOperator.</typeparam>
    public class SubscriptionVisitor<T1, T2> : SubscriptionVisitor<T1>
        where T1 : class, IOperator
        where T2 : class, IOperator
    {
        private readonly Action<T2> _visitor;

        /// <summary>
        /// Creates a new subscription visitor with the specified actions to visit operator nodes.
        /// </summary>
        /// <param name="visitor1">Action applied to each operator node of type <typeparamref name="T1"/>.</param>
        /// <param name="visitor2">Action applied to each operator node of type <typeparamref name="T2"/>.</param>
        public SubscriptionVisitor(Action<T1> visitor1, Action<T2> visitor2)
            : base(visitor1)
        {
            _visitor = visitor2;
        }

        /// <summary>
        /// Core implementation of the visitor functionality applied to operators of type <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="node">Node to apply the visitor to.</param>
        protected override void VisitCore(IOperator node)
        {
            base.VisitCore(node);

            if (node is T2 t)
            {
                _visitor(t);
            }
        }
    }
}
