using System.ComponentModel;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using static UC_ModelViewer.MVVM.Model.ImageConvert;
using System.Windows.Media;
using UC_ModelViewer.MVVM.Model;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UC_ModelViewer.MVVM.ViewModel
{
    public class ThreeDViewModel : INotifyPropertyChanged
    {
        private Model3DGroup _model;
        public Model3DGroup Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public DirectionalLight DirectionalLight { get; private set; }

        public ICommand ExportCommand { get; }
        public ThreeDViewModel()
        {
            DirectionalLight = new DirectionalLight
            {
                Color = Colors.White, // or any desired color
                Direction = new Vector3D(-1, -1, -1) // initial direction
            };

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadObjModel(string combinedContent)
        {
            int mtlTagIndex = combinedContent.IndexOf("[MTL]", StringComparison.Ordinal);

            string objContent, mtlContent = String.Empty;

            // Filenames
            string objFilename = "model.obj";
            string mtlFilename = "model.mtl";

            if (mtlTagIndex >= 0)
            {
                objContent = combinedContent.Substring(0, mtlTagIndex).TrimEnd('\r', '\n', ' ');
                mtlContent = combinedContent.Substring(mtlTagIndex + "[MTL]".Length).TrimStart('\r', '\n', ' ');

                // Save OBJ content to disk
                File.WriteAllText(objFilename, objContent);

                // Save MTL content to disk
                File.WriteAllText(mtlFilename, mtlContent);
            }
            else
            {
                objContent = combinedContent.TrimEnd('\r', '\n', ' ');

                // Save only OBJ content to disk
                File.WriteAllText(objFilename, objContent);

                // Ensure no stale MTL file remains
                if (File.Exists(mtlFilename))
                {
                    File.Delete(mtlFilename);
                }
            }

            // Get the full path of the file
            string fullPathObj = Path.Combine(Directory.GetCurrentDirectory(), objFilename);
            string fullPathMtl = Path.Combine(Directory.GetCurrentDirectory(), mtlFilename);
            List<string> materialFiles = new List<string>();

            //Get Materials
            string pattern = @"map_Kd\s+(tex\d+hi)\.bmp";
            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(mtlContent))
            {
                if (match.Groups.Count > 1)
                {
                    string filenameWithoutExtension = match.Groups[1].Value;
                    materialFiles.Add(filenameWithoutExtension);
                }
            }

            //Convert PRIM TEX to TGA to BMP
            Tex2Tga converter = new Tex2Tga();

            foreach (string materialFile in materialFiles)
            {
                try
                {
                    if (PrimTexTxcMap.fileMap.TryGetValue(materialFile, out string filePath))
                    {
                        string tga_outName = Path.Combine(Directory.GetCurrentDirectory(), materialFile + ".tga");
                        converter.ConvertTexToTga(filePath, tga_outName);
                        string bmpFilePath = Path.ChangeExtension(tga_outName, ".bmp");
                        TGA tgaFile = TGA.FromFile(tga_outName);
                        Bitmap bmp = tgaFile.ToBitmap();
                        bmp.Save(bmpFilePath);
                        File.Delete(tga_outName);
                    }
                }
                catch (ExternalException ex)
                {
                    //Debug.WriteLine($"Error processing material file '{materialFile}': {ex.Message}");
                }
            }

            ObjReader reader = new ObjReader();
            Model3DGroup model = reader.Read(fullPathObj);

            Model = model;
        }

        private Dictionary<string, string> ParseMtlFile(string mtlFilePath)
        {
            var materialTextures = new Dictionary<string, string>();
            var lines = File.ReadAllLines(mtlFilePath);

            string currentMaterial = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("newmtl "))
                {
                    currentMaterial = line.Substring(7).Trim();
                }
                else if (line.StartsWith("map_Kd ") && currentMaterial != null)
                {
                    string texturePath = line.Substring(7).Trim();
                    materialTextures[currentMaterial] = texturePath;
                }
            }

            return materialTextures;
        }

    }
}
