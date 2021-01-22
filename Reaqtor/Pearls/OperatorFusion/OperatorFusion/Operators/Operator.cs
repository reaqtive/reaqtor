// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Exposes query operators in a fluent interface pattern as a stepping stone
// towards an approach based on proper binding.
//
// BD - October 2014
//

using System;

namespace OperatorFusion
{
    internal abstract class Operator
    {
        public Operator[] Inputs;
        public IFusionOperator Factory;

        public abstract Type Type { get; }
    }

    internal class Operator<R> : Operator
    {
        public override Type Type => typeof(R);
    }
}
