$(() => {
    var txtName = $("#txtName").kendoTextBox().data("kendoTextBox");
    var txtSurname = $("#txtSurname").kendoTextBox().data("kendoTextBox");
    var txtEmailAddress = $("#txtEmailAddress").kendoTextBox().data("kendoTextBox");
    var txtPhone = $("#txtPhone").kendoMaskedTextBox({
        mask: "(999) 000-0000"
    }).data("kendoMaskedTextBox");
    var txtPassword = $("#txtPassword").kendoTextBox().data("kendoTextBox");
    var txtRePassword = $("#txtRePassword").kendoTextBox().data("kendoTextBox");

    var validator = $("#registerForm").kendoValidator().data("kendoValidator");

    $("#btnRegister").click((event) => {
        event.preventDefault();

        if (validator.validate()) {

            var data = {
                name: txtName.value(),
                surname: txtSurname.value(),
                emailAddress: txtEmailAddress.value(),
                phone: txtPhone.value(),
                password: txtPassword.value(),
                rePassword: txtRePassword.value()
            };

            $.ajax({
                type: "POST",
                url: "/api/account/add-member",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done((res) => {
                this.app.successNotification("İşlem Başarılı!", res.message);
                setTimeout(() => {
                    window.location.href = "/login";
                }, 2000);
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });
});
