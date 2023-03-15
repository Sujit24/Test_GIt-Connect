var columns = '[';
columns += '{"template": "\u003cinput id=\u0027delete${ QuoteOrderId }\u0027 value=\u0027${ QuoteOrderId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
columns += '{"title":" ", "width":"190px", "template": kendo.template($("#action-template").html())},';
columns += '{"title":"Purchase Date (CST)", "width":"140px", "field":"PurchaseDateFormated", "template": kendo.template($("#purchaseDate-template").html()) }, ';
columns += '{"title":" ", "width":"40px", "template": kendo.template($("#note-template").html())},';
columns += '{"field":"QuoteId", "width":"60px", "title":"Order Id","template":"\u003ca href=\u0027javascript:EditOrder(${ QuoteOrderId })\u0027\u003e${ QuoteId }\u003c/a\u003e"},';
columns += '{"field":"CustomerName", "width":"200px", "title":"Customer Name"},';
columns += '{"field":"OrderTypeTitle", "width":"175px","title":"Order Type"},';
columns += '{"title":"NetTrack Status", "width":"200px", "field":"NettrackStatus" },';
//columns += '{"title":"Order Status", "width":"200px", "field":"StatusTitle", "template":"\u003ca class=\u0027btn btn-info\u0027 style=\u0027width: 85%\u0027 onMouseEnter=\u0027javascript:ShowOrderStatusTooltip(this, ${ QuoteOrderId })\u0027 onMouseLeave=\u0027javascript:HideOrderStatusTooltip(this)\u0027 href=\u0027javascript:ChangeOrderStatus(${ QuoteOrderId }, ${ OrderStatusId })\u0027\u003e${ StatusTitle }\u003c/a\u003e"  },';
columns += '{"title":"Order Status", "width":"257px", "field":"StatusTitle", "template": kendo.template($("#orderstatus-template").html())  },';
columns += '{"title":"Ship Method", "width":"100px", "template": kendo.template($("#shipmethod-template").html()) },';
columns += '{"title":" ", "width":"40px", "template": kendo.template($("#info-template").html())},';
columns += '{"title":"Method of Payment", "width":"150px", "template": kendo.template($("#payment-method-template").html()) },';
columns += '{"title":"BluePay Trans. No", "width":"110px", "field":"TransactionId" },';
columns += '{"title":"Last 4 of CC", "width":"80px", "field":"CardNumber" },';
columns += '{"title":"CC Exp Date", "width":"80px", "field":"CardExpire" }';
columns += ']';

function ChangeOrderStatus(quoteOrderId, orderStatusId) {
    ShowOrderStatusHistory(quoteOrderId);

    $("#changeOrderStatusModal").modal('show');
    $("#changeQuoteOrderId").val(quoteOrderId);
    $("#changeOrderStatusId").val(orderStatusId);

    $("[type=radio][value=" + orderStatusId + "]").attr("checked", "checked");
}

function PDF(quoteOrderId) {
    window.open(baseUrl + "/SalesOrderRpt/ShowSalesOrderRpt/" + quoteOrderId);
}

function GetQuoteOrderQueryString() {
    return "dateFrom=" + $("#dateFrom").val() +
        "&dateTo=" + $("#dateTo").val() +
        "&searchSelect=" + $("#searchSelect").val() +
        "&searchText=" + $("#searchText").val();
    
}

function SetQuoteOrderStatus() {
    $("#btnChangeOrderStatus").attr("disabled", "disabled");

    $.ajax({
        url: baseUrl + "/SalesOrder/SetQuoteOrderStatus",
        type: 'POST',
        async: false,
        dataType: "json",
        data: { QuoteOrderId: $("#changeQuoteOrderId").val(), OrderStatusId: $("[name=ChangeOrderStatus]:checked").val() },
        success: function (result) {
            $("#btnChangeOrderStatus").removeAttr("disabled");
            $("#search").trigger('click');
            alert("Status changed successfully!");
            $("#changeOrderStatusModal").modal('hide');
        }
    });
}

function LoadOrderList() {
    createKendoGrid(columns, baseUrl + "/SalesOrder/GetSalesList?" + GetQuoteOrderQueryString(), false, 10, null, null, {
        onDataBound: function () {
            $.each($('.order-status-color'), function (index, item) {
                item = $(item);

                var color = item.attr('data-row-color');

                if (color == 'red') {
                    item.closest('tr').attr('style', 'background-color:' + 'rgba(255, 0, 0, 0.3) !important');
                } else {
                    item.closest('tr').attr('style', 'background-color:' + color + ' !important');
                }
            });
        }
    });
}

