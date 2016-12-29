using System.Data;
using System.Windows.Controls;

namespace MMDB.Utils
{
    internal class TableToFromDb
    {
        #region Constants

        private const string CONNETION_S = "";

        #endregion
        #region Public methods

        public DataTable GetDataFromDb(DataGrid dataGrid, string tableName)
        {
            var dataTable = new DataTable(tableName);
            dataGrid.DataContext = dataTable.DefaultView;
            dataGrid.ColumnWidth = 80;
            return dataTable;
        }

        public void SaveDataToDb(DataTable dataTable, string tableName)
        {
        }

        #endregion
    }
}