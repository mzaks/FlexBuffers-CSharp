using FlexBuffers;
using NUnit.Framework;

namespace FlexBuffersCSharpTests
{
    [TestFixture]
    public class CsvToFlexBuffersConverterTests
    {
        [Test]
        public void SimpleCSV()
        {
            var bytes = CsvToFlexBufferConverter.Convert("A,B,C");
            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(flx.AsVector.Length, 1);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
        }
        
        [Test]
        public void SimpleCSVWithEmptyValue()
        {
            var bytes = CsvToFlexBufferConverter.Convert("A,,C");
            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(flx.AsVector.Length, 1);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "");
            Assert.AreEqual(flx[0][2].AsString, "C");
        }
        
        [Test]
        public void SimpleCSVWithRN()
        {
            var bytes = CsvToFlexBufferConverter.Convert("A,B,C\r\n1,2,3");
            var flx = FlxValue.FromBytes(bytes);
            Assert.AreEqual(flx.AsVector.Length, 2);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1].AsVector.Length, 3);
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
        }
        
        [Test]
        public void MultipleLineCSV()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
@"A,B,C
1,2,3
dskfsdh,sdfsdf,sdfsf");
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "dskfsdh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
        
        [Test]
        public void MultipleLineWithNewLineAtTheEndCSV()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
@"A,B,C
1,2,3
dskfsdh,sdfsdf,sdfsf
");
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "dskfsdh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
        
        [Test]
        public void MultipleLineWithDoubleQuotes()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
                @"A,B,C
1,2,3
""dskfsdh"",sdfsdf,sdfsf
");
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "dskfsdh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
        
        [Test]
        public void MultipleLineWithDoubleQuotesEscapingComma()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
                @"A,B,C
1,2,3
""ds,kfs,dh"",sdfsdf,sdfsf
");
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "ds,kfs,dh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
        
        [Test]
        public void MultipleLineWithDoubleQuotesEscapingDoubleQuotes()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
                @"A,B,C
1,2,3
""ds""""kfsdh"",sdfsdf,sdfsf
");
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "ds\"kfsdh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
        
        [Test]
        public void MultipleLineWithDoubleQuotesEscapingLineBreak()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
                @"A,B,C
1,2,3
""ds
kfsdh"",sdfsdf,sdfsf
");
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "ds\nkfsdh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
        
        [Test]
        public void MultipleLineWithSpecialSeparatorAndDoubleQuotesEscapingLineBreak()
        {
            var bytes = CsvToFlexBufferConverter.Convert(
                @"A;B;C
1;2;3
""ds
kfsdh"";sdfsdf;sdfsf
", ';');
            var flx = FlxValue.FromBytes(bytes);
            var json = flx.ToJson;
            Assert.AreEqual(flx.AsVector.Length, 3);
            Assert.AreEqual(flx[0].AsVector.Length, 3);
            Assert.AreEqual(flx[0][0].AsString, "A");
            Assert.AreEqual(flx[0][1].AsString, "B");
            Assert.AreEqual(flx[0][2].AsString, "C");
            
            Assert.AreEqual(flx[1][0].AsString, "1");
            Assert.AreEqual(flx[1][1].AsString, "2");
            Assert.AreEqual(flx[1][2].AsString, "3");
            
            Assert.AreEqual(flx[2][0].AsString, "ds\nkfsdh");
            Assert.AreEqual(flx[2][1].AsString, "sdfsdf");
            Assert.AreEqual(flx[2][2].AsString, "sdfsf");
        }
    }
}