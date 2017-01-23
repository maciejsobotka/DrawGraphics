using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MMDB.Extensions;
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
        public SearchResultWindow SearchResultWindow { get; set; }

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
            dataTable.Rows.Add(1, "Jan", "Kowalski", 124436, DateTime.Now, "graphic5.xaml", AppDomain.CurrentDomain.BaseDirectory + "..\\..\\examples\\");
            dataTable.Rows.Add(2, "Izabela", "Skorpion", 114515, DateTime.Now, "graphic.xaml", AppDomain.CurrentDomain.BaseDirectory + "..\\..\\examples\\");
            dataTable.Rows.Add(3, "Grzegorz", "Cebula", 131234, DateTime.Now, "graphic2.xaml", AppDomain.CurrentDomain.BaseDirectory + "..\\..\\examples\\");
            dataTable.Rows.Add(4, "Joanna", "Palec", 156789, DateTime.Now, "graphic4.xaml", AppDomain.CurrentDomain.BaseDirectory + "..\\..\\examples\\");
            dataTable.Rows.Add(5, "Ewa", "Kot", 185367, DateTime.Now, "graphic3.xaml", AppDomain.CurrentDomain.BaseDirectory + "..\\..\\examples\\");

            return dataTable;
        }

        private DataTable InitializeDataGrid(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = TableToFromXml.GetDataFromXml(dataGrid, tableName);
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
                    foreach (string[] col in results)
                        SqlResultsBox.AppendText(col[i] + ' ');
                    SqlResultsBox.AppendText("\n");
                }
            }
        }

        private void StartScriptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // working queries
                // select name, id, add_date from table where graphic.Contains(Ellipse >= 2)
                // select name, id, add_date from table where graphic.Contains(Ellipse >= 1 with attribute Fill == Blue)
                // select name, id, add_date from table where graphic.Contains(Ellipse >= 1 with attribute Area == 14000)
                // select name, id, number, graphic from table where graphic.Contains(Elem >= 1 with attribute Perimeter == 500)
                string sqlCommand = new TextRange(SqlCommandsBox.Document.ContentStart, SqlCommandsBox.Document.ContentEnd).Text;
                SqlResultsBox.Document.Blocks.Clear();

                if (sqlCommand.Contains("select") && sqlCommand.Contains("from"))
                {
                    // getting column names
                    string sqlCommandColumns = sqlCommand.Substring("select ", " from");
                    // getting array of names
                    string[] sqlCommandColumnsNames = sqlCommandColumns.Split(new[] {", "}, StringSplitOptions.None);
                    // get result
                    var results = new string[sqlCommandColumnsNames.Length][];
                    if (sqlCommand.Contains("where"))
                    {
                        var filesFound = new List<string>();
                        string sqlMmQuery = sqlCommand.Substring("graphic.Contains(", ")");
                        var path = DataTable.Rows[0].Field<string>("PATH");
                        EnumerableRowCollection<string> files = from row in DataTable.AsEnumerable()
                            select row.Field<string>("GRAPHIC");
                        if (!sqlMmQuery.Contains("with attribute"))
                        {
                            string[] sqlMmQueryParts = sqlMmQuery.Split(' ');

                            foreach (string file in files)
                                if (SqlMmParser.SearchResult(path + file, sqlMmQueryParts[0], int.Parse(sqlMmQueryParts[2]), sqlMmQueryParts[1]))
                                {
                                    filesFound.Add(file);
                                }
                            for (var i = 0; i < sqlCommandColumnsNames.Length; ++i)
                                results[i] = SqlParser.GetSqlWhereResultString(DataTable, sqlCommandColumnsNames[i], filesFound);
                            PrintQueryResults(results);
                        }
                        else
                        {
                            string[] sqlMmSplit = sqlMmQuery.Split(new[] {" with attribute "}, StringSplitOptions.None);
                            string[] sqlMmQueryParams = sqlMmSplit[0].Split(' ');
                            string[] sqlMmQueryAttributes = sqlMmSplit[1].Split(' ');

                            foreach (string file in files)
                                if (SqlMmParser.SearchResult(path + file, sqlMmQueryParams[0], int.Parse(sqlMmQueryParams[2]), sqlMmQueryParams[1], sqlMmQueryAttributes[0], sqlMmQueryAttributes[2]))
                                {
                                    filesFound.Add(file);
                                }
                            for (var i = 0; i < sqlCommandColumnsNames.Length; ++i)
                                results[i] = SqlParser.GetSqlWhereResultString(DataTable, sqlCommandColumnsNames[i], filesFound);
                            PrintQueryResults(results);
                        }
                        SearchResultWindow = new SearchResultWindow(filesFound, path);
                        SearchResultWindow.Show();
                    }
                    else
                    {
                        for (var i = 0; i < sqlCommandColumnsNames.Length; ++i)
                            results[i] = SqlParser.GetSqlColumnResultString(DataTable, sqlCommandColumnsNames[i]);
                        PrintQueryResults(results);
                    }
                }
            }
            catch (Exception ex)
            {
                SqlResultsBox.AppendText(ex.Message + '\n' + ex.StackTrace);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                StartScriptButton_Click(sender, e);
            }
        }

        #endregion
    }
}