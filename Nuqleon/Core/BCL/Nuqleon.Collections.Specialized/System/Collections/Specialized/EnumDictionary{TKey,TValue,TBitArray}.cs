// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Collections.Specialized
{
    internal sealed class EnumDictionary<TKey, TValue, TBitArray> : IDictionary<TKey, TValue>
        where TKey : struct, IConvertible
        where TBitArray : struct, IBitArray
    {
        private static int _enumSize;

        private readonly TValue[] _values;

#pragma warning disable IDE0044 // Add readonly modifier (https://github.com/dotnet/roslyn/issues/49290)
        private TBitArray _hasValue;
#pragma warning restore IDE0044 // Add readonly modifier

        private int _version = 0;

        internal EnumDictionary(TBitArray bitArray)
        {
            try
            {
                _enumSize = EnumDictionary<TKey>.enumSize.Value;
            }
            catch (EnumSizeResolutionException e)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable CA2208 // Use of TKey as paramName
                switch (e.ErrorCode)
                {
                    case EnumSizeResolutionError.TypeIsNotEnum:
                        throw new ArgumentException("TKey needs to be an enum.", nameof(TKey), e);
                    case EnumSizeResolutionError.UnderlyingTypeIsNotIntOrSmaller:
                        throw new ArgumentException("The underlying type for enum TKey needs to be an int.", nameof(TKey), e);
                    case EnumSizeResolutionError.EnumContainsNegativeValues:
                        throw new ArgumentException("The enum contains negative values.", nameof(TKey), e);
                    case EnumSizeResolutionError.EnumHasFlagAttribute:
                        throw new ArgumentException("Flags enums are not supported.", nameof(TKey), e);
                    case EnumSizeResolutionError.Unknown:
                    default:
                        throw;
#pragma warning restore CA2208
#pragma warning restore IDE0079
                }
            }

            if (bitArray.Count < _enumSize)
            {
                throw new ArgumentException("Bit array is too small", nameof(bitArray));
            }

            _values = new TValue[_enumSize];
            _hasValue = bitArray;
        }

        public void Add(TKey key, TValue value) => Add(key, value, throwOnDuplicate: true);

        private void Add(TKey key, TValue value, bool throwOnDuplicate)
        {
            var k = key.ToInt32(provider: null);
            if (k < 0 || k >= _enumSize)
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            if (!_hasValue[k])
            {
                _version++;
                Count++;
                _hasValue[k] = true;
                _values[k] = value;
            }
            else if (throwOnDuplicate)
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }
            else
            {
                _version++;
                _values[k] = value;
            }
        }

        public bool ContainsKey(TKey key)
        {
            var k = key.ToInt32(provider: null);
            if (k < 0 || k >= _enumSize)
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            return _hasValue[k];
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Select(kvp => kvp.Key).ToList();

        public bool Remove(TKey key)
        {
            var k = key.ToInt32(provider: null);
            if (k < 0 || k >= _enumSize)
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            var temp = _hasValue[k];

            if (temp)
            {
                _version++;
                _hasValue[k] = false;
                _values[k] = default;
                Count--;
            }

            return temp;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var k = key.ToInt32(provider: null);
            if (k < 0 || k >= _enumSize)
            {
                // System.Collections.Dictionary throws when key is null, so keep that behavior here
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            var temp = _hasValue[k];
            value = temp ? _values[k] : default;
            return temp;
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Select(kvp => kvp.Value).ToList();

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue ret))
                {
                    return ret;
                }

                throw new KeyNotFoundException();
            }

            set => Add(key, value, throwOnDuplicate: false);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value, throwOnDuplicate: true);

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var k = item.Key.ToInt32(provider: null);
            if (k < 0 || k >= _enumSize)
            {
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            if (_hasValue[k] && EqualityComparer<TValue>.Default.Equals(_values[k], item.Value))
            {
                return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("Array plus offset is too small.");
            }

            var underlyingType = Enum.GetUnderlyingType(typeof(TKey));
            for (int i = 0; i < _values.Length; i++)
            {
                if (_hasValue[i])
                {
                    array[index++] = new KeyValuePair<TKey, TValue>((TKey)Convert.ChangeType(i, underlyingType, provider: null), _values[i]);
                }
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var k = item.Key.ToInt32(provider: null);
            if (k < 0 || k >= _enumSize)
            {
                // System.Collections.Dictionary throws when key is null, so keep that behavior here
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            var exists = _hasValue[k] && EqualityComparer<TValue>.Default.Equals(_values[k], item.Value);
            if (exists)
            {
                _version++;
                _hasValue[k] = false;
                _values[k] = default;
                Count--;
            }

            return exists;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var version = _version;
            var underlyingType = Enum.GetUnderlyingType(typeof(TKey));
            for (var i = 0; i < _values.Length; i++)
            {
                if (version != _version)
                {
                    throw new InvalidOperationException("Underlying collection was modified during enumeration.");
                }

                if (_hasValue[i])
                {
                    yield return new KeyValuePair<TKey, TValue>((TKey)Convert.ChangeType(i, underlyingType, CultureInfo.InvariantCulture), _values[i]);
                }
            }
        }

        public void Clear()
        {
            _version++;
            _hasValue.SetAll(false);
            Array.Clear(_values, 0, _values.Length);
            Count = 0;
        }

        public int Count { get; private set; } = 0;

        public bool IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
