$(() => {
    $("#content").hide();
    var template = kendo.template($("#template").html());

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/api/team",
                dataType: "json"
            }
        },
        change: function () {
            $("#teams").html(kendo.render(template, this.view()));
        },
        requestEnd: function () {
            $("#loading").hide();
            $("#content").show();
        }
    });

    dataSource.read();
});