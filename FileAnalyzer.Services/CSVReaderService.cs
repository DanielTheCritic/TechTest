using FileAnalyzer.Services.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer.Services
{
    public class CSVReaderService
    {
        private FileService fileService;
        public CSVReaderService()
        {
            fileService = new FileService();
        }

        /// <summary>
        /// Translates the raw CSV line items into a collection of Entry objects.
        /// </summary>
        /// <param name="filePath">Provide the full file path of a valid CSV file.</param>
        /// <returns></returns>
        public List<Entry> GetEntries(string filePath)
        {
            var lines = fileService.GetCSVContent(filePath);
            List<Entry> entries = new List<Entry>();
            for(int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    if (GetValues(lines[i]).Length != 4)
                        throw new FileAnalyzerException(10005);
                    continue;
                }

                entries.Add(GetEntry(lines[i]));
            }
            return entries;
        }

        private string[] GetValues(string line)
        {
            return line.Split(new[] { ";", "," }, StringSplitOptions.None);
        }

        private Entry GetEntry(string line)
        {
            var values = GetValues(line);
            if (values.Length != 4)
                throw new FileAnalyzerException(10005);
            return new Entry()
            {
                FirstName = values[0],
                LastName = values[1],
                Address = values[2],
                Number = values[3]
            };
        }
    }
}
