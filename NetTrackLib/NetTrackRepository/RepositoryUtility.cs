using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetTrackRepository
{
    public static class RepositoryUtility
    {
        public static System.Collections.ArrayList GetColumnList(SqlDataReader dr)
        {
            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                ar.Add(dr.GetName(i));
            }
            return ar;
        }

        public static System.Collections.ArrayList GetColumnList(DataTable dt)
        {
            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            DataColumnCollection dcList = dt.Columns;
            for (int i = 0; i < dcList.Count; i++)
            {
                ar.Add(dcList[i].ColumnName);
            }
            return ar;
        }
    }
}