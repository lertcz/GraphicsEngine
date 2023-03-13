using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;

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
                //drawPixel(x1, y1, color);
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

        public double[,] RotateX(double angle)
        {
            return new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, Math.Cos(angle), -Math.Sin(angle) },
                { 0, Math.Sin(angle), Math.Cos(angle) }
            };
        }
        public double[,] RotateY(double angle)
        {
            return new double[3, 3]
            {
                { Math.Cos(angle), 0, Math.Sin(angle) },
                { 0, 1, 0 },
                { -Math.Sin(angle), 0, Math.Cos(angle) }
            };
        }
        public double[,] RotateZ(double angle)
        {
            return new double[3, 3]
            {
                { Math.Cos(angle), -Math.Sin(angle), 0 },
                { Math.Sin(angle), Math.Cos(angle), 0 },
                { 1, 0, 0 }
            };
        }
    }
}
