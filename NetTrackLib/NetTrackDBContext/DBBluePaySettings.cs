using System.Data;
using System.Data.SqlClient;
using NetTrackModel;
using System;

namespace NetTrackDBContext
{
    public class DBBluePaySettings : DBContext
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
        public DBBluePaySettings()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBBluePaySettings public method

        public DataTable GetBluePaySettingsInfo(string serverName)
        {
            _spName = "ug_BluePaySettings";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
		                        new SqlParameter("@ServerName", serverName)
						    };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public BluePaySettingsModel SaveBluePaySettings(BluePaySettingsModel model)
        {
            _spName = "us_BluePaySettings";
            _spParameters = new SqlParameter[]{
							    new SqlParameter("@ServerName", model.ServerName),
                                new SqlParameter("@CurrentMode", model.CurrentMode)
						    };
            int result = ExecuteNoResult(_spName, _spParameters);

            return model;
        }

        public BluePayLogModel SaveBluePayLog(BluePayLogModel model)
        {
            _spName = "us_BluePayLog";

            SqlParameter spBluePayLogId = new SqlParameter("@BluePayLogId", model.BluePayLogId);
            spBluePayLogId.Direction = ParameterDirection.InputOutput;
            _spParameters = new SqlParameter[]{
                                spBluePayLogId,
							    new SqlParameter("@QuoteId", model.QuoteId),
                                new SqlParameter("@SubmittedUrl", model.SubmittedUrl),
                                new SqlParameter("@ReceivedUrl", model.ReceivedUrl)
						    };
            int result = ExecuteNoResult(_spName, _spParameters);

            model.BluePayLogId = Convert.ToInt32(spBluePayLogId.Value);

            return model;
        }

        #endregion DBBluePaySettings public method
    }
}
