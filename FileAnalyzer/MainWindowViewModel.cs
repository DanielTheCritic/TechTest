using FileAnalyzer.Services;
using FileAnalyzer.Services.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace FileAnalyzer
{
    public class MainWindowViewModel
    {
        private CSVReaderService readerService;
        private DataService dataService;
        private FileService fileService;

        public MainWindowViewModel()
        {
            readerService = new CSVReaderService();
            dataService = new DataService();
            fileService = new FileService();
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "output");
            if (Directory.Exists(dir))
                OutputDirectory = new DirectoryInfo(dir).FullName;
        }

        public string OutputDirectory { get; set; }

        public void UseSampleFile()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "sample", "data.csv");
            GenerateFiles(path);
        }

        public void BrowseFile()
        {
            var path = GetFile();
            if (string.IsNullOrEmpty(path))
                return;

            GenerateFiles(path);
        }

        private string GetFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                return dialog.FileName;
            return string.Empty;
        }

        public void ChangeOutputDirectory()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK || !string.IsNullOrEmpty(dialog.SelectedPath))
                return;
            OutputDirectory = dialog.SelectedPath;
        }

        private void GenerateFiles(string path)
        {
            try
            {
                var entries = readerService.GetEntries(path);
                SaveNamesFile(entries);
                SaveAddressesFile(entries);

                MessageBox.Show("File Processing Complete!", "PROCESS COMPLETE", MessageBoxButton.OK, MessageBoxImage.Information);
                var fileInfo = new FileInfo(path);
                OpenOutputDirectory();
            }
            catch(FileAnalyzerException fae)
            {
                DisplayError(fae.Message);
            }
            catch(Exception ex)
            {
                DisplayError("An unknown error occurred.\nDetails:\n" + ex.Message);
            }
        }

        private void SaveNamesFile(List<Entry> entries)
        {
            var names = dataService.GetNamesByFrequency(entries);
            var fileName = String.Concat("names ", DateTime.Now.ToString("dd MMM yyyy HHmmss"), ".txt");
            fileService.SaveFile(Path.Combine(OutputDirectory, fileName), names.Select(n => String.Concat(n.Key, ", ", n.Value)).ToArray());
        }

        private void SaveAddressesFile(List<Entry> entries)
        {
            var addresses = dataService.GetAddressesByName(entries);
            var fileName = String.Concat("addresses ", DateTime.Now.ToString("dd MMM yyyy HHmmss"), ".txt");
            fileService.SaveFile(Path.Combine(OutputDirectory, fileName), addresses.ToArray());
        }

        private void DisplayError(string message)
        {
            MessageBox.Show(message, "AN ERROR OCCURRED", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OpenOutputDirectory()
        {
            if (Directory.Exists(OutputDirectory))
                System.Diagnostics.Process.Start(OutputDirectory);
        }
    }
}
