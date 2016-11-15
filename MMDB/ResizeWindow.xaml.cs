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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public event EventHandler MyEvent;
        private bool iSDataSet = false;
        public EditWindow()
        {
            InitializeComponent();
            
        }

        public void SetShapeData(Point p1, Point p2)
        {
            x1.Text = p1.X.ToString();
            y1.Text = p1.Y.ToString();
            x2.Text = p2.X.ToString();
            y2.Text = p2.Y.ToString();
            iSDataSet = true;
        }

        private void point_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(iSDataSet)
                this.MyEvent(this, EventArgs.Empty);
        }
    }
}
