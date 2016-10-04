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
        private ImageWindow imgWindow;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void buttonImgSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG |*.JPG;*.JPEG";
            dlg.Multiselect = false;  // default
            dlg.ShowDialog();
            imgPath = dlg.FileName;
            textBoxSource.Text = "";
            textBoxSource.Foreground = Brushes.Black;
            textBoxSource.FontStyle = FontStyles.Normal;
            textBoxSource.Text = imgPath;
        }

        private void buttonShowImg_Click(object sender, EventArgs e)
        {
            if (File.Exists(imgPath))
            {
                imgWindow = new ImageWindow();
                imgWindow.SetImage(imgPath);
                imgWindow.Show();
                changePixelButton.IsEnabled = true;
                avgColorButton.IsEnabled = true;
            }
            else
                MessageBox.Show("Incorrect file path!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void changePixelButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource img = imgWindow.canvas.Source as BitmapSource;
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
          
            imgWindow.SetImage(img.PixelWidth, img.PixelHeight, img.DpiX, img.DpiY, PixelFormats.Bgr32, pixels, stride);
        }

        private void avgColorButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource img = imgWindow.canvas.Source as BitmapSource;
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
    }
}
