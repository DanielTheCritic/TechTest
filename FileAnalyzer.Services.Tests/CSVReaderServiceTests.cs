using NUnit.Framework;
using FileAnalyzer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileAnalyzer.Services.Domain;
using System.IO;

namespace FileAnalyzer.Services.Tests
{
    [TestFixture]
    public class CSVReaderServiceTests
    {
        private CSVReaderService csvReaderService;
        private string currentDirectory;
        private string testFileDirectory;

        [OneTimeSetUp]
        public void TestSetup()
        {
            csvReaderService = new CSVReaderService();
            currentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..");
            testFileDirectory = Path.Combine(currentDirectory, "TestFiles");
        }

        [TestCase("wrongdata.csv")]
        [TestCase("wrongdata2.csv")]
        public void GetEntries_GivenCSVFileWithUnexpectedFields_ThrowsError(string fileName)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, fileName);

            // Act
            TestDelegate del = () => csvReaderService.GetEntries(path);

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10005, exception.StatusCode);
        }

        [Test]
        public void GetEntries_GivenCSVFile_ReturnsMappedEntriesExcludingHeader()
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, "data.csv");

            // Act
            var entries = csvReaderService.GetEntries(path);

            // Assert
            Assert.AreEqual(8, entries.Count);
        }

        [TestCase(1, "Jimmy")]
        [TestCase(4, "Graham")]
        [TestCase(5, "John")]
        public void ReadEntries_GivenCSVFile_ReturnsMappedEntriesWhereFirstNameValuesMatch(int line, string expectedFirstName)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, "data.csv");

            // Act
            var entries = csvReaderService.GetEntries(path);

            // Assert
            Assert.AreEqual(expectedFirstName, entries[line - 1].FirstName);
        }

        [TestCase(2, "65 Ambling Way")]
        [TestCase(6, "49 Sutherland St")]
        [TestCase(8, "94 Roland St")]
        public void ReadEntries_GivenCSVFile_ReturnsMappedEntriesWhereAddressValuesMatch(int line, string expectedAddress)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, "data.csv");

            // Act
            var entries = csvReaderService.GetEntries(path);

            // Assert
            Assert.AreEqual(expectedAddress, entries[line - 1].Address);
        }
    }
}
