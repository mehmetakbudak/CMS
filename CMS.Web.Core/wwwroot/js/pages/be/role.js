new Vue({
    el: '#page',
    mixins: [alert],
    data: {
        roles: [],
        isModalActive: false,
        modalTitle: '',
        pageIndex: 1,
        role: {
            id: 0,
            name: '',
            isActive: true
        }
    },
    created: function () {
        this.roleLoad();
    },
    methods: {
        roleLoad() {
            axios.get("/api/Role?page=" + this.pageIndex)
                .then(res => {
                    this.roles = res.data;
                });
        },
        add() {
            this.modalTitle = 'Rol Ekle';
            this.isModalActive = true;
            this.role = {
                id: 0,
                name: '',
                isActive: true
            }
        },
        edit(e) {
            this.modalTitle = 'Rol Düzenle';
            this.isModalActive = true;
            this.role = e;
        },
        remove(e) {
            this.$confirm('Silmek istiyor musunuz?', 'Silme Onayı',
                {
                    confirmButtonText: 'Evet',
                    cancelButtonText: 'Hayır',
                    type: 'warning'
                }).then((a) => {
                    console.log(a);
                    axios.delete("/api/Role/" + e.id)
                        .then(res => {
                            this.roleLoad();
                            this.alertDelete();
                        });
                }).catch(() => { });
        },
        save() {
            axios.post("/api/Role/CreateOrUpdate", this.role)
                .then(res => {
                    this.roleLoad();
                    this.isModalActive = false;
                    this.alertSave();
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