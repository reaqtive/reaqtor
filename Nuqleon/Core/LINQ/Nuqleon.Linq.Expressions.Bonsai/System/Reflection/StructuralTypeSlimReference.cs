// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Reflection
{
    /// <summary>
    /// A mutable structural type slim.
    /// </summary>
    public abstract class StructuralTypeSlimReference : StructuralTypeSlim
    {
        #region Fields

        private readonly MutableReadOnlyCollection<PropertyInfoSlim> _properties;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a mutable structural type slim.
        /// </summary>
        /// <param name="capacity">The number of properties expected.</param>
        internal StructuralTypeSlimReference(int capacity)
        {
            _properties = new MutableReadOnlyCollection<PropertyInfoSlim>(capacity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the members of the structural type.
        /// </summary>
        public override ReadOnlyCollection<PropertyInfoSlim> Properties => _properties;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a property to the structural type.
        /// </summary>
        /// <param name="property">The property to add.</param>
        public void AddProperty(PropertyInfoSlim property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (_properties.IsFrozen)
                throw new InvalidOperationException("The structural type has been frozen.  Cannot add property to frozen structural type.");

            _properties.Items.Add(property);
        }

        /// <summary>
        /// Causes the structural type to no longer be mutable.
        /// </summary>
        public void Freeze() => _properties.Freeze();

        /// <summary>
        /// Creates a new structural type that's initially mutable.
        /// </summary>
        /// <param name="hasValueEqualitySemantics">true if the structural type has value equality semantics, false otherwise.</param>
        /// <param name="kind">The kind of structural type.</param>
        /// <returns>A new structural type that's initially mutable.</returns>
        public static StructuralTypeSlimReference Create(bool hasValueEqualitySemantics, StructuralTypeSlimKind kind) => Create(hasValueEqualitySemantics, kind, capacity: 0);

        /// <summary>
        /// Creates a new structural type that's initially mutable.
        /// </summary>
        /// <param name="hasValueEqualitySemantics">true if the structural type has value equality semantics, false otherwise.</param>
        /// <param name="kind">The kind of structural type.</param>
        /// <param name="capacity">The number of properties expected.</param>
        /// <returns>A new structural type that's initially mutable.</returns>
        public static StructuralTypeSlimReference Create(bool hasValueEqualitySemantics, StructuralTypeSlimKind kind, int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            return kind switch
            {
                StructuralTypeSlimKind.Anonymous => AnonymousStructuralTypeSlimReference.Create(hasValueEqualitySemantics, capacity),
                StructuralTypeSlimKind.Record => RecordStructuralTypeSlimReference.Create(hasValueEqualitySemantics, capacity),
                _ => throw new NotSupportedException(),
            };
        }

        #endregion

        #region Types

        private sealed class MutableReadOnlyCollection<T> : ReadOnlyCollection<T>
        {
            public MutableReadOnlyCollection(int capacity)
                : base(new List<T>(capacity))
            {
            }

            public new IList<T> Items => base.Items;

            public bool IsFrozen { get; private set; }

            public void Freeze()
            {
                IsFrozen = true;
            }
        }

        private abstract class AnonymousStructuralTypeSlimReference : StructuralTypeSlimReference
        {
            public AnonymousStructuralTypeSlimReference(int capacity)
                : base(capacity)
            {
            }

            public override StructuralTypeSlimKind StructuralKind => StructuralTypeSlimKind.Anonymous;

            public static AnonymousStructuralTypeSlimReference Create(bool hasValueEqualitySemantics, int capacity)
            {
                if (hasValueEqualitySemantics)
                {
                    return new DefaultAnonymousStructuralTypeSlimReference(capacity);
                }
                else
                {
                    return new NoEqualityAnonymousStructuralTypeSlimReference(capacity);
                }
            }

            private sealed class DefaultAnonymousStructuralTypeSlimReference : AnonymousStructuralTypeSlimReference
            {
                public DefaultAnonymousStructuralTypeSlimReference(int capacity)
                    : base(capacity)
                {
                }

                public override bool HasValueEqualitySemantics => true;
            }

            private sealed class NoEqualityAnonymousStructuralTypeSlimReference : AnonymousStructuralTypeSlimReference
            {
                public NoEqualityAnonymousStructuralTypeSlimReference(int capacity)
                    : base(capacity)
                {
                }

                public override bool HasValueEqualitySemantics => false;
            }
        }

        private abstract class RecordStructuralTypeSlimReference : StructuralTypeSlimReference
        {
            public RecordStructuralTypeSlimReference(int capacity)
                : base(capacity)
            {
            }

            public override StructuralTypeSlimKind StructuralKind => StructuralTypeSlimKind.Record;

            public static RecordStructuralTypeSlimReference Create(bool hasValueEqualitySemantics, int capacity)
            {
                if (hasValueEqualitySemantics)
                {
                    return new EqualityRecordStructuralTypeSlimReference(capacity);
                }
                else
                {
                    return new DefaultRecordStructuralTypeSlimReference(capacity);
                }
            }

            private sealed class DefaultRecordStructuralTypeSlimReference : RecordStructuralTypeSlimReference
            {
                public DefaultRecordStructuralTypeSlimReference(int capacity)
                    : base(capacity)
                {
                }

                public override bool HasValueEqualitySemantics => false;
            }

            private sealed class EqualityRecordStructuralTypeSlimReference : RecordStructuralTypeSlimReference
            {
                public EqualityRecordStructuralTypeSlimReference(int capacity)
                    : base(capacity)
                {
                }

                public override bool HasValueEqualitySemantics => true;
            }
        }

        #endregion
    }
}
