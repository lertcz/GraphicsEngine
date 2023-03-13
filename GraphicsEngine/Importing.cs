using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
using System.Windows;

namespace GraphicsEngine
{
    internal class Importing
    {
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
            List<double> vertices = new List<double>();
            List<double> normals = new List<double>();
            List<double> textureCoordinates = new List<double>();
            List<(int, int, int)> faces = new List<(int, int, int)>();

            foreach (string line in text.Split('\n'))
            {
                MessageBox.Show(line);
            }
            MessageBox.Show(text);
        }
    }
}
