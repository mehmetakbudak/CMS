var alert = {
    data: function () {
        return {
            message: 'hello',
            foo: 'abc'
        }
    },
    methods: {
        alertErrors(errors) {
            var message = '';
            errors.forEach(res => {
                message = message + "<p>- " + res + "</p>";
            });
            this.$notify({
                title: "İşlem Başarısız!",
                message: message,
                dangerouslyUseHTMLString: true,
                position: 'top-right',
                type: 'error'
            });
        },
        alertSave() {
            this.$notify({
                title: "İşlem Başarılı!",
                message: 'Kaydetme işlemi başarıyla gerçekleşti.',
                position: 'top-right',
                type: 'success'
            });
        },
        alertDelete() {
            this.$notify({
                title: "İşlem Başarılı!",
                message: 'Silme işlemi başarıyla gerçekleşti.',
                type: 'success',
                position: 'top-right'
            });
        }
    }
}