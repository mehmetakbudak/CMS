new Vue({
    el: '#contact',
    mixins: [alert],
    data: {
        categories: [],
        contact: {
            nameSurname: '',
            emailAddress: '',
            subject: 0,
            message: ''
        }
    },
    methods: {
        save(e) {
            axios.post('/api/contact', this.contact)
                .then(res => {
                    this.alertSave();
                    e.target.reset();
                })
                .catch(e => {
                    this.alertErrors(e.response.data.exceptions);
                });
            e.preventDefault();
        }
    },
    mounted() {
        axios.get('/api/Lookup/ContactCategories')
            .then(response => {
                this.categories = response.data
            });
    }
});