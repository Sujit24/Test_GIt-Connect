/* Global Variables */

var currentPopupInitiator = null;

/********************/

function cancel() {
    history.back();
}

function redirect(url) {
    window.location = url;
}

function submitForm(successHandler) {
    if ($(this).valid()) {
        showBusyScreen();

        $.ajax({
            url: $(this).attr("action"),
            global: false,
            type: "POST",
            cache: false,
            data: $(this).serialize(),
            dataType: "json",
            success: function () {
                hideBusyScreen();

                if ($.isFunction(successHandler)) {
                    successHandler();
                } else {
                    alert("Form successfully submitted!");
                    history.back();
                }
            }
        });
    }
    return false;
}

function resetForm(form) {
    form.each(function () {
        this.reset();
    });
}

//function createKendoGrid(grid, columns, dataUrl, onDataBound) {
//    grid.kendoGrid({
//        dataSource: {
//            transport: {
//                read: {
//                    type: "POST",
//                    url: function () {
//                        return dataUrl;
//                    },
//                    dataType: "json"
//                }
//            },
//            pageSize: 6
//        },
//        dataBound: function (e) {
//            if (onDataBound) onDataBound(this);
//        },
//        columns: columns,
//        pageable: true,
//        sortable: true
//    });
//}

function loadPopupGrid(sender, onDataBound) {
    currentPopupInitiator = sender;
    var popupInitiator = $(currentPopupInitiator);

    $("#popupGrid").kendoGrid().empty();

    $.ajax({
        url: popupInitiator.attr("columns-url"),
        type: "POST",
        dataType: "json",
        success: function (columns) {
            createKendoGrid($("#popupGrid"), columns, popupInitiator.attr("data-url"), onDataBound);
        }
    });

    $("#popupGridContainer h3").html(popupInitiator.attr("title"));
    $("#popupGridContainer").modal("show");
}

$("#popupGrid .popupSeletedRow").live("click", function (e) {
    e.preventDefault();

    var grid = $("#popupGrid").data("kendoGrid");
    var rowData = grid.dataItem($(this).closest("tr"));
    var popupInitiator = $(currentPopupInitiator);

    var valueMember = popupInitiator.attr("value-member-class");
    var displayMember = popupInitiator.attr("display-member-class");

    popupInitiator.closest("form").find("." + valueMember).val(rowData[valueMember]);
    popupInitiator.closest("form").find("." + displayMember).val(rowData[displayMember]);

    $("#popupGridContainer").modal("hide");
});

function loadFormData(form, obj) {
    form.find('input, select').each(function (i, field) {
        var name = getItemClassPropertyName(field.name);
        var ele = $(field);

        if (ele.attr("type") == "checkbox") {
            if (obj[name] == "True") {
                ele.attr("checked", "checked");
                //ele.attr("value", "true");
            }
            else {
                ele.removeAttr("checked");
                //ele.attr("value", "false");
            }
        }
        else {
            ele.val(obj[name]);
        }
    });
}

function removeFromList(list, primaryKeyName, primaryKeyValue) {
    $.each(list, function (index, result) {
        if (result[primaryKeyName] == primaryKeyValue) {
            list.splice(index, 1);
            return false;
        } else {
            return true;
        }
    });
}

function getKendoGridViewData(grid) {
    return grid.data("kendoGrid")._data;
}

function getCheckBoxListObject(section) {
    var listObject = new Array();
    var rows = section.find(".checkBoxRow");

    $.each(rows, function () {
        var inputs = $(this).find("input");
        var rowObject = {};

        $.each(inputs, function () {
            rowObject[getItemClassPropertyName(this.name)] = this.value;

            if (this.type == "checkbox" && !($(this).is(":checked"))) {
                rowObject[getItemClassPropertyName(this.name)] = null;
                rowObject.Deleted = "1";
            }
        });

        if (!(rowObject.Deleted == "1" && rowObject.PrimaryKey == "0")) {
            listObject.push(rowObject);
        }
    });

    return listObject;
}

