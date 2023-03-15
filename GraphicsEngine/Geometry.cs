using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GraphicsEngine
{
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

    class Point
    {
        public double x;
        public double y;
        public Color color;

        public Point(double x, double y, byte r=0, byte g=0, byte b=0)
        {
            this.x = x;
            this.y = y;
            this.color.R = (byte)r;
            this.color.G = (byte)g;
            this.color.B = (byte)b;
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }
    }
}
