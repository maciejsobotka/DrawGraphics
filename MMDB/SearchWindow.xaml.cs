using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using System.Xml;

namespace MMDB
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private SearchResultWindow searchResultWindow;
        private string[] symbols = { "==", "!=", ">", "<", ">=", "<=" };
        private string[] shapes = { "Line", "Ellipse", "Rectangle", "Triangle" };
        private string[] attributes = { "None", "Fill", "Stroke" };
        private string[] colors = { "White", "Black", "Gray", "Red", "Green", "Blue" };
        private Dictionary<string, string> colorDict;
        private string dbPath;

        public SearchWindow()
        {
            InitializeComponent();
            InitializeColorDictionary();
            dbPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            folderPathTextBox.Text = dbPath;
            foreach (var symbol in symbols)
                comparisonComboBox.Items.Add(symbol);
            comparisonComboBox.SelectedIndex = 0;
            foreach (var shape in shapes)
                shapeComboBox.Items.Add(shape);
            foreach (var attr in attributes)
                attributeComboBox.Items.Add(attr);
            attributeComboBox.SelectedIndex = 0;
            foreach (var color in colors)
                attributeValueComboBox.Items.Add(color);
            attributeValueComboBox.SelectedIndex = 0;
            shapeComboBox.SelectedIndex = 0;
        }

        private void InitializeColorDictionary()
        {
            colorDict = new Dictionary<string, string>();
            colorDict.Add("White", "#FFFFFFFF");
            colorDict.Add("Black", "#FF000000");
            colorDict.Add("Gray", "#FF808080");
            colorDict.Add("Red", "#FFFF0000");
            colorDict.Add("Green", "#FF008000");
            colorDict.Add("Blue", "#FF0000FF");
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dbPath);
            FileInfo[] info = dirInfo.GetFiles("*.xaml");
            string[] files = new string[info.Length];
            for (int i = 0; i < files.Length; ++i)
                files[i] = info[i].Name;
            List<string> filesFound = new List<string>();
            string shape;
            if (shapeComboBox.SelectedItem.ToString() == "Triangle")
                shape = "Polygon";
            else
                shape = shapeComboBox.SelectedItem.ToString();
            int numberOfShapes = 0;
            if(Int32.TryParse(numberOfShapesTextBox.Text, out numberOfShapes)){
                if(attributeComboBox.SelectedIndex == 0)
                    foreach(var file in files)
                    {
                        if (searchResult(file, shape, numberOfShapes, comparisonComboBox.SelectedItem.ToString()))
                            filesFound.Add(file);
                    }
                else
                    foreach (var file in files)
                    {
                        string attributeName = attributeComboBox.SelectedItem.ToString();
                        string attributeValue = attributeValueComboBox.SelectedItem.ToString();
                        if (searchResult(file, shape, numberOfShapes, comparisonComboBox.SelectedItem.ToString(), attributeName, colorDict[attributeValue]))
                            filesFound.Add(file);
                    }
                searchResultWindow = new SearchResultWindow(filesFound, dbPath);
                searchResultWindow.Show();
            }
        }

        private bool searchResult(string fileName, string shapeName, int shapeCount, string comparison, string attrName, string attrVal)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            XmlNodeList nodeList;
            nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "[@" + attrName + "='" + attrVal + "']", nsMgr);
            switch (comparison)
            {
                case "==":
                    if (nodeList.Count == shapeCount) return true;
                    break;
                case "!=":
                    if (nodeList.Count != shapeCount) return true;
                    break;
                case ">":
                    if (nodeList.Count > shapeCount) return true;
                    break;
                case "<":
                    if (nodeList.Count < shapeCount) return true;
                    break;
                case ">=":
                    if (nodeList.Count >= shapeCount) return true;
                    break;
                case "<=":
                    if (nodeList.Count <= shapeCount) return true;
                    break;
            }
            return false;
        }

        private bool searchResult(string fileName, string shapeName, int shapeCount, string comparison)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            XmlNodeList nodeList;
            nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "", nsMgr);
            switch (comparison)
            {
                case "==": 
                    if (nodeList.Count == shapeCount) return true;
                    break;
                case "!=":
                    if (nodeList.Count != shapeCount) return true;
                    break;
                case ">":
                    if (nodeList.Count > shapeCount) return true;
                    break;
                case "<":
                    if (nodeList.Count < shapeCount) return true;
                    break;
                case ">=":
                    if (nodeList.Count >= shapeCount) return true;
                    break;
                case "<=":
                    if (nodeList.Count <= shapeCount) return true;
                    break;
            }
            return false;


        }

        private void changeFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
