using System;
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
using System.Windows.Shapes;

namespace MMDB
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        public ImageWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
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
    }
}
