using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GraphicsEngine
{
    internal class Importing
    {
        private readonly Scene scene;
        public Importing(Scene scene)
        {
            this.scene = scene;
        }
        
        public void GetFolder()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == false) return;

            ProcessDir(dialog.SelectedPath);
        }
        public void ProcessDir(string path)
        {
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);
            }
        }

        readonly string[] extensions = { ".jpg", ".png" };
        public void ProcessFile(string path)
        {
            if (extensions.Any(ext => path.EndsWith(ext)))
            {

            }
            else if (path.EndsWith(".mtl"))
            {

            }
            else if (path.EndsWith(".obj"))
            {
                ParseObjFile(path);
            }
        }

        public void ParseObjFile(string path)
        {
            string text = File.ReadAllText(path);
            List<Vector3D> vertices = new List<Vector3D>();
            List<Vector3D> normals = new List<Vector3D>();
            List<Vector> textureCoordinates = new List<Vector>();
            List<Face> faces = new List<Face>();

            // load data
            foreach (string line in text.Split('\n'))
            {
                string[] split = line.Split();
                if (line.StartsWith("v "))
                {
                    double x = double.Parse(split[1]);
                    double y = double.Parse(split[2]);
                    double z = double.Parse(split[3]);
                    vertices.Add(new Vector3D(x, y, z));
                }
                else if (line.StartsWith("vn "))
                {
                    double x = double.Parse(split[1]);
                    double y = double.Parse(split[2]);
                    double z = double.Parse(split[3]);
                    normals.Add(new Vector3D(x, y, z));
                }
                else if (line.StartsWith("vt "))
                {
                    double x = double.Parse(split[1]);
                    double y = double.Parse(split[2]);
                    textureCoordinates.Add(new Vector(x, y));
                }
                else if (line.StartsWith("f "))
                {
                    Face face = new Face();
                    foreach (string element in split.Skip(1).ToArray())
                    {
                        string[] indexes = element.Split('/');
                        face.AddIndexes(int.Parse(indexes[0]) - 1,
                                        int.Parse(indexes[1]) - 1,
                                        int.Parse(indexes[2]) - 1);
                    }
                    faces.Add(face);
                }
            }

            // create triangles
            Mesh model = new Mesh();
            foreach (Face face in faces)
            {
                int numberOfFaces = face.VertexIndex.Count;
                if (numberOfFaces == 3)
                {
                    model.triangles.Add(
                        new Triangle( new Vector3D[]
                        {
                            vertices[face.VertexIndex[0]], vertices[face.VertexIndex[1]], vertices[face.VertexIndex[2]]
                        })
                    );
                }
                else if (numberOfFaces == 4)
                {
                    model.triangles.Add(
                        new Triangle(new Vector3D[]
                        {
                            vertices[face.VertexIndex[0]], vertices[face.VertexIndex[1]], vertices[face.VertexIndex[2]]
                        })
                    );
                    model.triangles.Add(
                        new Triangle(new Vector3D[]
                        {
                            vertices[face.VertexIndex[0]], vertices[face.VertexIndex[2]], vertices[face.VertexIndex[3]]
                        })
                    );
                }
                else
                {
                    MessageBox.Show("Model must have triangles or squares only!");
                    return;
                }
            }

            scene.model = model;
            scene.Render();
        }
    }
}
