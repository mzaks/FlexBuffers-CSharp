using FlexBuffers;
using NUnit.Framework;

namespace FlexBuffersCSharpTests
{
    [TestFixture]
    public class XmlToFlexBufferConverterTests
    {
        [Test]
        public void SingleTagWithAttributes()
        {
            var bytes = XmlToFlexBufferConverter.Convert("<tag1 a=\"123\" b=\"321\"/>");

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(3, flx.AsMap.Length);
            Assert.AreEqual("tag1", flx["tagName"].AsString);
            Assert.AreEqual("123", flx["a"].AsString);
            Assert.AreEqual("321", flx["b"].AsString);
        }
        
        [Test]
        public void SingleOpenCloseTagWithAttributes()
        {
            var bytes = XmlToFlexBufferConverter.Convert("<tag1 a=\"123\" b=\"321\"></tag1>");

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(3, flx.AsMap.Length);
            Assert.AreEqual("tag1", flx["tagName"].AsString);
            Assert.AreEqual("123", flx["a"].AsString);
            Assert.AreEqual("321", flx["b"].AsString);
        }
        
        [Test]
        public void SingleOpenCloseTagWithAttributesAndText()
        {
            var bytes = XmlToFlexBufferConverter.Convert("<tag1 a=\"123\" b=\"321\">hello</tag1>");

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(4, flx.AsMap.Length);
            Assert.AreEqual("tag1", flx["tagName"].AsString);
            Assert.AreEqual("123", flx["a"].AsString);
            Assert.AreEqual("321", flx["b"].AsString);
            
            Assert.AreEqual(1, flx["children"].AsVector.Length);
            Assert.AreEqual("hello", flx["children"][0].AsString);
        }
        
        [Test]
        public void SingleOpenCloseTagWithAttributesAndTextSplitByTag()
        {
            var bytes = XmlToFlexBufferConverter.Convert("<tag1 a=\"123\" b=\"321\">hello <br/> Maxim</tag1>");

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(4, flx.AsMap.Length);
            Assert.AreEqual("tag1", flx["tagName"].AsString);
            Assert.AreEqual("123", flx["a"].AsString);
            Assert.AreEqual("321", flx["b"].AsString);
            
            Assert.AreEqual(3, flx["children"].AsVector.Length);
            Assert.AreEqual("hello ", flx["children"][0].AsString);
            Assert.AreEqual(1, flx["children"][1].AsMap.Length);
            Assert.AreEqual("br", flx["children"][1]["tagName"].AsString);
            Assert.AreEqual(" Maxim", flx["children"][2].AsString);
        }
        
        [Test]
        public void ComplexXml()
        {
            var xml = @"<?xml version=""1.0""?>
<!-- This is a sample XML document -->
<!DOCTYPE Items [<!ENTITY number ""123"">]>
<Items>
<Item>Test with an entity: &number;</Item>
<Item>Test with a child element <more/> stuff</Item>
<Item>Test with a CDATA section <![CDATA[<456>]]> def</Item>
<Item>Test with a char entity: &#65;</Item>
<!-- Fourteen chars in this element.-->
<Item>1234567890ABCD</Item>
</Items>
";
            var bytes = XmlToFlexBufferConverter.Convert(xml);

            var flx = FlxValue.FromBytes(bytes);
            
            Assert.AreEqual(2, flx.AsMap.Length);
            Assert.AreEqual("Items", flx["tagName"].AsString);
            Assert.AreEqual(5, flx["children"].AsVector.Length);
            
            Assert.AreEqual(2, flx["children"][0].AsMap.Length);
            Assert.AreEqual("Item", flx["children"][0]["tagName"].AsString);
            Assert.AreEqual(2, flx["children"][0]["children"].AsVector.Length);
            Assert.AreEqual("Test with an entity: ", flx["children"][0]["children"][0].AsString);
            Assert.AreEqual(2, flx["children"][0]["children"][1].AsMap.Length);
            Assert.AreEqual("number", flx["children"][0]["children"][1]["tagName"].AsString);
            Assert.AreEqual(1, flx["children"][0]["children"][1]["children"].AsVector.Length);
            Assert.AreEqual("123", flx["children"][0]["children"][1]["children"][0].AsString);
            
            Assert.AreEqual("Item", flx["children"][1]["tagName"].AsString);
            Assert.AreEqual(3, flx["children"][1]["children"].AsVector.Length);
            Assert.AreEqual("Test with a child element ", flx["children"][1]["children"][0].AsString);
            Assert.AreEqual(1, flx["children"][1]["children"][1].AsMap.Length);
            Assert.AreEqual("more", flx["children"][1]["children"][1]["tagName"].AsString);
            Assert.AreEqual(" stuff", flx["children"][1]["children"][2].AsString);
            
            Assert.AreEqual("Item", flx["children"][2]["tagName"].AsString);
            Assert.AreEqual(3, flx["children"][2]["children"].AsVector.Length);
            Assert.AreEqual("Test with a CDATA section ", flx["children"][2]["children"][0].AsString);
            Assert.AreEqual("<456>", flx["children"][2]["children"][1].AsString);
            Assert.AreEqual(" def", flx["children"][2]["children"][2].AsString);
            
            Assert.AreEqual("Item", flx["children"][3]["tagName"].AsString);
            Assert.AreEqual(1, flx["children"][3]["children"].AsVector.Length);
            Assert.AreEqual("Test with a char entity: A", flx["children"][3]["children"][0].AsString);
            
            Assert.AreEqual("Item", flx["children"][4]["tagName"].AsString);
            Assert.AreEqual(1, flx["children"][4]["children"].AsVector.Length);
            Assert.AreEqual("1234567890ABCD", flx["children"][4]["children"][0].AsString);
        }
    }
}