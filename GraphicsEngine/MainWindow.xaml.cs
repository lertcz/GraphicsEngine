using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static readonly Scene scene = new Scene();
        private static readonly Importing import = new Importing(scene);
        private static readonly Functions func = new Functions(import);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            scene.canvas = RenderImage;
            scene.ScreenHeight = (int)RenderWindow.ActualHeight;
            scene.ScreenWidth = (int)RenderWindow.ActualWidth;

            foreach (Button btn in func.LoadDemos())
            {
                Demos.Children.Add(btn);
            }
        }
        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            import.GetFolder();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            scene.model = null;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Mesh test = new Mesh();
            test.triangles = new List<Triangle>{
                // South
                new Triangle(new Vector3D[]{ new Vector3D(0.0f, 0.0f, 0.0f), new Vector3D(0.0f, 1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f) }),
                new Triangle(new Vector3D[]{ new Vector3D(0.0f, 0.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f), new Vector3D(1.0f, 0.0f, 0.0f) }),
                
                // East
                new Triangle(new Vector3D[]{ new Vector3D(1.0f, 0.0f, 0.0f), new Vector3D(1.0f, 1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 1.0f) }),
                new Triangle(new Vector3D[]{ new Vector3D(1.0f, 0.0f, 0.0f), new Vector3D(1.0f, 1.0f, 1.0f), new Vector3D(1.0f, 0.0f, 1.0f) }),

                // North
                new Triangle(new Vector3D[]{ new Vector3D(1.0f, 0.0f, 1.0f), new Vector3D(1.0f, 1.0f, 1.0f), new Vector3D(0.0f, 1.0f, 1.0f) }),
                new Triangle(new Vector3D[]{ new Vector3D(1.0f, 0.0f, 1.0f), new Vector3D(0.0f, 1.0f, 1.0f), new Vector3D(0.0f, 0.0f, 1.0f) }),

                // West
                new Triangle(new Vector3D[]{ new Vector3D(0.0f, 0.0f, 1.0f), new Vector3D(0.0f, 1.0f, 1.0f), new Vector3D(0.0f, 1.0f, 0.0f) }),
                new Triangle(new Vector3D[]{ new Vector3D(0.0f, 0.0f, 1.0f), new Vector3D(0.0f, 1.0f, 0.0f), new Vector3D(0.0f, 0.0f, 0.0f) }),

                // Top
                new Triangle(new Vector3D[]{ new Vector3D(0.0f, 1.0f, 0.0f), new Vector3D(0.0f, 1.0f, 1.0f), new Vector3D(1.0f, 1.0f, 1.0f) }),
                new Triangle(new Vector3D[]{ new Vector3D(0.0f, 1.0f, 0.0f), new Vector3D(1.0f, 1.0f, 1.0f), new Vector3D(1.0f, 1.0f, 0.0f) }),

                // Bottom
                new Triangle(new Vector3D[]{ new Vector3D(1.0f, 0.0f, 1.0f), new Vector3D(0.0f, 0.0f, 1.0f), new Vector3D(0.0f, 0.0f, 0.0f) }),
                new Triangle(new Vector3D[]{ new Vector3D(1.0f, 0.0f, 1.0f), new Vector3D(0.0f, 0.0f, 0.0f), new Vector3D(1.0f, 0.0f, 0.0f) }),
            };
            scene.model = test;
            scene.Render();
        }
    }
}
