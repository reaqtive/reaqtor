// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal static class Protocol
    {
        /// <summary>
        /// Representation of the System.Object type.
        /// </summary>
        public const byte TYPE_UNIT = 0x01;

        /// <summary>
        /// Representation of the System.Boolean type.
        /// </summary>
        public const byte TYPE_BOOLEAN = 0x02;

        /// <summary>
        /// Representation of the System.Byte type.
        /// </summary>
        public const byte TYPE_BYTE = 0x03;

        /// <summary>
        /// Representation of the System.SByte type.
        /// </summary>
        public const byte TYPE_SBYTE = 0x04;

        /// <summary>
        /// Representation of the System.UInt16 type.
        /// </summary>
        public const byte TYPE_UINT16 = 0x05;

        /// <summary>
        /// Representation of the System.Int16 type.
        /// </summary>
        public const byte TYPE_INT16 = 0x06;

        /// <summary>
        /// Representation of the System.UInt32 type.
        /// </summary>
        public const byte TYPE_UINT32 = 0x07;

        /// <summary>
        /// Representation of the System.Int32 type.
        /// </summary>
        public const byte TYPE_INT32 = 0x08;

        /// <summary>
        /// Representation of the System.UInt64 type.
        /// </summary>
        public const byte TYPE_UINT64 = 0x09;

        /// <summary>
        /// Representation of the System.Int64 type.
        /// </summary>
        public const byte TYPE_INT64 = 0x0A;

        /// <summary>
        /// Representation of the System.Single type.
        /// </summary>
        public const byte TYPE_SINGLE = 0x0B;

        /// <summary>
        /// Representation of the System.Double type.
        /// </summary>
        public const byte TYPE_DOUBLE = 0x0C;

        /// <summary>
        /// Representation of the System.Char type.
        /// </summary>
        public const byte TYPE_CHAR = 0x0D;

        /// <summary>
        /// Representation of the System.String type.
        /// </summary>
        public const byte TYPE_STRING = 0x0E;

        /// <summary>
        /// Representation of the System.DateTime type.
        /// </summary>
        public const byte TYPE_DATETIME = 0x0F;

        /// <summary>
        /// Representation of the System.Decimal type.
        /// </summary>
        public const byte TYPE_DECIMAL = 0x10;

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

        /// <summary>
        /// Flag signaling that the nullable value is null.
        /// </summary>
        public const byte TYPE_FLAG_NULLVALUE = 0x80; //1000 0000

        /// <summary>
        /// Flag signaling that the type is primitive.
        /// </summary>
        public const byte TYPE_FLAG_NONPRIMITIVE = 0x40; //0100 0000

        /// <summary>
        /// Flag signaling that the type is nullable.
        /// </summary>
        public const byte TYPE_FLAG_ISNULLABLE = 0x20; //0010 0000

        /// <summary>
        /// Representation of an array type.
        /// </summary>
        public const byte TYPE_ARRAY = 0x70; //0111 0000

        /// <summary>
        /// Representation of a structural type.
        /// </summary>
        public const byte TYPE_STRUCTURAL = 0x60; //0110 0000

        /// <summary>
        /// Representation of a System.Linq.Expressions.Expression type.
        /// </summary>
        public const byte TYPE_EXPRESSION = 0x50; //0101 0000
    }
}
