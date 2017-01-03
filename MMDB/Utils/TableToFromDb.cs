using System.Data;
using System.Windows.Controls;

namespace MMDB.Utils
{
    public static class TableToFromDb
    {
        #region Constants

        private const string CONNETION_S = "";

        #endregion
        #region Public static methods

        public static DataTable GetDataFromDb(DataGrid dataGrid, string tableName)
        {
            var dataTable = new DataTable(tableName);
            dataGrid.DataContext = dataTable.DefaultView;
            return dataTable;
        }

        public static void SaveDataToDb(DataTable dataTable, string tableName)
        {
        }

        #endregion
    }
}