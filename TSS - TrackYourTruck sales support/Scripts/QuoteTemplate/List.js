var columns = '[';
columns += '{"template": "\u003cinput class=\u0027delete\u0027 id=\u0027delete${ QuoteId }\u0027 value=\u0027${ QuoteId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: "30px"},';
columns += '{"title":"Create Date/Time", "width":"130px", "template": kendo.template($("#date-template").html())},';
columns += '{"field":"IsActive", "title":"Active", "width":"40px", "template": kendo.template($("#is-active-template").html())},';
columns += '{"field":"QuoteId", "width":"80px", "title":"Template Id","template":"\u003ca href=\u0027javascript:Edit(${ QuoteId })\u0027\u003e${ QuoteId }\u003c/a\u003e"},';
columns += '{"field":"GroupName", "width":"100px", "title":"Template Group"},';
columns += '{"field":"TemplateName", "width":"900px", "title":"Template Name"}';
//columns += '{"field":"CustomerName","title":"Customer Name"},';
//columns += '{"field":"Qty","width":"150px","title":"Unit Types - Qty", encoded: false },';
//columns += '{"title":"Last Viewed", "width":"140px", "field":"LastViewDateFormated" }';
columns += ']';

function PDF(pdfToken) {
    window.open(baseUrl + "/QuoteProductRpt/ShowQuoteProductRpt/?id=" + pdfToken);
}

$(function () {
    LoadTemplate();

    $("#search").click(function () {
        applyFilter($("#searchSelect").val(), $("#searchText").val());
    });

    $("#resetSearch").click(function () {
        $("#searchText").val('');
        clearFilters();
    });
});

function LoadTemplate() {
    createKendoGrid(columns, baseUrl + "/QuoteTemplate/GetQuoteTemplateList", false, 10, null, null);
}

function Edit(primaryKey) {
    location.href = baseUrl + "/QuoteTemplate/Edit/" + primaryKey;
}

function New() {
    location.href = baseUrl + "/QuoteTemplate";
}

function RemoveProduct(primaryKey) {
    if (confirm("Are you sure to delete this Quote?")) {
        $.ajax({
            url: baseUrl + "/QuoteTemplate/Delete",
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
    var totalRecordToBeDeleted = $('.k-grid-content table input:checkbox:checked.delete').length;
    var successfuldelete = "";
    var countnodeletes = 0;
    var countdeletes = 0;

    if (totalRecordToBeDeleted > 0) {
        if (confirm("Are you sure to delete " + totalRecordToBeDeleted + " Quote Template(s)?")) {
            $('.k-grid-content table input:checkbox:checked.delete').each(function (index, element) {
                if (index == totalRecordToBeDeleted - 1) {//last record
                    removeKendoGridRow(this.value, baseUrl + "/QuoteTemplate/Delete",
                    function (successfuldelete) {
                        if (successfuldelete == "Delete failed.") {
                            countnodeletes++;
                        }
                        else {
                            countdeletes++;
                        }
                        if (countnodeletes > 0) {
                            alert(countnodeletes + " out of " + countdeletes + " Quote Template(s) delete failed.");
                        }
                        $("#divKendoGrid").data("kendoGrid").dataSource.read();
                    });
                }
                else { // all but last record
                    removeKendoGridRow(this.value, baseUrl + "/QuoteTemplate/Delete",
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

    $('#sendEmailModal').modal('show');
    $('#mailTo').val(email);
    $('#mailBody').val("Click the link below to view your Track Your Truck Vehicle Tracking Quote.\n<a target='_blank' href='" + url + "' >View your quote</a>");
    $('#mailQuoteId').val(quoteId);
    $('#customerName').val(customerName);
    $('#sendNowId').val(context.attr("id"));
}

function SendEmail() {
    var context = $('#' + $('#sendNowId').val());
    $.ajax({
        url: baseUrl + "/QuoteTemplate/SendUserUrl",
        type: "POST",
        dataType: "json",
        data: { QuoteId: $('#mailQuoteId').val(), MailTo: $('#mailTo').val(), Body: $('#mailBody').val(), CustomerName: $('#customerName').val() },
        success: function (result) {
            if (result.Message == "Url Send Successfully.") {
                context.parent().append("<div>" + result.UrlSendDate + "</div>");
                context.remove()
                alert(result.Message);

                $('#sendEmailModal').modal('hide');
            }
            else {
                alert(result.Message);
            }
        }
    });
}

function SetActive(quoteId, ele) {
    var ele = $(ele);
    var isActive = ele.is(":checked");

    $.ajax({
        url: baseUrl + "/QuoteTemplate/SetActive",
        type: "POST",
        dataType: "json",
        data: { quoteId: quoteId, isActive: isActive },
        success: function (result) {
            if (result.Message != "Success") {
                alert("Active Set Failed.");
            }

            $("#divKendoGrid").data("kendoGrid").dataSource.read();
        }
    });
}