using FlexBuffers;
using NUnit.Framework;

namespace FlexBuffersCSharpTests
{
    [TestFixture]
    public class FlexBufferBuilderTests
    {
        [Test]
        public void SmallIntVector()
        {
            var bytes = FlexBufferBuilder.Vector(builder =>
            {
                builder.Add(1);
                builder.Add(2);
                builder.Add(3);
                builder.Add(4);
                builder.Add(5);
            });

            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(5, flx.AsVector.Length);
            Assert.AreEqual(1, flx[0].AsLong);
            Assert.AreEqual(2, flx[1].AsLong);
            Assert.AreEqual(3, flx[2].AsLong);
            Assert.AreEqual(4, flx[3].AsLong);
            Assert.AreEqual(5, flx[4].AsLong);
        }
        
        [Test]
        public void SmallIntVectorAndNestedVector()
        {
            var bytes = FlexBufferBuilder.Vector(v1 =>
            {
                v1.Add(1);
                v1.Add(2);
                v1.Add(3);
                v1.Add(4);
                v1.Add(5);
                v1.Vector(v2 => {
                    v2.Add("hello");
                    v2.Add("world");
                });
            });

            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(6, flx.AsVector.Length);
            Assert.AreEqual(1, flx[0].AsLong);
            Assert.AreEqual(2, flx[1].AsLong);
            Assert.AreEqual(3, flx[2].AsLong);
            Assert.AreEqual(4, flx[3].AsLong);
            Assert.AreEqual(5, flx[4].AsLong);
            Assert.AreEqual(2, flx[5].AsVector.Length);
            Assert.AreEqual("hello", flx[5][0].AsString);
            Assert.AreEqual("world", flx[5][1].AsString);
        }
        
        [Test]
        public void SmallMap()
        {
            var bytes = FlexBufferBuilder.Map(builder =>
            {
                builder.Add("name", "Maxim");
                builder.Add("age", 38);
                builder.Add("weight", 72.5);
            });

            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(3, flx.AsMap.Length);
            Assert.AreEqual(38, flx["age"].AsLong);
            Assert.AreEqual(72.5, flx["weight"].AsDouble);
            Assert.AreEqual("Maxim", flx["name"].AsString);
        }
        
