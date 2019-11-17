using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using FlexBuffers;
using NUnit.Framework;

namespace FlexBuffersCSharpTests
{
    [TestFixture]
    public class FlexBufferTests
    {
        [Test]
        public void Null()
        {
            Check(new byte[]{0, 0, 1}, FlexBuffer.Null());
        }
        
        [Test]
        public void Bool()
        {
            Check(new byte[]{1, 104, 1}, FlexBuffer.SingleValue(true));
            Check(new byte[]{0, 104, 1}, FlexBuffer.SingleValue(false));
        }
        
        [Test]
        public void OneByte()
        {
            Check(new byte[]{25, 4, 1}, FlexBuffer.SingleValue(25));
            Check(new byte[]{231, 4, 1}, FlexBuffer.SingleValue(-25));
            Check(new byte[]{230, 8, 1}, FlexBuffer.SingleValue(230UL));
        }
        
        [Test]
        public void TwoBytes()
        {
            Check(new byte[]{230, 0, 5, 2}, FlexBuffer.SingleValue(230));
            Check(new byte[]{1, 4, 5, 2}, FlexBuffer.SingleValue(1025));
            Check(new byte[]{255, 251, 5, 2}, FlexBuffer.SingleValue(-1025));
            Check(new byte[]{1, 4, 9, 2}, FlexBuffer.SingleValue(1025UL));
        }
        
        [Test]
        public void FourBytes()
        {
            Check(new byte[]{255, 255, 255, 127, 6, 4}, FlexBuffer.SingleValue(int.MaxValue));
            Check(new byte[]{0, 0, 0, 128, 6, 4}, FlexBuffer.SingleValue(int.MinValue));
            Check(new byte[]{0, 0, 144, 64, 14, 4}, FlexBuffer.SingleValue(4.5));
            Check(new byte[]{205, 204, 204, 61, 14, 4}, FlexBuffer.SingleValue(0.1f));
        }
        
        [Test]
        public void EightBytes()
        {
            Check(new byte[]{255, 255, 255, 255, 0, 0, 0, 0, 7, 8}, FlexBuffer.SingleValue(uint.MaxValue));
            Check(new byte[]{255, 255, 255, 255, 255, 255, 255, 127, 7, 8}, FlexBuffer.SingleValue(long.MaxValue));
            Check(new byte[]{0, 0, 0, 0, 0, 0, 0, 128, 7, 8}, FlexBuffer.SingleValue(long.MinValue));
            Check(new byte[]{255, 255, 255, 255, 255, 255, 255, 255, 11, 8}, FlexBuffer.SingleValue(ulong.MaxValue));
            Check(new byte[]{154, 153, 153, 153, 153, 153, 185, 63, 15, 8}, FlexBuffer.SingleValue(0.1));
        }
        
        [Test]
        public void String()
        {
            Check(new byte[]{5, 77, 97, 120, 105, 109, 0, 6, 20, 1}, FlexBuffer.SingleValue("Maxim"));
            Check(new byte[]{10, 104, 101, 108, 108, 111, 32, 240, 159, 152, 177, 0, 11, 20, 1}, FlexBuffer.SingleValue("hello 😱"));
        }
        
        [Test]
        public void IntVector()
        {
            Check(new byte[]{3, 1, 2, 3, 3, 44, 1}, FlexBuffer.From(new []{1, 2, 3}));
            Check(new byte[]{3, 255, 2, 3, 3, 44, 1}, FlexBuffer.From(new []{-1, 2, 3}));
            Check(new byte[]{3, 0, 1, 0, 43, 2, 3, 0, 6, 45, 1}, FlexBuffer.From(new []{1,555,3}));
            Check(new byte[]{3, 0, 0, 0, 1, 0, 0, 0, 204, 216, 0, 0, 3, 0, 0, 0, 12, 46, 1}, FlexBuffer.From(new []{1,55500,3}));
            Check(new byte[]{3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 172, 128, 94, 239, 12, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 24, 47, 1}, FlexBuffer.From(new []{1, 55555555500, 3}));
        }
        
