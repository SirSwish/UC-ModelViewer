using System;
using System.Diagnostics;
using System.IO;

namespace UC_ModelViewer.MVVM.Model
{
    public class Tex2Tga
    {
        private static readonly byte[] LookupTable4BitTo8Bit = new byte[16];
        private static readonly byte[] LookupTable5BitTo8Bit = new byte[32];
        private static readonly byte[] LookupTable6BitTo8Bit = new byte[64];

        static Tex2Tga()
        {
            for (int i = 0; i < 16; i++)
            {
                LookupTable4BitTo8Bit[i] = (byte)(i | (i << 4));
            }

            for (int i = 0; i < 32; i++)
            {
                LookupTable5BitTo8Bit[i] = (byte)((i << 3) | (i >> 2));
            }

            for (int i = 0; i < 64; i++)
            {
                LookupTable6BitTo8Bit[i] = (byte)((i << 2) | (i >> 4));
            }
        }

        // Tex Header - 10 bytes
        public struct TxcTexHeader
        {
            public short Sig;      // Int16
            public bool HasAlpha;  // Bool16
            public ushort Width;   // UInt16
            public ushort Height;  // UInt16
            public ushort Size;    // UInt16
        }

        //TGA Header
        public struct TgaHeader
        {
            public byte IDlen;
            public byte ColorMapType;
            public byte ImageType;
            public ushort ColorMapOrigin;
            public ushort ColorMapLength;
            public byte ColorMapDepth;
            public ushort XOrigin;
            public ushort YOrigin;
            public ushort Width;
            public ushort Height;
            public byte Depth;
            public byte Descriptor;
        }

        public struct RGBA
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
        }

        public void ConvertTexToTga(string texFilename, string outfilename)
        {
            // Read the .tex file into a byte array
            byte[] texData = ReadTexFile(texFilename);

            ushort width, height;
            bool hasAlpha;


            // Extract width, height, hasAlpha, and image data from the texData
            ushort[] imageData = PrepareImage(texData, out width, out height, out hasAlpha);

            RGBA[] pixels = UncompressPixels(width, height, hasAlpha, imageData);

            // Export to TGA
            ExportTga(outfilename, width, height, pixels, hasAlpha);
        }

