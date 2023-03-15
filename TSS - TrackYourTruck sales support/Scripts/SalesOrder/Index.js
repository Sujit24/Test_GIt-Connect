var salesPersonList = [];
var maxProductCount = 1;

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
    LoadOrderStatus();
    LoadSalesPerson();

    LoadSalesTaxList();
    LoadStateDDL('US', $('#billState'));
    LoadStateDDL('US', $('#shipState'));

    LoadNettrackStatusList();

    $("#aConvertToNetTrack").click(function (e) {
        e.preventDefault();

        $("#ConvertToNetTrackClient-Modal").modal("show");
    });

    $("#aAddAsNewClient").click(function (e) {
        e.preventDefault();

        $("#ConvertToNetTrackClient-Modal").modal("hide");
        $("#Zuhu-Modal").modal("show");
        $("#btnZohoSearchNext200").attr('disabled', 'disabled');
    });

    $("#aAddOrLookupExistingClientInNetTrack").click(function (e) {
        e.preventDefault();

        $("#ConvertToNetTrackClient-Modal").modal("hide");
        $("#NetTrackClient-Modal").modal("show");

        $('#ddlNetTrackStatus').val($("#ddlOrderType").val());
    });

    $("#Search-NetTrackClient-btn").click(function (e) {
        NetTrackClientSearch($("#Search-NetTrackClient-Text").val());
    });

    $("#sameAsBillTo").click(function () {
        if ($(this).is(":checked")) {
            $("#shipCompanyName").val($("#billCompanyName").val());
            $("#shipCountry").val($("#billCountry").val());
            LoadStateDDL($('#shipCountry').val(), $('#shipState'));
            $("#shipAddress1").val($("#billAddress1").val());
            $("#shipAddress2").val($("#billAddress2").val());
            $("#shipCity").val($("#billCity").val());
            $("#shipState").val($("#billState").val());
            $("#shipZip").val($("#billZip").val());
            $("#shipContact").val($("#billContact").val());
            $("#shipEmail").val($("#billEmail").val());
            $("#shipPhone").val($("#billPhone").val());
        } else {
            $("#shipCompanyName").val('');
            $("#shipAddress1").val('');
            $("#shipCity").val('');
            $("#shipState").val('');
            $("#shipZip").val('');
            $("#shipContact").val('');
            $("#shipEmail").val('');
            $("#shipPhone").val('');
        }
    });

    //InitProductContainer();

    if ($("#quoteOrderId").val() != "") {
        Edit($("#quoteOrderId").val());
    }
    else {
        $("#quoteNumber").text("Auto Generated");
    }

    $("#Search-Zuhu-btn").click(function (e) {
        zohoCount = 0;
        ZohoSearch();
    });

    $("#btnZohoSearchNext200").click(function () {
        ZohoSearch(zohoCount + 1, zohoCount + 200);
    });

    loadTimezone();

    $("#billCountry").change(function () {
        LoadStateDDL($(this).val(), $('#billState'));
    });

    $("#shipCountry").change(function () {
        LoadStateDDL($(this).val(), $('#shipState'));
    });
});

var zohoCount = 0;
function ZohoSearch(from, to) {
    var fromIndex = from || 0;
    var toIndex = to || 200;

    zohoCount = toIndex;

    ListViewUtility.ShowLoadingImage();

    $('#btnSaveZohoContact').attr('disabled', true);
    $('#dvAccountList').html('');
    $('#dvContactList').html('');

    var requestUrl = baseUrl + "/Client/SearchZoho";
    $.ajax({
        url: requestUrl,
        type: 'POST',
        async: true,
        data: { searchText: $('#Search-Zuhu-pattern').val(), fromIndex: fromIndex, toIndex: toIndex },
        dataType: "json",
        success: function (result) {
            var jsonResult = result == '' ? null : eval("(" + result + ")");
            var accountList = [];
            var accountId = '', accountName = '';

            if (jsonResult != null && jsonResult.data) {
                $.each(jsonResult.data, function (i, row) {
                    accountId = row.id;
                    accountName = row.Account_Name;

                    accountList.push({
                        id: accountId, name: accountName
                    });

                    accountId = '', accountName = '';
                });

                accountList = _.sortBy(accountList, 'name');

                var htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto; text-align: center;">'
            + '<colgroup>'
                + '<col style="width:30px">'
                + '<col style="width:190px">'
                + '<col>'
            + '</colgroup>'
            + '<thead><tr>'
                + '<th class="k-header"></th>'
                + '<th class="k-header"><a href="#" class="k-link">Account ID</a></th>'
                + '<th class="k-header"><a href="#" class="k-link">Account Name</a></th>'
            + '</tr></thead><tbody>';
                $.each(accountList, function (idx, obj) {
                    //console.log(idx);
                    htmlStr += '<tr ' + (idx % 2 == 1 ? 'class="k-alt"' : '') + ' >'
                + '<td ><input type="radio" name="rdbAccount" onclick="GetContacts(\'' + obj.id + '\')"/></td>'
                + '<td >' + obj.id + '</td>'
                + '<td >' + obj.name + '</td>'
            + '</tr>';
                });
                htmlStr += '</tbody></table>';

                $("#btnZohoSearchNext200").removeAttr('disabled');
            } else {
                htmlStr = "No Data Found";

                $("#btnZohoSearchNext200").attr('disabled', 'disabled');
            }


            $('#dvAccountList').html(htmlStr);
            ListViewUtility.CloseLoadingImage();
            $("#dvAccountList-wrapper").css('display', 'block');
        },
        error: function (result) {
            ListViewUtility.CloseLoadingImage();
        }
    });
}

function NetTrackClientSearch(searchText) {
    ListViewUtility.ShowLoadingImage();

    $('#btnSelectClient').attr('disabled', true);
    $('#divNetTrackClientList').html('');

    $.ajax({
        url: baseUrl + "/Quote/GetNetTrackClientList",
        type: 'POST',
        async: true,
        data: { clientName: searchText },
        dataType: "json",
        success: function (result) {
            var htmlStr = "";

            if (result.length > 0) {
                htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto; text-align: center;">'
				+ '<colgroup>'
					+ '<col style="width:30px">'
					+ '<col style="width:190px">'
					+ '<col>'
				+ '</colgroup>'
				+ '<thead><tr>'
					+ '<th class="k-header"></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Client ID</a></th>'
					+ '<th class="k-header"><a href="#" class="k-link">NetTrack Client Name</a></th>'
				+ '</tr></thead><tbody>';

                $.each(result, function (idx, obj) {
                    htmlStr += '<tr ' + (idx % 2 == 1 ? 'class="k-alt"' : '') + ' >'
					+ '<td ><input type="radio" name="rdbClientEntity" onclick="clientSelected();" ClientName="' + obj.ClientName + '" ClientId="' + obj.ClientID + '"/></td>'
					+ '<td >' + obj.ClientID + '</td>'
					+ '<td >' + obj.ClientName + '</td>'
				+ '</tr>';
                });
                htmlStr += '</tbody></table>'
            }
            else {
                htmlStr = "No Data Found";
            }

            $('#divNetTrackClientList').html(htmlStr);
            ListViewUtility.CloseLoadingImage();
        },
        error: function (result) {
            ListViewUtility.CloseLoadingImage();
        }
    });
}

