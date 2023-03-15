$(function () {
    $(".btnSave").click(function (e) {
        var context = $(this).parent().parent();

        if (validation(context)) {
            var postData = [];

            $.ajax({
                url: baseUrl + "/Settings/Save",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify({
                    SettingsName: context.find('.SettingsName').val(),
                    SettingsValue: context.find('.SettingsValue').val()
                }),
                success: function (result) {
                    if (result.Message == "Successfully Saved.") {
                        alert("Successfully Saved.");
                    }
                    else {
                        alert("Save failed.");
                    }
                }
            });
        }
    });
});

function validation(context) {
    var message = "";
    var isValid = true;
    var settingLabel = context.attr('setting-label');
    var settingValueType = context.attr('setting-value-type');
    var eleSettingsValue = context.find('.SettingsValue');
    var settingsValue = eleSettingsValue.val();

    if (settingsValue.trim() == "") {
        message += "* Please enter " + settingLabel + ".\n";
        isValid = false;
    }

    if (settingValueType == "number") {
        if (!isNumber(settingsValue)) {
            message += "* Invalid " + settingLabel + ".\n";
            eleSettingsValue.focus();
            isValid = false;
        }
    }

    if (!isValid) {
        alert(message);
    }

    return isValid;
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}