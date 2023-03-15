var columns = '[';
columns += '{"template": "\u003cinput id=\u0027delete${ QuoteId }\u0027 value=\u0027${ QuoteId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
columns += '{"title":"Create Date/Time (CST)", "width":"575px", "template": kendo.template($("#date-template").html())},';
columns += '{"title":" ", "width":"40px", "template": kendo.template($("#note-template").html())},';
columns += '{"field":"QuoteId", "width":"60px", "title":"Quote Id","template":"\u003ca href=\u0027javascript:EditQuote(${ QuoteId })\u0027\u003e${ QuoteId }\u003c/a\u003e"},';
//columns += '{"field":"Url","title":"URL", "width":"170px", "template":"\u003ca target=\u0027_blank\u0027 href=\u0027${ Url }\u0027\u003eView your Quote\u003c/a\u003e \u003ca class=\u0027btn btn-small btn-info\u0027 href=\u0027javascript:ShowShortUrl(\\u0022${ Url }\\u0022)\u0027\u003eTiny Url\u003c/a\u003e"},';
columns += '{"title":"", "width":"112px","template": kendo.template($("#convert-to-order-template").html())},';
//columns += '{"title":"Short URL", "width":"70px", "template":"\u003ca href=\u0027javascript:ShowShortUrl(\\u0022${ Url }\\u0022)\u0027\u003eGenerate\u003c/a\u003e"},';
columns += '{"field":"CompanyName","width":"200px","title":"Company Name"},';
columns += '{"field":"OrderTypeTitle", "width":"175px","title":"Order Type"},';
//columns += '{"field":"Qty","width":"150px","title":"Unit Types - Qty", encoded: false },';
columns += '{"title":"Last Viewed (CST)", "width":"130px", "field":"LastViewDateFormated","template": kendo.template($("#lastViewDateTemplate").html()) },';
columns += '{"title":"L.V.C.", "width":"50px", "field":"LastViewCount" },';
columns += '{"title":"Order Status", "width":"120px", "field":"StatusTitle" },';
columns += '{"title":"Unsubscribe", "width":"90px", "template": kendo.template($("#unsubscribeTemplate").html()) }';
columns += ']';

function PDF(pdfToken) {
    window.open(baseUrl + "/QuoteProductRpt/ShowQuoteProductRpt/?id=" + pdfToken);
}

function GetQuoteQueryString() {
    return "dateFrom=" + $("#dateFrom").val() +
        "&dateTo=" + $("#dateTo").val() +
        "&searchSelect=" + $("#searchSelect").val() +
        "&searchText=" + $("#searchText").val();
}

function LoadQuoteList() {
    var settings = null;
    if ($("#divKendoGrid").data("kendoGrid") != undefined && $("#divKendoGrid").data("kendoGrid") != null) {
        settings = {};
        settings.Sort = $("#divKendoGrid").data("kendoGrid").dataSource.sort();
    }


    createKendoGrid(columns, baseUrl + "/Quote/GetQuoteList?" + GetQuoteQueryString(), false, 10, null, null, settings);
}

