$(function () {
    $("#btnUpdateGroup").hide();
    $("#btnCancelGroup").hide();

    $("#btnCancelGroup").click(function () {
        cleanTemplateGroupForm();
    });

    $("#btnUpdateGroup").click(function () {
        saveTemplateGroup({
            TemplateGroupId: $("#txtTemplateGroupId").val(),
            GroupName: $("#txtName").val()
        });
    });

    $("#btnAddGroup").click(function () {
        saveTemplateGroup({
            TemplateGroupId: 0,
            GroupName: $("#txtName").val()
        });
    });

    genericKendoGrid({
        el: $("#divGroupGrid"),
        columns: [
            { field: 'TemplateGroupId', title: 'Group ID', width: '70px' },
            { field: 'GroupName', title: 'Group Name' },
            {
                title: 'Action',
                template: '<button class="btn btn-small btn-default" onClick="editTemplateGroup(\'#= TemplateGroupId #\', \'#= GroupName #\')">Edit</button> '
                    + ' <button class="btn btn-small btn-warning" onClick="deleteTemplateGroup(\'#= TemplateGroupId #\', \'#= GroupName #\')">Delete</button>',
                width: '130px'
            }
        ],
        url: baseUrl + "/TemplateGroup/GetTemplateGroupList",
        height: 500
    });
});

function reloadTemplateGroupGridData() {
    $("#divGroupGrid").data("kendoGrid").dataSource.read();
}

function cleanTemplateGroupForm() {
    $("#txtTemplateGroupId").val(0);
    $("#txtName").val('');

    $("#btnAddGroup").show();
    $("#btnUpdateGroup").hide();
    $("#btnCancelGroup").hide();
}

function editTemplateGroup(templateGroupId, groupName) {
    $("#txtTemplateGroupId").val(templateGroupId);
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

function saveTemplateGroup(model) {
    if (validate(model)) {
        $.ajax({
            type: 'POST',
            url: baseUrl + '/TemplateGroup/Save',
            data: model,
            dataType: 'json',
            success: function (response) {
                if (response.Message == 'Success') {
                    cleanTemplateGroupForm();
                    reloadTemplateGroupGridData();

                    showMessage('Save successfully.');
                } else {
                    showMessage('Save failed.');
                }
            }
        });
    }
}

function deleteTemplateGroup(templateGroupId, groupName) {
    if (confirm('Do you want to delete \'' + groupName + '\' ?')) {
        var model = {
            TemplateGroupId: templateGroupId
        };

        $.ajax({
            type: 'POST',
            url: baseUrl + '/TemplateGroup/Delete',
            data: model,
            dataType: 'json',
            success: function (response) {
                if (response.Message == 'Success') {
                    reloadTemplateGroupGridData();
                    removeSalesPersonList();

                    showMessage('Delete successfully.');
                } else {
                    showMessage('Delete failed.');
                }
            }
        });
    }
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