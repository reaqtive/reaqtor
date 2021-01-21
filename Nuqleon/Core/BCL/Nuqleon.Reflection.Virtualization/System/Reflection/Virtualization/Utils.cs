// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System
{
    internal static class Utils
    {
        public static bool SequenceEqual(Type[] left, Type[] right)
        {
            if (left == right)
            {
                return true;
            }

            var count = left.Length;

            if (right.Length != count)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static int GetHashCode<TDefinition>(TDefinition def, Type[] args)
        {
#if NET5_0
            HashCode h = new();

            h.Add(def);

            foreach (var arg in args)
            {
                h.Add(arg);
            }

            return h.ToHashCode();
#else
            var res = def.GetHashCode();

            foreach (var arg in args)
            {
                res = res * 17 + arg.GetHashCode();
            }

            return res;
#endif
        }
    }
}
