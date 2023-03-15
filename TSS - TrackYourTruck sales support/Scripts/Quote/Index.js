var salesPersonList = [];
var maxProductCount = 1;
var dataLoadedForEdit = true;
var disableQuantitySync = false;
var valueChanged = false;

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
    LoadSalesTaxList();
    LoadStateDDL('US', $('#billState'));
    LoadStateDDL('US', $('#shipState'));

    LoadSalesPerson();
    loadQuoteTemplateDDL();

    $("#zohoLinkType").change(function () {
        if ($(this).val() == "Lead") {
            $("#Search-Zuhu-pattern").attr("placeholder", "Company Name");
        } else {
            $("#Search-Zuhu-pattern").attr("placeholder", "Account Name");
        }

        $("#dvAccountList-wrapper").css('display', 'none');
        $("#dvContactList-wrapper").css('display', 'none');
    });

    $("#aShowZuhuModal").click(function (e) {
        e.preventDefault();

        $("#Zuhu-Modal").modal('show');
        $("#btnZohoSearchNext200").attr('disabled', 'disabled');
    });

    $("#Search-Zuhu-pattern").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();

            zohoCount = 0;
            ZohoSearch();
        }
    });

    $("#Search-Zuhu-btn").click(function (e) {
        zohoCount = 0;
        ZohoSearch();
    });

    $("#btnZohoSearchNext200").click(function () {
        ZohoSearch(zohoCount + 1, zohoCount + 200);
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

            CalculateShippingAndHandling();
        } else {
            $("#shipCompanyName").val('');
            $("#shipAddress1").val('');
            $("#shipAddress2").val('');
            $("#shipCity").val('');
            $("#shipState").val('');
            $("#shipZip").val('');
            $("#shipContact").val('');
            $("#shipEmail").val('');
            $("#shipPhone").val('');
        }
    });

    //InitProductContainer();

    if ($("#quoteId").val() != "") {
        dataLoadedForEdit = false;
        Edit($("#quoteId").val());
    }
    else {
        dataLoadedForEdit = true;
        $("#quoteNumber").text("");
    }

    $("[name=SH]").change(function () {
        CalculateShippingAndHandling();

        valueChanged = true;
    });

    $("#shipAddress1").change(function () {
        if (HasPOBox($(this).val())) {
            alert("Cannot ship to P.O. Box via UPS");
        }
        else {
            CalculateShippingAndHandling();
        }
    });

    $("#shipAddress2").change(function () {
        if (HasPOBox($(this).val())) {
            alert("Cannot ship to P.O. Box via UPS");
        }
    });

    $("#shipCity").change(function () {
        CalculateShippingAndHandling();
    });

    $("#shipState").change(function () {
        CalculateShippingAndHandling();
        //CalculateTotalCharged();
    });

    $("#shipZip").change(function () {
        CalculateShippingAndHandling();
    });

    $("#billCountry").change(function () {
        LoadStateDDL($(this).val(), $('#billState'));
    });

    $("#shipCountry").change(function () {
        LoadStateDDL($(this).val(), $('#shipState'));

        ChangeShippingandHandlingServiceTitle($(this).val());
        CalculateShippingAndHandling();
    });

    $("#Search-NetTrackClient-Text").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            NetTrackClientSearch($(this).val());
        }
    });

    $("#Search-NetTrackClient-btn").click(function (e) {
        NetTrackClientSearch($("#Search-NetTrackClient-Text").val());
    });

    $("#adisableQuantityRestriction").click(function (e) {
        e.preventDefault();

        if (!disableQuantitySync) {
            DisableQuantitySync();
        } else {
            EnableQuantitySync();
        }
    });

    $("input,select").change(function () {
        valueChanged = true;
    });
});

function DisableQuantitySync() {
    disableQuantitySync = true;

    $(".product-quantity").removeAttr("readonly");
    //$(".update-item").hide();

    $("#adisableQuantityRestriction").text("Enable Qty Sync");

    $.each($(".remove-item"), function (index, item) {
        item = $(item);

        if (item.parent().find(".update-item").length == 0) {
            item.parent().prepend("<a href='' class='update-item update-item-disable-sync'>Update</a>&nbsp;|&nbsp;");
        }
    });

    $(".update-item-disable-sync").show();

    $(".update-item").unbind();
    $(".update-item").click(function (e) {
        e.preventDefault();

        var dataMaxProductQty, maxProductCount;

        var aUpdateItem = $(this);
        currentProductDiv = aUpdateItem.closest(".product");
        var rowItem = aUpdateItem.closest('tr');
        var productQuantity = rowItem.find('.product-quantity');

        if (!$.isNumeric(productQuantity.val()) || (parseInt(productQuantity.val()) + "") != productQuantity.val()) {
            alert("Invalid Quantity");
            return;
        }

        if (parseFloat(productQuantity.val()) < 1.0) {
            alert("Quantity can't be less than 1.");
            return;
        }

        var productPrice = 0;
        if (rowItem.find('.product-price').html()[0] == "-") {
            productPrice = rowItem.find('.product-price').html().substr(2);
            rowItem.find('.product-extprice').html('-$' + (productQuantity.val() * productPrice).toFixed(2));
        } else {
            productPrice = rowItem.find('.product-price').html().substr(1);
            rowItem.find('.product-extprice').html('$' + (productQuantity.val() * productPrice).toFixed(2));
        }

        if (productQuantity.closest("table").attr("product-category") == "Hardware" && rowItem.find('.product-price').html()[0] != "-") {
            dataMaxProductQty = rowItem.closest("[class=Package]").find("h2");
            maxProductCount = dataMaxProductQty.attr("data-maxproductqty");
            dataMaxProductQty.attr("data-maxproductqty", productQuantity.val());
        }

        /*if (productQuantity.closest("table").attr("product-category") == "Hardware") {
        AddAutoDiscount(currentProductDiv, productQuantity.val(), productPrice);
        }*/

        MakeSameQuantity(rowItem.closest("[class=Package]"));
        SumProductCost(currentProductDiv);
        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
            SumProductCost(currentProductDiv.parent().find("[product-category='Misc Fee']").closest(".product"));
        }
        ProductSummary(currentProductDiv.parent());
        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            CalculateShippingAndHandling();
        }
        MakeSummary();
    });
}

function EnableQuantitySync() {
    disableQuantitySync = false;

    if ($("[class=Package]").length > 0) {
        MakeSameQuantity($("[class=Package]").eq(0));

        //$(".update-item").show();
        $.each($(".update-item-disable-sync"), function (index, item) {
            item = $(item);

            //if (item.parent().find(".update-item").length == 0) {
            item.closest(".productDetail").find(".product-quantity").attr("readonly", "readonly");
            //}
        });
    }

    $(".update-item-disable-sync").hide();

    $("#adisableQuantityRestriction").text("Disable Qty Sync");
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
    var clientEntity = $("input:radio[name=rdbClientEntity]:checked");
    var clientId = clientEntity.attr("ClientId");
    var clientName = clientEntity.attr("ClientName");

    $("#nettrackClientId").val(clientId);
    $("#nettrackClient").val(clientName);

    $('#NetTrackClient-Modal').modal('hide');
}

