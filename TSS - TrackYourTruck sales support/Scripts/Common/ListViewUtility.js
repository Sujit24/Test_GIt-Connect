var attribName = '';
/*var userStateModel = null;
$(function () {
    userStateModel = new UserStateModel();
    userStateModel.urlRoot = baseUrl + "/UserState/MaintainUserStateGridPage/";
    userStateModel.fetch({ cache: null, async: false, data: $.param({ loadType: 'default' }) });
//    set_cookie('TruckGridPageSize',userStateModel.get('TruckGridPageSize'));
//    set_cookie('TerminalGridPageSize',userStateModel.get('TerminalGridPageSize'));
//    set_cookie('UserGridPageSize',userStateModel.get('UserGridPageSize'));
//    set_cookie('ClientGridPageSize',userStateModel.get('ClientGridPageSize'));
//    set_cookie('PointGridPageSize',userStateModel.get('PointGridPageSize'));
//    set_cookie('ClassGridPageSize', userStateModel.set('ClassGridPageSize'));
//    set_cookie('AlertGridPageSize', userStateModel.set('AlertGridPageSize'));
//    set_cookie('StatusGridPageSize', userStateModel.set('StatusGridPageSize'));
//    set_cookie('NewsGridPageSize', userStateModel.set('NewsGridPageSize'));
//    set_cookie('HolidayGridPageSize', userStateModel.set('HolidayGridPageSize'));
})*/


if (!ListViewUtility) {
    var ListViewUtility = {
        _RunningEventsCount: 0,
        ShowLoadingImage: function () {
            this._RunningEventsCount++;
            if (this._RunningEventsCount <= 1) {
                ShowLoadingImg();
            }
        },
        CloseLoadingImage: function () {
            this._RunningEventsCount--;
            if (this._RunningEventsCount < 1) {
                CloseLoadingImg();
                this._RunningEventsCount = 0;
            }
        },
        CloseUnhandledLoader: function () {
            if (ListViewUtility._RunningEventsCount > 0) {
                window.setTimeout(function () {
                    ListViewUtility._RunningEventsCount = 0;
                    ListViewUtility.CloseLoadingImage();
                }, 200);
            }
        }
    };
}

