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
    /// Interaction logic for SearchResultWindow.xaml
    /// </summary>
    public partial class SearchResultWindow : Window
    {
        public SearchResultWindow(List<string> fileNames)
        {
            InitializeComponent();
            numberOfFilesLabel.Content = fileNames.Count.ToString();
            foreach (var file in fileNames)
                filesFoundList.Items.Add(file);
        }

        private void filesFoundList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (filesFoundList.SelectedItem != null)
            {
                GraphicPreviewWindow gpw = new GraphicPreviewWindow(filesFoundList.SelectedItem.ToString());
                gpw.Show();
            }
        }
    }
}
