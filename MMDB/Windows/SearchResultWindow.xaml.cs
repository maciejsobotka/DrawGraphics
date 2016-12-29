using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for SearchResultWindow.xaml
    /// </summary>
    public partial class SearchResultWindow : Window
    {
        #region Private fields

        private readonly string dbPath;

        #endregion
        #region Ctors

        public SearchResultWindow(List<string> fileNames, string dbPath)
        {
            InitializeComponent();
            this.dbPath = dbPath;
            numberOfFilesLabel.Content = fileNames.Count.ToString();
            foreach (var file in fileNames)
                filesFoundList.Items.Add(file);
        }

        #endregion
        #region Private methods

        private void filesFoundList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (filesFoundList.SelectedItem != null)
            {
                var gpw = new GraphicPreviewWindow(dbPath + filesFoundList.SelectedItem);
                gpw.Show();
            }
        }

        #endregion
    }
}