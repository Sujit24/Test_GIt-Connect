var salesPersonList = [];
var maxProductCount = 1;

$(function () {
    InitProductContainer();

    if ($("#quoteOrderId").val() != "") {
        Edit($("#quoteOrderId").val());
    }
});

function AddNewProduct(currentProductDiv, newRecord) {
    var newRow = "<tr class='productDetail' deleted='0'>";
    if (((currentProductDiv.find(".productTable").attr("product-category") == "Hardware" && productType == "discount")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Monthly")
        || (currentProductDiv.find(".productTable").attr("product-category") == "Misc Fee"))) {
        newRow += "<td style='text-align: center;padding-bottom: 5px;'><input class='quote-product-id' type='hidden' value='" + newRecord.QuoteProductId + "'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><input readonly='readonly' class='product-quantity' type='text' style='width: 20px; text-align: center' value='" + newRecord.Quantity + "'></td>";
    }
    else {
        newRow += "<td style='text-align: center;padding-bottom: 5px;'><input class='quote-product-id' type='hidden' value='" + newRecord.QuoteProductId + "'><span class='product-id' product-id='" + newRecord.ProductId + "' style='display:none'></span><span ></span><input readonly='readonly' class='product-quantity' type='text' style='width: 20px; text-align: center' value='" + newRecord.Quantity + "'></td>";
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


var currentProductIndex = 1;
var currentPackageIndex = 2;
var activeProductIndex;
function InitProductContainer() {
    AddMoreProduct1();
}

function AddMoreProduct1() {
    var templateContent = $("#productTemplate").html();
    var template = kendo.template(templateContent);

    var data = [{ "Title": "Hardware", "ProductIndex": '#' + currentProductIndex}];
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

function Edit(quoteId) {
    try {
        $.ajax({
            url: baseUrl + "/SalesOrder/UserViewGetOrder",
            type: 'POST',
            async: false,
            data: { id: quoteId },
            dataType: 'json',
            success: function (result) {
                if (result.QuoteOrder != null) {

                    $("#orderNumber").text(result.QuoteOrder.QuoteOrderId);
                    $("#refQuoteNumber").text(result.QuoteOrder.QuoteId);
                    $('#spnOrderType').text(result.QuoteOrder.OrderTypeTitle);
                    $("#quoteDate").text(result.QuoteOrder.PurchaseDateFormated);
                    $("#contractTerm").text(result.QuoteOrder.ContractTerm);
                    if (result.QuoteOrder.ContractTerm == "1") {
                        $("#contractTerm").hide();
                        $("#contractTermLabel").text("Monthly");
                    }
                    $("#validUntil").text(result.QuoteOrder.ValidUntil);
                    $("#salesPerson").text(result.QuoteOrder.SalesPersonName);
                    $("#salesPersonId").text(result.QuoteOrder.SalesPersonId);
                    $("#salesPersonEmail").text(result.QuoteOrder.SalesPersonEmail);
                    $("#salesPersonCell").text(result.QuoteOrder.SalesPersonCellPhone);
                    $("#ZohoEntityId").text(result.QuoteOrder.ZohoEntityId);
                    $("#ZohoEntityType").text(result.QuoteOrder.ZohoEntityType);
                    $("#clientId").val(result.QuoteOrder.ClientId);
                    $("#netTrackClient").text(result.QuoteOrder.ClientName);

                    if (result.QuoteOrder.NettrackClientStatusId != 0) {
                        $('#nettrackStatus').text(result.QuoteOrder.NettrackStatus);
                        $('#rowNettrackStatus').show();
                    }

                    $("#billCompanyName").text(result.QuoteOrder.BillToCompanyName);
                    $("#billAddress1").text(result.QuoteOrder.BillToAddress1);
                    $("#billAddress2").text(result.QuoteOrder.BillToAddress2);
                    $("#billCityStateZip").text(result.QuoteOrder.BillToCityStateZip);
                    if (result.QuoteOrder.BillToCountry == "US") {
                        $("#billCountry").text("United States");
                    } else if (result.QuoteOrder.BillToCountry == "CA") {
                        $("#billCountry").text("Canada");
                    } else if (result.QuoteOrder.BillToCountry == "MX") {
                        $("#billCountry").text("Mexico");
                    }
                    //$("#billCountry").text(result.QuoteOrder.BillToCountry == "US" ? "United States" : "Canada");
                    $("#billContact").text(result.QuoteOrder.BillToBillingContact);
                    $("#billEmail").text(result.QuoteOrder.BillToBillingEmail);
                    $("#billPhone").text(result.QuoteOrder.BillToPhone);

                    $("#shipCompanyName").text(result.QuoteOrder.ShipToCompanyName);
                    $("#shipAddress1").text(result.QuoteOrder.ShipToAddress1);
                    $("#shipAddress2").text(result.QuoteOrder.ShipToAddress2);
                    $("#shipCityStateZip").text(result.QuoteOrder.ShipToCityStateZip);
                    if (result.QuoteOrder.ShipToCountry == "US") {
                        $("#shipCountry").text("United States");
                    } else if (result.QuoteOrder.ShipToCountry == "CA") {
                        $("#shipCountry").text("Canada");
                    } else if (result.QuoteOrder.ShipToCountry == "MX") {
                        $("#shipCountry").text("Mexico");
                    }
                    //$("#shipCountry").text(result.QuoteOrder.ShipToCountry == "US" ? "United States" : "Canada");
                    $("#shipContact").text(result.QuoteOrder.ShipToBillingContact);
                    $("#shipEmail").text(result.QuoteOrder.ShipToBillingEmail);
                    $("#shipPhone").text(result.QuoteOrder.ShipToPhone);

                    $("#txtAdminNote").text(result.QuoteOrder.Note);
                    if (result.QuoteOrder.Note == "") {
                        $("#divAdminNote").hide();
                    }

                    if (result.QuoteOrder.ShipToCountry == "US") {
                        if (result.QuoteOrder.ShippingAndHandlingType == "Ground") {
                            $("#sShippingMethod").text("UPS Ground");
                        } else if (result.QuoteOrder.ShippingAndHandlingType == "2Day") {
                            $("#sShippingMethod").text("2nd day AIR");
                        } else if (result.QuoteOrder.ShippingAndHandlingType == "NextAir") {
                            $("#sShippingMethod").text("Next day AIR");
                        }
                    } else {
                        if (result.QuoteOrder.ShippingAndHandlingType == "Ground") {
                            $("#sShippingMethod").text("UPS Standard");
                        } else if (result.QuoteOrder.ShippingAndHandlingType == "2Day") {
                            $("#sShippingMethod").text("UPS Expedited");
                        } else if (result.QuoteOrder.ShippingAndHandlingType == "NextAir") {
                            $("#sShippingMethod").text("UPS Saver");
                        }
                    }

                    if (result.QuoteOrder.ShippingAndHandlingType == "Free") {
                        $("#sShippingMethod").text("Free");
                    }

                    $("#shippingAndHandling").text(result.QuoteOrder.ShippingAndHandling.toFixed(2));

                    $("#totalSalesTaxFees").text("$" + result.QuoteOrder.SalesTax.toFixed(2));

                    $("#orderStatus").text(result.QuoteOrder.StatusTitle);

                    $("#paymentType").text(result.QuoteOrder.PaymentMethod);

                    if (result.QuoteOrder.PaymentMethodComment != "") {
                        $("#divNote").show();
                        $("#txtNote").text(result.QuoteOrder.PaymentMethodComment);
                    } else {
						$("#txtNote").text("");
                        $("#divNote").hide();
                    }

                    if (result.QuoteOrder.IsAccepted) {
                        $("#chkAcceptance").attr("checked", "checked");
                        $("#chkAcceptance").attr("disabled", "disabled");

                        $("#lblAcceptanceName").show();
                        $("#lblAcceptanceName").text(result.QuoteOrder.AcceptanceName);
                        $("#lblAcceptanceDateTime").text(result.QuoteOrder.AcceptanceDateFormated);
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

    $("#totalShippingAndHandlingFees").text($("#shippingAndHandling").text());
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

function CalculateTotalCharged() {
    $("#totalCharged").text((
        parseFloat($("#totalHardwareFees").text().replace("$", ""))
        + parseFloat($("#totalSalesTaxFees").text().replace("$", ""))
        + parseFloat($("#totalMiscFees").text().replace("$", ""))
        + parseFloat($("#shippingAndHandling").text())
    ).toFixed(2));
}