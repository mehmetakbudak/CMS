new Vue({
    el: '#login',
    mixins: [alert],
    data: {
        formData: {
            emailAddress: '',
            password: '',
            returnUrl: ''
        }
    },
    methods: {
        login(e) {
            axios.post('/api/login', this.formData)
                .then(res => {
                    this.$notify({
                        title: 'İşlem Başarılı',
                        message: "Hoşgeldiniz! " + res.data.data,
                        type: 'success'
                    });                   
                    setTimeout(function () {
                        window.location.href = "/";
                    }, 1000);
                })
                .catch(e => {
                    console.log(e);
                    //this.alertErrors(e.response.data.exceptions);
                });
            e.preventDefault();
        }
    }
});