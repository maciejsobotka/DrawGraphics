using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MMDB.Utils;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for TableFormWindow.xaml
    /// </summary>
    public partial class TableFormWindow : Window
    {
        #region Private fields

        private DataTable m_Dt;
        private SQLWindow m_SqlWindow;
        private readonly TableToFromDb m_TtfDb;
        private readonly TableToFromXml m_TtfXml;

        #endregion
        #region Ctors

        public TableFormWindow()
        {
            InitializeComponent();
            m_TtfXml = new TableToFromXml();
            m_TtfDb = new TableToFromDb();
            m_Dt = InitializeDataGrid(tableGrid, "dataTable");
        }

        #endregion
        #region Private methods

        private void buttonFromDB_Click(object sender, RoutedEventArgs e)
        {
            m_Dt = m_TtfDb.GetDataFromDb(tableGrid, "dataTable");
        }

        private void buttonFromXML_Click(object sender, RoutedEventArgs e)
        {
            m_Dt = m_TtfXml.GetDataFromXml(tableGrid, "dataTable");
        }

        private void buttonToDB_Click(object sender, RoutedEventArgs e)
        {
            m_TtfDb.SaveDataToDb(m_Dt, "dataTable");
        }

        private void buttonToXML_Click(object sender, RoutedEventArgs e)
        {
            m_TtfXml.SaveDataToXml(m_Dt, "dataTable");
        }

        private DataTable GenerateRows(DataTable dataTable)
        {
            dataTable.Rows.Add(1, "Jan", "Kowalski", 124436, DateTime.Now, "graphic5");
            dataTable.Rows.Add(2, "Izabela", "Skorpion", 114515, DateTime.Now, "graphic");
            dataTable.Rows.Add(3, "Grzegorz", "Cebula", 131234, DateTime.Now, "graphic2");
            dataTable.Rows.Add(4, "Joanna", "Palec", 156789, DateTime.Now, "graphic4");
            dataTable.Rows.Add(5, "Ewa", "Kot", 185367, DateTime.Now, "graphic3");

            return dataTable;
        }

        private DataTable InitializeDataGrid(DataGrid dataGrid, string tableName)
        {
            var dataTable = m_TtfXml.GetDataFromXml(dataGrid, tableName);
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                m_SqlWindow = new SQLWindow();
                m_SqlWindow.Show();
            }
        }

        #endregion
    }
}