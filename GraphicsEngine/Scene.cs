using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Threading;
using System.Drawing;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace GraphicsEngine
{
    internal class Scene
    {
        public Mesh model = new Mesh();
        public System.Windows.Controls.Image canvas { get; set; }
        
        // ! make it dynamic later not just on startup
        public int ScreenHeight;
        public int ScreenWidth;
        public Thread currentRenderProcess;

        public readonly double scale = 100;
        private readonly double[,] projection_matrix = new double[,] { { 1, 0, 0 }, { 0, 1, 0 } };
        
        private readonly Functions func = new Functions();
        public void Render()
        {
            currentRenderProcess?.Abort();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Name = "RenderProcess";
                currentRenderProcess = Thread.CurrentThread;
                double angle = 0;

                while (model != null)
                {
                    Bitmap image = new Bitmap(ScreenWidth, ScreenHeight);

                    foreach (Triangle triangle in model.triangles)
                    {
                        List<(int, int)> points = new List<(int, int)>();
                        foreach (Vector3D vertex in triangle.Vertices)
                        {
                            double[,] rotated2d = func.Multiply(func.RotateX(angle), new double[,] { { vertex.X }, { vertex.Y }, { vertex.Z } });
                            rotated2d = func.Multiply(func.RotateY(angle), rotated2d);
                            rotated2d = func.Multiply(func.RotateZ(angle), rotated2d);

                            double[,] projected2d = func.Multiply(projection_matrix, rotated2d);
                            int x = (int)(projected2d[0, 0] * scale + ScreenHeight/2);
                            int y = (int)(projected2d[1, 0] * scale + ScreenHeight/2);
                            points.Add((x, y));
                        }
                        func.Bresenham(image, points[0].Item1, points[0].Item2, points[1].Item1, points[1].Item2);
                        func.Bresenham(image, points[1].Item1, points[1].Item2, points[2].Item1, points[2].Item2);
                        func.Bresenham(image, points[2].Item1, points[2].Item2, points[0].Item1, points[0].Item2);
                    }
                    canvas.Dispatcher.Invoke(() => canvas.Source = func.BitmapToImageSource(image));
                    angle += .01;
                }
                // at the end clear screen
                canvas.Dispatcher.Invoke(() => canvas.Source = null);
            }).Start();
        }
    }
}
