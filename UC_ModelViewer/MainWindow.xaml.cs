using System.IO;
using System;
using System.Windows;
using UC_ModelViewer.MVVM.ViewModel;
using System.Diagnostics;

namespace UC_ModelViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClearCache();
            DataContext = new MainViewModel();
        }

        //removes all bitmaps and temporary models from previous run
        public static void ClearCache()
        {
            try
            {
                string[] filesToDelete = { "model.obj", "model.mtl" };

                foreach (var file in filesToDelete)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }

                string[] bmpFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.bmp");
                foreach (var file in bmpFiles)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Error clearing cache: " + ex.Message);
            }
        }

    }
}