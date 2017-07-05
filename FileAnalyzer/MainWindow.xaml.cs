using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new MainWindowViewModel();
            this.DataContext = ViewModel;
        }

        private void btnUseSampleFile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UseSampleFile();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.BrowseFile();
        }

        private void btnChangeOutput_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ChangeOutputDirectory();
        }
    }
}
