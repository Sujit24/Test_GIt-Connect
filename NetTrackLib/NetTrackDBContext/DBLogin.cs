using System;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;


namespace NetTrackDBContext
{
    public class DBLogin : DBContext
    {
        #region private property
        private SqlDataReader _dataReader;
        private SqlParameter[] _spParameters;
        private DataTable _dataTable;
        private DataSet _dataSet;
        private string _spName;
        #endregion


        #region Constructor

        // default constructor
        public DBLogin()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion

        #region DBLogin public method

        public DataTable GetUserInfo(UserModel userModel)
        {
            _spName = "us_logon_nettrack2";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
							  new SqlParameter("@login",userModel.Login),
							  new SqlParameter("@pin",userModel.Pin),
							  new SqlParameter("@newpin",userModel.NewPin),
							  new SqlParameter("@nTimeDiff",userModel.TimeDiff),
							  new SqlParameter("@browserinfo", userModel.BrowserInfo),
							  new SqlParameter("@urlinfo", userModel.UrlInfo)
						};
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }


        public DataTable GetUserDetailInfo(UserModel userModel)
        {
            //_spName = "us_logon_nettrack2";
            _dataTable = new DataTable();
            /*_spParameters = new SqlParameter[]{
							  new SqlParameter("@login",userModel.Login),
							  new SqlParameter("@pin",userModel.Pin),
							  new SqlParameter("@newpin",userModel.NewPin),
							  new SqlParameter("@nTimeDiff",userModel.TimeDiff),
							  new SqlParameter("@browserinfo", userModel.BrowserInfo),
							  new SqlParameter("@urlinfo", userModel.UrlInfo)
						};*/

            _spName = "TSS_AuthenticateCustomer";
            _spParameters = new SqlParameter[]{
                                new SqlParameter("@Username",userModel.Login),
                                new SqlParameter("@Password",userModel.Pin)
                        };

            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        public SqlDataReader GetUserLoginInfo(UserModel userModel)
        {
            _spName = "ug_employee_email";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[] { new SqlParameter("@email", userModel.Email) };
            _dataReader = ExecuteReader(_spName, _spParameters);
            return _dataReader;
        }
        #endregion
    }
}
