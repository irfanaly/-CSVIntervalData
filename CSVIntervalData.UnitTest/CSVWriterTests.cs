using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVIntervalData.Parser;
using CSVIntervalData.Reader;
using CSVIntervalData.Writer;
using NUnit.Framework;

namespace CSVIntervalData.UnitTest
{
    class CSVWriterTests
    {
        private CSVWriter _writer;
        private CSVParser _parser;
        private XMLReader _reader;
        private string _projectDirectory = string.Empty;

        [SetUp]
        public void Setup()
        {
            _projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            _parser = new CSVParser();
            _writer = new CSVWriter();
        }

        [Test]
        [TestCase(@"TestInputFiles\testfilevalid1.xml")]
        public void Test1(string filename)
        {
            //Arrange
            var testFile = Path.Combine(_projectDirectory, filename);
            _reader = new XMLReader(testFile);
            var data = _reader.GetData("//CSVIntervalData");
            var files = _parser.Parse(data);

            //Act
            var result = _writer.Write(files);

            //Assert
            Assert.IsTrue(result);
            File.Exists($"{files.FirstOrDefault().FileName}.csv");
            File.Exists($"{files.LastOrDefault().FileName}.csv");
        }
    }
}
