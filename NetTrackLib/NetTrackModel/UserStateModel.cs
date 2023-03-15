using System;
using System.Collections.Generic;

namespace NetTrackModel
{
    public class UserStateModel
    {
        // user state property
        public int SessionId { get; set; }
        public int ClientId { get; set; }
        public int StateId { get; set; }
        public int UserId { get; set; }
        public string IsHeaderView { get; set; }
        public string IsFooterView { get; set; }
        public string IsRightMenu { get; set; }
        public string IsLeftMapView { get; set; }
        public string IsShowLeftMapView { get; set; }
        public string IsTruckStatus { get; set; }
        public string IsLegends { get; set; }
        public string IsGridView { get; set; }
        public string IsTruckDetail { get; set; }
        public int GridViewPagingIndex { get; set; }
        public string ActiveButton { get; set; }
        public int MapZoomLevel { get; set; }
        public double MapCenterLat { get; set; }
        public double MapCenterLng { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string DbAction { get; set; }
        public List<LeftMapModel> LeftMapModelList { get; set; }
        public string MiniMaps { get; set; }
        public string PointId { get; set; }
        public string FollowMe { get; set; }
        public string IsAdmin { get; set; }
        public string LoadType { get; set; }
        public int OldClientId { get; set; }
        public string Login { get; set; }
        public int MapRefresh { get; set; }
        public int PreferredMap { get; set; }
        public int TruckGridPageSize { get; set; }
        public int TerminalGridPageSize { get; set; }
        public int UserGridPageSize { get; set; }
        public int ClientGridPageSize { get; set; }
        public int PointGridPageSize { get; set; }
        public int ClassGridPageSize { get; set; }
        public int AlertGridPageSize { get; set; }
        public int ImageGridPageSize { get; set; }
        public int StatusGridPageSize { get; set; }
        public int NewsGridPageSize { get; set; }
        public int HolidayGridPageSize { get; set; }
        public int HaveGoogleRouting { get; set; }

        // default constructor
        public UserStateModel()
        {

        }
        public UserStateModel(int stateId, int userId, string isHeaderView, string isFooterView, string isRightMenu, string isLeftMapView,
            string isShowLeftMapView, string isTruckStatus, string isLegends, string isGridView, string isTruckDetail, int gridViewPagingIndex,
            string activeButton, int mapZoomLevel, double mapCenterLat, double mapCenterLng, DateTime entryDate, DateTime changeDate, int clientId, string isAdmin)
        {
            this.StateId = stateId;
            this.UserId = userId;
            this.IsHeaderView = isHeaderView;
            this.IsFooterView = isFooterView;
            this.IsRightMenu = isRightMenu;
            this.IsLeftMapView = isLeftMapView;
            this.IsShowLeftMapView = isShowLeftMapView;
            this.IsTruckStatus = isTruckDetail;
            this.IsLegends = isLegends;
            this.IsGridView = isGridView;
            this.IsTruckDetail = isTruckDetail;
            this.GridViewPagingIndex = gridViewPagingIndex;
            this.ActiveButton = activeButton;
            this.MapZoomLevel = mapZoomLevel;
            this.MapCenterLat = mapCenterLat;
            this.MapCenterLng = mapCenterLng;
            this.EntryDate = entryDate;
            this.ChangeDate = changeDate;
            this.ClientId = clientId;
            this.IsAdmin = isAdmin;
        }
    }
    public class LeftMapModel
    {
        //public int StateId { get; set; }
        //public int UserId { get; set; }
        public int TruckId { get; set; }
        public int OrderId { get; set; }

        public LeftMapModel()
        {

        }
        public LeftMapModel(int truckId, int orderId)
        {
            this.TruckId = truckId;
            this.OrderId = orderId;
        }
    }
}
