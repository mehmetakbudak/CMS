new Vue({
    el: '#page',
    mixins: [alert],
    data: {
        users: [],
        isModalActive: false,
        modalTitle: '',
        id: 0,
        user: {
            id: 0,
            name: '',
            surname: '',
            emailAddress: '',
            userType: 0,
            userTypeName: '',
            isActive: true
        }
    },
    created: function () {
        this.userLoad();
    },
    methods: {
        userLoad() {
            axios.get("/api/User")
                .then(res => {
                    this.users = res.data;
                });
        },
        add(e) {
            this.modalTitle = 'Kullanıcı Ekle';
            this.isModalActive = true;
            this.user = {
                id: 0,
                name: '',
                surname: '',
                emailAddress: '',
                userType: 0,
                userTypeName: '',
                isActive: true
            }
        },
        edit(e) {
            this.modalTitle = 'Kullanıcı Düzenle';
            this.isModalActive = true;
            this.user = e;
        },
        remove(e) {
            this.$buefy.dialog.confirm({
                message: 'Silmek istiyor musunuz?',
                title: 'Silme Onayı',
                cancelText: 'Hayır',
                confirmText: 'Evet',
                onConfirm: () => {
                    axios.delete("/api/User/" + e.id)
                        .then(res => {
                            this.userLoad();
                            this.alertDelete();
                        });
                }
            });
        },
        save(e) {
            axios.post("/api/User/CreateOrUpdate", this.user)
                .then(res => {
                    this.userLoad();
                    this.isModalActive = false;
                    this.alertSave();
                    e.target.reset();
                })
                .catch(e => {
                    this.alertErrors(e.response.data.exceptions);
                });
        },
        hideModal() {
            this.isModalActive = false;
        }
    }
});