using System.Data;
using System.IO;
using System.Windows.Controls;

namespace MMDB.Utils
{
    internal class TableToFromXml
    {
        #region Public methods

        public DataTable GetDataFromXml(DataGrid dataGrid, string tableName)
        {
            var dataTable = new DataTable(tableName);
            if (!File.Exists(tableName + ".xml")) return null;

            var ds = new DataSet();
            ds.ReadXml(tableName + ".xml");
            dataTable = ds.Tables[0];
            dataGrid.DataContext = dataTable.DefaultView;
            dataGrid.ColumnWidth = 80;

            return dataTable;
        }

        public void SaveDataToXml(DataTable dataTable, string tableName)
        {
            dataTable.WriteXml(tableName + ".xml");
        }

        #endregion
    }
}