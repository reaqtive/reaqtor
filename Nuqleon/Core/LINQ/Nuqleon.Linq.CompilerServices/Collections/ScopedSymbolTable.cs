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

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a scoped symbol table with standard lexical scoping rules, allowing for lookup of the closest declaring scope of a symbol.
    /// </summary>
    /// <typeparam name="TSymbol">Type of the symbols stored in the table.</typeparam>
    /// <typeparam name="TValue">Type of the values associated with the symbols stored in the table.</typeparam>
    public class ScopedSymbolTable<TSymbol, TValue> : IEnumerable<Indexed<SymbolTable<TSymbol, TValue>>>
    {
        private readonly Stack<SymbolTable<TSymbol, TValue>> _environment;

        /// <summary>
        /// Creates a new empty scoped symbol table with an empty initial global scope.
        /// </summary>
        public ScopedSymbolTable()
            : this(EqualityComparer<TSymbol>.Default)
        {
        }

        /// <summary>
        /// Creates a new empty scoped symbol table with an empty initial global scope, using the specified equality comparer for symbols.
        /// </summary>
        /// <param name="symbolComparer">Equality comparer to compare symbols. A scoped symbol table can only contain distinct symbols within each level. Symbols can shadow declarations in the enclosing scope.</param>
        public ScopedSymbolTable(IEqualityComparer<TSymbol> symbolComparer)
        {
            GlobalScope = new SymbolTable<TSymbol, TValue>(parent: null, symbolComparer ?? throw new ArgumentNullException(nameof(symbolComparer)));
            _environment = new Stack<SymbolTable<TSymbol, TValue>>();
        }

        /// <summary>
        /// Gets the global scope associated with the scoped symbol table.
        /// </summary>
        public SymbolTable<TSymbol, TValue> GlobalScope { get; }

        /// <summary>
        /// Gets the current scope in the scoped symbol table.
        /// This may return the global scope if no other scopes were created.
        /// </summary>
        public SymbolTable<TSymbol, TValue> CurrentScope => _environment.Count == 0 ? GlobalScope : _environment.Peek();

        /// <summary>
        /// Pushes a new scope onto the scoped symbol table.
        /// </summary>
        public void Push() => _environment.Push(new SymbolTable<TSymbol, TValue>(CurrentScope, GlobalScope.Comparer));

        /// <summary>
        /// Pops the most recent scope from the scoped symbol table.
        /// </summary>
        public void Pop() => _environment.Pop();

        /// <summary>
        /// Adds an entry to the current scope in the scoped symbol table.
        /// </summary>
        /// <param name="symbol">Symbol to add to the current scope in the table.</param>
        /// <param name="value">Value to associate to the symbol.</param>
        /// <returns>Indexed entry in the current scope in the scoped symbol table.</returns>
        public Indexed<KeyValuePair<TSymbol, TValue>> Add(TSymbol symbol, TValue value) => CurrentScope.Add(symbol, value);

        /// <summary>
        /// Looks up the specified symbol by scanning the scoped symbol table, starting from the current scope.
        /// </summary>
        /// <param name="symbol">Symbol to look up in the scope symbol table.</param>
        /// <param name="value">Indexed value matching the specified symbol. The outer index represents the scope relative to the current scope, with a value of -1 representing the global scope. The inner index represents the index in the declaring symbol table.</param>
        /// <returns>true if the symbol was found; otherwise, false.</returns>
        public bool TryLookup(TSymbol symbol, out Indexed<Indexed<TValue>> value)
        {
            var level = 0;

            Indexed<TValue> res;

            foreach (var table in _environment)
            {
                if (table.TryGetValue(symbol, out res))
                {
                    value = new Indexed<Indexed<TValue>>(res, level);
                    return true;
                }

                level++;
            }

            if (GlobalScope.TryGetValue(symbol, out res))
            {
                value = new Indexed<Indexed<TValue>>(res, -1);
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Gets an enumerator to enumerate the symbol tables in the scoped symbol table, starting from the current scope. The last element with index -1 represents the global scope.
        /// </summary>
        /// <returns>Enumerator over the scoped symbol table entries.</returns>
        public IEnumerator<Indexed<SymbolTable<TSymbol, TValue>>> GetEnumerator()
        {
            var level = 0;
            foreach (var table in _environment)
            {
                yield return new Indexed<SymbolTable<TSymbol, TValue>>(table, level++);
            }

            yield return new Indexed<SymbolTable<TSymbol, TValue>>(GlobalScope, -1);
        }

        /// <summary>
        /// Gets an enumerator to enumerate the symbol tables in the scoped symbol table, starting from the current scope. The last element with index -1 represents the global scope.
        /// </summary>
        /// <returns>Enumerator over the scoped symbol table entries.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
