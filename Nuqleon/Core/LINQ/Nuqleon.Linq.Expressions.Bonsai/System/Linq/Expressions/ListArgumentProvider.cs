// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Expressions
{
    internal sealed class ListArgumentProviderSlim : IList<ExpressionSlim>
    {
        private readonly IArgumentProviderSlim _provider;
        private readonly ExpressionSlim _arg0;

        public ListArgumentProviderSlim(IArgumentProviderSlim provider, ExpressionSlim arg0)
        {
            _provider = provider;
            _arg0 = arg0;
        }

        public ExpressionSlim this[int index]
        {
            get => index == 0 ? _arg0 : _provider.GetArgument(index);
            set => throw new NotSupportedException();
        }

        public int Count => _provider.ArgumentCount;

        public bool IsReadOnly => true;

        public bool Contains(ExpressionSlim item) => IndexOf(item) != -1;

        public int IndexOf(ExpressionSlim item)
        {
            if (item == _arg0)
            {
                return 0;
            }

            for (int i = 1, n = _provider.ArgumentCount; i < n; i++)
            {
                if (item == _provider.GetArgument(i))
                {
                    return i;
                }
            }

            return -1;
        }

        public void CopyTo(ExpressionSlim[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            array[arrayIndex++] = _arg0;

            for (int i = 1, n = _provider.ArgumentCount; i < n; i++)
            {
                array[arrayIndex++] = _provider.GetArgument(i);
            }
        }

        public IEnumerator<ExpressionSlim> GetEnumerator()
        {
            yield return _arg0;

            for (int i = 1, n = _provider.ArgumentCount; i < n; i++)
            {
                yield return _provider.GetArgument(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(ExpressionSlim item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, ExpressionSlim item)
        {
            throw new NotSupportedException();
        }

        public bool Remove(ExpressionSlim item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
    }
}
