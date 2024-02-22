using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using File = System.IO.File;

namespace UC_ModelViewer.MVVM.Model
{
    //Texture related stuff lives here
    public class Textures
    {

        public static string GetTextureLines(Nprim nprim, int textureId)
        {
            StringBuilder builder = new StringBuilder();

            if (nprim.TextureLines.ContainsKey(textureId))
            {
                builder.AppendLine($"usemtl Material.{textureId}");

                foreach (string line in nprim.TextureLines[textureId])
                {
                    builder.AppendLine(line);
                }
            }
            else
            {
                builder.AppendLine($"No lines found for texture ID {textureId}");
            }


            return builder.ToString();
        }

        public static string GetAllTextureLines(Nprim nprim)
        {
            //Helix does not love mtllib definitions being anywhere but at the top so write it first
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("mtllib model.mtl");
            foreach (var kvp in nprim.TextureLines)
            {
                builder.Append(GetTextureLines(nprim, kvp.Key));
            }

            return builder.ToString();
        }
        public static string GetAllMtl(Nprim nprim, string directory) 
        {
            StringBuilder MtlContent = new StringBuilder();
            MtlContent.AppendLine($"[MTL]");
            foreach (var key in nprim.TextureLines.Keys)
            {
                string keyPadded = key.ToString("D3"); // Ensures the key is represented with 3 digits

                MtlContent.AppendLine($"\n");
                MtlContent.AppendLine($"newmtl Material.{key}");
                MtlContent.AppendLine("Ns 250.000000");
                MtlContent.AppendLine("Ns 0.000000");
                MtlContent.AppendLine("Ka 1.000000 1.000000 1.000000");
                MtlContent.AppendLine("Ks 0.000000 0.000000 0.000000");
                MtlContent.AppendLine("Ke 0.000000 0.000000 0.000000");
                MtlContent.AppendLine("Ni 1.450000");
                MtlContent.AppendLine("d 1.000000");
                MtlContent.AppendLine("illum 1");
                MtlContent.AppendLine($"map_Kd tex{keyPadded}hi.bmp");
            }

            return MtlContent.ToString();
        }

        public static UVQuadrangleResult CalcUVs(double u0, double v0, double u1, double v1, double u2, double v2, double u3, double v3, int texturePage, List<int> textureNumbers)
        {
            const int TextureNormSize = 32;
            const int TextureNormSquares = 8;

            double avU = CalcAverageUV(u0, u1, u2, u3);
            double avV = CalcAverageUV(v0, v1, v2, v3);
            int avUInt = (int)(avU / TextureNormSize);
            int avVInt = (int)(avV / TextureNormSize);

            int baseU = avUInt * TextureNormSize;
            int baseV = avVInt * TextureNormSize;

            double finalU0 = CalcFinalUV(u0, baseU);
            double finalV0 = CalcFinalUV(v0, baseV);
            double finalU1 = CalcFinalUV(u1, baseU);
            double finalV1 = CalcFinalUV(v1, baseV);
            double finalU2 = CalcFinalUV(u2, baseU);
            double finalV2 = CalcFinalUV(v2, baseV);
            double finalU3 = CalcFinalUV(u3, baseU);
            double finalV3 = CalcFinalUV(v3, baseV);

            int page = avUInt + avVInt * TextureNormSquares + texturePage * TextureNormSquares * TextureNormSquares;
            int textureImgNo = page - 64 * 11;

            if (textureImgNo < 0 || textureImgNo > 352 || textureImgNo == 161 || textureImgNo == 140)
            {
                textureImgNo = 0;
            }

            if (!textureNumbers.Contains(textureImgNo))
            {
                textureNumbers.Add(textureImgNo);
            }

            finalV0 = Math.Abs(1 - finalV0);
            finalV1 = Math.Abs(1 - finalV1);
            finalV2 = Math.Abs(1 - finalV2);
            finalV3 = Math.Abs(1 - finalV3);

            return new UVQuadrangleResult
            {
                TextureImgNo = textureImgNo,
                UV0 = new List<double> { finalU0, finalV0 },
                UV1 = new List<double> { finalU1, finalV1 },
                UV2 = new List<double> { finalU2, finalV2 },
                UV3 = new List<double> { finalU3, finalV3 }
            };
        }