$(function () {
    LoadOrderList();

    $("#search").click(function () {
        //applyFilter($("#searchSelect").val(), $("#searchText").val());
        if (validateSearch()) {
            LoadOrderList();
        }
    });

    $("#resetSearch").click(function () {
        $("#searchText").val('');
        LoadOrderList();
        //clearFilters();
    });

    $(".aSendUserUrl").on("click", function (e) {
        e.preventDefault();

    });

    /*$("#dateFrom").datepicker();
    $("#dateTo").datepicker();*/

    var dateFromDatePicker = new Pikaday({
        field: document.getElementById('dateFrom'),
        format: 'MM/DD/YYYY'
    });

    var dateToDatePicker = new Pikaday({
        field: document.getElementById('dateTo'),
        format: 'MM/DD/YYYY'
    });

    $.ajax({
        url: baseUrl + "/SalesOrder/GetOrderStatus",
        type: 'POST',
        async: false,
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, item) {
                $("#changeOrderStatus").append("<div><input type='radio' name='ChangeOrderStatus' value='" + item.value + "' /> " + item.keyfield + "</div>");
                //$("#orderStatus").append("<option value='" + item.value + "'>" + item.keyfield + "</option>");
            });
        }
    });

    $.ajax({
        url: baseUrl + "/Common/LoadDDLJson",
        type: 'POST',
        async: false,
        data: { 'spName': 'ug_TssOrderType' },
        success: function (result) {
            $("#ddlOrderType").append("<option value='0'> </option>");

            $.each(result, function (index, item) {
                $("#ddlOrderType").append("<option value='" + item.value + "'>" + item.keyfield + "</option>");
            });
        }
    });

    setInterval(LoadLastView, 15 * 60 * 1000);
});

function LoadLastView() {
    if (validateSearch()) {
        LoadOrderList();
    }

    $("#gridLastUpdate").text("Last Updated: " + moment().format('MM/DD/YYYY h:mm:ss A') + " (CST)");
}

function EditOrder(primaryKey) {
    //location.href = baseUrl + "/SalesOrder/Edit/" + primaryKey;

    ListViewUtility.ShowLoadingImage();

    ShowDetailPage();

    $("#quoteOrderId").val(primaryKey);
    Edit(primaryKey);

    ListViewUtility.CloseLoadingImage();
}

function New() {
    //location.href = baseUrl + "/SalesOrder";
}

function Cancel() {
    HideDetailPage();
}

function ShowDetailPage() {
    InitVar();
    ClearDetailForm();
    InitProductContainer();

    $("#detailPage").show();
    $('html, body').animate({
        scrollTop: $("#detailPage").offset().top - 10
    }, 1000);
}

function HideDetailPage() {
    $("#detailPage").hide();
    $('html, body').animate({
        scrollTop: $("#divOrderList").offset().top
    }, 1000);
}

function RemoveProduct(primaryKey) {
    if (confirm("Are you sure to delete this Sales Order?")) {
        $.ajax({
            url: baseUrl + "/SalesOrder/Delete",
            type: "POST",
            dataType: "json",
            data: { PrimaryKey: primaryKey },
            success: function (result) {
                if (result.Message == "Successfully Deleted.") {
                    alert("Successfully Deleted.");
                    $("#divKendoGrid").data("kendoGrid").dataSource.read();
                    $("#detail-container").html('');
                }
                else {
                    alert("This Sales  cannot be deleted");
                }
            },
            error: function (result) {
                alert(result.toString());
            }
        });
    }
}

