// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a tree with data of the specified type stored in the tree nodes.
    /// </summary>
    /// <typeparam name="T">Type of the data contained in the tree nodes.</typeparam>
    public class Tree<T> : ITree<T>
    {
        /// <summary>
        /// Creates a new tree node with the specified stored data and without any children.
        /// </summary>
        /// <param name="value">Data to store in the node.</param>
        public Tree(T value)
        {
            Value = value;
            Children = EmptyReadOnlyCollection<ITree<T>>.Instance;
        }

        /// <summary>
        /// Creates a new tree node with the specified stored data and with the specified children.
        /// </summary>
        /// <param name="value">Data stored in the node.</param>
        /// <param name="children">Child nodes.</param>
        public Tree(T value, IEnumerable<ITree<T>> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Value = value;
            Children = children.ToReadOnly();
        }

        /// <summary>
        /// Creates a new tree node with the specified stored data and with the specified children.
        /// </summary>
        /// <param name="value">Data stored in the node.</param>
        /// <param name="children">Child nodes.</param>
        public Tree(T value, params ITree<T>[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Value = value;
            Children = children.ToReadOnly();
        }

        /// <summary>
        /// Gets the data stored in the node.
        /// </summary>
        public virtual T Value { get; }

        /// <summary>
        /// Gets the children of the current tree node.
        /// </summary>
        public virtual IReadOnlyList<ITree<T>> Children { get; }

        /// <summary>
        /// Gets the data stored in the node.
        /// </summary>
        object ITree.Value => Value;

        /// <summary>
        /// Gets the children of the current tree node.
        /// </summary>
        IReadOnlyList<ITree> ITree.Children => Children;

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        public virtual string ToStringFormat()
        {
            var n = Children.Count;
            var v = Value == null ? "<null>" : Value.ToString().EscapeFormatString();
            return v + "(" + string.Join(", ", Enumerable.Range(0, n).Select(i => "{" + i + "}")) + ")";
        }

        /// <summary>
        /// Gets a string representation of the current node and its children.
        /// </summary>
        /// <returns>String representation of the current node.</returns>
        public override string ToString()
        {
            var fmt = ToStringFormat();
            return string.Format(CultureInfo.InvariantCulture, fmt, Children.Select(c => c != null ? c.ToString() : "<null>").ToArray());
        }

        /// <summary>
        /// Gets a string representation of the current node and its children, using the specified indentation level.
        /// </summary>
        /// <param name="indent">Indentation level. Recursive calls to obtain a string representation of child nodes will use an incremented indentation level.</param>
        /// <returns>String representation of the current node.</returns>
        public virtual string ToString(int indent)
        {
            if (indent < 0)
                throw new ArgumentOutOfRangeException(nameof(indent));

            var n = Children.Count;
            var ind = new string(' ', indent * 2);
            var fmt = ToStringFormat();
            return ind + string.Format(CultureInfo.InvariantCulture, fmt, Children.Select((c, j) => "\r\n" + (c != null ? c.ToString(indent + 1) : new string(' ', (indent + 1) * 2) + "<null>") + (j == n - 1 ? "\r\n" + ind : "")).ToArray());
        }

        /// <summary>
        /// Updates the tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public ITree<T> Update(IEnumerable<ITree<T>> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return UpdateImpl(children);
        }

        /// <summary>
        /// Updates the tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public ITree<T> Update(params ITree<T>[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return UpdateImpl(children);
        }

        /// <summary>
        /// Updates the tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        ITree ITree.Update(IEnumerable<ITree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return UpdateImpl(children.Cast<ITree<T>>());
        }

        /// <summary>
        /// Updates the tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        protected virtual ITree<T> UpdateCore(IEnumerable<ITree<T>> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return new Tree<T>(Value, children);
        }

        private ITree<T> UpdateImpl(IEnumerable<ITree<T>> children)
        {
            if (Children.SequenceEqual(children))
            {
                return this;
            }

            return UpdateCore(children);
        }

        /// <summary>
        /// Dispatches the current tree instance to the tree visitor's Visit method.
        /// </summary>
        /// <param name="visitor">Visitor to accept the tree.</param>
        /// <returns>Result of the visit.</returns>
        public ITree<T> Accept(ITreeVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            return visitor.Visit(this);
        }

        /// <summary>
        /// Dispatches the current tree instance to the tree visitor's Visit method.
        /// </summary>
        /// <param name="visitor">Visitor to accept the tree.</param>
        /// <returns>Result of the visit.</returns>
        ITree ITree.Accept(ITreeVisitor visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            return visitor.Visit(this);
        }
    }
}
