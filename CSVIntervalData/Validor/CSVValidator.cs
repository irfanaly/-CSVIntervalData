using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVIntervalData.Validator
{
    class CSVValidator
    {
        //_rule dictionary contains all instructions executed in order e.g. 100, 200, 300 and finally 900
        private Dictionary<string, bool> _rules = new Dictionary<string, bool>()
        {
            {"100", false},
            {"200", false},
            {"300", false},
            {"900", false}
        };

        /// <summary>
        /// This function verifies the validity of csv string.
        /// </summary>
        /// <param name="csvString">String from CSVIntervalData node in input xml file</param>
        /// <returns></returns>
        public bool Validate(string csvString)
        {
            try
            {
                var arr = csvString?.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var line in arr)
                {
                    if (line.StartsWith("100")) Validate100Instruction(line);
                    else if (line.StartsWith("200")) Validate200Instruction(line);
                    else if (line.StartsWith("300")) Validate300Instruction(line);
                    else if (line.StartsWith("900")) Validate900Instruction(line);
                }

                //check to validate 100, 200, 300 and 900 are executed at least onces
                if (_rules.Any(x => !x.Value))
                {
                    var missingInstruction = string.Join(",", _rules.Where(x => !x.Value).Select(x => x.Key));
                    throw new InvalidDataException($"Following instructions are missing: {missingInstruction}");
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This function validates that 100 instruction is executed first and only once.
        /// </summary>
        /// <param name="line">Pass CSVIntervalData data as string</param>
        /// <returns></returns>
        private bool Validate100Instruction(string line)
        {
            var currentLine = line.Split(',')[0];

            //200, 300 and 900 instruction shouldn't be executed before 100
            if (_rules.Where(x => new string[] { "200", "300", "900" }.Contains(x.Key)).Any(x => x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n100 instruction should be executed before 200, 300 and 900 instructions");

            //100 instruction should be executed only once
            if (_rules.Any(x => x.Key == "100" && x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n100 instruction should only occur once");

            _rules[currentLine] = true;
            return true;
        }

        /// <summary>
        /// This function validates 200 is executed after 100 and before 300 and 900, and 200 
        /// insruction must have second element for the file name.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool Validate200Instruction(string line)
        {
            var currentLine = line.Split(',')[0];

            //check 200 instruction executed before
            if (_rules.Where(x => new string[] { "100", "200", "300" }.Contains(x.Key)).All(x => x.Value) &&
                _rules.Any(x => x.Key == "900" && !x.Value))
            {
                _rules["200"] = false;
                _rules["300"] = false;
            }

            //check 100 instruction should be executed before 200
            if (_rules.Any(x => x.Key == "100" && !x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n200 instruction should be executed after 100 instruction");

            //check 300 and 900 should not be executed before 200
            if (_rules.Where(x => new string[] { "300", "900" }.Contains(x.Key)).All(x => x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n200 instruction should be executed before 300 and 900 instructions");

            //check 200 should have second element for filename
            if (line.Split(',').Length <= 1)
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n200 instruction does not contain second element for filename");

            _rules[currentLine] = true;
            return true;
        }

        /// <summary>
        /// This function validates 300 is executed after 100 and 200
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool Validate300Instruction(string line)
        {
            var currentLine = line.Split(',')[0];

            //check to verify 300 instruction executes after 100 and 200
            if (_rules.Where(x => new string[] { "100", "200" }.Contains(x.Key)).Any(x => !x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n300 instruction should be executed after 100 and 200 instructions");

            _rules[currentLine] = true;
            return true;
        }

        /// <summary>
        /// This function validates the 900 instruction is executed after 100, 200 and 300, 
        /// and executed only once.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool Validate900Instruction(string line)
        {
            var currentLine = line.Split(',')[0];

            //check to verify 900 instruction executes after 100, 200 and 300
            if (_rules.Where(x => new string[] { "100", "200", "300" }.Contains(x.Key)).Any(x => !x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n900 instruction should be executed after 100, 200 and 300 instructions");

            //check to verfiy 900 instruction executes only once
            if (_rules.Any(x => x.Key == "900" && x.Value))
                throw new InvalidDataException($"Error occured on line:\n{line} \n\n900 instruction should only occur once");

            _rules[currentLine] = true;
            return true;
        }
    }
}
