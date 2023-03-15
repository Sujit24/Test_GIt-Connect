using System.Data;
using System.Data.SqlClient;
using NetTrackModel;
using System;

namespace NetTrackDBContext
{
    public class DBSalesTax : DBContext
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
        public DBSalesTax()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBSalesTax public method

        public DataTable GetSalesTaxList()
        {
            _spName = "ug_SalesTax";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public SalesTaxModel SaveSalesTax(SalesTaxModel model)
        {
            _spName = "us_SalesTax";
            _spParameters = new SqlParameter[]{
							    new SqlParameter("@SalesTaxId", model.SalesTaxId),
                                new SqlParameter("@TaxRate", model.TaxRate)
						    };
            int result = ExecuteNoResult(_spName, _spParameters);

            return model;
        }

        #endregion DBSalesTax public method
    }
}
