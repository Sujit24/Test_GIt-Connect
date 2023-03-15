using NetTrackModel;
using System.Data;
using System.Data.SqlClient;

namespace NetTrackDBContext
{
    public class DBSecurityGroup : DBContext
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
        public DBSecurityGroup()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBSecurityGroup public method

        public DataTable GetAllSecurityGroup()
        {
            _spName = "ug_TssGroup";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public DataTable GetSecurityGroupById(long securityGroupId)
        {
            _spName = "ug_TssGroup";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] {
                new SqlParameter("@TssGroupId", securityGroupId)
            };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public void SaveSecurityGroup(SecurityGroupModel model)
        {
            _spName = "us_TssGroup";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@TssGroupId", model.SecurityGroupId),
                new SqlParameter("@GroupName", model.GroupName),
                new SqlParameter("@Action", model.Action)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        #endregion DBTSSSettings public method
    }
}
