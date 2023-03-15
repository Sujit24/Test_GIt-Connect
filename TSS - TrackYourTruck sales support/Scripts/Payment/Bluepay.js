$(function () {
    $("#cardExpireMonth").val($("#cardExpireMonth").attr("selected-value"));
    $("#cardExpireYear").val($("#cardExpireYear").attr("selected-value"));

    $("#process").click(function () {
        ProcessTransaction();
    });


    LoadSalesTaxList();



    ////////////////----ACH----/////////////////////////////////////////////////////////////
    if ($('#achstate').length > 0) {

        $("#processACH").click(function () {
            ACHProcessTransaction();
        });
        $("#achcountry").val($("#hACHCountry").val());
        LoadStateDDL($("#achcountry").val(), $('#achstate'));

        $('#achstate').val($('#hStateVal').val());

      
        

        $("#achcountry").change(function () {
            LoadStateDDL($(this).val(), $('#achstate'));
        });

        $("#routingNum").on("keyup", function () {
            confirmAccountAndRountNum("routingNum");
        });
    }
    else {
        /// not ach
        $("#country").val($("#hCountry").val());
        LoadStateDDL($("#country").val(), $('#state'));

        $('#state').val($('#hState').val());

        $("#country").change(function () {
            LoadStateDDL($(this).val(), $('#state'));
        });

    }

});