        [Test]
        public void DoubleVector()
        {
            Check(new byte[]{3, 0, 0, 0, 0, 0, 192, 63, 0, 0, 32, 64, 0, 0, 96, 64, 12, 54, 1}, FlexBuffer.From(new []{1.5, 2.5, 3.5}));
            Check(new byte[]{3, 0, 0, 0, 0, 0, 0, 0, 154, 153, 153, 153, 153, 153, 241, 63, 154, 153, 153, 153, 153, 153, 1, 64, 102, 102, 102, 102, 102, 102, 10, 64, 24, 55, 1}, FlexBuffer.From(new []{1.1, 2.2, 3.3}));
        }
        
        [Test]
        public void BoolVector()
        {
            Check(new byte[]{3, 1, 0, 1, 3, 144, 1}, FlexBuffer.From(new []{true, false, true}));
        }
        
        [Test]
        public void StringVector()
        {
            Check(new byte[]{3, 102, 111, 111, 0, 3, 98, 97, 114, 0, 3, 98, 97, 122, 0, 3, 15, 11, 7, 3, 60, 1}, FlexBuffer.From(new []{"foo", "bar", "baz"}));
        }
        
        [Test]
        public void StringVectorWithRepeatingStrings()
        {
            Check(new byte[]{3, 102, 111, 111, 0, 3, 98, 97, 114, 0, 3, 98, 97, 122, 0, 6, 15, 11, 7, 18, 14, 10, 6, 60, 1}, FlexBuffer.From(new []{"foo", "bar", "baz", "foo", "bar", "baz"}));
        }
        
        [Test]
        public void MixedVector()
        {
            Check(new byte[]
            {
                3, 102, 111, 111, 0, 0, 0, 0,
                5, 0, 0, 0, 0, 0, 0, 0, 
                15, 0, 0, 0, 0, 0, 0, 0, 
                1, 0, 0, 0, 0, 0, 0, 0, 
                251, 255, 255, 255, 255, 255, 255, 255, 
                205, 204, 204, 204, 204, 204, 244, 63, 
                1, 0, 0, 0, 0, 0, 0, 0, 
                20, 4, 4, 15, 104, 45, 43, 1
            }, FlexBuffer.From(new List<object>{"foo", 1, -5, 1.3, true}));
        }

        [Test]
        public void StringIntDictSingleValue()
        {
            var dict = new Dictionary<string, int>()
            {
                {"a", 12}
            };
            Check(
                new byte[]{97, 0, 1, 3, 1, 1, 1, 12, 4, 2, 36, 1}, 
                FlexBuffer.From(dict));
        }
        
        [Test]
        public void StringIntDict()
        {
            var dict = new Dictionary<string, int>()
            {
                {"", 45},
                {"a", 12}
            };
            Check(
                new byte[]{0, 97, 0, 2, 4, 4, 2, 1, 2, 45, 12, 4, 4, 4, 36, 1}, 
                FlexBuffer.From(dict));
        }
        
        [Test]
        public void VectorOfSameKeyDicts()
        {
            var dict = new List<Dictionary<string, int>>()
            {
                new Dictionary<string, int>()
                {
                    {"something", 12}
                },
                new Dictionary<string, int>()
                {
                    {"something", 45}
                }
            };
            Check(
                new byte[]
                {
                    115, 111, 109, 101, 116, 104, 105, 110, 103, 0, 
                    1, 11, 1, 1, 1, 12, 4, 6, 1, 1, 45, 4, 2, 8, 4, 36, 36, 4, 40, 1
                }, 
                FlexBuffer.From(dict));
        }
        
        [Test]
        public void MixedVectorWithVectorAndInt()
        {
            var value = new List<object>()
            {
                new []{61},
                64
            };
            // Swift has {1, 61, 4, 2, 3, 64, 40, 4, 4, 40, 1} but it is also untyped
            Check(
                new byte[]{1, 61, 2, 2, 64, 44, 4, 4, 40, 1}, 
                FlexBuffer.From(value));
        }
        
