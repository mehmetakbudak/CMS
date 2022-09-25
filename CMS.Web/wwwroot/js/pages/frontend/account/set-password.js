$(() => {
    var txtNewPassword = $("#txtNewPassword").kendoTextBox().data("kendoTextBox");
    var txtReNewPassword = $("#txtReNewPassword").kendoTextBox().data("kendoTextBox");
    var validator = $("#setPasswordForm").kendoValidator().data("kendoValidator");
    var code = $("#code").val(); 

    $("#btnSave").click((event) => {
        event.preventDefault();

        if (validator.validate()) {

            var data = {
                code: code,
                newPassword: txtNewPassword.value(),
                reNewPassword: txtReNewPassword.value()
            };

            $.ajax({
                type: "PUT",
                url: "/api/account/reset-password",
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