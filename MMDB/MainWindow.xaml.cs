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
        private Operations operation = Operations.None;
        private Point p;
        private Point p1;
        private Point p2;
        private VectorDraw vd;
        private List<Shape> shapes;
        private Brush color;
        
        public MainWindow()
        {
            InitializeComponent();
            vd = new VectorDraw();
            shapes = new List<Shape>();
            color = Brushes.White;
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

        private void menuOption_MouseEnter(object sender, MouseEventArgs e)
        {
            newFile.Foreground = Brushes.Black;
        }

        private void menuOption_MouseLeave(object sender, MouseEventArgs e)
        {
            newFile.Foreground = Brushes.White;
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
            lineButton.IsEnabled = true;
            ellipseButton.IsEnabled = true;
            rectangleButton.IsEnabled = true;
            triangleButton.IsEnabled = true;
            paintButton.IsEnabled = true;
        }

        private void ClearObjects()
        {
            canvasGrid.Children.Clear();
            shapes.Clear();
            listOfObjects.Items.Clear();
        }

        //=====================================================================
        // Options management
        private void shapeButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;

            switch (senderButton.Name)
            {
                case "lineButton":
                    shape = Shapes.Line;
                    break;
                case "ellipseButton":
                    shape = Shapes.Ellipse;
                    break;
                case "rectangleButton":
                    shape = Shapes.Rectangle;
                    break;
                case "triangleButton":
                    shape = Shapes.Triangle;
                    break;
            }
            operation = Operations.None;
            p1 = p;
            p2 = p;
        }

        private void paintButton_Click(object sender, RoutedEventArgs e)
        {
            operation = Operations.Paint;
        }

        //=====================================================================
        // Vector graphics
        private void canvasGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (operation == Operations.None)
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
                        Ellipse ellipse = vd.CreateEllipse(p1, p2, 2, Brushes.Black, color);
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
                        Rectangle rectangle = vd.CreateRectangle(p1, p2, 2, Brushes.Black, color);
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
                        Polygon triangle = vd.CreateTriangle(p1, p2, 2, Brushes.Black, color);
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
            if (operation == Operations.Paint) {
                int index = shapes.IndexOf((Shape)sender);
                shapes[index].Fill = color;
            }
        }

        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;

            switch (senderButton.Name)
            {
                case "whiteButton":
                    color = Brushes.White;
                    break;
                case "grayButton":
                    color = Brushes.Gray;
                    break;
                case "blackButton":
                    color = Brushes.Black;
                    break;
                case "redButton":
                    color = Brushes.Red;
                    break;
                case "greenButton":
                    color = Brushes.Green;
                    break;
                case "blueButton":
                    color = Brushes.Blue;
                    break;
            }
        }
    }
}