        [Test]
        public void ComplexMap()
        {
            var value = new Dictionary<string, dynamic>()
            {
                {"age", 35},
                {"flags", new bool[]{true, false, true, true}},
                {"weight", 72.5},
                {"name", "Maxim"},
                {"address", new Dictionary<string, dynamic>()
                {
                    {"city", "Bla"},
                    {"zip", "12345"},
                    {"countryCode", "XX"},
                }}, 
            };
            // Different in Swift
            Check(
                new byte[]
                {
                    97, 100, 100, 114, 101, 115, 115, 0, 
                    99, 105, 116, 121, 0, 3, 66, 108, 97, 0, 
                    99, 111, 117, 110, 116, 114, 121, 67, 111, 100, 101, 0, 
                    2, 88, 88, 0, 
                    122, 105, 112, 0, 
                    5, 49, 50, 51, 52, 53, 0, 
                    3, 38, 29, 14, 3, 1, 3, 38, 22, 15, 20, 20, 20, 
                    97, 103, 101, 0, 
                    102, 108, 97, 103, 115, 0, 
                    4, 1, 0, 1, 1, 
                    110, 97, 109, 101, 0, 
                    5, 77, 97, 120, 105, 109, 0, 
                    119, 101, 105, 103, 104, 116, 0, 
                    5, 93, 36, 33, 23, 12, 0, 0, 7, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 60, 0, 0, 0, 35, 0, 0, 0, 51, 0, 0, 0, 45, 0, 0, 0, 0, 0, 145, 66, 36, 4, 144, 20, 14, 25, 38, 1
                }, 
                FlexBuffer.From(value));
        }
        
        [Test]
        public void Long2()
        {
            Check(
                new byte[]{1, 2, 2, 64, 1}, 
                FlexBuffer.SingleValue(1,2));
            Check(
                new byte[]{255, 255, 0, 1, 4, 65, 1}, 
                FlexBuffer.SingleValue(-1,256));
            Check(
                new byte[]{211, 255, 255, 255, 0, 232, 3, 0, 8, 66, 1}, 
                FlexBuffer.SingleValue(-45,256000));
            Check(
                new byte[]{211, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 127, 16, 67, 1}, 
                FlexBuffer.SingleValue(-45,long.MaxValue));
        }
        
        [Test]
        public void ULong2()
        {
            Check(
                new byte[]{1, 2, 2, 68, 1}, 
                FlexBuffer.SingleValue(1UL,2));
            Check(
                new byte[]{1, 0, 0, 1, 4, 69, 1}, 
                FlexBuffer.SingleValue(1UL,256));
            Check(
                new byte[]{45, 0, 0, 0, 0, 232, 3, 0, 8, 70, 1}, 
                FlexBuffer.SingleValue(45UL,256000));
            Check(
                new byte[]{45, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 16, 71, 1}, 
                FlexBuffer.SingleValue(45,ulong.MaxValue));
        }
        
        [Test]
        public void Double2()
        {
            Check(
                new byte[]{205, 204, 140, 63, 0, 0, 0, 192, 8, 74, 1}, 
                FlexBuffer.SingleValue(1.1f, -2));
            Check(
                new byte[]{154, 153, 153, 153, 153, 153, 241, 63, 0, 0, 0, 0, 0, 0, 112, 192, 16, 75, 1}, 
                FlexBuffer.SingleValue(1.1,-256));
        }
        
        [Test]
        public void Long3()
        {
            Check(
                new byte[]{1, 2, 4, 3, 76, 1}, 
                FlexBuffer.SingleValue(1, 2, 4));
            Check(
                new byte[]{255, 255, 0, 1, 4, 0, 6, 77, 1}, 
                FlexBuffer.SingleValue(-1,256, 4));
            Check(
                new byte[]
                {
                    211, 255, 255, 255, 
                    0, 232, 3, 0, 
                    4, 0, 0, 0, 
                    12, 78, 1
                }, 
                FlexBuffer.SingleValue(-45,256000, 4));
            Check(
                new byte[]
                {
                    211, 255, 255, 255, 255, 255, 255, 255, 
                    255, 255, 255, 255, 255, 255, 255, 127, 
                    4, 0, 0, 0, 0, 0, 0, 0, 
                    24, 79, 1
                }, 
                FlexBuffer.SingleValue(-45,long.MaxValue, 4));
        }
        
