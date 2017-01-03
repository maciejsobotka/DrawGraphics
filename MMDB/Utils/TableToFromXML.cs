using System.Data;
using System.IO;
using System.Windows.Controls;

namespace MMDB.Utils
{
    public static class TableToFromXml
    {
        #region Public static methods

        public static DataTable GetDataFromXml(DataGrid dataGrid, string tableName)
        {
            if (!File.Exists(tableName + ".xml"))
            {
                return null;
            }

            var ds = new DataSet();
            ds.ReadXml(tableName + ".xml");
            var dataTable = ds.Tables[0];
            dataGrid.DataContext = dataTable.DefaultView;

            return dataTable;
        }

        public static void SaveDataToXml(DataTable dataTable, string tableName)
        {
            dataTable.WriteXml(tableName + ".xml");
        }

        #endregion
    }
}