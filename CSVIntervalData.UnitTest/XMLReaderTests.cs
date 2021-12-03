using NUnit.Framework;
using CSVIntervalData.Reader;
using System.IO;
using System;

namespace CSVIntervalData.UnitTest
{
    public class XMLReaderTests
    {
        private XMLReader _reader;

        [SetUp]
        public void Setup()
        {
            string filename = @"TestInputFiles\testfilevalid.xml";
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string testFile = Path.Combine(projectDirectory, filename);

            _reader = new XMLReader(testFile);
        }

        [Test]
        public void GetData_ValidNode_Success()
        {
            //Arrange
            var expectedResult = "\n          100,NEM12,201801211010,MYENRGY,URENRGY\n          200,12345678901,E1,E1,E1,N1,HGLMET501,KWH,30,\n          300,20180115,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,\n          300,20180116,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,\n          300,20180117,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,\n          300,20180118,5.000,3.000,3.008,3.000,4.000,3.000,2.96,3.6,4.212,1.992,2.132,2.532,6.192,5.396,5.616,6.012,5.544,7.436,7.472,5.888,4.316,4.66,5.368,5.644,5.392,6.612,5.8,6.636,6.572,6.36,10.992,9.52,10.268,9.704,9.616,9.308,13.1,20.36,16.456,11.144,9.712,6.076,6.064,5.324,7.18,6.228,5.628,5.94,A,,,20180120032031,\n          900\n        ";

            //Act
            var data = _reader.GetData("//CSVIntervalData");

            //Assert
            Assert.AreEqual(data, expectedResult);
        }

        [Test]
        public void GetData_InvalidNode_Fail()
        {
            //Arrange
            //Act
            var exception = Assert.Catch(() => _reader.GetData("//InvalidNode"));

            //Assert
            Assert.AreEqual(exception.GetType(), typeof(System.NullReferenceException));
            Assert.AreEqual(exception.Message, "Object reference not set to an instance of an object.");
        }

        [Test]
        public void GetData_WrongFileName_Fail()
        {
            //Arrange
            _reader = new XMLReader("wrongfile");

            //Act
            var exception = Assert.Catch(() => _reader.GetData("//CSVIntervalData"));

            //Assert
            Assert.AreEqual(exception.GetType(), typeof(FileNotFoundException));
            Assert.True(exception.Message.StartsWith("Could not find file "));
        }
    }
}