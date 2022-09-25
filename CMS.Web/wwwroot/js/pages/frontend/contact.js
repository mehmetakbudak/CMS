$(() => {
    var txtEmailAddress = $("#txtEmailAddress").kendoTextBox().data("kendoTextBox");
    var txtName = $("#txtName").kendoTextBox().data("kendoTextBox");
    var txtSurname = $("#txtSurname").kendoTextBox().data("kendoTextBox");
    var txtMessage = $("#txtMessage").kendoTextArea({
        rows: "5"
    }).data("kendoTextArea");

    var cmbContactCategory = $("#cmbContactCategory").kendoDropDownList({
        dataTextField: "name",
        dataValueField: "id",
        optionLabel: "Konu seçiniz...",
        dataSource: {
            transport: {
                read: {
                    dataType: "json",
                    url: "api/Lookup/ContactCategory",
                }
            }
        }
    }).data("kendoDropDownList");

    var validator = $("#contactForm").kendoValidator().data("kendoValidator");

    $("#btnSend").click((e) => {
        e.preventDefault();

        if (validator.validate()) {
            var data = {
                name: txtName.value(),
                surname: txtSurname.value(),
                contactCategoryId: cmbContactCategory.value(),
                emailAddress: txtEmailAddress.value(),
                message: txtMessage.value()
            };

            $.ajax({
                type: "POST",
                url: "/api/contact",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done((res) => {
                this.app.successNotification("İşlem Başarılı!", res.message);
                $("#contactForm").trigger("reset");
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });
});