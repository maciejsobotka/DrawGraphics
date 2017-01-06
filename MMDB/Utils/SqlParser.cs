using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMDB.Utils
{
    public static class SqlParser
    {
        #region Public static methods

        public static string[] GetSqlColumnResultString(DataTable dataTable, string columnName)
        {
            string[] result = null;
            if (dataTable.Columns.Contains(columnName))
            {
                var type = dataTable.Columns[columnName].DataType;
                if (type.Name == "String")
                {
                    result = GetSqlColumnResult<string>(dataTable, columnName);
                }
                if (type.Name == "Int32")
                {
                    result = GetSqlColumnResult<int>(dataTable, columnName);
                }
                if (type.Name == "DateTime")
                {
                    result = GetSqlColumnResult<DateTime>(dataTable, columnName);
                }
            }
            return result;
        }

        public static string[] GetSqlWhereResultString(DataTable dataTable, string columnName, List<string> graphics)
        {
            string[] result = null;
            if (dataTable.Columns.Contains(columnName))
            {
                var type = dataTable.Columns[columnName].DataType;
                if (type.Name == "String")
                {
                    result = GetSqlWhereResult<string>(dataTable, columnName, graphics);
                }
                if (type.Name == "Int32")
                {
                    result = GetSqlWhereResult<int>(dataTable, columnName, graphics);
                }
                if (type.Name == "DateTime")
                {
                    result = GetSqlWhereResult<DateTime>(dataTable, columnName, graphics);
                }
            }
            return result;
        }

        #endregion
        #region Private methods

        private static string[] GetSqlColumnResult<T>(DataTable dataTable, string columnName)
        {
            var columnData = from row in dataTable.AsEnumerable()
                select row.Field<T>(columnName);

            var columnDataArray = columnData.ToArray();

            var results = new string[columnData.Count()];
            for (var i = 0; i < columnData.Count(); ++i)
                results[i] = columnDataArray[i].ToString();
            return results;
        }

        private static string[] GetSqlWhereResult<T>(DataTable dataTable, string columnName, List<string> graphics)
        {
            var columnData = from row in dataTable.AsEnumerable()
                where graphics.Contains(row.Field<string>("PATH") + row.Field<string>("GRAPHIC"))
                select row.Field<T>(columnName);

            var columnDataArray = columnData.ToArray();

            var results = new string[columnData.Count()];
            for (var i = 0; i < columnData.Count(); ++i)
                results[i] = columnDataArray[i].ToString();
            return results;
        }

        #endregion
    }
}