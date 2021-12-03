using CSVIntervalData.Parser;
using CSVIntervalData.Reader;
using CSVIntervalData.Writer;
using System;
using System.IO;

namespace CSVIntervalData
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFile = @"InputFile\testfile.xml";
                string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                string testFile = Path.Combine(projectDirectory, inputFile);                

                var reader = new XMLReader(testFile);
                string csvString = reader.GetData("//CSVIntervalData");

                var parser = new CSVParser();
                var generatedFiles = parser.Parse(csvString);

                var writer = new CSVWriter();
                _ = writer.Write(generatedFiles);
                writer.PrintOnScreen(generatedFiles);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
