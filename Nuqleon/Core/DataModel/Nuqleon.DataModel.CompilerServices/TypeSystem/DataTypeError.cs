// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Nuqleon.DataModel.TypeSystem;

/// <summary>
/// Provides information about data type errors, when converting from CLR types.
/// </summary>
public class DataTypeError
{
    internal DataTypeError(Type type, string message, Type[] stack)
    {
        Type = type;
        Message = message;

        Stack = stack.ToList().AsReadOnly();
    }

    /// <summary>
    /// Gets the CLR type that is not compliant with the data model.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the error message explaining why the CLR type is not compliant with the data model.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the stack of types at the point the error was raised. This information can help to understand how the composition of an invalid data model type resulted in an error for another type.
    /// </summary>
    public ReadOnlyCollection<Type> Stack { get; }

    /// <summary>
    /// Returns an exception to signal the error.
    /// </summary>
    /// <returns>Exception representing the error.</returns>
    public Exception ToException() => new DataTypeException(this);

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    /// <returns>String representation of the error.</returns>
    public override string ToString()
    {
        using var psb = PooledStringBuilder.New();

        var sb = psb.StringBuilder;

        sb.Append(Message);

        sb.AppendLine("   " + Type.ToCSharpString());

        foreach (var type in Stack)
        {
            sb.AppendLine("in " + type.ToCSharpString());
        }

        return sb.ToString();
    }
}
