$(function () {
    $("#btnUpdateGroup").hide();
    $("#btnCancelGroup").hide();

    $("#btnCancelGroup").click(function () {
        cleanSecurityGroupForm();
    });

    $("#btnUpdateGroup").click(function () {
        saveSecurityGroup({
            SecurityGroupId: $("#txtSecurityGroupId").val(),
            GroupName: $("#txtName").val()
        });
    });

    $("#btnAddGroup").click(function () {
        saveSecurityGroup({
            SecurityGroupId: 0,
            GroupName: $("#txtName").val()
        });
    });

    genericKendoGrid({
        el: $("#divGroupGrid"),
        columns: [
            { field: 'SecurityGroupId', title: 'Group ID', width: '70px' },
            { field: 'GroupName', title: 'Group Name' },
            {
                title: 'Action',
                template: '<button class="btn btn-small btn-default" onClick="editSecurityGroup(\'#= SecurityGroupId #\', \'#= GroupName #\')">Edit</button> '
                    + ' <button class="btn btn-small btn-warning" onClick="deleteSecurityGroup(\'#= SecurityGroupId #\', \'#= GroupName #\')">Delete</button>'
                    + ' <button class="btn btn-small btn-info" onClick="loadSalesPersonList(\'#= SecurityGroupId #\', \'#= GroupName #\')">Show Sales Person</button>',
                width: '230px'
            }
        ],
        url: baseUrl + "/SecurityGroup/GetSecurityGroupList",
        height: 500
    });
});

function reloadSecurityGroupGridData() {
    $("#divGroupGrid").data("kendoGrid").dataSource.read();
}

function cleanSecurityGroupForm() {
    $("#txtSecurityGroupId").val(0);
    $("#txtName").val('');

    $("#btnAddGroup").show();
    $("#btnUpdateGroup").hide();
    $("#btnCancelGroup").hide();
}

function editSecurityGroup(securityGroupId, groupName) {
    $("#txtSecurityGroupId").val(securityGroupId);
    $("#txtName").val(groupName);

    $("#btnAddGroup").hide();
    $("#btnUpdateGroup").show();
    $("#btnCancelGroup").show();
}

function validate(model) {
    if (model.groupName == '') {
        showMessage('Please provide group name.');

        return false;
    }

    return true;
}

function saveSecurityGroup(model) {
    if (validate(model)) {
        $.ajax({
            type: 'POST',
            url: baseUrl + '/SecurityGroup/Save',
            data: model,
            dataType: 'json',
            success: function (response) {
                if (response.Message == 'Success') {
                    cleanSecurityGroupForm();
                    reloadSecurityGroupGridData();

                    showMessage('Save successfully.');
                } else {
                    showMessage('Save failed.');
                }
            }
        });
    }
}

function deleteSecurityGroup(securityGroupId, groupName) {
    if (confirm('Do you want to delete \'' + groupName + '\' ?')) {
        var model = {
            SecurityGroupId: securityGroupId
        };

        $.ajax({
            type: 'POST',
            url: baseUrl + '/SecurityGroup/Delete',
            data: model,
            dataType: 'json',
            success: function (response) {
                if (response.Message == 'Success') {
                    reloadSecurityGroupGridData();
                    removeSalesPersonList();

                    showMessage('Delete successfully.');
                } else {
                    showMessage('Delete failed.');
                }
            }
        });
    }
}

function loadSalesPersonList(securityGroupId, groupName) {
    $("#securityGroupTitle").text(groupName);
    $("#hidSalesPersonGroupId").val(securityGroupId);

    genericKendoGrid({
        el: $("#divSalesPersonGrid"),
        columns: [
            { field: 'EmployeeId', title: 'EmployeeId', width: '80px' },
            { field: 'FirstName', title: 'First Name' },
            { field: 'LastName', title: 'Last Name' },
            { field: 'WebLogin', title: 'Web Login' },
            { template: kendo.template($('#sales-person-template').html()), width: '80px' }
        ],
        parameterData: { SecurityGroupId: securityGroupId },
        url: baseUrl + "/SecurityGroup/GetSalesPersonList",
        height: 500
    });
}

function reloadSalesPersonList() {
    $("#divSalesPersonGrid").data('kendoGrid').dataSource.read();
}

function removeSalesPersonList() {
    $("#securityGroupTitle").text('');
    $("#hidSalesPersonGroupId").val(0);

    $("#divSalesPersonGrid").empty();
    $("#divSalesPersonGrid").removeClass();
}

function setSalesPerson(sender) {
    sender = $(sender);

    var isDelete = false;
    var url = baseUrl + "/SecurityGroup/SaveSalesPerson";

    if (!sender.is(":checked")) {
        isDelete = true;
        url = baseUrl + "/SecurityGroup/DeleteSalesPerson";
    }

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({ EmployeeId: sender.val(), SecurityGroupId: $('#hidSalesPersonGroupId').val() }),
        success: function (response) {
            if (response.Message == 'Failed') {
                showMessage('Sales person assign failed.');
            }

            reloadSalesPersonList();
        }
    });
}

function showMessage(message) {
    alert(message);
}

function genericKendoGrid(settings) {
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: 'POST',
                dataType: 'json',
                data: settings.parameterData != undefined ? settings.parameterData : {},
                url: function () {
                    return settings.url;
                },
                success: function () {
                    if (settings.onDataReceived != undefined && $.isFunction(settings.onDataReceived)) {
                        settings.onDataReceived();
                    }
                }
            }
        }
    });

    $(settings.el).kendoGrid({
        dataSource: dataSource,
        columns: settings.columns,
        dataBound: function () {
            if (settings.onDataBound != undefined && $.isFunction(settings.onDataBound)) {
                settings.onDataBound();
            }
        },
        sortable: settings.sortable == undefined ? true : settings.sortable,
        scrollable: settings.scrollable == undefined ? true : settings.scrollable,
        height: settings.height == undefined ? 685 : settings.height
    });
}