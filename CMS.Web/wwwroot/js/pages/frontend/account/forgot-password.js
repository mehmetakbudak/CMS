$(() => {
    var txtEmailAddress = $("#txtEmailAddress").kendoTextBox().data("kendoTextBox");
    var validator = $("#forgotPasswordForm").kendoValidator().data("kendoValidator");

    $("#btnSend").click((event) => {
        event.preventDefault();

        if (validator.validate()) {

            var data = {
                emailAddress: txtEmailAddress.value()
            };

            $.ajax({
                type: "POST",
                url: "/api/account/forgot-password",
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