        private byte[] ReadTexFile(string texFilename)
        {
            try
            {
                if (!File.Exists(texFilename))
                {
                    throw new FileNotFoundException($"The file {texFilename} was not found.");
                }

                // Read and return the file content as a byte array
                return File.ReadAllBytes(texFilename);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"An error occurred while reading the file: {ex.Message}");
                throw;
            }
        }

        public static ushort[] PrepareImage(byte[] inData, out ushort w, out ushort h, out bool hasAlpha)
        {
            var header = ReadTexHeader(inData);
            ushort[] imageData = ReadImageData(inData, header);

            if (header.Sig != -1)
                throw new Exception("Invalid header - input file is not an Urban Chaos .tex file");

            hasAlpha = header.HasAlpha;
            w = header.Width;
            h = header.Height;

            int wh = w * h;
            ushort[] pixels = new ushort[wh];
            ushort[] colorTable = new ushort[0x10000];
            int iSize = header.Size;
            Array.Copy(imageData, colorTable, iSize);

            int iTempSize = 1;
            if (iSize > 2)
            {
                do
                {
                    ++iTempSize;
                }
                while (iSize > (1 << iTempSize));
            }

            int dataIndex = iSize;
            ushort texPixelData = imageData[dataIndex];
            int n = 16;
            ushort colorIndex;

            //Off-by-one?
            for (int i = 1; i < wh; i++)
            {
                if (n <= iTempSize)
                {
                    int a = iTempSize - n;
                    ushort b = texPixelData;
                    texPixelData = imageData[++dataIndex];
                    ushort c = (ushort)(texPixelData >> (16 - (iTempSize - n)));
                    n = 16 - (iTempSize - n);
                    colorIndex = (ushort)(c | (b >> (16 - iTempSize)));
                    texPixelData <<= a;
                }
                else
                {
                    colorIndex = (ushort)(texPixelData >> (16 - iTempSize));
                    texPixelData <<= iTempSize;
                    n -= iTempSize;
                }

                pixels[i] = colorTable[colorIndex];
            }
            return pixels;
        }

        private static TxcTexHeader ReadTexHeader(byte[] inData)
        {
            using (var memoryStream = new MemoryStream(inData))
            using (var reader = new BinaryReader(memoryStream))
            {
                TxcTexHeader header = new TxcTexHeader
                {
                    Sig = reader.ReadInt16(),
                    HasAlpha = reader.ReadInt16() != 0,
                    Width = reader.ReadUInt16(),
                    Height = reader.ReadUInt16(),
                    Size = reader.ReadUInt16()
                };

                return header;
            }
        }

        private static ushort[] ReadImageData(byte[] inData, TxcTexHeader header)
        {
            int headerSize = 10; // header is 10 bytes setting manually because Marshal.SizeOf returns 16 for some reason.
            int imageDataLength = inData.Length - headerSize;

            ushort[] imageData = new ushort[imageDataLength / 2];

            using (var memoryStream = new MemoryStream(inData))
            {
                memoryStream.Position = headerSize; // skip passed the header
                using (var reader = new BinaryReader(memoryStream))
                {
                    for (int i = 0; i < imageData.Length; i++)
                    {
                        imageData[i] = reader.ReadUInt16();
                    }
                }
            }

            return imageData;
        }

        private RGBA[] UncompressPixels(ushort width, ushort height, bool hasAlpha, ushort[] data)
        {
            int totalPixels = width * height;
            RGBA[] outPixels = new RGBA[totalPixels];

            if (hasAlpha)
            {
                for (int i = 0; i < totalPixels; i++)
                {
                    int mask = data[i];
                    outPixels[i].R = LookupTable4BitTo8Bit[mask & 15];
                    outPixels[i].G = LookupTable4BitTo8Bit[(mask >> 4) & 15];
                    outPixels[i].B = LookupTable4BitTo8Bit[(mask >> 8) & 15];
                    outPixels[i].A = LookupTable4BitTo8Bit[mask >> 12];
                }
            }
            else
            {
                for (int i = 0; i < totalPixels; i++)
                {
                    int mask = data[i];
                    outPixels[i].R = LookupTable5BitTo8Bit[mask & 31];
                    outPixels[i].G = LookupTable6BitTo8Bit[(mask >> 5) & 63];
                    outPixels[i].B = LookupTable5BitTo8Bit[mask >> 11];
                    outPixels[i].A = 0; // Alpha is not present in RGB565
                }
            }

            return outPixels;
    }

        private void ExportTga(string filename, ushort w, ushort h, RGBA[] pixels, bool hasAlpha)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                TgaHeader header;
                header.IDlen = 0;
                header.ColorMapType = 0;
                header.ImageType = 2;
                header.ColorMapOrigin = 0;
                header.ColorMapLength = 0;
                header.ColorMapDepth = 0;
                header.XOrigin = 0;
                header.YOrigin = 0;
                header.Width = w;
                header.Height = h;
                header.Depth = (byte)(hasAlpha ? 32 : 24); // 24-bit for RGB, 32-bit for RGBA
                header.Descriptor = 0;

                writer.Write(header.IDlen);
                writer.Write(header.ColorMapType);
                writer.Write(header.ImageType);
                writer.Write(header.ColorMapOrigin);
                writer.Write(header.ColorMapLength);
                writer.Write(header.ColorMapDepth);
                writer.Write(header.XOrigin);
                writer.Write(header.YOrigin);
                writer.Write(header.Width);
                writer.Write(header.Height);
                writer.Write(header.Depth);
                writer.Write(header.Descriptor);

                // Write actual pixel data
                for (int y = h - 1; y >= 0; y--)
                {
                    for (int x = 0; x < w; x++)
                    {
                        var pixel = pixels[(w * y) + x];
                        writer.Write(pixel.R);
                        writer.Write(pixel.G);
                        writer.Write(pixel.B);
                        if (hasAlpha)
                        {
                            writer.Write(pixel.A);
                        }
                    }
                }
            }
        }
    }
}