function clientSelected() {
    $("#btnSelectClient").removeAttr('disabled');
}

function SelectClient() {
    ShowLoadingMessage("Processing...");
    ListViewUtility.ShowLoadingImage();

    var clientEntity = $("input:radio[name=rdbClientEntity]:checked");
    var clientId = clientEntity.attr("ClientId");
    var clientName = clientEntity.attr("ClientName");

    $("#clientId").val(clientId);

    var postData = {};
    postData.QuoteOrderModel = {};
    postData.QuoteOrderModel.QuoteId = $("#quoteId").val() == "" ? "0" : $("#quoteId").val();
    postData.QuoteOrderModel.QuoteOrderId = $("#quoteOrderId").val();
    postData.QuoteOrderModel.ClientID = $("#clientId").val();
    postData.QuoteOrderModel.NettrackClientStatusId = $("#ddlNetTrackStatus").val();

    try {
        $.ajax({
            url: baseUrl + "/SalesOrder/SaveNettrackStatus",
            type: 'POST',
            async: true,
            data: JSON.stringify(postData),
            dataType: 'json',
            contentType: "application/json",
            success: function (result) {
                HideLoadingMessage();
                ListViewUtility.CloseLoadingImage();

                if (result.Message == "Successfully Saved.") {
                    $('.clientName label').text(clientName);
                    $('.clientName').show();
                    $("#nettrackClientStatusId").val(postData.QuoteOrderModel.NettrackClientStatusId);

                    $("#aConvertToNetTrack").hide();
                    $("#aConvertToNetTrack").closest("div").css("padding-left", "63%");

                    alert("Order Successfully Converted to Nettrack Client.");

                    $('#NetTrackClient-Modal').modal('hide');
                }
                else {
                    alert("Order Save Failed.");
                    $('#lblWarning').text(result.Message)
                }
            },
            error: function (result) {
                HideLoadingMessage();
                ListViewUtility.CloseLoadingImage();

                $('#lblWarning').text("Error occurred")
            }
        });

    }
    catch (err) {
    }
}

var leadList = [];
function ProcessLeads(data) {
    var jsonResult = eval("(" + data + ")")
    var leadId = '', companyName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
    leadList = [];

    if (jsonResult.response.nodata == undefined) {
        if (jsonResult.response.result.Leads.row.length != undefined) {
            $.each(jsonResult.response.result.Leads.row, function (idx, obj) {
                $.each(obj.FL, function (idx1, obj1) {
                    if (obj1.val == 'LEADID') {
                        leadId = obj1.content;
                    }
                    if (obj1.val == 'Company') {
                        companyName = obj1.content;
                    }
                    if (obj1.val == 'SMOWNERID') {
                        SMOWNERID = obj1.content;
                    }
                    if (obj1.val == 'Street') {
                        street = obj1.content;
                    }
                    if (obj1.val == 'City') {
                        city = obj1.content;
                    }
                    if (obj1.val == 'State') {
                        state = obj1.content;
                    }
                    if (obj1.val == 'Zip Code') {
                        zipCode = obj1.content;
                    }
                    if (obj1.val == 'First Name') {
                        firstName = obj1.content;
                    }
                    if (obj1.val == 'Last Name') {
                        lastName = obj1.content;
                    }
                    if (obj1.val == 'Email') {
                        email = obj1.content;
                    }
                    if (obj1.val == 'Phone') {
                        phone = obj1.content;
                    }
                });
                leadList.push({ id: leadId, name: companyName, SMOWNERID: SMOWNERID, street: street, city: city, state: state, zipCode: zipCode, firstName: firstName, lastName: lastName, email: email, phone: phone });
                leadId = '', companyName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
            });
        }
        else {
            var obj = jsonResult.response.result.Leads.row;
            $.each(obj.FL, function (idx1, obj1) {
                if (obj1.val == 'LEADID') {
                    leadId = obj1.content;
                }
                if (obj1.val == 'Company') {
                    companyName = obj1.content;
                }
                if (obj1.val == 'SMOWNERID') {
                    SMOWNERID = obj1.content;
                }
                if (obj1.val == 'Street') {
                    street = obj1.content;
                }
                if (obj1.val == 'City') {
                    city = obj1.content;
                }
                if (obj1.val == 'State') {
                    state = obj1.content;
                }
                if (obj1.val == 'Zip Code') {
                    zipCode = obj1.content;
                }
                if (obj1.val == 'First Name') {
                    firstName = obj1.content;
                }
                if (obj1.val == 'Last Name') {
                    lastName = obj1.content;
                }
                if (obj1.val == 'Email') {
                    email = obj1.content;
                }
                if (obj1.val == 'Phone') {
                    phone = obj1.content;
                }
            });
            leadList.push({ id: leadId, name: companyName, SMOWNERID: SMOWNERID, street: street, city: city, state: state, zipCode: zipCode, firstName: firstName, lastName: lastName, email: email, phone: phone });
            leadId = '', companyName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
        }

        var htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto; text-align: center;">'
				+ '<colgroup>'
					+ '<col style="width:30px">'
					+ '<col style="width:190px">'
					+ '<col>'
				+ '</colgroup>'
				+ '<thead><tr>'
					+ '<th class="k-header"></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Lead ID</a></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Company Name</a></th>'
				+ '</tr></thead><tbody>';
        $.each(leadList, function (idx, obj) {
            htmlStr += '<tr ' + (idx % 2 == 1 ? 'class="k-alt"' : '') + ' >'
					+ '<td ><input type="radio" name="rdbZohoEntity" onclick="contactSelected();"  ZohoEntityType="lead" ZohoEntityId="' + obj.id + '"/></td>'
					+ '<td >' + obj.id + '</td>'
					+ '<td >' + obj.name + '</td>'
				+ '</tr>';
        });
        htmlStr += '</tbody></table>'
    } else {
        htmlStr = "No Data Found";
    }

    $('#dvAccountList').html(htmlStr);
    ListViewUtility.CloseLoadingImage();
    $("#dvAccountList-wrapper").css('display', 'block');

    salesPersonList = leadList;
}

