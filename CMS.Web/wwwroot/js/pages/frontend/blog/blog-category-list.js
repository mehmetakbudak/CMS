$(() => {
    var categoryUrl = $("#categoryUrl").val();
    var blogCategoryTemplate = kendo.template($("#blogCategoryTemplate").html());
    var blogTemplate = kendo.template($("#blogTemplate").html());
    var blogMostReadTemplate = kendo.template($("#blogMostReadTemplate").html());
    $("#content").hide();

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

    var blogDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: `/api/blog/by-category/${categoryUrl}`,
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

    blogCategoryDataSource.read();
    blogDataSource.read();
    blogMostReadDataSource.read();

    $("#pager").kendoPager({
        dataSource: blogDataSource,
        messages: {
            empty: "Kayıt bulunamadı."
        }
    });

    $("#btnSearch").click(() => {
        var text = $("#txtSearch").val();
        window.location.href = text ? `/blog?text=${text}` : "/blog";
    });

    $.get(`/api/blog-category/${categoryUrl}`).then((res) => {
        viewModel.set("blogCategory", res);
    });

    var viewModel = kendo.observable({
        blogCategory: {},
    });

    kendo.bind($("#main"), viewModel);
});