using System.Data;
using System.Data.SqlClient;

namespace NetTrackDBContext
{
    public class DBUserSession : DBContext
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
        public DBUserSession()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBUserSession public method

        public DataTable GetUserSessionStatus(int sessionId)
        {
            _spName = "ug_user_session_exists_nettrack2";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", sessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }       

        #endregion DBuser public method
    }
}
