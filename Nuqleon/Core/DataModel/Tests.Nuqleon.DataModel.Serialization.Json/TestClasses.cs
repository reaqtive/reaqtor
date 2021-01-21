// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// EK - June 2013
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nuqleon.DataModel.Serialization.JsonTest
{
    public enum EnumByte : byte
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumSbyte : sbyte
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumInt
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumUint : uint
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumShort : short
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumUshort : ushort
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumLong : long
    {
        EnumValue1,
        EnumValue2
    }

    public enum EnumUlong : ulong
    {
        EnumValue1,
        EnumValue2
    }

    public class ClassWithByteEnumMember
    {
        [Mapping("contoso://entities/enumclass/typeEnum")]
        public EnumByte EnumField { get; set; }
    }

    public class ClassWithShortEnumMember
    {
        [Mapping("contoso://entities/enumclass/typeEnum")]
        public EnumShort EnumField { get; set; }
    }

    public class FlatClass : IEquatable<FlatClass>
    {
        [Mapping("contoso://entities/flat/typebool")]
        public bool TypeBool { get; set; }

        [Mapping("contoso://entities/flat/typeshort")]
        public short TypeShort { get; set; }

        [Mapping("contoso://entities/flat/typeint")]
        public int TypeInt { get; set; }

        [Mapping("contoso://entities/flat/typenullableint")]
        public int? TypeNullableInt { get; set; }

        [Mapping("contoso://entities/flat/typelong")]
        public long TypeLong { get; set; }

        [Mapping("contoso://entities/flat/typeushort")]
        public ushort TypeUshort { get; set; }

        [Mapping("contoso://entities/flat/typeuint")]
        public uint TypeUint { get; set; }

        [Mapping("contoso://entities/flat/typeulong")]
        public ulong TypeUlong { get; set; }

        [Mapping("contoso://entities/flat/typestring")]
        public string TypeString { get; set; }

        [Mapping("contoso://entities/flat/typefloat")]
        public float TypeFloat { get; set; }

        [Mapping("contoso://entities/flat/typedouble")]
        public double TypeDouble { get; set; }

        [Mapping("contoso://entities/flat/typedatetime")]
        public DateTime TypeDateTime { get; set; }

        [Mapping("contoso://entities/flat/typeuri")]
        public Uri TypeUri { get; set; }

        [Mapping("contoso://entities/flat/typedatetimeoffset")]
        public DateTimeOffset TypeDateTimeOffset { get; set; }

        [Mapping("contoso://entities/flat/typetimespan")]
        public TimeSpan TypeTimeSpan { get; set; }

        [Mapping("contoso://entities/parent/typeguid")]
        public Guid TypeGuid { get; set; }

        [Mapping("contoso://entities/flat/typeienumerablebool")]
        public IEnumerable<bool> TypeIEnumerableBool { get; set; }

        [Mapping("contoso://entities/flat/typearraybool")]
        public bool[] TypeArrayBool { get; set; }

        [Mapping("contoso://entities/flat/typeenumint")]
        public EnumInt TypeEnumInt { get; set; }

        [Mapping("contoso://entities/flat/typenullableenumint")]
        public EnumInt? TypeNullableEnumInt { get; set; }

        public static FlatClass Create(int seed = 13)
        {
            var r = new Random(seed);
            return new FlatClass
            {
                TypeBool = (r.Next() % 2 == 1),
                TypeShort = (short)r.Next(),
                TypeInt = r.Next(),
                TypeNullableInt = null,
                TypeLong = r.Next(),
                TypeUshort = (ushort)r.Next(),
                TypeUint = (uint)r.Next(),
                TypeUlong = (ulong)r.Next(),
                TypeString = "Test" + r.Next().ToString(),
                TypeFloat = (float)r.NextDouble(),
                TypeDouble = r.NextDouble(),
                TypeDateTime = DateTime.Now,
                TypeUri = new Uri("contoso://entities/flat"),
                TypeDateTimeOffset = DateTime.Now,
                TypeTimeSpan = new TimeSpan(1, 2, 3, 4, 5),
                TypeGuid = Guid.NewGuid(),
                TypeIEnumerableBool = new[] { true, false, true },
                TypeArrayBool = new[] { false, true, false },
                TypeEnumInt = EnumInt.EnumValue1,
                TypeNullableEnumInt = EnumInt.EnumValue2,
            };
        }

        public bool Equals(FlatClass other)
        {
            return TypeBool == other.TypeBool
                && TypeShort == other.TypeShort
                && TypeInt == other.TypeInt
                && TypeNullableInt == other.TypeNullableInt
                && TypeLong == other.TypeLong
                && TypeUshort == other.TypeUshort
                && TypeUint == other.TypeUint
                && TypeUlong == other.TypeUlong
                && TypeUri == other.TypeUri
                && TypeString == other.TypeString
                // Ignoring floating precision for the test.
                && TypeFloat == other.TypeFloat
                && TypeDouble == other.TypeDouble
                && TypeDateTime == other.TypeDateTime
                && TypeUri == other.TypeUri
                && TypeDateTimeOffset == other.TypeDateTimeOffset
                && TypeTimeSpan == other.TypeTimeSpan
                && TypeGuid == other.TypeGuid
                && ((TypeIEnumerableBool == null && other.TypeIEnumerableBool == null)
                || (TypeIEnumerableBool != null && TypeIEnumerableBool.SequenceEqual(other.TypeIEnumerableBool)))
                && ((TypeArrayBool == null && other.TypeArrayBool == null)
                     || (TypeArrayBool != null && TypeArrayBool.SequenceEqual(other.TypeArrayBool)))
                && TypeEnumInt == other.TypeEnumInt
                && TypeNullableEnumInt == other.TypeNullableEnumInt;
        }

        public override bool Equals(object obj) => obj is FlatClass other ? Equals(other) : base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }

    public class NestedClass : IEquatable<NestedClass>
    {
        public int NotSerializable { get; set; }

        [Mapping("contoso://entities/nested/serializable")]
        public int Serializable { get; set; }

        [Mapping("contoso://entities/nested/typeflat")]
        public FlatClass TypeFlat { get; set; }

        public static NestedClass Create(int seed = 13)
        {
            var r = new Random(seed);
            return new NestedClass()
            {
                NotSerializable = r.Next(),
                Serializable = r.Next(),
                TypeFlat = FlatClass.Create(seed)
            };
        }

        public bool Equals(NestedClass other)
        {
            return other != null
                && Serializable == other.Serializable
                && TypeFlat.Equals(other.TypeFlat);
        }

        public override bool Equals(object obj) => obj is NestedClass other ? Equals(other) : base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }

    public class EmptyClass
    {
        public int NotSerializable { get; set; }
    }

    public class RecursiveClass : IEquatable<RecursiveClass>
    {
        [Mapping("contoso://entities/parent/typestring")]
        public string TypeString { get; set; }

        [Mapping("contoso://entities/parent/recursive")]
        public RecursiveClass Recursive { get; set; }

        public static RecursiveClass Create()
        {
            return new RecursiveClass
            {
                TypeString = "Hello",
                Recursive =
                               new RecursiveClass
                               {
                                   TypeString = ",",
                                   Recursive =
                                           new RecursiveClass
                                           {
                                               TypeString = "World",
                                               Recursive = null
                                           }
                               }
            };
        }

        public bool Equals(RecursiveClass other)
        {
            return other != null
                && TypeString == other.TypeString
                && ((Recursive == null && other.Recursive == null)
                    || (Recursive != null && Recursive.Equals(other.Recursive)));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RecursiveClass);
        }

        public override int GetHashCode()
        {
            return Recursive == null
                       ? TypeString.GetHashCode()
                       : TypeString.GetHashCode() ^ Recursive.GetHashCode();
        }
    }

    public class InheritedClass : FlatClass, IEquatable<InheritedClass>
    {
        [Mapping("contoso://entities/inherited/typeint")]
        public new int TypeInt { get; set; }

        public static new InheritedClass Create(int seed = 13)
        {
            var r = new Random(seed);
            return new InheritedClass { TypeInt = r.Next(), TypeLong = r.Next() };
        }

        public override bool Equals(object obj) => obj is InheritedClass c && Equals(c);

        public override int GetHashCode() => 0; // NB: Test code.

        public bool Equals(InheritedClass other)
        {
            return other != null
                && TypeInt == other.TypeInt
                && base.Equals(other);
        }
    }

    public class DuplicateMappingClass
    {
        [Mapping("contoso://entities/duplicate/typestring")]
        public string TypeString { get; set; }

        [Mapping("contoso://entities/duplicate/typestring")]
        public int TypeInt { get; set; }
    }

    public class CollectionsClass : IEquatable<CollectionsClass>
    {
        [Mapping("contoso://entities/collections/typehash")]
        public HashSet<string> TypeHash { get; set; }

        [Mapping("contoso://entities/collections/typelist")]
        public List<string> TypeList { get; set; }

        [Mapping("contoso://entities/collections/typedictionary")]
        public Dictionary<string, RecursiveClass> TypeDictionary { get; set; }

        public override bool Equals(object obj) => obj is CollectionsClass c && Equals(c);

        public override int GetHashCode() => 0; // NB: Test code.

        public bool Equals(CollectionsClass other)
        {
            return other != null
                   && ((TypeHash == null && other.TypeHash == null)
                       || (TypeHash != null && TypeHash.SequenceEqual(other.TypeHash)))
                   && ((TypeList == null && other.TypeList == null)
                       || (TypeList != null && TypeList.SequenceEqual(other.TypeList)))
                   && ((TypeDictionary == null && other.TypeDictionary == null) ||
                       (TypeDictionary != null && TypeDictionary.SequenceEqual(other.TypeDictionary)));
        }

        public static CollectionsClass Create()
        {
            return new CollectionsClass
            {
                TypeHash = new HashSet<string> { "Goodbye", ",", "world!" },
                TypeList =
                               new List<string> { "It", "was", "a", "pleasure", "to", "be", "here." },
                TypeDictionary =
                               new Dictionary<string, RecursiveClass>
                                   {
                                       { "Hello", RecursiveClass.Create() },
                                       { "Valhalla", RecursiveClass.Create() }
                                   }
            };
        }
    }

    public class TupleClass : Tuple<int>
    {
        public TupleClass(int item1) : base(item1)
        {
        }

        public int Property { get; set; }
    }

    public class DictionaryWithMapping : Dictionary<string, int>
    {
        [Mapping("contoso://entities/withmapping/mappingfield")]
        public int MappingField { get; set; }
    }

    public class ListWithMapping : List<string>
    {
        [Mapping("contoso://entities/withmapping/mappingfield")]
        public int MappingField { get; set; }
    }

    public class Person
    {
        [Mapping("contoso://entities/person/name")]
        public string Name { get; set; }

        [Mapping("contoso://entities/person/age")]
        public int Age { get; set; }
    }

    public class ExpressionContainer
    {
        [Mapping("contoso://entities/expression")]
        public ConstantExpression Expression { get; set; }
    }

    public class ExpressionContainer2
    {
        [Mapping("contoso://entities/expression")]
        public Expression Expression { get; set; }

        [Mapping("contoso://entities/expression2")]
        public ConstantExpression Expression2 { get; set; }
    }

    public class CheckpointState : IEquatable<CheckpointState>
    {
        [Mapping("contoso://entities/state/id")]
        private Uri Id { get; set; }

        [Mapping("contoso://entities/state/userstate")]
        private FlatClass userState;

        public CheckpointState()
        {
        }

        protected CheckpointState(Uri id, FlatClass userState)
        {
            Id = id;
            this.userState = userState;
        }

        public bool Equals(CheckpointState other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id &&
                   (userState == null ? other.userState == null : userState.Equals(other.userState));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CheckpointState);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static CheckpointState Create()
        {
            return new CheckpointState
            {
                Id = new Uri("bing://testuri/"),
                userState = FlatClass.Create()
            };
        }
    }

    public class InheritedCheckpointState : CheckpointState, IEquatable<InheritedCheckpointState>
    {
        [Mapping("contoso://entities/inherited/id")]
        private int AnotherId { get; set; }

#pragma warning disable IDE0044 // Add readonly modifier (serializer won't like it)
        [Mapping("contoso://entities/inherited/state")]
        private double _anotherState;
#pragma warning restore IDE0044 // Add readonly modifier

        public InheritedCheckpointState()
        {
        }

        private InheritedCheckpointState(double anotherState, int anotherId, Uri id, FlatClass userState)
            : base(id, userState)
        {
            AnotherId = anotherId;
            _anotherState = anotherState;
        }

        public bool Equals(InheritedCheckpointState other)
        {
            if (other == null)
            {
                return false;
            }

            return AnotherId == other.AnotherId
                && _anotherState == other._anotherState
                && base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InheritedCheckpointState);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static InheritedCheckpointState Create(int seed = 13)
        {
            var r = new Random(seed);
            return new InheritedCheckpointState(
                r.NextDouble(),
                r.Next(100),
                new Uri("bing://test"),
                FlatClass.Create());
        }
    }
}