function removeFromGrid() {
    var totalRecordToBeDeleted = $('.k-grid-content table input:checkbox:checked').length;
    var successfuldelete = "";
    var countnodeletes = 0;
    var countdeletes = 0;

    if (totalRecordToBeDeleted > 0) {
        if (confirm("Are you sure to delete " + totalRecordToBeDeleted + " Sales Order(s)?")) {
            $('.k-grid-content table input:checkbox:checked').each(function (index, element) {
                if (index == totalRecordToBeDeleted - 1) {//last record
                    removeKendoGridRow(this.value, baseUrl + "/SalesOrder/Delete",
                    function (successfuldelete) {
                        if (successfuldelete == "Delete failed.") {
                            countnodeletes++;
                        }
                        else {
                            countdeletes++;
                        }
                        if (countnodeletes > 0) {
                            alert(countnodeletes + " out of " + countdeletes + " Sales Order(s) are not drivers and cannot be deleted");
                        }
                        $("#divKendoGrid").data("kendoGrid").dataSource.read();
                    });
                }
                else { // all but last record
                    removeKendoGridRow(this.value, baseUrl + "/SalesOrder/Delete",
                    function (successfuldelete) {
                        if (successfuldelete == "Delete failed.") {
                            countnodeletes++;
                        }
                        else {
                            countdeletes++;
                        }
                    });
                }
            });
        }
    }
}

// applyFilter function accepts the Field Name and the new value to use for filter.
function applyFilter(filterField, filterValue) {

    // get the kendoGrid element.
    var gridData = $("#divKendoGrid").data("kendoGrid");

    // get currently applied filters from the Grid.
    var currFilterObj = gridData.dataSource.filter();

    // get current set of filters, which is supposed to be array.
    // if the oject we obtained above is null/undefined, set this to an empty array
    var currentFilters = []; // currFilterObj ? currFilterObj.filters : [];

    // iterate over current filters array. if a filter for "filterField" is already
    // defined, remove it from the array
    // once an entry is removed, we stop looking at the rest of the array.
    if (currentFilters && currentFilters.length > 0) {
        for (var i = 0; i < currentFilters.length; i++) {
            if (currentFilters[i].field == filterField) {
                currentFilters.splice(i, 1);
                break;
            }
        }
    }

    // if "filterValue" is "0", meaning "-- select --" option is selected, we don't 
    // do any further processing. That will be equivalent of removing the filter.
    // if a filterValue is selected, we add a new object to the currentFilters array.
    if (filterValue != "0") {
        currentFilters.push({
            field: filterField,
            operator: "contains",
            value: filterValue
        });
    }

    // finally, the currentFilters array is applied back to the Grid, using "and" logic.
    gridData.dataSource.filter({
        logic: "and",
        filters: currentFilters
    });

}

function clearFilters() {
    var gridData = $("#divKendoGrid").data("kendoGrid");
    gridData.dataSource.filter({});
}


function validateSearch() {
    var isValid = true;
    var message = "";

    if (!isValidDate($("#dateFrom").val())) {
        isValid = false;
        message += "* From date is invalid.\n"
    }

    if (!isValidDate($("#dateTo").val())) {
        isValid = false;
        message += "* From date is invalid.\n"
    }

    if (!isValid) {
        alert(message);
    }

    return isValid;
}

function isValidDate(value) {
    var date = value.split("/");
    var d = parseInt(date[1], 10),
    m = parseInt(date[0], 10),
    y = parseInt(date[2], 10);

    if (d > 31) return false;
    if (m > 12) return false;

    return !isNaN(new Date(y, m - 1, d));
}

function ExportToExcel() {
    if ($("#divKendoGrid").data("kendoGrid").items().length < 1) {
        alert("No data to export.");
    }
    else {
        var frmExportToExcel = $("#frmExportToExcel");
        frmExportToExcel.attr('target', '');
        frmExportToExcel.attr("action", baseUrl + "/SalesOrder/ExportToExcel");
        frmExportToExcel.submit();
    }
}

function ShowOrderStatusHistory(quoteOrderId) {
    var columns = '[';
    columns += '{"field":"StatusTitle","title":"Order Status"},';
    columns += '{"field":"StatusDateFormated", "title":"Status Date", "width":"135px"}';
    columns += ']';

    $("#orderStatusHistoryList").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                data: { id: quoteOrderId },
                url: function () {
                    return baseUrl + "/SalesOrder/GetOrderStatusHistoryList";
                },
                success: function (data) {
                    //console.log("success");
                }
            }
        }
    });

    $("#orderStatusHistoryList").kendoGrid({
        dataSource: gridDataSource,
        columns: eval(columns),
        sortable: true,
        scrollable: true,
        dataBound: function () {
            if ($("#orderStatusHistoryList .k-grid-content tr").length == 0) {
                $("#orderStatusHistoryList .k-grid-content").html("<br><b><center style='color:red'>No Order Status History</center></b><br>")
            }
        }
    });
}

