// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
    
using Nuqleon.DataModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    using Tests = DataModelSerializerFactoryTestCase.Tests;
    using IsotopeTestCase = DataModelSerializerFactoryTestCase.IsotopeTestCase;
    using ArrayComparer = DataModelSerializerFactoryTestCase.ArrayComparer;
    using DeepComparer = DataModelSerializerFactoryTestCase.DeepComparer;

    public partial class DataModelSerializerFactoryTests
    {
        private SerializationTestCases generatedTests = new SerializationTestCases();

        [TestMethod]
        public void BinarySerialization_Sbyte_Arrays()
        {
            Run(generatedTests.BinarySerialization_Sbyte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Byte_Arrays()
        {
            Run(generatedTests.BinarySerialization_Byte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Char_Arrays()
        {
            Run(generatedTests.BinarySerialization_Char_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Short_Arrays()
        {
            Run(generatedTests.BinarySerialization_Short_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Ushort_Arrays()
        {
            Run(generatedTests.BinarySerialization_Ushort_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Int_Arrays()
        {
            Run(generatedTests.BinarySerialization_Int_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Uint_Arrays()
        {
            Run(generatedTests.BinarySerialization_Uint_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Long_Arrays()
        {
            Run(generatedTests.BinarySerialization_Long_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Ulong_Arrays()
        {
            Run(generatedTests.BinarySerialization_Ulong_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Single_Arrays()
        {
            Run(generatedTests.BinarySerialization_Single_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Double_Arrays()
        {
            Run(generatedTests.BinarySerialization_Double_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Float_Arrays()
        {
            Run(generatedTests.BinarySerialization_Float_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Decimal_Arrays()
        {
            Run(generatedTests.BinarySerialization_Decimal_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Bool_Arrays()
        {
            Run(generatedTests.BinarySerialization_Bool_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Datetime_Arrays()
        {
            Run(generatedTests.BinarySerialization_Datetime_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Guid_Arrays()
        {
            Run(generatedTests.BinarySerialization_Guid_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Datetimeoffset_Arrays()
        {
            Run(generatedTests.BinarySerialization_Datetimeoffset_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Timespan_Arrays()
        {
            Run(generatedTests.BinarySerialization_Timespan_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_String_Arrays()
        {
            Run(generatedTests.BinarySerialization_String_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Uri_Arrays()
        {
            Run(generatedTests.BinarySerialization_Uri_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableSbyte_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableSbyte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableByte_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableByte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableChar_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableChar_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableShort_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableShort_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableUshort_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableUshort_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableUint_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableUint_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableLong_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableLong_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableUlong_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableUlong_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableSingle_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableSingle_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDouble_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDouble_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableFloat_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableFloat_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDecimal_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDecimal_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableBool_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableBool_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDatetime_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDatetime_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableGuid_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableGuid_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDatetimeoffset_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDatetimeoffset_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableTimespan_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableTimespan_Arrays_Tests());
        }


        [TestMethod]
        public void BinarySerialization_ClassWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithNullableIntArray_Arrays_Tests());
        }


        [TestMethod]
        public void BinarySerialization_IsotopeTest_BigStruct1()
        {
            RunIsotopeTest(new IsotopeTestCase
            {
                { typeof(BigStruct1_Permutation0), RandValue.BigStruct1_Permutation0},
                { typeof(BigStruct1_Permutation1), RandValue.BigStruct1_Permutation1},
                { typeof(BigStruct1_Permutation2), RandValue.BigStruct1_Permutation2},
                { typeof(BigStruct1_Permutation3), RandValue.BigStruct1_Permutation3},
                { typeof(BigStruct1_Permutation4), RandValue.BigStruct1_Permutation4},
                { typeof(BigStruct1_Permutation5), RandValue.BigStruct1_Permutation5},
            }, new DeepComparer());
        }
        
        [TestMethod]
        public void BinarySerialization_IsotopeTest_BigStruct2()
        {
            RunIsotopeTest(new IsotopeTestCase
            {
                { typeof(BigStruct2_Permutation0), RandValue.BigStruct2_Permutation0},
                { typeof(BigStruct2_Permutation1), RandValue.BigStruct2_Permutation1},
                { typeof(BigStruct2_Permutation2), RandValue.BigStruct2_Permutation2},
                { typeof(BigStruct2_Permutation3), RandValue.BigStruct2_Permutation3},
                { typeof(BigStruct2_Permutation4), RandValue.BigStruct2_Permutation4},
                { typeof(BigStruct2_Permutation5), RandValue.BigStruct2_Permutation5},
            }, new DeepComparer());
        }
        
        [TestMethod]
        public void BinarySerialization_IsotopeTest_BigStruct3()
        {
            RunIsotopeTest(new IsotopeTestCase
            {
                { typeof(BigStruct3_Permutation0), RandValue.BigStruct3_Permutation0},
                { typeof(BigStruct3_Permutation1), RandValue.BigStruct3_Permutation1},
                { typeof(BigStruct3_Permutation2), RandValue.BigStruct3_Permutation2},
                { typeof(BigStruct3_Permutation3), RandValue.BigStruct3_Permutation3},
                { typeof(BigStruct3_Permutation4), RandValue.BigStruct3_Permutation4},
                { typeof(BigStruct3_Permutation5), RandValue.BigStruct3_Permutation5},
                { typeof(BigStruct3_Permutation6), RandValue.BigStruct3_Permutation6},
                { typeof(BigStruct3_Permutation7), RandValue.BigStruct3_Permutation7},
                { typeof(BigStruct3_Permutation8), RandValue.BigStruct3_Permutation8},
                { typeof(BigStruct3_Permutation9), RandValue.BigStruct3_Permutation9},
                { typeof(BigStruct3_Permutation10), RandValue.BigStruct3_Permutation10},
                { typeof(BigStruct3_Permutation11), RandValue.BigStruct3_Permutation11},
                { typeof(BigStruct3_Permutation12), RandValue.BigStruct3_Permutation12},
                { typeof(BigStruct3_Permutation13), RandValue.BigStruct3_Permutation13},
                { typeof(BigStruct3_Permutation14), RandValue.BigStruct3_Permutation14},
                { typeof(BigStruct3_Permutation15), RandValue.BigStruct3_Permutation15},
                { typeof(BigStruct3_Permutation16), RandValue.BigStruct3_Permutation16},
                { typeof(BigStruct3_Permutation17), RandValue.BigStruct3_Permutation17},
                { typeof(BigStruct3_Permutation18), RandValue.BigStruct3_Permutation18},
                { typeof(BigStruct3_Permutation19), RandValue.BigStruct3_Permutation19},
                { typeof(BigStruct3_Permutation20), RandValue.BigStruct3_Permutation20},
                { typeof(BigStruct3_Permutation21), RandValue.BigStruct3_Permutation21},
                { typeof(BigStruct3_Permutation22), RandValue.BigStruct3_Permutation22},
                { typeof(BigStruct3_Permutation23), RandValue.BigStruct3_Permutation23},
            }, new DeepComparer());
        }
        
        [TestMethod]
        public void BinarySerialization_IsotopeTest_BigClass1()
        {
            RunIsotopeTest(new IsotopeTestCase
            {
                { typeof(BigClass1_Permutation0), RandValue.BigClass1_Permutation0},
                { typeof(BigClass1_Permutation1), RandValue.BigClass1_Permutation1},
                { typeof(BigClass1_Permutation2), RandValue.BigClass1_Permutation2},
                { typeof(BigClass1_Permutation3), RandValue.BigClass1_Permutation3},
                { typeof(BigClass1_Permutation4), RandValue.BigClass1_Permutation4},
                { typeof(BigClass1_Permutation5), RandValue.BigClass1_Permutation5},
            }, new DeepComparer());
        }
        
        [TestMethod]
        public void BinarySerialization_IsotopeTest_BigClass2()
        {
            RunIsotopeTest(new IsotopeTestCase
            {
                { typeof(BigClass2_Permutation0), RandValue.BigClass2_Permutation0},
                { typeof(BigClass2_Permutation1), RandValue.BigClass2_Permutation1},
                { typeof(BigClass2_Permutation2), RandValue.BigClass2_Permutation2},
                { typeof(BigClass2_Permutation3), RandValue.BigClass2_Permutation3},
                { typeof(BigClass2_Permutation4), RandValue.BigClass2_Permutation4},
                { typeof(BigClass2_Permutation5), RandValue.BigClass2_Permutation5},
            }, new DeepComparer());
        }
        
        [TestMethod]
        public void BinarySerialization_IsotopeTest_BigClass3()
        {
            RunIsotopeTest(new IsotopeTestCase
            {
                { typeof(BigClass3_Permutation0), RandValue.BigClass3_Permutation0},
                { typeof(BigClass3_Permutation1), RandValue.BigClass3_Permutation1},
                { typeof(BigClass3_Permutation2), RandValue.BigClass3_Permutation2},
                { typeof(BigClass3_Permutation3), RandValue.BigClass3_Permutation3},
                { typeof(BigClass3_Permutation4), RandValue.BigClass3_Permutation4},
                { typeof(BigClass3_Permutation5), RandValue.BigClass3_Permutation5},
                { typeof(BigClass3_Permutation6), RandValue.BigClass3_Permutation6},
                { typeof(BigClass3_Permutation7), RandValue.BigClass3_Permutation7},
                { typeof(BigClass3_Permutation8), RandValue.BigClass3_Permutation8},
                { typeof(BigClass3_Permutation9), RandValue.BigClass3_Permutation9},
                { typeof(BigClass3_Permutation10), RandValue.BigClass3_Permutation10},
                { typeof(BigClass3_Permutation11), RandValue.BigClass3_Permutation11},
                { typeof(BigClass3_Permutation12), RandValue.BigClass3_Permutation12},
                { typeof(BigClass3_Permutation13), RandValue.BigClass3_Permutation13},
                { typeof(BigClass3_Permutation14), RandValue.BigClass3_Permutation14},
                { typeof(BigClass3_Permutation15), RandValue.BigClass3_Permutation15},
                { typeof(BigClass3_Permutation16), RandValue.BigClass3_Permutation16},
                { typeof(BigClass3_Permutation17), RandValue.BigClass3_Permutation17},
                { typeof(BigClass3_Permutation18), RandValue.BigClass3_Permutation18},
                { typeof(BigClass3_Permutation19), RandValue.BigClass3_Permutation19},
                { typeof(BigClass3_Permutation20), RandValue.BigClass3_Permutation20},
                { typeof(BigClass3_Permutation21), RandValue.BigClass3_Permutation21},
                { typeof(BigClass3_Permutation22), RandValue.BigClass3_Permutation22},
                { typeof(BigClass3_Permutation23), RandValue.BigClass3_Permutation23},
            }, new DeepComparer());
        }
        
    }

    public partial class DataModelSerializerFactoryParallelTests
    {
        private SerializationTestCases generatedTests = new SerializationTestCases();

        [TestMethod]
        public void BinarySerialization_Sbyte_Arrays()
        {
            Run(generatedTests.BinarySerialization_Sbyte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Byte_Arrays()
        {
            Run(generatedTests.BinarySerialization_Byte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Char_Arrays()
        {
            Run(generatedTests.BinarySerialization_Char_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Short_Arrays()
        {
            Run(generatedTests.BinarySerialization_Short_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Ushort_Arrays()
        {
            Run(generatedTests.BinarySerialization_Ushort_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Int_Arrays()
        {
            Run(generatedTests.BinarySerialization_Int_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Uint_Arrays()
        {
            Run(generatedTests.BinarySerialization_Uint_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Long_Arrays()
        {
            Run(generatedTests.BinarySerialization_Long_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Ulong_Arrays()
        {
            Run(generatedTests.BinarySerialization_Ulong_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Single_Arrays()
        {
            Run(generatedTests.BinarySerialization_Single_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Double_Arrays()
        {
            Run(generatedTests.BinarySerialization_Double_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Float_Arrays()
        {
            Run(generatedTests.BinarySerialization_Float_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Decimal_Arrays()
        {
            Run(generatedTests.BinarySerialization_Decimal_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Bool_Arrays()
        {
            Run(generatedTests.BinarySerialization_Bool_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Datetime_Arrays()
        {
            Run(generatedTests.BinarySerialization_Datetime_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Guid_Arrays()
        {
            Run(generatedTests.BinarySerialization_Guid_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Datetimeoffset_Arrays()
        {
            Run(generatedTests.BinarySerialization_Datetimeoffset_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Timespan_Arrays()
        {
            Run(generatedTests.BinarySerialization_Timespan_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_String_Arrays()
        {
            Run(generatedTests.BinarySerialization_String_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_Uri_Arrays()
        {
            Run(generatedTests.BinarySerialization_Uri_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableSbyte_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableSbyte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableByte_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableByte_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableChar_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableChar_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableShort_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableShort_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableUshort_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableUshort_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableUint_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableUint_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableLong_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableLong_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableUlong_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableUlong_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableSingle_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableSingle_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDouble_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDouble_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableFloat_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableFloat_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDecimal_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDecimal_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableBool_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableBool_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDatetime_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDatetime_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableGuid_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableGuid_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableDatetimeoffset_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableDatetimeoffset_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_NullableTimespan_Arrays()
        {
            Run(generatedTests.BinarySerialization_NullableTimespan_Arrays_Tests());
        }


        [TestMethod]
        public void BinarySerialization_ClassWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithClassWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithClassWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_ClassWithStructWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_ClassWithStructWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithClassWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithClassWithNullableIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithNullableInt_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithNullableInt_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithIntArray_Arrays_Tests());
        }

        [TestMethod]
        public void BinarySerialization_StructWithStructWithNullableIntArray_Arrays()
        {
            Run(generatedTests.BinarySerialization_StructWithStructWithNullableIntArray_Arrays_Tests());
        }

    }

    public class SerializationTestCases
    {
        public Tests BinarySerialization_Sbyte_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(sbyte[])null, ArrayComparer.Instance", typeof(sbyte []), null, ArrayComparer.Instance },
                { "(sbyte[])new sbyte[0], ArrayComparer.Instance", typeof(sbyte []), new sbyte[0], ArrayComparer.Instance },
                { "(sbyte[])new sbyte[] {42}, ArrayComparer.Instance", typeof(sbyte []), new sbyte[] {42}, ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(((sbyte)0), 2).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(((sbyte)0), 2).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(((sbyte)0), 4).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(((sbyte)0), 4).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(((sbyte)0), 16).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(((sbyte)0), 16).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(((sbyte)0), 256).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(((sbyte)0), 256).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(((sbyte)0), 1024).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(((sbyte)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(sbyte[])Enumerable.Repeat(sbyte.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(sbyte []), Enumerable.Repeat<sbyte>(sbyte.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Byte_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(byte[])null, ArrayComparer.Instance", typeof(byte []), null, ArrayComparer.Instance },
                { "(byte[])new byte[0], ArrayComparer.Instance", typeof(byte []), new byte[0], ArrayComparer.Instance },
                { "(byte[])new byte[] {42}, ArrayComparer.Instance", typeof(byte []), new byte[] {42}, ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(((byte)0), 2).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(((byte)0), 2).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(((byte)0), 4).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(((byte)0), 4).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(((byte)0), 16).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(((byte)0), 16).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(((byte)0), 256).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(((byte)0), 256).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(((byte)0), 1024).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(((byte)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(byte[])Enumerable.Repeat(byte.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(byte []), Enumerable.Repeat<byte>(byte.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Char_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(char[])null, ArrayComparer.Instance", typeof(char []), null, ArrayComparer.Instance },
                { "(char[])new char[0], ArrayComparer.Instance", typeof(char []), new char[0], ArrayComparer.Instance },
                { "(char[])new char[] {'a'}, ArrayComparer.Instance", typeof(char []), new char[] {'a'}, ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Short_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(short[])null, ArrayComparer.Instance", typeof(short []), null, ArrayComparer.Instance },
                { "(short[])new short[0], ArrayComparer.Instance", typeof(short []), new short[0], ArrayComparer.Instance },
                { "(short[])new short[] {42}, ArrayComparer.Instance", typeof(short []), new short[] {42}, ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(((short)0), 2).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(((short)0), 2).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(((short)0), 4).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(((short)0), 4).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(((short)0), 16).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(((short)0), 16).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(((short)0), 256).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(((short)0), 256).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(((short)0), 1024).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(((short)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(short[])Enumerable.Repeat(short.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(short []), Enumerable.Repeat<short>(short.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Ushort_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(ushort[])null, ArrayComparer.Instance", typeof(ushort []), null, ArrayComparer.Instance },
                { "(ushort[])new ushort[0], ArrayComparer.Instance", typeof(ushort []), new ushort[0], ArrayComparer.Instance },
                { "(ushort[])new ushort[] {42}, ArrayComparer.Instance", typeof(ushort []), new ushort[] {42}, ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(((ushort)0), 2).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(((ushort)0), 2).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(((ushort)0), 4).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(((ushort)0), 4).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(((ushort)0), 16).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(((ushort)0), 16).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(((ushort)0), 256).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(((ushort)0), 256).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(((ushort)0), 1024).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(((ushort)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ushort[])Enumerable.Repeat(ushort.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ushort []), Enumerable.Repeat<ushort>(ushort.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Int_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(int[])null, ArrayComparer.Instance", typeof(int []), null, ArrayComparer.Instance },
                { "(int[])new int[0], ArrayComparer.Instance", typeof(int []), new int[0], ArrayComparer.Instance },
                { "(int[])new int[] {42}, ArrayComparer.Instance", typeof(int []), new int[] {42}, ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(((int)0), 2).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(((int)0), 2).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(((int)0), 4).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(((int)0), 4).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(((int)0), 16).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(((int)0), 16).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(((int)0), 256).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(((int)0), 256).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(((int)0), 1024).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(((int)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(int []), Enumerable.Repeat<int>(int.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Uint_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(uint[])null, ArrayComparer.Instance", typeof(uint []), null, ArrayComparer.Instance },
                { "(uint[])new uint[0], ArrayComparer.Instance", typeof(uint []), new uint[0], ArrayComparer.Instance },
                { "(uint[])new uint[] {42}, ArrayComparer.Instance", typeof(uint []), new uint[] {42}, ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(((uint)0), 2).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(((uint)0), 2).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(((uint)0), 4).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(((uint)0), 4).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(((uint)0), 16).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(((uint)0), 16).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(((uint)0), 256).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(((uint)0), 256).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(((uint)0), 1024).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(((uint)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(uint[])Enumerable.Repeat(uint.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(uint []), Enumerable.Repeat<uint>(uint.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Long_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(long[])null, ArrayComparer.Instance", typeof(long []), null, ArrayComparer.Instance },
                { "(long[])new long[0], ArrayComparer.Instance", typeof(long []), new long[0], ArrayComparer.Instance },
                { "(long[])new long[] {42}, ArrayComparer.Instance", typeof(long []), new long[] {42}, ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(((long)0), 2).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(((long)0), 2).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(((long)0), 4).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(((long)0), 4).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(((long)0), 16).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(((long)0), 16).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(((long)0), 256).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(((long)0), 256).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(((long)0), 1024).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(((long)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(long[])Enumerable.Repeat(long.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(long []), Enumerable.Repeat<long>(long.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Ulong_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(ulong[])null, ArrayComparer.Instance", typeof(ulong []), null, ArrayComparer.Instance },
                { "(ulong[])new ulong[0], ArrayComparer.Instance", typeof(ulong []), new ulong[0], ArrayComparer.Instance },
                { "(ulong[])new ulong[] {42}, ArrayComparer.Instance", typeof(ulong []), new ulong[] {42}, ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(((ulong)0), 2).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(((ulong)0), 2).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(((ulong)0), 4).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(((ulong)0), 4).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(((ulong)0), 16).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(((ulong)0), 16).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(((ulong)0), 256).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(((ulong)0), 256).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(((ulong)0), 1024).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(((ulong)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ulong[])Enumerable.Repeat(ulong.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ulong []), Enumerable.Repeat<ulong>(ulong.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Single_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(Single[])null, ArrayComparer.Instance", typeof(Single []), null, ArrayComparer.Instance },
                { "(Single[])new Single[0], ArrayComparer.Instance", typeof(Single []), new Single[0], ArrayComparer.Instance },
                { "(Single[])new Single[] {42}, ArrayComparer.Instance", typeof(Single []), new Single[] {42}, ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(((Single)0), 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(((Single)0), 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(((Single)0), 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(((Single)0), 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(((Single)0), 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(((Single)0), 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(((Single)0), 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(((Single)0), 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(((Single)0), 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(((Single)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.Epsilon, 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.Epsilon, 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.Epsilon, 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.Epsilon, 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.Epsilon, 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.Epsilon, 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.Epsilon, 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.Epsilon, 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.Epsilon, 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.Epsilon, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NaN, 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NaN, 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NaN, 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NaN, 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NaN, 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NaN, 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NaN, 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NaN, 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NaN, 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NaN, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(Single[])Enumerable.Repeat(Single.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(Single []), Enumerable.Repeat<Single>(Single.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Double_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(double[])null, ArrayComparer.Instance", typeof(double []), null, ArrayComparer.Instance },
                { "(double[])new double[0], ArrayComparer.Instance", typeof(double []), new double[0], ArrayComparer.Instance },
                { "(double[])new double[] {42}, ArrayComparer.Instance", typeof(double []), new double[] {42}, ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(((double)0), 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(((double)0), 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(((double)0), 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(((double)0), 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(((double)0), 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(((double)0), 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(((double)0), 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(((double)0), 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(((double)0), 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(((double)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.Epsilon, 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.Epsilon, 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.Epsilon, 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.Epsilon, 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.Epsilon, 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.Epsilon, 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.Epsilon, 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.Epsilon, 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.Epsilon, 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.Epsilon, 1024).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NaN, 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NaN, 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NaN, 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NaN, 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NaN, 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NaN, 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NaN, 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NaN, 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NaN, 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NaN, 1024).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(double[])Enumerable.Repeat(double.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(double []), Enumerable.Repeat<double>(double.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Float_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(float[])null, ArrayComparer.Instance", typeof(float []), null, ArrayComparer.Instance },
                { "(float[])new float[0], ArrayComparer.Instance", typeof(float []), new float[0], ArrayComparer.Instance },
                { "(float[])new float[] {42}, ArrayComparer.Instance", typeof(float []), new float[] {42}, ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(((float)0), 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(((float)0), 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(((float)0), 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(((float)0), 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(((float)0), 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(((float)0), 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(((float)0), 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(((float)0), 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(((float)0), 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(((float)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.Epsilon, 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.Epsilon, 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.Epsilon, 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.Epsilon, 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.Epsilon, 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.Epsilon, 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.Epsilon, 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.Epsilon, 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.Epsilon, 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.Epsilon, 1024).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NaN, 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NaN, 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NaN, 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NaN, 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NaN, 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NaN, 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NaN, 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NaN, 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NaN, 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NaN, 1024).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(float[])Enumerable.Repeat(float.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(float []), Enumerable.Repeat<float>(float.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Decimal_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(decimal[])null, ArrayComparer.Instance", typeof(decimal []), null, ArrayComparer.Instance },
                { "(decimal[])new decimal[0], ArrayComparer.Instance", typeof(decimal []), new decimal[0], ArrayComparer.Instance },
                { "(decimal[])new decimal[] {42}, ArrayComparer.Instance", typeof(decimal []), new decimal[] {42}, ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(((decimal)0), 2).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(((decimal)0), 2).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(((decimal)0), 4).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(((decimal)0), 4).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(((decimal)0), 16).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(((decimal)0), 16).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(((decimal)0), 256).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(((decimal)0), 256).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(((decimal)0), 1024).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(((decimal)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.Zero, 2).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.Zero, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.Zero, 4).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.Zero, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.Zero, 16).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.Zero, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.Zero, 256).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.Zero, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.Zero, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.Zero, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.One, 2).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.One, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.One, 4).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.One, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.One, 16).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.One, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.One, 256).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.One, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.One, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.One, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinusOne, 2).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinusOne, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinusOne, 4).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinusOne, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinusOne, 16).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinusOne, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinusOne, 256).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinusOne, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal[])Enumerable.Repeat(decimal.MinusOne, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal []), Enumerable.Repeat<decimal>(decimal.MinusOne, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Bool_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(bool[])null, ArrayComparer.Instance", typeof(bool []), null, ArrayComparer.Instance },
                { "(bool[])new bool[0], ArrayComparer.Instance", typeof(bool []), new bool[0], ArrayComparer.Instance },
                { "(bool[])new bool[] {default(bool)}, ArrayComparer.Instance", typeof(bool []), new bool[] {default(bool)}, ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Datetime_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(DateTime[])null, ArrayComparer.Instance", typeof(DateTime []), null, ArrayComparer.Instance },
                { "(DateTime[])new DateTime[0], ArrayComparer.Instance", typeof(DateTime []), new DateTime[0], ArrayComparer.Instance },
                { "(DateTime[])new DateTime[] {default(DateTime)}, ArrayComparer.Instance", typeof(DateTime []), new DateTime[] {default(DateTime)}, ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.Now, 2).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.Now, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.Now, 4).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.Now, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.Now, 16).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.Now, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.Now, 256).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.Now, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.Now, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.Now, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTime[])Enumerable.Repeat(DateTime.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTime []), Enumerable.Repeat<DateTime>(DateTime.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Guid_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(Guid[])null, ArrayComparer.Instance", typeof(Guid []), null, ArrayComparer.Instance },
                { "(Guid[])new Guid[0], ArrayComparer.Instance", typeof(Guid []), new Guid[0], ArrayComparer.Instance },
                { "(Guid[])new Guid[] {default(Guid)}, ArrayComparer.Instance", typeof(Guid []), new Guid[] {default(Guid)}, ArrayComparer.Instance },
                { "(Guid[])Enumerable.Repeat(Guid.Empty, 2).ToArray(), ArrayComparer.Instance", typeof(Guid []), Enumerable.Repeat<Guid>(Guid.Empty, 2).ToArray(), ArrayComparer.Instance },
                { "(Guid[])Enumerable.Repeat(Guid.Empty, 4).ToArray(), ArrayComparer.Instance", typeof(Guid []), Enumerable.Repeat<Guid>(Guid.Empty, 4).ToArray(), ArrayComparer.Instance },
                { "(Guid[])Enumerable.Repeat(Guid.Empty, 16).ToArray(), ArrayComparer.Instance", typeof(Guid []), Enumerable.Repeat<Guid>(Guid.Empty, 16).ToArray(), ArrayComparer.Instance },
                { "(Guid[])Enumerable.Repeat(Guid.Empty, 256).ToArray(), ArrayComparer.Instance", typeof(Guid []), Enumerable.Repeat<Guid>(Guid.Empty, 256).ToArray(), ArrayComparer.Instance },
                { "(Guid[])Enumerable.Repeat(Guid.Empty, 1024).ToArray(), ArrayComparer.Instance", typeof(Guid []), Enumerable.Repeat<Guid>(Guid.Empty, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Datetimeoffset_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(DateTimeOffset[])null, ArrayComparer.Instance", typeof(DateTimeOffset []), null, ArrayComparer.Instance },
                { "(DateTimeOffset[])new DateTimeOffset[0], ArrayComparer.Instance", typeof(DateTimeOffset []), new DateTimeOffset[0], ArrayComparer.Instance },
                { "(DateTimeOffset[])new DateTimeOffset[] {default(DateTimeOffset)}, ArrayComparer.Instance", typeof(DateTimeOffset []), new DateTimeOffset[] {default(DateTimeOffset)}, ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.Now, 2).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.Now, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.Now, 4).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.Now, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.Now, 16).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.Now, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.Now, 256).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.Now, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.Now, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.Now, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset[])Enumerable.Repeat(DateTimeOffset.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset []), Enumerable.Repeat<DateTimeOffset>(DateTimeOffset.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Timespan_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(TimeSpan[])null, ArrayComparer.Instance", typeof(TimeSpan []), null, ArrayComparer.Instance },
                { "(TimeSpan[])new TimeSpan[0], ArrayComparer.Instance", typeof(TimeSpan []), new TimeSpan[0], ArrayComparer.Instance },
                { "(TimeSpan[])new TimeSpan[] {default(TimeSpan)}, ArrayComparer.Instance", typeof(TimeSpan []), new TimeSpan[] {default(TimeSpan)}, ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.Zero, 2).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.Zero, 2).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.Zero, 4).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.Zero, 4).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.Zero, 16).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.Zero, 16).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.Zero, 256).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.Zero, 256).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.Zero, 1024).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.Zero, 1024).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan[])Enumerable.Repeat(TimeSpan.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(TimeSpan []), Enumerable.Repeat<TimeSpan>(TimeSpan.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_String_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(string[])null, ArrayComparer.Instance", typeof(string []), null, ArrayComparer.Instance },
                { "(string[])new string[0], ArrayComparer.Instance", typeof(string []), new string[0], ArrayComparer.Instance },
                { "(string[])new string[] {default(string)}, ArrayComparer.Instance", typeof(string []), new string[] {default(string)}, ArrayComparer.Instance },
                { "(string[])Enumerable.Repeat(string.Empty, 2).ToArray(), ArrayComparer.Instance", typeof(string []), Enumerable.Repeat<string>(string.Empty, 2).ToArray(), ArrayComparer.Instance },
                { "(string[])Enumerable.Repeat(string.Empty, 4).ToArray(), ArrayComparer.Instance", typeof(string []), Enumerable.Repeat<string>(string.Empty, 4).ToArray(), ArrayComparer.Instance },
                { "(string[])Enumerable.Repeat(string.Empty, 16).ToArray(), ArrayComparer.Instance", typeof(string []), Enumerable.Repeat<string>(string.Empty, 16).ToArray(), ArrayComparer.Instance },
                { "(string[])Enumerable.Repeat(string.Empty, 256).ToArray(), ArrayComparer.Instance", typeof(string []), Enumerable.Repeat<string>(string.Empty, 256).ToArray(), ArrayComparer.Instance },
                { "(string[])Enumerable.Repeat(string.Empty, 1024).ToArray(), ArrayComparer.Instance", typeof(string []), Enumerable.Repeat<string>(string.Empty, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_Uri_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(Uri[])null, ArrayComparer.Instance", typeof(Uri []), null, ArrayComparer.Instance },
                { "(Uri[])new Uri[0], ArrayComparer.Instance", typeof(Uri []), new Uri[0], ArrayComparer.Instance },
                { "(Uri[])new Uri[] {default(Uri)}, ArrayComparer.Instance", typeof(Uri []), new Uri[] {default(Uri)}, ArrayComparer.Instance },
                { "(Uri[])Enumerable.Repeat(new Uri(\"test:test\"), 2).ToArray(), ArrayComparer.Instance", typeof(Uri []), Enumerable.Repeat<Uri>(new Uri("test:test"), 2).ToArray(), ArrayComparer.Instance },
                { "(Uri[])Enumerable.Repeat(new Uri(\"test:test\"), 4).ToArray(), ArrayComparer.Instance", typeof(Uri []), Enumerable.Repeat<Uri>(new Uri("test:test"), 4).ToArray(), ArrayComparer.Instance },
                { "(Uri[])Enumerable.Repeat(new Uri(\"test:test\"), 16).ToArray(), ArrayComparer.Instance", typeof(Uri []), Enumerable.Repeat<Uri>(new Uri("test:test"), 16).ToArray(), ArrayComparer.Instance },
                { "(Uri[])Enumerable.Repeat(new Uri(\"test:test\"), 256).ToArray(), ArrayComparer.Instance", typeof(Uri []), Enumerable.Repeat<Uri>(new Uri("test:test"), 256).ToArray(), ArrayComparer.Instance },
                { "(Uri[])Enumerable.Repeat(new Uri(\"test:test\"), 1024).ToArray(), ArrayComparer.Instance", typeof(Uri []), Enumerable.Repeat<Uri>(new Uri("test:test"), 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableSbyte_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(sbyte?[])null, ArrayComparer.Instance", typeof(sbyte? []), null, ArrayComparer.Instance },
                { "(sbyte?[])new sbyte?[0], ArrayComparer.Instance", typeof(sbyte? []), new sbyte?[0], ArrayComparer.Instance },
                { "(sbyte?[])new sbyte?[] {42}, ArrayComparer.Instance", typeof(sbyte? []), new sbyte?[] {42}, ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (sbyte?)42 : null).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (sbyte?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(((sbyte?)0), 2).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(((sbyte?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(((sbyte?)0), 4).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(((sbyte?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(((sbyte?)0), 16).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(((sbyte?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(((sbyte?)0), 256).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(((sbyte?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(((sbyte?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(((sbyte?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(sbyte?[])Enumerable.Repeat(sbyte.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(sbyte? []), Enumerable.Repeat<sbyte?>(sbyte.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableByte_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(byte?[])null, ArrayComparer.Instance", typeof(byte? []), null, ArrayComparer.Instance },
                { "(byte?[])new byte?[0], ArrayComparer.Instance", typeof(byte? []), new byte?[0], ArrayComparer.Instance },
                { "(byte?[])new byte?[] {42}, ArrayComparer.Instance", typeof(byte? []), new byte?[] {42}, ArrayComparer.Instance },
                { "(byte?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (byte?)42 : null).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (byte?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(((byte?)0), 2).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(((byte?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(((byte?)0), 4).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(((byte?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(((byte?)0), 16).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(((byte?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(((byte?)0), 256).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(((byte?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(((byte?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(((byte?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(byte?[])Enumerable.Repeat(byte.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(byte? []), Enumerable.Repeat<byte?>(byte.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableChar_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(char?[])null, ArrayComparer.Instance", typeof(char? []), null, ArrayComparer.Instance },
                { "(char?[])new char?[0], ArrayComparer.Instance", typeof(char? []), new char?[0], ArrayComparer.Instance },
                { "(char?[])new char?[] {'a'}, ArrayComparer.Instance", typeof(char? []), new char?[] {'a'}, ArrayComparer.Instance },
                { "(char?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (char?)'a' : null).ToArray(), ArrayComparer.Instance", typeof(char? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (char?)'a' : null).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableShort_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(short?[])null, ArrayComparer.Instance", typeof(short? []), null, ArrayComparer.Instance },
                { "(short?[])new short?[0], ArrayComparer.Instance", typeof(short? []), new short?[0], ArrayComparer.Instance },
                { "(short?[])new short?[] {42}, ArrayComparer.Instance", typeof(short? []), new short?[] {42}, ArrayComparer.Instance },
                { "(short?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (short?)42 : null).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (short?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(((short?)0), 2).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(((short?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(((short?)0), 4).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(((short?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(((short?)0), 16).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(((short?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(((short?)0), 256).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(((short?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(((short?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(((short?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(short?[])Enumerable.Repeat(short.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(short? []), Enumerable.Repeat<short?>(short.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableUshort_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(ushort?[])null, ArrayComparer.Instance", typeof(ushort? []), null, ArrayComparer.Instance },
                { "(ushort?[])new ushort?[0], ArrayComparer.Instance", typeof(ushort? []), new ushort?[0], ArrayComparer.Instance },
                { "(ushort?[])new ushort?[] {42}, ArrayComparer.Instance", typeof(ushort? []), new ushort?[] {42}, ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ushort?)42 : null).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ushort?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(((ushort?)0), 2).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(((ushort?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(((ushort?)0), 4).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(((ushort?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(((ushort?)0), 16).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(((ushort?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(((ushort?)0), 256).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(((ushort?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(((ushort?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(((ushort?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ushort?[])Enumerable.Repeat(ushort.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ushort? []), Enumerable.Repeat<ushort?>(ushort.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(int?[])null, ArrayComparer.Instance", typeof(int? []), null, ArrayComparer.Instance },
                { "(int?[])new int?[0], ArrayComparer.Instance", typeof(int? []), new int?[0], ArrayComparer.Instance },
                { "(int?[])new int?[] {42}, ArrayComparer.Instance", typeof(int? []), new int?[] {42}, ArrayComparer.Instance },
                { "(int?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (int?)42 : null).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (int?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(((int?)0), 2).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(((int?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(((int?)0), 4).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(((int?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(((int?)0), 16).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(((int?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(((int?)0), 256).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(((int?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(((int?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(((int?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(int?[])Enumerable.Repeat(int.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(int? []), Enumerable.Repeat<int?>(int.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableUint_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(uint?[])null, ArrayComparer.Instance", typeof(uint? []), null, ArrayComparer.Instance },
                { "(uint?[])new uint?[0], ArrayComparer.Instance", typeof(uint? []), new uint?[0], ArrayComparer.Instance },
                { "(uint?[])new uint?[] {42}, ArrayComparer.Instance", typeof(uint? []), new uint?[] {42}, ArrayComparer.Instance },
                { "(uint?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (uint?)42 : null).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (uint?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(((uint?)0), 2).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(((uint?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(((uint?)0), 4).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(((uint?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(((uint?)0), 16).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(((uint?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(((uint?)0), 256).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(((uint?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(((uint?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(((uint?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(uint?[])Enumerable.Repeat(uint.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(uint? []), Enumerable.Repeat<uint?>(uint.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableLong_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(long?[])null, ArrayComparer.Instance", typeof(long? []), null, ArrayComparer.Instance },
                { "(long?[])new long?[0], ArrayComparer.Instance", typeof(long? []), new long?[0], ArrayComparer.Instance },
                { "(long?[])new long?[] {42}, ArrayComparer.Instance", typeof(long? []), new long?[] {42}, ArrayComparer.Instance },
                { "(long?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (long?)42 : null).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (long?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(((long?)0), 2).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(((long?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(((long?)0), 4).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(((long?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(((long?)0), 16).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(((long?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(((long?)0), 256).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(((long?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(((long?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(((long?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(long?[])Enumerable.Repeat(long.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(long? []), Enumerable.Repeat<long?>(long.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableUlong_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(ulong?[])null, ArrayComparer.Instance", typeof(ulong? []), null, ArrayComparer.Instance },
                { "(ulong?[])new ulong?[0], ArrayComparer.Instance", typeof(ulong? []), new ulong?[0], ArrayComparer.Instance },
                { "(ulong?[])new ulong?[] {42}, ArrayComparer.Instance", typeof(ulong? []), new ulong?[] {42}, ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ulong?)42 : null).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ulong?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(((ulong?)0), 2).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(((ulong?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(((ulong?)0), 4).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(((ulong?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(((ulong?)0), 16).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(((ulong?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(((ulong?)0), 256).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(((ulong?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(((ulong?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(((ulong?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(ulong?[])Enumerable.Repeat(ulong.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(ulong? []), Enumerable.Repeat<ulong?>(ulong.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableSingle_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(Single?[])null, ArrayComparer.Instance", typeof(Single? []), null, ArrayComparer.Instance },
                { "(Single?[])new Single?[0], ArrayComparer.Instance", typeof(Single? []), new Single?[0], ArrayComparer.Instance },
                { "(Single?[])new Single?[] {42}, ArrayComparer.Instance", typeof(Single? []), new Single?[] {42}, ArrayComparer.Instance },
                { "(Single?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (Single?)42 : null).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (Single?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(((Single?)0), 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(((Single?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(((Single?)0), 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(((Single?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(((Single?)0), 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(((Single?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(((Single?)0), 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(((Single?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(((Single?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(((Single?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.Epsilon, 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.Epsilon, 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.Epsilon, 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.Epsilon, 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.Epsilon, 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.Epsilon, 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.Epsilon, 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.Epsilon, 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.Epsilon, 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.Epsilon, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NaN, 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NaN, 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NaN, 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NaN, 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NaN, 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NaN, 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NaN, 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NaN, 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NaN, 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NaN, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(Single?[])Enumerable.Repeat(Single.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(Single? []), Enumerable.Repeat<Single?>(Single.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableDouble_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(double?[])null, ArrayComparer.Instance", typeof(double? []), null, ArrayComparer.Instance },
                { "(double?[])new double?[0], ArrayComparer.Instance", typeof(double? []), new double?[0], ArrayComparer.Instance },
                { "(double?[])new double?[] {42}, ArrayComparer.Instance", typeof(double? []), new double?[] {42}, ArrayComparer.Instance },
                { "(double?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (double?)42 : null).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (double?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(((double?)0), 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(((double?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(((double?)0), 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(((double?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(((double?)0), 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(((double?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(((double?)0), 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(((double?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(((double?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(((double?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.Epsilon, 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.Epsilon, 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.Epsilon, 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.Epsilon, 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.Epsilon, 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.Epsilon, 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.Epsilon, 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.Epsilon, 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.Epsilon, 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.Epsilon, 1024).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NaN, 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NaN, 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NaN, 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NaN, 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NaN, 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NaN, 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NaN, 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NaN, 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NaN, 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NaN, 1024).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(double?[])Enumerable.Repeat(double.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(double? []), Enumerable.Repeat<double?>(double.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableFloat_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(float?[])null, ArrayComparer.Instance", typeof(float? []), null, ArrayComparer.Instance },
                { "(float?[])new float?[0], ArrayComparer.Instance", typeof(float? []), new float?[0], ArrayComparer.Instance },
                { "(float?[])new float?[] {42}, ArrayComparer.Instance", typeof(float? []), new float?[] {42}, ArrayComparer.Instance },
                { "(float?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (float?)42 : null).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (float?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(((float?)0), 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(((float?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(((float?)0), 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(((float?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(((float?)0), 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(((float?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(((float?)0), 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(((float?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(((float?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(((float?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.Epsilon, 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.Epsilon, 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.Epsilon, 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.Epsilon, 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.Epsilon, 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.Epsilon, 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.Epsilon, 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.Epsilon, 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.Epsilon, 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.Epsilon, 1024).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NaN, 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NaN, 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NaN, 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NaN, 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NaN, 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NaN, 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NaN, 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NaN, 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NaN, 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NaN, 1024).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NegativeInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NegativeInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NegativeInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NegativeInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.NegativeInfinity, 1024).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.PositiveInfinity, 2).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.PositiveInfinity, 4).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.PositiveInfinity, 16).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.PositiveInfinity, 256).ToArray(), ArrayComparer.Instance },
                { "(float?[])Enumerable.Repeat(float.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance", typeof(float? []), Enumerable.Repeat<float?>(float.PositiveInfinity, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableDecimal_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(decimal?[])null, ArrayComparer.Instance", typeof(decimal? []), null, ArrayComparer.Instance },
                { "(decimal?[])new decimal?[0], ArrayComparer.Instance", typeof(decimal? []), new decimal?[0], ArrayComparer.Instance },
                { "(decimal?[])new decimal?[] {42}, ArrayComparer.Instance", typeof(decimal? []), new decimal?[] {42}, ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (decimal?)42 : null).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (decimal?)42 : null).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(((decimal?)0), 2).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(((decimal?)0), 2).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(((decimal?)0), 4).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(((decimal?)0), 4).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(((decimal?)0), 16).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(((decimal?)0), 16).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(((decimal?)0), 256).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(((decimal?)0), 256).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(((decimal?)0), 1024).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(((decimal?)0), 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.Zero, 2).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.Zero, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.Zero, 4).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.Zero, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.Zero, 16).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.Zero, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.Zero, 256).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.Zero, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.Zero, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.Zero, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.One, 2).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.One, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.One, 4).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.One, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.One, 16).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.One, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.One, 256).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.One, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.One, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.One, 1024).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinusOne, 2).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinusOne, 2).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinusOne, 4).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinusOne, 4).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinusOne, 16).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinusOne, 16).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinusOne, 256).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinusOne, 256).ToArray(), ArrayComparer.Instance },
                { "(decimal?[])Enumerable.Repeat(decimal.MinusOne, 1024).ToArray(), ArrayComparer.Instance", typeof(decimal? []), Enumerable.Repeat<decimal?>(decimal.MinusOne, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableBool_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(bool?[])null, ArrayComparer.Instance", typeof(bool? []), null, ArrayComparer.Instance },
                { "(bool?[])new bool?[0], ArrayComparer.Instance", typeof(bool? []), new bool?[0], ArrayComparer.Instance },
                { "(bool?[])new bool?[] {true}, ArrayComparer.Instance", typeof(bool? []), new bool?[] {true}, ArrayComparer.Instance },
                { "(bool?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (bool?)true : null).ToArray(), ArrayComparer.Instance", typeof(bool? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (bool?)true : null).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableDatetime_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(DateTime?[])null, ArrayComparer.Instance", typeof(DateTime? []), null, ArrayComparer.Instance },
                { "(DateTime?[])new DateTime?[0], ArrayComparer.Instance", typeof(DateTime? []), new DateTime?[0], ArrayComparer.Instance },
                { "(DateTime?[])new DateTime?[] {DateTime.Now}, ArrayComparer.Instance", typeof(DateTime? []), new DateTime?[] {DateTime.Now}, ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (DateTime?)DateTime.Now : null).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (DateTime?)DateTime.Now : null).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.Now, 2).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.Now, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.Now, 4).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.Now, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.Now, 16).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.Now, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.Now, 256).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.Now, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.Now, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.Now, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTime?[])Enumerable.Repeat(DateTime.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTime? []), Enumerable.Repeat<DateTime?>(DateTime.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableGuid_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(Guid?[])null, ArrayComparer.Instance", typeof(Guid? []), null, ArrayComparer.Instance },
                { "(Guid?[])new Guid?[0], ArrayComparer.Instance", typeof(Guid? []), new Guid?[0], ArrayComparer.Instance },
                { "(Guid?[])new Guid?[] {Guid.NewGuid()}, ArrayComparer.Instance", typeof(Guid? []), new Guid?[] {Guid.NewGuid()}, ArrayComparer.Instance },
                { "(Guid?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (Guid?)Guid.NewGuid() : null).ToArray(), ArrayComparer.Instance", typeof(Guid? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (Guid?)Guid.NewGuid() : null).ToArray(), ArrayComparer.Instance },
                { "(Guid?[])Enumerable.Repeat(Guid.Empty, 2).ToArray(), ArrayComparer.Instance", typeof(Guid? []), Enumerable.Repeat<Guid?>(Guid.Empty, 2).ToArray(), ArrayComparer.Instance },
                { "(Guid?[])Enumerable.Repeat(Guid.Empty, 4).ToArray(), ArrayComparer.Instance", typeof(Guid? []), Enumerable.Repeat<Guid?>(Guid.Empty, 4).ToArray(), ArrayComparer.Instance },
                { "(Guid?[])Enumerable.Repeat(Guid.Empty, 16).ToArray(), ArrayComparer.Instance", typeof(Guid? []), Enumerable.Repeat<Guid?>(Guid.Empty, 16).ToArray(), ArrayComparer.Instance },
                { "(Guid?[])Enumerable.Repeat(Guid.Empty, 256).ToArray(), ArrayComparer.Instance", typeof(Guid? []), Enumerable.Repeat<Guid?>(Guid.Empty, 256).ToArray(), ArrayComparer.Instance },
                { "(Guid?[])Enumerable.Repeat(Guid.Empty, 1024).ToArray(), ArrayComparer.Instance", typeof(Guid? []), Enumerable.Repeat<Guid?>(Guid.Empty, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableDatetimeoffset_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(DateTimeOffset?[])null, ArrayComparer.Instance", typeof(DateTimeOffset? []), null, ArrayComparer.Instance },
                { "(DateTimeOffset?[])new DateTimeOffset?[0], ArrayComparer.Instance", typeof(DateTimeOffset? []), new DateTimeOffset?[0], ArrayComparer.Instance },
                { "(DateTimeOffset?[])new DateTimeOffset?[] {DateTimeOffset.Now}, ArrayComparer.Instance", typeof(DateTimeOffset? []), new DateTimeOffset?[] {DateTimeOffset.Now}, ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (DateTimeOffset?)DateTimeOffset.Now : null).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (DateTimeOffset?)DateTimeOffset.Now : null).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.Now, 2).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.Now, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.Now, 4).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.Now, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.Now, 16).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.Now, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.Now, 256).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.Now, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.Now, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.Now, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(DateTimeOffset?[])Enumerable.Repeat(DateTimeOffset.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(DateTimeOffset? []), Enumerable.Repeat<DateTimeOffset?>(DateTimeOffset.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_NullableTimespan_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(TimeSpan?[])null, ArrayComparer.Instance", typeof(TimeSpan? []), null, ArrayComparer.Instance },
                { "(TimeSpan?[])new TimeSpan?[0], ArrayComparer.Instance", typeof(TimeSpan? []), new TimeSpan?[0], ArrayComparer.Instance },
                { "(TimeSpan?[])new TimeSpan?[] {TimeSpan.Zero}, ArrayComparer.Instance", typeof(TimeSpan? []), new TimeSpan?[] {TimeSpan.Zero}, ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (TimeSpan?)TimeSpan.Zero : null).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (TimeSpan?)TimeSpan.Zero : null).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.Zero, 2).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.Zero, 2).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.Zero, 4).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.Zero, 4).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.Zero, 16).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.Zero, 16).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.Zero, 256).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.Zero, 256).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.Zero, 1024).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.Zero, 1024).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MinValue, 2).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MinValue, 2).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MinValue, 4).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MinValue, 4).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MinValue, 16).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MinValue, 16).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MinValue, 256).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MinValue, 256).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MinValue, 1024).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MinValue, 1024).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(TimeSpan?[])Enumerable.Repeat(TimeSpan.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(TimeSpan? []), Enumerable.Repeat<TimeSpan?>(TimeSpan.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }


        public Tests BinarySerialization_ClassWithInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithInt[])null, DeepComparer.Instance", typeof(ClassWithInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithInt[])new ClassWithInt[0], DeepComparer.Instance", typeof(ClassWithInt []), new ClassWithInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithInt[])new ClassWithInt[] {new ClassWithInt() { IntValue = 42 } }, DeepComparer.Instance", typeof(ClassWithInt []), new ClassWithInt[] {new ClassWithInt() { IntValue = 42 } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithInt[])Enumerable.Range(0, 10).Select(i => ClassWithInt.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithInt []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithInt).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithInt[])new ClassWithInt[] { null }, DeepComparer.Instance", typeof(ClassWithInt []), new ClassWithInt[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithInt[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithInt)new ClassWithInt() { IntValue = 42 }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithInt []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithInt)new ClassWithInt() { IntValue = 42 }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithNullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithNullableInt[])null, DeepComparer.Instance", typeof(ClassWithNullableInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithNullableInt[])new ClassWithNullableInt[0], DeepComparer.Instance", typeof(ClassWithNullableInt []), new ClassWithNullableInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithNullableInt[])new ClassWithNullableInt[] {new ClassWithNullableInt() { NullableIntValue = 42 } }, DeepComparer.Instance", typeof(ClassWithNullableInt []), new ClassWithNullableInt[] {new ClassWithNullableInt() { NullableIntValue = 42 } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithNullableInt[])Enumerable.Range(0, 10).Select(i => ClassWithNullableInt.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithNullableInt []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithNullableInt).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithNullableInt[])new ClassWithNullableInt[] { null }, DeepComparer.Instance", typeof(ClassWithNullableInt []), new ClassWithNullableInt[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithNullableInt[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithNullableInt)new ClassWithNullableInt() { NullableIntValue = 42 }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithNullableInt []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithNullableInt)new ClassWithNullableInt() { NullableIntValue = 42 }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithIntArray[])null, DeepComparer.Instance", typeof(ClassWithIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithIntArray[])new ClassWithIntArray[0], DeepComparer.Instance", typeof(ClassWithIntArray []), new ClassWithIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithIntArray[])new ClassWithIntArray[] {new ClassWithIntArray() { IntArrayValue = new int [] {42} } }, DeepComparer.Instance", typeof(ClassWithIntArray []), new ClassWithIntArray[] {new ClassWithIntArray() { IntArrayValue = new int [] {42} } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithIntArray[])Enumerable.Range(0, 10).Select(i => ClassWithIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithIntArray).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithIntArray[])new ClassWithIntArray[] { null }, DeepComparer.Instance", typeof(ClassWithIntArray []), new ClassWithIntArray[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithIntArray[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithIntArray)new ClassWithIntArray() { IntArrayValue = new int [] {42} }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithIntArray []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithIntArray)new ClassWithIntArray() { IntArrayValue = new int [] {42} }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithNullableIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithNullableIntArray[])null, DeepComparer.Instance", typeof(ClassWithNullableIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithNullableIntArray[])new ClassWithNullableIntArray[0], DeepComparer.Instance", typeof(ClassWithNullableIntArray []), new ClassWithNullableIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithNullableIntArray[])new ClassWithNullableIntArray[] {new ClassWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} } }, DeepComparer.Instance", typeof(ClassWithNullableIntArray []), new ClassWithNullableIntArray[] {new ClassWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => ClassWithNullableIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithNullableIntArray).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithNullableIntArray[])new ClassWithNullableIntArray[] { null }, DeepComparer.Instance", typeof(ClassWithNullableIntArray []), new ClassWithNullableIntArray[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithNullableIntArray)new ClassWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithNullableIntArray)new ClassWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithInt[])null, DeepComparer.Instance", typeof(StructWithInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithInt[])new StructWithInt[0], DeepComparer.Instance", typeof(StructWithInt []), new StructWithInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithInt[])new StructWithInt[] {new StructWithInt() { IntValue = 42 } }, DeepComparer.Instance", typeof(StructWithInt []), new StructWithInt[] {new StructWithInt() { IntValue = 42 } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithInt[])Enumerable.Range(0, 10).Select(i => StructWithInt.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithInt []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithInt).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithNullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithNullableInt[])null, DeepComparer.Instance", typeof(StructWithNullableInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithNullableInt[])new StructWithNullableInt[0], DeepComparer.Instance", typeof(StructWithNullableInt []), new StructWithNullableInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithNullableInt[])new StructWithNullableInt[] {new StructWithNullableInt() { NullableIntValue = 42 } }, DeepComparer.Instance", typeof(StructWithNullableInt []), new StructWithNullableInt[] {new StructWithNullableInt() { NullableIntValue = 42 } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithNullableInt[])Enumerable.Range(0, 10).Select(i => StructWithNullableInt.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithNullableInt []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithNullableInt).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithIntArray[])null, DeepComparer.Instance", typeof(StructWithIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithIntArray[])new StructWithIntArray[0], DeepComparer.Instance", typeof(StructWithIntArray []), new StructWithIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithIntArray[])new StructWithIntArray[] {new StructWithIntArray() { IntArrayValue = new int [] {42} } }, DeepComparer.Instance", typeof(StructWithIntArray []), new StructWithIntArray[] {new StructWithIntArray() { IntArrayValue = new int [] {42} } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithIntArray[])Enumerable.Range(0, 10).Select(i => StructWithIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithIntArray).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithNullableIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithNullableIntArray[])null, DeepComparer.Instance", typeof(StructWithNullableIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithNullableIntArray[])new StructWithNullableIntArray[0], DeepComparer.Instance", typeof(StructWithNullableIntArray []), new StructWithNullableIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithNullableIntArray[])new StructWithNullableIntArray[] {new StructWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} } }, DeepComparer.Instance", typeof(StructWithNullableIntArray []), new StructWithNullableIntArray[] {new StructWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => StructWithNullableIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithNullableIntArray).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithClassWithInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithClassWithInt[])null, DeepComparer.Instance", typeof(ClassWithClassWithInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithClassWithInt[])new ClassWithClassWithInt[0], DeepComparer.Instance", typeof(ClassWithClassWithInt []), new ClassWithClassWithInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithClassWithInt[])new ClassWithClassWithInt[] {new ClassWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt } }, DeepComparer.Instance", typeof(ClassWithClassWithInt []), new ClassWithClassWithInt[] {new ClassWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithClassWithInt[])Enumerable.Range(0, 10).Select(i => ClassWithClassWithInt.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithInt []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithClassWithInt).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithClassWithInt[])new ClassWithClassWithInt[] { null }, DeepComparer.Instance", typeof(ClassWithClassWithInt []), new ClassWithClassWithInt[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithClassWithInt[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithInt)new ClassWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithInt []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithInt)new ClassWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithClassWithNullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithClassWithNullableInt[])null, DeepComparer.Instance", typeof(ClassWithClassWithNullableInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithClassWithNullableInt[])new ClassWithClassWithNullableInt[0], DeepComparer.Instance", typeof(ClassWithClassWithNullableInt []), new ClassWithClassWithNullableInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithClassWithNullableInt[])new ClassWithClassWithNullableInt[] {new ClassWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt } }, DeepComparer.Instance", typeof(ClassWithClassWithNullableInt []), new ClassWithClassWithNullableInt[] {new ClassWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithClassWithNullableInt[])Enumerable.Range(0, 10).Select(i => ClassWithClassWithNullableInt.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithNullableInt []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithClassWithNullableInt).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithClassWithNullableInt[])new ClassWithClassWithNullableInt[] { null }, DeepComparer.Instance", typeof(ClassWithClassWithNullableInt []), new ClassWithClassWithNullableInt[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithClassWithNullableInt[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithNullableInt)new ClassWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithNullableInt []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithNullableInt)new ClassWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithClassWithIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithClassWithIntArray[])null, DeepComparer.Instance", typeof(ClassWithClassWithIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithClassWithIntArray[])new ClassWithClassWithIntArray[0], DeepComparer.Instance", typeof(ClassWithClassWithIntArray []), new ClassWithClassWithIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithClassWithIntArray[])new ClassWithClassWithIntArray[] {new ClassWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray } }, DeepComparer.Instance", typeof(ClassWithClassWithIntArray []), new ClassWithClassWithIntArray[] {new ClassWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithClassWithIntArray[])Enumerable.Range(0, 10).Select(i => ClassWithClassWithIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithClassWithIntArray).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithClassWithIntArray[])new ClassWithClassWithIntArray[] { null }, DeepComparer.Instance", typeof(ClassWithClassWithIntArray []), new ClassWithClassWithIntArray[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithClassWithIntArray[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithIntArray)new ClassWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithIntArray []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithIntArray)new ClassWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithClassWithNullableIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithClassWithNullableIntArray[])null, DeepComparer.Instance", typeof(ClassWithClassWithNullableIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithClassWithNullableIntArray[])new ClassWithClassWithNullableIntArray[0], DeepComparer.Instance", typeof(ClassWithClassWithNullableIntArray []), new ClassWithClassWithNullableIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithClassWithNullableIntArray[])new ClassWithClassWithNullableIntArray[] {new ClassWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray } }, DeepComparer.Instance", typeof(ClassWithClassWithNullableIntArray []), new ClassWithClassWithNullableIntArray[] {new ClassWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithClassWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => ClassWithClassWithNullableIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithClassWithNullableIntArray).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithClassWithNullableIntArray[])new ClassWithClassWithNullableIntArray[] { null }, DeepComparer.Instance", typeof(ClassWithClassWithNullableIntArray []), new ClassWithClassWithNullableIntArray[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithClassWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithNullableIntArray)new ClassWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithClassWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithClassWithNullableIntArray)new ClassWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithStructWithInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithStructWithInt[])null, DeepComparer.Instance", typeof(ClassWithStructWithInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithStructWithInt[])new ClassWithStructWithInt[0], DeepComparer.Instance", typeof(ClassWithStructWithInt []), new ClassWithStructWithInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithStructWithInt[])new ClassWithStructWithInt[] {new ClassWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt } }, DeepComparer.Instance", typeof(ClassWithStructWithInt []), new ClassWithStructWithInt[] {new ClassWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithStructWithInt[])Enumerable.Range(0, 10).Select(i => ClassWithStructWithInt.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithInt []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithStructWithInt).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithStructWithInt[])new ClassWithStructWithInt[] { null }, DeepComparer.Instance", typeof(ClassWithStructWithInt []), new ClassWithStructWithInt[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithStructWithInt[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithInt)new ClassWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithInt []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithInt)new ClassWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithStructWithNullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithStructWithNullableInt[])null, DeepComparer.Instance", typeof(ClassWithStructWithNullableInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithStructWithNullableInt[])new ClassWithStructWithNullableInt[0], DeepComparer.Instance", typeof(ClassWithStructWithNullableInt []), new ClassWithStructWithNullableInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithStructWithNullableInt[])new ClassWithStructWithNullableInt[] {new ClassWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt } }, DeepComparer.Instance", typeof(ClassWithStructWithNullableInt []), new ClassWithStructWithNullableInt[] {new ClassWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithStructWithNullableInt[])Enumerable.Range(0, 10).Select(i => ClassWithStructWithNullableInt.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithNullableInt []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithStructWithNullableInt).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithStructWithNullableInt[])new ClassWithStructWithNullableInt[] { null }, DeepComparer.Instance", typeof(ClassWithStructWithNullableInt []), new ClassWithStructWithNullableInt[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithStructWithNullableInt[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithNullableInt)new ClassWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithNullableInt []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithNullableInt)new ClassWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithStructWithIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithStructWithIntArray[])null, DeepComparer.Instance", typeof(ClassWithStructWithIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithStructWithIntArray[])new ClassWithStructWithIntArray[0], DeepComparer.Instance", typeof(ClassWithStructWithIntArray []), new ClassWithStructWithIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithStructWithIntArray[])new ClassWithStructWithIntArray[] {new ClassWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray } }, DeepComparer.Instance", typeof(ClassWithStructWithIntArray []), new ClassWithStructWithIntArray[] {new ClassWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithStructWithIntArray[])Enumerable.Range(0, 10).Select(i => ClassWithStructWithIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithStructWithIntArray).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithStructWithIntArray[])new ClassWithStructWithIntArray[] { null }, DeepComparer.Instance", typeof(ClassWithStructWithIntArray []), new ClassWithStructWithIntArray[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithStructWithIntArray[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithIntArray)new ClassWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithIntArray []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithIntArray)new ClassWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_ClassWithStructWithNullableIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(ClassWithStructWithNullableIntArray[])null, DeepComparer.Instance", typeof(ClassWithStructWithNullableIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(ClassWithStructWithNullableIntArray[])new ClassWithStructWithNullableIntArray[0], DeepComparer.Instance", typeof(ClassWithStructWithNullableIntArray []), new ClassWithStructWithNullableIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(ClassWithStructWithNullableIntArray[])new ClassWithStructWithNullableIntArray[] {new ClassWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray } }, DeepComparer.Instance", typeof(ClassWithStructWithNullableIntArray []), new ClassWithStructWithNullableIntArray[] {new ClassWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(ClassWithStructWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => ClassWithStructWithNullableIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.ClassWithStructWithNullableIntArray).ToArray(), DeepComparer.Instance },
                /* array of 1 null value               */ { "(ClassWithStructWithNullableIntArray[])new ClassWithStructWithNullableIntArray[] { null }, DeepComparer.Instance", typeof(ClassWithStructWithNullableIntArray []), new ClassWithStructWithNullableIntArray[] { null }, DeepComparer.Instance },
                /* array of mixed null/non-null values */ { "(ClassWithStructWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithNullableIntArray)new ClassWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray }  : null).ToArray(), DeepComparer.Instance", typeof(ClassWithStructWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => i % 2 == 0? (ClassWithStructWithNullableIntArray)new ClassWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray }  : null).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithClassWithInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithClassWithInt[])null, DeepComparer.Instance", typeof(StructWithClassWithInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithClassWithInt[])new StructWithClassWithInt[0], DeepComparer.Instance", typeof(StructWithClassWithInt []), new StructWithClassWithInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithClassWithInt[])new StructWithClassWithInt[] {new StructWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt } }, DeepComparer.Instance", typeof(StructWithClassWithInt []), new StructWithClassWithInt[] {new StructWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithClassWithInt[])Enumerable.Range(0, 10).Select(i => StructWithClassWithInt.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithClassWithInt []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithClassWithInt).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithClassWithNullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithClassWithNullableInt[])null, DeepComparer.Instance", typeof(StructWithClassWithNullableInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithClassWithNullableInt[])new StructWithClassWithNullableInt[0], DeepComparer.Instance", typeof(StructWithClassWithNullableInt []), new StructWithClassWithNullableInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithClassWithNullableInt[])new StructWithClassWithNullableInt[] {new StructWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt } }, DeepComparer.Instance", typeof(StructWithClassWithNullableInt []), new StructWithClassWithNullableInt[] {new StructWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithClassWithNullableInt[])Enumerable.Range(0, 10).Select(i => StructWithClassWithNullableInt.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithClassWithNullableInt []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithClassWithNullableInt).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithClassWithIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithClassWithIntArray[])null, DeepComparer.Instance", typeof(StructWithClassWithIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithClassWithIntArray[])new StructWithClassWithIntArray[0], DeepComparer.Instance", typeof(StructWithClassWithIntArray []), new StructWithClassWithIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithClassWithIntArray[])new StructWithClassWithIntArray[] {new StructWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray } }, DeepComparer.Instance", typeof(StructWithClassWithIntArray []), new StructWithClassWithIntArray[] {new StructWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithClassWithIntArray[])Enumerable.Range(0, 10).Select(i => StructWithClassWithIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithClassWithIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithClassWithIntArray).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithClassWithNullableIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithClassWithNullableIntArray[])null, DeepComparer.Instance", typeof(StructWithClassWithNullableIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithClassWithNullableIntArray[])new StructWithClassWithNullableIntArray[0], DeepComparer.Instance", typeof(StructWithClassWithNullableIntArray []), new StructWithClassWithNullableIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithClassWithNullableIntArray[])new StructWithClassWithNullableIntArray[] {new StructWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray } }, DeepComparer.Instance", typeof(StructWithClassWithNullableIntArray []), new StructWithClassWithNullableIntArray[] {new StructWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithClassWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => StructWithClassWithNullableIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithClassWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithClassWithNullableIntArray).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithStructWithInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithStructWithInt[])null, DeepComparer.Instance", typeof(StructWithStructWithInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithStructWithInt[])new StructWithStructWithInt[0], DeepComparer.Instance", typeof(StructWithStructWithInt []), new StructWithStructWithInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithStructWithInt[])new StructWithStructWithInt[] {new StructWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt } }, DeepComparer.Instance", typeof(StructWithStructWithInt []), new StructWithStructWithInt[] {new StructWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithStructWithInt[])Enumerable.Range(0, 10).Select(i => StructWithStructWithInt.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithStructWithInt []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithStructWithInt).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithStructWithNullableInt_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithStructWithNullableInt[])null, DeepComparer.Instance", typeof(StructWithStructWithNullableInt []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithStructWithNullableInt[])new StructWithStructWithNullableInt[0], DeepComparer.Instance", typeof(StructWithStructWithNullableInt []), new StructWithStructWithNullableInt[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithStructWithNullableInt[])new StructWithStructWithNullableInt[] {new StructWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt } }, DeepComparer.Instance", typeof(StructWithStructWithNullableInt []), new StructWithStructWithNullableInt[] {new StructWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithStructWithNullableInt[])Enumerable.Range(0, 10).Select(i => StructWithStructWithNullableInt.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithStructWithNullableInt []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithStructWithNullableInt).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithStructWithIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithStructWithIntArray[])null, DeepComparer.Instance", typeof(StructWithStructWithIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithStructWithIntArray[])new StructWithStructWithIntArray[0], DeepComparer.Instance", typeof(StructWithStructWithIntArray []), new StructWithStructWithIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithStructWithIntArray[])new StructWithStructWithIntArray[] {new StructWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray } }, DeepComparer.Instance", typeof(StructWithStructWithIntArray []), new StructWithStructWithIntArray[] {new StructWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithStructWithIntArray[])Enumerable.Range(0, 10).Select(i => StructWithStructWithIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithStructWithIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithStructWithIntArray).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

        public Tests BinarySerialization_StructWithStructWithNullableIntArray_Arrays_Tests()
        {
            var tests = new Tests
            {
                /* null array               */ { "(StructWithStructWithNullableIntArray[])null, DeepComparer.Instance", typeof(StructWithStructWithNullableIntArray []), null, DeepComparer.Instance },
                /* empty array              */ { "(StructWithStructWithNullableIntArray[])new StructWithStructWithNullableIntArray[0], DeepComparer.Instance", typeof(StructWithStructWithNullableIntArray []), new StructWithStructWithNullableIntArray[0], DeepComparer.Instance },
                /* array of 1 value         */ { "(StructWithStructWithNullableIntArray[])new StructWithStructWithNullableIntArray[] {new StructWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray } }, DeepComparer.Instance", typeof(StructWithStructWithNullableIntArray []), new StructWithStructWithNullableIntArray[] {new StructWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray } }, DeepComparer.Instance },
                /* array of non-null values */ { "(StructWithStructWithNullableIntArray[])Enumerable.Range(0, 10).Select(i => StructWithStructWithNullableIntArray.RandValue).ToArray(), DeepComparer.Instance", typeof(StructWithStructWithNullableIntArray []), Enumerable.Range(0, 10).Select(i => RandValue.StructWithStructWithNullableIntArray).ToArray(), DeepComparer.Instance },
            };

            return tests;
        }

    }

    public static class RandValue
    {
        public static int Int = 42;
        public static int? NullableInt = 42;
        public static int [] IntArray = new int [] {42};
        public static int? [] NullableIntArray = new int? [] {42};
        public static DateTime DateTime = new DateTime(2042, 4, 2, 4, 2, 0);
        public static ClassWithInt ClassWithInt = new ClassWithInt() { IntValue = 42 } ;
        public static ClassWithNullableInt ClassWithNullableInt = new ClassWithNullableInt() { NullableIntValue = 42 } ;
        public static ClassWithIntArray ClassWithIntArray = new ClassWithIntArray() { IntArrayValue = new int [] {42} } ;
        public static ClassWithNullableIntArray ClassWithNullableIntArray = new ClassWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} } ;
        public static StructWithInt StructWithInt = new StructWithInt() { IntValue = 42 } ;
        public static StructWithNullableInt StructWithNullableInt = new StructWithNullableInt() { NullableIntValue = 42 } ;
        public static StructWithIntArray StructWithIntArray = new StructWithIntArray() { IntArrayValue = new int [] {42} } ;
        public static StructWithNullableIntArray StructWithNullableIntArray = new StructWithNullableIntArray() { NullableIntArrayValue = new int? [] {42} } ;
        public static ClassWithClassWithInt ClassWithClassWithInt = new ClassWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt } ;
        public static ClassWithClassWithNullableInt ClassWithClassWithNullableInt = new ClassWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt } ;
        public static ClassWithClassWithIntArray ClassWithClassWithIntArray = new ClassWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray } ;
        public static ClassWithClassWithNullableIntArray ClassWithClassWithNullableIntArray = new ClassWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray } ;
        public static ClassWithStructWithInt ClassWithStructWithInt = new ClassWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt } ;
        public static ClassWithStructWithNullableInt ClassWithStructWithNullableInt = new ClassWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt } ;
        public static ClassWithStructWithIntArray ClassWithStructWithIntArray = new ClassWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray } ;
        public static ClassWithStructWithNullableIntArray ClassWithStructWithNullableIntArray = new ClassWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray } ;
        public static StructWithClassWithInt StructWithClassWithInt = new StructWithClassWithInt() { ClassWithIntValue = RandValue.ClassWithInt } ;
        public static StructWithClassWithNullableInt StructWithClassWithNullableInt = new StructWithClassWithNullableInt() { ClassWithNullableIntValue = RandValue.ClassWithNullableInt } ;
        public static StructWithClassWithIntArray StructWithClassWithIntArray = new StructWithClassWithIntArray() { ClassWithIntArrayValue = RandValue.ClassWithIntArray } ;
        public static StructWithClassWithNullableIntArray StructWithClassWithNullableIntArray = new StructWithClassWithNullableIntArray() { ClassWithNullableIntArrayValue = RandValue.ClassWithNullableIntArray } ;
        public static StructWithStructWithInt StructWithStructWithInt = new StructWithStructWithInt() { StructWithIntValue = RandValue.StructWithInt } ;
        public static StructWithStructWithNullableInt StructWithStructWithNullableInt = new StructWithStructWithNullableInt() { StructWithNullableIntValue = RandValue.StructWithNullableInt } ;
        public static StructWithStructWithIntArray StructWithStructWithIntArray = new StructWithStructWithIntArray() { StructWithIntArrayValue = RandValue.StructWithIntArray } ;
        public static StructWithStructWithNullableIntArray StructWithStructWithNullableIntArray = new StructWithStructWithNullableIntArray() { StructWithNullableIntArrayValue = RandValue.StructWithNullableIntArray } ;
        public static BigStruct1_Permutation0 BigStruct1_Permutation0 = new BigStruct1_Permutation0() { P0_Int = RandValue.Int, P1_NullableIntArray = RandValue.NullableIntArray, P2_DateTime = RandValue.DateTime } ;
        public static BigStruct1_Permutation1 BigStruct1_Permutation1 = new BigStruct1_Permutation1() { P0_Int = RandValue.Int, P2_DateTime = RandValue.DateTime, P1_NullableIntArray = RandValue.NullableIntArray } ;
        public static BigStruct1_Permutation2 BigStruct1_Permutation2 = new BigStruct1_Permutation2() { P1_NullableIntArray = RandValue.NullableIntArray, P0_Int = RandValue.Int, P2_DateTime = RandValue.DateTime } ;
        public static BigStruct1_Permutation3 BigStruct1_Permutation3 = new BigStruct1_Permutation3() { P1_NullableIntArray = RandValue.NullableIntArray, P2_DateTime = RandValue.DateTime, P0_Int = RandValue.Int } ;
        public static BigStruct1_Permutation4 BigStruct1_Permutation4 = new BigStruct1_Permutation4() { P2_DateTime = RandValue.DateTime, P0_Int = RandValue.Int, P1_NullableIntArray = RandValue.NullableIntArray } ;
        public static BigStruct1_Permutation5 BigStruct1_Permutation5 = new BigStruct1_Permutation5() { P2_DateTime = RandValue.DateTime, P1_NullableIntArray = RandValue.NullableIntArray, P0_Int = RandValue.Int } ;
        public static BigStruct2_Permutation0 BigStruct2_Permutation0 = new BigStruct2_Permutation0() { P0_Int = RandValue.Int, P1_Int = RandValue.Int, P2_Int = RandValue.Int } ;
        public static BigStruct2_Permutation1 BigStruct2_Permutation1 = new BigStruct2_Permutation1() { P0_Int = RandValue.Int, P2_Int = RandValue.Int, P1_Int = RandValue.Int } ;
        public static BigStruct2_Permutation2 BigStruct2_Permutation2 = new BigStruct2_Permutation2() { P1_Int = RandValue.Int, P0_Int = RandValue.Int, P2_Int = RandValue.Int } ;
        public static BigStruct2_Permutation3 BigStruct2_Permutation3 = new BigStruct2_Permutation3() { P1_Int = RandValue.Int, P2_Int = RandValue.Int, P0_Int = RandValue.Int } ;
        public static BigStruct2_Permutation4 BigStruct2_Permutation4 = new BigStruct2_Permutation4() { P2_Int = RandValue.Int, P0_Int = RandValue.Int, P1_Int = RandValue.Int } ;
        public static BigStruct2_Permutation5 BigStruct2_Permutation5 = new BigStruct2_Permutation5() { P2_Int = RandValue.Int, P1_Int = RandValue.Int, P0_Int = RandValue.Int } ;
        public static BigStruct3_Permutation0 BigStruct3_Permutation0 = new BigStruct3_Permutation0() { P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigStruct3_Permutation1 BigStruct3_Permutation1 = new BigStruct3_Permutation1() { P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation2 BigStruct3_Permutation2 = new BigStruct3_Permutation2() { P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigStruct3_Permutation3 BigStruct3_Permutation3 = new BigStruct3_Permutation3() { P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation4 BigStruct3_Permutation4 = new BigStruct3_Permutation4() { P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation5 BigStruct3_Permutation5 = new BigStruct3_Permutation5() { P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation6 BigStruct3_Permutation6 = new BigStruct3_Permutation6() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigStruct3_Permutation7 BigStruct3_Permutation7 = new BigStruct3_Permutation7() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation8 BigStruct3_Permutation8 = new BigStruct3_Permutation8() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigStruct3_Permutation9 BigStruct3_Permutation9 = new BigStruct3_Permutation9() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int } ;
        public static BigStruct3_Permutation10 BigStruct3_Permutation10 = new BigStruct3_Permutation10() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation11 BigStruct3_Permutation11 = new BigStruct3_Permutation11() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigStruct3_Permutation12 BigStruct3_Permutation12 = new BigStruct3_Permutation12() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigStruct3_Permutation13 BigStruct3_Permutation13 = new BigStruct3_Permutation13() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation14 BigStruct3_Permutation14 = new BigStruct3_Permutation14() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigStruct3_Permutation15 BigStruct3_Permutation15 = new BigStruct3_Permutation15() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int } ;
        public static BigStruct3_Permutation16 BigStruct3_Permutation16 = new BigStruct3_Permutation16() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation17 BigStruct3_Permutation17 = new BigStruct3_Permutation17() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigStruct3_Permutation18 BigStruct3_Permutation18 = new BigStruct3_Permutation18() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation19 BigStruct3_Permutation19 = new BigStruct3_Permutation19() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation20 BigStruct3_Permutation20 = new BigStruct3_Permutation20() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation21 BigStruct3_Permutation21 = new BigStruct3_Permutation21() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigStruct3_Permutation22 BigStruct3_Permutation22 = new BigStruct3_Permutation22() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigStruct3_Permutation23 BigStruct3_Permutation23 = new BigStruct3_Permutation23() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigClass1_Permutation0 BigClass1_Permutation0 = new BigClass1_Permutation0() { P0_Int = RandValue.Int, P1_NullableIntArray = RandValue.NullableIntArray, P2_DateTime = RandValue.DateTime } ;
        public static BigClass1_Permutation1 BigClass1_Permutation1 = new BigClass1_Permutation1() { P0_Int = RandValue.Int, P2_DateTime = RandValue.DateTime, P1_NullableIntArray = RandValue.NullableIntArray } ;
        public static BigClass1_Permutation2 BigClass1_Permutation2 = new BigClass1_Permutation2() { P1_NullableIntArray = RandValue.NullableIntArray, P0_Int = RandValue.Int, P2_DateTime = RandValue.DateTime } ;
        public static BigClass1_Permutation3 BigClass1_Permutation3 = new BigClass1_Permutation3() { P1_NullableIntArray = RandValue.NullableIntArray, P2_DateTime = RandValue.DateTime, P0_Int = RandValue.Int } ;
        public static BigClass1_Permutation4 BigClass1_Permutation4 = new BigClass1_Permutation4() { P2_DateTime = RandValue.DateTime, P0_Int = RandValue.Int, P1_NullableIntArray = RandValue.NullableIntArray } ;
        public static BigClass1_Permutation5 BigClass1_Permutation5 = new BigClass1_Permutation5() { P2_DateTime = RandValue.DateTime, P1_NullableIntArray = RandValue.NullableIntArray, P0_Int = RandValue.Int } ;
        public static BigClass2_Permutation0 BigClass2_Permutation0 = new BigClass2_Permutation0() { P0_Int = RandValue.Int, P1_Int = RandValue.Int, P2_Int = RandValue.Int } ;
        public static BigClass2_Permutation1 BigClass2_Permutation1 = new BigClass2_Permutation1() { P0_Int = RandValue.Int, P2_Int = RandValue.Int, P1_Int = RandValue.Int } ;
        public static BigClass2_Permutation2 BigClass2_Permutation2 = new BigClass2_Permutation2() { P1_Int = RandValue.Int, P0_Int = RandValue.Int, P2_Int = RandValue.Int } ;
        public static BigClass2_Permutation3 BigClass2_Permutation3 = new BigClass2_Permutation3() { P1_Int = RandValue.Int, P2_Int = RandValue.Int, P0_Int = RandValue.Int } ;
        public static BigClass2_Permutation4 BigClass2_Permutation4 = new BigClass2_Permutation4() { P2_Int = RandValue.Int, P0_Int = RandValue.Int, P1_Int = RandValue.Int } ;
        public static BigClass2_Permutation5 BigClass2_Permutation5 = new BigClass2_Permutation5() { P2_Int = RandValue.Int, P1_Int = RandValue.Int, P0_Int = RandValue.Int } ;
        public static BigClass3_Permutation0 BigClass3_Permutation0 = new BigClass3_Permutation0() { P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigClass3_Permutation1 BigClass3_Permutation1 = new BigClass3_Permutation1() { P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation2 BigClass3_Permutation2 = new BigClass3_Permutation2() { P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigClass3_Permutation3 BigClass3_Permutation3 = new BigClass3_Permutation3() { P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation4 BigClass3_Permutation4 = new BigClass3_Permutation4() { P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation5 BigClass3_Permutation5 = new BigClass3_Permutation5() { P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation6 BigClass3_Permutation6 = new BigClass3_Permutation6() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigClass3_Permutation7 BigClass3_Permutation7 = new BigClass3_Permutation7() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation8 BigClass3_Permutation8 = new BigClass3_Permutation8() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigClass3_Permutation9 BigClass3_Permutation9 = new BigClass3_Permutation9() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int } ;
        public static BigClass3_Permutation10 BigClass3_Permutation10 = new BigClass3_Permutation10() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation11 BigClass3_Permutation11 = new BigClass3_Permutation11() { P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigClass3_Permutation12 BigClass3_Permutation12 = new BigClass3_Permutation12() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigClass3_Permutation13 BigClass3_Permutation13 = new BigClass3_Permutation13() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation14 BigClass3_Permutation14 = new BigClass3_Permutation14() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray } ;
        public static BigClass3_Permutation15 BigClass3_Permutation15 = new BigClass3_Permutation15() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int } ;
        public static BigClass3_Permutation16 BigClass3_Permutation16 = new BigClass3_Permutation16() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation17 BigClass3_Permutation17 = new BigClass3_Permutation17() { P2_StructWithNullableInt = RandValue.StructWithNullableInt, P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigClass3_Permutation18 BigClass3_Permutation18 = new BigClass3_Permutation18() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation19 BigClass3_Permutation19 = new BigClass3_Permutation19() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation20 BigClass3_Permutation20 = new BigClass3_Permutation20() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P2_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation21 BigClass3_Permutation21 = new BigClass3_Permutation21() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
        public static BigClass3_Permutation22 BigClass3_Permutation22 = new BigClass3_Permutation22() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int, P1_StructWithNullableInt = RandValue.StructWithNullableInt } ;
        public static BigClass3_Permutation23 BigClass3_Permutation23 = new BigClass3_Permutation23() { P3_StructWithNullableIntArray = RandValue.StructWithNullableIntArray, P2_StructWithNullableInt = RandValue.StructWithNullableInt, P1_StructWithNullableInt = RandValue.StructWithNullableInt, P0_Int = RandValue.Int } ;
    }

    public class ClassWithInt
    {
        [Mapping("datamodel:testtype/class/intvalue")]
        public int IntValue { get; set; }
    }

    public class ClassWithNullableInt
    {
        [Mapping("datamodel:testtype/class/nullableintvalue")]
        public int? NullableIntValue { get; set; }
    }

    public class ClassWithIntArray
    {
        [Mapping("datamodel:testtype/class/intarrayvalue")]
        public int [] IntArrayValue { get; set; }
    }

    public class ClassWithNullableIntArray
    {
        [Mapping("datamodel:testtype/class/nullableintarrayvalue")]
        public int? [] NullableIntArrayValue { get; set; }
    }

    public struct StructWithInt
    {
        [Mapping("datamodel:testtype/struct/intvalue")]
        public int IntValue { get; set; }
    }

    public struct StructWithNullableInt
    {
        [Mapping("datamodel:testtype/struct/nullableintvalue")]
        public int? NullableIntValue { get; set; }
    }

    public struct StructWithIntArray
    {
        [Mapping("datamodel:testtype/struct/intarrayvalue")]
        public int [] IntArrayValue { get; set; }
    }

    public struct StructWithNullableIntArray
    {
        [Mapping("datamodel:testtype/struct/nullableintarrayvalue")]
        public int? [] NullableIntArrayValue { get; set; }
    }

    public class ClassWithClassWithInt
    {
        [Mapping("datamodel:testtype/class/classwithintvalue")]
        public ClassWithInt ClassWithIntValue { get; set; }
    }

    public class ClassWithClassWithNullableInt
    {
        [Mapping("datamodel:testtype/class/classwithnullableintvalue")]
        public ClassWithNullableInt ClassWithNullableIntValue { get; set; }
    }

    public class ClassWithClassWithIntArray
    {
        [Mapping("datamodel:testtype/class/classwithintarrayvalue")]
        public ClassWithIntArray ClassWithIntArrayValue { get; set; }
    }

    public class ClassWithClassWithNullableIntArray
    {
        [Mapping("datamodel:testtype/class/classwithnullableintarrayvalue")]
        public ClassWithNullableIntArray ClassWithNullableIntArrayValue { get; set; }
    }

    public class ClassWithStructWithInt
    {
        [Mapping("datamodel:testtype/class/structwithintvalue")]
        public StructWithInt StructWithIntValue { get; set; }
    }

    public class ClassWithStructWithNullableInt
    {
        [Mapping("datamodel:testtype/class/structwithnullableintvalue")]
        public StructWithNullableInt StructWithNullableIntValue { get; set; }
    }

    public class ClassWithStructWithIntArray
    {
        [Mapping("datamodel:testtype/class/structwithintarrayvalue")]
        public StructWithIntArray StructWithIntArrayValue { get; set; }
    }

    public class ClassWithStructWithNullableIntArray
    {
        [Mapping("datamodel:testtype/class/structwithnullableintarrayvalue")]
        public StructWithNullableIntArray StructWithNullableIntArrayValue { get; set; }
    }

    public struct StructWithClassWithInt
    {
        [Mapping("datamodel:testtype/struct/classwithintvalue")]
        public ClassWithInt ClassWithIntValue { get; set; }
    }

    public struct StructWithClassWithNullableInt
    {
        [Mapping("datamodel:testtype/struct/classwithnullableintvalue")]
        public ClassWithNullableInt ClassWithNullableIntValue { get; set; }
    }

    public struct StructWithClassWithIntArray
    {
        [Mapping("datamodel:testtype/struct/classwithintarrayvalue")]
        public ClassWithIntArray ClassWithIntArrayValue { get; set; }
    }

    public struct StructWithClassWithNullableIntArray
    {
        [Mapping("datamodel:testtype/struct/classwithnullableintarrayvalue")]
        public ClassWithNullableIntArray ClassWithNullableIntArrayValue { get; set; }
    }

    public struct StructWithStructWithInt
    {
        [Mapping("datamodel:testtype/struct/structwithintvalue")]
        public StructWithInt StructWithIntValue { get; set; }
    }

    public struct StructWithStructWithNullableInt
    {
        [Mapping("datamodel:testtype/struct/structwithnullableintvalue")]
        public StructWithNullableInt StructWithNullableIntValue { get; set; }
    }

    public struct StructWithStructWithIntArray
    {
        [Mapping("datamodel:testtype/struct/structwithintarrayvalue")]
        public StructWithIntArray StructWithIntArrayValue { get; set; }
    }

    public struct StructWithStructWithNullableIntArray
    {
        [Mapping("datamodel:testtype/struct/structwithnullableintarrayvalue")]
        public StructWithNullableIntArray StructWithNullableIntArrayValue { get; set; }
    }

    public struct BigStruct1_Permutation0
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
    }

    public struct BigStruct1_Permutation1
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/struct/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
    }

    public struct BigStruct1_Permutation2
    {
        [Mapping("datamodel:testtype/struct/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
    }

    public struct BigStruct1_Permutation3
    {
        [Mapping("datamodel:testtype/struct/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct1_Permutation4
    {
        [Mapping("datamodel:testtype/struct/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
    }

    public struct BigStruct1_Permutation5
    {
        [Mapping("datamodel:testtype/struct/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/struct/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct2_Permutation0
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_int")]
        public int P2_Int { get; set; }
    }

    public struct BigStruct2_Permutation1
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_int")]
        public int P1_Int { get; set; }
    }

    public struct BigStruct2_Permutation2
    {
        [Mapping("datamodel:testtype/struct/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_int")]
        public int P2_Int { get; set; }
    }

    public struct BigStruct2_Permutation3
    {
        [Mapping("datamodel:testtype/struct/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct2_Permutation4
    {
        [Mapping("datamodel:testtype/struct/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_int")]
        public int P1_Int { get; set; }
    }

    public struct BigStruct2_Permutation5
    {
        [Mapping("datamodel:testtype/struct/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct3_Permutation0
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public struct BigStruct3_Permutation1
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation2
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public struct BigStruct3_Permutation3
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation4
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation5
    {
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation6
    {
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public struct BigStruct3_Permutation7
    {
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation8
    {
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public struct BigStruct3_Permutation9
    {
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct3_Permutation10
    {
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation11
    {
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct3_Permutation12
    {
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public struct BigStruct3_Permutation13
    {
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation14
    {
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public struct BigStruct3_Permutation15
    {
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct3_Permutation16
    {
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation17
    {
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct3_Permutation18
    {
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation19
    {
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation20
    {
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation21
    {
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public struct BigStruct3_Permutation22
    {
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public struct BigStruct3_Permutation23
    {
        [Mapping("datamodel:testtype/struct/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/struct/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/struct/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass1_Permutation0
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
    }

    public class BigClass1_Permutation1
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/class/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
    }

    public class BigClass1_Permutation2
    {
        [Mapping("datamodel:testtype/class/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
    }

    public class BigClass1_Permutation3
    {
        [Mapping("datamodel:testtype/class/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass1_Permutation4
    {
        [Mapping("datamodel:testtype/class/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
    }

    public class BigClass1_Permutation5
    {
        [Mapping("datamodel:testtype/class/p2_datetime")]
        public DateTime P2_DateTime { get; set; }
        [Mapping("datamodel:testtype/class/p1_nullableintarray")]
        public int? [] P1_NullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass2_Permutation0
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_int")]
        public int P2_Int { get; set; }
    }

    public class BigClass2_Permutation1
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_int")]
        public int P1_Int { get; set; }
    }

    public class BigClass2_Permutation2
    {
        [Mapping("datamodel:testtype/class/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_int")]
        public int P2_Int { get; set; }
    }

    public class BigClass2_Permutation3
    {
        [Mapping("datamodel:testtype/class/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass2_Permutation4
    {
        [Mapping("datamodel:testtype/class/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_int")]
        public int P1_Int { get; set; }
    }

    public class BigClass2_Permutation5
    {
        [Mapping("datamodel:testtype/class/p2_int")]
        public int P2_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_int")]
        public int P1_Int { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass3_Permutation0
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public class BigClass3_Permutation1
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation2
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public class BigClass3_Permutation3
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation4
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation5
    {
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation6
    {
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public class BigClass3_Permutation7
    {
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation8
    {
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public class BigClass3_Permutation9
    {
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass3_Permutation10
    {
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation11
    {
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass3_Permutation12
    {
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public class BigClass3_Permutation13
    {
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation14
    {
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
    }

    public class BigClass3_Permutation15
    {
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass3_Permutation16
    {
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation17
    {
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass3_Permutation18
    {
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation19
    {
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation20
    {
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation21
    {
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

    public class BigClass3_Permutation22
    {
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
    }

    public class BigClass3_Permutation23
    {
        [Mapping("datamodel:testtype/class/p3_structwithnullableintarray")]
        public StructWithNullableIntArray P3_StructWithNullableIntArray { get; set; }
        [Mapping("datamodel:testtype/class/p2_structwithnullableint")]
        public StructWithNullableInt P2_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p1_structwithnullableint")]
        public StructWithNullableInt P1_StructWithNullableInt { get; set; }
        [Mapping("datamodel:testtype/class/p0_int")]
        public int P0_Int { get; set; }
    }

}

