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

        private void PrintQueryResults(string[][] results)
        {
            if (results != null && results.Length > 0)
            {
                for (var i = 0; i < results[0].Length; ++i)
                {
                    foreach (var col in results)
                        SqlResultsBox.AppendText(col[i] + ' ');
                    SqlResultsBox.AppendText("\n");
                }
            }
        }

        private void StartScriptButton_Click(object sender, RoutedEventArgs e)
        {
            var sqlCommand = new TextRange(SqlCommandsBox.Document.ContentStart, SqlCommandsBox.Document.ContentEnd).Text;
            SqlResultsBox.Document.Blocks.Clear();

            if (sqlCommand.Contains("select") && sqlCommand.Contains("from"))
            {
                // getting column names
                var pFrom = sqlCommand.IndexOf("select ", StringComparison.Ordinal) + "select ".Length;
                var pTo = sqlCommand.LastIndexOf(" from", StringComparison.Ordinal);
                var sqlCommandColumns = sqlCommand.Substring(pFrom, pTo - pFrom);
                // getting array of names
                var sqlCommandColumnsNames = sqlCommandColumns.Split(new[] {", "}, StringSplitOptions.None);
                // get result
                var results = new string[sqlCommandColumnsNames.Length][];
                if (sqlCommand.Contains("where"))
                {
                    for (var i = 0; i < sqlCommandColumnsNames.Length; ++i)
                        results[i] = SqlParser.GetSqlWhereResultString(DataTable, sqlCommandColumnsNames[i]);
                    PrintQueryResults(results);
                }
                else
                {
                    for (var i = 0; i < sqlCommandColumnsNames.Length; ++i)
                        results[i] = SqlParser.GetSqlColumnResultString(DataTable, sqlCommandColumnsNames[i]);
                    PrintQueryResults(results);
                }
            }
        }

        #endregion
    }
}