function ShowOrderStatusTooltip(sender, quoteOrderId) {
    var context = $(sender);

    $.ajax({
        url: baseUrl + "/SalesOrder/GetOrderStatusHistoryListWithComment",
        type: "POST",
        async: false,
        dataType: "json",
        data: { id: quoteOrderId },
        success: function (result) {
            var template = "";
            var comment = result.Comment;
            result = result.Data;

            /*if (comment != '') {
                template += "<div style='color:black; font-size: 12px; text-align:justify; max-width: 300px; font-weight: bold'>Note: " + comment + "</div><br/>";
            }*/

            if (result.length > 0) {
                template += "<h3>";
                $.each(result, function (index, item) {
                    template += "<div style='color:black; font-size: 12px'>" + item.StatusTitle + ": " + item.StatusDateFormated + "</div>";
                });
                template += "</h3>";

                context.attr('data-original-title', template);
                context.tooltip({ title: template, placement: "bottom", trigger: "manual" });
                context.tooltip("show");
            }
        }
    });
}

function HideOrderStatusTooltip(sender) {
    var context = $(sender);
    context.tooltip("hide");
}

function ShowOrderStatusCommentTooltip(sender) {
    var context = $(sender);

    var template = "<div style='color:black; font-size: 12px; text-align:justify; max-width: 300px; font-weight: bold'>Note: " + context.attr("payment-comment") + "</div>";
    context.attr('data-original-title', template);
    context.tooltip({ title: template, placement: "top", trigger: "manual" });
    context.tooltip("show");
}

function HideOrderStatusCommentTooltip(sender) {
    var context = $(sender);
    context.tooltip("hide");
}

function ShowQuoteNoteTooltip(sender) {
    var context = $(sender);

    var template = "<div style='color:black; font-size: 12px; text-align:left; max-width: 300px; font-weight: bold'>Note: " + context.attr("note") + "</div>";
    context.attr('data-original-title', template);
    context.tooltip({ title: template, placement: "top", trigger: "manual" });
    context.tooltip("show");
}

function HideQuoteNoteTooltip(sender) {
    var context = $(sender);
    context.tooltip("hide");
}

function ShowShipping(quoteOrderId, quoteId) {
    $("#shippingModal").modal('show');
    $("#shippingOrderId").text(quoteId);
    $("#shippingQuoteOrderId").val(quoteOrderId);
    $("#divShipmentDetails").hide();

    $.ajax({
        url: baseUrl + "/SalesOrder/GetOrdeShipInfo",
        type: 'GET',
        async: false,
        dataType: "json",
        data: { id: quoteOrderId },
        success: function (result) {
            if (result != null) {
                $("#shipFromAddress1").text(result.ShipperAddress.ShipperName);
                $("#shipFromAddress2").text(result.ShipperAddress.ShipperAddressLine);
                $("#shipFromAddress3").text(result.ShipperAddress.ShipperCityStateZip + ", US");

                $("#shipFromAddress1_2").text(result.ShipperAddress.ShipperName);
                $("#shipFromAddress2_2").text(result.ShipperAddress.ShipperAddressLine2);
                $("#shipFromAddress3_2").text(result.ShipperAddress.ShipperCityStateZip2 + ", US");

                $("#shippToAddress1").text(result.QuoteOrder.ShipToCompanyName);
                $("#shippToAddress2").text(result.QuoteOrder.ShipToAddress1 + (result.QuoteOrder.ShipToAddress2 != "" && result.QuoteOrder.ShipToAddress2 != null ? ", " + result.QuoteOrder.ShipToAddress2 : ""));
                $("#shippToAddress3").text(result.QuoteOrder.ShipToCityStateZip + ", " + result.QuoteOrder.ShipToCountry);
                $("#shippToAddress4").text(result.QuoteOrder.ShipToPhone);

                //$("#shippingType").text(result.ShippingAndHandlingType);
                $("#shippingTotalWeight").val(result.QuoteOrder.TotalWeight);

                BindShippingTypeDdl(result.QuoteOrder.ShipToCountry);
                $("#ddlShippingType").val(result.QuoteOrder.ShippingAndHandlingType);

                var boxSize = result.QuoteOrder.ShippingBoxSize.split('x');
                $('#shippingLength').val(boxSize[0]);
                $('#shippingWidth').val(boxSize[1]);
                $('#shippingHeight').val(boxSize[2]);

                BindShippingHistoryGrid(quoteOrderId);
            }
        }
    });
}

