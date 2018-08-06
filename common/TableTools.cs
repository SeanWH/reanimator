using System.Data;

namespace Revival.Common
{
    public static class TableTools
    {
        //public static DataTable SelectDistinct(DataTable sourceTable, params string[] fieldNames)
        //{
        //    if (fieldNames == null || fieldNames.Length == 0)
        //        throw new ArgumentNullException(nameof(fieldNames));

        //    var lastValues = new object[fieldNames.Length];
        //    var newTable = new DataTable();

        //    foreach (string fieldName in fieldNames)
        //        newTable.Columns.Add(fieldName, sourceTable.Columns[fieldName].DataType);

        //    var orderedRows = sourceTable.Select("", string.Join(", ", fieldNames));

        //    foreach (DataRow row in orderedRows)
        //    {
        //        if ((int)row[fieldNames[0]] == -1) //lazy. function cant be reused
        //        {
        //            continue; // ignore values -1
        //        }

        //        if (!FieldValuesAreEqual(lastValues, row, fieldNames))
        //        {
        //            DataRow newRow = CreateRowClone(row, newTable.NewRow(), fieldNames);
        //            newTable.Rows.Add(newRow);
        //            SetLastValues(lastValues, row, fieldNames);
        //            newRow[0] = row[0];//modified to store index, not distinct value
        //        }
        //    }

        //    return newTable;
        //}

        public static bool FieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        public static DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private static void SetLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }
    }
}
