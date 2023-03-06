$(() => {
    $("#content").hide();

    var template = kendo.template($("#template").html());

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/api/admin/contact",
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

    app.delete = function (id) {
        if (confirm("Silmek istediğinize emin misiniz?")) {

        };
    }
});