var createKendoGridResponseDateFn;
function createKendoGrid(columns, dataUrl, isPostData, recordPerPage, successFn, attributeName, settings) {
    var postData;
    attribName = attributeName;

    var PageSize = 10; //userStateModel.get(attribName);
    
    if (recordPerPage == undefined) {
        recordPerPage = 6;
    }
    else if (PageSize == "0") {
        recordPerPage = 10;
    }
    else {
        recordPerPage =Number(PageSize);
    }

    if (PageSize == "1000000") {
        PageSize = "ALL";
    }

    if (isPostData == true) {
        postData = $("#searchForm").serialize();
    }

    //$("#divKendoGrid").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                url: function () {
                    return dataUrl;
                },
                success: function (data) {
                    //console.log("success");
                }
            },
            parameterMap: function (data, type) {
                return postData;
            }
        },
        requestStart: function () {
            //kendo.ui.progress($("#loading"), true);           
            //ListViewUtility.ShowLoadingImage();
        },
        requestEnd: function () {
            //kendo.ui.progress($("#loading"), false);   
            //            try {
            //                $.ajax({
            //                    url: baseUrl + "/Client/GetClientList/",
            //                    type: 'GET',
            //                    async: false,
            //                    success: function (data) {
            //                        var clientOption = "";
            //                        clientOption = clientOption + "<option value='0'></option>";

            //                        $.each(data, function (index) {
            //                            clientOption = clientOption + "<option class='option' value='" + data[index].ClientID + "'>" + data[index].ClientName + "</option>";
            //                        });

            //                        $("#ddlClient").html(clientOption);

            //                        MaintainUserState();
            //                    },
            //                    error: function (result) {
            //                        alert("error occurred");
            //                    }
            //                });
            //            }
            //            catch (err) {
            //                //$("#lblNumericPhone").css("display", "block");
            //            }        
            /*try {
            $.ajax({
            url: baseUrl + "/Client/GetClientList/",
            type: 'GET',
            async: false,
            success: function (data) {
            var clientOption = "";
            clientOption = clientOption + "<option value='0'></option>";

            $.each(data, function (index) {
            clientOption = clientOption + "<option class='option' value='" + data[index].ClientID + "'>" + data[index].ClientName + "</option>";
            });

            $("#ddlClient").html(clientOption);

            MaintainUserState();
            },
            error: function (result) {
            alert("error occurred");
            }
            });
            }
            catch (err) {
            //$("#lblNumericPhone").css("display", "block");
            }*/
            //ListViewUtility.CloseLoadingImage();
        },
        change: function (e) {
            if (createKendoGridResponseDateFn) { createKendoGridResponseDateFn(this.data()); }
            if ($('#hdnGridDataCount').length > 0) {
                $('#hdnGridDataCount').val(this.data().length);
                if (successFn) { successFn() }
            }
            if ($('.selectAll').is(":checked"))
                $('.selectAll').attr("checked", false);
            //Close those loader which has default kendo requestStarter but no requestEnder
            ListViewUtility.CloseUnhandledLoader();
        } //,
        //pageSize: recordPerPage
    });

    $("#divKendoGrid").kendoGrid({
        dataSource: gridDataSource,
        columns: eval(columns),
        //        pageable: {
        //            pageSizes: [10, 20, 50, 100]
        //        },
        sortable: true,
        scrollable: true,
        dataBound: function () {
            addExtraStylingToGrid(this);

            if (settings != undefined && settings != null) {
                if (settings.onDataBound != undefined && settings.onDataBound != null) {
                    settings.onDataBound();
                }
            }
        },
        height: 685
    });

    /*$("#pager").kendoPager({
        dataSource: gridDataSource,
        pageSizes: [10, 20, 50, 100],
        messages: {
            display: "{0} - {1} of {2} items",
            empty: "No items to display",
            page: "Page",
            of: "of {0}",
            itemsPerPage: "items per page",
            first: "Go to the first page",
            previous: "Go to the previous page",
            next: "Go to the next page",
            last: "Go to the last page"
        }
    });*/

    //var totalRecord = gridDataSource.total();
    //console.log(totalRecord);

    if (settings == undefined || settings.DisableCheckAll == undefined || (settings.DisableCheckAll != undefined && !settings.DisableCheckAll)) {
        var grid = $("#divKendoGrid").data("kendoGrid");

        if (grid.thead.find("th:first .selectAll").length == 0) {
            grid.thead.find("th:first").append($('<input class="selectAll" type="checkbox"/>')).delegate(".selectAll", "click", function () {
                var checkbox = $(this);
                grid.table.find("tr")
                    .find("td:first input")
                    .attr("checked", checkbox.is(":checked"))
                    .trigger("change");
            });
        }
    }

    if (settings != undefined && settings != null && settings.Sort != undefined && settings.Sort != null) {
        var grid = $("#divKendoGrid").data("kendoGrid");
        grid.dataSource.sort({ field: settings.Sort[0].field, dir: settings.Sort[0].dir });
    }

    /*var pager = $("#pager").data("kendoPager");
    var dropdown = pager.element
                     .find(".k-pager-sizes [data-role=dropdownlist]")
                     .data("kendoDropDownList");



    var isAllIncluded = false;
    var length = dropdown.dataSource.options.data.length;
    var item, i;
    for (i = length - 1; i >= 0; i--) {

        item = dropdown.dataSource.options.data[i];
        if (item[dropdown.options.dataTextField] == "ALL") {
            isAllIncluded = true;
            break;
        }

    }

    if (!isAllIncluded) {
        var item = {};
        item[dropdown.options.dataTextField] = "ALL";
        item[dropdown.options.dataValueField] = 1000000;
        dropdown.dataSource.add(item);
    }

    //if (PageSize == "ALL") {
    dropdown.select(function (dataItem) {
        return dataItem.text === PageSize;
    });
    //}

    /*dropdown.bind("change", function (e) {
        userStateModel.urlRoot = baseUrl + "/UserState/MaintainUserStateGridPage/";
        if (this.text() == "ALL") {
            userStateModel.set(attribName, 1000000);
            grid.one("dataBound", function () {
                setTimeout(function () {
                    dropdown.span.text("ALL");
                });
            });
        }
        else {
            userStateModel.set(attribName, this.text());
        }
        userStateModel.save(null, { async: true });
        // Close those loader which has default kendo requestStarter but no requestEnder
        ListViewUtility.CloseUnhandledLoader();
    });*/
}

