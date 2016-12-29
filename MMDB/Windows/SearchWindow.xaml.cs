using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        #region Private fields

        private readonly string[] m_Attributes = {"None", "Fill", "Stroke"};
        private Dictionary<string, string> m_ColorDict;
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
            InitializeColorDictionary();
            m_DbPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\..\\examples\\";
            //m_DbPath = "D:\\Sobot\\Programy\\Programy C#\\MMDB\\MMDB\\examples\\";
            folderPathTextBox.Text = m_DbPath;
            foreach (var symbol in m_Symbols)
                comparisonComboBox.Items.Add(symbol);
            comparisonComboBox.SelectedIndex = 0;
            foreach (var shape in m_Shapes)
                shapeComboBox.Items.Add(shape);
            foreach (var attr in m_Attributes)
                attributeComboBox.Items.Add(attr);
            attributeComboBox.SelectedIndex = 0;
            foreach (var color in m_Colors)
                attributeValueComboBox.Items.Add(color);
            attributeValueComboBox.SelectedIndex = 0;
            shapeComboBox.SelectedIndex = 0;
        }

        #endregion
        #region Private methods

        private static bool SearchResult(string fileName, string shapeName, int shapeCount, string comparison, string attrName, string attrVal)
        {
            var xml = new XmlDocument();
            xml.Load(fileName);
            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            var nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "[@" + attrName + "='" + attrVal + "']", nsMgr);
            if (nodeList != null)
            {
                switch (comparison)
                {
                    case "==":
                        if (nodeList.Count == shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "!=":
                        if (nodeList.Count != shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">":
                        if (nodeList.Count > shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<":
                        if (nodeList.Count < shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">=":
                        if (nodeList.Count >= shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<=":
                        if (nodeList.Count <= shapeCount)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        private static bool SearchResult(string fileName, string shapeName, int shapeCount, string comparison)
        {
            var xml = new XmlDocument();
            xml.Load(fileName);
            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            var nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "", nsMgr);
            if (nodeList != null)
            {
                switch (comparison)
                {
                    case "==":
                        if (nodeList.Count == shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "!=":
                        if (nodeList.Count != shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">":
                        if (nodeList.Count > shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<":
                        if (nodeList.Count < shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">=":
                        if (nodeList.Count >= shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<=":
                        if (nodeList.Count <= shapeCount)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        private void changeFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialogSource = new FolderBrowserDialog();
            folderBrowserDialogSource.ShowDialog();
            m_DbPath = folderBrowserDialogSource.SelectedPath + "\\";
            folderPathTextBox.Text = m_DbPath;
        }

        private void InitializeColorDictionary()
        {
            m_ColorDict = new Dictionary<string, string>
            {
                {"White", "#FFFFFFFF"},
                {"Black", "#FF000000"},
                {"Gray", "#FF808080"},
                {"Red", "#FFFF0000"},
                {"Green", "#FF008000"},
                {"Blue", "#FF0000FF"}
            };
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var dirInfo = new DirectoryInfo(m_DbPath);
            var info = dirInfo.GetFiles("*.xaml");
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
                    foreach (var file in files)
                        if (SearchResult(file, shape, numberOfShapes, comparisonComboBox.SelectedItem.ToString()))
                        {
                            filesFound.Add(file);
                        }
                }
                else
                {
                    foreach (var file in files)
                    {
                        var attributeName = attributeComboBox.SelectedItem.ToString();
                        var attributeValue = attributeValueComboBox.SelectedItem.ToString();
                        if (SearchResult(file, shape, numberOfShapes, comparisonComboBox.SelectedItem.ToString(), attributeName, m_ColorDict[attributeValue]))
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