function LoadStateDDL(country, ddlState) {
    ddlState.empty();

    $.each(salesTaxList, function (index, item) {
        if (item.Country == country) {
            ddlState.append("<option value='" + item.StateShortName + "'>" + item.StateFullName + "</option>");
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


function verify() {
    var themessage = "You are required to complete the following fields: <br />";

    if (document.forms.mainform.AMOUNT.value == "") {
        themessage = themessage + " - Amount <br />";
    }

    if (document.forms.mainform.CC_NUM.value == "") {
        themessage = themessage + " - Credit Card Number <br />";
    }

    if (document.forms.mainform.CC_EXPIRES_MONTH.value == "") {
        themessage = themessage + " - Card Expiration Month <br />";
    }

    if (document.forms.mainform.CC_EXPIRES_YEAR.value == "") {
        themessage = themessage + " - Card Expiration Year <br />";
    }
    if (document.forms.mainform.CVCCVV2.value == "") {
        themessage = themessage + " - CVV2 <br />";
    }

    if (document.forms.mainform.NAME.value == "") {
        themessage = themessage + " - Name On Card <br />";
    }

    /*if (document.forms.mainform.FIRST_NAME.value == "") {
        themessage = themessage + " - First Name on Card <br />";
    }

    if (document.forms.mainform.LAST_NAME.value == "") {
        themessage = themessage + " - Last Name on Card <br />";
    }*/

    if (document.forms.mainform.ADDR1.value == "") {
        themessage = themessage + " - Billing Address <br />";
    }

    if (document.forms.mainform.CITY.value == "") {
        themessage = themessage + " - City <br />";
    }

    if (document.forms.mainform.STATE.value == "") {
        themessage = themessage + " - State <br />";
    }

    if (document.forms.mainform.ZIPCODE.value == "") {
        themessage = themessage + " - Zip Code <br />";
    }

    if (document.forms.mainform.PHONE.value == "") {
        themessage = themessage + " - Phone Number <br />";
    }

    if (document.forms.mainform.EMAIL.value == "") {
        themessage = themessage + " - Email Address";
    }


    //alert if fields are empty and cancel form submit
    if (themessage != "You are required to complete the following fields: <br />") {
        //alert(themessage);
        $("#messageBody").html(themessage);
        $('#messageModal').modal('show');
        return false;
    }
    else {
        return true;
    }
}

function ProcessTransaction() {
    if (verify()) {
        $.isLoading({ text: "<img src='" + baseUrl + "/Content/images/processing.gif' />" });

        var bluePayTransaction = {
            OrderId: $("#orderId").val().trim(),
            CardNumber: $("#cardNumber").val().trim(),
            CVV2: $("#cvv2").val().trim(),
            CardExpireYear: $("#cardExpireYear").val(),
            CardExpireMonth: $("#cardExpireMonth").val(),
            Amount: $("#amount").val().trim(),
            CardHolderName: $("#cardHolderName").val().trim(),
            /*CardHolderFirstName: $("#cardHolderFirstName").val().trim(),
            CardHolderLastName: $("#cardHolderLastName").val().trim(),*/
            Address: $("#address").val().trim(),
            City: $("#city").val(),
            State: $("#state").val(),
            ZipCode: $("#zipCode").val().trim(),
            Country: $("#country").val().trim(),
            CompanyName: $("#companyName").val().trim(),
            Email: $("#email").val().trim(),
            Phone: $("#phone").val().trim()
        };

        $.ajax({
            url: baseUrl + "/Payment/Process",
            type: 'POST',
            data: JSON.stringify(bluePayTransaction),
            dataType: 'json',
            contentType: "application/json",
            success: function (response) {
                if ($(".isloading-overlay").length > 0) {
                    $.isLoading("hide");
                }

                if (response.Status == 0) {
                    // Error
                    //alert(response.Message);
                    $("#messageBody").html(response.Message);
                    $('#messageModal').modal('show');
                } else {
                    location.href = baseUrl + "/Payment/Processed"
                }
            }
        });
    }
}



////------------------ ACH-------------------------////


function verifyACH() {
    var themessage = "You are required to complete the following fields: <br />";

    if (document.forms.mainform.AMOUNT.value == "") {
        themessage = themessage + " - Amount <br />";
    }

    if (document.forms.mainform.ROUTING_NO.value == "") {
        themessage = themessage + " - Routing Number <br />";
    }

    if (document.forms.mainform.ACCOUNT_NO.value == "") {
        themessage = themessage + " - Account Number <br />";
    }

    if (document.forms.mainform.NAME.value == "") {
        themessage = themessage + " - Name <br />";
    }

    /*if (document.forms.mainform.FIRST_NAME.value == "") {
        themessage = themessage + " - First Name on Card <br />";
    }

    if (document.forms.mainform.LAST_NAME.value == "") {
        themessage = themessage + " - Last Name on Card <br />";
    }*/

    if (document.forms.mainform.ADDR1.value == "") {
        themessage = themessage + " - Billing Address <br />";
    }

    if (document.forms.mainform.CITY.value == "") {
        themessage = themessage + " - City <br />";
    }

    if (document.forms.mainform.STATE.value == "") {
        themessage = themessage + " - State <br />";
    }

    if (document.forms.mainform.ZIPCODE.value == "") {
        themessage = themessage + " - Zip Code <br />";
    }

    if (document.forms.mainform.PHONE.value == "") {
        themessage = themessage + " - Phone Number <br />";
    }

    if (document.forms.mainform.EMAIL.value == "") {
        themessage = themessage + " - Email Address";
    }


    //alert if fields are empty and cancel form submit
    if (themessage != "You are required to complete the following fields: <br />") {
        //alert(themessage);
        $("#messageBody").html(themessage);
        $('#messageModal').modal('show');
        return false;
    }
    else {
        return true;
    }
}




function ACHProcessTransaction() {
    if (verifyACH()) {
        $.isLoading({ text: "<img src='" + baseUrl + "/Content/images/processing.gif' />" });

        var accType = "";
        if ($("#checkmethod").is(":checked")) {
            accType = "C";
        } else {
            accType = "S";
        }

        var bluePayTransaction = {
            OrderId: $("#orderId").val().trim(),
            RoutingNum: $("#routingNum").val().trim(),
            AccountNum: $("#accountNum").val().trim(),
            AccountType: accType,
            Amount: $("#amount").val().trim(),
            CardHolderName: $("#cardHolderName").val().trim(),
            /*CardHolderFirstName: $("#cardHolderFirstName").val().trim(),
            CardHolderLastName: $("#cardHolderLastName").val().trim(),*/
            Address: $("#address").val().trim(),
            City: $("#city").val(),
            State: $("#achstate").val(),
            ZipCode: $("#zipCode").val().trim(),
            Country: $("#achcountry").val().trim(),
            CompanyName: $("#companyName").val().trim(),
            Email: $("#email").val().trim(),
            Phone: $("#phone").val().trim()
        };

        console.log(bluePayTransaction);
        $.ajax({
            url: baseUrl + "/Payment/achprocess",
            type: 'POST',
            data: JSON.stringify(bluePayTransaction),
            dataType: 'json',
            contentType: "application/json",
            success: function (response) {
                if ($(".isloading-overlay").length > 0) {
                    $.isLoading("hide");
                }

                if (response.Status == 0) {
                    // Error
                    //alert(response.Message);
                    $("#messageBody").html(response.Message);
                    $('#messageModal').modal('show');
                } else {
                    location.href = baseUrl + "/Payment/Processed"
                }
            }
        });
    }
}


function confirmAccountAndRountNum(inputBox) {
    if (inputBox == "routingNum") {
        $("#routingNum").on("keyup", function () {
            if ($("#routingNum").val() === $("#confirmRoutingNum").val()) {
                $("#confirmRoutingNum").css("border", "1px solid green");
                $("#crtick").html("&#10004");
                $("#crmessage").text("");
            } else {
                $("#crtick").html("");
                $("#crmessage").text("Routing number not matched");
                $("#confirmRoutingNum").css("border", "1px solid red");
            }
        })
        $("#confirmRoutingNum").on("keyup", function () {
            if ($("#routingNum").val() === $("#confirmRoutingNum").val()) {
                $("#confirmRoutingNum").css("border", "1px solid green");
                $("#crtick").html("&#10004");
                $("#crmessage").text("");
            } else {
                $("#crtick").html("");
                $("#crmessage").text("Routing number not matched");
                $("#confirmRoutingNum").css("border", "1px solid red");
            }
        })

    } else {
        $("#accountNum").on("keyup", function () {
            if ($("#accountNum").val() === $("#confirmAccountNum").val()) {
                $("#catick").html("&#10004");
                $("#camessage").text("");
                $("#confirmAccountNum").css("border", "1px solid green");
            } else {
                $("#catick").html("");
                $("#camessage").text("Account number not matched");
                $("#confirmAccountNum").css("border", "1px solid red");
            }
        })
        $("#confirmAccountNum").on("keyup", function () {
            if ($("#accountNum").val() === $("#confirmAccountNum").val()) {
                $("#catick").html("&#10004");
                $("#camessage").text("");
                $("#confirmAccountNum").css("border", "1px solid green");
            } else {
                $("#catick").html("");
                $("#camessage").text("Account number not matched");
                $("#confirmAccountNum").css("border", "1px solid red");
            }
        })
    }
}

