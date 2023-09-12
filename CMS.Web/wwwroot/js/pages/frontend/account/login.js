$(() => {

    var txtEmailAddress = $("#txtEmailAddress").kendoTextBox().data("kendoTextBox");
    var txtPassword = $("#txtPassword").kendoTextBox().data("kendoTextBox");
    var validator = $("#loginForm").kendoValidator().data("kendoValidator");


    $("#btnLogin").click((e) => {
        e.preventDefault();

        if (validator.validate()) {
            var data = {
                emailAddress: txtEmailAddress.value(),
                password: txtPassword.value()
            };

            $.ajax({
                type: "POST",
                url: "/api/account/login",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done(() => {
                var returnUrl = $("#txtReturnUrl").val();
                if (returnUrl) {
                    window.location.href = returnUrl;
                } else {
                    window.location.href = "/";
                }
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });
});