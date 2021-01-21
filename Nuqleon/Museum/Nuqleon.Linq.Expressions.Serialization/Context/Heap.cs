// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Heap for serialization and deserialization of objects referenced by closures.
    /// </summary>
    internal class Heap
    {
        #region Constants

        /// <summary>
        /// Address of the null reference.
        /// </summary>
        private const int ADDRESS_NULL = 0;

        /// <summary>
        /// First address for non-null objects.
        /// </summary>
        private const int ADDRESS_FIRST = ADDRESS_NULL + 1;

        #endregion

        #region Fields

        /// <summary>
        /// Mapping of objects to addresses during serialization.
        /// </summary>
        private readonly Dictionary<object, int> _addressOf;

        /// <summary>
        /// Heap representation for objects indexed by address.
        /// </summary>
        private readonly Dictionary<int, Json.Expression> _objects;

        /// <summary>
        /// Mapping of addresses to object instances during deserialization.
        /// </summary>
        private readonly Dictionary<int, object> _deserializedObjects;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new heap for use during serialization.
        /// </summary>
        public Heap()
            : this(serializing: true)
        {
        }

        /// <summary>
        /// Creates a new heap for use during serialization or deserialization.
        /// </summary>
        /// <param name="serializing">Indicates whether the heap is to be used during serialization or deserialization.</param>
        private Heap(bool serializing)
        {
            if (serializing)
            {
                _addressOf = new Dictionary<object, int>();
                _objects = new Dictionary<int, Json.Expression>();
            }
            else
            {
                _objects = new Dictionary<int, Json.Expression>();
                _deserializedObjects = new Dictionary<int, object>();
            }
        }

        #endregion

        #region Methods

        #region Accessors

        /// <summary>
        /// Gets an object from the heap at the specified address.
        /// </summary>
        /// <param name="address">Address of the object to retrieve.</param>
        /// <returns>Representation of the object at the specified address.</returns>
        public Json.Expression Get(Json.Expression address)
        {
            return _objects[AddressFromJson(address)];
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Tries to get the address of the specified object on the heap.
        /// This method can only be used during serialization.
        /// </summary>
        /// <param name="item">Object to lookup in the heap.</param>
        /// <param name="address">Object reference representation to use for serialization.</param>
        /// <returns>true if the object was found on the heap; otherwise, false.</returns>
        public bool TryGetAddress(object item, out Json.Expression address)
        {
            var res = ADDRESS_NULL;

            if (item == null || _addressOf.TryGetValue(item, out res))
            {
                address = AddressToJson(res);
                return true;
            }

            address = null;
            return false;
        }

        /// <summary>
        /// Adds the specified object to the heap.
        /// This method can only be used during serialization.
        /// </summary>
        /// <param name="item">Object to add to the heap.</param>
        /// <param name="serialized">Serialized object representation.</param>
        /// <returns>Object reference representation to use for serialization.</returns>
        public Json.Expression Add(object item, Json.Expression serialized)
        {
            var address = ADDRESS_NULL;

            if (item != null)
            {
                if (_addressOf.ContainsKey(item))
                    throw new InvalidOperationException("Heap already contains the specified object.");

                address = ADDRESS_FIRST + _addressOf.Count;
                _addressOf[item] = address;
                _objects[address] = serialized;
            }

            return AddressToJson(address);
        }

        #endregion

        #region Deserialization

        /// <summary>
        /// Tries to get an object instance from the heap at the specified address.
        /// This method can only be used during deserialization.
        /// </summary>
        /// <param name="address">Address to retrieve the object instance from.</param>
        /// <param name="item">Object instance found on the heap.</param>
        /// <returns>true if the object was found on the heap; otherwise, false.</returns>
        public bool TryLookup(Json.Expression address, out object item)
        {
            var n = AddressFromJson(address);
            if (n == ADDRESS_NULL)
            {
                item = null;
                return true;
            }
            else
            {
                return _deserializedObjects.TryGetValue(n, out item);
            }
        }

        /// <summary>
        /// Adds the specified object instance at the specified address.
        /// This method can only be used during deserialization.
        /// </summary>
        /// <param name="address">Address to store the object instance at.</param>
        /// <param name="item">Object instance to store at the specified address.</param>
        public void Add(Json.Expression address, object item)
        {
            _deserializedObjects[AddressFromJson(address)] = item;
        }

        #endregion

        #region Address helpers

        /// <summary>
        /// Converts an address to a JSON representation.
        /// </summary>
        /// <param name="address">Address to get a JSON representation for.</param>
        /// <returns>JSON representation of the specified address.</returns>
        private static Json.Expression AddressToJson(int address)
        {
            return Json.Expression.Number(address.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets an address from a JSON representation.
        /// </summary>
        /// <param name="address">JSON representation of an address.</param>
        /// <returns>Address in the specified JSON representation.</returns>
        private static int AddressFromJson(Json.Expression address)
        {
            return int.Parse((string)((Json.ConstantExpression)address).Value, CultureInfo.InvariantCulture);
        }

        #endregion

        #region Conversion to/from JSON

        /// <summary>
        /// Gets the JSON representation of the heap.
        /// </summary>
        /// <returns>JSON representation of the heap.</returns>
        public Json.Expression ToJson()
        {
            return Json.Expression.Array(from obj in _objects
                                         orderby obj.Key
                                         select obj.Value);
        }

        /// <summary>
        /// Restores a heap, used for deserialization, from the given JSON representation.
        /// </summary>
        /// <param name="expression">JSON representation of a heap.</param>
        /// <returns>Heap used for deserialization.</returns>
        public static Heap FromJson(Json.Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var objects = (Json.ArrayExpression)expression;

            var res = new Heap(false);

            var address = ADDRESS_FIRST;
            foreach (var obj in objects.Elements)
                res._objects[address++] = obj;

            return res;
        }

        #endregion

        #endregion
    }
}
