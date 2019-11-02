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
    }
}