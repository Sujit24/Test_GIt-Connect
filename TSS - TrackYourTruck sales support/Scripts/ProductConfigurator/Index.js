var columns = '[';
columns += '{"template": "\u003cinput id=\u0027delete${ ProductId }\u0027 value=\u0027${ ProductId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
columns += '{"field":"SKU","title":"SKU","template":"\u003ca href=\u0027javascript:LoadDetailView(${ ProductId })\u0027\u003e${ SKU }\u003c/a\u003e"},';
columns += '{"field":"ProductName","title":"Product Name"},';
columns += '{"field":"ProductDescription","title":"Product Description"},';
columns += '{"field":"Weight","title":"Weight(lbs)", "width":"80px"},';
columns += '{"field":"Price","title":"Price",format:"{0:c2}", "width":"100px"},';
columns += '{"field":"ProductTypeName","title":"Type"}';
columns += ']';



$(function () {
    createKendoGrid(columns, baseUrl + "/ProductConfigurator/GetProductList?FromProductPage=1", false, 10, null, null);

    $("#search").click(function () {
        applyFilter($("#searchSelect").val(), $("#searchText").val());
    });

    $("#resetSearch").click(function () {
        $("#searchText").val('');
        clearFilters();
    });
});

function LoadDetailView(ProductId, successFn) {
    var requestUrl = "";
    ListViewUtility.ShowLoadingImage();

    try {
        if (ProductId == 0) {
            requestUrl = baseUrl + "/ProductConfigurator/New";
        }
        else {
            requestUrl = baseUrl + "/ProductConfigurator/Edit" + "/" + ProductId;
        }

        $.ajax({
            url: requestUrl,
            type: 'POST',
            async: true,
            data: { id: ProductId },
            success: function (result) {
                //console.dir(result);
                if (result.Message != "Load failed.") {
                    $("#detail-container").html(result);
                    ListViewUtility.CloseLoadingImage();
                    scrollToDetailContainer();
                    if (successFn) { successFn() }
                }
            },
            error: function (result) {
                //console.dir(result);
                ListViewUtility.CloseLoadingImage();
            }
        });
    }
    catch (err) {
        //$("#lblNumericPhone").css("display", "block");
    }
}

function scrollToDetailContainer() {
    $('html, body').animate({
        scrollTop: $("#detail-container").offset().top
    }, 1000);
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

function CancelDetialView() {
    $("#detail-container").html('');
}

function ValidateProductForSaveAsNew() {
    var isValid = ValidateProduct();

    if (isValid) {
        $("#ProductId").val("0");
    }

    return isValid;
}

function ValidateProduct() {
    var requiredDataMessage = "";

    if ($("#SKU").val() == "") {
        requiredDataMessage += "SKU is required.<br/>";
    }
    if ($("#ProductName").val() == "") {
        requiredDataMessage += "Product Name is required.<br/>";
    }
    if ($("#ProductDescription").val() == "") {
        requiredDataMessage += "Product Description is required.<br/>";
    }

    if ($("#Price").val() == "") {
        requiredDataMessage += "Price is required.<br/>";
    } else if (!$.isNumeric($("#Price").val())) {
        requiredDataMessage += "Price is invalid.<br/>";
    }

    if ($("#Weight").val() == "") {
        requiredDataMessage += "Weight is required.<br/>";
    } else if (!$.isNumeric($("#Weight").val())) {
        requiredDataMessage += "Weight is invalid.<br/>";
    }

    if (requiredDataMessage != "") {
        $('#lblWarning').html(requiredDataMessage);
        return false;
    }

    return true;
}

function SubmitDetialView() {

    /*var postData;
    postData = $("#mainForm").serialize();*/

    /* try {
    $.ajax({
    url: baseUrl + "/ProductConfigurator/Save",
    type: 'POST',
    async: true,
    data: postData,
    success: function (result) {
    if (result.Message == "Successfully Saved.") {
    // alert("Saved successfully");
    LoadDetailView(parseInt(result.PrimaryKey), function () { $('#lblWarning').text(result.Message) });

    if ($("#divKendoGrid").data("kendoGrid")) {
    $("#divKendoGrid").data("kendoGrid").dataSource.read();
    }  

    }
    else {
    $('#lblWarning').text(result.Message)
    //alert("Not saved");
    }
    },
    error: function (result) {
    $('#lblWarning').text("Error occurred")
    //alert("error occurred");
    }
    });
        
    }
    catch (err) {
    //$("#lblNumericPhone").css("display", "block");
    }*/
}

function RemoveProduct(primaryKey) {
    if (confirm("Are you sure to delete this Product?")) {
        $.ajax({
            url: baseUrl + "/ProductConfigurator/Delete",
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
                    alert("This Product  cannot be deleted");
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
        if (confirm("Are you sure to delete " + totalRecordToBeDeleted + " Product(s)?")) {
            $('.k-grid-content table input:checkbox:checked').each(function (index, element) {
                if (index == totalRecordToBeDeleted - 1) {//last record
                    removeKendoGridRow(this.value, baseUrl + "/ProductConfigurator/Delete",
                    function (successfuldelete) {
                        if (successfuldelete == "Delete failed.") {
                            countnodeletes++;
                        }
                        else {
                            countdeletes++;
                        }
                        if (countnodeletes > 0) {
                            alert(countnodeletes + " out of " + countdeletes + " Product(s) are not drivers and cannot be deleted");
                        }
                        $("#divKendoGrid").data("kendoGrid").dataSource.read();
                    });
                }
                else { // all but last record
                    removeKendoGridRow(this.value, baseUrl + "/ProductConfigurator/Delete",
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