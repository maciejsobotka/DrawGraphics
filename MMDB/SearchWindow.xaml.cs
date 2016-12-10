using System;
using System.Collections.Generic;
using System.Globalization;
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
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string[] files = { "graphic.xaml", "graphic2.xaml" };
            List<string> filesFound = new List<string>();
            foreach(var file in files)
            {
                int x = int.Parse("FF0000FF", NumberStyles.HexNumber);
                if (searchResult(file, "Ellipse", 1, "Fill", x))
                    filesFound.Add(file);
            }
            searchResultWindow = new SearchResultWindow(filesFound);
            searchResultWindow.Show();
        }

        private bool searchResult(string fileName, string shapeName, int shapeCount, string attrName, int attrVal)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            XmlNodeList nodeList;
            nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "[@" + attrName + "='#" + attrVal.ToString("X8")+ "']", nsMgr);
            if (nodeList.Count >= shapeCount) return true;

            return false;
        }

        private bool searchResult(string fileName, string shapeName, int shapeCount)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            XmlNodeList nodeList;
            nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "", nsMgr);
            if (nodeList.Count >= shapeCount) return true;

            return false;


        }
    }
}
