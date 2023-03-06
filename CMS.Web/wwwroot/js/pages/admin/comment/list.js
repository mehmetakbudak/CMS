$(() => {
    var template = kendo.template($("#template").html());

    var dataSource = new kendo.data.DataSource({
        pageSize: 5,
        change: function () {
            $("#content-body").html(kendo.render(template, this.view()));
        }
    });

    getComments(1);

    app.selectStatus = function (e, id) {
        $(".btnStatus").removeClass("btn-primary").addClass("btn-secondary");
        $(e).removeClass("btn-secondary").addClass("btn-primary");
        getComments(id);
    }

    function getComments(id) {
        $.get(`/api/admin/comment/${id}`).then((res) => {
            dataSource.data(res);
        });

        $("#pager").kendoPager({
            dataSource: dataSource,
            responsive: false
        });
    }

});