var contactList = [];
function ProcessContacts(data) {
    var jsonResult = eval("(" + data + ")")
    var contactId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
    contactList = [];

    if (jsonResult.response.nodata == undefined) {
        if (jsonResult.response.result.Contacts.row.length != undefined) {
            $.each(jsonResult.response.result.Contacts.row, function (idx, obj) {
                $.each(obj.FL, function (idx1, obj1) {
                    if (obj1.val == 'CONTACTID') {
                        contactId = obj1.content;
                    }
                    if (obj1.val == 'Account Name') {
                        accountName = obj1.content;
                    }
                    if (obj1.val == 'SMOWNERID') {
                        SMOWNERID = obj1.content;
                    }
                    if (obj1.val == 'Mailing Street') {
                        street = obj1.content;
                    }
                    if (obj1.val == 'Mailing City') {
                        city = obj1.content;
                    }
                    if (obj1.val == 'Mailing State') {
                        state = obj1.content;
                    }
                    if (obj1.val == 'Mailing Zip') {
                        zipCode = obj1.content;
                    }
                    if (obj1.val == 'First Name') {
                        firstName = obj1.content;
                    }
                    if (obj1.val == 'Last Name') {
                        lastName = obj1.content;
                    }
                    if (obj1.val == 'Email') {
                        email = obj1.content;
                    }
                    if (obj1.val == 'Phone') {
                        phone = obj1.content;
                    }
                });
                contactList.push({ id: contactId, name: accountName, SMOWNERID: SMOWNERID, street: street, city: city, state: state, zipCode: zipCode, firstName: firstName, lastName: lastName, email: email, phone: phone });
                contactId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
            });
        }
        else {
            var obj = jsonResult.response.result.Contacts.row;
            $.each(obj.FL, function (idx1, obj1) {

                if (obj1.val == 'CONTACTID') {
                    contactId = obj1.content;
                }
                if (obj1.val == 'Account Name') {
                    accountName = obj1.content;
                }
                if (obj1.val == 'SMOWNERID') {
                    SMOWNERID = obj1.content;
                }
                if (obj1.val == 'Mailing Street') {
                    street = obj1.content;
                }
                if (obj1.val == 'Mailing City') {
                    city = obj1.content;
                }
                if (obj1.val == 'Mailing State') {
                    state = obj1.content;
                }
                if (obj1.val == 'Mailing Zip') {
                    zipCode = obj1.content;
                }
                if (obj1.val == 'First Name') {
                    firstName = obj1.content;
                }
                if (obj1.val == 'Last Name') {
                    lastName = obj1.content;
                }
                if (obj1.val == 'Email') {
                    email = obj1.content;
                }
                if (obj1.val == 'Phone') {
                    phone = obj1.content;
                }
            });
            contactList.push({ id: contactId, name: accountName, SMOWNERID: SMOWNERID, street: street, city: city, state: state, zipCode: zipCode, firstName: firstName, lastName: lastName, email: email, phone: phone });
            contactId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
        }

        var htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto; text-align: center;">'
				+ '<colgroup>'
					+ '<col style="width:30px">'
					+ '<col style="width:190px">'
					+ '<col>'
				+ '</colgroup>'
				+ '<thead><tr>'
					+ '<th class="k-header"></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Contact ID</a></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Account Name</a></th>'
				+ '</tr></thead><tbody>';
        $.each(contactList, function (idx, obj) {
            htmlStr += '<tr ' + (idx % 2 == 1 ? 'class="k-alt"' : '') + ' >'
					+ '<td ><input type="radio" name="rdbZohoEntity" onclick="contactSelected();"  ZohoEntityType="contact" ZohoEntityId="' + obj.id + '"/></td>'
					+ '<td >' + obj.id + '</td>'
					+ '<td >' + obj.name + '</td>'
				+ '</tr>';
        });
        htmlStr += '</tbody></table>'
    } else {
        htmlStr = "No Data Found";
    }


    $('#dvAccountList').html(htmlStr);
    ListViewUtility.CloseLoadingImage();
    $("#dvAccountList-wrapper").css('display', 'block');

    salesPersonList = contactList;
}

var accountList = [];
function ProcessAccounts(data) {
    var jsonResult = eval("(" + data + ")")
    var accountId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', phone = '';
    accountList = [];

    if (jsonResult.response.nodata == undefined) {
        if (jsonResult.response.result.Accounts.row.length != undefined) {
            $.each(jsonResult.response.result.Accounts.row, function (idx, obj) {
                $.each(obj.FL, function (idx1, obj1) {

                    if (obj1.val == 'ACCOUNTID') {
                        accountId = obj1.content;
                    }
                    if (obj1.val == 'Account Name') {
                        accountName = obj1.content;
                    }
                    if (obj1.val == 'SMOWNERID') {
                        SMOWNERID = obj1.content;
                    }
                    if (obj1.val == 'Billing Street') {
                        street = obj1.content;
                    }
                    if (obj1.val == 'Billing City') {
                        city = obj1.content;
                    }
                    if (obj1.val == 'Billing State') {
                        state = obj1.content;
                    }
                    if (obj1.val == 'Billing Code') {
                        zipCode = obj1.content;
                    }
                    if (obj1.val == 'Phone') {
                        phone = obj1.content;
                    }
                });
                accountList.push({ id: accountId, name: accountName, SMOWNERID: SMOWNERID, street: street, city: city, state: state, zipCode: zipCode, phone: phone });
                accountId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', phone = '';
            });
        }
        else {
            var obj = jsonResult.response.result.Accounts.row;
            $.each(obj.FL, function (idx1, obj1) {

                if (obj1.val == 'ACCOUNTID') {
                    accountId = obj1.content;
                }
                if (obj1.val == 'Account Name') {
                    accountName = obj1.content;
                }
                if (obj1.val == 'SMOWNERID') {
                    SMOWNERID = obj1.content;
                }
                if (obj1.val == 'Billing Street') {
                    street = obj1.content;
                }
                if (obj1.val == 'Billing City') {
                    city = obj1.content;
                }
                if (obj1.val == 'Billing State') {
                    state = obj1.content;
                }
                if (obj1.val == 'Billing Code') {
                    zipCode = obj1.content;
                }
                if (obj1.val == 'Phone') {
                    phone = obj1.content;
                }
            });
            accountList.push({ id: accountId, name: accountName, SMOWNERID: SMOWNERID, street: street, city: city, state: state, zipCode: zipCode, phone: phone });
            accountId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', phone = '';
        }

        var htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto; text-align: center;">'
				+ '<colgroup>'
					+ '<col style="width:30px">'
					+ '<col style="width:190px">'
					+ '<col>'
				+ '</colgroup>'
				+ '<thead><tr>'
					+ '<th class="k-header"></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Account ID</a></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Account Name</a></th>'
				+ '</tr></thead><tbody>';
        $.each(accountList, function (idx, obj) {
            //console.log(idx);
            htmlStr += '<tr ' + (idx % 2 == 1 ? 'class="k-alt"' : '') + ' >'
					+ '<td ><input type="radio" name="rdbZohoEntity" onclick="contactSelected();" ZohoEntityType="account" ZohoEntityId="' + obj.id + '"/></td>'
					+ '<td >' + obj.id + '</td>'
					+ '<td >' + obj.name + '</td>'
				+ '</tr>';
        });
        htmlStr += '</tbody></table>'
    } else {
        htmlStr = "No Data Found";
    }

    $('#dvAccountList').html(htmlStr);
    ListViewUtility.CloseLoadingImage();
    $("#dvAccountList-wrapper").css('display', 'block');

    salesPersonList = accountList;
}

function contactSelected() {
    $("#btnSaveZoho").removeAttr('disabled');
}

