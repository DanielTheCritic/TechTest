using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer.Services.Domain
{
    public class FileAnalyzerException : Exception
    {
        public FileAnalyzerException(int statusCode) : base(Constants.ErrorCodes[statusCode])
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