function deleteCheckboxListItem(checkboxItem) {
    checkboxItem = $(checkboxItem);

    var inputDeleted = checkboxItem.parent().find(".checkBoxListDeleted");
    if (inputDeleted.length != 0) {
        if (checkboxItem.is(":checked")) {
            inputDeleted.val("0");
        }
        else {
            inputDeleted.val("1");
        }
    }
}

function setDropdowTextForDetail(dropdown) {
    dropdown = $(dropdown);

    var hiddenInput = dropdown.parent().find("input[type=hidden]");
    if (hiddenInput.length != 0 && dropdown.val() != "0") {
        hiddenInput.val(dropdown.find("option:selected").text());
    }
    else {
        hiddenInput.val(null);
    }
}

function checkAllOrNone(checkbox) {
    checkbox = $(checkbox);

    $.each(checkbox.parent().parent().find("input[type=checkbox]"), function (e) {
        if (checkbox.is(":checked")) {
            $(this).attr("checked", "checked");
        } else {
            $(this).removeAttr("checked");
        }
    });
}

$(function () {
    /*$(".datepicker").datepicker({
    showOn: "button",
    changeMonth: true,
    changeYear: true,
    buttonImage: "/Content/images/calendar.png",
    buttonImageOnly: true
    });

    $("input[type=checkbox]").click(function () {
    var ele = $(this);

    if (ele.is(":checked")) {
    ele.attr("checked", "checked");
    ele.val("True");

    $('input[name="' + ele.attr('name') + '"]').filter('[type=hidden]').val("True");
    }
    else {
    ele.removeAttr("checked");
    ele.val("False");

    $('input[name="' + ele.attr('name') + '"]').filter('[type=hidden]').val("False");
    }
    });*/

    $(".btn input[type=radio]").click(function () {
        var _this = $(this);
        var optionName = _this.attr("name");

        $("input[name=" + optionName + "]").closest("label").removeClass("active");
        _this.closest("label").addClass("active");
    });

    $("input[type=text]").attr("autocomplete", "off");
});

function showBusyScreen() {
    $(".busy-loading-image-container").show();
}

function hideBusyScreen() {
    $(".busy-loading-image-container").hide();
}

/*$.validator.addMethod(
"regex",
function (value, element, regexp) {
var re = new RegExp(regexp);
return this.optional(element) || re.test(value);
},
function (regexp, element) {
var customErrorMessage = $(element).attr("regex-error-message");

if (customErrorMessage != undefined && customErrorMessage != '')
return customErrorMessage;
return "Please check your input.";
}
);*/

function customClientValidate(form) {
    var isValid = form.valid();
    if (!isValid) {
        $('html, body').animate({
            scrollTop: $(".error").eq(0).offset().top
        }, 1000);
    }

    return isValid;
}
/*
function ilchide() {
$("#panel").animate({ marginRight: "-260px" }, 260).hide("slow");
$("#right-menu").animate({ width: "0px", opacity: 0 }, 400);
$("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
$("#content").animate({ marginRight: "0" }, 500);
}
function ilcshow() {
//$("#content").animate({marginRight:"260px"}, 200);

$("#right-menu").animate({ width: "260px", opacity: 1 }, 400);
$("#showPanel").animate({ width: "0px", opacity: 0 }, 600).hide("slow");
$("#panel").animate({ marginRight: "0px" }, -260).show("slow");
}
jQuery(document).ready(function () { //ilchide();
$("#hidePanel").click(function () {
ilchide();
});
$("#showPanel").click(function () {
ilcshow();
});
});

*/
function ilchide() {
    $("#panel").animate({ marginRight: "-260px" }, 500);
    $("#right-menu").animate({ width: "0px", opacity: 0 }, 400);
    $("#showPanel").show("normal").animate({ width: "28px", opacity: 1 }, 200);
    $("#content").animate({ marginRight: "0" }, 500);
}