function removeKendoGridRow(primaryKey, removeUrl, successFn) {
    $.ajax({
        url: removeUrl,
        type: "POST",
        dataType: "json",
        data: { PrimaryKey: primaryKey },
        success: function (result) {
            if (result.Message == "Successfully Deleted.") {
                //Delete row from grid
                //$('#' + 'delete' + primaryKey).parent().parent().remove();
            }
            if (successFn != undefined) {
                successFn(result.Message);
                //console.log('success-' + result.Message);
            }
        },
        error: function () {
            //return "Delete failed.";
            if (successFn != undefined) {
                successFn("Delete failed.");
                //console.log('error-');
            }
        }
    });
}

//function removeKendoGridRow(primaryKey, removeUrl) {
//    $.ajax({
//        url: removeUrl,
//        type: "POST",
//        dataType: "json",
//        data: { PrimaryKey: primaryKey },
//        success: function (result) {
//            if (result.Message == "Successfully Deleted.") {
//                //Delete row from grid
//                $('#' + 'delete' + primaryKey).parent().parent().remove();
//            }
//        }
//    });
//}

/*function resetKendoGridData(columnsUrl, dataUrl) {
$("#divKendoGrid").kendoGrid().empty();

loadKendoGrid(columnsUrl, dataUrl, false);
}*/

function ConvertToDate(value) {
    var dateRegExp = /^\/Date\((.*?)\)\/$/;
    var date = dateRegExp.exec(value);
    var date2 = kendo.toString(new Date(parseInt(date[1])), "MM/dd/yyyy");
    if (date2 == "01/01/1900")
        return "";
    return new Date(parseInt(date[1]));
}