        [Test]
        public void Long4()
        {
            Check(
                new byte[]{1, 2, 4, 9, 4, 88, 1}, 
                FlexBuffer.SingleValue(1, 2, 4, 9));
            Check(
                new byte[]{255, 255, 0, 1, 4, 0, 9, 0, 8, 89, 1}, 
                FlexBuffer.SingleValue(-1,256, 4, 9));
            Check(
                new byte[]
                {
                    211, 255, 255, 255, 
                    0, 232, 3, 0, 
                    4, 0, 0, 0, 
                    9, 0, 0, 0, 
                    16, 90, 1
                }, 
                FlexBuffer.SingleValue(-45,256000, 4, 9));
            Check(
                new byte[]
                {
                    211, 255, 255, 255, 255, 255, 255, 255, 
                    255, 255, 255, 255, 255, 255, 255, 127, 
                    4, 0, 0, 0, 0, 0, 0, 0, 
                    9, 0, 0, 0, 0, 0, 0, 0, 
                    32, 91, 1
                }, 
                FlexBuffer.SingleValue(-45,long.MaxValue, 4, 9));
        }
        
        [Test]
        public void ULong3()
        {
            Check(
                new byte[]{1, 2, 4, 3, 80, 1}, 
                FlexBuffer.SingleValue(1UL, 2, 4));
            Check(
                new byte[]{1, 0, 0, 1, 4, 0, 6, 81, 1}, 
                FlexBuffer.SingleValue(1UL,256, 4));
            Check(
                new byte[]
                {
                    45, 0, 0, 0, 
                    0, 232, 3, 0, 
                    4, 0, 0, 0, 
                    12, 82, 1
                }, 
                FlexBuffer.SingleValue(45UL,256000, 4));
            Check(
                new byte[]
                {
                    45, 0, 0, 0, 0, 0, 0, 0, 
                    255, 255, 255, 255, 255, 255, 255, 127, 
                    4, 0, 0, 0, 0, 0, 0, 0, 
                    24, 83, 1
                }, 
                FlexBuffer.SingleValue(45UL,long.MaxValue, 4));
        }
        
        [Test]
        public void ULong4()
        {
            Check(
                new byte[]{1, 2, 4, 9, 4, 92, 1}, 
                FlexBuffer.SingleValue(1UL, 2, 4, 9));
            Check(
                new byte[]{1, 0, 0, 1, 4, 0, 9, 0, 8, 93, 1}, 
                FlexBuffer.SingleValue(1UL,256, 4, 9));
            Check(
                new byte[]
                {
                    45, 0, 0, 0, 
                    0, 232, 3, 0, 
                    4, 0, 0, 0, 
                    9, 0, 0, 0, 
                    16, 94, 1
                }, 
                FlexBuffer.SingleValue(45UL,256000, 4, 9));
            Check(
                new byte[]
                {
                    45, 0, 0, 0, 0, 0, 0, 0, 
                    255, 255, 255, 255, 255, 255, 255, 127, 
                    4, 0, 0, 0, 0, 0, 0, 0, 
                    9, 0, 0, 0, 0, 0, 0, 0, 
                    32, 95, 1
                }, 
                FlexBuffer.SingleValue(45UL,long.MaxValue, 4, 9));
        }
        
