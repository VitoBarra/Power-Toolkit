using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Win32;
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
using System.IO;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;

namespace RenameTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog fileDialog = new();



            if (fileDialog.ShowDialog() == true)
            {
                RenameFile(fileDialog.SelectedPath);
                System.Windows.MessageBox.Show("File Riscritti");
            }
        }

        private void RenameFile(string Directory)
        {

            //var Currentpath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"..\"));

            var FileDirs = new List<string>(System.IO.Directory.EnumerateFiles(Directory)); //File


            foreach (var FileDir in FileDirs)
            {
                List<MetadataExtractor.Directory> MetadataDirectory;
                FileInfo FileInfo = new(FileDir);
                try
                {
                    MetadataDirectory = new List<MetadataExtractor.Directory>(ImageMetadataReader.ReadMetadata(FileDir));
                }
                catch (Exception)
                {
                    Console.WriteLine("tipo di file ignorato" + FileDir + "\n");
                    continue;
                }

                var sub = MetadataDirectory.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                var DateTime = sub?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                if (!string.IsNullOrEmpty(DateTime))
                    try
                    {
                        File.Move(FileDir, $"{Directory}\\{DateTime.Replace(":", "-")}{FileInfo.Extension}");
                    }
                    catch { }
            }


            Console.WriteLine("File Riscritti");
            Console.ReadLine();
        }
    }
}
