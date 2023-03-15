using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NetTrackModel;
using NetTrackModel.Params;
using NetTrackDBContext;

namespace NetTrackRepository
{
    public class StatusCodesRepository
    {
        DBStatusCodes _SC;
        public StatusCodesRepository()
        {
            _SC = new DBStatusCodes();
        }
        public OutParams saveStatusCode(StatusCodesInputParams ip, UgsStatusCodeModel m)
        {
            return _SC.saveStatusCode(ip, m);
        }
        public List<UgsStatusCodeModel> getStatusCodeList(StatusCodesInputParams ip)
        {
            List<UgsStatusCodeModel> _tl = new List<UgsStatusCodeModel>();
            DataTable dt = _SC.getStatusCodeList(ip);
            UgsStatusCodeModel m;
            foreach (DataRow dr in dt.Rows)
            {
                m = new UgsStatusCodeModel();
                m.TruckStatus = Convert.ToInt16( dr.Field<string>("TruckStatus"));
                m.TruckStatusName = dr.Field<string>("TruckStatusName");
                _tl.Add(m);
            }

            return _tl;
        }
        public UgsStatusCodeModel getStatusCode(StatusCodesInputParams ip)
        {
            DataTable dt = _SC.getStatusCode(ip);
            UgsStatusCodeModel m = new UgsStatusCodeModel();
            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                m = new UgsStatusCodeModel();
                m.TruckStatus = Convert.ToInt16(dr.Field<string>("TruckStatusCode"));
                m.TruckStatusName = dr.Field<string>("TruckStatusName");
            }
            return m; 
        }

    }
}
