using System.Data;
using System.Data.SqlClient;
using NetTrackModel;


namespace NetTrackDBContext
{
    public class DBUserState : DBContext
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
        public DBUserState()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion

        #region DBUserState public method

        public DataTable UpdateUserState(UserStateModel userStateModel)
        {
            _spName = "ugs_user_state";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@SessionId", userStateModel.SessionId),
                                                new SqlParameter("@StateId", userStateModel.StateId), 
                                                new SqlParameter("@UserId", userStateModel.UserId),
                                                new SqlParameter("@IsHeaderView", userStateModel.IsHeaderView), 
                                                new SqlParameter("@IsFooterView", userStateModel.IsFooterView), 
                                                new SqlParameter("@IsRightMenu", userStateModel.IsRightMenu), 
                                                new SqlParameter("@IsLeftMapView", userStateModel.IsLeftMapView), 
                                                new SqlParameter("@IsShowLeftMapView", userStateModel.IsShowLeftMapView), 
                                                new SqlParameter("@IsTruckStatus", userStateModel.IsTruckStatus), 
                                                new SqlParameter("@IsLegends", userStateModel.IsLegends), 
                                                new SqlParameter("@IsGridView", userStateModel.IsGridView), 
                                                new SqlParameter("@IsTruckDetail", userStateModel.IsTruckDetail), 
                                                new SqlParameter("@GridViewPagingIndex", userStateModel.GridViewPagingIndex), 
                                                new SqlParameter("@ActiveButton", userStateModel.ActiveButton), 
                                                new SqlParameter("@MapZoomLevel", userStateModel.MapZoomLevel),
                                                new SqlParameter("@MapCenterLat", userStateModel.MapCenterLat),
                                                new SqlParameter("@MapCenterLng", userStateModel.MapCenterLng),
                                                new SqlParameter("@MiniMaps", userStateModel.MiniMaps),
                                                new SqlParameter("@PointId", userStateModel.PointId),
                                                new SqlParameter("@DbAction", userStateModel.DbAction)
                                              };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }        
        public DataTable GetUserState(UserStateModel userStateModel)
        {
            _spName = "ugs_user_state";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@SessionId", userStateModel.SessionId),
                                                new SqlParameter("@StateId", userStateModel.StateId), 
                                                new SqlParameter("@UserId", userStateModel.UserId)
                                              };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

        /*=======================================================================*/
        public DataSet UpdateUserStates(UserStateModel userStateModel)
        {
            _spName = "ugs_user_state";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@SessionId", userStateModel.SessionId),
                                                new SqlParameter("@StateId", userStateModel.StateId), 
                                                new SqlParameter("@UserId", userStateModel.UserId),
                                                new SqlParameter("@ClientId", userStateModel.ClientId),
                                                new SqlParameter("@IsHeaderView", userStateModel.IsHeaderView), 
                                                new SqlParameter("@IsFooterView", userStateModel.IsFooterView), 
                                                new SqlParameter("@IsRightMenu", userStateModel.IsRightMenu), 
                                                new SqlParameter("@IsLeftMapView", userStateModel.IsLeftMapView), 
                                                new SqlParameter("@IsShowLeftMapView", userStateModel.IsShowLeftMapView), 
                                                new SqlParameter("@IsTruckStatus", userStateModel.IsTruckStatus), 
                                                new SqlParameter("@IsLegends", userStateModel.IsLegends), 
                                                new SqlParameter("@IsGridView", userStateModel.IsGridView), 
                                                new SqlParameter("@IsTruckDetail", userStateModel.IsTruckDetail), 
                                                new SqlParameter("@GridViewPagingIndex", userStateModel.GridViewPagingIndex), 
                                                new SqlParameter("@ActiveButton", userStateModel.ActiveButton), 
                                                new SqlParameter("@MapZoomLevel", userStateModel.MapZoomLevel),
                                                new SqlParameter("@MapCenterLat", userStateModel.MapCenterLat),
                                                new SqlParameter("@MapCenterLng", userStateModel.MapCenterLng),
                                                new SqlParameter("@MiniMaps", userStateModel.MiniMaps),
                                                new SqlParameter("@PointId", userStateModel.PointId),
                                                new SqlParameter("@IsAdmin", userStateModel.IsAdmin),
                                                new SqlParameter("@LoadType", userStateModel.LoadType),
                                                new SqlParameter("@DbAction", userStateModel.DbAction)
                                              };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }
        public DataSet GetUserStates(UserStateModel userStateModel)
        {
            _spName = "ugs_user_state";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@SessionId", userStateModel.SessionId),
                                                new SqlParameter("@StateId", userStateModel.StateId), 
                                                new SqlParameter("@UserId", userStateModel.UserId),
                                                new SqlParameter("@ClientId", userStateModel.ClientId),
                                                new SqlParameter("@IsAdmin", userStateModel.IsAdmin),
                                                new SqlParameter("@LoadType", userStateModel.LoadType),
                                                new SqlParameter("@DbAction", userStateModel.DbAction)
                                              };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }

        public DataSet UpdateUserStatesGridPage(UserStateModel userStateModel)
        {
            _spName = "ugs_user_state_NetTrack2";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@SessionId", userStateModel.SessionId),
                                                new SqlParameter("@StateId", userStateModel.StateId), 
                                                new SqlParameter("@UserId", userStateModel.UserId),
                                                new SqlParameter("@ClientId", userStateModel.ClientId),
                                                new SqlParameter("@IsAdmin", userStateModel.IsAdmin),
                                                new SqlParameter("@LoadType", userStateModel.LoadType),
                                                new SqlParameter("@DbAction", userStateModel.DbAction),
                                                new SqlParameter("@TruckGridPageSize", userStateModel.TruckGridPageSize),
                                                new SqlParameter("@TerminalGridPageSize", userStateModel.TerminalGridPageSize),
                                                new SqlParameter("@UserGridPageSize", userStateModel.UserGridPageSize),
                                                new SqlParameter("@ClientGridPageSize", userStateModel.ClientGridPageSize),
                                                new SqlParameter("@PointGridPageSize", userStateModel.PointGridPageSize),
                                                new SqlParameter("@ClassGridPageSize", userStateModel.ClassGridPageSize),
                                                new SqlParameter("@AlertGridPageSize", userStateModel.AlertGridPageSize),
                                                new SqlParameter("@ImageGridPageSize", userStateModel.ImageGridPageSize),
                                                new SqlParameter("@StatusGridPageSize", userStateModel.StatusGridPageSize),
                                                new SqlParameter("@NewsGridPageSize", userStateModel.NewsGridPageSize),
                                                new SqlParameter("@HolidayGridPageSize", userStateModel.HolidayGridPageSize),
                                              };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }
        public DataSet GetUserStatesGridPage(UserStateModel userStateModel)
        {
            _spName = "ugs_user_state_NetTrack2";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@SessionId", userStateModel.SessionId),
                                                new SqlParameter("@StateId", userStateModel.StateId), 
                                                new SqlParameter("@UserId", userStateModel.UserId),
                                                new SqlParameter("@ClientId", userStateModel.ClientId),
                                                new SqlParameter("@IsAdmin", userStateModel.IsAdmin),
                                                new SqlParameter("@LoadType", userStateModel.LoadType),
                                                new SqlParameter("@DbAction", userStateModel.DbAction)
                                              };
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            return _dataSet;
        }
        #endregion
    }
}
