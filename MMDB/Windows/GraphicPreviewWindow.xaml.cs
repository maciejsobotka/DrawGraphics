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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMDB.Windows
{
    /// <summary>
    /// Interaction logic for GraphicPreviewWindow.xaml
    /// </summary>
    public partial class GraphicPreviewWindow : Window
    {
        public GraphicPreviewWindow(string fileName)
        {
            InitializeComponent();
            FileStream file = new FileStream(fileName, FileMode.Open);
            Canvas c = (Canvas)XamlReader.Load(file);
            while (c.Children.Count > 0)
            {
                Shape s = (Shape)c.Children[0];
                c.Children.RemoveAt(0);
                canvas.Children.Add(s);
            }
        }
    }
}
