$(() => {
    var id = $("#txtId").val();

    $("#content").hide();

    var txtTitle = $("#txtTitle").kendoTextBox().data("kendoTextBox");

    var cmbTaskCategories = $("#cmbTaskCategories").kendoDropDownList({
        optionLabel: "Kategori seçiniz.",
        dataTextField: "name",
        dataValueField: "id",
        autoBind: false,
        dataSource: {
            transport: {
                read: {
                    url: "/api/Admin/Lookup/TaskCategories",
                }
            }
        },
        change: function (e) {
            if (!this.value()) {
                cmbTaskStatuses.setDataSource([]);
            }
        }
    }).data("kendoDropDownList");

    var cmbTaskStatuses = $("#cmbTaskStatuses").kendoDropDownList({
        optionLabel: "Durum seçiniz.",
        dataTextField: "name",
        dataValueField: "id",
        autoBind: false,
        dataSource: {
            transport: {
                read: function (options) {
                    var taskId = cmbTaskCategories.value();
                    if (taskId) {
                        $.get(`/api/Admin/Lookup/TaskStatuses/${taskId}`).then((res) => {
                            options.success(res);
                        });
                    }
                }
            }
        }
    }).data("kendoDropDownList");

    var cmbAssignUsers = $("#cmbAssignUsers").kendoDropDownList({
        optionLabel: "Kullanıcı seçiniz.",
        dataTextField: "name",
        dataValueField: "id",
        autoBind: false,
        dataSource: {
            transport: {
                read: {
                    url: "/api/Admin/Lookup/Users",
                }
            }
        }
    }).data("kendoDropDownList");

    $("#txtDescription").summernote();

    var validator = $("#taskForm").kendoValidator().data("kendoValidator");

    if (id != 0) {
        $.get(`/api/admin/task/${id}`).then((res) => {
            txtTitle.value(res.title);
            cmbTaskCategories.value(res.taskCategoryId);
            cmbTaskStatuses.value(res.taskStatusId);
            cmbAssignUsers.value(res.assignUserId);
            $("#chkIsActive").attr("checked", res.isActive);
            $("#txtDescription").summernote('code', res.description);
            $("#loading").hide();
            $("#content").show();
        });
    } else {
        $("#loading").hide();
        $("#content").show();
        $("#chkIsActive").attr("checked", true);
    }

    $("#btnSave").click((e) => {
        e.preventDefault();
        if (validator.validate()) {
            var data = {
                id: id,
                title: txtTitle.value(),
                taskCategoryId: cmbTaskCategories.value(),
                taskStatusId: cmbTaskStatuses.value(),
                assignUserId: cmbAssignUsers.value(),
                isActive: $("#chkIsActive").prop('checked'),
                description: $("#txtDescription").summernote('code')
            };
            var requestType = "POST";
            if (id != 0) {
                requestType = "PUT";
            }
            $.ajax({
                type: requestType,
                url: "/api/admin/task",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done((res) => {
                app.successNotification("İşlem Başarılı!", res.message);
                setTimeout(() => {
                    window.location.href = "/admin/task";
                }, 1000);
            }).fail((err) => {
                app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });
});