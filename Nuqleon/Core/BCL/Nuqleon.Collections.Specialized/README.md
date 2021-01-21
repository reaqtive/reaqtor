# `Nuqleon.Collections.Specialized`

Provides specialized collection types.

## Bit arrays

`IBitArray` represents a fixed-sized bit array whose bits can be set and tested. `BitArrayFactory` can be used to construct bit array instances, given a length.

```csharp
IBitArray bits = BitArrayFactory.Create(5);
bits[0] = true;
bits[2] = true;
bits[3] = true;

bool isSet1 = bits[1];
```

Other members include `Count` to count the number of bits set, and `SetAll(bool)` to clear or set all bits.

## Enum dictionaries

`EnumDictionary` provides an efficient `IDictionary<TKey, TValue>` implementation where `TKey` is an enumeration type.

```csharp
IDictionary<ConsoleColor, string> map = EnumDictionary.Create<ConsoleColor, string>();
map[ConsoleColor.Red] = "Red";
map[ConsoleColor.Green] = "Green";
map[ConsoleColor.Blue] = "Blue";
```

> This type is used by Reactor Core for efficient storage of metric values which are indexed using an enumeration value.
