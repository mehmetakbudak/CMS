export default {
    data() {
        return {
            hello: "Merhaba",
            asd: this
        }
    },
    methods: {
        successMessage(e, message) {
            e.$toast.add({
                severity: "success",
                summary: "İşlem Başarılı",
                detail: message,
                life: 5000
            });
        },
        errorMessage(e, message) {
            e.$toast.add({
                severity: "error",
                summary: "İşlem Başarısız",
                detail: message,
                life: 5000
            });
        }
    }
}