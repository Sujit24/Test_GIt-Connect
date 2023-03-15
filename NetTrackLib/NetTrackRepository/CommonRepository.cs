using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NetTrackDBContext;
using NetTrackModel;

//using System.Text.RegularExpressions;

namespace NetTrackRepository
{
    public class CommonRepository
    {
        private DBCommon _dbContext;

        // Default constructor
        public CommonRepository()
        {
            _dbContext = new DBCommon();
        }

        public List<IDdlSourceModel> GetDdlsource(IDdlSourceModel ddlSourceModel)
        {
            List<IDdlSourceModel> ddlSourceModelList = new List<IDdlSourceModel>();

            DataTable dt = _dbContext.GetDdlsource(ddlSourceModel);
            foreach (DataRow dr in dt.Rows)
            {
                IDdlSourceModel ddlSourceModelTmp = (IDdlSourceModel)Activator.CreateInstance(ddlSourceModel.GetType());
                ddlSourceModelTmp.keyfield = dr["keyfield"].ToString();
                ddlSourceModelTmp.value = dr["value"].ToString();
                ddlSourceModelList.Add(ddlSourceModelTmp);
            }

            return ddlSourceModelList;
        }

        public List<IDdlSourceModel> GetDdlsource(string spName, string param)
        {
            List<IDdlSourceModel> ddlSourceModelList = new List<IDdlSourceModel>();

            DataTable dt = _dbContext.GetDdlsource(spName,param);
            IDdlSourceModel ddlSourceModel = new CommonDDLmodel();
            foreach (DataRow dr in dt.Rows)
            {
                IDdlSourceModel ddlSourceModelTmp = (IDdlSourceModel)Activator.CreateInstance(ddlSourceModel.GetType());
                ddlSourceModelTmp.keyfield = dr["keyfield"].ToString();
                ddlSourceModelTmp.value = dr["value"].ToString();
                try
                {
                    ddlSourceModelTmp.groupValue = dr["groupValue"].ToString();
                }
                catch { }
                
                ddlSourceModelList.Add(ddlSourceModelTmp);
            }

            return ddlSourceModelList;
        }
    }
}