var zohoCount = 0;
function ZohoSearch(from, to) {
    var fromIndex = from || 0;
    var toIndex = to || 200;

    zohoCount = toIndex;

    ListViewUtility.ShowLoadingImage();
    //showZuhuModalLoader();

    $('#btnSaveZoho').attr('disabled', true);
    $('#dvAccountList').html('');
    $('#dvContactList').html('');

    var requestUrl = baseUrl;
    if ($("#zohoLinkType").val() == "Lead") {
        requestUrl += "/Quote/SearchLeadInZoho";
    } else if ($("#zohoLinkType").val() == "Contact") {
        requestUrl += "/Quote/SearchContactInZoho";
    } else if ($("#zohoLinkType").val() == "Account") {
        requestUrl += "/Quote/SearchAccountInZoho";
    }

    $.ajax({
        url: requestUrl,
        type: 'POST',
        async: true,
        data: { searchText: $('#Search-Zuhu-pattern').val(), fromIndex: fromIndex, toIndex: toIndex },
        dataType: "json",
        success: function (result) {
            if ($("#zohoLinkType").val() == "Lead") {
                ProcessLeads(result);

                $("#btnZohoSearchNext200").removeAttr('disabled');
            } else if ($("#zohoLinkType").val() == "Contact") {
                ProcessContacts(result);

                $("#btnZohoSearchNext200").removeAttr('disabled');
            } else if ($("#zohoLinkType").val() == "Account") {
                ProcessAccounts(result);

                $("#btnZohoSearchNext200").removeAttr('disabled');
            } else {
                $("#btnZohoSearchNext200").attr('disabled', 'disabled');
            }

            ListViewUtility.CloseLoadingImage();
        },
        error: function (result) {
            ListViewUtility.CloseLoadingImage();
            //hideZuhuModalLoader();
        }
    });
}

