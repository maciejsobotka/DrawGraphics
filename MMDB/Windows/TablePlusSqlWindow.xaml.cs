using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MMDB.Utils;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for TableFormWindow.xaml
    /// </summary>
    public partial class TablePlusSqlWindow : Window
    {
        #region Properties

        public DataTable DataTable { get; set; }
        public SqlWindow SqlWindow { get; set; }

        #endregion
        #region Ctors

        public TablePlusSqlWindow()
        {
            InitializeComponent();
            DataTable = InitializeDataGrid(tableGrid, "Table");
            TableNameLabel.Content = DataTable.TableName.ToUpper();
        }

        #endregion
        #region Private methods

        private void buttonFromDB_Click(object sender, RoutedEventArgs e)
        {
            DataTable = TableToFromDb.GetDataFromDb(tableGrid, "Table");
        }

        private void buttonFromXML_Click(object sender, RoutedEventArgs e)
        {
            DataTable = TableToFromXml.GetDataFromXml(tableGrid, "Table");
        }

        private void buttonToDB_Click(object sender, RoutedEventArgs e)
        {
            TableToFromDb.SaveDataToDb(DataTable, "Table");
        }

        private void buttonToXML_Click(object sender, RoutedEventArgs e)
        {
            TableToFromXml.SaveDataToXml(DataTable, "Table");
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

        private void GetSqlResult<T>(string[] sqlCommandParts)
        {
            var results = from row in DataTable.AsEnumerable()
                select row.Field<T>(sqlCommandParts[1]);
            SqlResultsBox.Document.Blocks.Clear();
            foreach (var result in results)
            {
                SqlResultsBox.AppendText(result.ToString());
                SqlResultsBox.AppendText("\n");
            }
        }

        private DataTable InitializeDataGrid(DataGrid dataGrid, string tableName)
        {
            var dataTable = TableToFromXml.GetDataFromXml(dataGrid, tableName);
            if (dataTable == null)
            {
                dataTable = new DataTable("Table");
                dataTable.Columns.Add("ID", typeof(int));
                dataTable.Columns.Add("NAME", typeof(string));
                dataTable.Columns.Add("SURNAME", typeof(string));
                dataTable.Columns.Add("NUMBER", typeof(int));
                dataTable.Columns.Add("ADD_DATE", typeof(DateTime));
                dataTable.Columns.Add("GRAPHIC", typeof(string));
                dataTable.Columns.Add("PATH", typeof(string));
                dataTable = GenerateRows(dataTable);
                dataGrid.DataContext = dataTable.DefaultView;
            }
            dataGrid.ColumnWidth = (Width - dataTable.Columns.Count * 4) / dataTable.Columns.Count;
            return dataTable;
        }

        private void StartScriptButton_Click(object sender, RoutedEventArgs e)
        {
            var sqlCommand = new TextRange(SqlCommandsBox.Document.ContentStart, SqlCommandsBox.Document.ContentEnd).Text;
            var sqlCommandParts = sqlCommand.Split(' ');
            if (sqlCommandParts.Length >= 4)
            {
                if (DataTable.Columns.Contains(sqlCommandParts[1]))
                {
                    var type = DataTable.Columns[sqlCommandParts[1]].DataType;
                    if (type.Name == "String")
                    {
                        GetSqlResult<string>(sqlCommandParts);
                    }
                    if (type.Name == "Int32")
                    {
                        GetSqlResult<int>(sqlCommandParts);
                    }
                    if (type.Name == "DateTime")
                    {
                        GetSqlResult<DateTime>(sqlCommandParts);
                    }
                }
            }
        }

        #endregion
    }
}