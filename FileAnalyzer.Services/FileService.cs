using FileAnalyzer.Services.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer.Services
{
    public class FileService
    {
        /// <summary>
        /// Retrieves the raw CSV information line by line, from the file path provided.
        /// </summary>
        /// <param name="filePath">Provide the full file path of a valid CSV file.</param>
        /// <returns></returns>
        public string[] GetCSVContent(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new FileAnalyzerException(10001);
            if (!File.Exists(filePath))
                throw new FileAnalyzerException(10002);
            if (Path.GetExtension(filePath).ToLower() != ".csv")
                throw new FileAnalyzerException(10003);

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(content))
                        throw new FileAnalyzerException(10004);
                    var lines = content.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length == 0)
                        throw new FileAnalyzerException(10004);
                    return lines;
                }
            } 
        }

        /// <summary>
        /// Saves a file and it's contents to disc.
        /// </summary>
        /// <param name="fileName">The file name and directory of the file to be created.</param>
        /// <param name="lines">The content of the file represented line by line.</param>
        public void SaveFile(string fileName, string[] lines)
        {
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    foreach (var line in lines)
                        writer.WriteLine(line);
                }
            }
        }
    }
}
