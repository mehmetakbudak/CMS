

$(() => {
    var id = $("#txtId").val();

    var txtTitle = $("#txtTitle").kendoTextBox().data("kendoTextBox");

    var txtUrl = $("#txtUrl").kendoTextBox().data("kendoTextBox");

    var txtDescription = $("#txtDescription").kendoTextArea({
        rows: 3
    }).data("kendoTextArea");

    var cmbBlogCategories = $("#cmbBlogCategories").kendoMultiSelect({
        placeholder: "Kategori seçiniz.",
        dataTextField: "name",
        dataValueField: "id",
        autoBind: false,
        dataSource: {            
            transport: {
                read: {
                    url: "/api/Admin/Lookup/BlogCategories",
                }
            }
        }
    }).data("kendoMultiSelect");

    var txtContent = $("#txtContent").kendoEditor().data("kendoEditor");

    var txtDisplayOder = $("#txtDisplayOder").kendoNumericTextBox({
        min: 1,
        step: 1,
        format: "#"
    }).data("kendoNumericTextBox");

    var chkPublished = $("#chkPublished").kendoCheckBox().data("kendoCheckBox");

    var chkIsActive = $("#chkIsActive").kendoCheckBox().data("kendoCheckBox");

    if (id != 0) {
        $.get(`/api/admin/blog/${id}`).then((res) => {
            txtTitle.value(res.title);
            txtUrl.value(res.url);
            cmbBlogCategories.value(res.blogCategories);
            txtDescription.value(res.description);
            txtContent.value(res.content);
            txtDisplayOder.value(res.displayOrder);
            chkIsActive.value(res.isActive);
            chkPublished.value(res.published);
            $("#imgUrl").attr("src", res.imageUrl);
        });
    }

    $("#btnSave").click(() => {
        var data = new FormData();
        var file = $('#fileImage')[0].files[0];

        if (id == 0 && !file) {
            errorNotification("İşlem Başarısız!", "Resim ekleyiniz!");
            return;
        }
        data.append("id", id);
        data.append("image", file);
        data.append("title", txtTitle.value());
        data.append("description", txtDescription.value());
        data.append("blogCategories", cmbBlogCategories.value());
        data.append("content", txtContent.value());
        data.append("displayOrder", txtDisplayOder.value());
        data.append("published", chkPublished.value());
        data.append("isActive", chkIsActive.value());
        var requestType = "POST";

        if (id != 0) {
            requestType = "PUT";
        }

        $.ajax({
            type: requestType,
            url: "/api/admin/blog",
            contentType: false,
            processData: false,
            data: data
        }).done((res) => {
            app.successNotification("İşlem Başarılı!", res.message);
            setTimeout(() => {
                window.location.href = "/admin/blog";
            }, 1000);

        }).fail((err) => {
            app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
        });
    });
});