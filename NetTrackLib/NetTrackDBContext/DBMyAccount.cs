using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;

namespace NetTrackDBContext
{
    public class DBMyAccount : DBContext
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
        public DBMyAccount()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        public DataTable GetMyAccountInfo(int clientID)
        {
            _spName = "ug_MyAccount";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@ClientId", clientID) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public int SaveMyAccountInfo(MyAccountModel myAccountModel)
        {
            _spName = "us_MyAccount";
            _spParameters = new SqlParameter[]{
                    new SqlParameter("@MyAccountId", myAccountModel.MyAccountId),
                    new SqlParameter("@ClientId", myAccountModel.ClientId),
                    new SqlParameter("@CardNumber", myAccountModel.CardNumber),
					new SqlParameter("@CVV2", myAccountModel.CVV2),
					new SqlParameter("@CardExpireYear", myAccountModel.CardExpireYear),
					new SqlParameter("@CardExpireMonth", myAccountModel.CardExpireMonth),
					new SqlParameter("@CardHolderFirstName", myAccountModel.CardHolderFirstName),
					new SqlParameter("@CardHolderLastName", myAccountModel.CardHolderLastName),	
					new SqlParameter("@Country", myAccountModel.Country),
					new SqlParameter("@Address", myAccountModel.Address),
                    new SqlParameter("@City", myAccountModel.City),
                    new SqlParameter("@State", myAccountModel.State),
                    new SqlParameter("@ZipCode", myAccountModel.ZipCode),
			    };

            return ExecuteNoResult(_spName, _spParameters);
        }
    }
}
