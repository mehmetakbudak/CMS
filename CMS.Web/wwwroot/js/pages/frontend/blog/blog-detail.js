$(() => {
    var id = $("#blogId").val();
    var sourceType = $("#sourceType").val();
    $("#blogDetail").hide();
    $("#contentComment").hide();

    var validatorAddComment = $("#formAddComment").kendoValidator().data("kendoValidator");
    var validatorReplyComment = $("#formReplyComment").kendoValidator().data("kendoValidator");

    var txtComment = $("#txtComment").kendoTextArea({
        rows: 5,
        maxLength: 500
    }).data("kendoTextArea");

    var txtReplyComment = $("#txtReplyComment").kendoTextArea({
        rows: 3,
        maxLength: 500
    }).data("kendoTextArea");

    $("#txtComment").on('input', function (e) {
        $('#k-counter-comment .k-counter-value').html($(e.target).val().length);
    });

    $("#txtReplyComment").on('input', function (e) {
        $('#k-counter-reply-comment .k-counter-value').html($(e.target).val().length);
    });

    var viewModel = kendo.observable({
        blog: {},
        insertedDate: "",
        mostReads: [],
        comment: {
            id: 0,
            userFullName: "",
            description: ""
        }
    });

    $.get(`/api/blog/${id}`).then((res) => {
        viewModel.set("blog", res);
        viewModel.set("insertedDate", dateFormat(res.insertedDate));
        $("#loading").hide();
        $("#blogDetail").show();
        seen();
    });

    $.get(`/api/Blog/most-read`).then((res) => {
        viewModel.set("mostReads", res);
    });

    $("#btnAddComment").click((e) => {
        e.preventDefault();
        if (validatorAddComment.validate()) {
            var data = {
                sourceType: parseInt(sourceType),
                sourceId: parseInt(id),
                description: txtComment.value()
            };

            addComment(data).done((res) => {
                this.app.successNotification("İşlem Başarılı!", res.message);
                $("#formAddComment").trigger("reset");
                $("#k-counter-comment .k-counter-value").text("0");
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });

    function seen() {
        $.get(`/api/Blog/Seen/${id}`).then(() => { });
    }

    function addComment(data) {
        return $.ajax({
            type: "POST",
            url: "/api/comment",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    app.commentReply = function (e) {
        var id = $(e).data("id");
        var userFullName = $(e).data("user");
        var description = $(e).data("description");
        $("#formReplyComment").trigger("reset");
        $("#k-counter-reply-comment .k-counter-value").text("0");
        viewModel.set("comment.id", id);
        viewModel.set("comment.userFullName", userFullName);
        viewModel.set("comment.description", description);

        $("#replyCommentModal").modal("show");
    };

    app.login = function () {
        var blog = viewModel.get("blog");
        window.location.href = `/login?returnUrl=/blog/${blog.url}/${blog.id}`;
    };

    $("#btnCommentReply").click((e) => {
        e.preventDefault();
        if (validatorReplyComment.validate()) {
            var data = {
                sourceType: sourceType,
                sourceId: id,
                parentId: parseInt(viewModel.get("comment.id")),
                description: txtReplyComment.value()
            };
            addComment(data).done((res) => {
                this.app.successNotification("İşlem Başarılı!", res.message);
                $("#replyCommentModal").modal("hide");
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });

        }
    });

    kendo.bind($("#main"), viewModel);

});