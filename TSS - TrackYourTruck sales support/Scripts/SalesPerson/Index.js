var columns = '[';
//columns += '{"template": "\u003cinput id=\u0027delete${ EmployeeId }\u0027 value=\u0027${ EmployeeId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
columns += '{"title":"Employee Id", "field":"EmployeeId", "width":"80px" },';
columns += '{"title":"Web Login", "field":"Login" },';
//columns += '{"title":"Name", "field":"Name" },';
columns += '{"title":"First Name", "field":"FirstName" },';
columns += '{"title":"Last Name", "field":"LastName" },';
columns += '{"title":"Email", "field":"Email" },';
columns += '{"title":"Work Phone", "field":"CellPhone" },';
columns += '{"title":"Sales Person", "width":"80px", "field":"IsSalesPerson", "template": kendo.template($("#sales-person-template").html()) },';
columns += ']';

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

$(function () {
    LoadSalesPerson();

    $("#btnSearch").click(function () {
        applyFilter('Name', $('#txtName').val());
    });

    $("#btnResetSearch").click(function () {
        $("#txtName").val('');

        clearFilters();
    });
});


function LoadSalesPerson() {
    createKendoGrid(columns, baseUrl + "/SalesPerson/GetSalesPersonList", false, 10, null, null, { DisableCheckAll: true });
}

function SetSalesPerson(sender) {
    sender = $(sender);

    ListViewUtility.ShowLoadingImage();

    var isDelete = false;
    var url = baseUrl + "/SalesPerson/Save";

    if (!sender.is(":checked")) {
        isDelete = true;
        url = baseUrl + "/SalesPerson/Delete";
    }

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({ id: sender.val() }),
        success: function (result) {
            ListViewUtility.CloseLoadingImage();

            var divMessage = $("#divMessage");

            if (result.Message == "Success") {
                divMessage.show();
                divMessage.css('color', 'blue');

                if (isDelete) {
                    divMessage.html(sender.data('name') + ' has been unassigned from sales person.');
                } else {
                    divMessage.html(sender.data('name') + ' has been assigned as sales person.');
                }
            }
            else {
                divMessage.show();
                divMessage.css('color', 'red');

                if (isDelete) {
                    divMessage.html('Sales person unassigned failed.');
                } else {
                    divMessage.html('Sales person assign failed.');
                }
            }

            HideMessage();
        }
    });
}

function HideMessage() {
    setTimeout(function () {
        $("#divMessage").hide();
    }, 30000);
}


// applyFilter function accepts the Field Name and the new value to use for filter.
function applyFilter(filterField, filterValue) {

    // get the kendoGrid element.
    var gridData = $("#divKendoGrid").data("kendoGrid");

    // get currently applied filters from the Grid.
    var currFilterObj = gridData.dataSource.filter();

    // get current set of filters, which is supposed to be array.
    // if the oject we obtained above is null/undefined, set this to an empty array
    var currentFilters = currFilterObj ? currFilterObj.filters : [];

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