// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
    /// <summary>
    /// Defines various flags used in the protocol for serialization.
    /// </summary>
    internal static class Protocol
    {
        /// <summary>
        /// Indicates that the unary expression node has a method, e.g. due to use of operator overloading.
        /// </summary>
        public const byte UNARY_FLAG_HASMETHOD = 0x01;

        /// <summary>
        /// Indicates that the binary expression node has a method, e.g. due to use of operator overloading.
        /// </summary>
        public const byte BINARY_FLAG_HASMETHOD = 0x01;

        /// <summary>
        /// Indicates that the binary exprssion node has a conversion, e.g. for coalesce operations or compound assignments.
        /// </summary>
        public const byte BINARY_FLAG_HASCONVERSION = 0x02;

        /// <summary>
        /// Indicates that the binary expression node has a result that's lifted, e.g. for nullable relational operators.
        /// </summary>
        public const byte BINARY_FLAG_ISLIFTED = 0x04;

        /// <summary>
        /// Indicates that the catch block has a variable containing the caught exception.
        /// </summary>
        public const byte CATCHBLOCK_FLAG_HASVARIABLE = 0x01;

        /// <summary>
        /// Indicates that the catch block has a filter that needs to be evaluated on candidate exceptions.
        /// </summary>
        public const byte CATCHBLOCK_FLAG_HASFILTER = 0x02;

        /// <summary>
        /// Indicates that the new expression node has no constructor, e.g. for instantiation of structs.
        /// </summary>
        public const byte NEW_NOCTOR = 0x01;

        /// <summary>
        /// Indicates that the new expression node has a members collection, e.g. for anonymous types.
        /// </summary>
        public const byte NEW_HASMEMBERS = 0x02;

        /// <summary>
        /// Indicates that the switch expression has a user-specified type, rather than the result type getting inferred from the first case.
        /// </summary>
        public const byte SWITCH_HASTYPE = 0x01;

        /// <summary>
        /// Indicates that the switch expression has a comparison method used to check for equality between the input expressions and the cases.
        /// </summary>
        public const byte SWITCH_HASCOMPARISON = 0x02;

        /// <summary>
        /// Indicates that the switch expression has a default case.
        /// </summary>
        public const byte SWITCH_HASDEFAULT = 0x04;

        /// <summary>
        /// Indicates that the try expression has a user-specified type, rather than the result type getting inferred from the body expression.
        /// </summary>
        public const byte TRY_HASTYPE = 0x01;

        /// <summary>
        /// Indicates that the try expression has a catch block.
        /// </summary>
        public const byte TRY_HASCATCH = 0x02;

        /// <summary>
        /// Indicates that the try expression has a finally block.
        /// </summary>
        public const byte TRY_HASFINALLY = 0x04;

        /// <summary>
        /// Indicates that the try expression has a fault block.
        /// </summary>
        public const byte TRY_HASFAULT = 0x08;

        /// <summary>
        /// Indicates that the label has a default value.
        /// </summary>
        public const byte LABEL_HASDEFAULTVALUE = 0x01;

        /// <summary>
        /// Indicates that the goto expression carries value, e.g. for return statements.
        /// </summary>
        public const byte GOTO_HASVALUE = 0x01;

        /// <summary>
        /// Indicates that the block expression has a user-specified type, rather than the result type getting inferred from the last expression.
        /// </summary>
        public const byte BLOCK_HASTYPE = 0x01;

#if USE_SLIM
        /// <summary>
        /// Indicates that the conditional expression has a user-specified type, rather than the result type getting inferred from its children.
        /// </summary>
        public const byte CONDITIONAL_HASTYPE = 0x01;
#endif

        /// <summary>
        /// Indicates that the lambda expression has a user-specified delegate type.
        /// </summary>
        public const byte LAMBDA_HASTYPE = 0x01;

        /// <summary>
        /// Indicates that the lambda expression has a name.
        /// </summary>
        public const byte LAMBDA_HASNAME = 0x02;

        /// <summary>
        /// Indicates that the lambda expression should be compiled using tail call optimization.
        /// </summary>
        public const byte LAMBDA_ISTAILCALL = 0x04;

        /// <summary>
        /// Indicates that the loop expression has a break label.
        /// </summary>
        public const byte LOOP_HASBREAK = 0x01;

        /// <summary>
        /// Indicates that the loop expression has a continue label.
        /// </summary>
        public const byte LOOP_HASCONTINUE = 0x02;

        /// <summary>
        /// Indicates that the type is a simple type, i.e. a type that is not constructed from other types.
        /// </summary>
        public const byte TYPE_SIMPLE = 0x01;

        /// <summary>
        /// Indicates that the type is a multi-dimensional array type.
        /// </summary>
        public const byte TYPE_ARRAY = 0x02;

        /// <summary>
        /// Indicates that the type is an open generic type.
        /// </summary>
        public const byte TYPE_GENDEF = 0x03;

        /// <summary>
        /// Indicates that the type is a closed generic type.
        /// </summary>
        public const byte TYPE_GENTYPE = 0x04;

        /// <summary>
        /// Indicates that the type is a generic parameter type.
        /// </summary>
        public const byte TYPE_GENPAR = 0x05;

        /// <summary>
        /// Indicates that the type is a by-ref type.
        /// </summary>
        public const byte TYPE_BYREF = 0x06;

        /// <summary>
        /// Indicates that the type is a single-dimensional array type, i.e. a vector.
        /// </summary>
        public const byte TYPE_VECTOR = 0x07;

        /// <summary>
        /// Indicates that the property represents an indexer, i.e. it has parameters.
        /// </summary>
        public const byte PROPERTY_INDEXER = 0x01;

        /// <summary>
        /// Indicates that the type is represented by an index in the type table.
        /// </summary>
        public const byte TYPE_FLAG_INDEXED = 0x80;

        /// <summary>
        /// Maximum value for a short index that can be represented compactably.
        /// </summary>
        public const byte TYPE_SHORT_INDEX_MAX = 0x80;

        /// <summary>
        /// Bitmask to obtain the short index of a type that's represented by an index in the type table.
        /// </summary>
        public const byte TYPE_SHORT_INDEX_MASK = 0x7F;

        /// <summary>
        /// Indicates that the type is represented using a long index in the type table.
        /// </summary>
        public const uint TYPE_LONG_INDEX_FLAG = 0xC0000000;

        /// <summary>
        /// Bitmask to obtain the long index of a type that's represented by an index in the type table.
        /// </summary>
        public const uint TYPE_LONG_INDEX_MASK = 0x3FFFFFFF;

        /// <summary>
        /// Indicates that the type is a single-dimensional array.
        /// </summary>
        public const byte TYPE_FLAG_ARRAY = 0x40;

        /// <summary>
        /// Indicates that the type is nullable.
        /// </summary>
        public const byte TYPE_FLAG_NULLABLE = 0x20;

        /// <summary>
        /// Bitmask to obtain the known type value.
        /// </summary>
        public const byte TYPE_KNOWN_MASK = 0x1F;

        /// <summary>
        /// Representation of the System.Object type.
        /// </summary>
        public const byte TYPE_OBJECT = 0x00;

        /// <summary>
        /// Representation of the System.Boolean type.
        /// </summary>
        public const byte TYPE_BOOLEAN = 0x01;

        /// <summary>
        /// Representation of the System.Byte type.
        /// </summary>
        public const byte TYPE_UINT8 = 0x02;

        /// <summary>
        /// Representation of the System.SByte type.
        /// </summary>
        public const byte TYPE_INT8 = 0x03;

        /// <summary>
        /// Representation of the System.UInt16 type.
        /// </summary>
        public const byte TYPE_UINT16 = 0x04;

        /// <summary>
        /// Representation of the System.Int16 type.
        /// </summary>
        public const byte TYPE_INT16 = 0x05;

        /// <summary>
        /// Representation of the System.UInt32 type.
        /// </summary>
        public const byte TYPE_UINT32 = 0x06;

        /// <summary>
        /// Representation of the System.Int32 type.
        /// </summary>
        public const byte TYPE_INT32 = 0x07;

        /// <summary>
        /// Representation of the System.UInt64 type.
        /// </summary>
        public const byte TYPE_UINT64 = 0x08;

        /// <summary>
        /// Representation of the System.Int64 type.
        /// </summary>
        public const byte TYPE_INT64 = 0x09;

        /// <summary>
        /// Representation of the System.Single type.
        /// </summary>
        public const byte TYPE_FLOAT4 = 0x0A;

        /// <summary>
        /// Representation of the System.Double type.
        /// </summary>
        public const byte TYPE_FLOAT8 = 0x0B;

        /// <summary>
        /// Representation of the System.Char type.
        /// </summary>
        public const byte TYPE_CHAR = 0x0C;

        /// <summary>
        /// Representation of the System.String type.
        /// </summary>
        public const byte TYPE_STRING = 0x0D;

        /// <summary>
        /// Representation of the System.DateTime type.
        /// </summary>
        public const byte TYPE_DATETIME = 0x0E;

        /// <summary>
        /// Representation of the System.Decimal type.
        /// </summary>
        public const byte TYPE_DECIMAL = 0x0F;

        /// <summary>
        /// Representation of the System.Void type.
        /// </summary>
        public const byte TYPE_VOID = 0x10;

        /// <summary>
        /// Representation of the System.Guid type.
        /// </summary>
        public const byte TYPE_GUID = 0x11;

        /// <summary>
        /// Representation of the System.DateTimeOffset type.
        /// </summary>
        public const byte TYPE_DATETIMEOFFSET = 0x12;

        /// <summary>
        /// Representation of the System.TimeSpan type.
        /// </summary>
        public const byte TYPE_TIMESPAN = 0x13;

        /// <summary>
        /// Representation of the System.Uri type.
        /// </summary>
        public const byte TYPE_URI = 0x14;

#if USE_SLIM
        /// <summary>
        /// Indicates that the method call expression node has an object (i.e. non-static).
        /// </summary>
        public const byte CALL_HASOBJ = 0x01;

        /// <summary>
        /// Indicates that the member expression node has an object (i.e. non-static).
        /// </summary>
        public const byte MEMBER_HASOBJ = 0x01;
#endif
    }
}
