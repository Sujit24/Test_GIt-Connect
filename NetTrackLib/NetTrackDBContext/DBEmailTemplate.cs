using System;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;

namespace NetTrackDBContext
{
    public class DBEmailTemplate : DBContext
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
        public DBEmailTemplate()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion

        public DataTable GetEmailTemplateList()
        {
            _spName = "ug_EmailTemplate";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public DataTable GetEmailTemplate(int emailTemplateId)
        {
            _spName = "ug_EmailTemplate";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@EmailTemplateId", emailTemplateId) };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public EmailTemplateModel SaveEmailTemplate(EmailTemplateModel model)
        {
            SqlParameter spemp = new SqlParameter("@EmailTemplateId", model.EmailTemplateId);
            spemp.Direction = ParameterDirection.InputOutput;

            _spName = "us_EmailTemplate";
            _spParameters = new SqlParameter[]{
                spemp,
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Template", model.Template),	
                new SqlParameter("@CreatedBy", model.CreatedBy),	
                new SqlParameter("@CreatedDate", model.CreatedDate),	
                new SqlParameter("@ModifiedBy", model.ModifiedBy),
                new SqlParameter("@ModifiedDate", model.ModifiedDate)		 
			};

            int result = ExecuteNoResult(_spName, _spParameters);
            model.EmailTemplateId = Convert.ToInt32(spemp.Value);

            return model;
        }
    }
}
