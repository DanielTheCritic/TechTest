using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FileAnalyzer.Services.Domain;

namespace FileAnalyzer.Services.Tests
{
    [TestFixture]
    public class DataServiceTests
    {
        private CSVReaderService csvReaderService;
        private DataService dataService;
        private string currentDirectory;
        private string testFileDirectory;

        [OneTimeSetUp]
        public void TestSetup()
        {
            csvReaderService = new CSVReaderService();
            dataService = new DataService();
            currentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..");
            testFileDirectory = Path.Combine(currentDirectory, "TestFiles");
        }

        [Test]
        public void GetNamesByFrequency_GivenNoEntries_ThrowsError()
        {
            // Act
            TestDelegate del = () => dataService.GetNamesByFrequency(null);

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10006, exception.StatusCode);
        }

        [TestCase("data.csv", "Smith", 2)]
        [TestCase("data.csv", "John", 1)]
        [TestCase("data2.csv", "Daniel", 3)]
        public void GetNamesByFrequency_GivenCSVFile_ReturnsFrequencyOfEachName(string fileName, string firstName, int frequency)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, fileName);

            // Act
            var entries = csvReaderService.GetEntries(path);
            var results = dataService.GetNamesByFrequency(entries);

            // Assert
            Assert.AreEqual(frequency, results[firstName]);
        }

        [TestCase("data.csv")]
        [TestCase("data2.csv")]
        public void GetNamesByFrequency_GivenCSVFile_OrdersListByFrequencyDescending(string fileName)
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, fileName);

            // Act
            var entries = csvReaderService.GetEntries(path);
            var results = dataService.GetNamesByFrequency(entries);

            // Assert
            Assert.AreEqual(results.Values.Max(), results.Values.First());
            Assert.AreEqual(results.Values.Min(), results.Values.Last());
        }

        [Test]
        public void GetNamesByFrequency_GivenCSVFile_OrdersListByFrequencyThenName()
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, "data.csv");

            // Act
            var entries = csvReaderService.GetEntries(path);
            var results = dataService.GetNamesByFrequency(entries);
            var names = results.Keys.ToArray();

            // Assert
            Assert.AreEqual("Brown", names[0]);
            Assert.AreEqual("Clive", names[1]);
            Assert.AreEqual("Graham", names[2]);
            Assert.AreEqual("Howe", names[3]);
            Assert.AreEqual("James", names[4]);
            Assert.AreEqual("Owen", names[5]);
            Assert.AreEqual("Smith", names[6]);
            Assert.AreEqual("Jimmy", names[7]);
            Assert.AreEqual("John", names[8]);
        }

        [Test]
        public void GetAddressesByName_GivenNoEntries_ThrowsError()
        {
            // Act
            TestDelegate del = () => dataService.GetAddressesByName(null);

            // Assert
            var exception = Assert.Throws<FileAnalyzerException>(del);
            Assert.AreEqual(10006, exception.StatusCode);
        }

        [Test]
        public void GetAddressesByName_GivenCSVFile_OrdersAddressByStreetName()
        {
            // Arrange
            var path = Path.Combine(testFileDirectory, "data.csv");

            // Act
            var entries = csvReaderService.GetEntries(path);
            var results = dataService.GetAddressesByName(entries);

            // Assert
            Assert.AreEqual("65 Ambling Way", results.First());
            Assert.AreEqual("49 Sutherland St", results.Last());
        }
    }
}
