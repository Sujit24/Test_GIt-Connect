var salesPersonList = [];
var maxProductCount = 1;
var dataLoadedForEdit = true;

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
    LoadTemplateGroup();
    LoadSalesPerson();
    LoadSalesTaxList();
    LoadStateDDL('US', $('#billState'));
    LoadStateDDL('US', $('#shipState'));

    $("#zohoLinkType").change(function () {
        if ($(this).val() == "Lead") {
            $("#Search-Zuhu-pattern").attr("placeholder", "Company Name");
        } else {
            $("#Search-Zuhu-pattern").attr("placeholder", "Account Name");
        }

        $("#dvAccountList-wrapper").css('display', 'none');
        $("#dvContactList-wrapper").css('display', 'none');
    });

    $("#Search-Zuhu-btn").click(function (e) {
        ListViewUtility.ShowLoadingImage();
        showZuhuModalLoader();

        $('#btnSaveZoho').attr('disabled', true);
        $('#dvAccountList').html('');
        $('#dvContactList').html('');

        var requestUrl = baseUrl;
        if ($("#zohoLinkType").val() == "Lead") {
            requestUrl += "/QuoteTemplate/SearchLeadInZoho";
        } else if ($("#zohoLinkType").val() == "Contact") {
            requestUrl += "/QuoteTemplate/SearchContactInZoho";
        } else if ($("#zohoLinkType").val() == "Account") {
            requestUrl += "/QuoteTemplate/SearchAccountInZoho";
        }

        $.ajax({
            url: requestUrl,
            type: 'POST',
            async: true,
            data: { searchText: $('#Search-Zuhu-pattern').val() },
            dataType: "json",
            success: function (result) {
                if ($("#zohoLinkType").val() == "Lead") {
                    ProcessLeads(result);
                } else if ($("#zohoLinkType").val() == "Contact") {
                    ProcessContacts(result);
                } else if ($("#zohoLinkType").val() == "Account") {
                    ProcessAccounts(result);
                }

                hideZuhuModalLoader();
            },
            error: function (result) {
                ListViewUtility.CloseLoadingImage();
                hideZuhuModalLoader();
            }
        });
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

    InitProductContainer();

    if ($("#quoteId").val() != "") {
        dataLoadedForEdit = false;
        Edit($("#quoteId").val());
    }
    else {
        dataLoadedForEdit = true;
        $("#quoteNumber").text("");
    }

    $("#billCountry").change(function () {
        LoadStateDDL($(this).val(), $('#billState'));
    });

    $("#shipCountry").change(function () {
        LoadStateDDL($(this).val(), $('#shipState'));
    });
});

var leadList = [];
function ProcessLeads(responsedata) {
    responsedata = responsedata == '' ? null : eval("(" + responsedata + ")");

    //var jsonResult = eval("(" + data + ")")
    var leadId = '', companyName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
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

    //var jsonResult = eval("(" + data + ")")
    var accountId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', phone = '';
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

    var requestUrl = baseUrl + "/QuoteTemplate/SearchContactInZoho";

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

    //var jsonResult = eval("(" + data + ")");
    var contactId = '', accountName = '', SMOWNERID = '', street = '', city = '', state = '', zipCode = '', firstName = '', lastName = '', email = '', phone = '';
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

    $.each(context.find(".product-quantity"), function (index, item) {
        if ($(item).closest("table").attr("product-category") != "Optional") {
            if ($(item).val() > tmpMaxProdCount) {
                tmpMaxProdCount = $(item).val();
            }
        }
    });

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
    } else {
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
        }
    });
}

