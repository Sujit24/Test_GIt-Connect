using System;
using System.Data;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections.Generic;

namespace NetTrackRepository
{
    public class UserStateRepository
    {
        private UserStateModel _userStateModel;
        private DBUserState _dbUserState;

        // default constructor
        public UserStateRepository()
        {
            _userStateModel = new UserStateModel();
            _dbUserState = new DBUserState();
        }

        public UserStateModel MaintainUserState(UserStateModel userStateModel)
        {
            _userStateModel = new UserStateModel();
            _dbUserState = new DBUserState();
            try
            {
                List<LeftMapModel> LeftMapModelList = new List<LeftMapModel>();
                DataTable dtUserState = new DataTable();
                DataTable dtUserStateMiniMap = new DataTable();
                DataSet dsUserStates = new DataSet();
                if (userStateModel.DbAction != "U")
                {
                    //dtUserState = _dbUserState.GetUserState(userStateModel);
                    dsUserStates = _dbUserState.GetUserStates(userStateModel);
                    if (dsUserStates.Tables[0] != null)
                    {
                        dtUserState = dsUserStates.Tables[0];
                    }
                    if (dsUserStates.Tables[1] != null)
                    {
                        dtUserStateMiniMap = dsUserStates.Tables[1];
                    }
                }
                else {
                    //dtUserState = _dbUserState.UpdateUserState(userStateModel);
                    dsUserStates = _dbUserState.UpdateUserStates(userStateModel);
                    if (dsUserStates.Tables[0] != null)
                    {
                        dtUserState = dsUserStates.Tables[0];
                    }
                    if (dsUserStates.Tables[1] != null)
                    {
                        dtUserStateMiniMap = dsUserStates.Tables[1];
                    }
                }
                
                foreach (DataRow dr in dtUserState.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["StateId"].ToString()))
                    {
                        _userStateModel.StateId = Int32.Parse(dr["StateId"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["UserId"].ToString()))
                    {
                        _userStateModel.UserId = Int32.Parse(dr["UserId"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["IsHeaderView"].ToString()))
                    {
                        _userStateModel.IsHeaderView = dr["IsHeaderView"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsFooterView"].ToString()))
                    {
                        _userStateModel.IsFooterView = dr["IsFooterView"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsRightMenu"].ToString()))
                    {
                        _userStateModel.IsRightMenu = dr["IsRightMenu"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsLeftMapView"].ToString()))
                    {
                        _userStateModel.IsLeftMapView = dr["IsLeftMapView"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsShowLeftMapView"].ToString()))
                    {
                        _userStateModel.IsShowLeftMapView = dr["IsShowLeftMapView"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsTruckStatus"].ToString()))
                    {
                        _userStateModel.IsTruckStatus = dr["IsTruckStatus"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsLegends"].ToString()))
                    {
                        _userStateModel.IsLegends = dr["IsLegends"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsGridView"].ToString()))
                    {
                        _userStateModel.IsGridView = dr["IsGridView"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsTruckDetail"].ToString()))
                    {
                        _userStateModel.IsTruckDetail = dr["IsTruckDetail"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["GridViewPagingIndex"].ToString()))
                    {
                        _userStateModel.GridViewPagingIndex = Int32.Parse(dr["GridViewPagingIndex"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["ActiveButton"].ToString()))
                    {
                        _userStateModel.ActiveButton = dr["ActiveButton"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["MapZoomLevel"].ToString()))
                    {
                        _userStateModel.MapZoomLevel = Int32.Parse(dr["MapZoomLevel"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["MapCenterLat"].ToString()))
                    {
                        _userStateModel.MapCenterLat = Double.Parse(dr["MapCenterLat"].ToString()); 
                    }
                    if (!string.IsNullOrEmpty(dr["MapCenterLng"].ToString()))
                    {
                        _userStateModel.MapCenterLng = Double.Parse(dr["MapCenterLng"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["EntryDate"].ToString()))
                    {
                        _userStateModel.EntryDate = DateTime.Parse(dr["EntryDate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["ChangeDate"].ToString()))
                    {
                        _userStateModel.ChangeDate = DateTime.Parse(dr["ChangeDate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["clientid"].ToString()))
                    {
                        _userStateModel.ClientId = Int32.Parse(dr["clientid"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["PointId"].ToString()))
                    {
                        _userStateModel.PointId = dr["PointId"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["FollowMe"].ToString()))
                    {
                        _userStateModel.FollowMe = dr["FollowMe"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["IsAdmin"].ToString()))
                    {
                        _userStateModel.IsAdmin = dr["IsAdmin"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr["MapRefresh"].ToString()))
                    {
                        _userStateModel.MapRefresh = Convert.ToInt32(dr["MapRefresh"]);
                    }
                    if (!string.IsNullOrEmpty(dr["PreferredMap"].ToString()))
                    {
                        _userStateModel.PreferredMap = Convert.ToInt32(dr["PreferredMap"]);
                    }
                    if (!string.IsNullOrEmpty(dr["HaveGoogleRouting"].ToString()))
                    {
                        _userStateModel.HaveGoogleRouting = Convert.ToInt32(dr["HaveGoogleRouting"]);
                    }
                    

                }
                foreach (DataRow dr in dtUserStateMiniMap.Rows)
                {
                    LeftMapModelList.Add(new LeftMapModel(Int32.Parse(dr["TruckId"].ToString()), Int32.Parse(dr["OrderId"].ToString())));
                }
                _userStateModel.LeftMapModelList = LeftMapModelList;
            }
            catch (Exception exception) { 
                //
            }

            return _userStateModel;
        }
        
        public UserStateModel MaintainUserStateGridPage(UserStateModel userStateModel)
        {
            _userStateModel = new UserStateModel();
            _dbUserState = new DBUserState();
            try
            {
               
                DataTable dtUserState = new DataTable();
               
                DataSet dsUserStates = new DataSet();
                if (userStateModel.DbAction != "U")
                {
                    //dtUserState = _dbUserState.GetUserState(userStateModel);
                    dsUserStates = _dbUserState.GetUserStatesGridPage(userStateModel);
                    if (dsUserStates.Tables[0] != null)
                    {
                        dtUserState = dsUserStates.Tables[0];
                    }
                    
                }
                else {
                    //dtUserState = _dbUserState.UpdateUserState(userStateModel);
                    dsUserStates = _dbUserState.UpdateUserStatesGridPage(userStateModel);
                    if (dsUserStates.Tables[0] != null)
                    {
                        dtUserState = dsUserStates.Tables[0];
                    }
                    
                }
                
                foreach (DataRow dr in dtUserState.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["StateId"].ToString()))
                    {
                        _userStateModel.StateId = Int32.Parse(dr["StateId"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["UserId"].ToString()))
                    {
                        _userStateModel.UserId = Int32.Parse(dr["UserId"].ToString());
                    }
                   
                    if (!string.IsNullOrEmpty(dr["clientid"].ToString()))
                    {
                        _userStateModel.ClientId = Int32.Parse(dr["clientid"].ToString());
                    }
                   
                    if (!string.IsNullOrEmpty(dr["IsAdmin"].ToString()))
                    {
                        _userStateModel.IsAdmin = dr["IsAdmin"].ToString();
                    }
                    _userStateModel.TruckGridPageSize=!string.IsNullOrEmpty(dr["TruckGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["TruckGridPageSize"].ToString()):0;
                    _userStateModel.TerminalGridPageSize=!string.IsNullOrEmpty(dr["TerminalGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["TerminalGridPageSize"].ToString()):0;
                    _userStateModel.UserGridPageSize=!string.IsNullOrEmpty(dr["UserGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["UserGridPageSize"].ToString()):0;
                    _userStateModel.ClientGridPageSize=!string.IsNullOrEmpty(dr["ClientGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["ClientGridPageSize"].ToString()):0;
                     _userStateModel.PointGridPageSize=!string.IsNullOrEmpty(dr["PointGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["PointGridPageSize"].ToString()):0;
                       _userStateModel.ClassGridPageSize=!string.IsNullOrEmpty(dr["ClassGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["ClassGridPageSize"].ToString()):0;
                     _userStateModel.AlertGridPageSize=!string.IsNullOrEmpty(dr["AlertGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["AlertGridPageSize"].ToString()):0;
                    _userStateModel.ImageGridPageSize=!string.IsNullOrEmpty(dr["ImageGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["ImageGridPageSize"].ToString()):0;
                     _userStateModel.StatusGridPageSize=!string.IsNullOrEmpty(dr["StatusGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["StatusGridPageSize"].ToString()):0;
                    _userStateModel.NewsGridPageSize=!string.IsNullOrEmpty(dr["NewsGridPageSize"].ToString())
                        ?Convert.ToInt32(dr["NewsGridPageSize"].ToString()):0;
                    _userStateModel.HolidayGridPageSize = !string.IsNullOrEmpty(dr["HolidayGridPageSize"].ToString())
                        ? Convert.ToInt32(dr["HolidayGridPageSize"].ToString()) : 0;
                }
                
            }
            catch (Exception exception) { 
                //
            }

            return _userStateModel;
        }
    }
}
