using NetTrackModel;
using System.Data;
using System.Data.SqlClient;

namespace NetTrackDBContext
{
    public class DBTemplateGroup : DBContext
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
        public DBTemplateGroup()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBTemplateGroup public method

        public DataTable GetAllTemplateGroup()
        {
            _spName = "ug_TemplateGroup";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public void SaveTemplateGroup(TemplateGroupModel model)
        {
            _spName = "us_TemplateGroup";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@TemplateGroupId", model.TemplateGroupId),
                new SqlParameter("@GroupName", model.GroupName),
                new SqlParameter("@Action", model.Action)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        #endregion DBTSSSettings public method
    }
}
