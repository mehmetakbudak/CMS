$(() => {

    var template = kendo.template($("#template").html());
    $("#content").hide();

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/api/testimonial",
                dataType: "json"
            }
        },
        pageSize: 4,
        change: function () {
            $("#testimonialList").html(kendo.render(template, this.view()));
        },
        requestEnd: function () {
            $("#loading").hide();
            $("#content").show();
        }
    });

    dataSource.read();

    $("#pager").kendoPager({
        dataSource: dataSource,
        input: false,
        numeric: true
    });

});