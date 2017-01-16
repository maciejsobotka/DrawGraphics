using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MMDB.Utils;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        #region Private fields

        private readonly string[] m_Attributes = {"None", "Fill", "Stroke"};
        private readonly string[] m_Colors = {"White", "Black", "Gray", "Red", "Green", "Blue"};
        private string m_DbPath;
        private SearchResultWindow m_SearchResultWindow;
        private readonly string[] m_Shapes = {"Line", "Ellipse", "Rectangle", "Triangle"};
        private readonly string[] m_Symbols = {"==", "!=", ">", "<", ">=", "<="};

        #endregion
        #region Ctors

        public SearchWindow()
        {
            InitializeComponent();
            m_DbPath = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\examples\\";
            //m_DbPath = "D:\\Sobot\\Programy\\Programy C#\\MMDB\\MMDB\\examples\\";
            folderPathTextBox.Text = m_DbPath;
            foreach (string symbol in m_Symbols)
                comparisonComboBox.Items.Add(symbol);
            comparisonComboBox.SelectedIndex = 0;
            foreach (string shape in m_Shapes)
                shapeComboBox.Items.Add(shape);
            foreach (string attr in m_Attributes)
                attributeComboBox.Items.Add(attr);
            attributeComboBox.SelectedIndex = 0;
            foreach (string color in m_Colors)
                attributeValueComboBox.Items.Add(color);
            attributeValueComboBox.SelectedIndex = 0;
            shapeComboBox.SelectedIndex = 0;
        }

        #endregion
        #region Private methods

        private void changeFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialogSource = new FolderBrowserDialog();
            folderBrowserDialogSource.ShowDialog();
            m_DbPath = folderBrowserDialogSource.SelectedPath + "\\";
            folderPathTextBox.Text = m_DbPath;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var dirInfo = new DirectoryInfo(m_DbPath);
            FileInfo[] info = dirInfo.GetFiles("*.xaml");
            var files = new string[info.Length];
            for (var i = 0; i < files.Length; ++i)
                files[i] = info[i].Name;
            var filesFound = new List<string>();
            string shape;
            if (shapeComboBox.SelectedItem.ToString() == "Triangle")
            {
                shape = "Polygon";
            }
            else
            {
                shape = shapeComboBox.SelectedItem.ToString();
            }
            var numberOfShapes = 0;
            if (int.TryParse(numberOfShapesTextBox.Text, out numberOfShapes))
            {
                if (attributeComboBox.SelectedIndex == 0)
                {
                    foreach (string file in files)
                        if (SqlMmParser.SearchResult(file, shape, numberOfShapes, comparisonComboBox.SelectedItem.ToString()))
                        {
                            filesFound.Add(file);
                        }
                }
                else
                {
                    foreach (string file in files)
                    {
                        string attributeName = attributeComboBox.SelectedItem.ToString();
                        string attributeValue = attributeValueComboBox.SelectedItem.ToString();
                        if (SqlMmParser.SearchResult(file, shape, numberOfShapes, comparisonComboBox.SelectedItem.ToString(), attributeName, attributeValue))
                        {
                            filesFound.Add(file);
                        }
                    }
                }
                m_SearchResultWindow = new SearchResultWindow(filesFound, m_DbPath);
                m_SearchResultWindow.Show();
            }
        }

        #endregion
    }
}