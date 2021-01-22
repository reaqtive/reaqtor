// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtive.TestingFramework.Mocks
{
    /// <summary>
    /// Mock object to represent state reader/writer.
    /// </summary>
    public sealed class MockOperatorStateContainer : IOperatorStateContainer
    {
        private readonly List<object> _values;
        private int _readPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockOperatorStateContainer"/> class.
        /// </summary>
        public MockOperatorStateContainer()
        {
            _values = new List<object>();
        }

        /// <summary>
        /// Creates a new reader of the current state.
        /// </summary>
        public IOperatorStateReaderFactory CreateReader() => new Reader(this);

        /// <summary>
        /// Creates a new writer. The state is reset.
        /// </summary>
        public IOperatorStateWriterFactory CreateWriter() => new Writer(this);

        private sealed class Reader : IOperatorStateReaderFactory, IOperatorStateReader
        {
            private readonly MockOperatorStateContainer _parent;
            private readonly int _initialPosition;

            public Reader(MockOperatorStateContainer parent)
            {
                _parent = parent;
                _initialPosition = _parent._readPosition;
            }

            public IOperatorStateReader Create(IStatefulOperator o)
            {
                return this;
            }

            public T Read<T>()
            {
                return (T)_parent._values[_parent._readPosition++];
            }

            public bool TryRead<T>(out T value)
            {
                if (_parent._readPosition < _parent._values.Count)
                {
                    value = Read<T>();
                    return true;
                }

                value = default;
                return false;
            }

            public void Reset()
            {
                _parent._readPosition = _initialPosition;
            }

            public IOperatorStateReader CreateChild()
            {
                return new Reader(_parent);
            }

            public void Dispose()
            {
            }
        }

        private sealed class Writer : IOperatorStateWriterFactory, IOperatorStateWriter
        {
            private readonly MockOperatorStateContainer _parent;

            public Writer(MockOperatorStateContainer parent)
            {
                _parent = parent;
                _parent._values.Clear();
            }

            public IOperatorStateWriter Create(IStatefulOperator o)
            {
                return this;
            }

            public void Write<T>(T value)
            {
                _parent._values.Add(value);
            }

            public IOperatorStateWriter CreateChild()
            {
                return this;
            }

            public void Dispose()
            {
            }
        }
    }
}