function ilcshow() {
    //$("#content").animate({marginRight:"260px"}, 200);
    $("#panel").animate({ marginRight: "0px" }, 400);
    $("#right-menu").animate({ width: "260px", opacity: 1 }, 400);
    $("#showPanel").animate({ width: "0px", opacity: 0 }, 600).hide("slow");
}


jQuery(document).ready(function () { //ilchide();
    $("#headerSelectedClient").html("Client: " + get_cookie("strClientName"));

    $("#hidePanel").click(function () {
        ilchide();
    });
    $("#showPanel").click(function () {
        ilcshow();
    });
});

function ShowLoadingImg() {
    var loadingdiv = document.getElementById('loading');
    loadingdiv.style.display = "block";
    document.getElementById('backgroundElement').style.display = "block";
}

function CloseLoadingImg() {
    var loadingdiv = document.getElementById('loading');
    loadingdiv.style.display = "none";
    document.getElementById('backgroundElement').style.display = "none";
}

function scrollToDetailContainer() {
    $('html, body').animate({
        scrollTop: $("#detail-container").offset().top
    }, 1000);
}


function GetCookieValue(vCookieName) {
    var vCookieValue = "";
    if ((vCookieName != null) && (vCookieName.length > 0)) {
        var vCookie;
        vCookie = document.cookie;

        //First check that there is a cookie with the name in vCookieName
        if (vCookie.indexOf(vCookieName) != -1) {
            vCookieName = vCookieName + '=';

            var vStartingIndex;
            var vEndingIndex;

            //Retrieve the starting and ending index of the cookie value
            vStartingIndex = vCookie.indexOf(vCookieName);

            if (vStartingIndex > -1) {
                vStartingIndex = vStartingIndex + vCookieName.length;
                vEndingIndex = vCookie.indexOf(";", vStartingIndex);

                if (vEndingIndex < vStartingIndex) {
                    vEndingIndex = vCookie.length;
                }
            }
            //Retrieve the cookie value
            vCookieValue = unescape(vCookie.substring(vStartingIndex, vEndingIndex));
        }
    }
    //alert(vCookieValue);
    return vCookieValue;
}


//A function to retrieve a cookie
function get_cookie(cookie_name) {
    var results = document.cookie.match('(^|;) ?' + cookie_name + '=([^;]*)(;|$)');

    if (results)
        return (unescape(results[2]));
    else
        return null;
}
//A function to delete a cookie
function delete_cookie(cookie_name) {
    var cookie_date = new Date();  // current date & time
    cookie_date.setTime(cookie_date.getTime() - 1);
    document.cookie = cookie_name += "=; expires=" + cookie_date.toGMTString();
}

function set_cookie(cookie_name, cookie_value) {
    //document.cookie = "strClientName=" + $("#ddlClient option:selected").text();
    document.cookie = cookie_name + "=" + cookie_value + "; path=/";
}

//function IsLoggedoutMessage(response) {
//    var isLoggedout = false;
//    if (response == "Please sign in.") { //logged our redirect o login page
//        isLoggedout= true;
//    }
//    return isLoggedout;
//}
//function RedirectToLoginPage() {
//    var urlOfLoginpage = baseUrl + "/Login/index";
//    redirect(urlOfLoginpage)
//}


function IsInternetExplorer() {
    if (!!navigator.userAgent.match(/Trident\/7\./))
        return true;
    else
        return false;
}

function ShowLoadingMessage(message) {
    $("#loadingInfoSH b").text(message);
    $("#loadingInfoSH").attr("style", "padding-left:" + "-" + ($("#loadingInfoSH b").width() / 2 - 23) + "px")
    $("#loading").attr("style", "margin-left:" + "-" + ($("#loadingInfoSH b").width() / 2 - 23) + "px")

    $("#loadingInfoSH").show();
}

function HideLoadingMessage() {
    $("#loadingInfoSH").hide();
}

function LoadStateDDL(country, ddlState) {
    ddlState.empty();

    $.each(salesTaxList, function (index, item) {
        if (item.Country == country) {
            ddlState.append("<option value='" + item.StateShortName + "'>" + item.StateShortName + "</option>");
        }
    });
}