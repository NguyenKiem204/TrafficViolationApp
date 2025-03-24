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
using System.Windows.Shapes;
using System.IO;
using System.Security.Cryptography;
using TrafficViolationApp.config;
using Path = System.IO.Path;
namespace TrafficViolationApp
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }
        private void TestCredentialFile()
        {
            string filePath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                 "TrafficViolationApp", "credentials.dat");

            MessageBox.Show($"File exists: {File.Exists(filePath)}\nPath: {filePath}");
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            TestCredentialFile();
        }
    }
}
