using NetTrackModel;
using System.Data;
using System.Data.SqlClient;

namespace NetTrackDBContext
{
    public class DBTSSSettings : DBContext
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
        public DBTSSSettings()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBTSSSettings public method

        public DataTable GetSettings(string settingsName)
        {
            _spName = "ug_TSSSettings";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@SettingsName", settingsName) };
            _dataTable = ExecuteDataTable(_spName, _spParameters);

            return _dataTable;
        }

        public void SetSettings(TSSSettings model)
        {
            _spName = "us_TSSSettings";
            _spParameters = new SqlParameter[]{
                new SqlParameter("@SettingsName", model.SettingsName),
                new SqlParameter("@SettingsValue", model.SettingsValue)
            };

            int result = ExecuteNoResult(_spName, _spParameters);
        }

        #endregion DBTSSSettings public method
    }
}
