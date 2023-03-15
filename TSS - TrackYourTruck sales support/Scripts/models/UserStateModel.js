//UserState Model
var UserStateModel = Backbone.Model.extend({
    newTab: null,
    initialize: function () {
        //this.bind('change', this.onModelChange);
    },
    onModelChange: function (option) {
        var isSuccessFull;
        this.save(null, {
            async: false,
            success: function (model, response) {
                isSuccessFull = true;
            },
            error: function (model, response) {
                isSuccessFull = false;
            }
        });
        return isSuccessFull;
    },
    onLogout: function () {
        try {
            $.ajax({
                url: baseUrl + "/Login/Logout2",
                type: 'POST',
                async: false,
                data: {},
                success: function (result) {
                    if (result.Message == "Success") {
                        window.location = baseUrl + "/Login";
                    }
                }
            });
        }
        catch (err) {
        }
    },
    defaults: {
        StateId: 0,
        UserId: 0,
        IsHeaderView: '1',
        IsFooterView: '1',
        IsRightMenu: '1',
        IsLeftMapView: '0',
        IsShowLeftMapView: '0',
        IsTruckStatus: '0',
        IsLegends: '0',
        IsGridView: '0',
        IsTruckDetail: '0',
        GridViewPagingIndex: 0,
        ActiveButton: "",
        MapZoomLevel: 0,
        MapCenterLat: 0.0,
        MapCenterLng: 0.0,
        EntryDate: '1/1/1999',
        ChangeDate: '1/1/1999',
        ClientId: 0,
        OldClientId: 0,
        PointId: '',
        IsAdmin: '0',
        TruckGridPageSize : 0 ,
        TerminalGridPageSize : 0 ,
        UserGridPageSize : 0 ,
        ClientGridPageSize : 0 ,
        PointGridPageSize : 0 ,
        ClassGridPageSize : 0 ,
        AlertGridPageSize : 0 ,
        ImageGridPageSize : 0 ,
        StatusGridPageSize : 0 ,
        NewsGridPageSize : 0 ,
        HolidayGridPageSize : 0 ,
        HaveGoogleRouting:0
    },
    idAttribute: "UserId",
    urlRoot: baseUrl + "/UserState/MaintainUserState/"
});
//End of UserState Model
