// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nuqleon.Json.Serialization;
using System;
using System.Numerics;

namespace Tests
{
    partial class ParserTests
    {
        [TestMethod]
        public void FastParser_SByte()
        {
            AssertParse(Parser.ParseSByte, SByte.MinValue.ToString(), (SByte)SByte.MinValue);
            AssertParse(Parser.ParseSByte, SByte.MaxValue.ToString(), (SByte)SByte.MaxValue);
            AssertParse(Parser.ParseSByte, ((SByte)(SByte.MinValue + 1)).ToString(), (SByte)(SByte.MinValue + 1));
            AssertParse(Parser.ParseSByte, ((SByte)(SByte.MaxValue - 1)).ToString(), (SByte)(SByte.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseSByte, i.ToString(), (SByte)i);
            }

            AssertParseFail(Parser.ParseSByte, "-");

            AssertParseFail(Parser.ParseSByte, "");
            AssertParseFail(Parser.ParseSByte, " ");

            AssertParseFail(Parser.ParseSByte, "true");
            AssertParseFail(Parser.ParseSByte, "[");
            AssertParseFail(Parser.ParseSByte, "]");
            AssertParseFail(Parser.ParseSByte, "{");
            AssertParseFail(Parser.ParseSByte, "}");
            AssertParseFail(Parser.ParseSByte, ",");
            AssertParseFail(Parser.ParseSByte, ":");

            AssertParseFail(Parser.ParseSByte, "null");

            AssertParseFail(Parser.ParseSByte, (new BigInteger(SByte.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseSByte, (new BigInteger(SByte.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableSByte()
        {
            AssertParse(Parser.ParseNullableSByte, "null", (SByte?)null);
            AssertParse(Parser.ParseNullableSByte, SByte.MinValue.ToString(), (SByte?)SByte.MinValue);
            AssertParse(Parser.ParseNullableSByte, SByte.MaxValue.ToString(), (SByte?)SByte.MaxValue);
            AssertParse(Parser.ParseNullableSByte, ((SByte)(SByte.MinValue + 1)).ToString(), (SByte?)(SByte.MinValue + 1));
            AssertParse(Parser.ParseNullableSByte, ((SByte)(SByte.MaxValue - 1)).ToString(), (SByte?)(SByte.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseNullableSByte, i.ToString(), (SByte?)i);
            }

            AssertParseFail(Parser.ParseNullableSByte, "-");

            AssertParseFail(Parser.ParseNullableSByte, "");
            AssertParseFail(Parser.ParseNullableSByte, " ");

            AssertParseFail(Parser.ParseNullableSByte, "true");
            AssertParseFail(Parser.ParseNullableSByte, "[");
            AssertParseFail(Parser.ParseNullableSByte, "]");
            AssertParseFail(Parser.ParseNullableSByte, "{");
            AssertParseFail(Parser.ParseNullableSByte, "}");
            AssertParseFail(Parser.ParseNullableSByte, ",");
            AssertParseFail(Parser.ParseNullableSByte, ":");

            AssertParseFail(Parser.ParseNullableSByte, "n");
            AssertParseFail(Parser.ParseNullableSByte, "nu");
            AssertParseFail(Parser.ParseNullableSByte, "nul");

            AssertParseFail(Parser.ParseNullableSByte, "Null");
            AssertParseFail(Parser.ParseNullableSByte, "nUll");
            AssertParseFail(Parser.ParseNullableSByte, "nuLl");
            AssertParseFail(Parser.ParseNullableSByte, "nulL");

            AssertParseFail(Parser.ParseNullableSByte, (new BigInteger(SByte.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableSByte, (new BigInteger(SByte.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_Int16()
        {
            AssertParse(Parser.ParseInt16, Int16.MinValue.ToString(), (Int16)Int16.MinValue);
            AssertParse(Parser.ParseInt16, Int16.MaxValue.ToString(), (Int16)Int16.MaxValue);
            AssertParse(Parser.ParseInt16, ((Int16)(Int16.MinValue + 1)).ToString(), (Int16)(Int16.MinValue + 1));
            AssertParse(Parser.ParseInt16, ((Int16)(Int16.MaxValue - 1)).ToString(), (Int16)(Int16.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseInt16, i.ToString(), (Int16)i);
            }

            AssertParseFail(Parser.ParseInt16, "-");

            AssertParseFail(Parser.ParseInt16, "");
            AssertParseFail(Parser.ParseInt16, " ");

            AssertParseFail(Parser.ParseInt16, "true");
            AssertParseFail(Parser.ParseInt16, "[");
            AssertParseFail(Parser.ParseInt16, "]");
            AssertParseFail(Parser.ParseInt16, "{");
            AssertParseFail(Parser.ParseInt16, "}");
            AssertParseFail(Parser.ParseInt16, ",");
            AssertParseFail(Parser.ParseInt16, ":");

            AssertParseFail(Parser.ParseInt16, "null");

            AssertParseFail(Parser.ParseInt16, (new BigInteger(Int16.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseInt16, (new BigInteger(Int16.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableInt16()
        {
            AssertParse(Parser.ParseNullableInt16, "null", (Int16?)null);
            AssertParse(Parser.ParseNullableInt16, Int16.MinValue.ToString(), (Int16?)Int16.MinValue);
            AssertParse(Parser.ParseNullableInt16, Int16.MaxValue.ToString(), (Int16?)Int16.MaxValue);
            AssertParse(Parser.ParseNullableInt16, ((Int16)(Int16.MinValue + 1)).ToString(), (Int16?)(Int16.MinValue + 1));
            AssertParse(Parser.ParseNullableInt16, ((Int16)(Int16.MaxValue - 1)).ToString(), (Int16?)(Int16.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseNullableInt16, i.ToString(), (Int16?)i);
            }

            AssertParseFail(Parser.ParseNullableInt16, "-");

            AssertParseFail(Parser.ParseNullableInt16, "");
            AssertParseFail(Parser.ParseNullableInt16, " ");

            AssertParseFail(Parser.ParseNullableInt16, "true");
            AssertParseFail(Parser.ParseNullableInt16, "[");
            AssertParseFail(Parser.ParseNullableInt16, "]");
            AssertParseFail(Parser.ParseNullableInt16, "{");
            AssertParseFail(Parser.ParseNullableInt16, "}");
            AssertParseFail(Parser.ParseNullableInt16, ",");
            AssertParseFail(Parser.ParseNullableInt16, ":");

            AssertParseFail(Parser.ParseNullableInt16, "n");
            AssertParseFail(Parser.ParseNullableInt16, "nu");
            AssertParseFail(Parser.ParseNullableInt16, "nul");

            AssertParseFail(Parser.ParseNullableInt16, "Null");
            AssertParseFail(Parser.ParseNullableInt16, "nUll");
            AssertParseFail(Parser.ParseNullableInt16, "nuLl");
            AssertParseFail(Parser.ParseNullableInt16, "nulL");

            AssertParseFail(Parser.ParseNullableInt16, (new BigInteger(Int16.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableInt16, (new BigInteger(Int16.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_Int32()
        {
            AssertParse(Parser.ParseInt32, Int32.MinValue.ToString(), (Int32)Int32.MinValue);
            AssertParse(Parser.ParseInt32, Int32.MaxValue.ToString(), (Int32)Int32.MaxValue);
            AssertParse(Parser.ParseInt32, ((Int32)(Int32.MinValue + 1)).ToString(), (Int32)(Int32.MinValue + 1));
            AssertParse(Parser.ParseInt32, ((Int32)(Int32.MaxValue - 1)).ToString(), (Int32)(Int32.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseInt32, i.ToString(), (Int32)i);
            }

            AssertParseFail(Parser.ParseInt32, "-");

            AssertParseFail(Parser.ParseInt32, "");
            AssertParseFail(Parser.ParseInt32, " ");

            AssertParseFail(Parser.ParseInt32, "true");
            AssertParseFail(Parser.ParseInt32, "[");
            AssertParseFail(Parser.ParseInt32, "]");
            AssertParseFail(Parser.ParseInt32, "{");
            AssertParseFail(Parser.ParseInt32, "}");
            AssertParseFail(Parser.ParseInt32, ",");
            AssertParseFail(Parser.ParseInt32, ":");

            AssertParseFail(Parser.ParseInt32, "null");

            AssertParseFail(Parser.ParseInt32, (new BigInteger(Int32.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseInt32, (new BigInteger(Int32.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableInt32()
        {
            AssertParse(Parser.ParseNullableInt32, "null", (Int32?)null);
            AssertParse(Parser.ParseNullableInt32, Int32.MinValue.ToString(), (Int32?)Int32.MinValue);
            AssertParse(Parser.ParseNullableInt32, Int32.MaxValue.ToString(), (Int32?)Int32.MaxValue);
            AssertParse(Parser.ParseNullableInt32, ((Int32)(Int32.MinValue + 1)).ToString(), (Int32?)(Int32.MinValue + 1));
            AssertParse(Parser.ParseNullableInt32, ((Int32)(Int32.MaxValue - 1)).ToString(), (Int32?)(Int32.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseNullableInt32, i.ToString(), (Int32?)i);
            }

            AssertParseFail(Parser.ParseNullableInt32, "-");

            AssertParseFail(Parser.ParseNullableInt32, "");
            AssertParseFail(Parser.ParseNullableInt32, " ");

            AssertParseFail(Parser.ParseNullableInt32, "true");
            AssertParseFail(Parser.ParseNullableInt32, "[");
            AssertParseFail(Parser.ParseNullableInt32, "]");
            AssertParseFail(Parser.ParseNullableInt32, "{");
            AssertParseFail(Parser.ParseNullableInt32, "}");
            AssertParseFail(Parser.ParseNullableInt32, ",");
            AssertParseFail(Parser.ParseNullableInt32, ":");

            AssertParseFail(Parser.ParseNullableInt32, "n");
            AssertParseFail(Parser.ParseNullableInt32, "nu");
            AssertParseFail(Parser.ParseNullableInt32, "nul");

            AssertParseFail(Parser.ParseNullableInt32, "Null");
            AssertParseFail(Parser.ParseNullableInt32, "nUll");
            AssertParseFail(Parser.ParseNullableInt32, "nuLl");
            AssertParseFail(Parser.ParseNullableInt32, "nulL");

            AssertParseFail(Parser.ParseNullableInt32, (new BigInteger(Int32.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableInt32, (new BigInteger(Int32.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_Int64()
        {
            AssertParse(Parser.ParseInt64, Int64.MinValue.ToString(), (Int64)Int64.MinValue);
            AssertParse(Parser.ParseInt64, Int64.MaxValue.ToString(), (Int64)Int64.MaxValue);
            AssertParse(Parser.ParseInt64, ((Int64)(Int64.MinValue + 1)).ToString(), (Int64)(Int64.MinValue + 1));
            AssertParse(Parser.ParseInt64, ((Int64)(Int64.MaxValue - 1)).ToString(), (Int64)(Int64.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseInt64, i.ToString(), (Int64)i);
            }

            AssertParseFail(Parser.ParseInt64, "-");

            AssertParseFail(Parser.ParseInt64, "");
            AssertParseFail(Parser.ParseInt64, " ");

            AssertParseFail(Parser.ParseInt64, "true");
            AssertParseFail(Parser.ParseInt64, "[");
            AssertParseFail(Parser.ParseInt64, "]");
            AssertParseFail(Parser.ParseInt64, "{");
            AssertParseFail(Parser.ParseInt64, "}");
            AssertParseFail(Parser.ParseInt64, ",");
            AssertParseFail(Parser.ParseInt64, ":");

            AssertParseFail(Parser.ParseInt64, "null");

            AssertParseFail(Parser.ParseInt64, (new BigInteger(Int64.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseInt64, (new BigInteger(Int64.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableInt64()
        {
            AssertParse(Parser.ParseNullableInt64, "null", (Int64?)null);
            AssertParse(Parser.ParseNullableInt64, Int64.MinValue.ToString(), (Int64?)Int64.MinValue);
            AssertParse(Parser.ParseNullableInt64, Int64.MaxValue.ToString(), (Int64?)Int64.MaxValue);
            AssertParse(Parser.ParseNullableInt64, ((Int64)(Int64.MinValue + 1)).ToString(), (Int64?)(Int64.MinValue + 1));
            AssertParse(Parser.ParseNullableInt64, ((Int64)(Int64.MaxValue - 1)).ToString(), (Int64?)(Int64.MaxValue - 1));

            for (var i = -128; i <= 127; i++)
            {
                AssertParse(Parser.ParseNullableInt64, i.ToString(), (Int64?)i);
            }

            AssertParseFail(Parser.ParseNullableInt64, "-");

            AssertParseFail(Parser.ParseNullableInt64, "");
            AssertParseFail(Parser.ParseNullableInt64, " ");

            AssertParseFail(Parser.ParseNullableInt64, "true");
            AssertParseFail(Parser.ParseNullableInt64, "[");
            AssertParseFail(Parser.ParseNullableInt64, "]");
            AssertParseFail(Parser.ParseNullableInt64, "{");
            AssertParseFail(Parser.ParseNullableInt64, "}");
            AssertParseFail(Parser.ParseNullableInt64, ",");
            AssertParseFail(Parser.ParseNullableInt64, ":");

            AssertParseFail(Parser.ParseNullableInt64, "n");
            AssertParseFail(Parser.ParseNullableInt64, "nu");
            AssertParseFail(Parser.ParseNullableInt64, "nul");

            AssertParseFail(Parser.ParseNullableInt64, "Null");
            AssertParseFail(Parser.ParseNullableInt64, "nUll");
            AssertParseFail(Parser.ParseNullableInt64, "nuLl");
            AssertParseFail(Parser.ParseNullableInt64, "nulL");

            AssertParseFail(Parser.ParseNullableInt64, (new BigInteger(Int64.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableInt64, (new BigInteger(Int64.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_Byte()
        {
            AssertParse(Parser.ParseByte, Byte.MinValue.ToString(), (Byte)Byte.MinValue);
            AssertParse(Parser.ParseByte, Byte.MaxValue.ToString(), (Byte)Byte.MaxValue);
            AssertParse(Parser.ParseByte, ((Byte)(Byte.MinValue + 1)).ToString(), (Byte)(Byte.MinValue + 1));
            AssertParse(Parser.ParseByte, ((Byte)(Byte.MaxValue - 1)).ToString(), (Byte)(Byte.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseByte, i.ToString(), (Byte)i);
            }

            AssertParseFail(Parser.ParseByte, "-1");

            AssertParseFail(Parser.ParseByte, "");
            AssertParseFail(Parser.ParseByte, " ");

            AssertParseFail(Parser.ParseByte, "true");
            AssertParseFail(Parser.ParseByte, "[");
            AssertParseFail(Parser.ParseByte, "]");
            AssertParseFail(Parser.ParseByte, "{");
            AssertParseFail(Parser.ParseByte, "}");
            AssertParseFail(Parser.ParseByte, ",");
            AssertParseFail(Parser.ParseByte, ":");

            AssertParseFail(Parser.ParseByte, "null");

            AssertParseFail(Parser.ParseByte, (new BigInteger(Byte.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseByte, (new BigInteger(Byte.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableByte()
        {
            AssertParse(Parser.ParseNullableByte, "null", (Byte?)null);
            AssertParse(Parser.ParseNullableByte, Byte.MinValue.ToString(), (Byte?)Byte.MinValue);
            AssertParse(Parser.ParseNullableByte, Byte.MaxValue.ToString(), (Byte?)Byte.MaxValue);
            AssertParse(Parser.ParseNullableByte, ((Byte)(Byte.MinValue + 1)).ToString(), (Byte?)(Byte.MinValue + 1));
            AssertParse(Parser.ParseNullableByte, ((Byte)(Byte.MaxValue - 1)).ToString(), (Byte?)(Byte.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseNullableByte, i.ToString(), (Byte?)i);
            }

            AssertParseFail(Parser.ParseNullableByte, "-1");

            AssertParseFail(Parser.ParseNullableByte, "");
            AssertParseFail(Parser.ParseNullableByte, " ");

            AssertParseFail(Parser.ParseNullableByte, "true");
            AssertParseFail(Parser.ParseNullableByte, "[");
            AssertParseFail(Parser.ParseNullableByte, "]");
            AssertParseFail(Parser.ParseNullableByte, "{");
            AssertParseFail(Parser.ParseNullableByte, "}");
            AssertParseFail(Parser.ParseNullableByte, ",");
            AssertParseFail(Parser.ParseNullableByte, ":");

            AssertParseFail(Parser.ParseNullableByte, "n");
            AssertParseFail(Parser.ParseNullableByte, "nu");
            AssertParseFail(Parser.ParseNullableByte, "nul");

            AssertParseFail(Parser.ParseNullableByte, "Null");
            AssertParseFail(Parser.ParseNullableByte, "nUll");
            AssertParseFail(Parser.ParseNullableByte, "nuLl");
            AssertParseFail(Parser.ParseNullableByte, "nulL");

            AssertParseFail(Parser.ParseNullableByte, (new BigInteger(Byte.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableByte, (new BigInteger(Byte.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_UInt16()
        {
            AssertParse(Parser.ParseUInt16, UInt16.MinValue.ToString(), (UInt16)UInt16.MinValue);
            AssertParse(Parser.ParseUInt16, UInt16.MaxValue.ToString(), (UInt16)UInt16.MaxValue);
            AssertParse(Parser.ParseUInt16, ((UInt16)(UInt16.MinValue + 1)).ToString(), (UInt16)(UInt16.MinValue + 1));
            AssertParse(Parser.ParseUInt16, ((UInt16)(UInt16.MaxValue - 1)).ToString(), (UInt16)(UInt16.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseUInt16, i.ToString(), (UInt16)i);
            }

            AssertParseFail(Parser.ParseUInt16, "-1");

            AssertParseFail(Parser.ParseUInt16, "");
            AssertParseFail(Parser.ParseUInt16, " ");

            AssertParseFail(Parser.ParseUInt16, "true");
            AssertParseFail(Parser.ParseUInt16, "[");
            AssertParseFail(Parser.ParseUInt16, "]");
            AssertParseFail(Parser.ParseUInt16, "{");
            AssertParseFail(Parser.ParseUInt16, "}");
            AssertParseFail(Parser.ParseUInt16, ",");
            AssertParseFail(Parser.ParseUInt16, ":");

            AssertParseFail(Parser.ParseUInt16, "null");

            AssertParseFail(Parser.ParseUInt16, (new BigInteger(UInt16.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseUInt16, (new BigInteger(UInt16.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableUInt16()
        {
            AssertParse(Parser.ParseNullableUInt16, "null", (UInt16?)null);
            AssertParse(Parser.ParseNullableUInt16, UInt16.MinValue.ToString(), (UInt16?)UInt16.MinValue);
            AssertParse(Parser.ParseNullableUInt16, UInt16.MaxValue.ToString(), (UInt16?)UInt16.MaxValue);
            AssertParse(Parser.ParseNullableUInt16, ((UInt16)(UInt16.MinValue + 1)).ToString(), (UInt16?)(UInt16.MinValue + 1));
            AssertParse(Parser.ParseNullableUInt16, ((UInt16)(UInt16.MaxValue - 1)).ToString(), (UInt16?)(UInt16.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseNullableUInt16, i.ToString(), (UInt16?)i);
            }

            AssertParseFail(Parser.ParseNullableUInt16, "-1");

            AssertParseFail(Parser.ParseNullableUInt16, "");
            AssertParseFail(Parser.ParseNullableUInt16, " ");

            AssertParseFail(Parser.ParseNullableUInt16, "true");
            AssertParseFail(Parser.ParseNullableUInt16, "[");
            AssertParseFail(Parser.ParseNullableUInt16, "]");
            AssertParseFail(Parser.ParseNullableUInt16, "{");
            AssertParseFail(Parser.ParseNullableUInt16, "}");
            AssertParseFail(Parser.ParseNullableUInt16, ",");
            AssertParseFail(Parser.ParseNullableUInt16, ":");

            AssertParseFail(Parser.ParseNullableUInt16, "n");
            AssertParseFail(Parser.ParseNullableUInt16, "nu");
            AssertParseFail(Parser.ParseNullableUInt16, "nul");

            AssertParseFail(Parser.ParseNullableUInt16, "Null");
            AssertParseFail(Parser.ParseNullableUInt16, "nUll");
            AssertParseFail(Parser.ParseNullableUInt16, "nuLl");
            AssertParseFail(Parser.ParseNullableUInt16, "nulL");

            AssertParseFail(Parser.ParseNullableUInt16, (new BigInteger(UInt16.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableUInt16, (new BigInteger(UInt16.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_UInt32()
        {
            AssertParse(Parser.ParseUInt32, UInt32.MinValue.ToString(), (UInt32)UInt32.MinValue);
            AssertParse(Parser.ParseUInt32, UInt32.MaxValue.ToString(), (UInt32)UInt32.MaxValue);
            AssertParse(Parser.ParseUInt32, ((UInt32)(UInt32.MinValue + 1)).ToString(), (UInt32)(UInt32.MinValue + 1));
            AssertParse(Parser.ParseUInt32, ((UInt32)(UInt32.MaxValue - 1)).ToString(), (UInt32)(UInt32.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseUInt32, i.ToString(), (UInt32)i);
            }

            AssertParseFail(Parser.ParseUInt32, "-1");

            AssertParseFail(Parser.ParseUInt32, "");
            AssertParseFail(Parser.ParseUInt32, " ");

            AssertParseFail(Parser.ParseUInt32, "true");
            AssertParseFail(Parser.ParseUInt32, "[");
            AssertParseFail(Parser.ParseUInt32, "]");
            AssertParseFail(Parser.ParseUInt32, "{");
            AssertParseFail(Parser.ParseUInt32, "}");
            AssertParseFail(Parser.ParseUInt32, ",");
            AssertParseFail(Parser.ParseUInt32, ":");

            AssertParseFail(Parser.ParseUInt32, "null");

            AssertParseFail(Parser.ParseUInt32, (new BigInteger(UInt32.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseUInt32, (new BigInteger(UInt32.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableUInt32()
        {
            AssertParse(Parser.ParseNullableUInt32, "null", (UInt32?)null);
            AssertParse(Parser.ParseNullableUInt32, UInt32.MinValue.ToString(), (UInt32?)UInt32.MinValue);
            AssertParse(Parser.ParseNullableUInt32, UInt32.MaxValue.ToString(), (UInt32?)UInt32.MaxValue);
            AssertParse(Parser.ParseNullableUInt32, ((UInt32)(UInt32.MinValue + 1)).ToString(), (UInt32?)(UInt32.MinValue + 1));
            AssertParse(Parser.ParseNullableUInt32, ((UInt32)(UInt32.MaxValue - 1)).ToString(), (UInt32?)(UInt32.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseNullableUInt32, i.ToString(), (UInt32?)i);
            }

            AssertParseFail(Parser.ParseNullableUInt32, "-1");

            AssertParseFail(Parser.ParseNullableUInt32, "");
            AssertParseFail(Parser.ParseNullableUInt32, " ");

            AssertParseFail(Parser.ParseNullableUInt32, "true");
            AssertParseFail(Parser.ParseNullableUInt32, "[");
            AssertParseFail(Parser.ParseNullableUInt32, "]");
            AssertParseFail(Parser.ParseNullableUInt32, "{");
            AssertParseFail(Parser.ParseNullableUInt32, "}");
            AssertParseFail(Parser.ParseNullableUInt32, ",");
            AssertParseFail(Parser.ParseNullableUInt32, ":");

            AssertParseFail(Parser.ParseNullableUInt32, "n");
            AssertParseFail(Parser.ParseNullableUInt32, "nu");
            AssertParseFail(Parser.ParseNullableUInt32, "nul");

            AssertParseFail(Parser.ParseNullableUInt32, "Null");
            AssertParseFail(Parser.ParseNullableUInt32, "nUll");
            AssertParseFail(Parser.ParseNullableUInt32, "nuLl");
            AssertParseFail(Parser.ParseNullableUInt32, "nulL");

            AssertParseFail(Parser.ParseNullableUInt32, (new BigInteger(UInt32.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableUInt32, (new BigInteger(UInt32.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_UInt64()
        {
            AssertParse(Parser.ParseUInt64, UInt64.MinValue.ToString(), (UInt64)UInt64.MinValue);
            AssertParse(Parser.ParseUInt64, UInt64.MaxValue.ToString(), (UInt64)UInt64.MaxValue);
            AssertParse(Parser.ParseUInt64, ((UInt64)(UInt64.MinValue + 1)).ToString(), (UInt64)(UInt64.MinValue + 1));
            AssertParse(Parser.ParseUInt64, ((UInt64)(UInt64.MaxValue - 1)).ToString(), (UInt64)(UInt64.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseUInt64, i.ToString(), (UInt64)i);
            }

            AssertParseFail(Parser.ParseUInt64, "-1");

            AssertParseFail(Parser.ParseUInt64, "");
            AssertParseFail(Parser.ParseUInt64, " ");

            AssertParseFail(Parser.ParseUInt64, "true");
            AssertParseFail(Parser.ParseUInt64, "[");
            AssertParseFail(Parser.ParseUInt64, "]");
            AssertParseFail(Parser.ParseUInt64, "{");
            AssertParseFail(Parser.ParseUInt64, "}");
            AssertParseFail(Parser.ParseUInt64, ",");
            AssertParseFail(Parser.ParseUInt64, ":");

            AssertParseFail(Parser.ParseUInt64, "null");

            AssertParseFail(Parser.ParseUInt64, (new BigInteger(UInt64.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseUInt64, (new BigInteger(UInt64.MaxValue) + 1).ToString());
        }

        [TestMethod]
        public void FastParser_NullableUInt64()
        {
            AssertParse(Parser.ParseNullableUInt64, "null", (UInt64?)null);
            AssertParse(Parser.ParseNullableUInt64, UInt64.MinValue.ToString(), (UInt64?)UInt64.MinValue);
            AssertParse(Parser.ParseNullableUInt64, UInt64.MaxValue.ToString(), (UInt64?)UInt64.MaxValue);
            AssertParse(Parser.ParseNullableUInt64, ((UInt64)(UInt64.MinValue + 1)).ToString(), (UInt64?)(UInt64.MinValue + 1));
            AssertParse(Parser.ParseNullableUInt64, ((UInt64)(UInt64.MaxValue - 1)).ToString(), (UInt64?)(UInt64.MaxValue - 1));

            for (var i = 0; i <= 255; i++)
            {
                AssertParse(Parser.ParseNullableUInt64, i.ToString(), (UInt64?)i);
            }

            AssertParseFail(Parser.ParseNullableUInt64, "-1");

            AssertParseFail(Parser.ParseNullableUInt64, "");
            AssertParseFail(Parser.ParseNullableUInt64, " ");

            AssertParseFail(Parser.ParseNullableUInt64, "true");
            AssertParseFail(Parser.ParseNullableUInt64, "[");
            AssertParseFail(Parser.ParseNullableUInt64, "]");
            AssertParseFail(Parser.ParseNullableUInt64, "{");
            AssertParseFail(Parser.ParseNullableUInt64, "}");
            AssertParseFail(Parser.ParseNullableUInt64, ",");
            AssertParseFail(Parser.ParseNullableUInt64, ":");

            AssertParseFail(Parser.ParseNullableUInt64, "n");
            AssertParseFail(Parser.ParseNullableUInt64, "nu");
            AssertParseFail(Parser.ParseNullableUInt64, "nul");

            AssertParseFail(Parser.ParseNullableUInt64, "Null");
            AssertParseFail(Parser.ParseNullableUInt64, "nUll");
            AssertParseFail(Parser.ParseNullableUInt64, "nuLl");
            AssertParseFail(Parser.ParseNullableUInt64, "nulL");

            AssertParseFail(Parser.ParseNullableUInt64, (new BigInteger(UInt64.MinValue) - 1).ToString());
            AssertParseFail(Parser.ParseNullableUInt64, (new BigInteger(UInt64.MaxValue) + 1).ToString());
        }

    }
}
