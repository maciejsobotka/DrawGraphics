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
using System.Xml;

namespace MMDB
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string[] files = { "graphic.xaml", "graphic2.xaml" };
            foreach(var file in files)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(file);
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
                nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                XmlNodeList nodeList;
                nodeList = xml.SelectNodes("//x:Canvas/x:Ellipse[@Fill='#FF0000FF']",nsMgr);
                if (nodeList.Count > 0)
                    foundFiles.Items.Add(file);
            }
        }
    }
}
