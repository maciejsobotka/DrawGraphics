using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string imgPath = "";
        private bool imageLoaded = false;
        private bool imageNew = false;
        private Shapes shape = Shapes.None;
        private Point p;
        private Point p1;
        private Point p2;
        private VectorDraw vd;
        private List<Shape> shapes;
        private bool editMode = false;
        private Brush color;
        
        public MainWindow()
        {
            InitializeComponent();
            vd = new VectorDraw();
            shapes = new List<Shape>();
            color = Brushes.LightSteelBlue;
            p.X = 0.0;
            p.Y = 0.0;
        }

        //=====================================================================
        // Menu
        private void NewFile_Click(object sender, EventArgs e)
        {
            if (imageLoaded || imageNew)
            {
                canvas.Source = null;
                ClearObjects();
                textBoxSource.Text = "New File";
            }
            canvasGrid.Background = Brushes.White;
            imageNew = true;
            imageLoaded = false;
            textBoxSource.Text = "";
            textBoxSource.Foreground = Brushes.Black;
            textBoxSource.FontStyle = FontStyles.Normal;
            textBoxSource.Text = "New File";
            EnableButtons();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG |*.JPG;*.JPEG";
            dlg.Multiselect = false;  // default
            dlg.ShowDialog();
            imgPath = dlg.FileName;
            
            textBoxSource.Text = "";
            textBoxSource.Foreground = Brushes.Black;
            textBoxSource.FontStyle = FontStyles.Normal;
            textBoxSource.Text = dlg.SafeFileName;

            if (imageNew || imageLoaded)
            {
                canvasGrid.Background = null;
                ClearObjects();
                canvasGrid.Children.Add(canvas);
            }
            ShowImage();
            imageLoaded = true;
            imageNew = false;
            EnableButtons();
        }

        private void newFile_MouseEnter(object sender, MouseEventArgs e)
        {
            newFile.Foreground = Brushes.Black;
        }

        private void newFile_MouseLeave(object sender, MouseEventArgs e)
        {
            newFile.Foreground = Brushes.White;
        }

        private void openFile_MouseEnter(object sender, MouseEventArgs e)
        {
            openFile.Foreground = Brushes.Black;
        }

        private void openFile_MouseLeave(object sender, MouseEventArgs e)
        {
            openFile.Foreground = Brushes.White;
        }
        //=====================================================================
        // Load image
        private void ShowImage()
        {
            if (File.Exists(imgPath))
            {
                SetImage(imgPath);
            }
            else
                MessageBox.Show("Incorrect file path!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void SetImage(string imgPath)
        {
            BitmapImage img = new BitmapImage(new Uri(imgPath));
            SetCanvasSize(img.PixelWidth, img.PixelHeight);
            canvas.Source = img;
        }

        public void SetImage(int width, int height, double dpiX, double dpiY, PixelFormat pf, byte[] pixels, int stride)
        {
            BitmapSource img = BitmapSource.Create(
                width,
                height,
                dpiX,
                dpiY,
                pf,
                /* palette: */ null,
                pixels,
                stride);
            SetCanvasSize(img.PixelWidth, img.PixelHeight);
            canvas.Source = img;
        }

        private void SetCanvasSize(int pixelWidth, int pixelHeight)
        {
            canvas.Height = 600;
            canvas.Width = 600;
            if (pixelHeight > pixelWidth)
                if (canvas.Height > pixelHeight)
                {
                    canvas.Height = pixelHeight;
                    canvas.Width = pixelWidth;
                }
                else
                    canvas.Width = (Height / pixelHeight) * pixelWidth;
            if (pixelHeight < pixelWidth)
                if (canvas.Width > pixelWidth)
                {
                    canvas.Height = pixelHeight;
                    canvas.Width = pixelWidth;
                }
                else
                    canvas.Height = (Width / pixelWidth) * pixelHeight;
        }
        private void EnableButtons()
        {
            //changePixelButton.IsEnabled = true;
            //avgColorButton.IsEnabled = true;
            lineButton.IsEnabled = true;
            ellipseButton.IsEnabled = true;
            rectangleButton.IsEnabled = true;
            triangleButton.IsEnabled = true;
            editButton.IsEnabled = true;
        }

        private void ClearObjects()
        {
            canvasGrid.Children.Clear();
            shapes.Clear();
            listOfObjects.Items.Clear();
        }

        //=====================================================================
        // Options management
        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            shape = Shapes.Line;
            p1 = p;
            p2 = p;
        }

        private void ellipseButton_Click(object sender, RoutedEventArgs e)
        {
            shape = Shapes.Ellipse;
            p1 = p;
            p2 = p;
        }

        private void rectangleButton_Click(object sender, RoutedEventArgs e)
        {
            shape = Shapes.Rectangle;
            p1 = p;
            p2 = p;
        }

        private void triangleButton_Click(object sender, RoutedEventArgs e)
        {
            shape = Shapes.Triangle;
            p1 = p;
            p2 = p;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (editMode) editMode = false;
            else editMode = true;
        }

        //=====================================================================
        // Vector graphics
        private void canvasGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!editMode)
            {
                if (shape == Shapes.Line)
                    if (p1 == p) p1 = Mouse.GetPosition(canvasGrid);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvasGrid);
                        Line line = vd.CreateLine(p1, p2, 2, Brushes.Black);
                        line.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvasGrid.Children.Add(line);
                        shapes.Add(line);
                        listOfObjects.Items.Add(vd.ParametersToString(line));
                        p1 = p;
                        p2 = p;
                    }
                if (shape == Shapes.Ellipse)
                    if (p1 == p) p1 = Mouse.GetPosition(canvasGrid);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvasGrid);
                        Ellipse ellipse = vd.CreateEllipse(p1, p2, 2, Brushes.Black, Brushes.LightSteelBlue);
                        ellipse.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvasGrid.Children.Add(ellipse);
                        shapes.Add(ellipse);
                        listOfObjects.Items.Add(vd.ParametersToString(ellipse));
                        p1 = p;
                        p2 = p;
                    }
                if (shape == Shapes.Rectangle)
                    if (p1 == p) p1 = Mouse.GetPosition(canvasGrid);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvasGrid);
                        Rectangle rectangle = vd.CreateRectangle(p1, p2, 2, Brushes.Black, Brushes.LightSteelBlue);
                        rectangle.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvasGrid.Children.Add(rectangle);
                        shapes.Add(rectangle);
                        listOfObjects.Items.Add(vd.ParametersToString(rectangle));
                        p1 = p;
                        p2 = p;
                    }
                if (shape == Shapes.Triangle)
                    if (p1 == p) p1 = Mouse.GetPosition(canvasGrid);
                    else if (p2 == p)
                    {
                        p2 = Mouse.GetPosition(canvasGrid);
                        Polygon triangle = vd.CreateTriangle(p1, p2, 2, Brushes.Black, Brushes.LightSteelBlue);
                        triangle.MouseDown += new MouseButtonEventHandler(shape_MouseDown);
                        canvasGrid.Children.Add(triangle);
                        shapes.Add(triangle);
                        listOfObjects.Items.Add(vd.ParametersToString(triangle));
                        p1 = p;
                        p2 = p;
                    }
            }
        }

        private void shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (editMode) {
                int index = shapes.IndexOf((Shape)sender);
                shapes[index].Fill = color;
            }
        }

        private void redB_Click(object sender, RoutedEventArgs e)
        {
            color = Brushes.Red;
        }

        private void greenB_Click(object sender, RoutedEventArgs e)
        {
            color = Brushes.Green;
        }

        private void blueB_Click(object sender, RoutedEventArgs e)
        {
            color = Brushes.Blue;
        }

        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            color = Brushes.Wheat;
        }
    }
}
