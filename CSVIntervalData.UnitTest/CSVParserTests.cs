using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CSVIntervalData.Parser;
using CSVIntervalData.Reader;
using System.IO;

namespace CSVIntervalData.UnitTest
{
    class CSVParserTests
    {
        private CSVParser _parser;
        private XMLReader _reader;
        private string _projectDirectory = string.Empty;

        [SetUp]
        public void Setup()
        {
            _projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            _parser = new CSVParser();
        }

        [Test]
        [TestCase(@"TestInputFiles\testfilevalid.xml")]
        [TestCase(@"TestInputFiles\testfilevalid1.xml")]
        public void Parse_ValidData_Pass(string filename)
        {
            //Arrange
            var testFile = Path.Combine(_projectDirectory, filename);
            _reader = new XMLReader(testFile);
            var data = _reader.GetData("//CSVIntervalData");

            //Act
            var result = _parser.Parse(data);            

            //Assert
            if (filename == @"TestInputFiles\testfilevalid.xml")
            {
                var file = result.FirstOrDefault();
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(file.FileName, "12345678901");
                Assert.AreEqual(file.Header, "100,NEM12,201801211010,MYENRGY,URENRGY");
                Assert.AreEqual(file.Body.Count(), 5);
                Assert.AreEqual(file.Footer, "900");
            }
            else if (filename == @"TestInputFiles\testfilevalid1.xml")
            {
                var file = result.FirstOrDefault();
                Assert.AreEqual(result.Count, 2);
                Assert.AreEqual(file.FileName, "12345678901");
                Assert.AreEqual(file.Header, "100,NEM12,201801211010,MYENRGY,URENRGY");
                Assert.AreEqual(file.Body.Count(), 7);
                Assert.AreEqual(file.Footer, "900");

                file = result.LastOrDefault();
                Assert.AreEqual(file.FileName, "98765432109");
                Assert.AreEqual(file.Header, "100,NEM12,201801211010,MYENRGY,URENRGY");
                Assert.AreEqual(file.Body.Count(), 7);
                Assert.AreEqual(file.Footer, "900");
            }
        }

        [Test]
        [TestCase(@"TestInputFiles\testfileinvalid.xml")]
        [TestCase(@"TestInputFiles\testfileinvalid1.xml")]
        [TestCase(@"TestInputFiles\testfileinvalid2.xml")]
        [TestCase(@"TestInputFiles\testfileinvalid3.xml")]
        public void Parse_InvalidData_Fail(string filename)
        {
            //Arrange
            string testFile = Path.Combine(_projectDirectory, filename);
            _reader = new XMLReader(testFile);
            var data = _reader.GetData("//CSVIntervalData");

            //Act
            var exception = Assert.Catch(() => _parser.Parse(data));

            //Assert
            Assert.AreEqual(exception.GetType(), typeof(InvalidDataException));
            if (filename == @"TestInputFiles\testfileinvalid.xml")
                Assert.AreEqual(exception.Message, "Error occured on line:\n900 \n\n900 instruction should be executed after 100, 200 and 300 instructions");
            
            else if (filename == @"TestInputFiles\testfileinvalid1.xml")
                Assert.AreEqual(exception.Message, "Error occured on line:\n100,NEM12,201801211010,MYENRGY,URENRGY \n\n100 instruction should only occur once");
            
            else if (filename == @"TestInputFiles\testfileinvalid2.xml")
                Assert.AreEqual(exception.Message, "Error occured on line:\n900 \n\n900 instruction should only occur once");

            else if (filename == @"TestInputFiles\testfileinvalid3.xml")
                Assert.AreEqual(exception.Message, "Error occured on line:\n900 \n\n900 instruction should be executed after 100, 200 and 300 instructions");
        }        
    }
}
