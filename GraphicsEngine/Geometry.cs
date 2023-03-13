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
        public List<Triangle> triangles;
    }
}
