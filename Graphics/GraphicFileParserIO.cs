using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CS3D.struts;
using CS3D.DataTypes.FundamentalGraphics;
using CS3D.dataTypes;
using System.Drawing;

namespace CS3D.StaticData
{
    static class ObjParser
    {
        public static Triangle[] LoadObj(string filename)
        {
            //buffers used to make triangles
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<VerticeUVIndex> indexes = new List<VerticeUVIndex>();

            //loading file line by line and parsing
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = String.Empty; //line buffer

                while ((line = sr.ReadLine()) != null) //read till end of file
                {
                    //loading line
                    var values = Regex.Split(line, " ");
                    var type = values[0];

                    //Parser line type
                    switch (type)
                    {
                        case "vt": //uv
                            uvs.Add(CreateUV(values[1], values[2]));
                            break;

                        case "v": //Vertice
                            vertices.Add(CreateVertice(values[1], values[2], values[3]));
                            break;

                        case "f": //face
                            indexes.Add(CreateIndexes(values[1], values[2], values[3]));
                            break;
                    }
                }
            }

            //make triangle list
            Triangle[] returnList = new Triangle[indexes.Count];

            for(int i = 0; i < indexes.Count; i++)
            {
                returnList[i] = new Triangle(
                    vertices[indexes[i].vertexIndex1 - 1], uvs[indexes[i].uvIndex1 - 1],
                    vertices[indexes[i].vertexIndex2 - 1], uvs[indexes[i].uvIndex2 - 1],
                    vertices[indexes[i].vertexIndex3 - 1], uvs[indexes[i].uvIndex3 - 1]);
            }

            return returnList;
        }

        private struct VerticeUVIndex
        {
            public int vertexIndex1, uvIndex1;
            public int vertexIndex2, uvIndex2;
            public int vertexIndex3, uvIndex3;
        }

        private static VerticeUVIndex CreateIndexes(string index1, string index2, string index3 )
        {
            string[] values1 = index1.Split('/');
            string[] values2 = index2.Split('/');
            string[] values3 = index3.Split('/');

            return new VerticeUVIndex
            {
                vertexIndex1 = int.Parse(values1[0]),
                uvIndex1 = int.Parse(values1[0]),

                vertexIndex2 = int.Parse(values2[0]),
                uvIndex2 = int.Parse(values2[0]),

                vertexIndex3 = int.Parse(values3[0]),
                uvIndex3 = int.Parse(values3[0])
            };
        }

        private static Vector2 CreateUV(string u, string v)
        {
            return new Vector2() { u = float.Parse(u), v = float.Parse(v) };
        }

        private static Vector3 CreateVertice(string x, string y, string z)
        {
            return new Vector3() { x = float.Parse(x), y = float.Parse(y), z = float.Parse(z) };
        }
    }

    //image to pixal array
    static class ImgParser
    {
        public static Pixel[,] LoadImage(string filename)
        {
            Bitmap tmpImg = (Bitmap)Image.FromFile(filename);
            Pixel[,] returnArray = new Pixel[tmpImg.Width, tmpImg.Height];

            for(int x = 0; x < tmpImg.Width; x++)
                for (int y = 0; y < tmpImg.Height; y++)
                {
                    Color tmpColor = tmpImg.GetPixel(x, y);

                    returnArray[x, y] = new Pixel(
                        tmpColor.R,
                        tmpColor.G,
                        tmpColor.B,
                        tmpColor.A,
                        0);
                }

            return returnArray;
        }
    }
}
