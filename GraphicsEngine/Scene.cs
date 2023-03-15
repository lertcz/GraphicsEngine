﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Threading;
using System.Drawing;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Windows;

namespace GraphicsEngine
{
    internal class Scene : INotifyPropertyChanged
    {
        #region FPS
        public string WindowTitle { 
            get
            {
                return _WindowTitle;
            } 
            set 
            {
                if(this._WindowTitle != value)
                {
                    this._WindowTitle = value;
                    OnPropertyChanged("WindowTitle");
                }
                _WindowTitle = value; 
            } 
        }
        private string _WindowTitle = "GraphicsEngine";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private double fps;
        private double FrameCount;
        private DateTime Lastcounted = DateTime.Now;
        #endregion

        public Mesh model = new Mesh();
        public System.Windows.Controls.Image canvas { get; set; }

        public CheckBox Textures { get; set; }
        public CheckBox LowRes { get; set; }

        public double scale;
        public int FullScreenHeight, ScreenHeight;
        public int FullScreenWidth, ScreenWidth;

        private double[,] projection_matrix = new double[4, 4] {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        };

        public void InitializeProjectionMatrix(double hFOV, double near, double far)
        {
            // http://www.songho.ca/opengl/gl_projectionmatrix.html
            double AspectRatio = (double)ScreenWidth / (double)ScreenHeight;

            double vFOV = 2 * Math.Atan(Math.Tan(hFOV / 2.0 * (Math.PI / 180.0)) * AspectRatio) * 180.0 / Math.PI;

            double halfHeight = near * Math.Tan(vFOV / 2.0 * (Math.PI / 180.0)); // half height of near plane
            double halfWidth = halfHeight * AspectRatio;

            double left   =  halfWidth / 2;
            double right  = -halfWidth / 2;
            double top    =  halfHeight / 2;
            double bottom = -halfHeight / 2;

            projection_matrix[0, 0] = 2 * near / (right - left);
            projection_matrix[0, 2] = (right + left) / (right - left);
            projection_matrix[1, 1] = 2 * near / (top - bottom);
            projection_matrix[1, 2] = (top + bottom) / (top - bottom);
            projection_matrix[2, 2] = -(far + near) / (far - near);
            projection_matrix[2, 3] = -2 * far * near / (far - near);
            projection_matrix[3, 2] = -1;
        }

        private readonly Functions func = new Functions();
        public Thread currentRenderProcess;

        public void Render()
        {
            currentRenderProcess?.Abort();

            if (LowRes.IsChecked == false)
            {
                ScreenHeight = FullScreenHeight;
                ScreenWidth = FullScreenWidth;
                scale = 100;
            }
            else
            {
                ScreenHeight = FullScreenHeight / 4;
                ScreenWidth = FullScreenWidth / 4;
                scale = 25;
            }

            InitializeProjectionMatrix(90, .1, 100);

            bool RenderTextures = (bool)Textures.IsChecked;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Name = "RenderProcess";
                currentRenderProcess = Thread.CurrentThread;
                double angle = 0;
                fps = 60;

                while (model != null)
                {
                    Bitmap image = new Bitmap(ScreenWidth, ScreenHeight);

                    #region FPS
                    if (Lastcounted.AddSeconds(.1) <= DateTime.Now) {
                        fps = FrameCount / (DateTime.Now - Lastcounted).TotalSeconds;
                        WindowTitle = String.Format("GraphicsEngine - FPS: {0}", fps);
                        Lastcounted = DateTime.Now;
                        FrameCount = 0;
                    }
                    #endregion

                    foreach (Triangle triangle in model.triangles)
                    {
                        List<Vector> points = new List<Vector>();
                        foreach (Vector3D vertex in triangle.Vertices)
                        {
                            //angle = 6.28318531; // one rotation
                            double[,] rotated2d = func.Multiply(func.RotateX(angle), new double[,] { { vertex.X }, { -vertex.Y }, { vertex.Z }, { 1 } });
                            rotated2d = func.Multiply(func.RotateY(angle), rotated2d);
                            rotated2d = func.Multiply(func.RotateZ(angle), rotated2d);

                            double[,] projected2d = func.Multiply(projection_matrix, rotated2d);

                            int x = (int)(projected2d[0, 0] * scale + ScreenWidth / 2);
                            int y = (int)(projected2d[1, 0] * scale + ScreenHeight / 2);
                            points.Add(new Vector(x, y));
                        }
                        // options
                        if (RenderTextures) func.FillTriangle(image, points);
                        else func.DrawTriangle(image, points);

                    }
                    canvas.Dispatcher.Invoke(() => canvas.Source = func.BitmapToImageSource(image));
                    angle += 1 / (fps + .01);
                    FrameCount++;
                }
                // at the end clear screen
                canvas.Dispatcher.Invoke(() => canvas.Source = null);
                WindowTitle = "GraphicsEngine";
            }).Start();
        }
    }
}