function LoadProductGrid(filterValue, context) {
    var tmpMaxProdCount = 1;

    $.each(context.find(".product-quantity"), function (index, item) {
        if ($(item).val() > tmpMaxProdCount) {
            tmpMaxProdCount = $(item).val();
        }
    });

    context.find("h2").attr("data-maxproductqty", tmpMaxProdCount);

    var columns = '[';
    //columns += '{"template": "\u003cinput id=\u0027delete${ ProductId }\u0027 value=\u0027${ ProductId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
    columns += '{"width":"150px", "field":"SKU","title":"SKU"},';
    columns += '{"field":"ProductName","title":"Product Name"},';
    //columns += '{"field":"ProductDescription","title":"Product Description"},';
    columns += '{"width":"100px", "field":"Price","title":"Price",format:"{0:c2}"},';
    //columns += '{"field":"ProductTypeName","title":"Type"},';
    columns += '{"width":"70px", "title":"Quantity", "template":"\u003cinput type=text value =' + tmpMaxProdCount + ' class=quantity style=\u0027width: 40px\u0027 \u003e"},';
    columns += '{"width":"60px", "title":"","template":"\u003ca class=\u0027add-product btn btn-primary\u0027 product-id=\u0027${ ProductId }\u0027 \u003eAdd\u003c/a\u003e"}';
    columns += ']';

    $("#divKendoGridProductList").kendoGrid().empty();

    var gridDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "POST",
                dataType: "json",
                url: function () {
                    return baseUrl + "/ProductConfigurator/GetProductList";
                },
                success: function (data) {
                    //console.log("success");
                }
            }
        },
        filter: { field: "ProductTypeName", operator: "contains", value: filterValue }
    });

    $("#divKendoGridProductList").kendoGrid({
        dataSource: gridDataSource,
        columns: eval(columns),
        sortable: true,
        scrollable: true,
        dataBound: divKendoGridProductListDataBound
    });
}

// applyFilter function accepts the Field Name and the new value to use for filter.
function applyFilter(filterField, filterValue) {

    // get the kendoGrid element.
    var gridData = $("#divKendoGridProductList").data("kendoGrid");

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
    var gridData = $("#divKendoGridProductList").data("kendoGrid");
    gridData.dataSource.filter({});
}

function divKendoGridProductListDataBound() {
    addExtraStylingToGrid();

    $(".add-product").click(function (e) {
        e.preventDefault();

        var content = $("#productContent");
        if (activeProductIndex == 999) content = $("#optionalService");
        var currentProductDiv = content.find('[product-index=#' + activeProductIndex + ']');

        var aAddProduct = $(this);
        var productId = aAddProduct.attr("product-id");
        var quantity = aAddProduct.parent().parent().find(".quantity").val();
        var newRecord = null;


        if ($("[name=ProductOrOptionalServices]:checked").val() == "product") {
            var divKendoGridProductListData = $("#divKendoGridProductList").data("kendoGrid").dataSource.data();
            for (i = 0; i < divKendoGridProductListData.length; i++) {
                var item = divKendoGridProductListData[i];
                if (item.ProductId == productId) {
                    newRecord = { QuoteProductId: 0, ProductId: item.ProductId, Quantity: quantity, SKU: item.SKU, ProductDescription: item.ProductDescription, Price: item.Price, ExtendedPrice: (item.Price * quantity) };
                    break;
                }
            }
        }
        else {
            var divKendoGridProductListData = $("#divKendoGridProductList").data("kendoGrid").dataSource.data();
            for (i = 0; i < divKendoGridProductListData.length; i++) {
                var item = divKendoGridProductListData[i];
                if (item.ProductId == productId) {
                    newRecord = { QuoteProductId: 0, ProductId: item.ProductId, Quantity: quantity, SKU: item.SKU, ProductDescription: item.ProductDescription, Price: item.Price, ExtendedPrice: (item.Price * quantity) };
                    break;
                }
            }
        }

        if (newRecord != null) {
            AddNewProduct(currentProductDiv, newRecord);
        }
    });
}

function AddNewProduct(currentProductDiv, newRecord) {
    var newRow = "<tr class='productDetail' deleted='0'>";
    newRow += "<td style='text-align: center;'><input class='quote-product-id' type='hidden' value='" + newRecord.SalesOrderId + "'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><span class='product-quantity' product-quantity='" + newRecord.Quantity + "'>" + newRecord.Quantity + "</span></td>";
    newRow += "<td>" + newRecord.SKU + "</td>";
    newRow += "<td>" + newRecord.ProductDescription + "</td>";
    if (productType == "discount") {
        newRow += "<td class='product-price' style='text-align: right; padding-right: 20px;'>-$" + newRecord.Price.toFixed(2).replace('-', '') + "</td>";
        newRow += "<td style='text-align: right'><span style='padding-right: 40px' class='product-extprice'>-$" + (newRecord.Quantity * newRecord.Price).toFixed(2).replace('-', '') + "</span></td>";
    } else {
        newRow += "<td class='product-price' style='text-align: right; padding-right: 20px;'>$" + newRecord.Price.toFixed(2) + "</td>";
        newRow += "<td style='text-align: right'><span style='padding-right: 40px' class='product-extprice'>$" + (newRecord.Quantity * newRecord.Price).toFixed(2) + "</span></td>";
    }
    newRow += "</tr>";

    var dataMaxProductQty = currentProductDiv.closest("[class=Package]").find("h2");
    maxProductCount = dataMaxProductQty.attr("data-maxproductqty");

    var addedProduct = currentProductDiv.find(".productTable").find("span[product-id=" + newRecord.ProductId + "]");
    if (addedProduct.length > 0) {
        var rowItem = addedProduct.closest('tr');
        var productQuantity = rowItem.find('.product-quantity');
        var productQuantityValue = parseInt(productQuantity.val()) + parseInt(newRecord.Quantity);

        if (rowItem.closest("table").attr("product-category") == "Hardware" && productType != "discount") {
            dataMaxProductQty.attr("data-maxproductqty", productQuantityValue);
        }

        if (productType == "discount") {
            var productPrice = rowItem.find('.product-price').html().substr(2);

            productQuantity.val(productQuantityValue);
            rowItem.find('.product-extprice').html('-$' + (productQuantityValue * productPrice).toFixed(2).replace('-', ''));
        } else {
            var productPrice = rowItem.find('.product-price').html().substr(1);

            productQuantity.val(productQuantityValue);
            rowItem.find('.product-extprice').html('$' + (productQuantityValue * productPrice).toFixed(2));
        }
    }
    else {
        if (currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType != "discount") {
            dataMaxProductQty.attr("data-maxproductqty", newRecord.Quantity);
        }

        currentProductDiv.find(".productTable").append(newRow);
    }

    $(".remove-item").unbind();
    $(".remove-item").click(function (e) {
        e.preventDefault();
        var deletedRow = $(this).closest('tr');
        deletedRow.hide();
        deletedRow.attr("deleted", "1");
        deletedRow.find(".product-extprice").text("$0.00");

        SumProductCost(currentProductDiv);
        ProductSummary(currentProductDiv.parent());
    });

    $(".product-quantity").unbind();
    $(".product-quantity").change(function () {
        var productQuantity = $(this);
        currentProductDiv = productQuantity.closest(".product");
        var rowItem = productQuantity.closest('tr');

        if (rowItem.find('.product-price').html()[0] == "-") {
            var productPrice = rowItem.find('.product-price').html().substr(2);
            rowItem.find('.product-extprice').html('-$' + (productQuantity.val() * productPrice).toFixed(2));
        } else {
            var productPrice = rowItem.find('.product-price').html().substr(1);
            rowItem.find('.product-extprice').html('$' + (productQuantity.val() * productPrice).toFixed(2));
        }

        if (productQuantity.closest("table").attr("product-category") == "Hardware" && rowItem.find('.product-price').html()[0] != "-") {
            dataMaxProductQty = rowItem.closest("[class=Package]").find("h2");
            maxProductCount = dataMaxProductQty.attr("data-maxproductqty");
            dataMaxProductQty.attr("data-maxproductqty", productQuantity.val());
        }

        MakeSameQuantity(rowItem.closest("[class=Package]"));
        SumProductCost(currentProductDiv);
        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
        }
        ProductSummary(currentProductDiv.parent());
        MakeSummary();
    });

    //MakeSameQuantity(currentProductDiv.closest("[class=Package]"));
    SumProductCost(currentProductDiv);
    if (currentProductDiv.find("table").attr("product-category") == "Hardware") {
        SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
    }
    ProductSummary(currentProductDiv.parent());
    MakeSummary();
}

