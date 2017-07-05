using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer.Services.Domain
{
    public class Constants
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>()
        {
            { 10001, "File path provided cannot be null or empty." },
            { 10002, "File path provided is invalid." },
            { 10003, "File type is not CSV." },
            { 10004, "File has no data." },
            { 10005, "CSV file content has unexpected format." },
            { 10006, "No entries provided for output." }
        };
    }
}