function BindShippingHistoryGrid(quoteOrderId) {
    $("#shippingHistory").show();

    var columns = [
        { field: "CreatedDateFormated", title: "Created Date (CST)", width: "140px" },
        { field: "TrackingNo", title: "Tracking No", width: "135px", template: "<a target='_blank' href='https://www.ups.com/track?loc=en_US&tracknum=#:TrackingNo#&requester=WT/trackdetails'>#:TrackingNo#</a>" },
        { field: "ShippingType", title: "Shipping Type", width: "100px" },
        { field: "ShippingCost", title: "Cost", width: "60px", template: "$#:ShippingCost#" },
        { field: "Weight", title: "Weight", width: "50px" },
        { field: "SentEmailDateFormated", title: "Sent Email Date (CST)", width: "140px" },
        { field: "", title: "", width: "260px", template: kendo.template($("#shipping-history-action-template").html()) },
        //{ field: "", title: "", width: "135px", template: "<a >" }
    ];

    $("#shippingHistoryGrid").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                data: { id: quoteOrderId },
                url: function () {
                    return baseUrl + "/SalesOrder/GetShippingHistoryList";
                },
                success: function (data) { }
            }
        }
    });

    $("#shippingHistoryGrid").kendoGrid({
        height: 110,
        dataSource: gridDataSource,
        columns: columns,
        sortable: true,
        scrollable: true,
        dataBound: function () {
            $("#shippingHistoryGrid .k-grid-content").css('height', '78px');

            if ($("#shippingHistoryGrid .k-grid-content tr").length == 0) {
                //$("#shippingHistoryGrid .k-grid-content").html("<br><b><center style='color:red'>No Order Status History</center></b><br>");
                $("#shippingHistory").hide();
            } else {
                $("#shippingHistory").show();
            }
        }
    });
}

function DownloadShippingLabel(quoteOrderId, trackingNo, imageType) {
    imageType = "PNG";

    ListViewUtility.ShowLoadingImage();

    $.ajax({
        type: 'POST',
        url: baseUrl + '/SalesOrder/DownloadLabel',
        data: { id: trackingNo },
        success: function (response) {
            if (response != null && response != '') {
                if (response.ImageFormat == "ZPL") {
                    download("data:text/plain;base64," + btoa('${' + atob(response.LabelImage) + '}$'), quoteOrderId + ".txt", "text/plain");
                } else if (response.ImageFormat == "GIF") {
                    download("data:image/gif;base64," + response.LabelImage, quoteOrderId + ".gif", "image/gif");
                } else {
                    download("data:image/png;base64," + response.LabelImage, quoteOrderId + ".png", "image/png");
                }
            } else {
                alert('Image not found.');
            }

            ListViewUtility.CloseLoadingImage();
        }
    });
}

function SendShippingEmail(tssShippingId, quoteOrderId, trackingNo) {
    ListViewUtility.ShowLoadingImage();

    $.ajax({
        url: baseUrl + "/SalesOrder/SendShipEmail",
        type: 'POST',
        dataType: "json",
        data: {
            TssShippingId: tssShippingId,
            QuoteOrderId: quoteOrderId,
            TrackingNo: trackingNo
        },
        success: function (result) {
            if (result.Status == 'OK') {
                BindShippingHistoryGrid(quoteOrderId);

                alert('Email sent successfully.');
            } else {
                alert('Email sent failed. Please retry.');
            }

            ListViewUtility.CloseLoadingImage();
        }
    });
}

function BindShippingTypeDdl(country) {
    var ddlShippingType = $("#ddlShippingType");
    ddlShippingType.empty();

    if (country == 'US') {
        ddlShippingType.append("<option value='Ground'>UPS Ground</option>");
        ddlShippingType.append("<option value='2Day'>UPS 2nd Air</option>");
        ddlShippingType.append("<option value='NextAir'>UPS Next Air</option>");
    } else {
        ddlShippingType.append("<option value='Ground'>UPS Standard</option>");
        ddlShippingType.append("<option value='2Day'>UPS Expedited</option>");
        ddlShippingType.append("<option value='NextAir'>UPS Saver</option>");
    }
}

