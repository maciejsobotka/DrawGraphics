using System;
using System.Windows;
using System.Windows.Controls;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        #region Private fields

        private bool iSDataSet;

        #endregion
        #region Ctors

        public EditWindow()
        {
            InitializeComponent();
        }

        #endregion
        #region Public methods

        public void SetShapeData(Point p1, Point p2)
        {
            x1.Text = p1.X.ToString();
            y1.Text = p1.Y.ToString();
            x2.Text = p2.X.ToString();
            y2.Text = p2.Y.ToString();
            iSDataSet = true;
        }

        #endregion
        #region Private methods

        private void point_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (iSDataSet)
                MyEvent(this, EventArgs.Empty);
        }

        #endregion
        public event EventHandler MyEvent;
    }
}