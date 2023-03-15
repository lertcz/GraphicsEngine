﻿using System;
using System.Collections.Generic;
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
        private static readonly Scene scene = new Scene();
        private static readonly Importing import = new Importing(scene);
        private static readonly Functions func = new Functions(import);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = scene;
        }

        private void InitScene()
        {
            scene.canvas = RenderImage;

            scene.Textures = TexturesCheckBox;
            scene.LowRes = Potato;

            scene.FullScreenHeight = (int)RenderWindow.ActualHeight;
            scene.FullScreenWidth = (int)RenderWindow.ActualWidth;
            //scene.InitializeProjectionMatrix(90, 0.1, 1000);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitScene();

            foreach (Button btn in func.LoadDemos())
            {
                Demos.Children.Add(btn);
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            scene.currentRenderProcess?.Abort();
        }
        
        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            import.GetFolder();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            scene.model = null;
        }
    }
}