function ProcessShipping() {
    var isValid = true;

    var shipToCompanyName = $("#shippToAddress1").text();
    if (shipToCompanyName.length > 35) {
        isValid = confirm("Ship to company name cannot exceed a length of 35 characters. Extra characters will be truncated. Do you want to continue?\n\n" + shipToCompanyName.substr(0, 35));
    }

    if (isValid) {
        ListViewUtility.ShowLoadingImage();

        $.ajax({
            url: baseUrl + "/SalesOrder/ProcessUPSShipping",
            type: 'POST',
            dataType: "json",
            data: {
                QuoteOrderId: $("#shippingQuoteOrderId").val(),
                ShippingType: $("#ddlShippingType").val(),
                Weight: $("#shippingTotalWeight").val(),
                Height: $("#shippingHeight").val(),
                Width: $("#shippingWidth").val(),
                Length: $("#shippingLength").val(),
                ShipFromId: $('[name=ShipFrom]:checked').val(),
                LabelImageType: $("#ddlLabelImageType").val()
            },
            success: function (result) {
                if (result.Error == undefined) {
                    $("#aShippingTrackingNo").text(result.TrackingNo);
                    $("#aShippingTrackingNo").attr("href", result.TrackingUrl);
                    $("#shippingCost").text("$" + result.TotalCost);

                    $("#divShipmentDetails").show();

                    $("#aDownloadLable").unbind();
                    $("#aDownloadLable").click(function (e) {
                        e.preventDefault();

                        if (result.LabelImageType == "GIF") {
                            download("data:image/gif;base64," + result.LabelImage, $("#shippingOrderId").text() + ".gif", "image/gif");
                        } else if (result.LabelImageType == "PNG") {
                            download("data:image/png;base64," + result.LabelImage, $("#shippingOrderId").text() + ".png", "image/png");
                        } else if (result.LabelImageType == "ZPL") {
                            download("data:text/plain;base64," + btoa('${' + atob(result.LabelImage) + '}$'), $("#shippingOrderId").text() + ".txt", "text/plain");
                        }
                    });

                    $("#aPrintLabel").unbind();
                    $("#aPrintLabel").click(function (e) {
                        e.preventDefault();

                        if (result.LabelImageType == "GIF") {
                            PrintShippingLabel($("#shippingOrderId").text() + ".gif", "data:image/gif;base64," + result.LabelImage);
                        } else if (result.LabelImageType == "PNG") {
                            PrintShippingLabel($("#shippingOrderId").text() + ".png", "data:image/png;base64," + result.LabelImage);
                        } else if (result.LabelImageType == "ZPL") {
                            PrintZPLShippingLabel($("#shippingOrderId").text() + ".txt", result.LabelImage);
                        }

                    });

                    BindShippingHistoryGrid($("#shippingQuoteOrderId").val());

                    $('#shippingModal .modal-body').animate({
                        scrollTop: 99999
                    }, 1000);

                    //$("#shippingModal .modal-body").scrollTop(9999);     

                    $("#search").trigger('click');
                } else {
                    alert(result.Error);
                }

                ListViewUtility.CloseLoadingImage();
            }
        });
    }
}

