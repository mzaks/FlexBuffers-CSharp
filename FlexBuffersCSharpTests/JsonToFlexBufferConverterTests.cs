using FlexBuffers;
using NUnit.Framework;

namespace FlexBuffersCSharpTests
{
    [TestFixture]
    public class JsonToFlexBufferConverterTests
    {
        [Test]
        public void Null()
        {
            var buffer = JsonToFlexBufferConverter.Convert("null");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(true, flx.IsNull);
        }
        
        [Test]
        public void Int()
        {
            var buffer = JsonToFlexBufferConverter.Convert("3456");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(3456, flx.AsLong);
        }
        
        [Test]
        public void NegativeInt()
        {
            var buffer = JsonToFlexBufferConverter.Convert("-3456");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(-3456, flx.AsLong);
        }
        
        [Test]
        public void Float()
        {
            var buffer = JsonToFlexBufferConverter.Convert("34.56");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(34.56, flx.AsDouble);
        }
        
        [Test]
        public void NegativeFloat()
        {
            var buffer = JsonToFlexBufferConverter.Convert("-34.56");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(-34.56, flx.AsDouble);
        }
        
        [Test]
        public void MixedVector()
        {
            var buffer = JsonToFlexBufferConverter.Convert("[null, true, false, \"hello 🙀\", -34, 6.1]");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(6, flx.AsVector.Length);
            Assert.AreEqual(true, flx[0].IsNull);
            Assert.AreEqual(true, flx[1].AsBool);
            Assert.AreEqual(false, flx[2].AsBool);
            Assert.AreEqual("hello 🙀", flx[3].AsString);
            Assert.AreEqual(-34, flx[4].AsLong);
            Assert.AreEqual(6.1, flx[5].AsDouble);
        }
        
        [Test]
        public void EmptyVector()
        {
            var buffer = JsonToFlexBufferConverter.Convert("[]");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(0, flx.AsVector.Length);
        }
        
        [Test]
        public void EmptyMap()
        {
            var buffer = JsonToFlexBufferConverter.Convert("{}");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(0, flx.AsMap.Length);
        }
        
        [Test]
        public void OneKeyMap()
        {
            var buffer = JsonToFlexBufferConverter.Convert("{\"\":1}");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(1, flx.AsMap.Length);
            Assert.AreEqual(1, flx[""].AsLong);
            
        }
        
        [Test]
        public void TwoKeysMap()
        {
            var buffer = JsonToFlexBufferConverter.Convert("{\"a\":1, \"b\":2}");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(2, flx.AsMap.Length);
            Assert.AreEqual(1, flx["a"].AsLong);
            Assert.AreEqual(2, flx["b"].AsLong);
        }
        
        [Test]
        public void TwoKeysMapUnsorted()
        {
            var buffer = JsonToFlexBufferConverter.Convert("{\"b\":2,\"a\":1}");
            var flx = FlxValue.FromBytes(buffer);
            Assert.AreEqual(2, flx.AsMap.Length);
            Assert.AreEqual(1, flx["a"].AsLong);
            Assert.AreEqual(2, flx["b"].AsLong);
        }
        
        [Test]
        public void ComplexMap()
        {

            const string json = @"
{
    ""age"": 35,
    ""weight"": 72.5,
    ""name"": ""Maxim"",
    ""flags"": [true, false, true, true],
    ""something"": null,
    ""address"": {
        ""city"": ""Bla"",
        ""zip"": ""12345"",
        ""countryCode"": ""XX""
    }
}
";
            var bytes = JsonToFlexBufferConverter.Convert(json);
            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(6, flx.AsMap.Length);
            
            Assert.AreEqual(35, flx["age"].AsLong);
            Assert.AreEqual(72.5, flx["weight"].AsDouble);
            Assert.AreEqual("Maxim", flx["name"].AsString);
            Assert.AreEqual(true, flx["something"].IsNull);
            
            Assert.AreEqual(4, flx["flags"].AsVector.Length);
            Assert.AreEqual(true, flx["flags"][0].AsBool);
            Assert.AreEqual(false, flx["flags"][1].AsBool);
            Assert.AreEqual(true, flx["flags"][2].AsBool);
            Assert.AreEqual(true, flx["flags"][3].AsBool);
            
            Assert.AreEqual(3, flx["address"].AsMap.Length);
            Assert.AreEqual("Bla", flx["address"]["city"].AsString);
            Assert.AreEqual("12345", flx["address"]["zip"].AsString);
            Assert.AreEqual("XX", flx["address"]["countryCode"].AsString);
        }
        
        [Test]
        public void VectorOfMaps()
        {

            const string json = @"
[
    {""name"": ""Max"", ""age"": 38},
    {""name"": ""Maxim"", ""age"": 35},
    {""age"": 18, ""name"": ""Alex""}
]
";
            var bytes = JsonToFlexBufferConverter.Convert(json);
            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(3, flx.AsVector.Length);

            Assert.AreEqual(2, flx[0].AsMap.Length);
            Assert.AreEqual("Max", flx[0]["name"].AsString);
            Assert.AreEqual(38, flx[0]["age"].AsLong);

            Assert.AreEqual(2, flx[1].AsMap.Length);
            Assert.AreEqual("Maxim", flx[1]["name"].AsString);
            Assert.AreEqual(35, flx[1]["age"].AsLong);
            
            Assert.AreEqual(2, flx[2].AsMap.Length);
            Assert.AreEqual("Alex", flx[2]["name"].AsString);
            Assert.AreEqual(18, flx[2]["age"].AsLong);
        }
        
        [Test]
        public void TestOffsetAndLengthAreOfTypeULong()
        {
            var json = @"{""channels_in"":64,""dilation_height_factor"":1,""dilation_width_factor"":1,""fused_activation_function"":1,""pad_values"":1,""padding"":0,""stride_height"":1,""stride_width"":1}";
            var bytes = JsonToFlexBufferConverter.Convert(json);
            var expectedBytes = new byte[] {99, 104, 97, 110, 110, 101, 108, 115, 95, 105, 110, 0, 100, 105, 108, 97, 116, 105, 111, 110, 95, 104, 101, 105, 103, 104, 116, 95, 102, 97, 99, 116, 111, 114, 0, 100, 105, 108, 97, 116, 105, 111, 110, 95, 119, 105, 100, 116, 104, 95, 102, 97, 99, 116, 111, 114, 0, 102, 117, 115, 101, 100, 95, 97, 99, 116, 105, 118, 97, 116, 105, 111, 110, 95, 102, 117, 110, 99, 116, 105, 111, 110, 0, 112, 97, 100, 95, 118, 97, 108, 117, 101, 115, 0, 112, 97, 100, 100, 105, 110, 103, 0, 115, 116, 114, 105, 100, 101, 95, 104, 101, 105, 103, 104, 116, 0, 115, 116, 114, 105, 100, 101, 95, 119, 105, 100, 116, 104, 0, 8, 130, 119, 97, 76, 51, 41, 34, 21, 8, 1, 8, 64, 1, 1, 1, 1, 0, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 16, 36, 1};
            Assert.AreEqual(expectedBytes, bytes);
        }
    }
}