using MMDB.Utils;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for TableFormWindow.xaml
    /// </summary>
    public partial class TableFormWindow : Window
    {
        private DataTable dt;
        private TableToFromXML ttfXML;
        private TableToFromDB ttfDB;
        public TableFormWindow()
        {
            InitializeComponent();
            ttfXML = new TableToFromXML();
            ttfDB = new TableToFromDB();
            dt = InitializeDataGrid(tableGrid, "dataTable");
        }

        private DataTable InitializeDataGrid(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = ttfXML.GetDataFromXML(dataGrid, tableName);
            if (dataTable == null)
            {
                dataTable = new DataTable("dataTable");
                dataTable.Columns.Add("ID", typeof(int));
                dataTable.Columns.Add("Imię", typeof(string));
                dataTable.Columns.Add("Nazwisko", typeof(string));
                dataTable.Columns.Add("Nr ewidencji", typeof(int));
                dataTable.Columns.Add("Data dodania", typeof(DateTime));
                dataTable.Columns.Add("Grafika", typeof(string));
                dataTable.Columns.Add("Ścieżka", typeof(string));
                dataTable = GenerateRows(dataTable);
                dataGrid.DataContext = dataTable.DefaultView;
                dataGrid.ColumnWidth = 80;
            }

            return dataTable;
        }

        private DataTable GenerateRows(DataTable dataTable)
        {
            dataTable.Rows.Add(1, "Jan", "Kowalski", 124436, DateTime.Now, "graphic5");
            dataTable.Rows.Add(2, "Izabela", "Skorpion",114515, DateTime.Now, "graphic");
            dataTable.Rows.Add(3, "Grzegorz", "Cebula", 131234, DateTime.Now, "graphic2");
            dataTable.Rows.Add(4, "Joanna", "Palec", 156789, DateTime.Now ,"graphic4");
            dataTable.Rows.Add(5, "Ewa", "Kot", 185367, DateTime.Now, "graphic3");

            return dataTable;
        }

        private void buttonToXML_Click(object sender, RoutedEventArgs e)
        {
            ttfXML.SaveDataToXML(dt, "dataTable");
        }

        private void buttonFromXML_Click(object sender, RoutedEventArgs e)
        {
            dt = ttfXML.GetDataFromXML(tableGrid, "dataTable");
        }

        private void buttonToDB_Click(object sender, RoutedEventArgs e)
        {
            ttfDB.SaveDataToDB(dt, "dataTable");
        }

        private void buttonFromDB_Click(object sender, RoutedEventArgs e)
        {
            dt = ttfDB.GetDataFromDB(tableGrid, "dataTable");
}
    }
}
