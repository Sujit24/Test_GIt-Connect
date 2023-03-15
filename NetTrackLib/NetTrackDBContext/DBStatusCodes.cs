using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;
using NetTrackModel.Params;

namespace NetTrackDBContext
{
    public class DBStatusCodes : DBContext
    {
        public OutParams saveStatusCode(StatusCodesInputParams ip, UgsStatusCodeModel m)
        {
            OutParams result = null;

            string _spName = "us_status_code";
            List<SqlParameter> pl = new List<SqlParameter>();
            pl.AddRange(new SqlParameter[] { new SqlParameter("@sessionid", ip.SessionID), 
                                             new SqlParameter("@action", ip.action),
                                             new SqlParameter("@truckstatuscode",m != null ? m.TruckStatus : ip.TruckStatus),
                                             new SqlParameter("@TruckStatusName", m!= null ? m.TruckStatusName : "")
                                           });

            int sqlResult = ExecuteNoResult(_spName, pl.ToArray());
            string msg = "";

            switch (ip.action)
            {
                case "I": msg = "Insert"; break;
                case "U": msg = "Update"; break;
                case "D": msg = "Delete"; break;
            }


            if (sqlResult > 0)
            {
                result = new OutParams(0, msg+" Successful", sqlResult.ToString() + " rows "+msg+"ed");
            }
            else
            {
                result = new OutParams(1, msg + " Failed", sqlResult.ToString() + " rows " + msg + "ed");
            }

            return result;
        }
        public DataTable getStatusCodeList(StatusCodesInputParams ip)
        {
            return ExecuteDataTable("ug_status_codes", new SqlParameter[] { new SqlParameter("@sessionid", ip.SessionID) });
        }

        public DataTable getStatusCode(StatusCodesInputParams ip)
        {
            return ExecuteDataTable("ug_status_code", new SqlParameter[]{ new SqlParameter("@sessionid", ip.SessionID),
                                                                        new SqlParameter("@truckstatuscode", String.Format("{0:00}", ip.TruckStatus))
                                                                      });
        }

    }
}