        public static UVTriangleResult CalcUVs(double u0, double v0, double u1, double v1, double u2, double v2, int texturePage, List<int> textureNumbers)
        {
            const int TextureNormSize = 32;
            const int TextureNormSquares = 8;

            double avU = CalcAverageUV(u0, u1, u2);
            double avV = CalcAverageUV(v0, v1, v2);
            int avUInt = (int)(avU / TextureNormSize);
            int avVInt = (int)(avV / TextureNormSize);

            int baseU = avUInt * TextureNormSize;
            int baseV = avVInt * TextureNormSize;

            double finalU0 = CalcFinalUV(u0, baseU);
            double finalV0 = CalcFinalUV(v0, baseV);
            double finalU1 = CalcFinalUV(u1, baseU);
            double finalV1 = CalcFinalUV(v1, baseV);
            double finalU2 = CalcFinalUV(u2, baseU);
            double finalV2 = CalcFinalUV(v2, baseV);

            int page = avUInt + avVInt * TextureNormSquares + texturePage * TextureNormSquares * TextureNormSquares;
            int textureImgNo = page - 64 * 11;

            if (!textureNumbers.Contains(textureImgNo))
            {
                textureNumbers.Add(textureImgNo);
            }

            finalV0 = Math.Abs(1 - finalV0);
            finalV1 = Math.Abs(1 - finalV1);
            finalV2 = Math.Abs(1 - finalV2);

            return new UVTriangleResult
            {
                TextureImgNo = textureImgNo,
                UV0 = new List<double> { finalU0, finalV0 },
                UV1 = new List<double> { finalU1, finalV1 },
                UV2 = new List<double> { finalU2, finalV2 }
            };
        }

        private static double CalcAverageUV(double a, double b, double c, double d)
        {
            return (a + b + c + d) / 4.0;
        }
        private static double CalcAverageUV(double a, double b, double c)
        {
            return (a + b + c) / 3.0;
        }
        private static double CalcFinalUV(double u, int baseU)
        {
            double result = (u - baseU) / 32.0;
            if (result == 31)
            {
                result = 32;
            }
            return result;
        }
        public static void ProcessTXCFile(List<string> filenames)
        {

            foreach (var fileName in filenames) {
                
                if (!File.Exists(fileName))
                {
                   // Debug.WriteLine($"Cannot open {fileName}");
                    return;
                }

                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                using (var reader = new BinaryReader(fileStream))
                {
                    int nNumElements = reader.ReadInt32();
                    int[] offsets = new int[nNumElements];
                    int[] sizes = new int[nNumElements];

                    for (int i = 0; i < nNumElements; i++)
                    {
                        offsets[i] = reader.ReadInt32();
                    }

                    for (int i = 0; i < nNumElements; i++)
                    {
                        sizes[i] = reader.ReadInt32();
                    }

                    for (int i = 0; i < nNumElements; i++)
                    {
                        if (offsets[i] != 0 && sizes[i] != 0)
                        {
                            string file = Path.GetFileName(fileName);
                            string outFileName = Directory.GetCurrentDirectory() + $"\\{file}_out\\tex{i}.tex";

                            fileStream.Seek(offsets[i], SeekOrigin.Begin);
                            byte[] data = reader.ReadBytes(sizes[i]);

                          //  Debug.WriteLine($"filename: {outFileName}, offset: 0x{offsets[i]:X}, filesize: {sizes[i]}");
                            SaveFile(outFileName, data);
                        }
                        else
                        {
                          //  Debug.WriteLine($"Empty file: filename: {fileName}, offset: 0x{offsets[i]:X}, size: {sizes[i]}");
                        }
                    }

                }
            
            }
        }

        static void SaveFile(string fileName, byte[] data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllBytes(fileName, data);
        }

    }
}
