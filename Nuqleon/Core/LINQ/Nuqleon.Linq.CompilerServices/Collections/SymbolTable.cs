// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a symbol table that associates symbols to values.
    /// </summary>
    /// <typeparam name="TSymbol">Type of the symbols stored in the table.</typeparam>
    /// <typeparam name="TValue">Type of the values associated with the symbols stored in the table.</typeparam>
    public class SymbolTable<TSymbol, TValue> : IEnumerable<Indexed<KeyValuePair<TSymbol, TValue>>>
    {
        private readonly Dictionary<TSymbol, Indexed<TValue>> _table;
        private readonly List<KeyValuePair<TSymbol, TValue>> _ordered;

        /// <summary>
        /// Creates a new empty symbol table with the specified (optional) parent symbol table and using the default equality comparer for symbols.
        /// </summary>
        /// <param name="parent">Parent symbol table (optional).</param>
        public SymbolTable(SymbolTable<TSymbol, TValue> parent)
            : this(parent, EqualityComparer<TSymbol>.Default)
        {
        }

        /// <summary>
        /// Creates a new empty symbol table with the specified (optional) parent symbol table and using the specified equality comparer for symbols.
        /// </summary>
        /// <param name="parent">Parent symbol type (optional).</param>
        /// <param name="symbolComparer">Equality comparer to compare symbols. A symbol table can only contain distinct symbols.</param>
        public SymbolTable(SymbolTable<TSymbol, TValue> parent, IEqualityComparer<TSymbol> symbolComparer)
        {
            if (symbolComparer == null)
                throw new ArgumentNullException(nameof(symbolComparer));

            Parent = parent;
            _table = new Dictionary<TSymbol, Indexed<TValue>>(symbolComparer);
            _ordered = new List<KeyValuePair<TSymbol, TValue>>();
        }

        /// <summary>
        /// Gets the parent symbol table. Returns null if no parent table was specified during creation of the symbol table.
        /// </summary>
        public SymbolTable<TSymbol, TValue> Parent { get; }

        /// <summary>
        /// Gets the symbols defined in the symbol table, in order of definition.
        /// </summary>
        public IEnumerable<TSymbol> Symbols => _ordered.Select(kv => kv.Key).ToArray();

        /// <summary>
        /// Gets the value associated with the specified symbol.
        /// </summary>
        /// <param name="symbol">Symbol to look up in the symbol table.</param>
        /// <returns>Value associated with the specified symbol.</returns>
        public Indexed<TValue> this[TSymbol symbol] => _table[symbol];

        /// <summary>
        /// Adds an entry to the symbol table.
        /// </summary>
        /// <param name="symbol">Symbol to add to the table.</param>
        /// <param name="value">Value to associate to the symbol.</param>
        /// <returns>Indexed entry in the symbol table.</returns>
        public Indexed<KeyValuePair<TSymbol, TValue>> Add(TSymbol symbol, TValue value)
        {
            if (_table.ContainsKey(symbol))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Symbol table already contains symbol '{0}'.", symbol));
            }

            var res = new Indexed<TValue>(value, _table.Count);
            _table[symbol] = res;

            var entry = new KeyValuePair<TSymbol, TValue>(symbol, value);
            _ordered.Add(entry);

            return new Indexed<KeyValuePair<TSymbol, TValue>>(entry, res.Index);
        }

        /// <summary>
        /// Gets the symbol table entry associated with the specified symbol.
        /// </summary>
        /// <param name="symbol">Symbol to look up in the symbol table.</param>
        /// <param name="value">Indexed value matching the specified symbol.</param>
        /// <returns>true if the symbol was found; otherwise, false.</returns>
        public bool TryGetValue(TSymbol symbol, out Indexed<TValue> value)
        {
            if (_table.TryGetValue(symbol, out Indexed<TValue> state))
            {
                value = state;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Looks up the specified symbol by following the symbol table's parent chain.
        /// </summary>
        /// <param name="symbol">Symbol to look up in the symbol table and its parents.</param>
        /// <param name="value">Indexed value matching the specified symbol. The outer index represents the scope relative to the current table. The inner index represents the index in the declaring symbol table.</param>
        /// <returns>true if the symbol was found; otherwise, false.</returns>
        public bool TryLookup(TSymbol symbol, out Indexed<Indexed<TValue>> value)
        {
            var level = 0;

            var table = this;
            while (table != null)
            {
                if (table.TryGetValue(symbol, out Indexed<TValue> res))
                {
                    value = new Indexed<Indexed<TValue>>(res, level);
                    return true;
                }

                table = table.Parent;
                level++;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Gets an enumerator to enumerate the entries in the symbol table, in order of their index value.
        /// </summary>
        /// <returns>Enumerator over the symbol table entries.</returns>
        public IEnumerator<Indexed<KeyValuePair<TSymbol, TValue>>> GetEnumerator()
        {
            var i = 0;
            foreach (var entry in _ordered)
            {
                yield return new Indexed<KeyValuePair<TSymbol, TValue>>(entry, i++);
            }
        }

        /// <summary>
        /// Gets an enumerator to enumerate the entries in the symbol table, in order of their index value.
        /// </summary>
        /// <returns>Enumerator over the symbol table entries.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
