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
        private bool drawLine = false;
        private bool drawCircle = false;
        private Point p;
        private Point p1;
        private Point p2;
        private VectorDraw vd;
        private List<Shape> shapes;
        
        public MainWindow()
        {
            InitializeComponent();
            vd = new VectorDraw();
            shapes = new List<Shape>();
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
            circleButton.IsEnabled = true;
        }

        private void ClearObjects()
        {
            canvasGrid.Children.Clear();
            shapes.Clear();
            listOfObjects.Items.Clear();
        }

        //=====================================================================
        // Old stuff not for Vector graphics
        private void changePixelButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource img = canvas.Source as BitmapSource;
            int stride = img.PixelWidth * 4;                // 4*8
            int size = img.PixelHeight * stride;
            byte[] pixels = new byte[size];
            img.CopyPixels(pixels, stride, 0);
            int index = GetPixelIndex(10,10,stride);
            int index2 = GetPixelIndex(35, 50, stride);

            pixels[index] = 0;
            pixels[index+1] = 0;
            pixels[index+2] = 255;

            pixels[index2] = 0;
            pixels[index2 + 1] = 255;
            pixels[index2 + 2] = 255;
          
            SetImage(img.PixelWidth, img.PixelHeight, img.DpiX, img.DpiY, PixelFormats.Bgr32, pixels, stride);
        }

        private void avgColorButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource img = canvas.Source as BitmapSource;
            int stride = img.PixelWidth * 4;                // 4*8
            int size = img.PixelHeight * stride;
            byte[] pixels = new byte[size];
            img.CopyPixels(pixels, stride, 0);
            int red=0, green=0, blue=0;
            for(int i=0; i < img.PixelWidth; ++i)
                for(int j=0; j< img.PixelHeight; ++j)
                {
                    var index = GetPixelIndex(i, j, stride);
                    red += pixels[index];
                    green += pixels[index + 1];
                    blue += pixels[index + 2];
                }
            red = red / (img.PixelWidth * img.PixelHeight);
            green = green / (img.PixelWidth * img.PixelHeight);
            blue = blue / (img.PixelWidth * img.PixelHeight);
            colorRectangle.Fill = new SolidColorBrush(Color.FromRgb((byte) red, (byte) green,(byte)  blue));
        }
        private int GetPixelIndex(int x, int y, int stride)
        {
            return y * stride + 4 * x;
        }

        //=====================================================================
        // Vector graphics
        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            drawLine = true;
            drawCircle = false;
            p1 = p;
            p2 = p;
        }

        private void circleButton_Click(object sender, RoutedEventArgs e)
        {
            drawCircle = true;
            drawLine = false;
            p1 = p;
            p2 = p;
        }

        private void canvasGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(drawLine)
                if (p1 == p) p1 = Mouse.GetPosition(canvasGrid);
                else if (p2 == p)
                {
                    p2 = Mouse.GetPosition(canvasGrid);
                    Line line = vd.CreateLine(p1, p2, 2, Brushes.LightSteelBlue);
                    canvasGrid.Children.Add(line);
                    shapes.Add(line);
                    listOfObjects.Items.Add(vd.ParametersToString(line));
                    p1 = p;
                    p2 = p;        
                }
            if(drawCircle){
                p1 = Mouse.GetPosition(canvasGrid);
                Ellipse circle = vd.CreateEllipse(p1, 50, 50, 2, Brushes.LightSteelBlue, Brushes.LightSteelBlue, canvasGrid.ActualWidth, canvasGrid.ActualHeight);
                canvasGrid.Children.Add(circle);
                listOfObjects.Items.Add(vd.ParametersToString(circle));
                  
                }
        }

    }
}
