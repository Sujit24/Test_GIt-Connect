//UserState Model
var UserSessionModel = Backbone.Model.extend({
    initialize: function () {
        //this.bind('change', this.onModelChange);
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
                                            
                        var url = baseUrl + "/SessionEnd.cshtml";
                        winUrl = window.open(url, '_self', 'location=no,menubar=no,resizable=no,scrollbars=yes,status=yes,toolbar=no,left=0, top=0'); //'location=no,toolbar=no,statusbar=no');
                        document.cookie = "winUrl=" + baseUrl + "/Login";
                        
                        //window.close();
                        //Local storage
                        //Storage.set('myObj', winUrl);

                        //var winObject = { 'winUrl': winUrl };
                        // Put the object into storage
                        //localStorage.setItem('winObject', JSON.stringify(winObject));
                        // Retrieve the object from storage
                        //var retrievedObject = localStorage.getItem('testObject');
                        //self.close();
                    }
                }
            });
        }
        catch (err) {
        }
    },
    defaults: {
        SessionId: 0,
        UserId: 0,
        SessionAlive: ''
    },
    idAttribute: "UserId",
    urlRoot: baseUrl + "/User/GetUserSessionStatus/"
});
//End of UserState Model
