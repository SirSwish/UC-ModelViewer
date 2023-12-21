using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace UC_ModelViewer.MVVM.Model
{
    public class Nprim
    {
        public int Signature { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ObjContent { get; set; }
        public string MtlContent { get; set; }//dont think we need this anymore
        public int FirstPointId { get; set; }
        public int LastPointId { get; set; }
        public int FirstQuadrangleId { get; set; }
        public int LastQuadrangleId { get; set; }
        public int FirstTriangleId { get; set; }
        public int LastTriangleId { get; set; }
        public string CollisionType { get; set; }
        public int ReactionToImpactByVehicle { get; set; }
        public int ShadowType { get; set; }
        public int VariousProperties { get; set; }
        public List<Point3D> Points { get; set; }
        public List<Triangle> Triangles { get; set; }
        public List<Quadrangle> Quadrangles { get; set; }
        public List<List<double>> UVQuadCoordinates { get; set; }
        public List<List<double>> UVTriCoordinates { get; set; }
        public Dictionary<int, List<string>> TextureLines { get; } = new Dictionary<int, List<string>>();

        // Function to add lines for a specific texture ID - this will be useful when passing NPRIMS around to object loader
        public void AddTextureLine(int textureId, string line)
        {
            if (!TextureLines.ContainsKey(textureId))
            {
                TextureLines[textureId] = new List<string>();
            }

            TextureLines[textureId].Add(line);
        }

        public int FindCursorPos(Nprim nprim, string itemType)
        {
            int cursorPos = 50;

            int bytesForPoints = (nprim.LastPointId - nprim.FirstPointId) * 6;

            if (itemType == "Points")
            {
                // cursorPos remains at the default starting position (50)
            }
            else if (itemType == "Triangles")
            {
                cursorPos += bytesForPoints;
            }
            else if (itemType == "Quadrangles")
            {
                int bytesForTriangles = (nprim.LastTriangleId - nprim.FirstTriangleId) * 28;
                cursorPos += bytesForPoints + bytesForTriangles;
            }

            return cursorPos;
        }
        public List<Point3D> DecodePoints(byte[] data, int firstPointId, int pointCount, int cursor)
        {
            List<Point3D> Points = new List<Point3D>();

            for (int pId = 0; pId < pointCount; pId++)
            {
                
                int pGlobalId = firstPointId + pId;

                short x = (short)BitConverter.ToInt16(data, cursor);
                short y = (short)BitConverter.ToInt16(data, cursor + 2);
                short z = (short)BitConverter.ToInt16(data, cursor + 4);

                cursor += 6;

                Points.Add(new Point3D { PGlobalId = pGlobalId, X = x, Y = y, Z = z });
            }
            return Points;
        }
        public List<Triangle> DecodeTriangles(byte[] data, int firstTriangleId, int triangleCount, int cursor)
        {
            List<Triangle> triangles = new List<Triangle>();

            for (int tId = 0; tId < triangleCount; tId++)
            {
                Triangle triangle = new Triangle();
                triangle.TextureIdGroup = data[cursor];
                triangle.Properties = data[cursor + 1];

                triangle.PointAId = BitConverter.ToInt16(data, cursor + 2);
                triangle.PointBId = BitConverter.ToInt16(data, cursor + 4);
                triangle.PointCId = BitConverter.ToInt16(data, cursor + 6);

                triangle.UA = data[cursor + 8];
                triangle.VA = data[cursor + 9];
                triangle.UB = data[cursor + 10];
                triangle.VB = data[cursor + 11];
                triangle.UC = data[cursor + 12];
                triangle.VC = data[cursor + 13];

                triangle.BrightA = data[cursor + 14];
                triangle.BrightB = data[cursor + 15];
                triangle.BrightC = data[cursor + 16];

                triangles.Add(triangle);

                cursor += 28;
            }

            return triangles;
        }

        public List<Quadrangle> DecodeQuadrangles(byte[] data, int firstQuadrangleId, int quadrangleCount, int cursor)
        {
            List<Quadrangle> quadrangles = new List<Quadrangle>();

            for (int qId = 0; qId < quadrangleCount; qId++)
            {
                Quadrangle quadrangle = new Quadrangle();
                quadrangle.TextureIdGroup = data[cursor];
                quadrangle.Properties = data[cursor + 1];

                quadrangle.PointAId = BitConverter.ToInt16(data, cursor + 2);
                quadrangle.PointBId = BitConverter.ToInt16(data, cursor + 4);
                quadrangle.PointCId = BitConverter.ToInt16(data, cursor + 6);
                quadrangle.PointDId = BitConverter.ToInt16(data, cursor + 8);

                quadrangle.UA = data[cursor + 10];
                quadrangle.VA = data[cursor + 11];
                quadrangle.UB = data[cursor + 12];
                quadrangle.VB = data[cursor + 13];
                quadrangle.UC = data[cursor + 14];
                quadrangle.VC = data[cursor + 15];
                quadrangle.UD = data[cursor + 16];
                quadrangle.VD = data[cursor + 17];

                quadrangle.BrightA = data[cursor + 18];
                quadrangle.BrightB = data[cursor + 19];
                quadrangle.BrightC = data[cursor + 20];
                quadrangle.BrightD = data[cursor + 21];

                quadrangles.Add(quadrangle);

                cursor += 34;
            }

            return quadrangles;
        }
        //Given we are technically dealing with Nprim object, not vanilla StringBuilder object, quick method to add text to it
        public void AppendToObjContent(string line)
        {
            if (line == null) return;
            ObjContent += line + Environment.NewLine;
        }

    }

    public class Point3D
    {
        public int PGlobalId { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
    }

    public class Triangle
    {
        public int TextureIdGroup { get; set; }
        public int Properties { get; set; }

        public int PointAId { get; set; }
        public int PointBId { get; set; }
        public int PointCId { get; set; }

        public int UA { get; set; }
        public int VA { get; set; }
        public int UB { get; set; }
        public int VB { get; set; }
        public int UC { get; set; }
        public int VC { get; set; }

        public int BrightA { get; set; }
        public int BrightB { get; set; }
        public int BrightC { get; set; }

    }

    public class Quadrangle
    {
        public byte TextureIdGroup { get; set; }
        public byte Properties { get; set; }

        public short PointAId { get; set; }
        public short PointBId { get; set; }
        public short PointCId { get; set; }
        public short PointDId { get; set; }

        public byte UA { get; set; }
        public byte VA { get; set; }
        public byte UB { get; set; }
        public byte VB { get; set; }
        public byte UC { get; set; }
        public byte VC { get; set; }
        public byte UD { get; set; }
        public byte VD { get; set; }

        public byte BrightA { get; set; }
        public byte BrightB { get; set; }
        public byte BrightC { get; set; }
        public byte BrightD { get; set; }

    }
}
