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
    public class FileServiceTests
    {
        private FileService fileService;
        private string currentDirectory;
        private string testFileDirectory;

        [OneTimeSetUp]
        public void TestSetup()
        {
            fileService = new FileService();
            currentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..");
            testFileDirectory = Path.Combine(currentDirectory, "TestFiles");
        }

        [TestCase(null)]
        [TestCase("")]
        public void ReadFileContents_GivenNullOrEmptyPath_ThrowsError(string path)
        {
            // Act
            TestDelegate del = () => fileService.GetCSVContent(path);

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10001, exception.StatusCode);
        }

        [Test]
        public void ReadFileContents_GivenInvalidFilePath_ThrowsError()
        {
            // Act
            TestDelegate del = () => fileService.GetCSVContent(@"C:\FakePath\FakeFile.csv");

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10002, exception.StatusCode);
        }

        [TestCase("invalidFileType")]
        [TestCase("invalidFileType.txt")]
        [TestCase("invalidFileType.bmp")]
        public void ReadFileContents_GivenInvalidFileType_ThrowsError(string fileName)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, fileName);

            // Act
            TestDelegate del = () => fileService.GetCSVContent(path);

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10003, exception.StatusCode);
        }

        [Test]
        public void ReadFileContents_GivenEmptyCSVFile_ThrowsError()
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, "emptyfile.csv");

            // Act
            TestDelegate del = () => fileService.GetCSVContent(path);

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10004, exception.StatusCode);
        }

        [TestCase("data2lines.csv", 2)]
        [TestCase("data5lines.csv", 5)]
        public void ReadFileContents_GivenCSVFileWithSpecifiedLines_ReturnsContentsOfFile(string fileName, int expectedLines)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, fileName);

            // Act
            var content = fileService.GetCSVContent(path);

            // Assert
            Assert.AreEqual(expectedLines, content.Length);
        }

        [TestCase("data3lines1empty.csv", 2)]
        [TestCase("data5lines2empty.csv", 3)]
        public void ReadFileContents_GivenFileWithBlankLines_ReturnsContentsOfFileAndIgnoresEmptyLines(string fileName, int expectedLines)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, fileName);

            // Act
            var content = fileService.GetCSVContent(path);

            // Assert
            Assert.AreEqual(expectedLines, content.Length);
        }
    }
}