$(function () {
    LoadQuoteList();

    $("#search").click(function () {
        //applyFilter($("#searchSelect").val(), $("#searchText").val());
        if (validateSearch()) {
            LoadQuoteList()
        }
    });

    $("#resetSearch").click(function () {
        $("#searchText").val('');
        LoadQuoteList()
        //clearFilters();
    });

    $("input[name=bluePayState]").click(function () {
        var _this = $(this);
        var value = _this.val();

        $.ajax({
            url: baseUrl + "/Quote/SaveBluePayMode",
            type: "POST",
            async: false,
            dataType: "json",
            data: { bluePayMode: value },
            success: function (result) {
                if (result.Message == "Failed") {
                    alert("BluePay mode set failed.");

                    _this.closest("label").removeClass("active");

                    if (value == "TEST") { $("input[name=bluePayState][value=LIVE]").parent().addClass("active"); }
                    if (value == "LIVE") { $("input[name=bluePayState][value=TEST]").parent().addClass("active"); }
                }
            }
        });
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

    var sentEmailFromDatePicker = new Pikaday({
        field: document.getElementById('sentEmailDateFrom'),
        format: 'MM/DD/YYYY'
    });

    var sentEmailToDatePicker = new Pikaday({
        field: document.getElementById('sentEmailDateTo'),
        format: 'MM/DD/YYYY'
    });

    setInterval(LoadLastView, 30000);

    if ($("#quoteId").val() != "") {
        EditQuote($("#quoteId").val());
    }

    var clipboard = new Clipboard('#aCopy');
    clipboard.on('success', function () {
        $('#spanCopyStatus').text("Copied!");
    });

    $.ajax({
        url: baseUrl + "/Common/LoadDDLJson",
        type: 'POST',
        async: false,
        data: { 'spName': 'ug_TssOrderType' },
        success: function (result) {
            $("#ddlOrderType").append("<option value='0'> </option>");

            $.each(result, function (index, item) {
                $("#orderTypeContent").append("<div><input type='radio' name='ChangeOrderType' " + (index == 0 ? "checked" : "") + " value='" + item.value + "' /> " + item.keyfield + "</div>");
                $("#ddlOrderType").append("<option value='" + item.value + "'>" + item.keyfield + "</option>");
            });
        }
    });
});

function EditQuote(primaryKey) {
    //location.href = baseUrl + "/Quote/Edit/" + primaryKey;
    ListViewUtility.ShowLoadingImage();

    ShowDetailPage();

    dataLoadedForEdit = false;
    $("#quoteId").val(primaryKey);
    Edit(primaryKey);

    ListViewUtility.CloseLoadingImage();
}

function ShowOrderType() {
    $('#orderTypeModal').modal('show');
    $('#btnConvertToOrder').attr('onclick', 'SetOrderType();');
}

function SetOrderType() {
    $('#ddlOrderType').val($('[name=ChangeOrderType]:checked').val());
    $('#orderTypeModal').modal('hide');

    New();
}

function New() {
    //location.href = baseUrl + "/Quote";
    dataLoadedForEdit = true;
    $("#quoteNumber").text("");
    $("#quoteId").val("");
    $('#aApprove').hide();

    ShowDetailPage();
}

function Cancel() {
    if (valueChanged) {
        if (confirm("Do you want to continue without saving?")) {
            HideDetailPage();
        }
    }
    else {
        HideDetailPage();
    }
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
        scrollTop: $("#divQuoteList").offset().top
    }, 1000);
}

