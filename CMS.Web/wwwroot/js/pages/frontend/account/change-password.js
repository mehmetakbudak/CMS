$(() => {
    var txtOldPassword = $("#txtOldPassword").kendoTextBox().data("kendoTextBox");
    var txtNewPassword = $("#txtNewPassword").kendoTextBox().data("kendoTextBox");
    var txtReNewPassword = $("#txtReNewPassword").kendoTextBox().data("kendoTextBox");
    var validator = $("#changePasswordForm").kendoValidator().data("kendoValidator");

    $("#btnChangePassword").click((event) => {
        event.preventDefault();

        if (validator.validate()) {
            var data = {
                oldPassword: txtOldPassword.value(),
                newPassword: txtNewPassword.value(),
                reNewPassword: txtReNewPassword.value()
            };

            $.ajax({
                type: "PUT",
                url: "/api/account/change-password",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done((res) => {
                this.app.successNotification("İşlem Başarılı!", res.message);
                $("#changePasswordForm").trigger("reset");
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });
});