function MakeSameQuantity(currentProductDiv) {
    maxProductCount = currentProductDiv.find("h2").attr("data-maxproductqty");

    $.each(currentProductDiv.find(".product-quantity"), function (index, item) {
        var productQuantity = $(this);
        var productCategory = productQuantity.closest("table").attr("product-category");
        if (productCategory == "Hardware" || productCategory == "Monthly" || productCategory == "Misc Fee") {
            var rowItem = productQuantity.closest('tr');

            productQuantity.val(maxProductCount);

            if (rowItem.find('.product-price').html()[0] == "-") {
                var productPrice = rowItem.find('.product-price').html().substr(2);
                rowItem.find('.product-extprice').html('-$' + (productQuantity.val() * productPrice).toFixed(2));
            } else {
                var productPrice = rowItem.find('.product-price').html().substr(1);
                rowItem.find('.product-extprice').html('$' + (productQuantity.val() * productPrice).toFixed(2));
            }
        }
    });
}

var packageTitleIndex = 1;
var currentProductIndex = 1;
var currentPackageIndex = 2;
var activeProductIndex;
function InitProductContainer() {
    $("#productContent").empty();

    AddMoreProduct1();

    $("#aAddMoreProduct").click(function () {
        AddMoreProduct1();

        currentPackageIndex++;
    });
}

function AddMoreProduct() {
    var templateContent = $("#productTemplate").html();
    var template = kendo.template(templateContent);

    var data = [{ "Title": "Product #" + currentProductIndex, "ProductIndex": '#' + currentProductIndex }];
    var result = kendo.render(template, data);
    $("#productContent").append(result);

    BindProductEvent();

    currentProductIndex++;
}

function AddMoreProduct1() {
    var templateContent = $("#productTemplate").html();
    var template = kendo.template(templateContent);

    var data = [{ "PackageTitleIndex": packageTitleIndex, "ProductIndex": '#' + currentProductIndex }];
    var result = kendo.render(template, data);
    $("#productContent").append(result);

    $("[product-index=#" + currentProductIndex + "]").find("[product-type=product]").hide();
    $("[product-index=#" + currentProductIndex + "]").find("[product-type=monthly]").hide();
    $("[product-index=#" + currentProductIndex + "]").find("[product-type=service]").hide();

    $(".remove-package").unbind();
    $(".remove-package").click(function (e) {
        e.preventDefault();

        var context = $(this);
        context.closest(".Package").hide();

        var idx = 1;
        $.each($("h2.PackageId"), function (index, item) {
            var h2 = $(this);
            if (h2.is(":visible")) {
                h2.find(".package-title").text("Product " + idx);
                h2.data("packageid", idx);
                idx++;
            }
            else {
                h2.attr("data-deleted", "1");
            }
        });

        packageTitleIndex--;
    });

    BindProductEvent();

    currentProductIndex++;
    packageTitleIndex++;
}

var productType = "";
function BindProductEvent() {
    $(".product-action").unbind();
    $(".product-action").click(function (e) {
        e.preventDefault();

        var productAction = $(this);
        productType = productAction.attr("product-type");

        LoadProductGrid(productType, productAction.closest("[class=Package]"));
        activeProductIndex = productAction.closest(".product").attr("product-index").substr(1);

        $('#Product-Modal').modal('show');
    });
}

function SumProductCost(currentProductDiv) {
    var total = 0;

    $.each(currentProductDiv.find(".product-extprice"), function (index, item) {
        if ($(this).html()[0] == "-") {
            total += (-parseFloat($(this).html().substr(2)));
        }
        else {
            total += parseFloat($(this).html().substr(1));
        }
    });

    if (total < 0) {
        currentProductDiv.find(".product-total").html("-$" + (-1 * total).toFixed(2));
    } else {
        currentProductDiv.find(".product-total").html("$" + total.toFixed(2));
    }

    SumUnitProductCost(currentProductDiv);
}

function SumUnitProductCost(currentProductDiv) {
    var total = 0;

    $.each(currentProductDiv.find(".product-price"), function (index, item) {
        if ($(this).html()[0] == "-") {
            total += (-parseFloat($(this).html().substr(2)));
        }
        else {
            total += parseFloat($(this).html().substr(1));
        }
    });

    if (total < 0) {
        currentProductDiv.find(".unit-product-total").html("-$" + (-1 * total).toFixed(2));
    } else {
        currentProductDiv.find(".unit-product-total").html("$" + total.toFixed(2));
    }
}

function ProductSummary(currentProductDiv) {
    var total = 0;

    $.each(currentProductDiv.find(".product-total"), function (index, item) {
        if ($(this).html()[0] == "-") {
            total += (-parseFloat($(this).html().substr(2)));
        }
        else {
            total += parseFloat($(this).html().substr(1));
        }
    });

    total += parseFloat($("#shippingAndHandling").val());

    if (total < 0) {
        currentProductDiv.find(".product-summary").html("-$" + (-1 * total).toFixed(2));
    } else {
        currentProductDiv.find(".product-summary").html("$" + total.toFixed(2));
    }
}

var tytSalesPersonList = [];
function LoadSalesPerson() {
    $.ajax({
        url: baseUrl + "/Quote/GetSalesPerson",
        type: 'POST',
        async: false,
        dataType: "json",
        success: function (result) {
            tytSalesPersonList = result;

            $.each(result, function (index, item) {
                $("#salesPerson").append("<option value='" + item.EmployeeId + "'>" + item.Name + "</option>");
            });

            var salesPerson = $("#salesPerson");
            salesPerson.val(employeeId);
            if (salesPerson.val() == employeeId) {
                salesPerson.attr("disabled", "disabled");
            }

            SetTYTSalesPerson();
        }
    });

    $("#salesPerson").change(function () {
        SetTYTSalesPerson();
    });
}

function SetTYTSalesPerson() {
    $.each(tytSalesPersonList, function (index, item) {
        if (item.EmployeeId == $("#salesPerson").val()) {
            $("#salesPersonEmail").text(item.Email);
            $("#salesPersonCell").text(item.CellPhone);
        }
    });
}

