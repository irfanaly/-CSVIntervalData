using System.Collections.Generic;

namespace CSVIntervalData.Model
{
    public class CSVFile
    {
        public CSVFile(string defaultHeader)
        {
            Body = new List<string>();

            if (!string.IsNullOrEmpty(defaultHeader))
                Header = defaultHeader;
        }

        public string FileName { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public List<string> Body { get; set; }
        public bool IsEmpty() => Body.Count == 0;
    }
}
