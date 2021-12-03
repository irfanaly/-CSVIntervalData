using System;
using System.Collections.Generic;
using System.IO;
using CSVIntervalData.Enum;
using CSVIntervalData.Model;
using CSVIntervalData.Validator;

namespace CSVIntervalData.Parser
{
    public class CSVParser
    {
        private readonly string _headerIdentifier = "100";
        private readonly string _footerIdentifier = "900";
        private readonly string _bodyIdentifier = "200";
        
        private int _lastPosition = 0;
        private string _defaultHeaderRow = string.Empty;
        private ParsingStatus _parseStatus = ParsingStatus.NotStarted;
        private CSVValidator _validator;

        public CSVParser()
        {
            _validator = new CSVValidator();
        }

        public List<CSVFile> Parse(string csvString)
        {
            _validator.Validate(csvString);

            var fileList = new List<CSVFile>();
            var arr = csvString?.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            while (_parseStatus != ParsingStatus.Completed)
            {
                var file = new CSVFile(_defaultHeaderRow);
                for (int i = _lastPosition; i < arr.Length; i++)
                {
                    ProcessFile(file, arr[i], i);
                }
                fileList.Add(file);
            }
            
            return fileList;
        }

        private void ProcessFile(CSVFile file, string row, int index)
        {
            if (IsHeaderRow(row)) //100
            {
                file.Header = row;                
                _defaultHeaderRow = row;
                _parseStatus = ParsingStatus.InProgress;
                return;
            }

            if (IsBodyStarterRow(row)) //200
            {
                if (file.IsEmpty())
                {
                    file.FileName = row.Split(',')?[1];
                    file.Body.Add(row);
                }
                else if (_parseStatus == ParsingStatus.InProgress)
                {
                    _parseStatus = ParsingStatus.Hold;
                    _lastPosition = index;
                }
                return;
            }

            if (IsFooterRow(row)) //900
            {
                file.Footer = row;
                if (_parseStatus == ParsingStatus.InProgress)
                    _parseStatus = ParsingStatus.Completed;
                else if (_parseStatus == ParsingStatus.Hold)
                    _parseStatus = ParsingStatus.InProgress;
                return;
            }

            // 300
            if (_parseStatus == ParsingStatus.InProgress) 
                file.Body.Add(row);
        }

        private bool IsHeaderRow(string row)
        {
            return row.Split(',')?[0] == _headerIdentifier;
        }

        private bool IsFooterRow(string row)
        {
            return row.Split(',')?[0] == _footerIdentifier;
        }

        private bool IsBodyStarterRow(string row)
        {
            return row.Split(',')?[0] == _bodyIdentifier;
        }
    }
}
