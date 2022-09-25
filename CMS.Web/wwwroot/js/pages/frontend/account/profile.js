$(() => {
    var txtEmailAddress = $("#txtEmailAddress").kendoTextBox({
        enable: false
    }).data("kendoTextBox");

    var txtName = $("#txtName").kendoTextBox().data("kendoTextBox");

    var txtSurname = $("#txtSurname").kendoTextBox().data("kendoTextBox");

    var txtPhone = $("#txtPhone").kendoMaskedTextBox({
        mask: "(999) 000-0000"
    }).data("kendoMaskedTextBox");

    getProfile();

    function getProfile() {
        $.get("/api/account/profile", (res => {
            txtEmailAddress.value(res.emailAddress);
            txtName.value(res.name);
            txtSurname.value(res.surname);
            txtPhone.value(res.phone);
        }));
    }

    var validator = $("#profileForm").kendoValidator().data("kendoValidator");

    $("#btnProfile").click((event) => {
        event.preventDefault();

        if (validator.validate()) {
            var data = {
                emailAddress: txtEmailAddress.value(),
                name: txtName.value(),
                surname: txtSurname.value(),
                phone: txtPhone.value()
            };

            $.ajax({
                type: "PUT",
                url: "/api/account/profile",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8"                
            }).done((res) => {                
                getProfile();
                this.app.successNotification("İşlem Başarılı!", res.message);
            }).fail((err) => {
                this.app.errorNotification("İşlem Başarısız!", err.responseJSON.message);
            });
        }
    });
});