function AddNewProduct(currentProductDiv, newRecord) {
    var productQuantityValue = newRecord.Quantity;
    var newRow = "<tr class='productDetail' deleted='0'><td style='padding-top: 5px;text-align: right; width: 118px'><a class='up-item pull-left' style='text-decoration: none' href=''>↑</a><a class='down-item pull-left' style='text-decoration: none' href=''>↓</a>&nbsp;<a href='' class='remove-item'>Remove</a><input class='quote-product-id' type='hidden' value='" + newRecord.QuoteProductId + "'><input class='weight' type='hidden' value='" + newRecord.Weight + "'></td>";

    if ((currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType != "discount")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Optional")) {
        newRow = newRow.replace("<a href='' class='remove-item'>Remove</a>", "<a href='' class='update-item'>Update</a>&nbsp;|&nbsp;<a href='' class='remove-item'>Remove</a>");
    }

    if (((currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType == "discount")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Monthly")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Misc Fee"))) {
        newRow += "<td style='text-align: center;'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><input readonly='readonly' class='product-quantity' type='text' style='width: 40px; text-align: center' value='" + newRecord.Quantity + "'></td>";
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
        MakeSummary();
    });

    $(".update-item").unbind();
    $(".update-item").click(function (e) {
        e.preventDefault();
        var aUpdateItem = $(this);
        currentProductDiv = aUpdateItem.closest(".product");
        var rowItem = aUpdateItem.closest('tr');
        var productQuantity = rowItem.find('.product-quantity');

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

        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            AddAutoDiscount(currentProductDiv, productQuantity.val(), productPrice);
        }

        MakeSameQuantity(rowItem.closest("[class=Package]"));
        SumProductCost(currentProductDiv);
        if (productQuantity.closest("table").attr("product-category") == "Hardware") {
            SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
            SumProductCost(currentProductDiv.parent().find("[product-category='Misc Fee']").closest(".product"));
        }
        MakeSummary();
    });

    if (currentProductDiv.find("table").attr("product-category") == "Hardware" && productType != "discount") {
        AddAutoDiscount(currentProductDiv, productQuantityValue, newRecord.Price);
    }

    MakeSameQuantity(currentProductDiv.closest("[class=Package]"));
    SumProductCost(currentProductDiv);
    if (currentProductDiv.find("table").attr("product-category") == "Hardware") {
        SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
    }
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