        [Test]
        public void ComplexMap()
        {
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

            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(5, flx.AsMap.Length);
            
            Assert.AreEqual(38, flx["age"].AsLong);
            Assert.AreEqual(72.5, flx["weight"].AsDouble);
            Assert.AreEqual("Maxim", flx["name"].AsString);
            
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
        public void FixVectors()
        {
            var bytes = FlexBufferBuilder.Map(root =>
            {
                root.Add("a", 1, 2);
                root.Add("b", 1, 2, 3);
                root.Add("c", 1, 2, 3, 4);
                root.Add("d", 1UL, 2);
                root.Add("e", 1UL, 2, 3);
                root.Add("f", 1UL, 2, 3, 4);
                root.Add("g", 1.1, 2);
                root.Add("h", 1.1, 2, 3);
                root.Add("i", 1.1, 2, 3, 4);
                root.Vector("j", vec =>
                {
                    vec.Add(1, 2);
                    vec.Add(1, 2, 3);
                    vec.Add(1, 2, 3, 4);
                    vec.Add(1UL, 2);
                    vec.Add(1UL, 2, 3);
                    vec.Add(1UL, 2, 3, 4);
                    vec.Add(1.1, 2);
                    vec.Add(1.1, 2, 3);
                    vec.Add(1.1, 2, 3, 4);
                });
            });

            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(10, flx.AsMap.Length);
            Assert.AreEqual(1, flx["a"][0].AsLong);
            Assert.AreEqual(2, flx["a"][1].AsLong);
            Assert.AreEqual(1, flx["b"][0].AsLong);
            Assert.AreEqual(2, flx["b"][1].AsLong);
            Assert.AreEqual(3, flx["b"][2].AsLong);
            Assert.AreEqual(1, flx["c"][0].AsLong);
            Assert.AreEqual(2, flx["c"][1].AsLong);
            Assert.AreEqual(3, flx["c"][2].AsLong);
            Assert.AreEqual(4, flx["c"][3].AsLong);
            Assert.AreEqual(1, flx["d"][0].AsULong);
            Assert.AreEqual(2, flx["d"][1].AsULong);
            Assert.AreEqual(1, flx["e"][0].AsULong);
            Assert.AreEqual(2, flx["e"][1].AsULong);
            Assert.AreEqual(3, flx["e"][2].AsULong);
            Assert.AreEqual(1, flx["f"][0].AsULong);
            Assert.AreEqual(2, flx["f"][1].AsULong);
            Assert.AreEqual(3, flx["f"][2].AsULong);
            Assert.AreEqual(4, flx["f"][3].AsULong);
            Assert.AreEqual(1.1, flx["g"][0].AsDouble);
            Assert.AreEqual(2, flx["g"][1].AsDouble);
            Assert.AreEqual(1.1, flx["h"][0].AsDouble);
            Assert.AreEqual(2, flx["h"][1].AsDouble);
            Assert.AreEqual(3, flx["h"][2].AsDouble);
            Assert.AreEqual(1.1, flx["i"][0].AsDouble);
            Assert.AreEqual(2, flx["i"][1].AsDouble);
            Assert.AreEqual(3, flx["i"][2].AsDouble);
            Assert.AreEqual(4, flx["i"][3].AsDouble);

            var v = flx["j"].AsVector;
            Assert.AreEqual(9, v.Length);
            Assert.AreEqual(1, v[0][0].AsLong);
            Assert.AreEqual(2, v[0][1].AsLong);
            Assert.AreEqual(1, v[1][0].AsLong);
            Assert.AreEqual(2, v[1][1].AsLong);
            Assert.AreEqual(3, v[1][2].AsLong);
            Assert.AreEqual(1, v[2][0].AsLong);
            Assert.AreEqual(2, v[2][1].AsLong);
            Assert.AreEqual(3, v[2][2].AsLong);
            Assert.AreEqual(4, v[2][3].AsLong);
            Assert.AreEqual(1, v[3][0].AsULong);
            Assert.AreEqual(2, v[3][1].AsULong);
            Assert.AreEqual(1, v[4][0].AsULong);
            Assert.AreEqual(2, v[4][1].AsULong);
            Assert.AreEqual(3, v[4][2].AsULong);
            Assert.AreEqual(1, v[5][0].AsULong);
            Assert.AreEqual(2, v[5][1].AsULong);
            Assert.AreEqual(3, v[5][2].AsULong);
            Assert.AreEqual(4, v[5][3].AsULong);
            Assert.AreEqual(1.1, v[6][0].AsDouble);
            Assert.AreEqual(2, v[6][1].AsDouble);
            Assert.AreEqual(1.1, v[7][0].AsDouble);
            Assert.AreEqual(2, v[7][1].AsDouble);
            Assert.AreEqual(3, v[7][2].AsDouble);
            Assert.AreEqual(1.1, v[8][0].AsDouble);
            Assert.AreEqual(2, v[8][1].AsDouble);
            Assert.AreEqual(3, v[8][2].AsDouble);
            Assert.AreEqual(4, v[8][3].AsDouble);
        }

        [Test]
        public void IndirectValuesInMap()
        {
            var bytes = FlexBufferBuilder.Map(root =>
            {
                root.Add("a", -123, true);
                root.Add("b", 123UL, true);
                root.Add("c", 123.3, true);
            });

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(3, flx.AsMap.Length);
            Assert.AreEqual(-123, flx["a"].AsLong);
            Assert.AreEqual(123, flx["b"].AsULong);
            Assert.AreEqual(123.3, flx["c"].AsDouble);
        }
        
        [Test]
        public void IndirectValuesInMapToJson()
        {
            var bytes = FlexBufferBuilder.Map(root =>
            {
                root.Add("a", -123, true);
                root.Add("b", 123UL, true);
                root.Add("c", 123.3, true);
            });

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(
                "{\"a\":-123,\"b\":123,\"c\":123.3}", 
                flx.ToJson);
        }
        
        [Test]
        public void IndirectValuesInVector()
        {
            var bytes = FlexBufferBuilder.Vector(root =>
            {
                root.Add(-123, true);
                root.Add(123UL, true);
                root.Add(123.3, true);
            });

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(3, flx.AsVector.Length);
            Assert.AreEqual(-123, flx[0].AsLong);
            Assert.AreEqual(123, flx[1].AsULong);
            Assert.AreEqual(123.3, flx[2].AsDouble);
        }
        
        [Test]
        public void IndirectValuesInVectorToJson()
        {
            var bytes = FlexBufferBuilder.Vector(root =>
            {
                root.Add(-123, true);
                root.Add(123UL, true);
                root.Add(123.3, true);
            });

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(
                "[-123,123,123.3]", 
                flx.ToJson);
        }
        
        [Test]
        public void AddNullAndBlobToNestedToJson()
        {
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

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(
                "[null,\"AQID\",{\"a\":null,\"b\":\"AwQF\"}]", 
                flx.ToJson);
        }
    }
}