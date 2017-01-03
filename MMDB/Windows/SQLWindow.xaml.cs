using System;
using System.Data;
using System.Windows;
using System.Windows.Documents;

namespace MMDB.Windows
{
    /// <summary>
    ///     Interaction logic for SqlWindow.xaml
    /// </summary>
    public partial class SqlWindow : Window
    {
        #region Properties

        private DataTable Data { get; set; }

        #endregion
        #region Ctors

        public SqlWindow(DataTable dataTable)
        {
            InitializeComponent();
            Data = dataTable;
        }

        #endregion
        #region Private methods

        private void GetSqlResult<T>(string[] sqlCommandParts)
        {
            var results = from row in Data.AsEnumerable()
                select row.Field<T>(sqlCommandParts[1]);
            foreach (var result in results)
            {
                SqlResultsBox.AppendText(result.ToString());
                SqlResultsBox.AppendText("\n");
            }
        }

        private void StartScriptButton_Click(object sender, RoutedEventArgs e)
        {
            var sqlCommand = new TextRange(SqlCommandsBox.Document.ContentStart, SqlCommandsBox.Document.ContentEnd).Text;
            var sqlCommandParts = sqlCommand.Split(' ');
            if (sqlCommandParts.Length >= 4)
            {
                if (Data.Columns.Contains(sqlCommandParts[1]))
                {
                    var type = Data.Columns[sqlCommandParts[1]].DataType;
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