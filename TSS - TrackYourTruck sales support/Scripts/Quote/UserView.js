var quoteItem = null;
var salesPersonList = [];
var maxProductCount = 1;
var dataLoadedForEdit = false;
var disableQuantitySync = false;
var shippingErrorCount = 0;
var shippingErrorCountForNotResolve = 0;
var quantityValueChanged = false;
var isDemo = false;

function ViewConfirmAlert() {
    var d = $.Deferred();

    $("#viewConfirmModal").modal({
        backdrop: 'static',
        keyboard: false
    });

    if ($.browser.version == "10.0" || $.browser.version == "11.0") {
        $('#viewConfirmModal').addClass('in');
        $('#viewConfirmModal').css('display', 'block');
    }

    $("#btnViewQuote").click(function () {
        d.resolve();

        $("#viewConfirmModal").modal('hide');
        if ($.browser.version == "10.0" || $.browser.version == "11.0") {
            $(".modal-backdrop.fade").remove();
        }
    });

    return d.promise();
}

$(function () {
    $('.modal').on('hidden', function () {
        if ($.browser.version == "10.0" || $.browser.version == "11.0") {
            $(".modal-backdrop.fade").remove();
        }
    })

    ViewConfirmAlert().done(function () {
        LoadSalesTaxList();

        InitProductContainer();
        LoadPaymentMethods();

        if ($("#quoteId").val() != "") {
            Edit($("#quoteId").val());
        }
        else {
            $(".main-content").removeClass("col-md-8");
            $(".main-content").html("<div style='text-align: center;'><h1 style='color:red'>Quote Link Expired</h1></div>");
        }

        $("#chkAcceptance").change(function () {
            if ($(this).is(":checked")) {
                $("#txtAcceptanceName").show();
                $("#txtAcceptanceName").focus();
            } else {
                $("#txtAcceptanceName").hide();
            }
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

        $("[name=SH]").change(function () {
            CalculateShippingAndHandling();
        });

        $("#shipAddress1").change(function () {
            if (HasPOBox($(this).val())) {
                alert("Cannot ship to P.O. Box via UPS");
            }
            else {
                //CalculateShippingAndHandling();
            }
        });

        $("#shipAddress2").change(function () {
            if (HasPOBox($(this).val())) {
                alert("Cannot ship to P.O. Box via UPS");
            }
        });

        $("#shipCity").change(function () {
            //CalculateShippingAndHandling();
        });

        $("#shipState").change(function () {
            //CalculateShippingAndHandling();
            //CalculateTotalCharged();
        });

        $("#shipZip").change(function () {
            //CalculateShippingAndHandling();
        });

        $("#billCountry").change(function () {
            LoadStateDDL($(this).val(), $('#billState'));
        });

        $("#shipCountry").change(function () {
            LoadStateDDL($(this).val(), $('#shipState'));

            ChangeShippingandHandlingServiceTitle($(this).val());
            //CalculateShippingAndHandling();
        });
    });
});

function AddNewProduct(currentProductDiv, newRecord) {
    var newRow = "<tr class='productDetail' deleted='0'>";
    if (((currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType == "discount")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Monthly")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Misc Fee"))) {
        newRow += "<td style='text-align: center;padding-bottom: 5px;'><input class='quote-product-id' type='hidden' value='" + newRecord.QuoteProductId + "'><input class='weight' type='hidden' value='" + newRecord.Weight + "'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><input " + (disableQuantitySync ? "" : "readonly='readonly'") + " class='product-quantity' type='text' style='" + (disableQuantitySync ? "border:1px solid darkcyan;" : "") + "width: 40px; text-align: center' value='" + newRecord.Quantity + "'>" + (disableQuantitySync ? "<br /><a href='#' class='product-update'>Update</a>" : "") + "</td>";
    }
    else {
        newRow += "<td style='text-align: center;padding-bottom: 5px;'><input class='quote-product-id' type='hidden' value='" + newRecord.QuoteProductId + "'><input class='weight' type='hidden' value='" + newRecord.Weight + "'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><input class='product-quantity' type='text' style='border:1px solid darkcyan; width: 40px; text-align: center' value='" + newRecord.Quantity + "'><br /><a href='#' class='product-update'>Update</a></td>";
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
        var productQuantityValue = parseInt(productQuantity.val()) + parseInt(newRecord.Quantity);

        if (!rowItem.is(":visible")) {
            rowItem.attr("deleted", "0");
            rowItem.show();
            productQuantityValue = newRecord.Quantity;
        }

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

    $(".product-update").unbind();
    $(".product-update").click(function (e) {
        e.preventDefault();
        var _this = $(this);

        var productQuantity = $(this).parent().find(".product-quantity");
        currentProductDiv = productQuantity.closest(".product");
        var rowItem = productQuantity.closest('tr');

        if (!$.isNumeric(productQuantity.val()) || (parseInt(productQuantity.val()) + "") != productQuantity.val()) {
            alert("Invalid Quantity");
            return;
        }

        if (parseFloat(productQuantity.val()) < 1.0) {
            alert("Quantity can't be less than 1.");
            return;
        }

        var isQuantityDecreased = false;
        $.each(quoteItem.QuoteProductList, function (index, item) {
            if (_this.parent().find(".quote-product-id").val() == item.QuoteProductId && parseInt(productQuantity.val()) < item.Quantity) {
                isQuantityDecreased = true;
                productQuantity.val(item.Quantity);
            }
        });
        if (isQuantityDecreased) {
            alert("Please contact sales person listed on your quote to reduce quantity of Units purchased.");
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
        if ($("#shipCity").val() != "" && $("#shipState").val() != null && $("#shipZip").val() != "") {
            CalculateShippingAndHandling();
        }
        MakeSummary();

        quantityValueChanged = false;
    });

    $(".product-quantity").unbind();
    $(".product-quantity").keydown(function (e) {
        // Allow: backspace, delete, tab, escape and enter
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        } else {
            quantityValueChanged = true;
        }
    });

    MakeSameQuantity(currentProductDiv.closest("[class=Package]"));
    SumProductCost(currentProductDiv);
    if (currentProductDiv.find("table").attr("product-category") == "Hardware") {
        SumProductCost(currentProductDiv.parent().find("[product-category=Monthly]").closest(".product"));
    }
    ProductSummary(currentProductDiv.parent());
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


var currentProductIndex = 1;
var currentPackageIndex = 2;
var activeProductIndex;
function InitProductContainer() {
    AddMoreProduct1();
}

function AddMoreProduct1() {
    var templateContent = $("#productTemplate").html();
    var template = kendo.template(templateContent);

    var data = [{ "Title": "Hardware", "ProductIndex": '#' + currentProductIndex }];
    var result = kendo.render(template, data);
    $("#productContent").append(result);

    $("[product-index=#" + currentProductIndex + "]").find("[product-type=product]").hide();
    $("[product-index=#" + currentProductIndex + "]").find("[product-type=monthly]").hide();
    $("[product-index=#" + currentProductIndex + "]").find("[product-type=service]").hide();

    currentProductIndex++;
}

var productType = "";

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

    total += parseFloat($("#shippingAndHandling").text().substr(1));

    if (total < 0) {
        currentProductDiv.find(".product-summary").html("-$" + (-1 * total).toFixed(2));
    } else {
        currentProductDiv.find(".product-summary").html("$" + total.toFixed(2));
    }
}

function ContinueSave() {
    ListViewUtility.ShowLoadingImage();

    var postData = {};
    postData.QuoteOrderModel = {};
    postData.QuoteOrderModel.QuoteId = $("#quoteId").val() == "" ? "0" : $("#quoteId").val();
    postData.QuoteOrderModel.QuoteNumber = "";
    postData.QuoteOrderModel.QuoteDate = $("#quoteDate").text();
    postData.QuoteOrderModel.ContractTerm = $("#contractTerm").text();
    postData.QuoteOrderModel.ValidUntil = $("#validUntil").text();
    postData.QuoteOrderModel.SalesPersonId = $("#salesPersonId").text();
    postData.QuoteOrderModel.ZohoEntityId = $("#ZohoEntityId").text();
    postData.QuoteOrderModel.ZohoEntityType = $("#ZohoEntityType").text();
    postData.QuoteOrderModel.ClientID = $("#clientId").val();

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
    postData.QuoteOrderModel.ShippingAndHandlingType = $("[name=SH]:Checked").val();

    postData.QuoteOrderModel.IsAccepted = $("#chkAcceptance").is(":checked");
    postData.QuoteOrderModel.AcceptanceName = $("#txtAcceptanceName").val();
    postData.QuoteOrderModel.AcceptanceDate = $("#lblAcceptanceDateTime").text();

    postData.QuoteOrderModel.SalesTax = $("#totalSalesTaxFees").text().replace("$", "");

    postData.QuoteOrderModel.QuotePaymentMethodId = $("#paymentMethodType").val();
    postData.QuoteOrderModel.PaymentMethodComment = $("#paymentMethodComment").val();

    postData.QuoteOrderModel.Note = $("#txtNote").text();

    postData.QuoteOrderModel.IsDemo = isDemo;

   
    postData.SalesOrderModelList = [];
    var tempProduct = null;
    $('.Package').each(function () {
        var Package = $(this);

        Package.find('.productDetail').each(function () {
            var product = $(this);
            tempProduct = {};
            //tempProduct.SalesOrderId = product.find('.quote-product-id').val();
            tempProduct.ProductId = product.find('.product-id').attr('product-id');
            tempProduct.Quantity = product.find('.product-quantity').val();
            tempProduct.Price = product.find('.product-price').text().replace('$', '');
            tempProduct.PackageId = Package.find('.PackageId').data('packageid');
            tempProduct.ProductCategory = product.closest("table").attr("Product-Category");
            tempProduct.Deleted = product.attr("deleted") == "1" ? true : false;
            postData.SalesOrderModelList.push(tempProduct);
        })
    })

    var total = 0.0;
    total += parseFloat($("#totalHardwareFees").text().replace("$", ""));
    total += parseFloat($("#totalSalesTaxFees").text().replace("$", ""));
    total += parseFloat($("#totalMiscFees").text().replace("$", ""));
    total += parseFloat($("#totalShippingAndHandlingFees").text().replace("$", ""));
    total = total.toFixed(2);

    postData.TotalValue = total;
    ///////--- for "In Contract Replacement" , set payment method Id to "0" / empty payment id
    if ($("#spnOrderType").text() === "Contract Renewal Only") {
        postData.QuoteOrderModel.QuotePaymentMethodId = 8;
        //postData.TotalValue = 0;
    }

    try {
        $.ajax({
            url: baseUrl + "/Quote/UserViewSave",
            type: 'POST',
            async: true,
            data: JSON.stringify(postData),
            dataType: 'json',
            contentType: "application/json",
            success: function (result) {
                ListViewUtility.CloseLoadingImage();

                if (result.PrimaryKey == 0) {
                    alert("Purchase failed. Please try again later.");
                }
                else {
                    $('#convertToOrderModal').modal('hide');
                    if ($.browser.version == "10.0" || $.browser.version == "11.0") {
                        $(".modal-backdrop.fade").remove();
                    }

                    if (result.Message != "") {
                        location.href = result.Message;
                    } else {
                        var message = "";
                        message += "Thank you for your order # " + postData.QuoteOrderModel.QuoteId + " in the amount of $" + postData.TotalValue + "!\n";
                        message += "Our team will review your order and contact you promptly.";

                        alert(message);
                    }

                    $('.purchaseNow').attr('disabled', true);
                }
            },
            error: function (result) {
                ListViewUtility.CloseLoadingImage();

                $('#lblWarning').text("Error occurred");
            }
        });

    }
    catch (err) {
    }
}

function Save() {
    if ($("#spnOrderType").text() === "Contract Renewal Only") {
        ContinueSave();
        return;
    }
    if (quantityValueChanged) {
        alert('Quantity has been changed. Please click on Update to synchronize the quantity.');

        return;
    }

    ValidateShippingAddress();
}

function InitSaveConfirm() {
    CalculateShippingAndHandling(false);

    if (!Validate()) {
        $('html, body').animate({
            scrollTop: $("#billCompanyName").offset().top
        }, 1000);

        return;
    }

    if (!$("#chkAcceptance").is(":checked")) {
        alert("Please accept the Terms and Condition first.");

        $('html, body').animate({
            scrollTop: $("#chkAcceptance").offset().top
        }, 1000);
        $("#chkAcceptance").focus();

        return;
    }

    if ($("#txtAcceptanceName").val().trim() == "") {
        alert("Please enter your name.");

        $('html, body').animate({
            scrollTop: $("#txtAcceptanceName").offset().top
        }, 1000);
        $("#txtAcceptanceName").focus();

        return;
    }

    if (parseFloat($("#shippingAndHandling").text()) <= 0.0 && $("[name=SH]:Checked").val() != "Free" && shippingErrorCount == 0 && !isDemo) {
        alert("The system cannot process Shipping & Handling charge. Please re-check and provide proper Shipping address.");
        return;
    }

    //Now show hardware confirmation modal

    ShowHardwareConfirmationModal();
}

function ShowHardwareConfirmationModal() {
    $('#confirmHardwareModal').modal('show');

    if ($.browser.version == "10.0" || $.browser.version == "11.0") {
        $('#confirmHardwareModal').addClass('in');
        $('#confirmHardwareModal').css('display', 'block');
    }

    $("#btnCloseConfirmHardwareModal").unbind();
    $("#btnCloseConfirmHardwareModal").click(function () {
        if ($.browser.version == "10.0" || $.browser.version == "11.0") {
            $(".modal-backdrop.fade").remove();
        }
    });

    var hardwareProductIdList = [];
    var hardwareProductId = null;
    $.each($("[product-category=Hardware] .productDetail"), function (index, item) {
        if ($(item).find(".product-price").text().indexOf('-') == -1) {
            hardwareProductId = $(item).find(".product-id").attr("product-id");
            hardwareProductIdList.push(hardwareProductId);
        }
    });

    if (hardwareProductId != null) {
        $.ajax({
            url: baseUrl + "/ProductConfigurator/GetHardwareProducts",
            type: 'POST',
            async: false,
            data: JSON.stringify({ id: hardwareProductIdList }),
            dataType: 'json',
            contentType: "application/json",
            success: function (result) {
                if (result != null) {
                    var html = "<table style='width: 100%;border-collapse:separate;border-spacing:1em;'>";
                    $.each(result, function (index, item) {
                        if (hardwareProductIdList.indexOf(item.ProductId + '') > -1) {
                            html += "<tr>";
                            html += "<td><input type='hidden' class='SKU' value='" + item.SKU + "' /><input type='hidden' class='ProductDescription' value='" + item.ProductDescription + "' /><input type='hidden' class='Price' value='" + item.Price + "' /><input type='hidden' class='Weight' value='" + item.Weight + "' />"
                                    + "</td>";
                            html += "<td class='image-td-width'>";
                            if (item.ProductImageFileName != '') {
                                html += "<img class='image-max-width' src='" + baseUrl + "/Content/ProductImage/" + item.ProductImageFileName + "' alt='' />";
                            }
                            html += "</td>";
                            html += "<td style='vertical-align: top;text-align: justify;'><div style='font-weight: bold;'>" + item.ProductName + "</div>";
                            html += "<div>Quantity: " + $("[product-id=" + item.ProductId + "]").parent().find(".product-quantity").val() + "</div>";
                            html += "<div style='line-height: 10px;'>&nbsp;</div>";
                            if (item.Notes != "") {
                                html += "<div style='color:red'><span style='text-decoration: underline; color:red; font-weight:bold'>Note:</span> " + item.Notes + "</div>";
                            }
                            html += "</td>";
                            html += "</tr>";
                        }
                    });
                    html += "</table>";

                    $("#hardwareConfirm").html(html);
                }
            }
        });

        /*$("[name=selectedHardware]").unbind();
        $("[name=selectedHardware]").change(function () {
            hardwareProductId = null;
            $.each($("[product-category=Hardware] .productDetail"), function (index, item) {
                if ($(item).find(".product-price").text().indexOf('-') == -1) {
                    hardwareProductId = $(item).find(".product-id").attr("product-id");
                }
            });

            if (hardwareProductId != null) {
                var tr = $(this).closest('tr');

                var productId = $(this).val();
                var sku = tr.find(".SKU").val();
                var productDescription = tr.find(".ProductDescription").val();
                var price = tr.find(".Price").val();
                var weight = tr.find(".Weight").val();

                var hRow = $("[product-category=Hardware] .productDetail [product-id=" + hardwareProductId + "]").closest('tr');
                hRow.find('.product-id').attr('product-id', productId);
                hRow.find('.weight').val(weight);
                hRow.find('td').eq(1).html(sku);
                hRow.find('td').eq(2).html(productDescription);
                hRow.find('.product-price').text('$' + price);
                hRow.find('.product-extprice').text('$' + parseFloat(hRow.find('.product-quantity').val()) * parseFloat(price));

                SumProductCost(hRow.closest("[class=product]"));
                if ($("#shipCity").val() != "" && $("#shipState").val() != null && $("#shipZip").val() != "") {
                    CalculateShippingAndHandling();
                }
                MakeSummary();
            }
        });*/
    }
}

function ShowPaymentModal() {
    $('#confirmHardwareModal').modal('hide');
    if ($.browser.version == "10.0" || $.browser.version == "11.0") {
        $(".modal-backdrop.fade").remove();
    }
    if ($("#spnOrderType").text() === "New Customer") {
        $("#achMsg").text("ACH payment may delay your order by 5-7 business days as we wait for ACH payment to be received before shipment is released.  You may want to choose Credit Card for faster processing.");
    }
    $("#paymentMethodType").val(7);
    $("#paymentMethodType").unbind();
    $("#paymentMethodType").change(function () {
        if (this.value != "3" && this.value != "5") {
            $("#divPaymentMethodMessage").text('* Please note that your order will NOT be shipped until we receive payment.');
            $("#achMsg").text("");
            console.log(this.value);
        }else {
            $("#achMsg").text("");
            $("#divPaymentMethodMessage").text('');
        }
         
        if ($("#spnOrderType").text() === "New Customer" && this.value == "7") {
            $("#achMsg").text("ACH payment may delay your order by 5-7 business days as we wait for ACH payment to be received before shipment is released.  You may want to choose Credit Card for faster processing.");
            console.log(this.value);
        }

    });

    $('#convertToOrderModal').modal('show');

    if ($.browser.version == "10.0" || $.browser.version == "11.0") {
        $('#convertToOrderModal').addClass('in');
        $('#convertToOrderModal').css('display', 'block');
    }

    $("#btnCloseConvertToOrderModal").unbind();
    $("#btnCloseConvertToOrderModal").click(function () {
        if ($.browser.version == "10.0" || $.browser.version == "11.0") {
            $(".modal-backdrop.fade").remove();
        }
    });
}

function Edit(quoteId) {
    try {
        $.ajax({
            url: baseUrl + "/Quote/UserViewGetQuote",
            type: 'POST',
            async: false,
            data: { id: quoteId },
            dataType: 'json',
            success: function (result) {
                if (result.Quote != null) {
                    quoteItem = result;

                    $("#quoteNumber").text(result.Quote.QuoteId);
                    $('#spnOrderType').text(result.Quote.OrderTypeTitle);
                    $("#quoteDate").text(result.Quote.QuoteDateFormated);
                    $("#contractTerm").text(result.Quote.ContractTerm);
                    if (result.Quote.ContractTerm == "1") {
                        $("#contractTerm").hide();
                        $("#contractTermLabel").text("Monthly");
                    }
                    $("#validUntil").text(result.Quote.ValidUntil);
                    $("#salesPerson").text(result.Quote.SalesPersonName);
                    $("#salesPersonId").text(result.Quote.SalesPersonId);
                    $("#salesPersonEmail").text(result.Quote.SalesPersonEmail);
                    $("#salesPersonCell").text(result.Quote.SalesPersonCellPhone);
                    $("#ZohoEntityId").text(result.Quote.ZohoEntityId);
                    $("#ZohoEntityType").text(result.Quote.ZohoEntityType);
                    $("#clientId").val(result.Quote.ClientId);

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

                    $("#txtNote").text(result.Quote.Note);
                    if (result.Quote.Note == "") {
                        $("#divNote").hide();
                    }

                    isDemo = result.Quote.IsDemo;
                    if (isDemo) {
                        $("#paymentMethodType option").not('[value=5]').remove();
                    }

                    ChangeShippingandHandlingServiceTitle(result.Quote.ShipToCountry);

                    $("#shippingAndHandling").text(result.Quote.ShippingAndHandling.toFixed(2));
                    $("#shippingAndHandlingType").val(result.Quote.ShippingAndHandlingType);
                    $("[name=SH][value=" + result.Quote.ShippingAndHandlingType + "]").attr("checked", true);
                    if (result.Quote.ShippingAndHandlingType == "Free") {
                        $("#shGround").hide();
                        $("#shSecondAir").hide();
                        $("#shNextAir").hide();

                        $("[name=SH][value=Ground]").hide();
                        $("[name=SH][value=2Day]").hide();
                        $("[name=SH][value=NextAir]").hide();
                    } else {
                        $("#shFree").hide();

                        $("[name=SH][value=Free]").hide();
                    }

                    $("#totalSalesTaxFees").text("$" + result.Quote.SalesTax.toFixed(2));

                    disableQuantitySync = result.Quote.IsQuantitySyncDisabled;

                    if (result.Quote.Purchased) {
                        $('.purchaseNow').attr('disabled', true);
                    }

                    if (result.Quote.IsAccepted) {
                        $("#chkAcceptance").attr("checked", "checked");
                        $("#chkAcceptance").attr("disabled", "disabled");

                        $("#lblAcceptanceName").show();
                        $("#lblAcceptanceName").text(result.Quote.AcceptanceName);
                        $("#lblAcceptanceDateTime").text(result.Quote.AcceptanceDateFormated);
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

    $("#totalShippingAndHandlingFees").text($("#shippingAndHandling").text());

    //$("#totalCharged").text((hardwareSummary + miscSummary + parseFloat($("#shippingAndHandling").text())).toFixed(2));
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

function CalculateShippingAndHandling(isAsync) {
    if (isAsync == undefined) {
        isAsync = true;
    }

    if ($("#shipCity").val().trim() == "" || $("#shipState").val() == null || $("#shipZip").val().trim() == "") {
        var message = "Please fill out fields bellow for calculating Shipping & Handling : \n";
        var isValid = true;

        if ($("#shipAddress1").val().trim() == "") {
            message += "* Ship to: Address1 is required.\n";
            isValid = false;
        }

        if ($("#shipCity").val().trim() == "") {
            message += "* Ship to: City is required.\n";
            isValid = false;
        }

        if ($("#shipState").val() == null) {
            message += "* Ship to: State is required.\n";
            isValid = false;
        }

        if ($("#shipZip").val().trim() == "") {
            message += "* Ship to: Zip is required.\n";
            isValid = false;
        }

        if (!isValid) {
            alert(message);
        }

        return;
    }

    if ($("[name=SH]:checked").val() == "Free") {
        CalculateSalesTax();
        $("#shippingAndHandling").text("0.00");
        $("#totalShippingAndHandlingFees").text("0.00");
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

        if ($("#shipCity").val() == "" || $("#shipState").val() == null || $("#shipZip").val() == "") {
            alert("Invalid Shipping Address.");

            HideLoadingMessage();
            ListViewUtility.CloseLoadingImage();
        }
        else {
            $.ajax({
                url: baseUrl + "/Quote/GetShippingAndHandling",
                type: 'POST',
                async: isAsync,
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
                        if (result.Error == "The remote name could not be resolved.") {
                            if (shippingErrorCountForNotResolve == 0) {
                                alert("Not able calculate shipping cost at this time.\n\nPlease submit your order and we'll contact you with shipping cost.");

                                setTimeout(function () {
                                    CalculateShippingAndHandling();
                                }, 2000);
                            }
                            shippingErrorCountForNotResolve++;
                        }
                        else if (result.Error == "Service is down. Please try later.") {
                            shippingErrorCount++;
                            //alert(result.Error + "\n\n" + "There is some error on calculating shipping and handling but you can continue. \nOur support team will contact for shipping and handling charge.");
                            alert("Sorry, we're not able to calculate the shipping at this moment.\n\nYou can continue the process and our support team will contact you for the shipping and handling charge that will be added with your total amount.");
                        } else {
                            //alert("Invalid Shipping Address.");
                            alert("Shipping and Handling:\n\n" + result.Error);
                        }
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
                    $("#totalShippingAndHandlingFees").text($("#shippingAndHandling").text());
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

function Validate() {
    var isValid = true;
    var message = "";

    if ($("#billCompanyName").val().trim() == "") {
        message += "* Bill to: Company Name is required.\n";
        isValid = false;
    }

    if ($("#billAddress1").val().trim() == "") {
        message += "* Bill to: Address1 is required.\n";
        isValid = false;
    }

    if ($("#billCity").val().trim() == "") {
        message += "* Bill to: City is required.\n";
        isValid = false;
    }

    if ($("#billState").val() == null) {
        message += "* Bill to: State is required.\n";
        isValid = false;
    }

    if ($("#billZip").val().trim() == "") {
        message += "* Bill to: Zip is required.\n";
        isValid = false;
    }

    if ($("#billContact").val().trim() == "") {
        message += "* Bill to: Contact is required.\n";
        isValid = false;
    }

    if ($("#billEmail").val().trim() == "") {
        message += "* Bill to: Email is required.\n";
        isValid = false;
    }

    if ($("#billPhone").val().trim() == "") {
        message += "* Bill to: Phone is required.\n";
        isValid = false;
    }

    message += "\n";

    if ($("#shipCompanyName").val().trim() == "") {
        message += "* Ship to: Company Name is required.\n";
        isValid = false;
    }

    if ($("#shipAddress1").val().trim() == "") {
        message += "* Ship to: Address1 is required.\n";
        isValid = false;
    }

    if (HasPOBox($("#shipAddress1").val())) {
        message += "* Cannot ship to P.O. Box via UPS.\n";
        isValid = false;
    }

    if (HasPOBox($("#shipAddress2").val())) {
        message += "* Cannot ship to P.O. Box via UPS.\n";
        isValid = false;
    }

    if ($("#shipCity").val().trim() == "") {
        message += "* Ship to: City is required.\n";
        isValid = false;
    }

    if ($("#shipState").val() == null) {
        message += "* Ship to: State is required.\n";
        isValid = false;
    }

    if ($("#shipZip").val().trim() == "") {
        message += "* Ship to: Zip is required.\n";
        isValid = false;
    }

    if ($("#shipContact").val().trim() == "") {
        message += "* Ship to: Contact is required.\n";
        isValid = false;
    }

    if ($("#shipEmail").val().trim() == "") {
        message += "* Ship to: Email is required.\n";
        isValid = false;
    }

    if ($("#shipPhone").val().trim() == "") {
        message += "* Ship to: Phone is required.\n";
        isValid = false;
    }

    if (!isValid) {
        alert(message);
    }

    return isValid;
}

var productList = null;
var discount10ProductItem;
var discount15ProductItem;
var discount20ProductItem;

function AddAutoDiscount(currentProductDiv, productQuantity, productPrice) {
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
        discount20ProductItem.Price = productPrice * 0.20;
        discount20ProductItem.Quantity = productQuantity;
        AddNewProduct(currentProductDiv, discount20ProductItem);
    } else if (productQuantity > 49 && productQuantity <= 99) {
        RemoveAutoDiscountItem(currentProductDiv, discount10ProductItem.ProductId);
        RemoveAutoDiscountItem(currentProductDiv, discount20ProductItem.ProductId);

        productType = "discount";
        discount15ProductItem.QuoteProductId = 0;
        discount15ProductItem.Price = productPrice * 0.15;
        discount15ProductItem.Quantity = productQuantity;
        AddNewProduct(currentProductDiv, discount15ProductItem);
    } else if (productQuantity > 9 && productQuantity <= 49) {
        RemoveAutoDiscountItem(currentProductDiv, discount15ProductItem.ProductId);
        RemoveAutoDiscountItem(currentProductDiv, discount20ProductItem.ProductId);

        productType = "discount";
        discount10ProductItem.QuoteProductId = 0;
        discount10ProductItem.Price = productPrice * 0.10;
        discount10ProductItem.Quantity = productQuantity;
        AddNewProduct(currentProductDiv, discount10ProductItem);
    } else {
        RemoveAutoDiscountItem(currentProductDiv, discount10ProductItem.ProductId);
        RemoveAutoDiscountItem(currentProductDiv, discount15ProductItem.ProductId);
        RemoveAutoDiscountItem(currentProductDiv, discount20ProductItem.ProductId);
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

function TytRound(value,places) {
    return +(Math.round(value + "e+" + places) + "e-" + places);
}
function CalculateTotalCharged() {
    $(".totalCharged").text((
        parseFloat($("#totalHardwareFees").text().replace("$", ""))
        + parseFloat($("#totalSalesTaxFees").text().replace("$", ""))
        + parseFloat($("#totalMiscFees").text().replace("$", ""))
        + parseFloat($("#shippingAndHandling").text())
    ).toFixed(2));
}

function LoadPaymentMethods() {
    $.ajax({
        url: baseUrl + "/Quote/GetQuotePaymentMethodList",
        type: "POST",
        dataType: "json",
        async: false,
        success: function (result) {
            quotePaymentMethodList = result
            //var sortedResult = result.sort().reverse();
            //console.log(sortedResult);
            //console.log(result);
            $.each(result, function (index, item) {
                if (item.QuotePaymentMethodId == 7) {
                    $("#paymentMethodType").append("<option selected ='selected' value='" + item.QuotePaymentMethodId + "'>" + item.PaymentMethod + "</option>");                    
                } else if (item.QuotePaymentMethodId == 8) {
                    return;
                }
                else {
                    $("#paymentMethodType").append("<option value='" + item.QuotePaymentMethodId + "'>" + item.PaymentMethod + "</option>");
                }
            });
            //$('#paymentMethodType').find(":selected").text("ACH Payment");
        }
    });
}

function ChangeShippingandHandlingServiceTitle(country) {
    if (country == "CA") {
        $("#shGround").text("Standard,");
        $("#shGround").attr("title", "Day Definite by Date Scheduled");

        $("#shSecondAir").text("Expedited,");
        $("#shSecondAir").attr("title", "2-5 Business Days");

        $("#shNextAir").text("Saver");
        $("#shNextAir").attr("title", "1-3 Business Days");
    }
    else {
        $("#shGround").text("Ground,");
        $("#shGround").attr("title", "1-5 Business Days");

        $("#shSecondAir").text("2nd day AIR,");
        $("#shSecondAir").attr("title", "2 Business Days");

        $("#shNextAir").text("Next day AIR");
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
                        var shipAddress1 = $('#shipAddress1').val();
                        var shipAddress2 = $('#shipAddress2').val();
                        var shipCity = $("#shipCity").val();
                        var shipState = $("#shipState").val();
                        var shipZip = $("#shipZip").val();

                        var shipAddress = shipAddress1 != '' ? shipAddress1 + (shipAddress2 != '' ? ' ' + shipAddress2 : '') : '';
                        if (shipAddress != '') {
                            shipAddress += ', ';
                        }

                        var html = '<h4 style="margin-top:0">You entered:</h4>';
                        html += '<div>';
                        html += '<label><input type="radio" name="upsva" style="margin-right: 10px;" class="ups-va" data-city="' + shipCity + '" data-state="' + shipState + '" data-zip="' + shipZip + '">' + shipAddress + shipCity + ', ' + shipState + ', ' + shipZip + '</label>';
                        html += '</div><br>';

                        html += '<h4>Suggested:</h4>';
                        $.each(result.AddressValidationResult, function (index, item) {
                            html += '<div>';
                            html += '<label><input ' + (index == 0 ? 'checked' : '') + ' type="radio" name="upsva" style="margin-right: 10px;" class="ups-va" data-city="' + item.Address.City + '" data-state="' + item.Address.StateProvinceCode + '" data-zip="' + item.Address.PostalCode + '">' + shipAddress + item.Address.City + ', ' + item.Address.StateProvinceCode + ', ' + item.Address.PostalCode + '</label>';
                            html += '</div>';
                        });

                        $('#divSuggestedAddress').html(html);

                        $('#suggestedAddressModal').modal('show');
                        if ($.browser.version == "10.0" || $.browser.version == "11.0") {
                            $('#suggestedAddressModal').addClass('in');
                            $('#suggestedAddressModal').css('display', 'block');
                        }
                    }
                } else {
                    alert('No similar address found!');//result.Response.ResponseStatusDescription
                }
            }
        });
    } else {
        InitSaveConfirm();
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
        if ($.browser.version == "10.0" || $.browser.version == "11.0") {
            $(".modal-backdrop.fade").remove();
        }

        InitSaveConfirm();
    }
}