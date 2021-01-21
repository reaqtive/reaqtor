// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Memory;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a collection of keys and values.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public partial class PooledDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly DictionaryPool<TKey, TValue> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledDictionary(DictionaryPool<TKey, TValue> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledDictionary(DictionaryPool<TKey, TValue> pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledDictionary(DictionaryPool<TKey, TValue> pool, System.Collections.Generic.IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledDictionary(DictionaryPool<TKey, TValue> pool, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
            _pool = pool;
        }

        internal PooledDictionary(DictionaryPool<TKey, TValue> pool, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity)
            : base(capacity, comparer)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledDictionary<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PooledDictionary{TKey, TValue}"/> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="PooledDictionary{TKey, TValue}"/>.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="PooledDictionary{TKey, TValue}"/>.</param>
        protected PooledDictionary(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
#endif

    /// <summary>
    /// Object pool for <see cref="Dictionary{TKey, TValue}"/> instances.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public abstract partial class DictionaryPool<TKey, TValue> : ObjectPoolBase<PooledDictionary<TKey, TValue>>
    {
        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="Dictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected DictionaryPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Dictionary{TKey, TValue}"/> by calling the default constructor on Dictionary{TKey, TValue}.
        /// </summary>
        /// <param name="size">Number of <see cref="Dictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="Dictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static DictionaryPool<TKey, TValue> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : DictionaryPool<TKey, TValue>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledDictionary<TKey, TValue> CreateInstance() => new PooledDictionary<TKey, TValue>(this);
        }

        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Dictionary{TKey, TValue}"/> by calling the constructor on <see cref="Dictionary{TKey, TValue}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Dictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        /// <returns>Newly created pool for <see cref="Dictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static DictionaryPool<TKey, TValue> Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : DictionaryPool<TKey, TValue>
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledDictionary<TKey, TValue> CreateInstance() => new PooledDictionary<TKey, TValue>(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Dictionary{TKey, TValue}"/> by calling the constructor on <see cref="Dictionary{TKey, TValue}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Dictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <returns>Newly created pool for <see cref="Dictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static DictionaryPool<TKey, TValue> Create(int size, System.Collections.Generic.IEqualityComparer<TKey> comparer)
        {
            CheckArguments(comparer);

            return new Impl2(size, comparer);
        }

        static partial void CheckArguments(System.Collections.Generic.IEqualityComparer<TKey> comparer);

        private sealed class Impl2 : DictionaryPool<TKey, TValue>
        {
            private readonly System.Collections.Generic.IEqualityComparer<TKey> _comparer;

            public Impl2(int size, System.Collections.Generic.IEqualityComparer<TKey> comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledDictionary<TKey, TValue> CreateInstance() => new PooledDictionary<TKey, TValue>(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Dictionary{TKey, TValue}"/> by calling the constructor on <see cref="Dictionary{TKey, TValue}"/>, passing in the specified <paramref name="capacity">capacity</paramref> and <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Dictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <returns>Newly created pool for <see cref="Dictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static DictionaryPool<TKey, TValue> Create(int size, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer)
        {
            CheckArguments(capacity, comparer);

            return new Impl3(size, capacity, comparer);
        }

        static partial void CheckArguments(int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer);

        private sealed class Impl3 : DictionaryPool<TKey, TValue>
        {
            private readonly int _capacity;
            private readonly System.Collections.Generic.IEqualityComparer<TKey> _comparer;

            public Impl3(int size, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer)
                : base(size)
            {
                _capacity = capacity;
                _comparer = comparer;
            }

            protected override PooledDictionary<TKey, TValue> CreateInstance() => new PooledDictionary<TKey, TValue>(this, _capacity, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="Dictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Dictionary{TKey, TValue}"/> by calling the constructor on <see cref="Dictionary{TKey, TValue}"/>, passing in the specified <paramref name="capacity">capacity</paramref> and <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Dictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="Dictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static DictionaryPool<TKey, TValue> Create(int size, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity)
        {
            CheckArguments(capacity, comparer, maxCapacity);

            return new Impl4(size, capacity, comparer, maxCapacity);
        }

        static partial void CheckArguments(int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity);

        private sealed class Impl4 : DictionaryPool<TKey, TValue>
        {
            private readonly int _capacity;
            private readonly System.Collections.Generic.IEqualityComparer<TKey> _comparer;
            private readonly int _maxCapacity;

            public Impl4(int size, int capacity, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _comparer = comparer;
                _maxCapacity = maxCapacity;
            }

            protected override PooledDictionary<TKey, TValue> CreateInstance() => new PooledDictionary<TKey, TValue>(this, _capacity, _comparer, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled dictionary instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled dictionary instance.</returns>
        public new PooledDictionaryHolder<TKey, TValue> New()
        {
            var res = new PooledObject<PooledDictionary<TKey, TValue>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledDictionaryHolder<TKey, TValue>(res);
        }

        partial void OnAllocate(PooledObject<PooledDictionary<TKey, TValue>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="Dictionary{TKey, TValue}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="Dictionary{TKey, TValue}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledDictionaryHolder<TKey, TValue> : IDisposable
    {
        private readonly PooledObject<PooledDictionary<TKey, TValue>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled dictionary object.
        /// </summary>
        /// <param name="obj">Pooled dictionary object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledDictionaryHolder(PooledObject<PooledDictionary<TKey, TValue>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="Dictionary{TKey, TValue}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="Dictionary{TKey, TValue}"/> instance held by the holder.</returns>
        public Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="Dictionary{TKey, TValue}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledDictionaryHolder<TKey, TValue>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a set of values.To browse the .NET Framework source code for this type, see the Reference Source.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public partial class PooledHashSet<T> : HashSet<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly HashSetPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledHashSet(HashSetPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledHashSet(HashSetPool<T> pool, System.Collections.Generic.IEqualityComparer<T> comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledHashSet(HashSetPool<T> pool, System.Collections.Generic.IEqualityComparer<T> comparer, int maxCapacity)
            : base(comparer)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledHashSet<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PooledHashSet{T}"/> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="PooledHashSet{T}"/>.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="PooledHashSet{T}"/>.</param>
        protected PooledHashSet(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
#endif

    /// <summary>
    /// Object pool for <see cref="HashSet{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public abstract partial class HashSetPool<T> : ObjectPoolBase<PooledHashSet<T>>
    {
        /// <summary>
        /// Creates a new <see cref="HashSet{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="HashSet{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected HashSetPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="HashSet{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="HashSet{T}"/> by calling the default constructor on HashSet{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="HashSet{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="HashSet{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static HashSetPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : HashSetPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledHashSet<T> CreateInstance() => new PooledHashSet<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="HashSet{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="HashSet{T}"/> by calling the constructor on <see cref="HashSet{T}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="HashSet{T}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing values in the set, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> implementation for the set type.</param>
        /// <returns>Newly created pool for <see cref="HashSet{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static HashSetPool<T> Create(int size, System.Collections.Generic.IEqualityComparer<T> comparer)
        {
            CheckArguments(comparer);

            return new Impl1(size, comparer);
        }

        static partial void CheckArguments(System.Collections.Generic.IEqualityComparer<T> comparer);

        private sealed class Impl1 : HashSetPool<T>
        {
            private readonly System.Collections.Generic.IEqualityComparer<T> _comparer;

            public Impl1(int size, System.Collections.Generic.IEqualityComparer<T> comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledHashSet<T> CreateInstance() => new PooledHashSet<T>(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="HashSet{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="HashSet{T}"/> by calling the constructor on <see cref="HashSet{T}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="HashSet{T}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing values in the set, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> implementation for the set type.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="HashSet{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static HashSetPool<T> Create(int size, System.Collections.Generic.IEqualityComparer<T> comparer, int maxCapacity)
        {
            CheckArguments(comparer, maxCapacity);

            return new Impl2(size, comparer, maxCapacity);
        }

        static partial void CheckArguments(System.Collections.Generic.IEqualityComparer<T> comparer, int maxCapacity);

        private sealed class Impl2 : HashSetPool<T>
        {
            private readonly System.Collections.Generic.IEqualityComparer<T> _comparer;
            private readonly int _maxCapacity;

            public Impl2(int size, System.Collections.Generic.IEqualityComparer<T> comparer, int maxCapacity)
                : base(size)
            {
                _comparer = comparer;
                _maxCapacity = maxCapacity;
            }

            protected override PooledHashSet<T> CreateInstance() => new PooledHashSet<T>(this, _comparer, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled hash set instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled hash set instance.</returns>
        public new PooledHashSetHolder<T> New()
        {
            var res = new PooledObject<PooledHashSet<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledHashSetHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledHashSet<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="HashSet{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="HashSet{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledHashSetHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledHashSet<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled hash set object.
        /// </summary>
        /// <param name="obj">Pooled hash set object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledHashSetHolder(PooledObject<PooledHashSet<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="HashSet{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="HashSet{T}"/> instance held by the holder.</returns>
        public HashSet<T> HashSet
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="HashSet{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledHashSetHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public partial class PooledList<T> : List<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly ListPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledList(ListPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledList(ListPool<T> pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledList(ListPool<T> pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledList<T>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="List{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public abstract partial class ListPool<T> : ObjectPoolBase<PooledList<T>>
    {
        /// <summary>
        /// Creates a new <see cref="List{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="List{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ListPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="List{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="List{T}"/> by calling the default constructor on List{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="List{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="List{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ListPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : ListPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledList<T> CreateInstance() => new PooledList<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="List{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="List{T}"/> by calling the constructor on <see cref="List{T}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="List{T}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <returns>Newly created pool for <see cref="List{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ListPool<T> Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : ListPool<T>
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledList<T> CreateInstance() => new PooledList<T>(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="List{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="List{T}"/> by calling the constructor on <see cref="List{T}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="List{T}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="List{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ListPool<T> Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : ListPool<T>
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledList<T> CreateInstance() => new PooledList<T>(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled list instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled list instance.</returns>
        public new PooledListHolder<T> New()
        {
            var res = new PooledObject<PooledList<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledListHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledList<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="List{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="List{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledListHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledList<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled list object.
        /// </summary>
        /// <param name="obj">Pooled list object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledListHolder(PooledObject<PooledList<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="List{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> instance held by the holder.</returns>
        public List<T> List
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="List{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledListHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    public partial class PooledQueue<T> : Queue<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly QueuePool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledQueue(QueuePool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledQueue(QueuePool<T> pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledQueue(QueuePool<T> pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledQueue<T>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="Queue{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    public abstract partial class QueuePool<T> : ObjectPoolBase<PooledQueue<T>>
    {
        /// <summary>
        /// Creates a new <see cref="Queue{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected QueuePool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Queue{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Queue{T}"/> by calling the default constructor on Queue{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="Queue{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static QueuePool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : QueuePool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledQueue<T> CreateInstance() => new PooledQueue<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="Queue{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Queue{T}"/> by calling the constructor on <see cref="Queue{T}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue{T}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Queue`1" /> can contain.</param>
        /// <returns>Newly created pool for <see cref="Queue{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static QueuePool<T> Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : QueuePool<T>
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledQueue<T> CreateInstance() => new PooledQueue<T>(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="Queue{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Queue{T}"/> by calling the constructor on <see cref="Queue{T}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue{T}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Queue`1" /> can contain.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="Queue{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static QueuePool<T> Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : QueuePool<T>
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledQueue<T> CreateInstance() => new PooledQueue<T>(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled queue instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled queue instance.</returns>
        public new PooledQueueHolder<T> New()
        {
            var res = new PooledObject<PooledQueue<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledQueueHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledQueue<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="Queue{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="Queue{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledQueueHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledQueue<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled queue object.
        /// </summary>
        /// <param name="obj">Pooled queue object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledQueueHolder(PooledObject<PooledQueue<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="Queue{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="Queue{T}"/> instance held by the holder.</returns>
        public Queue<T> Queue
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="Queue{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledQueueHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a variable size last-in-first-out (LIFO) collection of instances of the same specified type.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    public partial class PooledStack<T> : Stack<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly StackPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledStack(StackPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledStack(StackPool<T> pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledStack(StackPool<T> pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledStack<T>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="Stack{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    public abstract partial class StackPool<T> : ObjectPoolBase<PooledStack<T>>
    {
        /// <summary>
        /// Creates a new <see cref="Stack{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected StackPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Stack{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Stack{T}"/> by calling the default constructor on Stack{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="Stack{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StackPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : StackPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledStack<T> CreateInstance() => new PooledStack<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="Stack{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Stack{T}"/> by calling the constructor on <see cref="Stack{T}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack{T}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Stack`1" /> can contain.</param>
        /// <returns>Newly created pool for <see cref="Stack{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StackPool<T> Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : StackPool<T>
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledStack<T> CreateInstance() => new PooledStack<T>(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="Stack{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Stack{T}"/> by calling the constructor on <see cref="Stack{T}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack{T}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Stack`1" /> can contain.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="Stack{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StackPool<T> Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : StackPool<T>
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledStack<T> CreateInstance() => new PooledStack<T>(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled stack instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled stack instance.</returns>
        public new PooledStackHolder<T> New()
        {
            var res = new PooledObject<PooledStack<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledStackHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledStack<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="Stack{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="Stack{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledStackHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledStack<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled stack object.
        /// </summary>
        /// <param name="obj">Pooled stack object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledStackHolder(PooledObject<PooledStack<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="Stack{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="Stack{T}"/> instance held by the holder.</returns>
        public Stack<T> Stack
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="Stack{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledStackHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}

namespace System.IO
{
    /// <summary>
    /// Creates a stream whose backing store is memory.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledMemoryStream : MemoryStream, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly MemoryStreamPool _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledMemoryStream(MemoryStreamPool pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledMemoryStream(MemoryStreamPool pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledMemoryStream(MemoryStreamPool pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (!CanWrite || Capacity > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

        /// <summary>
        /// Clears the object.
        /// </summary>
        public void Clear()
        {
            ClearCore();
        }

        partial void ClearCore();
    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledMemoryStream
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="MemoryStream"/> instances.
    /// </summary>
    public abstract partial class MemoryStreamPool : ObjectPoolBase<PooledMemoryStream>
    {
        /// <summary>
        /// Creates a new <see cref="MemoryStream"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="MemoryStream"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected MemoryStreamPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="MemoryStream"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="MemoryStream"/> by calling the default constructor on MemoryStream.
        /// </summary>
        /// <param name="size">Number of <see cref="MemoryStream"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="MemoryStream"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static MemoryStreamPool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : MemoryStreamPool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledMemoryStream CreateInstance() => new PooledMemoryStream(this);
        }

        /// <summary>
        /// Creates a new <see cref="MemoryStream"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="MemoryStream"/> by calling the constructor on <see cref="MemoryStream"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="MemoryStream"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial size of the internal array in bytes.</param>
        /// <returns>Newly created pool for <see cref="MemoryStream"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static MemoryStreamPool Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : MemoryStreamPool
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledMemoryStream CreateInstance() => new PooledMemoryStream(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="MemoryStream"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="MemoryStream"/> by calling the constructor on <see cref="MemoryStream"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="MemoryStream"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial size of the internal array in bytes.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="MemoryStream"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static MemoryStreamPool Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : MemoryStreamPool
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledMemoryStream CreateInstance() => new PooledMemoryStream(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled memory stream instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled memory stream instance.</returns>
        public new PooledMemoryStreamHolder New()
        {
            var res = new PooledObject<PooledMemoryStream>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Length == 0);

            System.Diagnostics.Debug.Assert(res.Object.Length == 0, "A dirty object was returned from the pool.");

            return new PooledMemoryStreamHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledMemoryStream> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="MemoryStream"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="MemoryStream"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledMemoryStreamHolder : IDisposable
    {
        private readonly PooledObject<PooledMemoryStream> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled memory stream object.
        /// </summary>
        /// <param name="obj">Pooled memory stream object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledMemoryStreamHolder(PooledObject<PooledMemoryStream> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="MemoryStream"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="MemoryStream"/> instance held by the holder.</returns>
        public MemoryStream MemoryStream
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="MemoryStream"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledMemoryStreamHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}

namespace System.Text
{
    /// <summary>
    /// Represents a mutable string of characters. This class cannot be inherited.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledStringBuilder : IFreeable, IClearable
    {
        private readonly StringBuilderPool _pool;
        private readonly int _maxCapacity = 1024;

        /// <summary>
        /// Gets the <see cref="StringBuilder"/> instance held by this pooled string builder instance.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "StringBuilder is sealed, so we can't inherit from it to provide straight accesses. We want to get as close as direct invocations of the pooled object's members without extra layers of abstractions on the code path.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "No amount of 'security' will protect against aliasing of mutable object instances.")]
        public readonly StringBuilder StringBuilder;

        internal PooledStringBuilder(StringBuilderPool pool)
        {
            _pool = pool;
            StringBuilder = new StringBuilder();
        }

        internal PooledStringBuilder(StringBuilderPool pool, int capacity)
        {
            _pool = pool;
            StringBuilder = new StringBuilder(capacity);
        }

        internal PooledStringBuilder(StringBuilderPool pool, int capacity, int maxCapacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
            StringBuilder = new StringBuilder(capacity);
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (StringBuilder.Capacity > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

        /// <summary>
        /// Clears the object.
        /// </summary>
        public void Clear()
        {
            StringBuilder.Clear();
        }
    }


    /// <summary>
    /// Object pool for <see cref="StringBuilder"/> instances.
    /// </summary>
    public abstract partial class StringBuilderPool : ObjectPoolBase<PooledStringBuilder>
    {
        /// <summary>
        /// Creates a new <see cref="StringBuilder"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="StringBuilder"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected StringBuilderPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="StringBuilder"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="StringBuilder"/> by calling the default constructor on StringBuilder.
        /// </summary>
        /// <param name="size">Number of <see cref="StringBuilder"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="StringBuilder"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StringBuilderPool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : StringBuilderPool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledStringBuilder CreateInstance() => new PooledStringBuilder(this);
        }

        /// <summary>
        /// Creates a new <see cref="StringBuilder"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="StringBuilder"/> by calling the constructor on <see cref="StringBuilder"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="StringBuilder"/> instances to keep in the pool.</param>
        /// <param name="capacity">The suggested starting size of this instance.</param>
        /// <returns>Newly created pool for <see cref="StringBuilder"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StringBuilderPool Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : StringBuilderPool
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledStringBuilder CreateInstance() => new PooledStringBuilder(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="StringBuilder"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="StringBuilder"/> by calling the constructor on <see cref="StringBuilder"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="StringBuilder"/> instances to keep in the pool.</param>
        /// <param name="capacity">The suggested starting size of this instance.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="StringBuilder"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StringBuilderPool Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : StringBuilderPool
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledStringBuilder CreateInstance() => new PooledStringBuilder(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled string builder instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled string builder instance.</returns>
        public new PooledStringBuilderHolder New()
        {
            var res = new PooledObject<PooledStringBuilder>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.StringBuilder.Length == 0);

            System.Diagnostics.Debug.Assert(res.Object.StringBuilder.Length == 0, "A dirty object was returned from the pool.");

            return new PooledStringBuilderHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledStringBuilder> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="StringBuilder"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="StringBuilder"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledStringBuilderHolder : IDisposable
    {
        private readonly PooledObject<PooledStringBuilder> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled string builder object.
        /// </summary>
        /// <param name="obj">Pooled string builder object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledStringBuilderHolder(PooledObject<PooledStringBuilder> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="StringBuilder"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="StringBuilder"/> instance held by the holder.</returns>
        public StringBuilder StringBuilder
        {
            get
            {
                CheckAccess();

                return _obj.Object.StringBuilder;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="StringBuilder"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledStringBuilderHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#if FALSE

namespace System.Collections
{
    /// <summary>
    /// Implements the <see cref="T:System.Collections.IList" /> interface using an array whose size is dynamically increased as required.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledArrayList : ArrayList, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly ArrayListPool _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledArrayList(ArrayListPool pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledArrayList(ArrayListPool pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledArrayList(ArrayListPool pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledArrayList
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="ArrayList"/> instances.
    /// </summary>
    public abstract partial class ArrayListPool : ObjectPoolBase<PooledArrayList>
    {
        /// <summary>
        /// Creates a new <see cref="ArrayList"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="ArrayList"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ArrayListPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ArrayList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ArrayList"/> by calling the default constructor on ArrayList.
        /// </summary>
        /// <param name="size">Number of <see cref="ArrayList"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="ArrayList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ArrayListPool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : ArrayListPool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledArrayList CreateInstance() => new PooledArrayList(this);
        }

        /// <summary>
        /// Creates a new <see cref="ArrayList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ArrayList"/> by calling the constructor on <see cref="ArrayList"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="ArrayList"/> instances to keep in the pool.</param>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <returns>Newly created pool for <see cref="ArrayList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ArrayListPool Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : ArrayListPool
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledArrayList CreateInstance() => new PooledArrayList(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="ArrayList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ArrayList"/> by calling the constructor on <see cref="ArrayList"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="ArrayList"/> instances to keep in the pool.</param>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="ArrayList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ArrayListPool Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : ArrayListPool
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledArrayList CreateInstance() => new PooledArrayList(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled array list instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled array list instance.</returns>
        public new PooledArrayListHolder New()
        {
            var res = new PooledObject<PooledArrayList>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledArrayListHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledArrayList> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="ArrayList"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="ArrayList"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledArrayListHolder : IDisposable
    {
        private readonly PooledObject<PooledArrayList> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled array list object.
        /// </summary>
        /// <param name="obj">Pooled array list object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledArrayListHolder(PooledObject<PooledArrayList> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="ArrayList"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="ArrayList"/> instance held by the holder.</returns>
        public ArrayList ArrayList
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="ArrayList"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledArrayListHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections
{
    /// <summary>
    /// Represents a collection of key/value pairs that are organized based on the hash code of the key.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledHashtable : Hashtable, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly HashtablePool _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledHashtable(HashtablePool pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledHashtable(HashtablePool pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledHashtable(HashtablePool pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledHashtable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PooledHashtable"/> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="PooledHashtable"/>.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="PooledHashtable"/>.</param>
        protected PooledHashtable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
#endif

    /// <summary>
    /// Object pool for <see cref="Hashtable"/> instances.
    /// </summary>
    public abstract partial class HashtablePool : ObjectPoolBase<PooledHashtable>
    {
        /// <summary>
        /// Creates a new <see cref="Hashtable"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="Hashtable"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected HashtablePool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Hashtable"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Hashtable"/> by calling the default constructor on Hashtable.
        /// </summary>
        /// <param name="size">Number of <see cref="Hashtable"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="Hashtable"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static HashtablePool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : HashtablePool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledHashtable CreateInstance() => new PooledHashtable(this);
        }

        /// <summary>
        /// Creates a new <see cref="Hashtable"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Hashtable"/> by calling the constructor on <see cref="Hashtable"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Hashtable"/> instances to keep in the pool.</param>
        /// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain.</param>
        /// <returns>Newly created pool for <see cref="Hashtable"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static HashtablePool Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : HashtablePool
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledHashtable CreateInstance() => new PooledHashtable(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="Hashtable"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Hashtable"/> by calling the constructor on <see cref="Hashtable"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Hashtable"/> instances to keep in the pool.</param>
        /// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="Hashtable"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static HashtablePool Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : HashtablePool
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledHashtable CreateInstance() => new PooledHashtable(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled hashtable instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled hashtable instance.</returns>
        public new PooledHashtableHolder New()
        {
            var res = new PooledObject<PooledHashtable>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledHashtableHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledHashtable> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="Hashtable"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="Hashtable"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledHashtableHolder : IDisposable
    {
        private readonly PooledObject<PooledHashtable> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled hashtable object.
        /// </summary>
        /// <param name="obj">Pooled hashtable object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledHashtableHolder(PooledObject<PooledHashtable> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="Hashtable"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="Hashtable"/> instance held by the holder.</returns>
        public Hashtable Hashtable
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="Hashtable"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledHashtableHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledQueue : Queue, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly QueuePool _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledQueue(QueuePool pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledQueue(QueuePool pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledQueue(QueuePool pool, int capacity, int maxCapacity)
            : base(capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledQueue
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="Queue"/> instances.
    /// </summary>
    public abstract partial class QueuePool : ObjectPoolBase<PooledQueue>
    {
        /// <summary>
        /// Creates a new <see cref="Queue"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected QueuePool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Queue"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Queue"/> by calling the default constructor on Queue.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="Queue"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static QueuePool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : QueuePool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledQueue CreateInstance() => new PooledQueue(this);
        }

        /// <summary>
        /// Creates a new <see cref="Queue"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Queue"/> by calling the constructor on <see cref="Queue"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Queue" /> can contain.</param>
        /// <returns>Newly created pool for <see cref="Queue"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static QueuePool Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : QueuePool
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledQueue CreateInstance() => new PooledQueue(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="Queue"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Queue"/> by calling the constructor on <see cref="Queue"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Queue"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Queue" /> can contain.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="Queue"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static QueuePool Create(int size, int capacity, int maxCapacity)
        {
            CheckArguments(capacity, maxCapacity);

            return new Impl2(size, capacity, maxCapacity);
        }

        static partial void CheckArguments(int capacity, int maxCapacity);

        private sealed class Impl2 : QueuePool
        {
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int capacity, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledQueue CreateInstance() => new PooledQueue(this, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled queue instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled queue instance.</returns>
        public new PooledQueueHolder New()
        {
            var res = new PooledObject<PooledQueue>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledQueueHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledQueue> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="Queue"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="Queue"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledQueueHolder : IDisposable
    {
        private readonly PooledObject<PooledQueue> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled queue object.
        /// </summary>
        /// <param name="obj">Pooled queue object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledQueueHolder(PooledObject<PooledQueue> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="Queue"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="Queue"/> instance held by the holder.</returns>
        public Queue Queue
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="Queue"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledQueueHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections
{
    /// <summary>
    /// Represents a simple last-in-first-out (LIFO) non-generic collection of objects.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledStack : Stack, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly StackPool _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledStack(StackPool pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledStack(StackPool pool, int initialCapacity)
            : base(initialCapacity)
        {
            _pool = pool;
        }

        internal PooledStack(StackPool pool, int initialCapacity, int maxCapacity)
            : base(initialCapacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledStack
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="Stack"/> instances.
    /// </summary>
    public abstract partial class StackPool : ObjectPoolBase<PooledStack>
    {
        /// <summary>
        /// Creates a new <see cref="Stack"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected StackPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Stack"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Stack"/> by calling the default constructor on Stack.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="Stack"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StackPool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : StackPool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledStack CreateInstance() => new PooledStack(this);
        }

        /// <summary>
        /// Creates a new <see cref="Stack"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Stack"/> by calling the constructor on <see cref="Stack"/>, passing in the specified <paramref name="initialCapacity">initialCapacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack"/> instances to keep in the pool.</param>
        /// <param name="initialCapacity">The initial number of elements that the <see cref="T:System.Collections.Stack" /> can contain.</param>
        /// <returns>Newly created pool for <see cref="Stack"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StackPool Create(int size, int initialCapacity)
        {
            CheckArguments(initialCapacity);

            return new Impl1(size, initialCapacity);
        }

        static partial void CheckArguments(int initialCapacity);

        private sealed class Impl1 : StackPool
        {
            private readonly int _initialCapacity;

            public Impl1(int size, int initialCapacity)
                : base(size)
            {
                _initialCapacity = initialCapacity;
            }

            protected override PooledStack CreateInstance() => new PooledStack(this, _initialCapacity);
        }

        /// <summary>
        /// Creates a new <see cref="Stack"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="Stack"/> by calling the constructor on <see cref="Stack"/>, passing in the specified <paramref name="initialCapacity">initialCapacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="Stack"/> instances to keep in the pool.</param>
        /// <param name="initialCapacity">The initial number of elements that the <see cref="T:System.Collections.Stack" /> can contain.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="Stack"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static StackPool Create(int size, int initialCapacity, int maxCapacity)
        {
            CheckArguments(initialCapacity, maxCapacity);

            return new Impl2(size, initialCapacity, maxCapacity);
        }

        static partial void CheckArguments(int initialCapacity, int maxCapacity);

        private sealed class Impl2 : StackPool
        {
            private readonly int _initialCapacity;
            private readonly int _maxCapacity;

            public Impl2(int size, int initialCapacity, int maxCapacity)
                : base(size)
            {
                _initialCapacity = initialCapacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledStack CreateInstance() => new PooledStack(this, _initialCapacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled stack instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled stack instance.</returns>
        public new PooledStackHolder New()
        {
            var res = new PooledObject<PooledStack>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledStackHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledStack> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="Stack"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="Stack"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledStackHolder : IDisposable
    {
        private readonly PooledObject<PooledStack> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled stack object.
        /// </summary>
        /// <param name="obj">Pooled stack object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledStackHolder(PooledObject<PooledStack> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="Stack"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="Stack"/> instance held by the holder.</returns>
        public Stack Stack
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="Stack"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledStackHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections
{
    /// <summary>
    /// Represents a collection of key/value pairs that are sorted by the keys and are accessible by key and by index.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    public partial class PooledSortedList : SortedList, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly SortedListPool _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledSortedList(SortedListPool pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool pool, int initialCapacity)
            : base(initialCapacity)
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool pool, System.Collections.IComparer comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool pool, System.Collections.IComparer comparer, int capacity)
            : base(comparer, capacity)
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool pool, System.Collections.IComparer comparer, int capacity, int maxCapacity)
            : base(comparer, capacity)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledSortedList
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="SortedList"/> instances.
    /// </summary>
    public abstract partial class SortedListPool : ObjectPoolBase<PooledSortedList>
    {
        /// <summary>
        /// Creates a new <see cref="SortedList"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected SortedListPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SortedList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList"/> by calling the default constructor on SortedList.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="SortedList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : SortedListPool
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledSortedList CreateInstance() => new PooledSortedList(this);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList"/> by calling the constructor on <see cref="SortedList"/>, passing in the specified <paramref name="initialCapacity">initialCapacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList"/> instances to keep in the pool.</param>
        /// <param name="initialCapacity">The initial number of elements that the <see cref="T:System.Collections.SortedList" /> object can contain.</param>
        /// <returns>Newly created pool for <see cref="SortedList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool Create(int size, int initialCapacity)
        {
            CheckArguments(initialCapacity);

            return new Impl1(size, initialCapacity);
        }

        static partial void CheckArguments(int initialCapacity);

        private sealed class Impl1 : SortedListPool
        {
            private readonly int _initialCapacity;

            public Impl1(int size, int initialCapacity)
                : base(size)
            {
                _initialCapacity = initialCapacity;
            }

            protected override PooledSortedList CreateInstance() => new PooledSortedList(this, _initialCapacity);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList"/> by calling the constructor on <see cref="SortedList"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing keys.  
 -or-  
 <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each key.</param>
        /// <returns>Newly created pool for <see cref="SortedList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool Create(int size, System.Collections.IComparer comparer)
        {
            CheckArguments(comparer);

            return new Impl2(size, comparer);
        }

        static partial void CheckArguments(System.Collections.IComparer comparer);

        private sealed class Impl2 : SortedListPool
        {
            private readonly System.Collections.IComparer _comparer;

            public Impl2(int size, System.Collections.IComparer comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledSortedList CreateInstance() => new PooledSortedList(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList"/> by calling the constructor on <see cref="SortedList"/>, passing in the specified <paramref name="comparer">comparer</paramref> and <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing keys.  
 -or-  
 <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each key.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.SortedList" /> object can contain.</param>
        /// <returns>Newly created pool for <see cref="SortedList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool Create(int size, System.Collections.IComparer comparer, int capacity)
        {
            CheckArguments(comparer, capacity);

            return new Impl3(size, comparer, capacity);
        }

        static partial void CheckArguments(System.Collections.IComparer comparer, int capacity);

        private sealed class Impl3 : SortedListPool
        {
            private readonly System.Collections.IComparer _comparer;
            private readonly int _capacity;

            public Impl3(int size, System.Collections.IComparer comparer, int capacity)
                : base(size)
            {
                _comparer = comparer;
                _capacity = capacity;
            }

            protected override PooledSortedList CreateInstance() => new PooledSortedList(this, _comparer, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList"/> by calling the constructor on <see cref="SortedList"/>, passing in the specified <paramref name="comparer">comparer</paramref> and <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing keys.  
 -or-  
 <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each key.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.SortedList" /> object can contain.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="SortedList"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool Create(int size, System.Collections.IComparer comparer, int capacity, int maxCapacity)
        {
            CheckArguments(comparer, capacity, maxCapacity);

            return new Impl4(size, comparer, capacity, maxCapacity);
        }

        static partial void CheckArguments(System.Collections.IComparer comparer, int capacity, int maxCapacity);

        private sealed class Impl4 : SortedListPool
        {
            private readonly System.Collections.IComparer _comparer;
            private readonly int _capacity;
            private readonly int _maxCapacity;

            public Impl4(int size, System.Collections.IComparer comparer, int capacity, int maxCapacity)
                : base(size)
            {
                _comparer = comparer;
                _capacity = capacity;
                _maxCapacity = maxCapacity;
            }

            protected override PooledSortedList CreateInstance() => new PooledSortedList(this, _comparer, _capacity, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled sorted list instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled sorted list instance.</returns>
        public new PooledSortedListHolder New()
        {
            var res = new PooledObject<PooledSortedList>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledSortedListHolder(res);
        }

        partial void OnAllocate(PooledObject<PooledSortedList> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="SortedList"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="SortedList"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledSortedListHolder : IDisposable
    {
        private readonly PooledObject<PooledSortedList> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled sorted list object.
        /// </summary>
        /// <param name="obj">Pooled sorted list object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledSortedListHolder(PooledObject<PooledSortedList> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="SortedList"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="SortedList"/> instance held by the holder.</returns>
        public SortedList SortedList
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="SortedList"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledSortedListHolder
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a doubly linked list.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    public partial class PooledLinkedList<T> : LinkedList<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly LinkedListPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledLinkedList(LinkedListPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledLinkedList(LinkedListPool<T> pool, int maxCapacity)
            : base()
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledLinkedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PooledLinkedList{T}"/> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="PooledLinkedList{T}"/>.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="PooledLinkedList{T}"/>.</param>
        protected PooledLinkedList(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
#endif

    /// <summary>
    /// Object pool for <see cref="LinkedList{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    public abstract partial class LinkedListPool<T> : ObjectPoolBase<PooledLinkedList<T>>
    {
        /// <summary>
        /// Creates a new <see cref="LinkedList{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="LinkedList{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected LinkedListPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="LinkedList{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="LinkedList{T}"/> by calling the default constructor on LinkedList{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="LinkedList{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="LinkedList{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static LinkedListPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : LinkedListPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledLinkedList<T> CreateInstance() => new PooledLinkedList<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="LinkedList{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="LinkedList{T}"/> by calling the default constructor on LinkedList{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="LinkedList{T}"/> instances to keep in the pool.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="LinkedList{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static LinkedListPool<T> Create(int size, int maxCapacity)
        {
            CheckArguments(maxCapacity);

            return new Impl1(size, maxCapacity);
        }

        static partial void CheckArguments(int maxCapacity);

        private sealed class Impl1 : LinkedListPool<T>
        {
            private readonly int _maxCapacity;

            public Impl1(int size, int maxCapacity)
                : base(size)
            {
                _maxCapacity = maxCapacity;
            }

            protected override PooledLinkedList<T> CreateInstance() => new PooledLinkedList<T>(this, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled linked list instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled linked list instance.</returns>
        public new PooledLinkedListHolder<T> New()
        {
            var res = new PooledObject<PooledLinkedList<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledLinkedListHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledLinkedList<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="LinkedList{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="LinkedList{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledLinkedListHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledLinkedList<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled linked list object.
        /// </summary>
        /// <param name="obj">Pooled linked list object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledLinkedListHolder(PooledObject<PooledLinkedList<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="LinkedList{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="LinkedList{T}"/> instance held by the holder.</returns>
        public LinkedList<T> LinkedList
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="LinkedList{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledLinkedListHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a collection of key/value pairs that are sorted on the key.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public partial class PooledSortedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly SortedDictionaryPool<TKey, TValue> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledSortedDictionary(SortedDictionaryPool<TKey, TValue> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledSortedDictionary(SortedDictionaryPool<TKey, TValue> pool, System.Collections.Generic.IComparer<TKey> comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledSortedDictionary(SortedDictionaryPool<TKey, TValue> pool, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity)
            : base(comparer)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledSortedDictionary<TKey, TValue>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="SortedDictionary{TKey, TValue}"/> instances.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public abstract partial class SortedDictionaryPool<TKey, TValue> : ObjectPoolBase<PooledSortedDictionary<TKey, TValue>>
    {
        /// <summary>
        /// Creates a new <see cref="SortedDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected SortedDictionaryPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SortedDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedDictionary{TKey, TValue}"/> by calling the default constructor on SortedDictionary{TKey, TValue}.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="SortedDictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedDictionaryPool<TKey, TValue> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : SortedDictionaryPool<TKey, TValue>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledSortedDictionary<TKey, TValue> CreateInstance() => new PooledSortedDictionary<TKey, TValue>(this);
        }

        /// <summary>
        /// Creates a new <see cref="SortedDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedDictionary{TKey, TValue}"/> by calling the constructor on <see cref="SortedDictionary{TKey, TValue}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
        /// <returns>Newly created pool for <see cref="SortedDictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedDictionaryPool<TKey, TValue> Create(int size, System.Collections.Generic.IComparer<TKey> comparer)
        {
            CheckArguments(comparer);

            return new Impl1(size, comparer);
        }

        static partial void CheckArguments(System.Collections.Generic.IComparer<TKey> comparer);

        private sealed class Impl1 : SortedDictionaryPool<TKey, TValue>
        {
            private readonly System.Collections.Generic.IComparer<TKey> _comparer;

            public Impl1(int size, System.Collections.Generic.IComparer<TKey> comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledSortedDictionary<TKey, TValue> CreateInstance() => new PooledSortedDictionary<TKey, TValue>(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="SortedDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedDictionary{TKey, TValue}"/> by calling the constructor on <see cref="SortedDictionary{TKey, TValue}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="SortedDictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedDictionaryPool<TKey, TValue> Create(int size, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity)
        {
            CheckArguments(comparer, maxCapacity);

            return new Impl2(size, comparer, maxCapacity);
        }

        static partial void CheckArguments(System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity);

        private sealed class Impl2 : SortedDictionaryPool<TKey, TValue>
        {
            private readonly System.Collections.Generic.IComparer<TKey> _comparer;
            private readonly int _maxCapacity;

            public Impl2(int size, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity)
                : base(size)
            {
                _comparer = comparer;
                _maxCapacity = maxCapacity;
            }

            protected override PooledSortedDictionary<TKey, TValue> CreateInstance() => new PooledSortedDictionary<TKey, TValue>(this, _comparer, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled sorted dictionary instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled sorted dictionary instance.</returns>
        public new PooledSortedDictionaryHolder<TKey, TValue> New()
        {
            var res = new PooledObject<PooledSortedDictionary<TKey, TValue>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledSortedDictionaryHolder<TKey, TValue>(res);
        }

        partial void OnAllocate(PooledObject<PooledSortedDictionary<TKey, TValue>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="SortedDictionary{TKey, TValue}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="SortedDictionary{TKey, TValue}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledSortedDictionaryHolder<TKey, TValue> : IDisposable
    {
        private readonly PooledObject<PooledSortedDictionary<TKey, TValue>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled sorted dictionary object.
        /// </summary>
        /// <param name="obj">Pooled sorted dictionary object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledSortedDictionaryHolder(PooledObject<PooledSortedDictionary<TKey, TValue>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="SortedDictionary{TKey, TValue}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="SortedDictionary{TKey, TValue}"/> instance held by the holder.</returns>
        public SortedDictionary<TKey, TValue> SortedDictionary
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="SortedDictionary{TKey, TValue}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledSortedDictionaryHolder<TKey, TValue>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a collection of key/value pairs that are sorted by key based on the associated <see cref="T:System.Collections.Generic.IComparer`1" /> implementation.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of values in the collection.</typeparam>
    public partial class PooledSortedList<TKey, TValue> : SortedList<TKey, TValue>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly SortedListPool<TKey, TValue> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledSortedList(SortedListPool<TKey, TValue> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool<TKey, TValue> pool, int capacity)
            : base(capacity)
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool<TKey, TValue> pool, System.Collections.Generic.IComparer<TKey> comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool<TKey, TValue> pool, int capacity, System.Collections.Generic.IComparer<TKey> comparer)
            : base(capacity, comparer)
        {
            _pool = pool;
        }

        internal PooledSortedList(SortedListPool<TKey, TValue> pool, int capacity, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity)
            : base(capacity, comparer)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledSortedList<TKey, TValue>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="SortedList{TKey, TValue}"/> instances.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of values in the collection.</typeparam>
    public abstract partial class SortedListPool<TKey, TValue> : ObjectPoolBase<PooledSortedList<TKey, TValue>>
    {
        /// <summary>
        /// Creates a new <see cref="SortedList{TKey, TValue}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected SortedListPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SortedList{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList{TKey, TValue}"/> by calling the default constructor on SortedList{TKey, TValue}.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="SortedList{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool<TKey, TValue> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : SortedListPool<TKey, TValue>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledSortedList<TKey, TValue> CreateInstance() => new PooledSortedList<TKey, TValue>(this);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList{TKey, TValue}"/> by calling the constructor on <see cref="SortedList{TKey, TValue}"/>, passing in the specified <paramref name="capacity">capacity</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.SortedList`2" /> can contain.</param>
        /// <returns>Newly created pool for <see cref="SortedList{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool<TKey, TValue> Create(int size, int capacity)
        {
            CheckArguments(capacity);

            return new Impl1(size, capacity);
        }

        static partial void CheckArguments(int capacity);

        private sealed class Impl1 : SortedListPool<TKey, TValue>
        {
            private readonly int _capacity;

            public Impl1(int size, int capacity)
                : base(size)
            {
                _capacity = capacity;
            }

            protected override PooledSortedList<TKey, TValue> CreateInstance() => new PooledSortedList<TKey, TValue>(this, _capacity);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList{TKey, TValue}"/> by calling the constructor on <see cref="SortedList{TKey, TValue}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys.  
 -or-  
 <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
        /// <returns>Newly created pool for <see cref="SortedList{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool<TKey, TValue> Create(int size, System.Collections.Generic.IComparer<TKey> comparer)
        {
            CheckArguments(comparer);

            return new Impl2(size, comparer);
        }

        static partial void CheckArguments(System.Collections.Generic.IComparer<TKey> comparer);

        private sealed class Impl2 : SortedListPool<TKey, TValue>
        {
            private readonly System.Collections.Generic.IComparer<TKey> _comparer;

            public Impl2(int size, System.Collections.Generic.IComparer<TKey> comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledSortedList<TKey, TValue> CreateInstance() => new PooledSortedList<TKey, TValue>(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList{TKey, TValue}"/> by calling the constructor on <see cref="SortedList{TKey, TValue}"/>, passing in the specified <paramref name="capacity">capacity</paramref> and <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.SortedList`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys.  
 -or-  
 <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
        /// <returns>Newly created pool for <see cref="SortedList{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool<TKey, TValue> Create(int size, int capacity, System.Collections.Generic.IComparer<TKey> comparer)
        {
            CheckArguments(capacity, comparer);

            return new Impl3(size, capacity, comparer);
        }

        static partial void CheckArguments(int capacity, System.Collections.Generic.IComparer<TKey> comparer);

        private sealed class Impl3 : SortedListPool<TKey, TValue>
        {
            private readonly int _capacity;
            private readonly System.Collections.Generic.IComparer<TKey> _comparer;

            public Impl3(int size, int capacity, System.Collections.Generic.IComparer<TKey> comparer)
                : base(size)
            {
                _capacity = capacity;
                _comparer = comparer;
            }

            protected override PooledSortedList<TKey, TValue> CreateInstance() => new PooledSortedList<TKey, TValue>(this, _capacity, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="SortedList{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedList{TKey, TValue}"/> by calling the constructor on <see cref="SortedList{TKey, TValue}"/>, passing in the specified <paramref name="capacity">capacity</paramref> and <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedList{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.SortedList`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys.  
 -or-  
 <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="SortedList{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedListPool<TKey, TValue> Create(int size, int capacity, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity)
        {
            CheckArguments(capacity, comparer, maxCapacity);

            return new Impl4(size, capacity, comparer, maxCapacity);
        }

        static partial void CheckArguments(int capacity, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity);

        private sealed class Impl4 : SortedListPool<TKey, TValue>
        {
            private readonly int _capacity;
            private readonly System.Collections.Generic.IComparer<TKey> _comparer;
            private readonly int _maxCapacity;

            public Impl4(int size, int capacity, System.Collections.Generic.IComparer<TKey> comparer, int maxCapacity)
                : base(size)
            {
                _capacity = capacity;
                _comparer = comparer;
                _maxCapacity = maxCapacity;
            }

            protected override PooledSortedList<TKey, TValue> CreateInstance() => new PooledSortedList<TKey, TValue>(this, _capacity, _comparer, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled sorted list instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled sorted list instance.</returns>
        public new PooledSortedListHolder<TKey, TValue> New()
        {
            var res = new PooledObject<PooledSortedList<TKey, TValue>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledSortedListHolder<TKey, TValue>(res);
        }

        partial void OnAllocate(PooledObject<PooledSortedList<TKey, TValue>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="SortedList{TKey, TValue}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="SortedList{TKey, TValue}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of values in the collection.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledSortedListHolder<TKey, TValue> : IDisposable
    {
        private readonly PooledObject<PooledSortedList<TKey, TValue>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled sorted list object.
        /// </summary>
        /// <param name="obj">Pooled sorted list object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledSortedListHolder(PooledObject<PooledSortedList<TKey, TValue>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="SortedList{TKey, TValue}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="SortedList{TKey, TValue}"/> instance held by the holder.</returns>
        public SortedList<TKey, TValue> SortedList
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="SortedList{TKey, TValue}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledSortedListHolder<TKey, TValue>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a collection of objects that is maintained in sorted order.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public partial class PooledSortedSet<T> : SortedSet<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly SortedSetPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledSortedSet(SortedSetPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledSortedSet(SortedSetPool<T> pool, System.Collections.Generic.IComparer<T> comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledSortedSet(SortedSetPool<T> pool, System.Collections.Generic.IComparer<T> comparer, int maxCapacity)
            : base(comparer)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledSortedSet<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PooledSortedSet{T}"/> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="PooledSortedSet{T}"/>.</param>
        /// <param name="context">A <see cref="System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="PooledSortedSet{T}"/>.</param>
        protected PooledSortedSet(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
#endif

    /// <summary>
    /// Object pool for <see cref="SortedSet{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public abstract partial class SortedSetPool<T> : ObjectPoolBase<PooledSortedSet<T>>
    {
        /// <summary>
        /// Creates a new <see cref="SortedSet{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedSet{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected SortedSetPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SortedSet{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedSet{T}"/> by calling the default constructor on SortedSet{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedSet{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="SortedSet{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedSetPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : SortedSetPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledSortedSet<T> CreateInstance() => new PooledSortedSet<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="SortedSet{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedSet{T}"/> by calling the constructor on <see cref="SortedSet{T}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedSet{T}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The default comparer to use for comparing objects.</param>
        /// <returns>Newly created pool for <see cref="SortedSet{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedSetPool<T> Create(int size, System.Collections.Generic.IComparer<T> comparer)
        {
            CheckArguments(comparer);

            return new Impl1(size, comparer);
        }

        static partial void CheckArguments(System.Collections.Generic.IComparer<T> comparer);

        private sealed class Impl1 : SortedSetPool<T>
        {
            private readonly System.Collections.Generic.IComparer<T> _comparer;

            public Impl1(int size, System.Collections.Generic.IComparer<T> comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledSortedSet<T> CreateInstance() => new PooledSortedSet<T>(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="SortedSet{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="SortedSet{T}"/> by calling the constructor on <see cref="SortedSet{T}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="SortedSet{T}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The default comparer to use for comparing objects.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="SortedSet{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static SortedSetPool<T> Create(int size, System.Collections.Generic.IComparer<T> comparer, int maxCapacity)
        {
            CheckArguments(comparer, maxCapacity);

            return new Impl2(size, comparer, maxCapacity);
        }

        static partial void CheckArguments(System.Collections.Generic.IComparer<T> comparer, int maxCapacity);

        private sealed class Impl2 : SortedSetPool<T>
        {
            private readonly System.Collections.Generic.IComparer<T> _comparer;
            private readonly int _maxCapacity;

            public Impl2(int size, System.Collections.Generic.IComparer<T> comparer, int maxCapacity)
                : base(size)
            {
                _comparer = comparer;
                _maxCapacity = maxCapacity;
            }

            protected override PooledSortedSet<T> CreateInstance() => new PooledSortedSet<T>(this, _comparer, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled sorted set instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled sorted set instance.</returns>
        public new PooledSortedSetHolder<T> New()
        {
            var res = new PooledObject<PooledSortedSet<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledSortedSetHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledSortedSet<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="SortedSet{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="SortedSet{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledSortedSetHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledSortedSet<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled sorted set object.
        /// </summary>
        /// <param name="obj">Pooled sorted set object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledSortedSetHolder(PooledObject<PooledSortedSet<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="SortedSet{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="SortedSet{T}"/> instance held by the holder.</returns>
        public SortedSet<T> SortedSet
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="SortedSet{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledSortedSetHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe, unordered collection of objects.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be stored in the collection.</typeparam>
    public partial class PooledConcurrentBag<T> : ConcurrentBag<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly ConcurrentBagPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledConcurrentBag(ConcurrentBagPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledConcurrentBag(ConcurrentBagPool<T> pool, int maxCapacity)
            : base()
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

        /// <summary>
        /// Clears the object.
        /// </summary>
        public void Clear()
        {
            ClearCore();
        }

        partial void ClearCore();
    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledConcurrentBag<T>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="ConcurrentBag{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be stored in the collection.</typeparam>
    public abstract partial class ConcurrentBagPool<T> : ObjectPoolBase<PooledConcurrentBag<T>>
    {
        /// <summary>
        /// Creates a new <see cref="ConcurrentBag{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentBag{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ConcurrentBagPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentBag{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentBag{T}"/> by calling the default constructor on ConcurrentBag{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentBag{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentBag{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentBagPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : ConcurrentBagPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledConcurrentBag<T> CreateInstance() => new PooledConcurrentBag<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentBag{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentBag{T}"/> by calling the default constructor on ConcurrentBag{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentBag{T}"/> instances to keep in the pool.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentBag{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentBagPool<T> Create(int size, int maxCapacity)
        {
            CheckArguments(maxCapacity);

            return new Impl1(size, maxCapacity);
        }

        static partial void CheckArguments(int maxCapacity);

        private sealed class Impl1 : ConcurrentBagPool<T>
        {
            private readonly int _maxCapacity;

            public Impl1(int size, int maxCapacity)
                : base(size)
            {
                _maxCapacity = maxCapacity;
            }

            protected override PooledConcurrentBag<T> CreateInstance() => new PooledConcurrentBag<T>(this, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled concurrent bag instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled concurrent bag instance.</returns>
        public new PooledConcurrentBagHolder<T> New()
        {
            var res = new PooledObject<PooledConcurrentBag<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledConcurrentBagHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledConcurrentBag<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="ConcurrentBag{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="ConcurrentBag{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be stored in the collection.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledConcurrentBagHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledConcurrentBag<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled concurrent bag object.
        /// </summary>
        /// <param name="obj">Pooled concurrent bag object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledConcurrentBagHolder(PooledObject<PooledConcurrentBag<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="ConcurrentBag{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="ConcurrentBag{T}"/> instance held by the holder.</returns>
        public ConcurrentBag<T> ConcurrentBag
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="ConcurrentBag{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledConcurrentBagHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe collection of key/value pairs that can be accessed by multiple threads concurrently.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public partial class PooledConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly ConcurrentDictionaryPool<TKey, TValue> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledConcurrentDictionary(ConcurrentDictionaryPool<TKey, TValue> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledConcurrentDictionary(ConcurrentDictionaryPool<TKey, TValue> pool, System.Collections.Generic.IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            _pool = pool;
        }

        internal PooledConcurrentDictionary(ConcurrentDictionaryPool<TKey, TValue> pool, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity)
            : base(comparer)
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledConcurrentDictionary<TKey, TValue>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="ConcurrentDictionary{TKey, TValue}"/> instances.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public abstract partial class ConcurrentDictionaryPool<TKey, TValue> : ObjectPoolBase<PooledConcurrentDictionary<TKey, TValue>>
    {
        /// <summary>
        /// Creates a new <see cref="ConcurrentDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ConcurrentDictionaryPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentDictionary{TKey, TValue}"/> by calling the default constructor on ConcurrentDictionary{TKey, TValue}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentDictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentDictionaryPool<TKey, TValue> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : ConcurrentDictionaryPool<TKey, TValue>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledConcurrentDictionary<TKey, TValue> CreateInstance() => new PooledConcurrentDictionary<TKey, TValue>(this);
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentDictionary{TKey, TValue}"/> by calling the constructor on <see cref="ConcurrentDictionary{TKey, TValue}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The equality comparison implementation to use when comparing keys.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentDictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentDictionaryPool<TKey, TValue> Create(int size, System.Collections.Generic.IEqualityComparer<TKey> comparer)
        {
            CheckArguments(comparer);

            return new Impl1(size, comparer);
        }

        static partial void CheckArguments(System.Collections.Generic.IEqualityComparer<TKey> comparer);

        private sealed class Impl1 : ConcurrentDictionaryPool<TKey, TValue>
        {
            private readonly System.Collections.Generic.IEqualityComparer<TKey> _comparer;

            public Impl1(int size, System.Collections.Generic.IEqualityComparer<TKey> comparer)
                : base(size)
            {
                _comparer = comparer;
            }

            protected override PooledConcurrentDictionary<TKey, TValue> CreateInstance() => new PooledConcurrentDictionary<TKey, TValue>(this, _comparer);
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentDictionary{TKey, TValue}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentDictionary{TKey, TValue}"/> by calling the constructor on <see cref="ConcurrentDictionary{TKey, TValue}"/>, passing in the specified <paramref name="comparer">comparer</paramref>.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentDictionary{TKey, TValue}"/> instances to keep in the pool.</param>
        /// <param name="comparer">The equality comparison implementation to use when comparing keys.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentDictionary{TKey, TValue}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentDictionaryPool<TKey, TValue> Create(int size, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity)
        {
            CheckArguments(comparer, maxCapacity);

            return new Impl2(size, comparer, maxCapacity);
        }

        static partial void CheckArguments(System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity);

        private sealed class Impl2 : ConcurrentDictionaryPool<TKey, TValue>
        {
            private readonly System.Collections.Generic.IEqualityComparer<TKey> _comparer;
            private readonly int _maxCapacity;

            public Impl2(int size, System.Collections.Generic.IEqualityComparer<TKey> comparer, int maxCapacity)
                : base(size)
            {
                _comparer = comparer;
                _maxCapacity = maxCapacity;
            }

            protected override PooledConcurrentDictionary<TKey, TValue> CreateInstance() => new PooledConcurrentDictionary<TKey, TValue>(this, _comparer, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled concurrent dictionary instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled concurrent dictionary instance.</returns>
        public new PooledConcurrentDictionaryHolder<TKey, TValue> New()
        {
            var res = new PooledObject<PooledConcurrentDictionary<TKey, TValue>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledConcurrentDictionaryHolder<TKey, TValue>(res);
        }

        partial void OnAllocate(PooledObject<PooledConcurrentDictionary<TKey, TValue>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="ConcurrentDictionary{TKey, TValue}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="ConcurrentDictionary{TKey, TValue}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledConcurrentDictionaryHolder<TKey, TValue> : IDisposable
    {
        private readonly PooledObject<PooledConcurrentDictionary<TKey, TValue>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled concurrent dictionary object.
        /// </summary>
        /// <param name="obj">Pooled concurrent dictionary object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledConcurrentDictionaryHolder(PooledObject<PooledConcurrentDictionary<TKey, TValue>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="ConcurrentDictionary{TKey, TValue}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="ConcurrentDictionary{TKey, TValue}"/> instance held by the holder.</returns>
        public ConcurrentDictionary<TKey, TValue> ConcurrentDictionary
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="ConcurrentDictionary{TKey, TValue}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledConcurrentDictionaryHolder<TKey, TValue>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe first in-first out (FIFO) collection.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the queue.</typeparam>
    public partial class PooledConcurrentQueue<T> : ConcurrentQueue<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly ConcurrentQueuePool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledConcurrentQueue(ConcurrentQueuePool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledConcurrentQueue(ConcurrentQueuePool<T> pool, int maxCapacity)
            : base()
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

        /// <summary>
        /// Clears the object.
        /// </summary>
        public void Clear()
        {
            ClearCore();
        }

        partial void ClearCore();
    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledConcurrentQueue<T>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="ConcurrentQueue{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the queue.</typeparam>
    public abstract partial class ConcurrentQueuePool<T> : ObjectPoolBase<PooledConcurrentQueue<T>>
    {
        /// <summary>
        /// Creates a new <see cref="ConcurrentQueue{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentQueue{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ConcurrentQueuePool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentQueue{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentQueue{T}"/> by calling the default constructor on ConcurrentQueue{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentQueue{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentQueue{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentQueuePool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : ConcurrentQueuePool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledConcurrentQueue<T> CreateInstance() => new PooledConcurrentQueue<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentQueue{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentQueue{T}"/> by calling the default constructor on ConcurrentQueue{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentQueue{T}"/> instances to keep in the pool.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentQueue{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentQueuePool<T> Create(int size, int maxCapacity)
        {
            CheckArguments(maxCapacity);

            return new Impl1(size, maxCapacity);
        }

        static partial void CheckArguments(int maxCapacity);

        private sealed class Impl1 : ConcurrentQueuePool<T>
        {
            private readonly int _maxCapacity;

            public Impl1(int size, int maxCapacity)
                : base(size)
            {
                _maxCapacity = maxCapacity;
            }

            protected override PooledConcurrentQueue<T> CreateInstance() => new PooledConcurrentQueue<T>(this, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled concurrent queue instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled concurrent queue instance.</returns>
        public new PooledConcurrentQueueHolder<T> New()
        {
            var res = new PooledObject<PooledConcurrentQueue<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledConcurrentQueueHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledConcurrentQueue<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="ConcurrentQueue{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="ConcurrentQueue{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the queue.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledConcurrentQueueHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledConcurrentQueue<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled concurrent queue object.
        /// </summary>
        /// <param name="obj">Pooled concurrent queue object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledConcurrentQueueHolder(PooledObject<PooledConcurrentQueue<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="ConcurrentQueue{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="ConcurrentQueue{T}"/> instance held by the holder.</returns>
        public ConcurrentQueue<T> ConcurrentQueue
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="ConcurrentQueue{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledConcurrentQueueHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

#if FALSE

namespace System.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe last in-first out (LIFO) collection.
    /// Instances of this type are kept in a pool for recycling.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the stack.</typeparam>
    public partial class PooledConcurrentStack<T> : ConcurrentStack<T>, IFreeable, IClearable
    {
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly ConcurrentStackPool<T> _pool;
#if !NO_SERIALIZATION
        [NonSerialized]
#endif
        private readonly int _maxCapacity = 1024;

        internal PooledConcurrentStack(ConcurrentStackPool<T> pool)
            : base()
        {
            _pool = pool;
        }

        internal PooledConcurrentStack(ConcurrentStackPool<T> pool, int maxCapacity)
            : base()
        {
            _pool = pool;
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// Frees the object and returns it to the pool.
        /// </summary>
        public void Free()
        {
            if (_pool != null)
            {
                if (Count > _maxCapacity)
                {
                    _pool.ForgetTrackedObject(this);
                    return;
                }

                _pool.FreeFast(this);
            }
        }

    }

#if !NO_SERIALIZATION
    [Serializable]
    partial class PooledConcurrentStack<T>
    {
    }
#endif

    /// <summary>
    /// Object pool for <see cref="ConcurrentStack{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the stack.</typeparam>
    public abstract partial class ConcurrentStackPool<T> : ObjectPoolBase<PooledConcurrentStack<T>>
    {
        /// <summary>
        /// Creates a new <see cref="ConcurrentStack{T}"/> pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentStack{T}"/> instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ConcurrentStackPool(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentStack{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentStack{T}"/> by calling the default constructor on ConcurrentStack{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentStack{T}"/> instances to keep in the pool.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentStack{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentStackPool<T> Create(int size)
        {
            return new Impl0(size);
        }

        private sealed class Impl0 : ConcurrentStackPool<T>
        {

            public Impl0(int size)
                : base(size)
            {
            }

            protected override PooledConcurrentStack<T> CreateInstance() => new PooledConcurrentStack<T>(this);
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentStack{T}"/> pool of the specified pool size.
        /// The pool creates new instances of <see cref="ConcurrentStack{T}"/> by calling the default constructor on ConcurrentStack{T}.
        /// </summary>
        /// <param name="size">Number of <see cref="ConcurrentStack{T}"/> instances to keep in the pool.</param>
        /// <param name="maxCapacity">The maximum capacity allowed for pooled instances.</param>
        /// <returns>Newly created pool for <see cref="ConcurrentStack{T}"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Standard factory pattern.")]
        public static ConcurrentStackPool<T> Create(int size, int maxCapacity)
        {
            CheckArguments(maxCapacity);

            return new Impl1(size, maxCapacity);
        }

        static partial void CheckArguments(int maxCapacity);

        private sealed class Impl1 : ConcurrentStackPool<T>
        {
            private readonly int _maxCapacity;

            public Impl1(int size, int maxCapacity)
                : base(size)
            {
                _maxCapacity = maxCapacity;
            }

            protected override PooledConcurrentStack<T> CreateInstance() => new PooledConcurrentStack<T>(this, _maxCapacity);
        }


        /// <summary>
        /// Gets a holder to a pooled concurrent stack instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Holder to a pooled concurrent stack instance.</returns>
        public new PooledConcurrentStackHolder<T> New()
        {
            var res = new PooledObject<PooledConcurrentStack<T>>(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (_, o) => o.Free());

            OnAllocate(res, res.Object.Count == 0);

            System.Diagnostics.Debug.Assert(res.Object.Count == 0, "A dirty object was returned from the pool.");

            return new PooledConcurrentStackHolder<T>(res);
        }

        partial void OnAllocate(PooledObject<PooledConcurrentStack<T>> obj, bool isCleared);
    }

#pragma warning disable 0282 // Order of fields and their initialization doesn't matter for us

    /// <summary>
    /// Struct holding a pooled <see cref="ConcurrentStack{T}"/> instance. Values of this type get
    /// returned from the New methods on Pooled<see cref="ConcurrentStack{T}"/> and provide a
    /// strongly typed disposable wrapper around the resource.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the stack.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "RAII pattern does not require equality checks.")]
    public partial struct PooledConcurrentStackHolder<T> : IDisposable
    {
        private readonly PooledObject<PooledConcurrentStack<T>> _obj;
#if DEBUG
        private int _disposed; // Put here to work around CS0171 prior to Roslyn
#endif

        /// <summary>
        /// Creates a new holder for the given pooled concurrent stack object.
        /// </summary>
        /// <param name="obj">Pooled concurrent stack object to create a holder for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design.")]
        public PooledConcurrentStackHolder(PooledObject<PooledConcurrentStack<T>> obj)
        {
            _obj = obj;
#if DEBUG
            _disposed = 0; // Put here to work around CS0171 prior to Roslyn
#endif
        }

        /// <summary>
        /// Gets the <see cref="ConcurrentStack{T}"/> instance held by this instance.
        /// </summary>
        /// <returns>The <see cref="ConcurrentStack{T}"/> instance held by the holder.</returns>
        public ConcurrentStack<T> ConcurrentStack
        {
            get
            {
                CheckAccess();

                return _obj.Object;
            }
        }

        /// <summary>
        /// Returns the pooled <see cref="ConcurrentStack{T}"/> instance back to the pool.
        /// </summary>
        public void Dispose()
        {
            AssertSingleDispose();

            _obj.Dispose();
        }

        partial void CheckAccess();

        partial void AssertSingleDispose();
    }

#if DEBUG
    partial struct PooledConcurrentStackHolder<T>
    {
        [ExcludeFromCodeCoverage]
        partial void CheckAccess()
        {
            if (System.Threading.Volatile.Read(ref _disposed) != 0)
                throw new ObjectDisposedException("holder");
        }

        [ExcludeFromCodeCoverage]
        partial void AssertSingleDispose()
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) != 0)
                throw new InvalidOperationException("Holder object of type " + ToString() + " got disposed more than once.");
        }
    }
#endif

#pragma warning restore 0282
}
#endif

