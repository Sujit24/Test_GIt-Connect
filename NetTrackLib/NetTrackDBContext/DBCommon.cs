using System;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;
using System.Collections;
using System.Collections.Generic;
namespace NetTrackDBContext
{
    public class DBCommon : DBContext
    {
        #region private property

        private SqlDataReader _dataReader;
        private SqlParameter[] _spParameters;
        private DataTable _dataTable;
        private DataSet _dataSet;
        private string _spName;

        #endregion private property

        #region Constructor

        // default constructor
        public DBCommon()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region common public method

        public DataTable GetDdlsource(IDdlSourceModel ddlModel)
        {
            _spName = ddlModel.SpName;
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
							  new SqlParameter("@sessionid",ddlModel.sessionid)
						};
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }
        public DataTable GetDdlsource(string spName,string param)
        {
            _spName = spName;
            _dataTable = new DataTable();
            
            _spParameters = convertToSqlParam(param);
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }
        #endregion common public method

        SqlParameter[] convertToSqlParam(string param)
        {
            string[] paramArray = param.Split(',');
            List<SqlParameter> paramDict =new  List<SqlParameter>();
            foreach (string i in paramArray)
            {
                
                string[] tempArray = i.Split(':');
                paramDict.Add(new SqlParameter( tempArray[0], tempArray[1]));
            }
            return paramDict.ToArray();
        }
    }
}