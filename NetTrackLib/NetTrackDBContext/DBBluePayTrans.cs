using System.Data;
using System.Data.SqlClient;
using NetTrackModel;
using System;

namespace NetTrackDBContext
{
    public class DBBluePayTrans : DBContext
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
        public DBBluePayTrans()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region BluePayTrans public method

        public BluePayTransactionModel SaveBluePayTrans(BluePayTransactionModel model)
        {
            _spName = "us_BluePayTrans";
            _spParameters = new SqlParameter[]{
							    new SqlParameter("@TransactionId", model.TransactionId),
                                new SqlParameter("@OrderId", model.OrderId),

                                new SqlParameter("@CardNumber", model.CardNumber),
                                new SqlParameter("@CVV2", model.CVV2),
                                new SqlParameter("@CardExpireYear", model.CardExpireYear),
                                new SqlParameter("@CardExpireMonth", model.CardExpireMonth),
                                new SqlParameter("@Amount", model.Amount),

                                new SqlParameter("@CardHolderFirstName", model.CardHolderFirstName),
                                new SqlParameter("@CardHolderLastName", model.CardHolderLastName),
                                new SqlParameter("@Address", model.Address),
                                new SqlParameter("@City", model.City),
                                new SqlParameter("@State", model.State),
                                new SqlParameter("@ZipCode", model.ZipCode),
                                new SqlParameter("@Country", model.Country),

                                new SqlParameter("@CompanyName", model.CompanyName),
                                new SqlParameter("@Email", model.Email),
                                new SqlParameter("@Phone", model.Phone)
						    };
            int result = ExecuteNoResult(_spName, _spParameters);

            return model;
        }

        #endregion DBSalesTax public method


        public BluePayACHTransactionModel SaveBluePayACHTrans(BluePayACHTransactionModel model)
        {
            _spName = "us_BluePayACHTrans";
            _spParameters = new SqlParameter[]{
                                new SqlParameter("@TransactionId", model.TransactionId),
                                new SqlParameter("@OrderId", model.OrderId),

                                new SqlParameter("@RoutingNumber", model.routingNum),
                                new SqlParameter("@AccountNumber", model.accountNum),
                                new SqlParameter("@Amount", model.Amount),

                                new SqlParameter("@FirstName", model.FirstName),
                                new SqlParameter("@LastName", model.LastName),

                                new SqlParameter("@Address", model.Address),
                                new SqlParameter("@City", model.City),
                                new SqlParameter("@State", model.State),
                                new SqlParameter("@ZipCode", model.ZipCode),
                                new SqlParameter("@Country", model.Country),

                                new SqlParameter("@CompanyName", model.CompanyName),
                                new SqlParameter("@Email", model.Email),
                                new SqlParameter("@Phone", model.Phone),
                                new SqlParameter("@AccountType",model.accountType),
                                new SqlParameter("@PaymentDate",model.PaymentDate),
                                new SqlParameter("@IsFundTransferred",model.IsFundTransferred),
                                new SqlParameter("@BackendId",model.BackendId)
                            };
            int result = ExecuteNoResult(_spName, _spParameters);

            return model;
        }

    }
}