        [Test]
        public void Double3()
        {
            Check(
                new byte[]
                {
                    205, 204, 140, 63, 
                    0, 0, 0, 64, 
                    0, 0, 128, 64, 
                    12, 86, 1
                }, 
                FlexBuffer.SingleValue(1.1f, 2, 4));
            Check(
                new byte[]
                {
                    154, 153, 153, 153, 153, 153, 241, 63, 
                    0, 0, 0, 0, 0, 0, 112, 64, 
                    0, 0, 0, 0, 0, 0, 16, 64, 
                    24, 87, 1
                }, 
                FlexBuffer.SingleValue(1.1,256, 4));
        }
        
        [Test]
        public void Double4()
        {
            Check(
                new byte[]
                {
                    205, 204, 140, 63, 
                    0, 0, 0, 64, 
                    0, 0, 128, 64, 
                    0, 0, 16, 65, 
                    16, 98, 1
                }, 
                FlexBuffer.SingleValue(1.1f, 2, 4, 9));
            Check(
                new byte[]
                {
                    154, 153, 153, 153, 153, 153, 241, 63, 
                    0, 0, 0, 0, 0, 0, 112, 64, 
                    0, 0, 0, 0, 0, 0, 16, 64, 
                    0, 0, 0, 0, 0, 0, 34, 64, 
                    32, 99, 1
                }, 
                FlexBuffer.SingleValue(1.1,256, 4, 9));
        }
        
        [Test]
        public void Blob()
        {
            Check(
                new byte[]
                {
                    3, 1, 2, 3, 3, 100, 1
                }, 
                FlexBuffer.SingleValue(new byte[]{1, 2, 3}));
            
            var buffer = new byte[1001];
            for (int i = 0; i <= 1000; i++)
            {
                buffer[i] = 5;
            }

            var expected = new byte[1008];
            expected[0] = 233;
            expected[1] = 3;
            Buffer.BlockCopy(buffer, 0, expected, 2, 1000);
            expected[1002] = 5;
            expected[1003] = 0;
            expected[1004] = 234;
            expected[1005] = 3;
            expected[1006] = 101;
            expected[1007] = 2;
            Check(
                expected, 
                FlexBuffer.SingleValue(buffer));
        }

        [Test]
        public void LongStringArray()
        {
            var s1 = new StringBuilder();
            for (int i = 0; i < 260; i++)
            {
                s1.Append("a");
            }
            var s2 = new StringBuilder();
            for (int i = 0; i < 260000; i++)
            {
                s2.Append("b");
            }

            var list = new List<string>(2) {s1.ToString(), s2.ToString()};

            var bytes = FlexBuffer.From(list);

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(2, flx.AsVector.Length);
            Assert.AreEqual(s1.ToString(), flx[0].AsString);
            Assert.AreEqual(s2.ToString(), flx[1].AsString);
            
        }
        
        [Test]
        public void BiggerStringArray()
        {
            var list = new List<string>();
            for (var i = 0; i < 2600; i++)
            {
                list.Add("abc");
            }
            
            var bytes = FlexBuffer.From(list);

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(2600, flx.AsVector.Length);
            for (var i = 0; i < 2600; i++)
            {
                Assert.AreEqual("abc", flx[i].AsString);
            }
        }
        
        [Test]
        public void BiggerBoolArray()
        {
            var list = new List<bool>();
            for (var i = 0; i < 2600; i++)
            {
                list.Add(i%3 == 0);
            }
            
            var bytes = FlexBuffer.From(list);

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(2600, flx.AsVector.Length);
            for (var i = 0; i < 2600; i++)
            {
                Assert.AreEqual(list[i], flx[i].AsBool);
            }
        }
        
        [Test]
        public void BiggerDictionary()
        {
            var dict = new Dictionary<string, int>();
            for (var i = 0; i < 2600; i++)
            {
                dict[i.ToString()] = i;
            }
            
            var bytes = FlexBuffer.From(dict);

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(2600, flx.AsMap.Length);
            for (var i = 0; i < 2600; i++)
            {
                Assert.AreEqual(i, flx[i.ToString()].AsLong);
            }
        }

        private void Check(byte[] expected, byte[] actual)
        {
            Assert.AreEqual(expected, actual, string.Join(", ", actual));
        }
    }
}