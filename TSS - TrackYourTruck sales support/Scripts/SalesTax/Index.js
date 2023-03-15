$.fn.navPills = function () {
    var _element = $(this);
    var _class = {};

    if (!_element.hasClass("nav nav-pills")) {
        _element.css("margin-bottom", "0px");
        _element.addClass("nav nav-pills");
    }

    _class = {
        add: function (value, attributes) {
            var attributesAndValues = "";
            for (var item in attributes) {

                attributesAndValues = attributesAndValues + item + "=" + attributes[item] + " ";
            }
            _element.append('<li class="active" ><a href="#" ' + attributesAndValues + '>' + value + '<i style="color:white;margin-top:-5px;" class="icon-remove-category glyphicon glyphicon-remove-circle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;X</i></a> </li>');
        }
    };

    return _class;
};

var columns = '[';
//columns += '{"template": "\u003cinput id=\u0027delete${ SalesTaxId }\u0027 value=\u0027${ SalesTaxId }\u0027 type=\u0027checkbox\u0027 /\u003e",width: 30},';
columns += '{"title":"Country", "width":"10%", "field":"Country" },';
columns += '{"title":"State Full Name", "width":"40%", "field":"StateFullName" },';
columns += '{"title":"State Short Name", "width":"30%", "field":"StateShortName" },';
columns += '{"title":"Tax Rate", "width":"20%", "field":"TaxRate", "format": "{0:n2} %", "template": kendo.template($("#taxrate-template").html()) },';
columns += ']';

$(function () {
    createKendoGridResponseDateFn = function (data) {
        $.each(data, function (index, item) {
            $("#state").append("<option value='" + item.SalesTaxId + "'>" + item.StateFullName + "</option>");
        });
    };
    createKendoGrid(columns, baseUrl + "/SalesTax/GetSalesTaxList", false, 10, null, null, { DisableCheckAll: true });

    $("#btnAddState").click(function () {
        if ($("#ulNavsStates [id=" + $("#state").val() + "]").length == 0) {
            var stateNavs = $("#ulNavsStates").navPills();
            stateNavs.add($("#state option:selected").text(), { id: $("#state").val() });

            $(".icon-remove-category").unbind();
            $(".icon-remove-category").click(function (e) {
                e.preventDefault();
                $(this).parent().parent().remove();
            });
        }
    });

    $("#btnUpdateTaxRate").click(function () {
        if (validation()) {
            var postData = [];
            var taxRate = $("#txtTaxRate").val();

            $.each($("#ulNavsStates a"), function (index, item) {
                var SalesTaxId = $(item).attr("id");
                postData.push({ SalesTaxId: SalesTaxId, TaxRate: taxRate });
            });

            $.ajax({
                url: baseUrl + "/SalesTax/Save",
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify({ model: postData }),
                success: function (result) {
                    if (result.Message == "Successfully Saved.") {
                        createKendoGrid(columns, baseUrl + "/SalesTax/GetSalesTaxList", false, 10, null, null, { DisableCheckAll: true });
                        alert("Successfully Updated.");

                        $("#ulNavsStates").empty();
                        $("#txtTaxRate").val("0.00");
                    }
                    else {
                        alert("Update failed.");
                    }
                }
            });
        }
    });

    LoadSalesTaxList();
    LoadSalesStateDDL('US', $('#state'));

    $("#country").change(function () {
        LoadSalesStateDDL($(this).val(), $('#state'));
    });
});

function validation() {
    var message = "";
    var isValid = true;

    if ($("#ulNavsStates a").length == 0) {
        message += "* Please at least add one State.\n";
        isValid = false;
    }

    if ($("#txtTaxRate").val().trim() == "") {
        message += "* Please enter Tax Rate.\n";
        isValid = false;
    }

    if (!isNumber($("#txtTaxRate").val())) {
        message += "* Invalid Tax Rate.\n";
        $("#txtTaxRate").focus();
        isValid = false;
    }

    if (!isValid) {
        alert(message);
    }

    return isValid;
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
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

function LoadSalesStateDDL(country, ddlState) {
    ddlState.empty();

    $.each(salesTaxList, function (index, item) {
        if (item.Country == country) {
            ddlState.append("<option value='" + item.SalesTaxId + "'>" + item.StateFullName + "</option>");
        }
    });
}