$(() => {
    $("#content").hide();

    var template = kendo.template($("#template").html());

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: function (options) {
                var data = {
                    taskCategoryId: null,
                    title: null,
                    assignUserId: null,
                    taskStatusId: null,
                    startDate: null,
                    endDate: null,
                    isActive: null
                };
                $.ajax({
                    url: "/api/admin/task/getbyfilter",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    success: function (result) {
                        options.success(result);
                    }
                });

            },
        },
        pageSize: 5,
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

    app.delete = function (id) {
        if (confirm("Silmek istediğinize emin misiniz?")) {
            $.ajax({
                type: "DELETE",
                url: `/api/admin/task/${id}`,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done((res) => {
                app.successNotification("İşlem Başarılı!", res.message);
                dataSource.read();
            }).fail((err) => {
                app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        };
    }
});