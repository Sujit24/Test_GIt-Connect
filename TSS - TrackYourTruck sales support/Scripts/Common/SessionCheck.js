var SecondsToCheckSession = 10;
setTimeout(CheckSessionAndTakeAction, SecondsToCheckSession * 1000);

function CheckSessionAndTakeAction() {
    try {

        if (get_cookie("strTSSSessionId") == null || get_cookie("strTSSSessionId") == '') {

            var html = "<div id='dvSessionOutInfodialog' class='modal-popup' style=' font-family: arial,helvetica,clean,sans-serif;display: none; width: 315px;border: 8px solid #909090;border-radius: 10px 10px 10px 10px;padding:0px;'>" +
    "<div class='modal-container'>" +
        "<div class='mpModalHeader'>" +
            "<span id='lblPopupHeader' class='modal-msg' style='padding-left: 7px; color: #FFFFFF;" +
                "font-size: 16px;'>Not Logged In / Session Timeout</span>" +
        "</div>" +
        "<div >" +
            "<div class='modalBody' style='height: auto; overflow: auto;padding: 0px;overflow: hidden;'>" +
                "<div style='height: 28px;background-color: #FFFFFF;padding-top: 10px;'>" +
                    "<div style='float: left; margin-left: 19px;font-size: 14px;'>" +
                        "Please log in to continue." +
                    "</div>" +
                "</div>" +
                "<div style='height: 44px; font-size: 12px;  background: none repeat scroll 0 0 #F2F2F2;'>" +
                "<div style='clear: both;width: 100%;' class='dialog_buttons clearfix'>" +
                    "<div class='rfloat mlm' style='padding-top:10px;'>" +
                        "<label class='uiButton uiButtonLarge uiButtonConfirm' style='text-align: center;background-color: #5B74A8;height: auto;float:right; border-style: solid;" +
                            "border-width: 1px;box-shadow: 0 1px 0 rgba(0, 0, 0, 0.1);line-height: 16px;margin-right: 19px;  border-style: outset;margin-bottom: 10px;'>" +
                            "<input type='button' onclick='javascript:GoToLoginPage()' style='border:0;background: none repeat scroll 0 0 rgba(0, 0, 0, 0);color:white;width:60px;height:22px;margin: 2px;' name='login' value='Log In'>" +
                        "</label>" +
                    "</div>" +
                "</div>" +
                "</div>" +
            "</div>" +
        "</div>" +
    "</div>" +
"</div>";



            $(html).appendTo(document.body);

            $("#dvSessionOutInfodialog").showModal();
        }
        else {
            setTimeout(CheckSessionAndTakeAction, SecondsToCheckSession * 1000);
        }
    }
    catch (e) {
        setTimeout(CheckSessionAndTakeAction, SecondsToCheckSession * 1000);
    }

}


function GoToLoginPage() {
    var urlOfLoginpage = baseUrl + "/Login/index";
    window.location = urlOfLoginpage;
}