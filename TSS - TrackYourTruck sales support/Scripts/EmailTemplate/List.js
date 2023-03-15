var columns = '[';
columns += '{"template": "\u003cinput class=\u0027delete\u0027 id=\u0027delete${ EmailTemplateId }\u0027 value=\u0027${ EmailTemplateId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: "30px"},';
columns += '{"title":"Create Date/Time", "width":"130px", "field":"CreatedDateFormatted"},';
columns += '{"field":"EmailTemplateId", "width":"80px", "title":"Template Id","template":"\u003ca href=\u0027javascript:Edit(${ EmailTemplateId })\u0027\u003e${ EmailTemplateId }\u003c/a\u003e"},';
columns += '{"field":"Title", "width":"900px", "title":"Template Name"}';
columns += ']';

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
    createKendoGrid(columns, baseUrl + "/EmailTemplate/GetEmailTemplateList", false, 10, null, null);
}

function Edit(primaryKey) {
    location.href = baseUrl + "/EmailTemplate/Edit/" + primaryKey;
}

function New() {
    location.href = baseUrl + "/EmailTemplate/New";
}

function RemoveProduct(primaryKey) {
    if (confirm("Are you sure to delete this Email?")) {
        $.ajax({
            url: baseUrl + "/EmailTemplate/Delete",
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
                    alert("This Email cannot be deleted");
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
        if (confirm("Are you sure to delete " + totalRecordToBeDeleted + " Email Template(s)?")) {
            $('.k-grid-content table input:checkbox:checked.delete').each(function (index, element) {
                if (index == totalRecordToBeDeleted - 1) {//last record
                    removeKendoGridRow(this.value, baseUrl + "/EmailTemplate/Delete",
                    function (successfuldelete) {
                        if (successfuldelete == "Delete failed.") {
                            countnodeletes++;
                        }
                        else {
                            countdeletes++;
                        }
                        if (countnodeletes > 0) {
                            alert(countnodeletes + " out of " + countdeletes + " Email Template(s) delete failed.");
                        }
                        $("#divKendoGrid").data("kendoGrid").dataSource.read();
                    });
                }
                else { // all but last record
                    removeKendoGridRow(this.value, baseUrl + "/EmailTemplate/Delete",
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