function ShowSendShippingEmail(context) {
    context = $(context);

    var tssShippingId = context.attr("data-TssShippingId");
    var quoteId = context.attr("data-QuoteId");
    var quoteOrderId = context.attr("data-QuoteOrderId");
    var trackingNo = context.attr("data-TrackingNo");
    var email = context.attr("data-Email");
    var customerName = context.attr("data-CustomerName");
    var companyName = context.attr("data-CompanyName");

    $('#sendShippingEmailModal').modal('show');
    $("#btnSendShippingEmail").removeAttr("disabled");
    $('#shippingMailTo').val(email);
    $('#tssShippingId').val(tssShippingId);

    var template = kendo.template($("#userShippmentEmailTemplate").html());
    var templateData = {
        CopyrightYear: new Date().getFullYear(),
        CustomerName: customerName,
        Url: "https://www.ups.com/track?loc=en_US&tracknum=" + trackingNo + "&requester=WT/trackdetails",
        CompanyName: companyName,
        QuoteId: quoteId,
        UnsubscribeUrl: userViewUrl + "Quote/Unsubscribe/" + quoteId
    };

    var htmlMailBody = template(templateData);

    $('#shippingMailBody').val(htmlMailBody);
    if ($("#shippingMailBody").sceditor('instance') != null) {
        $("#shippingMailBody").sceditor('instance').destroy();
    }

    $("#shippingMailBody").sceditor({
        plugins: 'xhtml',
        toolbar: 'source|bold,italic,underline|left,center,right|font,size,color,removeformat|cut,copy,paste,pastetext|bulletlist,orderedlist,table|quote,image,email,link,unlink|maximize',
        width: '100%',
        resizeMaxWidth: '100%',
        resizeMinWidth: '100%',
        height: '100%',
        style: baseUrl + "/Scripts/libs/SCEditor/jquery.sceditor.default.min.css",
        emoticonsEnabled: false
    });
    $(".sceditor-container iframe, textarea").css('width', '98%');
    $(".sceditor-container iframe, textarea").css('height', '300px');

    $("#formSendShippingMail").unbind();
    $("#formSendShippingMail").ajaxForm({
        beforeSerialize: function () {
            $("#shippingMailBody").val($("#shippingMailBody").sceditor('instance').getBody().html());

            if ($('#shippingMailTo').val().trim() == '') {
                alert('Please enter To email.');
                return false;
            }
        },
        beforeSubmit: function (formData, jqForm, options) {
            ListViewUtility.ShowLoadingImage();

            $("#btnSendShippingEmail").attr("disabled", "disabled");
        },
        success: function (result, statusText) {
            ListViewUtility.CloseLoadingImage();

            if (result.Message == "Email Send Successfully.") {
                alert(result.Message);

                $('#sendShippingEmailModal').modal('hide');
                BindShippingHistoryGrid(quoteOrderId);
            }
            else {
                alert(result.Message);

                $("#btnSendShippingEmail").removeAttr("disabled");
            }
        },
        error: function () {
            ListViewUtility.CloseLoadingImage();

            alert("Email Send Successfully.");
            $('#sendShippingEmailModal').modal('hide');
        }
    });
}

function ShippingEmailPreview() {
    var emailHtml = $("#shippingMailBody").sceditor('instance').getBody().html();

    var newwindow = window.open();
    var newDocument = newwindow.document;
    newDocument.write(emailHtml);
    newDocument.close();
}

function ShowShippingEmailHistory(tssShippingId, trackingNo) {
    $("#shippingMailHistoryModal").modal('show');
    $('#shippingEmailHistoryTrackingNo').text(trackingNo);

    var columns = [
        { field: "MessageId", title: "MailGun Id", width: "260px" },
        { field: "Recipent", title: "Recipient" },
        { field: "Event", title: "Status", width: "80px" },
        { field: "EventTimeFormated", title: "Event Time (CST)", width: "140px" },
    ];

    $("#divShippingMailHistoryGrid").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                data: { id: tssShippingId },
                url: function () {
                    return baseUrl + "/SalesOrder/GetTssShippingEmailHistory";
                },
                success: function (data) { }
            }
        }
    });

    $("#divShippingMailHistoryGrid").kendoGrid({
        //height: 110,
        dataSource: gridDataSource,
        columns: columns,
        sortable: true,
        scrollable: true,
        dataBound: function () {
            //$("#divShippingMailHistoryGrid .k-grid-content").css('height', '78px');

            if ($("#divShippingMailHistoryGrid .k-grid-content tr").length == 0) {
                $("#divShippingMailHistoryGrid .k-grid-content").html("<br><b><center style='color:red'>No Email History</center></b><br>");
            }
        }
    });
}

function PrintShippingLabel(imgName, imgSrc) {
    var labelHtml = "<html><head><style type='text/css' media='print'>@page { size:  auto; margin: 0mm; } body,html,img { margin-top:0%; display:block; height:99%; }</style></head><body><center><img src='" + imgSrc + "' /></center></body></html>";

    var newwindow = window.open(imgName, "_blank", "width=781");
    var newDocument = newwindow.document;
    newDocument.write(labelHtml);

    setTimeout(function () {
        newwindow.print();
        newwindow.close();
    }, 250);
}

function PrintZPLShippingLabel(fileName, zpl) {
    var newwindow = window.open(fileName, "_blank", "width=781");
    var newDocument = newwindow.document;
    newDocument.open('text/plain');
    newDocument.write('${' + atob(zpl) + '}$');

    setTimeout(function () {
        newwindow.print();
        newwindow.close();
    }, 250);
}