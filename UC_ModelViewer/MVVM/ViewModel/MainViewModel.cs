using System.Collections.ObjectModel;
using System.Windows.Input;
using UC_ModelViewer.MVVM.Model;
using System.Linq;
using System.Windows;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace UC_ModelViewer.MVVM.ViewModel
{
    public class MainViewModel : ThreeDViewModel
    {
        public ThreeDViewModel ThreeDViewModel { get; private set; }

        private Nprim selectedNprim;
        public Nprim SelectedNprim
        {
            get => selectedNprim;
            set
            {
                selectedNprim = value;
                ThreeDViewModel.LoadObjModel(selectedNprim?.ObjContent);
                OnPropertyChanged(nameof(SelectedNprim));
            }
        }

        private ObservableCollection<Nprim> nprimList = new ObservableCollection<Nprim>();
        public ObservableCollection<Nprim> NprimList
        {
            get => nprimList;
            set
            {
                nprimList = value;
                OnPropertyChanged(nameof(NprimList));
            }
        }

        private string _primDirectory;
        public string PrimDirectory
        {
            get { return _primDirectory; }
            set
            {
                if (_primDirectory != value)
                {
                    _primDirectory = value;
                    OnPropertyChanged(nameof(PrimDirectory));
                }
            }
        }

        public ICommand ExportCommand { get; private set; }
        public ICommand LoadFilesCommand { get; private set; }
        public ICommand ViewPrimInfoCommand { get; private set; }
        public ICommand SelectDirectoryCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            PrimDirectory = "No Directory Selected";
            ThreeDViewModel = new ThreeDViewModel();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ExportCommand = new RelayCommand(param => ExportModelToObj(selectedNprim));
            LoadFilesCommand = new RelayCommand(param => LoadDirectory());
            ViewPrimInfoCommand = new RelayCommand(param => ViewPrimInfo(selectedNprim));
            SelectDirectoryCommand = new RelayCommand(param => SelectDirectory());
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //Hacky WPF directory picker makes a return
        private void SelectDirectory()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = PrimDirectory,
                Title = "Select a Directory",
                Filter = "Directory|*.this.directory",
                FileName = "select"
            };

            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");
                PrimDirectory = path;

                LoadDirectory();

            }
        }
        private void LoadDirectory()
        {

            if (string.IsNullOrEmpty(PrimDirectory) || PrimDirectory == null || PrimDirectory.Equals("No Directory Selected"))
            {
                System.Windows.MessageBox.Show("Please select a directory before loading files.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            var prmFiles = System.IO.Directory.GetFiles(PrimDirectory, "*.PRM");
            nprimList.Clear();

            if (prmFiles.Count() == 0)
            {
                System.Windows.MessageBox.Show("No models found in folder. Have you selected the correct directory? It is /server/prims", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            foreach (var prmFile in prmFiles)
            {
                try
                {
                    byte[] binaryData = File.ReadAllBytes(prmFile);
                    Nprim nprim = DecodeNprim(Path.GetFileName(prmFile), binaryData);
                    NprimList.Add(nprim);
                }
                catch (Exception ex)
                {
                   // Debug.WriteLine($"Error reading {prmFile}: {ex.Message}");
                }
            }
            ConvertToObj(NprimList);

        }
        public static Nprim DecodeNprim(string prmFile, byte[] data)
        {
            Nprim nprim = new Nprim
            {
                FileName = prmFile,
                Signature = BitConverter.ToInt16(data, 0),
                Name = ReadUntilNull(data, 2),
                FirstPointId = BitConverter.ToInt16(data, 34),
                LastPointId = BitConverter.ToInt16(data, 36),
                FirstQuadrangleId = BitConverter.ToInt16(data, 38),
                LastQuadrangleId = BitConverter.ToInt16(data, 40),
                FirstTriangleId = BitConverter.ToInt16(data, 42),
                LastTriangleId = BitConverter.ToInt16(data, 44),
                CollisionType = PrimMaps.GetCollisionTypeDescription(data[46]),
                ReactionToImpactByVehicle = data[47],
                ShadowType = data[48],
                VariousProperties = data[49],
            };

            nprim.Points = nprim.DecodePoints(data, BitConverter.ToInt16(data, 34), BitConverter.ToInt16(data, 36) - BitConverter.ToInt16(data, 34), nprim.FindCursorPos(nprim, "Points"));
            nprim.Triangles = nprim.DecodeTriangles(data, BitConverter.ToInt16(data, 42), BitConverter.ToInt16(data, 44) - BitConverter.ToInt16(data, 42), nprim.FindCursorPos(nprim, "Triangles"));
            nprim.Quadrangles = nprim.DecodeQuadrangles(data, BitConverter.ToInt16(data, 38), BitConverter.ToInt16(data, 40) - BitConverter.ToInt16(data, 38), nprim.FindCursorPos(nprim, "Quadrangles"));
            nprim.UVQuadCoordinates = new List<List<double>>();
            nprim.UVTriCoordinates = new List<List<double>>();
            nprim.ObjContent = "";
            nprim.MtlContent = ""; //Not needed anymore

            return nprim;
        }
        private void ViewPrimInfo(Nprim selectedNprim)
        {
            if (selectedNprim == null)
            {
                MessageBox.Show("No PRM selected. Have you selected the PRM directory?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StringBuilder info = new StringBuilder();
            info.AppendLine($"Name: \n\t{selectedNprim.Name}");
            info.AppendLine($"Filename: \n\t{selectedNprim.FileName}\n");
            info.AppendLine($"Collision Type: \n\t{selectedNprim.CollisionType}");
            byte impactProperties = (byte)selectedNprim.ReactionToImpactByVehicle;
            //Damage properties are Boolean flags - definiotions stolen from Urban Chaos Source code - not sure what they mean in practise
            info.AppendLine($"\nDamage Type(s):");
            info.AppendLine($"\tIs Damagable: {(impactProperties & 0x01) != 0}");
            info.AppendLine($"\tExplodes: {(impactProperties & 0x02) != 0}");
            info.AppendLine($"\tCrumples: {(impactProperties & 0x04) != 0}");
            info.AppendLine($"\tNo Line-of-Sight: {(impactProperties & 0x08) != 0}");
            string shadowTypeDescription = PrimMaps.GetShadowTypeDescription(selectedNprim.ShadowType);
            info.AppendLine($"\nShadow Type: \n\t{shadowTypeDescription}\n");
            //Various properties are Boolean flags - meaning stolen from Urban Chaos Source code
            byte variousProperties = (byte)selectedNprim.VariousProperties;
            info.AppendLine("Various Properties:");
            info.AppendLine($"\tIs Light: {(variousProperties & 0x01) != 0}");
            info.AppendLine($"\tContains Walk-able Faces: {(variousProperties & 0x02) != 0}");
            info.AppendLine($"\tHas Glare: {(variousProperties & 0x04) != 0}");
            info.AppendLine($"\tIs Rotating Item: {(variousProperties & 0x08) != 0}");
            info.AppendLine($"\tIs Tree: {(variousProperties & 0x10) != 0}");
            info.AppendLine($"\tEnv Mapped: {(variousProperties & 0x20) != 0}");
            info.AppendLine($"\tJust Floor: {(variousProperties & 0x40) != 0}");
            info.AppendLine($"\tOn Floor: {(variousProperties & 0x80) != 0}");

            info.AppendLine($"\nModel Information:");
            info.AppendLine($"\tPoints: {selectedNprim.Points.Count}");
            info.AppendLine($"\tTriangles: {selectedNprim.Triangles.Count}");
            info.AppendLine($"\tQuadrangles: {selectedNprim.Quadrangles.Count}");

            MessageBox.Show(info.ToString(), "Nprim Information");
        }
        private void ConvertToObj(ObservableCollection<Nprim> nprimList)
        {
            if (nprimList == null || nprimList.Count == 0)
            {
                System.Windows.MessageBox.Show("No Nprims to display.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            foreach (var nprim in nprimList)
            {
                StringBuilder objContent = new StringBuilder();
                List<UVQuadrangleResult> uvFullSetQ = new List<UVQuadrangleResult>();
                List<UVTriangleResult> uvFullSetT = new List<UVTriangleResult>();
                List<int> textureFileNumbers = new List<int>();
                List<int> texturesPerQuadFace = new List<int>();
                List<int> texturesPerTriFace = new List<int>();

                //vertices
                foreach (var point in nprim.Points)
                {
                    nprim.AppendToObjContent($"v {point.X} {point.Y} {point.Z}");
                }

                //quadrangles
                foreach (var quadrangle in nprim.Quadrangles)
                {
                    int textureIdGroup = quadrangle.TextureIdGroup;
                    float ua = quadrangle.UA;
                    float va = quadrangle.VA;
                    float ub = quadrangle.UB;
                    float vb = quadrangle.VB;
                    float uc = quadrangle.UC;
                    float vc = quadrangle.VC;
                    float ud = quadrangle.UD;
                    float vd = quadrangle.VD;

                    UVQuadrangleResult quadrangleUV = Textures.CalcUVs(ua, va, ub, vb, uc, vc, ud, vd, textureIdGroup, textureFileNumbers);
                    nprim.UVQuadCoordinates.Add(quadrangleUV.UV0.ToList());
                    nprim.UVQuadCoordinates.Add(quadrangleUV.UV1.ToList());
                    nprim.UVQuadCoordinates.Add(quadrangleUV.UV2.ToList());
                    nprim.UVQuadCoordinates.Add(quadrangleUV.UV3.ToList());

                    texturesPerQuadFace.Add(quadrangleUV.TextureImgNo);
                    uvFullSetQ.Add(quadrangleUV);

                    nprim.AppendToObjContent($"vt {quadrangleUV.UV0[0]} {quadrangleUV.UV0[1]}");
                    nprim.AppendToObjContent($"vt {quadrangleUV.UV1[0]} {quadrangleUV.UV1[1]}");
                    nprim.AppendToObjContent($"vt {quadrangleUV.UV2[0]} {quadrangleUV.UV2[1]}");
                    nprim.AppendToObjContent($"vt {quadrangleUV.UV3[0]} {quadrangleUV.UV3[1]}");

                    List<int> a = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == quadrangle.PointAId) };
                    List<int> b = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == quadrangle.PointBId) };
                    List<int> c = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == quadrangle.PointCId) };
                    List<int> d = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == quadrangle.PointDId) };

                    int uvaid = nprim.UVQuadCoordinates.FindIndex(coord => coord.SequenceEqual(quadrangleUV.UV0)) + 1;
                    int uvbid = nprim.UVQuadCoordinates.FindIndex(coord => coord.SequenceEqual(quadrangleUV.UV1)) + 1;
                    int uvcid = nprim.UVQuadCoordinates.FindIndex(coord => coord.SequenceEqual(quadrangleUV.UV2)) + 1;
                    int uvdid = nprim.UVQuadCoordinates.FindIndex(coord => coord.SequenceEqual(quadrangleUV.UV3)) + 1;

                    string line = $"f {a[0] + 1}/{uvaid} {b[0] + 1}/{uvbid} {d[0] + 1}/{uvdid} {c[0] + 1}/{uvcid}";
                    nprim.AddTextureLine(quadrangleUV.TextureImgNo, line);

                }

                //triangles
                foreach (var triangles in nprim.Triangles)
                {
                    int textureIdGroup = triangles.TextureIdGroup;
                    float ua = triangles.UA;
                    float va = triangles.VA;
                    float ub = triangles.UB;
                    float vb = triangles.VB;
                    float uc = triangles.UC;
                    float vc = triangles.VC;

                    UVTriangleResult triangleUV = Textures.CalcUVs(ua, va, ub, vb, uc, vc, textureIdGroup, textureFileNumbers);
                    nprim.UVTriCoordinates.Add(triangleUV.UV0.ToList());
                    nprim.UVTriCoordinates.Add(triangleUV.UV1.ToList());
                    nprim.UVTriCoordinates.Add(triangleUV.UV2.ToList());

                    texturesPerTriFace.Add(triangleUV.TextureImgNo);
                    uvFullSetT.Add(triangleUV);

                    nprim.AppendToObjContent($"vt {triangleUV.UV0[0]} {triangleUV.UV0[1]}");
                    nprim.AppendToObjContent($"vt {triangleUV.UV1[0]} {triangleUV.UV1[1]}");
                    nprim.AppendToObjContent($"vt {triangleUV.UV2[0]} {triangleUV.UV2[1]}");

                    //material lines go here
                    List<int> a = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == triangles.PointAId) };
                    List<int> b = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == triangles.PointBId) };
                    List<int> c = new List<int> { nprim.Points.FindIndex(point => point.PGlobalId == triangles.PointCId) };

                    int uvaid = nprim.UVTriCoordinates.FindIndex(coord => coord.SequenceEqual(triangleUV.UV0)) + 1 + nprim.UVQuadCoordinates.Count;
                    int uvbid = nprim.UVTriCoordinates.FindIndex(coord => coord.SequenceEqual(triangleUV.UV1)) + 1 + nprim.UVQuadCoordinates.Count;
                    int uvcid = nprim.UVTriCoordinates.FindIndex(coord => coord.SequenceEqual(triangleUV.UV2)) + 1 + nprim.UVQuadCoordinates.Count;

                    string line = $"f {a[0] + 1}/{uvaid} {b[0] + 1}/{uvbid} {c[0] + 1}/{uvcid}";
                    nprim.AddTextureLine(triangleUV.TextureImgNo, line);
                }
                string material = Textures.GetAllTextureLines(nprim);
                nprim.AppendToObjContent(material);
                string MtlContent = Textures.GetAllMtl(nprim, PrimDirectory);
                nprim.AppendToObjContent(MtlContent);
            }
            List<string> fileNames = new List<string>
                {
                    PrimDirectory + "/../../clumps/assault1.txc",
                    PrimDirectory + "/../../clumps/awol1.txc",
                    PrimDirectory + "/../../clumps/Baalrog3.txc",
                    PrimDirectory + "/../../clumps/e3.txc",
                    PrimDirectory + "/../../clumps/botanicc.txc",
                    PrimDirectory + "/../../clumps/bankbomb1.txc",
                    PrimDirectory + "/../../clumps/bball2.txc",
                    PrimDirectory + "/../../clumps/creds1.txc",
                    PrimDirectory + "/../../clumps/estate2.txc",
                    PrimDirectory + "/../../clumps/finale1.txc",
                    PrimDirectory + "/../../clumps/jung3.txc",
                    PrimDirectory + "/../../clumps/police1.txc"
                };

            //Unpack the required TXC Files - should add a check to see if this is already done? This takes a fair chunk of compute time
            Textures.ProcessTXCFile(fileNames);
        }
        private void ExportModelToObj(Nprim nprim)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string modelObjPath = Path.Combine(currentDirectory, "model.obj");
            string modelMtlPath = Path.Combine(currentDirectory, "model.mtl");

            // Check if model.obj and model.mtl exist
            if (!File.Exists(modelObjPath) || !File.Exists(modelMtlPath))
            {
                MessageBox.Show("Models for export not found. Have you selected a Model?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var materials = new List<string>();
            var lines = File.ReadAllLines(modelMtlPath);
            var nprimName = Path.GetFileNameWithoutExtension(nprim.FileName) + "-" + nprim.Name.Trim();

            foreach (var line in lines)
            {
                if (line.StartsWith("map_Kd"))
                {
                    materials.Add(line.Split(new[] { ' ' }, 2)[1].Trim());
                }
            }

            // Create "Exports" folder and subfolder with nprim.Name
            string exportsFolderPath = Path.Combine(currentDirectory, "Exported Objects");
            string newFolderPath = Path.Combine(exportsFolderPath, nprimName);
            Directory.CreateDirectory(newFolderPath);

            // Copy and rename model files
            File.Copy(modelObjPath, Path.Combine(newFolderPath, nprimName + ".obj"), true);
            File.Copy(modelMtlPath, Path.Combine(newFolderPath, nprimName + ".mtl"), true);

            // Copy material bmp files
            foreach (var materialPath in materials)
            {
                string fileName = Path.GetFileName(materialPath);
                File.Copy(materialPath, Path.Combine(newFolderPath, fileName), true);
            }

            string message = $"Export Successful - Saved to {newFolderPath}";
            MessageBox.Show(message, "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        public static string ReadUntilNull(byte[] data, int startIndex)
        {
            int index = startIndex;

            while (index < data.Length && data[index] != 0)
            {
                index++;
            }

            return System.Text.Encoding.UTF8.GetString(data, startIndex, index - startIndex);
        }
    }
}
