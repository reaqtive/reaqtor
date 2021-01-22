// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

/*
 * Example of a simple continuation passing style (CPS) transformation, in order to rewrite synchronous
 * query execution plan expression trees into asynchronous variants, without any significant changes to
 * existing rewriters that assume the synchronous model.
 * 
 * This file contains the custom attribute used to annotate synchronous methods that have an asynchronous
 * variant which will be discovered and used as a rewrite target by the CPS rewriter that's being used.
 * 
 * BD - 5/8/2013
 */

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Attribute used to indicate methods that should be rewritten to asynchronous variants, for use by any of the CPS rewriter classes.
    /// Each CPS rewriter class can define its own mapping between the method the attribute is applied to and its asynchronous variant.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UseAsyncMethodAttribute : Attribute
    {
    }
}
