$(() => {   
    var blogTemplate = kendo.template($("#blogTemplate").html());
    var blogCategoryTemplate = kendo.template($("#blogCategoryTemplate").html());
    var blogMostReadTemplate = kendo.template($("#blogMostReadTemplate").html());

    var blogCategoryDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/api/blog-category",
                dataType: "json"
            }
        },
        change: function () {
            $("#blogCategoryList").html(kendo.render(blogCategoryTemplate, this.view()));
        }
    });

    $("#content").hide();
    var searchText = $("#searchText").val();   

    var blogDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: searchText ? `/api/blog?text=${searchText}` : "/api/Blog",
                dataType: "json"
            }
        },
        pageSize: 5,
        change: function () {
            $("#blogList").html(kendo.render(blogTemplate, this.view()));
        },
        requestEnd: function () {
            $("#loading").hide();
            $("#content").show();
        }
    });

    var blogMostReadDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/api/blog/most-read",
                dataType: "json"
            }
        },
        pageSize: 5,
        change: function () {
            $("#blogMostReadList").html(kendo.render(blogMostReadTemplate, this.view()));
        }
    });

    blogDataSource.read();
    blogCategoryDataSource.read();
    blogMostReadDataSource.read();

    $("#pager").kendoPager({
        dataSource: blogDataSource,
        responsive: false
    });

    $("#txtSearch").val(searchText);

    $("#btnSearch").click(() => {
        var text = $("#txtSearch").val();
        window.location.href = text ? `/blog?text=${text}` : "/blog";
    });
});