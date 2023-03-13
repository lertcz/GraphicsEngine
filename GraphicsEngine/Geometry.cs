using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GraphicsEngine
{
    //class Vector3
    //{
    //    public string X { get; set; }
    //    public string Y { get; set; }
    //    public string Z { get; set; }
    //    public Vector3(string x, string y, string z)
    //    {
    //        X = x;
    //        Y = y;
    //        Z = z;
    //    }
    //}
    class Triangle
    {
        public Vector3D[] Vertices;
        public Triangle(Vector3D[] vertices)
        {
            Vertices = vertices;
        }
    }
    class Mesh
    {
        public List<Triangle> triangles = new List<Triangle>();
    }

    class Face
    {
        public List<int> VertexIndex = new List<int>();
        public List<int> NormalIndex = new List<int>();
        public List<int> TextureIndex = new List<int>();
        public void AddIndexes(int vertex, int normal, int texture)
        {
            VertexIndex.Add(vertex);
            NormalIndex.Add(normal);
            TextureIndex.Add(texture);
        }
        public override string ToString()
        {
            string message = "";
            for (int i = 0; i < VertexIndex.Count; i++)
            {
                message += String.Format("V: {0}, N: {1}. T: {2}\n", VertexIndex[i], NormalIndex[i], TextureIndex[i]);
            }
            message += "------------\n";
            return message;
        }
    }
}