function Save() {
    var postData = {};
    postData.QuoteOrderModel = {};
    postData.QuoteOrderModel.QuoteId = $("#quoteId").val() == "" ? "0" : $("#quoteId").val();
    postData.QuoteOrderModel.QuoteOrderId = $("#quoteOrderId").val();
    postData.QuoteOrderModel.TssOrderTypeId = $("#ddlOrderType").val();
    postData.QuoteOrderModel.QuoteDate = $("#quoteDate").text();
    postData.QuoteOrderModel.ContractTerm = $("#contractTerm").val();
    postData.QuoteOrderModel.ValidUntil = $("#validUntil").val();
    postData.QuoteOrderModel.SalesPersonId = $("#salesPerson").val();
    postData.QuoteOrderModel.ZohoEntityId = $("#ZohoEntityId").val();
    postData.QuoteOrderModel.ZohoEntityType = $("#ZohoEntityType").val();
    postData.QuoteOrderModel.ClientID = $("#clientId").val();
    postData.QuoteOrderModel.NettrackClientStatusId = $("#nettrackClientStatusId").val();

    postData.QuoteOrderModel.BillToCompanyName = $("#billCompanyName").val();
    postData.QuoteOrderModel.BillToAddress1 = $("#billAddress1").val();
    postData.QuoteOrderModel.BillToAddress2 = $("#billAddress2").val();
    postData.QuoteOrderModel.BillToCity = $("#billCity").val();
    postData.QuoteOrderModel.BillToState = $("#billState").val();
    postData.QuoteOrderModel.BillToZip = $("#billZip").val();
    postData.QuoteOrderModel.BillToCountry = $("#billCountry").val();
    postData.QuoteOrderModel.BillToBillingContact = $("#billContact").val();
    postData.QuoteOrderModel.BillToBillingEmail = $("#billEmail").val();
    postData.QuoteOrderModel.BillToPhone = $("#billPhone").val();

    postData.QuoteOrderModel.IsShipSameAsBill = $("#sameAsBillTo").is(":checked");

    postData.QuoteOrderModel.ShipToCompanyName = $("#shipCompanyName").val();
    postData.QuoteOrderModel.ShipToAddress1 = $("#shipAddress1").val();
    postData.QuoteOrderModel.ShipToAddress2 = $("#shipAddress2").val();
    postData.QuoteOrderModel.ShipToCity = $("#shipCity").val();
    postData.QuoteOrderModel.ShipToState = $("#shipState").val();
    postData.QuoteOrderModel.ShipToZip = $("#shipZip").val();
    postData.QuoteOrderModel.ShipToCountry = $("#shipCountry").val();
    postData.QuoteOrderModel.ShipToBillingContact = $("#shipContact").val();
    postData.QuoteOrderModel.ShipToBillingEmail = $("#shipEmail").val();
    postData.QuoteOrderModel.ShipToPhone = $("#shipPhone").val();

    postData.QuoteOrderModel.ShippingAndHandling = $("#shippingAndHandling").text();

    postData.QuoteOrderModel.OrderStatusId = $("#orderStatus").val();
    postData.QuoteOrderModel.Note = $("#txtAdminNote").text();//1/20/2020

    try {
        $.ajax({
            url: baseUrl + "/SalesOrder/Save",
            type: 'POST',
            async: true,
            data: JSON.stringify(postData),
            dataType: 'json',
            contentType: "application/json",
            success: function (result) {
                if (result.Message == "Successfully Saved.") {
                    console.log(result);
                    alert("Order Successfully Saved.");
                    //location.href = baseUrl + "/SalesOrder/List";

                    $("#detailPage").hide();
                    LoadOrderList();
                }
                else {
                    alert("Order Save Failed.");
                    $('#lblWarning').text(result.Message)
                }
            },
            error: function (result) {
                $('#lblWarning').text("Error occurred")
            }
        });

    }
    catch (err) {
    }
}

function Edit(quoteOrderId) {
    try {
        $.ajax({
            url: baseUrl + "/SalesOrder/GetQuoteSales",
            type: 'POST',
            async: false,
            data: { id: quoteOrderId },
            dataType: 'json',
            success: function (result) {
                if (result.Quote != null) {

                    $("#orderNumber").text(result.Quote.QuoteOrderId);
                    $("#quoteId").val(result.Quote.QuoteId);
                    $("#ddlOrderType").val(result.Quote.TssOrderTypeId);
                    $("#refQuoteNumber").text(result.Quote.QuoteId);
                    $("#quoteDate").text(result.Quote.QuoteDateFormated);
                    $("#purchaseDate").text(result.Quote.PurchaseDateFormated);
                    $("#contractTerm").val(result.Quote.ContractTerm);
                    $("#validUntil").val(result.Quote.ValidUntil);
                    $("#salesPerson").val(result.Quote.SalesPersonId);
                    SetTYTSalesPerson();
                    $("#ZohoEntityId").val(result.Quote.ZohoEntityId);
                    $("#ZohoEntityType").val(result.Quote.ZohoEntityType);
                    $("#clientId").val(result.Quote.ClientId);
                    $("#nettrackClientStatusId").val(result.Quote.NettrackClientStatusId);

                    if (result.Quote.ClientId != 0) {
                        $("#aConvertToNetTrack").hide();
                        $("#aConvertToNetTrack").closest("div").css("padding-left", "63%");

                        $('.clientName label').text(result.Quote.ClientName);
                        $('.clientName').show();
                    }

                    if (result.Quote.NettrackClientStatusId != 0) {
                        $('.nettrackStatus label').text(result.Quote.NettrackStatus);
                        $('.nettrackStatus').show();
                    }

                    $("#billCompanyName").val(result.Quote.BillToCompanyName);
                    $("#billCountry").val(result.Quote.BillToCountry);
                    LoadStateDDL(result.Quote.BillToCountry, $('#billState'));
                    $("#billAddress1").val(result.Quote.BillToAddress1);
                    $("#billAddress2").val(result.Quote.BillToAddress2);
                    $("#billCity").val(result.Quote.BillToCity);
                    $("#billState").val(result.Quote.BillToState);
                    $("#billZip").val(result.Quote.BillToZip);
                    $("#billContact").val(result.Quote.BillToBillingContact);
                    $("#billEmail").val(result.Quote.BillToBillingEmail);
                    $("#billPhone").val(result.Quote.BillToPhone);

                    if (result.Quote.IsShipSameAsBill) {
                        $("#sameAsBillTo").attr("checked", "checked");
                    }

                    $("#shipCompanyName").val(result.Quote.ShipToCompanyName);
                    $("#shipCountry").val(result.Quote.ShipToCountry);
                    LoadStateDDL(result.Quote.ShipToCountry, $('#shipState'));
                    $("#shipAddress1").val(result.Quote.ShipToAddress1);
                    $("#shipAddress2").val(result.Quote.ShipToAddress2);
                    $("#shipCity").val(result.Quote.ShipToCity);
                    $("#shipState").val(result.Quote.ShipToState);
                    $("#shipZip").val(result.Quote.ShipToZip);
                    $("#shipContact").val(result.Quote.ShipToBillingContact);
                    $("#shipEmail").val(result.Quote.ShipToBillingEmail);
                    $("#shipPhone").val(result.Quote.ShipToPhone);

                    $("#txtAdminNote").text(result.Quote.Note);

                    if (result.Quote.ShipToCountry == "US") {
                        if (result.Quote.ShippingAndHandlingType == "Ground") {
                            $("#sShippingMethod").text("UPS Ground");
                        } else if (result.Quote.ShippingAndHandlingType == "2Day") {
                            $("#sShippingMethod").text("2nd day AIR");
                        } else if (result.Quote.ShippingAndHandlingType == "NextAir") {
                            $("#sShippingMethod").text("Next day AIR");
                        }
                    } else {
                        if (result.Quote.ShippingAndHandlingType == "Ground") {
                            $("#sShippingMethod").text("UPS Standard");
                        } else if (result.Quote.ShippingAndHandlingType == "2Day") {
                            $("#sShippingMethod").text("UPS Expedited");
                        } else if (result.Quote.ShippingAndHandlingType == "NextAir") {
                            $("#sShippingMethod").text("UPS Saver");
                        }
                    }

                    if (result.Quote.ShippingAndHandlingType == "Free") {
                        $("#sShippingMethod").text("Free");
                    }

                    $("#shippingAndHandling").text(result.Quote.ShippingAndHandling.toFixed(2));

                    $("#totalSalesTaxFees").text("$" + result.Quote.SalesTax.toFixed(2));

                    $("#orderStatus").val(result.Quote.OrderStatusId);

                    $("#paymentType").text(result.Quote.PaymentMethod);

                    if (result.Quote.PaymentMethodComment != "") {
                        $("#divNote").show();
                        $("#txtNote").text(result.Quote.PaymentMethodComment);
                    } else {
						$("#txtNote").text("");
                        $("#divNote").hide();
                    }

                    if (result.Quote.Processed) {
                        $('#aSaveToDB').attr('disabled', true);
                        $('#aSaveToDB').css('pointer-events', 'none');
                    }

                    var currentPackageId = 1;
                    $.each(result.SalesOrderList, function (index, quoteProduct) {
                        if (currentPackageId != quoteProduct.PackageId) {
                            AddMoreProduct1();
                            currentPackageIndex++;

                            currentPackageId = quoteProduct.PackageId;
                        }

                        var divPackage = $(".PackageId[data-packageid=#" + currentPackageId + "]").parent();
                        var currentProductDiv = divPackage.find("[product-category='" + quoteProduct.ProductCategory + "']").parent().parent();

                        productType = quoteProduct.Price < 0 ? "discount" : "";
                        AddNewProduct(currentProductDiv, quoteProduct);
                    });
                }
            }
        });

    }
    catch (err) {
    }
}