// defined function to add hover effect and remove it when row is clicked
isCollapsibleGrid = false;
addExtraStylingToGrid = function (context) {
    $("div.k-grid-content table tbody tr").hover(
          function () {
              $(context).toggleClass("k-state-hover");
          }

        );


    if (isCollapsibleGrid == true) {
       // $.browser.chrome = /chrom(e|ium)/.test(navigator.userAgent.toLowerCase());
        //transfer columnwidth to header
        //$.browser.mozilla
        //$.browser.chrome
        $(".k-grid-header-wrap table").css('width', "auto");
        $(".k-grid-header-wrap table").css('table-layout', "auto");
        $(".k-grid-content table").css('width', "auto");
        $(".k-grid-content table").css('table-layout', "auto");

        $(".k-grid-header-wrap table thead tr th").each(function (i, c) {
            $(c).css('width', "auto");
        });

        $(".k-grid-content table tbody tr:first td").each(function (i, c) {
            $(c).css('width', "auto");
        });

        if ($.browser.chrome || IsInternetExplorer()) {

            $(".k-grid-header-wrap table").css('display', 'none');
            $(".k-grid-header-wrap table").css('width');
            $(".k-grid-header-wrap table").css('display', 'table');

            $(".k-grid-content table").css('display', 'none');
            $(".k-grid-content table").css('width');
            $(".k-grid-content table").css('display', 'table');
        }

        var colWidthHeader = [];
        $(".k-grid-header-wrap table thead tr th").each(function (i, c) {
            if ($.browser.chrome || IsInternetExplorer()) {
                colWidthHeader[i] = Math.round(c.clientWidth);
            }
            else {
                colWidthHeader[i] = Math.round($(c).width());
            }
            //colWidthHeader[i] = Math.round(c.clientWidth);

        });

        var colWidthContent = [];
        $(".k-grid-content table tbody tr:first td").each(function (i, c) {
            if ($.browser.chrome || IsInternetExplorer()) {
                colWidthContent[i] = Math.round(c.clientWidth);
            }
            else {
                colWidthContent[i] = Math.round($(c).width());
            }
            //colWidthContent[i] = Math.round(c.clientWidth);
            //clientWidth,outerWidth,offsetWidth
        });



        var colWidth = [];
        $.each(colWidthContent, function (index, value) {
            colWidth[index] = colWidthContent[index] > colWidthHeader[index] ? colWidthContent[index] : colWidthHeader[index];
            //console.log(index + "-" + colWidthHeader[index] + "-" + colWidthContent[index]);
        });

        var total = 0;
        $.each(colWidth, function () {
            total += this;
        });

        $(".k-grid-header-wrap table").css('table-layout', "fixed");
        $(".k-grid-header-wrap table").css('width', total);

        $(".k-grid-header-wrap table thead tr th").each(function (i, c) {
            $(c).css('width', colWidth[i]);
        });

        $(".k-grid-content table").css('table-layout', "fixed");
        $(".k-grid-content table").css('width', total);
        $(".k-grid-content table tbody tr:first td").each(function (i, c) {
            $(c).css('width', colWidth[i]);
        });


        //console.log(total);

    }
};


/*$(document).ready(function () {
    if ($('#ddlClient').length > 0) {
        try {
            $.ajax({
                url: baseUrl + "/Client/GetClientList/",
                type: 'GET',
                async: true,
                success: function (data) {
                    var clientOption = "";
                    clientOption = clientOption + "<option value='0'></option>";

                    $.each(data, function (index) {
                        clientOption = clientOption + "<option class='option' value='" + data[index].ClientID + "'>" + data[index].ClientName + "</option>";
                    });

                    $("#ddlClient").html(clientOption);

                    MaintainUserState();
                },
                error: function (result) {
                    alert("error occurred");
                }
            });
        }
        catch (err) {
            //$("#lblNumericPhone").css("display", "block");
        }
    }

});*/

function MaintainUserState() {
    var postData;
    var clientId = 0;
    if ($('#ddlClient').length > 0) {
        clientId = $('#ddlClient').val();
        /*if (clientId == '0') {
        return;
        }*/
        try {
            $.ajax({
                url: baseUrl + "/UserState/MaintainUserState/" + clientId,
                type: 'GET',
                async: true,
                success: function (data) {
                    $('#ddlClient').val(data.ClientId);
                    $("#headerSelectedClient").html("Client: " + $("#ddlClient option:selected").text());
                    set_cookie("strClientName", $("#ddlClient option:selected").text());
                },
                error: function (result) {
                    alert("error occurred");
                }
            });
        }
        catch (err) {
            //$("#lblNumericPhone").css("display", "block");
        }
    }
}


function SaveUserState() {
    var postData;
    var clientId = 0;
    if ($('#ddlClient')) {
        clientId = $('#ddlClient').val();
        if (clientId == '0') {
            $('#ddlClient').val(get_cookie("strClientId"));
            $("#headerSelectedClient").html("Client: " + get_cookie("strClientName"));
            return;
        }
        try {
            $.ajax({
                url: baseUrl + "/Client/UpdateWebSession/0",
                type: 'PUT',
                data: { ClientId: clientId },
                async: false,
                success: function (result) {
                    RefreshGridFromDatabase();
                },
                error: function (result) {
                    alert("error occurred");
                }
            });
        }
        catch (err) {
            //$("#lblNumericPhone").css("display", "block");
        }
    }
}


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