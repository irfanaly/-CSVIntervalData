using CSVIntervalData.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace CSVIntervalData.Writer
{
    public class CSVWriter
    {
        public bool Write(List<CSVFile> files)
        {
            foreach (var file in files)
            {
                var content = string.Format("{0}\n{1}\n{2}\n", file.Header, string.Join("\n", file.Body), file.Footer);
                using (var sw = new StreamWriter($"{file.FileName}.csv", false))
                {
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            return true;
        }

        [ExcludeFromCodeCoverage]
        public void PrintOnScreen(List<CSVFile> files)
        {
            foreach (var file in files)
            {
                Console.WriteLine($"=========={new string('=', file.FileName.Length)}");
                Console.WriteLine($"FileName: {file.FileName}");
                Console.WriteLine($"=========={new string('=', file.FileName.Length)}");
                Console.WriteLine($"Header: {file.Header}");
                Console.WriteLine($"Body: \n{string.Join("\n", file.Body)}");
                Console.WriteLine($"Footer: {file.Footer}");
                Console.WriteLine();
            }
            Console.WriteLine($"{files.Count()} generated");
        }
    }
}