function MakeSummary() {
    var hardwareSummary = 0.0;
    var monthlySummary = 0.0;
    var optionalSummary = 0.0;
    var miscSummary = 0.0;

    $.each($("[product-category=Hardware]").parent().find(".product-total"), function (index, item) {
        hardwareSummary += parseFloat($(this).text().replace('$', ''));
    });
    $.each($("[product-category=Monthly]").parent().find(".product-total"), function (index, item) {
        monthlySummary += parseFloat($(this).text().replace('$', ''));
    });
    $.each($("[product-category=Optional]").parent().find(".product-total"), function (index, item) {
        optionalSummary += parseFloat($(this).text().replace('$', ''));
    });
    $.each($("[product-category='Misc Fee']").parent().find(".product-total"), function (index, item) {
        miscSummary += parseFloat($(this).text().replace('$', ''));
    });

    if (hardwareSummary < 0)
        $("#totalHardwareFees").text("-$" + hardwareSummary.toFixed(2).replace("-", ""));
    else
        $("#totalHardwareFees").text("$" + hardwareSummary.toFixed(2));

    if (monthlySummary < 0)
        $("#totalMonthlyFees").text("-$" + monthlySummary.toFixed(2).replace("-", ""));
    else
        $("#totalMonthlyFees").text("$" + monthlySummary.toFixed(2));

    if (optionalSummary < 0)
        $("#totalOptionalFees").text("-$" + optionalSummary.toFixed(2).replace("-", ""));
    else
        $("#totalOptionalFees").text("$" + optionalSummary.toFixed(2));

    if (miscSummary < 0)
        $("#totalMiscFees").text("-$" + miscSummary.toFixed(2).replace("-", ""));
    else
        $("#totalMiscFees").text("$" + miscSummary.toFixed(2));

    $("#totalShippingAndHandlingFees").text("$" + $("#shippingAndHandling").text());
    CalculateTotalCharged();

    $.each($(".product-price"), function (index, item) {
        var item = $(item);

        if (item.text().indexOf('-') > -1) {
            var tr = item.closest('tr');
            var td = tr.find("td");
            for (var tdi = 1; tdi < 3; tdi++) {
                td.eq(tdi).css("color", "red");
            }
            tr.find(".product-price").css("color", "red");
            tr.find(".product-extprice").css("color", "red");
        }
    });
}

function loadTimezone() {
    var hDDL = new Utility().loadTemplateWithParam(baseUrl + "/Common/LoadDDL", { 'spName': 'ug_Time_Zone' });
    $("#zuhu-modal-wrapper #TimeZone-wrapper").append(hDDL);
    $("#zuhu-modal-wrapper #TimeZone-wrapper select").val('CST');
}

function GetContacts(accountid) {
    ListViewUtility.ShowLoadingImage();

    $('#btnSaveZohoContact').attr('disabled', true);
    $('#dvContactList').html('');

    var htmlStr = "";
    var requestUrl = baseUrl + "/Client/GetZohoContacts";
    $.ajax({
        url: requestUrl,
        type: 'POST',
        async: true,
        data: { accountid: accountid },
        dataType: "json",
        success: function (result) {
            var jsonResult = result == '' ? null : eval("(" + result + ")");

            var contactList = [];
            var contactId = '', firstName = '', lastName = '', email = '';
            if (jsonResult != null && jsonResult.data) {
                $.each(jsonResult.data, function (i, row) {
                    contactId = row.id;
                    firstName = row.First_Name == null ? '' : row.First_Name;
                    lastName = row.Last_Name == null ? '' : row.Last_Name;
                    email = row.Email;

                    contactList.push({
                        id: contactId, firstName: firstName, lastName: lastName, email: email
                    });

                    contactId = '', firstName = '', lastName = '', email = '';
                });

                contactList = _.sortBy(contactList, 'firstName');

                htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto;text-align: center;">'
				+ '<colgroup>'
					+ '<col style="width:30px">'
					+ '<col>'
					+ '<col>'
                    + '<col>'
                    + '<col>'
				+ '</colgroup>'
                + '<thead><tr>'
					+ '<th class="k-header"></th>'
					+ '<th class="k-header"><a href="#" class="k-link">Contact ID</a></th>'
					+ '<th class="k-header"><a href="#" class="k-link">First Name</a></th>'
                    + '<th class="k-header"><a href="#" class="k-link">Last Name</a></th>'
                    + '<th class="k-header"><a href="#" class="k-link">Email</a></th>'
				+ '</tr></thead><tbody>';
                $.each(contactList, function (idx, obj) {
                    htmlStr += '<tr>'
					+ '<td ><input type="radio" name="rdbContact" onclick="contactSelected();"/></td>'
					+ '<td >' + obj.id + '</td>'
					+ '<td >' + obj.firstName + '</td>'
                    + '<td >' + obj.lastName + '</td>'
                    + '<td >' + obj.email + '</td>'
				+ '</tr>';
                });
                htmlStr += '</tbody></table>'

            } else {
                htmlStr = "No Data Found";
            }
            $('#dvContactList').html(htmlStr);
            ListViewUtility.CloseLoadingImage();
            $("#dvContactList-wrapper").css('display', 'block');
        },
        error: function (result) {
            ListViewUtility.CloseLoadingImage();
        }
    });

}

function contactSelected() {
    $("#btnSaveZohoContact").removeAttr('disabled');
}

