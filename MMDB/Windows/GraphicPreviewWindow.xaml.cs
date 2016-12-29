using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for GraphicPreviewWindow.xaml
    /// </summary>
    public partial class GraphicPreviewWindow : Window
    {
        #region Ctors

        public GraphicPreviewWindow(string fileName)
        {
            InitializeComponent();
            var file = new FileStream(fileName, FileMode.Open);
            var c = (Canvas) XamlReader.Load(file);
            while (c.Children.Count > 0)
            {
                var s = (Shape) c.Children[0];
                c.Children.RemoveAt(0);
                canvas.Children.Add(s);
            }
        }

        #endregion
    }
}