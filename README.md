# FlexBuffers-CSharp
FlexBuffers is JSON comparable binary format with random value access capabilities.
The binary format and data layout was designed at Google as part of [FlatBuffers project](https://google.github.io/flatbuffers/flexbuffers.html).
This projects brings FlexBuffers as a standalone solution to C# with the focus to convert large JSON files to randomly accessable FlexBuffers. 

The main focus of this project is to be used with Unity3D. 
However current solution has no Unity3D dependencies and should be usable in other C# environments.
Please feel free to contribute tests and patches for other environments.

## Supported types
As mentioned in the project description, FlexBuffers is a JSON comparable data format. It supports all the types JSON does, but also stores more detailed informations about the values.

Here is the list of supported types, as of November 2019:
- `Null` represents an absence of a value, specifically useful in collections
- `Int` represents positive and negative integer numbers, FlexBuffers additionally stores the bit width of the number which can be 8, 16, 32 or 64 bits
- `UInt` represents only positive numbers, also stores the bit width of 8, 16, 32 or 64 bits
- `Float` represents a floating point number. The possible bit width is limited to 32 and 64
- `Key` an internal zero terminated string representation for `Map` keys. Is not relevant for the end users
- `String` a length prepanded, zero terminated, UTF-8 encoded text representation
- `IndirectInt`, `IndirectUInt`, `IndirectFloat` are `Int`, `UInt` and `Float` stored by reference. Relevant for end users only if they want to achieve smallest binary size (will be discussed in a separate section)
- `Map` is similar to JSON object. Keys are strings, values can be any other here listed supported type. Speaking in terms of C# it is a `Dictionary<string, dyanmic>`
- `Vector` is similar to JSON array. An array which can host any types. FlexBuffers stores the values together with value type information
- `VecorInt`, `VectorUInt`, `VectorFloat`, `VectorKey`, `VectorString`, `VectorBool` are arrays of a given type. Given this type FlatBuffer does not store the type information for every individual entry and there for achieves smaller buffer size
- `VectorInt2`, `VectorInt3`, `VectorInt4`, `VectorUInt2`, `VectorUInt3`, `VectorUInt4`, `VectorFloat2`, `VectorFloat3`, `VectorFloat4` is a special type of array which has a fix type and fix size of elements. A vector with non fixed size need to store size, however fix sized vectors don't have to do it as it is encoded in type directly
- `Blob` stores a byte array
- `Bool` stores bnoolean values `true` or `false`

## Creation of FlexBuffer
There are multple ways how we can create a FlexBuffer

### Single value
It is posible to store just one value in a FlexBuffer. It is not that probable to do so in day to day business, but was helpful for unit testing the format.
- `FlexBuffer.Null()` returns a byte array, which represents a FlexBuffer with `null` as single value.
- `FlexBuffer.SingleValue(value)` returns a byte array, which represents a FlexBuffer with as single value. The `value` parameter can be of type `long`, `ulong`, `string`, `bool`
- `FlexBuffer.SingleValue(x, y)`, `FlexBuffer.SingleValue(x, y, z)`, `FlexBuffer.SingleValue(x, y, z, w)` creates a typed fixed size vector. Parameters `x`, `y`, `z`, `w` can be of type `long`, `ulong` or `double`
- `FlexBuffer.From(dict)` creates a FlexBuffer based on a `Dictionary<string, dynamic>` instance. It is not the fastest but very convinient way of creating a FlexBuffer. It assumes that the values are of compatible types
- `FlexBuffer.From(value)` where `value` is of type `IEnumerable` will try to create a FlexBuffer with `Vector` as root element
- `FlexBuffer.SingleValue(new byte[]{1, 2, 3})`creates a FlexBuffer with `Blob` as single value.

### From JSON
FlexBuffers-CSharp has a special type called `JsonToFlexBufferConverter` which allows user to convert a JSON string to FlexBuffer byte array. 
```
var buffer = JsonToFlexBufferConverter.Convert("{\"a\":1, \"b\":2}");
```

`JsonToFlexBufferConverter` Contains an actualt JSON parser (based on [LightJson](https://github.com/MarcosLopezC/LightJson)) and there for converts one byte array (JSON string) directly to another byte aray (FlexBuffer) efficient and with a minimal amount of temporary objects.

### FlexBuffer Builder
The `FlexBufferBuilder` enable users to create FlexBuffers directly form values, without a need for temporary representations.
The Root elelemnt can be defined as a `Map`:
```
var bytes = FlexBufferBuilder.Map(root =>
{
    root.Add("name", "Maxim");
    root.Add("age", 38);
    root.Add("weight", 72.5);
    root.Map("address", address =>
    {
        address.Add("city", "Bla");
        address.Add("zip", "12345");
        address.Add("countryCode", "XX");
    });
    root.Vector("flags", tags =>
    {
        tags.Add(true);
        tags.Add(false);
        tags.Add(true);
        tags.Add(true);
    });
});
```

Or as a `Vector`:
```
var bytes = FlexBufferBuilder.Vector(root =>
{
    root.AddNull();
    root.Add(new byte[]{1,2,3});
    root.Map(map =>
    {
        map.AddNull("a");
        map.Add("b", new byte[]{3,4,5});
    });
});
```

## FlexBuffers to JSON
As Flexbuffer has JSON compatible types it is very easy to conver a FlexBuffer to JSON.

The FlexBuffer from previous paragraph can be converted to following JSON string:
`[null,"AQID",{"a":null,"b":"AwQF"}]`
`"AQID"` and `"AwQF"` are Base64 representations of `new byte[]{1,2,3}` and `new byte[]{3,4,5}`

## Byte array to FlexBuffer value
With FlexBuffers we can extract values directly from the buffer, without any parsing or complex upfront conversions.
With `var flx = FlxValue.FromBytes(bytes);` users can create an instance of a FlexBuffer value which allows conversion to types and access of sub elements.
`FlxValue` struct has following getters:
- `ValueType` returns the `Type` enum representing the value type (types are the cases listed Supported Types section)
- `IsNull` return `true` or `false` if the value is `null`
- `AsLong`, `AsULong`, `AsDouble`, `AsBool`, `AsString` and `AsBlob` tries to interpret the undelying value as expected type and returns the value
- `AsVector` checks if the underlying value is one of the `Vector` types and returns an instace of `FlxVector` struct, which implements `IEnumerable<FlxValue>` and has a `Length` getter
- `AsMap` checks if the underlying value is a `Map` type and return an instance of `FlxMap` struct, which implements `IEnumerable<KeyValuePair<string, FlxValue>>` and also has a `Length` getter
- `ToJson` returns a JSON string (see FlexBuffers to JSON section)
- User can wirte `flx[0]` in order to convert the `FlxValue` into `FlxVector` and access the first element of the vector
- When user wirte `flx["a"]`, then `FlxValue` is converted to `FlxMap` and value for the key `"a"` is accessed

---

# Outlook
As next steps we are cosnidering:
- object graph to FlexBuffer and FlexBuffer to object graph conversion
- profiling and performance tuning

Contribution is welcome.