function RemoveProduct(primaryKey) {
    if (confirm("Are you sure to delete this Quote?")) {
        $.ajax({
            url: baseUrl + "/Quote/Delete",
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
                    alert("This Quote  cannot be deleted");
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
        if (confirm("Are you sure to delete " + totalRecordToBeDeleted + " Quote(s)?")) {
            $('.k-grid-content table input:checkbox:checked').each(function (index, element) {
                if (index == totalRecordToBeDeleted - 1) {//last record
                    removeKendoGridRow(this.value, baseUrl + "/Quote/Delete",
                    function (successfuldelete) {
                        if (successfuldelete == "Delete failed.") {
                            countnodeletes++;
                        }
                        else {
                            countdeletes++;
                        }
                        if (countnodeletes > 0) {
                            alert(countnodeletes + " out of " + countdeletes + " Quote(s) delete failed.");
                        }
                        $("#divKendoGrid").data("kendoGrid").dataSource.read();
                    });
                }
                else { // all but last record
                    removeKendoGridRow(this.value, baseUrl + "/Quote/Delete",
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

function SendUserUrl(context) {
    context = $(context);
    var quoteId = context.attr("data-QuoteId");
    var url = context.attr("data-Url");
    var email = context.attr("data-Email");
    var customerName = context.attr("data-CustomerName");
    var companyName = context.attr("data-CompanyName");

    $("#divEmailTemplateSection").show();
    LoadEmailTemplate(quoteId, companyName, customerName, url);

    $('#sendEmailModal').modal('show');
    $("#divEmailAttachmentSec").hide();

    $('#mailTo').val(email);
    $('#mailQuoteId').val(quoteId);
    $('#customerName').val(customerName);
    $('#sendNowId').val(context.attr("id"));
    $("#btnSendEmail").removeAttr("disabled");
    $("#btnSendEmailLocal").removeAttr("disabled");

    $('#isAttachDoc').val(false);
    $('#chkIncludePdf').removeAttr('checked');

    var htmlMailBody = "Click the link below to view your Track Your Truck Vehicle Tracking Quote.<br><a target='_blank' href='" + url + "' >View your quote</a>";
    htmlMailBody += "";

    var template = kendo.template($("#userEmailTemplate").html());
    var templateData = {
        CopyrightYear: new Date().getFullYear(),
        CustomerName: customerName,
        Url: url,
        CompanyName: companyName,
        QuoteId: quoteId,
        UnsubscribeUrl: userViewUrl + "Quote/Unsubscribe/" + quoteId
    };
    var result = template(templateData);
    htmlMailBody = result;

    $('#mailBody').val(htmlMailBody);
    if ($("#mailBody").sceditor('instance') != null) {
        $("#mailBody").sceditor('instance').destroy();
    }
    $("#divEmailAttachment").html('');

    $("#mailBody").sceditor({
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

    $("#formSendMail").unbind();
    $("#formSendMail").ajaxForm({
        beforeSerialize: function () {
            $("#mailBody").val($("#mailBody").sceditor('instance').getBody().html());

            if (!ValidateSendEmail()) {
                return false;
            }
        },
        beforeSubmit: function (formData, jqForm, options) {
            ListViewUtility.ShowLoadingImage();

            $("#btnSendEmail").attr("disabled", "disabled");
            $("#btnSendEmailLocal").attr("disabled", "disabled");
        },
        success: function (result, statusText) {
            ListViewUtility.CloseLoadingImage();

            if (result.Message == "Email Send Successfully.") {
                alert(result.Message);

                $('#sendEmailModal').modal('hide');
            }
            else {
                alert(result.Message);

                $("#btnSendEmail").removeAttr("disabled");
                $("#btnSendEmailLocal").removeAttr("disabled");
            }
        },
        error: function () {
            ListViewUtility.CloseLoadingImage();

            alert("Email Send Successfully.");
            $('#sendEmailModal').modal('hide');
        }
    });

    $.ajax({
        url: baseUrl + "/Quote/GetUnsubscribeList",
        type: "POST",
        dataType: "json",
        data: { id: quoteId },
        success: function (result) {
            var html = "", unsubscribeEmailList = "";
            $.each(result, function (index, item) {
                unsubscribeEmailList += item.Email + ";";
                html += "<div style='color: black; font-size: medium; margin-bottom: 5px'>" + item.Email + "</div>";
            });

            $("#aUnsubscribeList").attr('unsubscribe-email-list', unsubscribeEmailList);
            $("#aUnsubscribeList").attr('data-original-title', html);
            $("#aUnsubscribeList").tooltip({ title: html, placement: "bottom" });
        }
    });
}

function ValidateSendEmail() {
    if ($('#mailTo').val().trim() == '') {
        alert('Please enter To email.');
        return false;
    }

    var isNotInUnsubscribeList = true;
    var unsubscribeEmailList = $("#aUnsubscribeList").attr('unsubscribe-email-list');
    var unsubscribeEmailFound = '';
    $.each($('#mailTo').val().split(';'), function (idx, email) {
        if (unsubscribeEmailList.indexOf(email) > -1) {
            unsubscribeEmailFound += (unsubscribeEmailFound == '' ? '' : ';') + email;
            isNotInUnsubscribeList = false;
        }
    });

    if (!isNotInUnsubscribeList) {
        alert('You can not send email to: ' + unsubscribeEmailFound + '.\nPlease check unsubscribe list.');
        return false;
    }

    return true;
}

function ShowShortUrl(url) {
    $('#spanCopyStatus').text("");
    $("#txtShortUrl").val("Generating...");
    $('#copyModal').modal('show');
    var jsonToBeSent = JSON.stringify({
        "dynamicLinkInfo": {
            "domainUriPrefix": "https://tyt1.net",
            "link": url

        }
    });
    $.ajax({
        url: "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + ShortUrlKey,
        type: "POST",
        contentType: "application/json; charset=UTF-8",
        data: jsonToBeSent
    }).done(function (response) {
        $("#txtShortUrl").val(response.shortLink);
    }).error(function (response) {
        $("#txtShortUrl").val("Generation Error. Try Again.");
        alert(eval("(" + response.responseText + ")").error.message);
        setTimeout(function () {
            $('#copyModal').modal('hide');
        }, 1000);
    });
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

var quotePaymentMethodList = null;
function ShowConvertToOrderPopup(context) {
    context = $(context);
    var quoteId = context.attr("data-QuoteId");
    var isValidShippingAddress = context.attr("data-IsValidShippingAddress");

    if (isValidShippingAddress == "false" && !confirm("Incomplete Shipping address, unable to calculate shipping & handling cost. Would you like to continue?")) {
        return;
    }

    if (quotePaymentMethodList == null) {
        $.ajax({
            url: baseUrl + "/Quote/GetQuotePaymentMethodList",
            type: "POST",
            dataType: "json",
            success: function (result) {
                quotePaymentMethodList = result
                $.each(result, function (index, item) {
                    if (item.QuotePaymentMethodId == 8) {
                        return;
                    }
                    $("#paymentMethodType").append("<option value='" + item.QuotePaymentMethodId + "'>" + item.PaymentMethod + "</option>");
                });
            }
        });
    }

    $('#convertToOrderModal').modal('show');
    $('#convertToOrderQuoteId').val(quoteId);
}

function ConvertToOrder() {
    ListViewUtility.ShowLoadingImage();

    var quoteId = $('#convertToOrderQuoteId').val();

    $.ajax({
        url: baseUrl + "/Quote/ConvertToOrder",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: JSON.stringify({ quoteId: quoteId, quotePaymentMethod: $('#paymentMethodType :selected').text(), quotePaymentMethodId: $('#paymentMethodType').val(), comment: $("#paymentMethodComment").val() }),
        success: function (result) {
            ListViewUtility.CloseLoadingImage();

            if (result.Message == "Success") {
                alert("Quote successfully converted to Order.");
                $('#convertToOrderModal').modal('hide');

                $("#paymentMethodComment").val("");
                $('#convertToOrderQuoteId').val("");

                LoadQuoteList();
            } else {
                alert("Quote convert failed.");
            }
        }
    });
}

function ShowLastViewDDL(sender, quoteId) {
    var context = $(sender);

    $.ajax({
        url: baseUrl + "/Quote/GetQuoteLastViewList",
        type: "POST",
        async: false,
        dataType: "json",
        data: { id: quoteId },
        success: function (result) {
            if (result.length > 0) {
                var template = "<h3><div style='color:black; font-size: 12px'>Total Viewed: " + result.length + "</div>";
                $.each(result, function (index, item) {
                    template += "<div style='color:black; font-size: 12px'>" + item.LastViewDateFormated + "</div>";
                });
                template += "</h3>";

                context.attr('data-original-title', template);
                context.tooltip({ title: template, placement: "bottom", trigger: "manual" });
                context.tooltip("show");
            }
        }
    });
}

function HideLastViewDDL(sender) {
    var context = $(sender);
    context.tooltip("hide");
}

function ShowEmailHistory(quoteId) {
    var columns = [
        { "field": "SentDateFormated", "title": "Sent Date (CST)", "width": "135px" },
        { "field": "Recipent", "title": "Recipient" },
        { "field": "SenderName", "title": "Sent By" },
        { "field": "EmailId", "title": "MailGun Id", "width": "220px" },
        { "field": "EmailStatus", "title": "Status", "width": "80px" },
        { "field": "EmailStatusTimeFormated", "title": "Received Date (CST)", "width": "140px" },
        { "title": "<center>Resend</center>", "width": "80px", "template": "<a style='display: #if(IsResendAvailable){# block #} else {# none #}#' href='javascript:ShowResendEmail(${ TSSSentEmailId })' class='btn'>Resend</a>" }
    ];

    $("#divKendoGridEmailHistory").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                data: { id: quoteId },
                url: function () {
                    return baseUrl + "/Quote/GetSentEmailList";
                },
                success: function (data) {
                    //console.log("success");
                }
            }
        }
    });

    $("#divKendoGridEmailHistory").kendoGrid({
        dataSource: gridDataSource,
        columns: columns,
        sortable: true,
        scrollable: true,
        dataBound: function () {
            if ($("#divKendoGridEmailHistory .k-grid-content tr").length == 0) {
                $("#divKendoGridEmailHistory .k-grid-content").html("<br><b><center style='color:red'>No E-mail History</center></b><br>")
            }
        }
    });

    $('#mailHistoryModal').modal('show');
}

function ShowResendEmail(tssSentEmailId) {
    $.ajax({
        url: baseUrl + "/Quote/GetSentEmail",
        type: "POST",
        dataType: "json",
        data: { id: tssSentEmailId },
        success: function (result) {
            //$('#mailHistoryModal').modal('hide');

            $('#sendEmailModal').modal('show');
            $("#sendEmailModal").css("z-index", "99999");
            $(".modal-backdrop").last().css("z-index", "99990");

            $("#divEmailTemplateSection").hide();
            $("#divEmailAttachmentSec").hide();

            $('#mailTo').val(result.Recipent);
            $('#mailQuoteId').val(result.QuoteId);
            $("#btnSendEmail").removeAttr("disabled");
            $("#btnSendEmailLocal").removeAttr("disabled");

            $('#isAttachDoc').val(false);
            $('#chkIncludePdf').removeAttr('checked');

            $('#mailBody').val(result.MessageBody);
            if ($("#mailBody").sceditor('instance') != null) {
                $("#mailBody").sceditor('instance').destroy();
            }
            $("#divEmailAttachment").html('');

            $("#mailBody").sceditor({
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

            $("#formSendMail").unbind();
            $("#formSendMail").ajaxForm({
                beforeSerialize: function () {
                    $("#mailBody").val($("#mailBody").sceditor('instance').getBody().html());

                    if (!ValidateSendEmail()) {
                        return false;
                    }
                },
                beforeSubmit: function (formData, jqForm, options) {
                    ListViewUtility.ShowLoadingImage();

                    $("#btnSendEmail").attr("disabled", "disabled");
                    $("#btnSendEmailLocal").attr("disabled", "disabled");
                },
                success: function (result, statusText) {
                    ListViewUtility.CloseLoadingImage();

                    if (result.Message == "Email Send Successfully.") {
                        alert(result.Message);

                        $('#sendEmailModal').modal('hide');
                    }
                    else {
                        alert(result.Message);

                        $("#btnSendEmail").removeAttr("disabled");
                        $("#btnSendEmailLocal").removeAttr("disabled");
                    }
                },
                error: function () {
                    ListViewUtility.CloseLoadingImage();
                }
            });
        }
    });
}

function ShowEmailHistoryReport() {
    var columns = '[';
    columns += '{"field":"QuoteId", "title":"Quote Id", "width":"100px"},';
    columns += '{"field":"CustomerName", "title":"Customer Name"},';
    columns += '{"field":"SentDateFormated", "title":"Sent Date (CST)", "width":"135px"},';
    columns += '{"field":"Recipent","title":"Recipient"},';
    columns += '{"field":"EmailStatus","title":"Status", "width":"80px"},';
    columns += '{"field":"EmailStatusTimeFormated","title":"Received Date (CST)", "width":"140px"}';
    columns += ']';

    $("#divKendoGridEmailHistoryReport").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                data: { SearchFromDate: $("#sentEmailDateFrom").val(), SearchToDate: $("#sentEmailDateTo").val(), EmailStatus: $("#searchEmailStatus").val() },
                url: function () {
                    return baseUrl + "/Quote/GetSentEmailReportList";
                },
                success: function (data) {
                    //console.log("success");
                }
            }
        }
    });

    $("#divKendoGridEmailHistoryReport").kendoGrid({
        dataSource: gridDataSource,
        columns: eval(columns),
        sortable: true,
        scrollable: true,
        dataBound: function () {
            if ($("#divKendoGridEmailHistoryReport .k-grid-content tr").length == 0) {
                $("#divKendoGridEmailHistoryReport .k-grid-content").html("<br><b><center style='color:red'>No E-mail History</center></b><br>");

                $("#aEmailHistoryExportToExcel").hide();
            } else {
                $("#aEmailHistoryExportToExcel").show();
            }
        }
    });

    $('#mailHistoryReportModal').modal('show');
}

function EmailHistoryExportToExcel() {
    var frmExportToExcel = $("#emailHistoryReportForm");
    frmExportToExcel.attr('target', '');
    frmExportToExcel.attr("action", baseUrl + "/Quote/EmailHistoryExportToExcel");
    frmExportToExcel.submit();
}

function AddEmailAttachment() {
    var emailAttachment = "<div><a class='remove-email-attachment' href='#'>X</a>&nbsp;<input type='file' name='EmailAttachment' style='width: 70%' /></div>";

    $("#divEmailAttachmentSec").show();
    $("#divEmailAttachment").append(emailAttachment);
    $("#sendEmailModal .modal-body").scrollTop(9999);

    $(".remove-email-attachment").unbind();
    $(".remove-email-attachment").click(function () {
        $(this).parent().remove();
    });
}

function EmailPreview() {
    var emailHtml = $("#mailBody").sceditor('instance').getBody().html();

    var newwindow = window.open();
    var newDocument = newwindow.document;
    newDocument.write(emailHtml);
    newDocument.close();
}

function LoadEmailTemplate(quoteId, companyName, customerName, url) {
    var emailTemplate = $("#emailTemplate");

    $.ajax({
        url: baseUrl + "/EmailTemplate/GetEmailTemplateList",
        type: "POST",
        dataType: "json",
        success: function (result) {
            var options = "<option value=''>Default</option>";
            $.each(result, function (index, item) {
                options += "<option value='" + item.EmailTemplateId + "'>" + item.Title + "</option>";
            });

            emailTemplate.empty();
            emailTemplate.append(options);
        }
    });

    emailTemplate.unbind();
    emailTemplate.change(function () {
        var templateId = $(this).val();

        if (templateId == "") {
            var template = kendo.template($("#userEmailTemplate").html());
            var templateData = {
                CopyrightYear: new Date().getFullYear(),
                CustomerName: customerName,
                Url: url,
                CompanyName: companyName,
                QuoteId: quoteId,
                UnsubscribeUrl: userViewUrl + "Quote/Unsubscribe/" + quoteId
            };
            var htmlMailBody = template(templateData);

            $('#mailBody').val(htmlMailBody);
            if ($("#mailBody").sceditor('instance') != null) {
                $("#mailBody").sceditor('instance').destroy();
            }
            $("#divEmailAttachment").html('');

            $("#mailBody").sceditor({
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
        }
        else {
            $.ajax({
                url: baseUrl + "/EmailTemplate/Get",
                type: "POST",
                async: false,
                dataType: "json",
                data: { id: emailTemplate.val() },
                success: function (result) {
                    if (result != null) {
                        var template = kendo.template(result.Template);
                        var templateData = {
                            CopyrightYear: new Date().getFullYear(),
                            CustomerName: customerName,
                            Url: url,
                            CompanyName: companyName,
                            QuoteId: quoteId,
                            UnsubscribeUrl: userViewUrl + "Quote/Unsubscribe/" + quoteId
                        };
                        var htmlMailBody = template(templateData);

                        $('#mailBody').val(htmlMailBody);
                        if ($("#mailBody").sceditor('instance') != null) {
                            $("#mailBody").sceditor('instance').destroy();
                        }
                        $("#divEmailAttachment").html('');

                        $("#mailBody").sceditor({
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
                    }
                }
            });
        }
    });
}

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

function LoadLastView() {
    if (validateSearch()) {
        LoadQuoteList();
    }

    /*$.ajax({
    url: baseUrl + "/Quote/GetLastView?" + GetQuoteQueryString(),
    type: "POST",
    dataType: "json",
    success: function (result) {
    if (result.length > 0) {
    $.each(result, function (index, item) {
    $("input[value=" + item.QuoteId + "]").closest('tr').find('.LastViewDateFormated').html(item.LastViewDateFormated);
    });
    }
    }
    });*/

    $("#gridLastUpdate").text("Last Updated: " + moment().format('MM/DD/YYYY h:mm:ss A') + " (CST)");
}

function ShowUnsubscribeList(sender, quoteId) {
    var context = $(sender);

    $.ajax({
        url: baseUrl + "/Quote/GetUnsubscribeList",
        type: "POST",
        dataType: "json",
        data: { id: quoteId },
        success: function (result) {
            var html = "";
            $.each(result, function (index, item) {
                html += "<div style='color:black; font-size: 14px;'>" + item.Email + "</div>";
            });

            context.attr('data-original-title', html);
            context.tooltip({ title: html, placement: "left", trigger: "manual" });
            context.tooltip("show");
        }
    });
}

function HideUnsubscribeList(sender) {
    var context = $(sender);
    context.tooltip("hide");
}


function ShowQuoteNoteTooltip(sender) {
    var context = $(sender);

    var template = "<div style='color:black; font-size: 12px; text-align:justify; max-width: 300px; font-weight: bold'>Note: " + context.attr("note") + "</div>";
    context.attr('data-original-title', template);
    context.tooltip({ title: template, placement: "top", trigger: "manual" });
    context.tooltip("show");
}

function HideQuoteNoteTooltip(sender) {
    var context = $(sender);
    context.tooltip("hide");
}

function ResetQuote(quoteId) {
    if (confirm("Do you want to reset quote date?")) {
        $.ajax({
            url: baseUrl + "/Quote/ResetQuoteStartDate",
            type: "POST",
            dataType: "json",
            data: { id: quoteId },
            success: function (result) {
                if (result.Message == "Success") {
                    alert("Quote date reset successfully.");
                    LoadQuoteList();
                } else {
                    alert("Quote date reset failed.");
                }
            }
        });
    }
}

function ApproveQuote(quoteId) {
    if (confirm("Do you want to approve this quote?")) {
        $.ajax({
            url: baseUrl + "/Quote/ApproveQuote",
            type: "POST",
            dataType: "json",
            data: { id: quoteId },
            success: function (result) {
                if (result.Message == "Success") {
                    alert("Quote approved successfully.");
                    LoadLastView();
                    $("#aApprove").hide();
                } else {
                    alert("Quote approved failed.");
                }
            }
        });
    }
}

function ShowApprovedTooltip(sender) {
    var context = $(sender);

    var template = "<div style='color:black; font-size: 14px; text-align:justify; max-width: 450px;'>Approved By: " + context.attr("ApprovedUserName") + " <br>Date: " + context.attr("ApprovedDateFormated") + "</div>";
    context.attr('data-original-title', template);
    context.tooltip({ title: template, placement: "top", trigger: "manual" });
    context.tooltip("show");
}

function HideApprovedTooltip(sender) {
    var context = $(sender);
    context.tooltip("hide");
}

function IncludePdf() {
    if ($('#chkIncludePdf').is(':checked')) {
        $('#isAttachDoc').val(true);
    } else {
        $('#isAttachDoc').val(false);
    }
}