var tytSalesPersonList = [];
function LoadSalesPerson() {
    $.ajax({
        url: baseUrl + "/QuoteTemplate/GetSalesPerson",
        type: 'POST',
        async: false,
        dataType: "json",
        success: function (result) {
            tytSalesPersonList = result;

            $.each(result, function (index, item) {
                $("#salesPerson").append("<option value='" + item.EmployeeId + "'>" + item.Name + "</option>");
            });

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

function SaveAs() {
    $("#quoteId").val("0");
    $('.quote-product-id').val("0")

    Save();
}

function Save() {
    var postData = {};
    postData.QuoteTemplateModel = {};
    postData.QuoteTemplateModel.QuoteId = $("#quoteId").val() == "" ? "0" : $("#quoteId").val();
    postData.QuoteTemplateModel.TemplateName = $("#TemplateName").val();
    postData.QuoteTemplateModel.QuoteTemplateGroupId = $("#ddlTemplateGroup").val();
    postData.QuoteTemplateModel.QuoteDate = $("#quoteDate").text();
    postData.QuoteTemplateModel.ContractTerm = $("#contractTerm").val();
    postData.QuoteTemplateModel.ValidUntil = $("#validUntil").val();
    postData.QuoteTemplateModel.SalesPersonId = $("#salesPerson").val();
    postData.QuoteTemplateModel.ZohoEntityId = $("#ZohoEntityId").val();
    postData.QuoteTemplateModel.ZohoEntityType = $("#ZohoEntityType").val();

    postData.QuoteTemplateModel.BillToCompanyName = $("#billCompanyName").val();
    postData.QuoteTemplateModel.BillToAddress1 = $("#billAddress1").val();
    postData.QuoteTemplateModel.BillToAddress2 = $("#billAddress2").val();
    postData.QuoteTemplateModel.BillToCity = $("#billCity").val();
    postData.QuoteTemplateModel.BillToState = $("#billState").val();
    postData.QuoteTemplateModel.BillToZip = $("#billZip").val();
    postData.QuoteTemplateModel.BillToCountry = $("#billCountry").val();
    postData.QuoteTemplateModel.BillToBillingContact = $("#billContact").val();
    postData.QuoteTemplateModel.BillToBillingEmail = $("#billEmail").val();
    postData.QuoteTemplateModel.BillToPhone = $("#billPhone").val();

    postData.QuoteTemplateModel.IsShipSameAsBill = $("#sameAsBillTo").is(":checked");

    postData.QuoteTemplateModel.ShipToCompanyName = $("#shipCompanyName").val();
    postData.QuoteTemplateModel.ShipToAddress1 = $("#shipAddress1").val();
    postData.QuoteTemplateModel.ShipToAddress2 = $("#shipAddress2").val();
    postData.QuoteTemplateModel.ShipToCity = $("#shipCity").val();
    postData.QuoteTemplateModel.ShipToState = $("#shipState").val();
    postData.QuoteTemplateModel.ShipToZip = $("#shipZip").val();
    postData.QuoteTemplateModel.ShipToCountry = $("#shipCountry").val();
    postData.QuoteTemplateModel.ShipToBillingContact = $("#shipContact").val();
    postData.QuoteTemplateModel.ShipToBillingEmail = $("#shipEmail").val();
    postData.QuoteTemplateModel.ShipToPhone = $("#shipPhone").val();

    postData.QuoteProductModelList = [];

    var errorMessage = "";

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

            if (product.closest("table").attr("product-category") == "Hardware"
                && tempProduct.Price > -1
                && !tempProduct.Deleted) {
                hasHardware = true;
            }
        })

        if (!hasHardware && Package.is(":visible")) {
            errorMessage += ", Product " + Package.find("h2").data("packageid");
        }
    })

    if (errorMessage != "") {
        alert(errorMessage.substr(2) + " must have at least one hardware product.\n");
    }
    else {
        try {
            $.ajax({
                url: baseUrl + "/QuoteTemplate/Save",
                type: 'POST',
                async: true,
                data: JSON.stringify(postData),
                dataType: 'json',
                contentType: "application/json",
                success: function (result) {
                    if (result.Message == "Successfully Saved.") {
                        alert("Quote Template Successfully Saved.\nQuote Template Number: " + result.PrimaryKey);
                        location.href = baseUrl + "/QuoteTemplate/List";
                    }
                    else {
                        alert("Quote Template Save Failed.");
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
}

function Edit(quoteId) {
    try {
        $.ajax({
            url: baseUrl + "/QuoteTemplate/GetQuoteTemplate",
            type: 'POST',
            async: false,
            data: { id: quoteId },
            dataType: 'json',
            success: function (result) {
                if (result.Quote != null) {
                    $("#templateId").text(result.Quote.QuoteId);
                    $("#divTemplateId").show();
                    $("#TemplateName").val(result.Quote.TemplateName);
                    $("#ddlTemplateGroup").val(result.Quote.QuoteTemplateGroupId);
                    $("#quoteDate").text(result.Quote.QuoteDateFormated);
                    $("#contractTerm").val(result.Quote.ContractTerm);
                    $("#validUntil").val(result.Quote.ValidUntil);
                    $("#salesPerson").val(result.Quote.SalesPersonId);
                    SetTYTSalesPerson();
                    $("#ZohoEntityId").val(result.Quote.ZohoEntityId);
                    $("#ZohoEntityType").val(result.Quote.ZohoEntityType);

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

function LoadTemplateGroup() {
    $.ajax({
        url: baseUrl + "/QuoteTemplate/GetQuoteTemplateGroupList",
        type: 'GET',
        async: false,
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, item) {
                $("#ddlTemplateGroup").append("<option value='" + item.keyfield + "'>" + item.value + "</option>");
            });
        }
    });
}