function SaveZohoContact() {
    ShowLoadingMessage("Processing...");
    ListViewUtility.ShowLoadingImage();

    var zohoAccountid = '', zohoaccountName = '', zohoContactId = '', firstName = '', lastName = '', email = '', timezone = '';
    var orderId = $('#quoteOrderId').val();
    timezone = $("#zuhu-modal-wrapper #TimeZone-wrapper select").val();
    $("input:radio[name=rdbAccount]:checked").parent().parent().children().each(function (idx, val) {
        if (idx == 1) { zohoAccountid = $(val).html(); }
        if (idx == 2) { zohoaccountName = $(val).html(); }
    });

    $("input:radio[name=rdbContact]:checked").parent().parent().children().each(function (idx, val) {
        if (idx == 1) { zohoContactId = $(val).html(); }
        if (idx == 2) { firstName = $(val).html(); }
        if (idx == 3) { lastName = $(val).html(); }
        if (idx == 4) { email = $(val).html(); }
    });

    try {
        $.ajax({
            url: baseUrl + "/Client/SaveZohoClient",
            type: 'POST',
            async: true,
            data: {
                zohoAccountid: zohoAccountid,
                zohoaccountName: zohoaccountName,
                zohoContactId: zohoContactId,
                firstName: firstName,
                lastName: lastName,
                email: email,
                zohoUserTz: timezone,
                orderId: orderId
            },
            success: function (result) {
                HideLoadingMessage();
                ListViewUtility.CloseLoadingImage();

                if (result.Message == "Successfully Saved.") {
                    $("#clientId").val(result.PrimaryKey);
                    $('.clientName label').text(zohoaccountName);
                    $('.clientName').show();
                    alert("NetTrack Client successfully created.");
                    $('#Zuhu-Modal').modal('hide');

                    $("#aConvertToNetTrack").hide();
                    $("#aConvertToNetTrack").closest("div").css("padding-left", "63%");
                }
                else {
                    alert(result.Message);
                }
            },
            error: function (result) {
                HideLoadingMessage();
                ListViewUtility.CloseLoadingImage();

                alert("error occurred");
            }
        });
    }
    catch (err) {
    }
}

function showZuhuModalLoader() {
    $("#zuhu-modal-wrapper #zuhu-modal-overlay").css('display', 'block');
}

function hideZuhuModalLoader() {
    $("#zuhu-modal-wrapper #zuhu-modal-overlay").css('display', 'none');
}

function LoadOrderStatus() {
    $.ajax({
        url: baseUrl + "/SalesOrder/GetOrderStatus",
        type: 'POST',
        async: false,
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, item) {
                $("#orderStatus").append("<option value='" + item.value + "'>" + item.keyfield + "</option>");
            });
        }
    });
}

var salesTaxList = null;
function LoadSalesTaxList() {
    if (salesTaxList == null) {
        $.ajax({
            url: baseUrl + "/SalesTax/GetSalesTaxList",
            type: 'POST',
            async: false,
            dataType: "json",
            success: function (data) {
                salesTaxList = data;
            }
        });
    }
}

function LoadNettrackStatusList() {
    $.ajax({
        url: baseUrl + "/Common/LoadDDLJson",
        type: 'POST',
        data: { 'spName': 'ug_OrderNettrackClientStatus' },
        success: function (result) {
            $.each(result, function (index, item) {
                $("#ddlNetTrackStatus").append("<option value='" + item.keyfield + "'>" + item.value + "</option>");
            });
        }
    });
}

function InitVar() {
    maxProductCount = 1;

    packageTitleIndex = 1;
    currentProductIndex = 1;
    currentPackageIndex = 2;
}

function ClearDetailForm() {
    /*$("#orderNumber").text("");
    $("#quoteId").val("");
    $("#refQuoteNumber").text("");
    $("#contractTerm").val("");
    $("#validUntil").val("");
    $("#salesPerson").val("");

    $("#ZohoEntityId").val("");
    $("#ZohoEntityType").val("");
    $("#clientId").val("");
    $("#nettrackClientStatusId").val("");

    $("#billCompanyName").val("");
    $("#billCountry").val("US");

    $("#billAddress1").val("");
    $("#billAddress2").val("");
    $("#billCity").val("");
    $("#billState").val("");
    $("#billZip").val("");
    $("#billContact").val("");
    $("#billEmail").val("");
    $("#billPhone").val("");

    $("#sameAsBillTo").removeAttr("checked");

    $("#shipCompanyName").val("");
    $("#shipCountry").val("US");

    $("#shipAddress1").val("");
    $("#shipAddress2").val("");
    $("#shipCity").val("");
    $("#shipState").val("");
    $("#shipZip").val("");
    $("#shipContact").val("");
    $("#shipEmail").val("");
    $("#shipPhone").val("");

    $("#sShippingMethod").text("");

    $("#shippingAndHandling").text("0.00");

    $("#totalSalesTaxFees").text("$0.00");

    $("#orderStatus").val("");

    $("#paymentType").text("");*/

    $("#aConvertToNetTrack").show();
    $('.clientName').hide();
    $('.nettrackStatus').hide();
    $("#totalCharged").text("0.00");
}

function CalculateTotalCharged() {
    $("#totalCharged").text((
        parseFloat($("#totalHardwareFees").text().replace("$", ""))
        + parseFloat($("#totalSalesTaxFees").text().replace("$", ""))
        + parseFloat($("#totalMiscFees").text().replace("$", ""))
        + parseFloat($("#shippingAndHandling").text())
    ).toFixed(2));
}

function ValidateShippingAddress() {
    var shipToCountry = $("#shipCountry").val();

    if (shipToCountry == "US") {
        ListViewUtility.ShowLoadingImage();

        $.ajax({
            url: baseUrl + "/SalesOrder/ValidateAddress",
            type: 'POST',
            //async: false,
            data: JSON.stringify({
                City: $("#shipCity").val().trim(),
                StateProvinceCode: $("#shipState").val(),
                PostalCode: $("#shipZip").val().trim()
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (result) {
                ListViewUtility.CloseLoadingImage();

                if (result.Response.ResponseStatusCode == "1") {
                    if (result.AddressValidationResult.length > 0) {
                        var html = '';
                        $.each(result.AddressValidationResult, function (index, item) {
                            html += '<div>';
                            html += '<label><input ' + (index == 0 ? 'checked' : '') + ' type="radio" name="upsva" style="margin-right: 10px;" class="ups-va" data-city="' + item.Address.City + '" data-state="' + item.Address.StateProvinceCode + '" data-zip="' + item.Address.PostalCode + '">' + item.Address.City + ', ' + item.Address.StateProvinceCode + ', ' + item.Address.PostalCode + '</label>';
                            html += '</div>';
                        });

                        $('#divSuggestedAddress').html(html);
                        $('#suggestedAddressModal').modal('show');
                    }
                } else {
                    alert('No similar address found!');//result.Response.ResponseStatusDescription
                }
            }
        });
    }
}

function SetNewShippingAddress() {
    if ($('[name=upsva]:checked').length == 0) {
        alert('Please select address first');
    } else {
        $("#shipCity").val($('[name=upsva]:checked').data('city'));
        $("#shipState").val($('[name=upsva]:checked').data('state'));
        $("#shipZip").val($('[name=upsva]:checked').data('zip'));

        $('#suggestedAddressModal').modal('hide');
    }
}