var leadList = [];
function ProcessLeads(responsedata) {
    responsedata = responsedata == '' ? null : eval("(" + responsedata + ")");
    //var jsonResult = eval("(" + data + ")");

    //console.log(jsonResult);

    var leadId = '', companyName = '', SMOWNERID = '', street = '',
        city = '', state = '', zipCode = '', firstName = '',
        lastName = '', email = '', phone = '', country = '';
    leadList = [];

    if (responsedata != null && responsedata.data) {
        $.each(responsedata.data, function (i, row) {
            leadId = row.id;
            companyName = row.Company;
            SMOWNERID = row.Owner.id;
            street = row.Street;
            city = row.City;
            state = row.State;
            zipCode = row.Zip_Code;
            firstName = row.First_Name == null ? '' : row.First_Name;
            lastName = row.Last_Name == null ? '' : row.Last_Name;
            email = row.Email;
            phone = row.Phone;

            country = row.Country || '';
            if (country == 'CANADA') country = 'CA';
            else if (country == 'MEXICO') country = 'MX';
            else country = 'US';

            leadList.push({
                id: leadId, name: companyName, SMOWNERID: SMOWNERID, street: street,
                city: city, state: state, zipCode: zipCode, firstName: firstName,
                lastName: lastName, email: email, phone: phone, country: country
            });
            leadId = '', companyName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '', country = '';

        });
        leadList = _.sortBy(leadList, 'name');

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

var accountList = [];
function ProcessAccounts(responsedata) {
    responsedata = responsedata == '' ? null : eval("(" + responsedata + ")");

    //console.log(data);
    //var jsonResult = eval("(" + data + ")");

    var accountId = '', accountName = '', SMOWNERID = '', street = '',
        city = '', state = '', zipCode = '', phone = '', country = '';
    accountList = [];

    if (responsedata != null && responsedata.data) {
        $.each(responsedata.data, function (i, row) {
            accountId = row.id;
            accountName = row.Account_Name;
            SMOWNERID = row.Owner.id;
            street = row.Billing_Street;
            city = row.Billing_City;
            state = row.Billing_State;
            zipCode = row.Billing_Code;
            phone = row.Phone;

            country = (row.Billing_Country || '').toUpperCase();
            if (country == 'CANADA') country = 'CA';
            else if (country == 'MEXICO') country = 'MX';
            else country = 'US';

            accountList.push({
                id: accountId, name: accountName, SMOWNERID: SMOWNERID, street: street,
                city: city, state: state, zipCode: zipCode, phone: phone, country: country
            });

            accountId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', phone = '', country = '';
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
					+ '<td ><input type="radio" name="rdbZohoEntity" onclick="LoadContacts(\'' + obj.id + '\');" ZohoEntityType="account" ZohoEntityId="' + obj.id + '"/></td>'
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

function LoadContacts(accountId) {
    ListViewUtility.ShowLoadingImage();
    showZuhuModalLoader();

    $('#btnSaveZoho').attr('disabled', true);
    $('#dvContactList').html('');

    var requestUrl = baseUrl + "/Quote/SearchContactInZoho";

    $.ajax({
        url: requestUrl,
        type: 'POST',
        async: true,
        data: { searchText: accountId },
        dataType: "json",
        success: function (result) {
            ProcessContacts(result);

            hideZuhuModalLoader();
        },
        error: function (result) {
            ListViewUtility.CloseLoadingImage();
            hideZuhuModalLoader();
        }
    });
}

var contactList = [];
function ProcessContacts(responsedata) {
    responsedata = responsedata == '' ? null : eval("(" + responsedata + ")");

    //var jsonResult = eval("(" + data + ")")

    var contactId = '', accountName = '', SMOWNERID = '', street = '',
        city = '', state = '', zipCode = '', firstName = '',
        lastName = '', email = '', phone = '', country = '';
    contactList = [];

    if (responsedata != null && responsedata.data) {
        $.each(responsedata.data, function (i, row) {
            contactId = row.id;
            accountName = row.Account_Name.name;
            SMOWNERID = row.Owner.id;
            street = row.Mailing_Street;
            city = row.Mailing_City;
            state = row.Mailing_State;
            zipCode = row.Mailing_Zip;
            firstName = row.First_Name == null ? '' : row.First_Name;
            lastName = row.Last_Name == null ? '' : row.Last_Name;
            email = row.Email;
            phone = row.Phone;

            country = (row.Mailing_Country || '').toUpperCase();
            if (country == 'CANADA') country = 'CA';
            else if (country == 'MEXICO') country = 'MX';
            else country = 'US';

            contactList.push({
                id: contactId, name: accountName, SMOWNERID: SMOWNERID, street: street,
                city: city, state: state, zipCode: zipCode, firstName: firstName,
                lastName: lastName, email: email, phone: phone, country: country
            });

            contactId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '', country = '';
        });
        contactList = _.sortBy(contactList, 'firstName');

        var htmlStr = '<table cellspacing="0" role="grid"  style="table-layout: auto; text-align: center;">'
				+ '<colgroup>'
					+ '<col style="width:30px">'
					+ '<col style="width:190px">'
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
            htmlStr += '<tr ' + (idx % 2 == 1 ? 'class="k-alt"' : '') + ' >'
					+ '<td ><input type="radio" name="rdbZohoEntity" onclick="contactSelected();"  ZohoEntityType="contact" ZohoEntityId="' + obj.id + '"/></td>'
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

    salesPersonList = contactList;
}

function contactSelected() {
    $("#btnSaveZoho").removeAttr('disabled');
}

function SaveZoho() {
    var zohoEntity = $("input:radio[name=rdbZohoEntity]:checked");
    var zohoEntityId = zohoEntity.attr("ZohoEntityId");
    $("#ZohoEntityType").val(zohoEntity.attr("ZohoEntityType"));
    $("#ZohoEntityId").val(zohoEntityId);

    var salesPersonId = $("#salesPerson").val();

    var salesPersonItem = null;

    $.each(salesPersonList, function (index, item) {
        if (item.id == zohoEntityId) {
            salesPersonItem = item;
        }
    });

    if (salesPersonItem != null) {
        $("#billCompanyName").val(salesPersonItem.name);
        $("#billAddress1").val(salesPersonItem.street);
        $("#billCity").val(salesPersonItem.city);

        var billCountry = $("#billCountry");
        var billState = $("#billState");

        if (salesPersonItem.country != '') {
            billCountry.val(salesPersonItem.country);
            LoadStateDDL(billCountry.val(), billState);
            billState.val(salesPersonItem.state == '' ? 'MEX' : salesPersonItem.state);
        }
        else {
            billState.val(salesPersonItem.state);
            if (billState.val() != salesPersonItem.state) {

                billCountry.val("CA");
                LoadStateDDL(billCountry.val(), billState);

                billState.val(salesPersonItem.state);
                if (billState.val() != salesPersonItem.state) {
                    billCountry.val("MX");
                    LoadStateDDL(billCountry.val(), billState);

                    billState.val(salesPersonItem.state);
                    if (billState.val() != salesPersonItem.state) {

                        billCountry.val("US");
                        LoadStateDDL(billCountry.val(), billState);
                    }
                }
            }
        }

        $("#billZip").val(salesPersonItem.zipCode);
        $("#billContact").val(salesPersonItem.firstName + " " + salesPersonItem.lastName);
        $("#billEmail").val(salesPersonItem.email);
        $("#billPhone").val(salesPersonItem.phone);
    }

    $('#Zuhu-Modal').modal('hide');
}

function showZuhuModalLoader() {
    $("#zuhu-modal-wrapper #zuhu-modal-overlay").css('display', 'block');
}

function hideZuhuModalLoader() {
    $("#zuhu-modal-wrapper #zuhu-modal-overlay").css('display', 'none');
}


function LoadProductGrid(filterValue, context, sender) {
    var tmpMaxProdCount = 1;
    var productCategory = sender.closest(".product").find(".productTable").attr("product-category");

    if (!disableQuantitySync) {
        $.each(context.find(".product-quantity"), function (index, item) {
            if ($(item).closest("table").attr("product-category") != "Optional") {
                if ($(item).val() > tmpMaxProdCount) {
                    tmpMaxProdCount = $(item).val();
                }
            }
        });
    }

    if (productCategory == "Optional") {
        tmpMaxProdCount = 1;
        productCategory = "Service";
    }

    var columns = '[';
    //columns += '{"template": "\u003cinput id=\u0027delete${ ProductId }\u0027 value=\u0027${ ProductId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
    columns += '{"width":"150px", "field":"SKU","title":"SKU"},';
    columns += '{"field":"ProductName","title":"Product Name"},';
    //columns += '{"field":"ProductDescription","title":"Product Description"},';
    columns += '{"width":"100px", "field":"Price","title":"Price",format:"{0:c2}"},';
    //columns += '{"field":"ProductTypeName","title":"Type"},';
    columns += '{"width":"70px", "title":"Quantity", "template":"\u003cinput type=text value =' + tmpMaxProdCount + ' class=quantity style=\u0027width: 40px\u0027 \u003e"},';
    columns += '{"width":"60px", "title":"","template":"\u003ca class=\u0027add-product btn btn-primary\u0027 weight=\u0027${ Weight }\u0027 product-id=\u0027${ ProductId }\u0027 \u003eAdd\u003c/a\u003e"}';
    columns += ']';

    $("#divKendoGridProductList").kendoGrid().empty();

    var gridDataSource;

    if (filterValue == "discount") {
        gridDataSource = new kendo.data.DataSource({
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
            filter: [
                { field: "ProductTypeName", operator: "contains", value: filterValue },
                { field: "DiscountProductTypeName", operator: "eq", value: productCategory }
            ]
        });
    }
    else {
        gridDataSource = new kendo.data.DataSource({
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
    }

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
        var weight = aAddProduct.attr("weight");
        var quantity = aAddProduct.parent().parent().find(".quantity").val();
        var newRecord = null;


        if ($("[name=ProductOrOptionalServices]:checked").val() == "product") {
            var divKendoGridProductListData = $("#divKendoGridProductList").data("kendoGrid").dataSource.data();
            for (i = 0; i < divKendoGridProductListData.length; i++) {
                var item = divKendoGridProductListData[i];
                if (item.ProductId == productId) {
                    newRecord = { Weight: weight, QuoteProductId: 0, ProductId: item.ProductId, Quantity: quantity, SKU: item.SKU, ProductDescription: item.ProductDescription, Price: item.Price, ExtendedPrice: (item.Price * quantity) };
                    break;
                }
            }
        }
        else {
            var divKendoGridProductListData = $("#divKendoGridProductList").data("kendoGrid").dataSource.data();
            for (i = 0; i < divKendoGridProductListData.length; i++) {
                var item = divKendoGridProductListData[i];
                if (item.ProductId == productId) {
                    newRecord = { Weight: weight, QuoteProductId: 0, ProductId: item.ProductId, Quantity: quantity, SKU: item.SKU, ProductDescription: item.ProductDescription, Price: item.Price, ExtendedPrice: (item.Price * quantity) };
                    break;
                }
            }
        }

        if (newRecord != null) {
            AddNewProduct(currentProductDiv, newRecord);
            valueChanged = true;
        }
    });
}

function AddNewProduct(currentProductDiv, newRecord) {
    var productQuantityValue = newRecord.Quantity;
    var newRow = "<tr class='productDetail' deleted='0'><td style='padding-top: 5px;text-align: right; width: 118px'><a class='up-item pull-left' style='text-decoration: none' href=''>↑</a><a class='down-item pull-left' style='text-decoration: none' href=''>↓</a>&nbsp;<a href='' class='remove-item'>Remove</a><input class='quote-product-id' type='hidden' value='" + newRecord.QuoteProductId + "'><input class='weight' type='hidden' value='" + newRecord.Weight + "'></td>";

    if ((currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType != "discount")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Optional")) {
        newRow = newRow.replace("<a href='' class='remove-item'>Remove</a>", "<a href='' class='update-item'>Update</a>&nbsp;|&nbsp;<a href='' class='remove-item'>Remove</a>");
    } else {
        if (disableQuantitySync) {
            newRow = newRow.replace("<a href='' class='remove-item'>Remove</a>", "<a href='' class='update-item update-item-disable-sync'>Update</a>&nbsp;|&nbsp;<a href='' class='remove-item'>Remove</a>");
        }
    }

    if (((currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType == "discount")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Monthly")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Misc Fee"))) {
        newRow += "<td style='text-align: center;'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><input "
               + (disableQuantitySync ? "" : "readonly='readonly'")
               + " class='product-quantity' type='text' style='width: 40px; text-align: center' value='" + newRecord.Quantity + "'></td>";
    }
    else {
        newRow += "<td style='text-align: center;'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><input class='product-quantity' type='text' style='width: 40px; text-align: center' value='" + newRecord.Quantity + "'></td>";
    }
    newRow += "<td>" + newRecord.SKU + "</td>";
    newRow += "<td>" + newRecord.ProductDescription + "</td>";
    if (productType == "discount") {
        newRow += "<td style='text-align: right'><span style='padding-right: 20px' class='product-price'>-$" + newRecord.Price.toFixed(2).replace('-', '') + "</span></td>";
        newRow += "<td style='text-align: right'><span style='padding-right: 40px' class='product-extprice'>-$" + (newRecord.Quantity * newRecord.Price).toFixed(2).replace('-', '') + "</span></td>";
    } else {
        newRow += "<td style='text-align: right'><span style='padding-right: 20px' class='product-price'>$" + newRecord.Price.toFixed(2) + "</span></td>";
        newRow += "<td style='text-align: right'><span style='padding-right: 40px' class='product-extprice'>$" + (newRecord.Quantity * newRecord.Price).toFixed(2) + "</span></td>";
    }
    newRow += "</tr>";

    var dataMaxProductQty = currentProductDiv.closest("[class=Package]").find("h2");

    var addedProduct = currentProductDiv.find(".productTable").find("span[product-id=" + newRecord.ProductId + "]");
    if (addedProduct.length > 0) {
        var rowItem = addedProduct.closest('tr');
        var productQuantity = rowItem.find('.product-quantity');
        productQuantityValue = parseInt(productQuantity.val()) + parseInt(newRecord.Quantity);

        if (!rowItem.is(":visible")) {
            rowItem.attr("deleted", "0");
            rowItem.show();
            productQuantityValue = newRecord.Quantity;
        }

        rowItem.find('.weight').val(newRecord.Weight);

        if (rowItem.closest("table").attr("product-category") == "Hardware" && productType != "discount") {
            dataMaxProductQty.attr("data-maxproductqty", productQuantityValue);
        }

        if (productType == "discount") {
            var productPrice = newRecord.Price.toFixed(2);
            rowItem.find('.product-price').html('-$' + productPrice.replace('-', ''));

            productQuantity.val(productQuantityValue);
            rowItem.find('.product-extprice').html('-$' + (productQuantityValue * productPrice).toFixed(2).replace('-', ''));
        } else {
            var productPrice = newRecord.Price.toFixed(2);
            rowItem.find('.product-price').html('$' + productPrice);

            productQuantity.val(productQuantityValue);
            rowItem.find('.product-extprice').html('$' + (productQuantityValue * productPrice).toFixed(2));
        }
    }
    else {
        if (currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType != "discount") {
            dataMaxProductQty.attr("data-maxproductqty", newRecord.Quantity);
        }

        if (productType == "hardware") {
            if (currentProductDiv.find(".productTable .productDetail").length > 0) {
                currentProductDiv.find(".productTable .productDetail").first().before(newRow);
            } else {
                currentProductDiv.find(".productTable").append(newRow);
            }
        }
        else {
            currentProductDiv.find(".productTable").append(newRow);
        }
    }

    $(".up-item").unbind();
    $(".up-item").click(function (e) {
        e.preventDefault();

        var row = $(this).parents("tr:first");
        var prev = row.prev();

        while (prev.attr('deleted') == "1") {
            prev = prev.prev();
        }

        if (prev.hasClass('productDetail')) {
            row.insertBefore(prev);

            valueChanged = true;
        }
    });

    $(".down-item").unbind();
    $(".down-item").click(function (e) {
        e.preventDefault();

        var row = $(this).parents("tr:first");
        var next = row.next();

        while (next.attr('deleted') == "1") {
            next = next.next();
        }

        row.insertAfter(next);

        valueChanged = true;
    });

    $(".remove-item").unbind();
    $(".remove-item").click(function (e) {
        e.preventDefault();
        var deletedRow = $(this).closest('tr');
        deletedRow.hide();
        deletedRow.attr("deleted", "1");
        deletedRow.find(".product-extprice").text("$0.00");

        SumProductCost(deletedRow.closest(".product"));
        ProductSummary(deletedRow.closest(".product").parent());
        if (deletedRow.closest("table").attr("product-category") == "Hardware") {
            CalculateShippingAndHandling();
        }
        MakeSummary();

        valueChanged = true;
    });

    $(".update-item").unbind();
    $(".update-item").click(function (e) {
        e.preventDefault();
        var aUpdateItem = $(this);
        currentProductDiv = aUpdateItem.closest(".product");
        var rowItem = aUpdateItem.closest('tr');
        var productQuantity = rowItem.find('.product-quantity');

        if (!$.isNumeric(productQuantity.val()) || (parseInt(productQuantity.val()) + "") != productQuantity.val()) {
            alert("Invalid Quantity");
            return;
        }

        if (parseFloat(productQuantity.val()) < 1.0) {
            alert("Quantity can't be less than 1.");
            return;
        }

        var productPrice = 0;
        if (rowItem.find('.product-price').html()[0] == "-") {
            productPrice = rowItem.find('.product-price').html().substr(2);
            rowItem.find('.product-extprice').html('-$' + (productQuantity.val() * productPrice).toFixed(2));
        } else {
            productPrice = rowItem.find('.product-price').html().substr(1);
            rowItem.find('.product-extprice').html('$' + (productQuantity.val() * productPrice).toFixed(2));
        }

        if (productQuantity.closest("table").attr("product-category") == "Hardware" && rowItem.find('.product-price').html()[0] != "-") {
            dataMaxProductQty = rowItem.closest("[class=Package]").find("h2");
            maxProductCount = dataMaxProductQty.attr("data-maxproductqty");
            dataMaxProductQty.attr("data-maxproductqty", productQuantity.val());
        }

        /*if (productQuantity.closest("table").attr("product-category") == "Hardware") {
        AddAutoDiscount(currentProductDiv, productQuantity.val(), productPrice);
        }*/

        MakeSameQuantity(rowItem.closest("[class=Package]"));
        SumProductCost(currentProductDiv);
        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
            SumProductCost(currentProductDiv.parent().find("[product-category='Misc Fee']").closest(".product"));
        }
        ProductSummary(currentProductDiv.parent());
        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            CalculateShippingAndHandling();
        }
        MakeSummary();

        valueChanged = true;
    });

    /*if (currentProductDiv.find("table").attr("product-category") == "Hardware" && productType != "discount") {
    AddAutoDiscount(currentProductDiv, productQuantityValue, newRecord.Price);
    }*/

    MakeSameQuantity(currentProductDiv.closest("[class=Package]"));
    SumProductCost(currentProductDiv);
    if (currentProductDiv.find("table").attr("product-category") == "Hardware") {
        SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
    }
    ProductSummary(currentProductDiv.parent());
    if (currentProductDiv.find("table").attr("product-category") == "Hardware" && productType != "discount") {
        CalculateShippingAndHandling();
    }
    MakeSummary();
}

function MakeSameQuantity(currentProductDiv) {
    if (!disableQuantitySync) {
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
}

var packageTitleIndex = 1;
var currentProductIndex = 1;
var currentPackageIndex = 2;
var activeProductIndex;
function InitProductContainer() {
    $("#productContent").empty();

    AddMoreProduct1();

    $("#aAddMoreProduct").unbind();
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

        RemovePackage(this);
    });

    BindProductEvent();

    currentProductIndex++;
    packageTitleIndex++;
}

function RemovePackage(context) {
    context = $(context);
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
    CalculateShippingAndHandling();
}

var productType = "";
function BindProductEvent() {
    $(".product-action").unbind();
    $(".product-action").click(function (e) {
        e.preventDefault();

        var productAction = $(this);
        productType = productAction.attr("product-type");

        LoadProductGrid(productType, productAction.closest("[class=Package]"), productAction);
        activeProductIndex = productAction.closest(".product").attr("product-index").substr(1);

        $('#Product-Modal').modal('show');
    });
}

function SumProductCost(currentProductDiv) {
    var total = 0;

    $.each(currentProductDiv.find(".product-extprice"), function (index, item) {
        if ($(this).closest('.productDetail').attr("deleted") != "1") {
            if ($(this).html()[0] == "-") {
                total += (-parseFloat($(this).html().substr(2)));
            }
            else {
                total += parseFloat($(this).html().substr(1));
            }
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
        if ($(this).closest('.productDetail').attr("deleted") != "1") {
            if ($(this).html()[0] == "-") {
                total += (-parseFloat($(this).html().substr(2)));
            }
            else {
                total += parseFloat($(this).html().substr(1));
            }
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

    total += parseFloat($("#shippingAndHandling").text());

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

function SaveAs() {
    $("#quoteId").val("0");
    $('.quote-product-id').val("0")
    $("#quoteDate").text(GetFormattedDate(new Date()));

    $('#orderTypeModal').modal('show');
    $('#btnConvertToOrder').attr('onclick', 'SetOrderTypeOnSaveAs();');
}

function SetOrderTypeOnSaveAs() {
    $('#ddlOrderType').val($('[name=ChangeOrderType]:checked').val());
    $('#orderTypeModal').modal('hide');

    Save();
}

function Save() {
    /*if ($("#shipCity").val().trim() == "") {
    alert("Please enter shipping City.");
    return;
    }

    if ($("#shipState").val().trim() == "") {
    alert("Please enter shipping State.");
    return;
    }

    if ($("#shipZip").val().trim() == "") {
    alert("Please enter shipping Zip.");
    return;
    }*/

    if ($("[name=SH]:Checked").val() == undefined) {
        alert("Please select Shipping and Handling service.");
        return;
    }

    var postData = {};
    postData.QuoteModel = {};
    postData.QuoteModel.TssOrderTypeId = $("#ddlOrderType").val();
    postData.QuoteModel.QuoteId = $("#quoteId").val() == "" ? "0" : $("#quoteId").val();
    postData.QuoteModel.QuoteNumber = "";
    postData.QuoteModel.QuoteDate = $("#quoteDate").text();
    postData.QuoteModel.ContractTerm = $("#contractTerm").val();
    postData.QuoteModel.ValidUntil = $("#validUntil").val();
    postData.QuoteModel.SalesPersonId = $("#salesPerson").val();
    postData.QuoteModel.ZohoEntityId = $("#ZohoEntityId").val();
    postData.QuoteModel.ZohoEntityType = $("#ZohoEntityType").val();
    postData.QuoteModel.ClientID = $("#nettrackClientId").val();

    postData.QuoteModel.BillToCompanyName = $("#billCompanyName").val();
    postData.QuoteModel.BillToAddress1 = $("#billAddress1").val();
    postData.QuoteModel.BillToAddress2 = $("#billAddress2").val();
    postData.QuoteModel.BillToCity = $("#billCity").val();
    postData.QuoteModel.BillToState = $("#billState").val();
    postData.QuoteModel.BillToZip = $("#billZip").val();
    postData.QuoteModel.BillToCountry = $("#billCountry").val();
    postData.QuoteModel.BillToBillingContact = $("#billContact").val();
    postData.QuoteModel.BillToBillingEmail = $("#billEmail").val();
    postData.QuoteModel.BillToPhone = $("#billPhone").val();

    postData.QuoteModel.IsShipSameAsBill = $("#sameAsBillTo").is(":checked");

    postData.QuoteModel.ShipToCompanyName = $("#shipCompanyName").val();
    postData.QuoteModel.ShipToAddress1 = $("#shipAddress1").val();
    postData.QuoteModel.ShipToAddress2 = $("#shipAddress2").val();
    postData.QuoteModel.ShipToCity = $("#shipCity").val();
    postData.QuoteModel.ShipToState = $("#shipState").val();
    postData.QuoteModel.ShipToZip = $("#shipZip").val();
    postData.QuoteModel.ShipToCountry = $("#shipCountry").val();
    postData.QuoteModel.ShipToBillingContact = $("#shipContact").val();
    postData.QuoteModel.ShipToBillingEmail = $("#shipEmail").val();
    postData.QuoteModel.ShipToPhone = $("#shipPhone").val();

    postData.QuoteModel.ShippingAndHandling = $("#shippingAndHandling").text();
    postData.QuoteModel.ShippingAndHandlingType = $("[name=SH]:Checked").val();
    postData.QuoteModel.SalesTax = $("#totalSalesTaxFees").text().replace("$", "");

    postData.QuoteModel.IsQuantitySyncDisabled = disableQuantitySync;

    postData.QuoteModel.Note = $("#txtNote").val();

    postData.QuoteModel.IsDemo = $("#chkIsDemo").is(":checked");

    postData.QuoteProductModelList = [];

    //var errorMessage = "";

    var tempProduct = null;
    $('.Package').each(function () {
        var Package = $(this);

        var deleteAll = Package.find("h2").attr("data-deleted") == "0" ? false : true;
        var hasHardware = false;

        Package.find('.productDetail').each(function () {
            var product = $(this);
            tempProduct = {};
            tempProduct.QuoteProductId = product.find('.quote-product-id').val();
            tempProduct.ProductId = product.find('.product-id').attr('product-id');
            tempProduct.Quantity = product.find('.product-quantity').val();
            tempProduct.Price = product.find('.product-price').text().replace('$', '');
            tempProduct.PackageId = Package.find('.PackageId').data('packageid');
            tempProduct.ProductCategory = product.closest("table").attr("Product-Category");
            tempProduct.Deleted = (deleteAll ? true : (product.attr("deleted") == "1" ? true : false));
            postData.QuoteProductModelList.push(tempProduct);

            /*if (product.closest("table").attr("product-category") == "Hardware"
                && tempProduct.Price > -1
                && !tempProduct.Deleted) {
                hasHardware = true;
            }*/
        })

        /*if (!hasHardware && Package.is(":visible")) {
            errorMessage += ", Product " + Package.find("h2").data("packageid");
        }*/
    })

    /*if (errorMessage != "") {
        alert(errorMessage.substr(2) + " must have at least one hardware product.\n");
    }
    else*/ {
        try {
            ListViewUtility.ShowLoadingImage();

            $.ajax({
                url: baseUrl + "/Quote/Save",
                type: 'POST',
                async: true,
                data: JSON.stringify(postData),
                dataType: 'json',
                contentType: "application/json",
                success: function (result) {
                    if (result.Message == "Successfully Saved.") {
                        alert("Quote Successfully Saved.\nQuote Number: " + result.PrimaryKey);
                        //location.href = baseUrl + "/Quote/List";

                        $("#detailPage").hide();
                        LoadQuoteList();
                    }
                    else if (result.Message.indexOf("Successfully Saved.") > -1) {
                        alert(result.Message);

                        $("#detailPage").hide();
                        LoadQuoteList();
                    }
                    else {
                        alert("Quote Save Failed.");
                        $('#lblWarning').text(result.Message);
                    }

                    ListViewUtility.CloseLoadingImage();
                },
                error: function (result) {
                    $('#lblWarning').text("Error occurred");

                    ListViewUtility.CloseLoadingImage();
                }
            });

        }
        catch (err) {
        }
    }
}

function Edit(quoteId) {
    try {
        $.ajax({
            url: baseUrl + "/Quote/GetQuote",
            type: 'POST',
            async: false,
            data: { id: quoteId },
            dataType: 'json',
            success: function (result) {
                if (result.Quote != null) {

                    $("#quoteNumber").text(result.Quote.QuoteId);
                    $("#ddlOrderType").val(result.Quote.TssOrderTypeId);
                    $("#quoteDate").text(result.Quote.QuoteDateFormated);
                    $("#contractTerm").val(result.Quote.ContractTerm);
                    $("#validUntil").val(result.Quote.ValidUntil);
                    $("#salesPerson").val(result.Quote.SalesPersonId);
                    SetTYTSalesPerson();
                    $("#ZohoEntityId").val(result.Quote.ZohoEntityId);
                    $("#ZohoEntityType").val(result.Quote.ZohoEntityType);
                    $("#nettrackClientId").val(result.Quote.ClientID);
                    $("#nettrackClient").val(result.Quote.ClientName);

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

                    $("#txtNote").val(result.Quote.Note);

                    if (result.Quote.IsApproved == "N") {
                        $("#aApprove").show();
                    } else {
                        $("#aApprove").hide();
                    }

                    if (result.Quote.IsDemo) {
                        $("#chkIsDemo").attr("checked", "checked");
                    }

                    ChangeShippingandHandlingServiceTitle(result.Quote.ShipToCountry);

                    $("#shippingAndHandling").text(result.Quote.ShippingAndHandling.toFixed(2));
                    $("[name=SH][value=" + result.Quote.ShippingAndHandlingType + "]").attr("checked", true);

                    $("#totalSalesTaxFees").text("$" + result.Quote.SalesTax.toFixed(2));

                    disableQuantitySync = result.Quote.IsQuantitySyncDisabled;
                    if (disableQuantitySync) {
                        DisableQuantitySync();
                    }

                    var currentPackageId = 1;
                    $.each(result.QuoteProductList, function (index, quoteProduct) {
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

                    if (result.Quote.Purchased) {
                        MakeReadOnly();
                    }

                    dataLoadedForEdit = true;
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

    var hardwareCount = 0;
    var hardwareWeight = 0.0;
    $.each($("[product-category=Hardware]").parent().find(".weight"), function (index, item) {
        var _this = $(this);
        var rowItem = _this.closest("tr");
        if (rowItem.attr("deleted") == "0") {
            hardwareWeight += (parseFloat(_this.val()) * parseFloat(rowItem.find(".product-quantity").val()));
        }
    });
    $.each($("[product-category='Misc Fee']").parent().find(".weight"), function (index, item) {
        var _this = $(this);
        var rowItem = _this.closest("tr");
        if (rowItem.attr("deleted") == "0") {
            hardwareWeight += (parseFloat(_this.val()) * parseFloat(rowItem.find(".product-quantity").val()));
        }
    });
     $("#totalProductWeight").text('' + hardwareWeight.toFixed(2));

    $.each($("[product-category=Hardware]").parent().find(".product-quantity"), function (index, item) {
        if ($(this).closest("tr").is(':visible') && $(this).closest("tr").find(".product-price").text().indexOf("-") == "-1") {
            hardwareCount += parseInt($(this).val());
        }
    });
    if (hardwareCount < 4) {
        $("#totalShippingBoxSize").text('' + DevicesLessThan3);
    } else if (hardwareCount > 3 && hardwareCount < 7) {
        $("#totalShippingBoxSize").text('' + Devices4To6);
    } else {
        $("#totalShippingBoxSize").text('' + DevicesGreaterThan7);
    }


    //$("#totalCharged").text((hardwareSummary + miscSummary + parseFloat($("#shippingAndHandling").text())).toFixed(2));
    CalculateTotalCharged();

    $.each($(".product-price"), function (index, item) {
        var item = $(item);

        if (item.text().indexOf('-') > -1) {
            var tr = item.closest('tr');
            var td = tr.find("td");
            for (var tdi = 2; tdi < 4; tdi++) {
                td.eq(tdi).css("color", "red");
            }
            tr.find(".product-price").css("color", "red");
            tr.find(".product-extprice").css("color", "red");
        }
    });
}

function CalculateShippingAndHandling() {
    if ($("#shipCity").val().trim() == "" || $("#shipState").val() == null || $("#shipZip").val().trim() == "") {
        return;
    }

    if ($("[name=SH]:checked").val() == "Free") {
        CalculateSalesTax();
        $("#shippingAndHandling").text("0.00");
        $("#totalShippingAndHandlingFees").text("$0.00");
        CalculateTotalCharged();

        return;
    }

    var total = 0;
    var hardwareSummary = 0.0;
    var hardwareWeight = 0.0;
    var hardwareCount = 0;

    if (dataLoadedForEdit) {
        ShowLoadingMessage("Calculating Shipping and Handling");
        ListViewUtility.ShowLoadingImage();

        $.each($("[product-category=Hardware]").parent().find(".product-quantity"), function (index, item) {
            if ($(this).closest("tr").is(':visible') && $(this).closest("tr").find(".product-price").text().indexOf("-") == "-1") {
                hardwareCount += parseInt($(this).val());
            }
        });

        $.each($("[product-category=Hardware]").parent().find(".product-total"), function (index, item) {
            hardwareSummary += parseFloat($(this).text().replace('$', ''));
        });

        $.each($("[product-category=Hardware]").parent().find(".weight"), function (index, item) {
            var _this = $(this);
            var rowItem = _this.closest("tr");
            if (rowItem.attr("deleted") == "0") {
                hardwareWeight += (parseFloat(_this.val()) * parseFloat(rowItem.find(".product-quantity").val()));
            }
        });

        $.each($("[product-category='Misc Fee']").parent().find(".product-total"), function (index, item) {
            hardwareSummary += parseFloat($(this).text().replace('$', ''));
        });

        $.each($("[product-category='Misc Fee']").parent().find(".weight"), function (index, item) {
            var _this = $(this);
            var rowItem = _this.closest("tr");
            if (rowItem.attr("deleted") == "0") {
                hardwareWeight += (parseFloat(_this.val()) * parseFloat(rowItem.find(".product-quantity").val()));
            }
        });

        if ($("#shipCity").val().trim() == "" || $("#shipState").val() == null || $("#shipZip").val().trim() == "") {
            alert("Invalid Shipping Address.");

            HideLoadingMessage();
            ListViewUtility.CloseLoadingImage();
        }
        else {
            $.ajax({
                url: baseUrl + "/Quote/GetShippingAndHandling",
                type: 'POST',
                async: false,
                data: JSON.stringify({
                    TotalProduct: hardwareCount,
                    PackageWeight: "" + hardwareWeight,
                    PackageInsuredValue: "" + hardwareSummary,
                    ShipToAddress: $("#shipAddress1").val().trim(),
                    ShipToCity: $("#shipCity").val().trim(),
                    ShipToState: $("#shipState").val(),
                    ShipToZip: $("#shipZip").val().trim(),
                    ShipToCountry: $("#shipCountry").val()
                }),
                dataType: 'json',
                contentType: "application/json",
                success: function (result) {
                    var shType = $("[name=SH]:checked").val();
                    if (shType == "Ground") {
                        total = result.GroundRate;
                    } else if (shType == "2Day") {
                        total = result.SecondDayAirRate;
                    } else if (shType == "NextAir") {
                        total = result.NextDayAirRate;
                    }
                    //total += (hardwareCount * shippingAndHandlingServiceFee);
                    total += shippingAndHandlingServiceFee + (hardwareCount > 1 ? ((hardwareCount - 1) * 1) : 0);

                    ChangeShippingandHandlingServiceTitle($("#shipCountry").val());

                    if (result.Error != "" && result.Error != null) {
                        //alert("Invalid Shipping Address.");
                        alert("Shipping and Handling:\n\n" + result.Error);
                        total = 0;
                        $("#totalSalesTaxFees").text("$0.00");
                    } else {
                        /*if ($("#shipCountry").val() == "CA") {
                        $("#totalSalesTaxFees").text("$0.00");
                        } else {*/
                        CalculateSalesTax();
                        //}
                    }

                    $("#shippingAndHandling").text(total.toFixed(2));
                    $("#totalShippingAndHandlingFees").text("$" + total.toFixed(2));
                    //$("#totalCharged").text((parseFloat($("#totalHardwareFees").text().replace("$", "")) + parseFloat($("#totalMiscFees").text().replace("$", "")) + parseFloat($("#shippingAndHandling").text())).toFixed(2));
                    CalculateTotalCharged();

                    HideLoadingMessage();
                    ListViewUtility.CloseLoadingImage();
                },
                error: function () {
                    alert("Couldn't connect to the server. Please try again.");
                    HideLoadingMessage();
                    ListViewUtility.CloseLoadingImage();
                }
            });
        }
    }

    return total;
}
function loadQuoteTemplateDDL() {
    var hDDL = new Utility().loadTemplateWithParam(baseUrl + "/Common/LoadDDLWithParamJson", { 'spName': 'ug_Quote_Template_DDL', 'parameters': '@ClientID:0' });

    var groupName = '', ddlTemplateHtml, groupHtml = '';
    ddlTemplateHtml = '<option value="-1" disabled>----Select Template Here----</option>';
    $.each(hDDL, function (index, item) {
        if (item.groupValue != groupName) {
            if (index > 0) {
                groupHtml += '</optgroup>';
            }

            if (item.groupValue != '') {
                groupHtml += '<optgroup label="' + item.groupValue + '">';
            }

            groupName = item.groupValue;
        }

        if (item.groupValue == groupName) {
            groupHtml += "<option value='" + item.keyfield + "'>" + item.value + "</option>";
        } else {
            ddlTemplateHtml += groupHtml;
            groupHtml = '';
        }
    });

    if (groupHtml != '') {
        ddlTemplateHtml += groupHtml + '</optgroup>';
    }
    //$(".QuoteTemplateDDL-wrapper").append(hDDL);

    $("#SelVal").html(ddlTemplateHtml);
    $(".QuoteTemplateDDL-wrapper select").css("width", "450px");

    $('#SelVal').on('change', function () {
        if ($('#SelVal').val() != '-1') {
            try {
                dataLoadedForEdit = false;
                $.ajax({
                    url: baseUrl + "/QuoteTemplate/GetQuoteTemplate",
                    type: 'POST',
                    async: false,
                    data: { id: $('#SelVal').val() },
                    dataType: 'json',
                    success: function (result) {
                        if (result.Quote != null) {

                            $("#contractTerm").val(result.Quote.ContractTerm);
                            $("#validUntil").val(result.Quote.ValidUntil);
                            //$("#salesPerson").val(result.Quote.SalesPersonId);
                            $("#ZohoEntityId").val(result.Quote.ZohoEntityId);
                            $("#ZohoEntityType").val(result.Quote.ZohoEntityType);

                            if ($("#quoteId").val() == "") {
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

                                $("#shippingAndHandling").text(result.Quote.ShippingAndHandling.toFixed(2));
                                $("[name=SH][value=" + result.Quote.ShippingAndHandlingType + "]").attr("checked", true);

                                /*Start - Reset Form*/
                                $.each($(".Package"), function (index, item) {
                                    $(item).next().remove();
                                    $(item).remove();
                                });
                                packageTitleIndex = 1;
                                currentProductIndex = 1;
                                currentPackageIndex = 2;
                                InitProductContainer();
                                /*End - Reset Form*/
                            } else {
                                //Remove previous product
                                $.each($(".productDetail"), function (index, item) {
                                    item = $(item);
                                    item.attr("deleted", "1");
                                    item.hide();
                                });
                            }

                            var currentPackageId = 1;
                            $.each(result.QuoteProductList, function (index, quoteProduct) {
                                if (currentPackageId != quoteProduct.PackageId) {
                                    AddMoreProduct1();
                                    currentPackageIndex++;

                                    currentPackageId = quoteProduct.PackageId;
                                }

                                var divPackage = $(".PackageId[data-packageid=#" + currentPackageId + "]").parent();
                                var currentProductDiv = divPackage.find("[product-category='" + quoteProduct.ProductCategory + "']").parent().parent();

                                productType = quoteProduct.Price < 0 ? "discount" : "";
                                quoteProduct.QuoteProductId = 0;
                                AddNewProduct(currentProductDiv, quoteProduct);
                            });

                            dataLoadedForEdit = true;

                            if (result.Quote.ShipToCity != '' && result.Quote.ShipToState != '' && result.Quote.ShipToZip != '') {
                                CalculateShippingAndHandling();
                            }
                        }
                    }
                });

            }
            catch (err) {
            }
        }
    })
}

function MakeReadOnly() {
    $(".remove-item").parent().html("");
    $(".product-quantity").attr("readonly", "readonly");
    $(".product-action").parent().attr("style", "line-height: 0");
    $(".product-action").parent().html("&nbsp;");
}

var productList = null;
var discount10ProductItem;
var discount15ProductItem;
var discount20ProductItem;

function AddAutoDiscount(currentProductDiv, productQuantity, productPrice) {
    if (dataLoadedForEdit) {
        if (productList == null) {
            $.ajax({
                url: baseUrl + "/ProductConfigurator/GetProductList",
                type: 'POST',
                async: false,
                dataType: "json",
                success: function (data) {
                    productList = data;

                    discount10ProductItem = $.grep(productList, function (item) { return item.ProductId == discount10ProductId; })[0];
                    discount15ProductItem = $.grep(productList, function (item) { return item.ProductId == discount15ProductId; })[0];
                    discount20ProductItem = $.grep(productList, function (item) { return item.ProductId == discount20ProductId; })[0];
                }
            });
        }

        var currentDiscountProductId = null;
        if (productQuantity > 99) {
            RemoveAutoDiscountItem(currentProductDiv, discount10ProductItem.ProductId);
            RemoveAutoDiscountItem(currentProductDiv, discount15ProductItem.ProductId);

            productType = "discount";
            discount20ProductItem.QuoteProductId = 0;
            discount20ProductItem.Quantity = productQuantity;
            discount20ProductItem.Price = productPrice * 0.20;
            AddNewProduct(currentProductDiv, discount20ProductItem);
        } else if (productQuantity > 49 && productQuantity <= 99) {
            RemoveAutoDiscountItem(currentProductDiv, discount10ProductItem.ProductId);
            RemoveAutoDiscountItem(currentProductDiv, discount20ProductItem.ProductId);

            productType = "discount";
            discount15ProductItem.QuoteProductId = 0;
            discount15ProductItem.Quantity = productQuantity;
            discount15ProductItem.Price = productPrice * 0.15;
            AddNewProduct(currentProductDiv, discount15ProductItem);
        } else if (productQuantity > 9 && productQuantity <= 49) {
            RemoveAutoDiscountItem(currentProductDiv, discount15ProductItem.ProductId);
            RemoveAutoDiscountItem(currentProductDiv, discount20ProductItem.ProductId);

            productType = "discount";
            discount10ProductItem.QuoteProductId = 0;
            discount10ProductItem.Quantity = productQuantity;
            discount10ProductItem.Price = productPrice * 0.10;
            AddNewProduct(currentProductDiv, discount10ProductItem);
        } else {
            RemoveAutoDiscountItem(currentProductDiv, discount10ProductItem.ProductId);
            RemoveAutoDiscountItem(currentProductDiv, discount15ProductItem.ProductId);
            RemoveAutoDiscountItem(currentProductDiv, discount20ProductItem.ProductId);
        }
    }
}

function RemoveAutoDiscountItem(currentProductDiv, productId) {
    var deletedRow = currentProductDiv.find(".product-id[product-id=" + productId + "]").closest("tr");
    deletedRow.hide();
    deletedRow.attr("deleted", "1");
    deletedRow.find(".product-price").text("$0.00");
    deletedRow.find(".product-extprice").text("$0.00");
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

function CalculateSalesTax() {
    if (dataLoadedForEdit) {
        var salesTaxItem = $.grep(salesTaxList, function (item) { return item.StateShortName == $("#shipState").val(); })[0];
        if ($("#shipState").val() == null || salesTaxItem == null) {
            alert("Sales Tax\n* Invalid Shipping State.");
            $("#totalSalesTaxFees").text("$0.00");
            return;
        }

        var hardwareSummary = 0.0;
        $.each($("[product-category=Hardware]").parent().find(".product-total"), function (index, item) {
            hardwareSummary += parseFloat($(this).text().replace('$', ''));
        });

        //$("#totalSalesTaxFees").text("$" + (hardwareSummary * (salesTaxItem.TaxRate / 100.0)).toFixed(2));
        $("#totalSalesTaxFees").text("$" + TytRound((hardwareSummary * (salesTaxItem.TaxRate / 100.0)),2));
    }
}

function TytRound(value, places) {
    return +(Math.round(value + "e+" + places) + "e-" + places);
}

function CalculateTotalCharged() {
    $("#totalCharged").text((
        parseFloat($("#totalHardwareFees").text().replace("$", ""))
        + parseFloat($("#totalSalesTaxFees").text().replace("$", ""))
        + parseFloat($("#totalMiscFees").text().replace("$", ""))
        + parseFloat($("#shippingAndHandling").text())
    ).toFixed(2));
}

function ChangeShippingandHandlingServiceTitle(country) {
    if (country == "CA") {
        $("#shGround").text("Standard,");
        $("#shGround").attr("title", "Day Definite by Date Scheduled");

        $("#shSecondAir").text("Expedited,");
        $("#shSecondAir").attr("title", "2-5 Business Days");

        $("#shNextAir").text("Saver,");
        $("#shNextAir").attr("title", "1-3 Business Days");
    }
    else {
        $("#shGround").text("Ground,");
        $("#shGround").attr("title", "1-5 Business Days");

        $("#shSecondAir").text("2nd day AIR,");
        $("#shSecondAir").attr("title", "2 Business Days");

        $("#shNextAir").text("Next day AIR,");
        $("#shNextAir").attr("title", "Next Business Day");
    }
}

function HasPOBox(data) {
    data = data.toLowerCase();

    if (data.indexOf("p.o. box") > -1
        || data.indexOf("p.o.") > -1
        || data.indexOf("po box") > -1
        || data.indexOf("p o box") > -1) {
        return true;
    }

    return false;
}


function InitVar() {
    maxProductCount = 1;
    dataLoadedForEdit = true;
    disableQuantitySync = false;

    packageTitleIndex = 1;
    currentProductIndex = 1;
    currentPackageIndex = 2;

    valueChanged = false;
}

function ClearDetailForm() {
    $("#SelVal").val('-1');
    $("#adisableQuantityRestriction").text("Disable Qty Sync");
    $("#chkIsDemo").removeAttr("checked");

    $("#quoteDate").text(GetFormattedDate(new Date()));
    $("#quoteNumber").text("");
    $("#contractTerm").val("");
    $("#validUntil").val("");

    var salesPerson = $("#salesPerson");
    salesPerson.val(employeeId);
    if (salesPerson.val() == employeeId) {
        salesPerson.attr("disabled", "disabled");
    }

    SetTYTSalesPerson();

    $("#ZohoEntityId").val("");
    $("#ZohoEntityType").val("");
    $("#nettrackClientId").val("");
    $("#nettrackClient").val("");

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

    $("#txtNote").val("");

    $("#shippingAndHandling").text("0.00");
    $("[name=SH][value=Ground]").attr("checked", true);

    $("#totalHardwareFees").text("$0.00");
    $("#totalSalesTaxFees").text("$0.00");
    $("#totalMonthlyFees").text("$0.00");
    $("#totalOptionalFees").text("$0.00");
    $("#totalMiscFees").text("$0.00");
    $("#totalShippingAndHandlingFees").text("$0.00");
    $("#totalCharged").text("0.00");

    $("#totalProductWeight").text("0");
}

function GetFormattedDate(date) {
    var year = date.getFullYear();
    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return month + '/' + day + '/' + year;
}