$(() => {
    $("#content").hide();

    var template = kendo.template($("#template").html());

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "api/comment/user-comments",
                dataType: "json"
            }
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

    app.commentDelete = function (id) {
        if (confirm("Silmek istediğinize emin misiniz?")) {
            $.ajax({
                type: "DELETE",
                url: `/api/comment/${id}`,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done((res) => {
                app.successNotification("İşlem Başarılı!", res.message);
                $("#formAddComment").trigger("reset");
                dataSource.read();
            }).fail((err) => {
                app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        };
    }
});