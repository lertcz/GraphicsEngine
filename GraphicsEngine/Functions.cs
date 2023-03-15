using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Documents;

namespace GraphicsEngine
{
    internal class Functions
    {
        private Importing Import { get; set; }
        public Functions(Importing import=null)
        {
            this.Import = import;
        }

        public List<Button> LoadDemos()
        {
            List<Button> buttons = new List<Button>();

            foreach (string dir in Directory.GetDirectories(@"..\..\..\DemoModels"))
            {
                string dirName = Path.GetFileName(dir);
                Button btn = new Button
                {
                    Content = dirName
                };
                btn.Click += (obj, e) =>
                {
                    Import.ProcessDir(dir);
                };
                buttons.Add(btn);
            }
            return buttons;
        }
        public double[,] Multiply(double[,] matrix1, double[,] matrix2)
        {
            // cahing matrix lengths for better performance  
            var matrix1Rows = matrix1.GetLength(0);
            var matrix1Cols = matrix1.GetLength(1);
            var matrix2Rows = matrix2.GetLength(0);
            var matrix2Cols = matrix2.GetLength(1);

            // checking if product is defined  
            if (matrix1Cols != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

            // creating the final product matrix  
            double[,] product = new double[matrix1Rows, matrix2Cols];

            // looping through matrix 1 rows  
            for (int matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
            {
                // for each matrix 1 row, loop through matrix 2 columns  
                for (int matrix2_col = 0; matrix2_col < matrix2Cols; matrix2_col++)
                {
                    // loop through matrix 1 columns to calculate the dot product  
                    for (int matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                    {
                        product[matrix1_row, matrix2_col] +=
                          matrix1[matrix1_row, matrix1_col] *
                          matrix2[matrix1_col, matrix2_col];
                    }
                }
            }

            return product;
        }
        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        public void Bresenham(Bitmap canvas,double X1, double Y1, double X2, double Y2)
        {
            double x1 = Math.Round(X1);
            double y1 = Math.Round(Y1);
            double x2 = Math.Round(X2);
            double y2 = Math.Round(Y2);


            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            double err = dx - dy;

            while (true)
            {
                canvas.SetPixel((int)x1, (int)y1, Color.Yellow);

                if (x1 == x2 && y1 == y2) break;
                double e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        public void InterpolatePixel(Bitmap canvas, Point v1, Point v2, Point v3, Point p)
        {
            double p1_weight = 1.0 / (Math.Sqrt(Math.Pow(v1.x - p.x, 2) + Math.Pow(v1.y - p.y, 2)) + 1);
            double p2_weight = 1.0 / (Math.Sqrt(Math.Pow(v2.x - p.x, 2) + Math.Pow(v2.y - p.y, 2)) + 1);
            double p3_weight = 1.0 / (Math.Sqrt(Math.Pow(v3.x - p.x, 2) + Math.Pow(v3.y - p.y, 2)) + 1);

            int R = (int)((v1.color.R * p1_weight + v2.color.R * p2_weight + v3.color.R * p3_weight) / (p1_weight + p2_weight + p3_weight));
            int G = (int)((v1.color.G * p1_weight + v2.color.G * p2_weight + v3.color.G * p3_weight) / (p1_weight + p2_weight + p3_weight));
            int B = (int)((v1.color.B * p1_weight + v2.color.B * p2_weight + v3.color.B * p3_weight) / (p1_weight + p2_weight + p3_weight));

            canvas.SetPixel((int)p.x, (int)p.y, Color.FromArgb(255, R, G, B));
        }

        private double EdgeFunction(Point a, Point b, Point c)
        {
            return (c.x - a.x) * (b.y - a.y)
                 - (c.y - a.y) * (b.x - a.x);
        }

        public void PixelOverlapTriangle(Bitmap canvas, Point vertex1, Point vertex2, Point vertex3, Point point)
        {
            double w1 = EdgeFunction(vertex2, vertex3, point);
            double w2 = EdgeFunction(vertex3, vertex1, point);
            double w3 = EdgeFunction(vertex1, vertex2, point);

            if (w1 >= 0 && w2 >= 0 && w3 >= 0)
            {
                Point edge1 = vertex3 - vertex2;
                Point edge2 = vertex1 - vertex3;
                Point edge3 = vertex2 - vertex1;

                bool overlaps = true;

                overlaps &= (w1 == 0) ? (edge1.y == 0 && edge1.x > 0 || edge1.y > 0) : (w1 > 0);
                overlaps &= (w2 == 0) ? (edge2.y == 0 && edge2.x > 0 || edge2.y > 0) : (w2 > 0);
                overlaps &= (w3 == 0) ? (edge3.y == 0 && edge3.x > 0 || edge3.y > 0) : (w3 > 0); // w3 == 0 -> w2 == 0

                if (overlaps)
                {
                    InterpolatePixel(canvas, vertex1, vertex2, vertex3, point);
                }
            }
        }

        public void DrawTriangle(Bitmap canvas, List<Vector> points)
        {
            Bresenham(canvas, points[0].X, points[0].Y, points[1].X, points[1].Y);
            Bresenham(canvas, points[1].X, points[1].Y, points[2].X, points[2].Y);
            Bresenham(canvas, points[2].X, points[2].Y, points[0].X, points[0].Y);
        }
        
        public void FillTriangle(Bitmap canvas, List<Vector> points)
        {
            // https://www.youtube.com/watch?v=t7Ztio8cwqM
            //https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage.html

            double[] X = new double[3] { points[0].X, points[1].X, points[2].X };
            double[] Y = new double[3] { points[0].Y, points[1].Y, points[2].Y };
            int minX = (int)X.Min();
            int maxX = (int)X.Max();
            int minY = (int)Y.Min();
            int maxY = (int)Y.Max();
            
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Point v1 = new Point(points[0].X, points[0].Y, r: (byte)255);
                    Point v2 = new Point(points[1].X, points[1].Y, g: (byte)255);
                    Point v3 = new Point(points[2].X, points[2].Y, b: (byte)255); 
                    PixelOverlapTriangle(canvas, v1, v2, v3, new Point(x, y));
                }
            }
        }

        public double[,] RotateX(double angle)
        {
            return new double[4, 4]
            {
                { 1, 0,               0,                0 },
                { 0, Math.Cos(angle), -Math.Sin(angle), 0 },
                { 0, Math.Sin(angle), Math.Cos(angle),  0 },
                { 0, 0,               0,                1 },
            };
        }
        public double[,] RotateY(double angle)
        {
            return new double[4, 4]
            {
                { Math.Cos(angle),  0, -Math.Sin(angle), 0 },
                { 0,                1, 0,               0 },
                { Math.Sin(angle),  0, Math.Cos(angle), 0 },
                { 0,                0, 0,               1 },
            };
        }
        public double[,] RotateZ(double angle)
        {
            return new double[4, 4]
            {
                { Math.Cos(angle), -Math.Sin(angle), 0, 0 },
                { Math.Sin(angle), Math.Cos(angle),  0, 0 },
                { 0,               0,                1, 0 },
                { 0,               0,                0, 1 },
            };
        }
    }
}
