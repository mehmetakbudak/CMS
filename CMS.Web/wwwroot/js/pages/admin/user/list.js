$(() => {
    $("#content").hide();

    var txtName = $("#txtName").kendoTextBox({
        placeholder: "Adı",
    }).data("kendoTextBox");

    var txtSurname = $("#txtSurname").kendoTextBox({
        placeholder: "Soyadı",
    }).data("kendoTextBox");

    var txtEmailAddress = $("#txtEmailAddress").kendoTextBox({
        placeholder: "Email Adresi",
    }).data("kendoTextBox");

    var cmbUserTypes = $("#cmbUserTypes").kendoDropDownList({
        optionLabel: "Kullanıcı Türü",
        dataTextField: "name",
        dataValueField: "id",
        autoBind: false,
        dataSource: {
            transport: {
                read: {
                    url: "/api/Admin/Lookup/UserTypes",
                }
            }
        }
    }).data("kendoDropDownList");

    var template = kendo.template($("#template").html());

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: function (options) {
                var value = options.data;
                var data = {
                    skip: value.skip,
                    take: value.take,
                    page: value.page,
                    pageSize: value.pageSize,
                    name: txtName.value(),
                    surname: txtSurname.value(),
                    emailAddress: txtEmailAddress.value(),
                    userType: cmbUserTypes.value()
                };

                $.ajax({
                    url: '/api/admin/user',
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'POST',
                    data: JSON.stringify(data),
                    success: function (result) {
                        options.success(result);
                    }
                });
            }
        },
        pageSize: 5,
        serverPaging: true,
        serverFiltering: true,
        schema: {
            data: "list",
            total: "total"
        },
        requestEnd: function () {
            $("#loading").hide();
            $("#content").show();
        },
        change: function () {
            $("#content-body").html(kendo.render(template, this.view()));
        }
    });

    $("#pager").kendoPager({
        dataSource: dataSource,
        responsive: false
    });

    dataSource.read();

    $("#btnSearch").click(() => {
        dataSource.query({ page: 1, pageSize: 5 });
        dataSource.read();
    });

    app.delete = function (id) {
        if (confirm("Silmek istediğinize emin misiniz?")) {

        };
    }
});