using FlexBuffers;
using NUnit.Framework;

namespace FlexBuffersCSharpTests
{
    [TestFixture]
    public class BitWidthTests
    {
        [Test]
        public void Zero()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width(0));
        }
        
        [Test]
        public void One()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width(1));
        }
        
        [Test]
        public void MinusOne()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width(-1));
        }
        
        [Test]
        public void ZeroPointFiveAsDouble()
        {
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width(0.5));
        }
        
        [Test]
        public void ZeroPointOne()
        {
            Assert.AreEqual(BitWidth.Width64, BitWidthUtil.Width(0.1));
        }
        
        [Test]
        public void ZeroPointOneAsFloat()
        {
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width(0.1f));
        }
        
        [Test]
        public void ByteMaxAndMin()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width(byte.MaxValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width(byte.MinValue));
        }
        
        [Test]
        public void ShortMaxAndMin()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((short)sbyte.MaxValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((short)sbyte.MinValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((short)byte.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((short)short.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((short)short.MinValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((ushort)byte.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((ushort)ushort.MaxValue));
        }
        
        [Test]
        public void IntMaxAndMin()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((int)sbyte.MaxValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((int)sbyte.MinValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((int)byte.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((int)short.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((int)short.MinValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((int)ushort.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((int)int.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((int)int.MinValue));
        }
        
        [Test]
        public void UIntMaxAndMin()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((uint)byte.MaxValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((uint)byte.MinValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((uint)ushort.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((uint)uint.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((uint)int.MaxValue));
        }
        
        [Test]
        public void LongMaxAndMin()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((long)sbyte.MaxValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((long)sbyte.MinValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((long)byte.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((long)short.MaxValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((long)short.MinValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((long)ushort.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((long)int.MaxValue));
            Assert.AreEqual(BitWidth.Width64, BitWidthUtil.Width((long)uint.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((long)int.MinValue));
            Assert.AreEqual(BitWidth.Width64, BitWidthUtil.Width((long)long.MaxValue));
            Assert.AreEqual(BitWidth.Width64, BitWidthUtil.Width((long)long.MinValue));
        }
        
        [Test]
        public void ULongMaxAndMin()
        {
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((ulong)byte.MaxValue));
            Assert.AreEqual(BitWidth.Width8, BitWidthUtil.Width((ulong)byte.MinValue));
            Assert.AreEqual(BitWidth.Width16, BitWidthUtil.Width((ulong)ushort.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((ulong)uint.MaxValue));
            Assert.AreEqual(BitWidth.Width32, BitWidthUtil.Width((ulong)int.MaxValue));
            Assert.AreEqual(BitWidth.Width64, BitWidthUtil.Width((ulong)ulong.MaxValue));
        }

        [Test]
        public void PaddingSize()
        {
            Assert.AreEqual(0, BitWidthUtil.PaddingSize(0, 1));
            Assert.AreEqual(0, BitWidthUtil.PaddingSize(7, 1));
            Assert.AreEqual(1, BitWidthUtil.PaddingSize(7, 2));
            Assert.AreEqual(1, BitWidthUtil.PaddingSize(7, 4));
            Assert.AreEqual(3, BitWidthUtil.PaddingSize(5, 4));
            Assert.AreEqual(7, BitWidthUtil.PaddingSize(1, 8));
        }
    }
}