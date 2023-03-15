using NetTrackModel;
using System.Data;
using System.Data.SqlClient;

namespace NetTrackDBContext
{
    public class DBSecurityGroupSalesPerson : DBContext
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
        public DBSecurityGroupSalesPerson()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBSecurityGroupSalesPerson public method

        public DataTable GetSalesPersons(SecurityGroupSalesPersonModel model)
        {
            _spName = "ug_TssGroupSalesPerson";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] {
                new SqlParameter("@ClientId", model.ClientIds),
                new SqlParameter("@TssGroupId", model.SecurityGroupId),
                new SqlParameter("@EmployeeId", model.EmployeeId),
                new SqlParameter("@Action", model.Action)
            };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public void SaveSecurityGroupSalesPerson(SecurityGroupSalesPersonModel model)
        {
            _spName = "us_TssGroupSalesPerson";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@TssGroupSalesPersonId", model.SecurityGroupSalesPersonId),
                new SqlParameter("@TssGroupId", model.SecurityGroupId),
                new SqlParameter("@EmployeeId", model.EmployeeId),
                new SqlParameter("@Action", model.Action)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        #endregion DBTSSSettings public method
    }
}
