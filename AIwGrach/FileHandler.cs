using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AIwGrach
{
    public class FileHandler
    {
        #region Properties

        public string FileName { get; set; }

        #endregion

        #region Public Methods

        public List<List<string>> ReadFile()
        {
            if (string.IsNullOrEmpty(FileName))
                throw new Exception("Empty File ParentName.");

            var fileLines = File.ReadAllLines(FileName);
            var splitLines = fileLines.Select(line => line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();

            return splitLines;
        }

        #endregion
    }
}