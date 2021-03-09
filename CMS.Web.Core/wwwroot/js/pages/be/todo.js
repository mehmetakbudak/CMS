new Vue({
    el: '#page',
    mixins: [alert],
    data: {
        todos: [],
        todoCategories: [],
        todoStatuses: [],
        lookupCategories: [],
        lookupStatuses: [],
        lookupUsers: [],
        isTodoModalActive: false,
        isTodoCategoryModalActive: false,
        isTodoStatusModalActive: false,
        todoModalTitle: '',
        todoCategoryModalTitle: '',
        todoStatusModalTitle: '',
        currentTab: 1,
        todo: {
            id: 0,
            todoCategoryId: 0,
            todoCategoryName: '',
            todoStatusId: 0,
            todoStatusName: '',
            userId: 0,
            userNameSurname: '',
            title: '',
            description: '',
            userComment: '',
            isActive: true
        },
        todoCategory: {
            id: 0,
            name: '',
            isActive: true
        },
        todoStatus: {
            id: 0,
            name: '',
            todoCategoryId: 0,
            isActive: true
        },
    },
    created: function () {
        this.todoLoad();
    },
    methods: {
        todoLoad() {
            axios.get("/api/Todo")
                .then(res => {
                    this.todos = res.data;
                });
        },
        todoCategoryLoad() {
            axios.get("/api/TodoCategory")
                .then(res => {
                    this.todoCategories = res.data;
                });
        },
        todoStatusLoad() {
            axios.get("/api/TodoStatus")
                .then(res => {
                    this.todoStatuses = res.data;
                });
        },
        lookupCategoryLoad() {
            axios.get("/api/Lookup/TodoCategories")
                .then(res => {
                    this.lookupCategories = res.data;
                });
        },
        lookupStatusLoad(categoryId) {
            axios.get("/api/Lookup/TodoStatuses/" + categoryId)
                .then(res => {
                    this.lookupStatuses = res.data;
                });
        },
        lookupUsersLoad() {
            axios.get("/api/Lookup/Users")
                .then(res => {
                    this.lookupUsers = res.data;
                });
        },
        selectCategory(e) {
            this.lookupStatusLoad(e);
        },
        add() {
            this.errors = [];
            if (this.currentTab == 1) {
                this.todo = {
                    id: 0,
                    todoCategoryId: 0,
                    todoCategoryName: '',
                    todoStatusId: 0,
                    todoStatusName: '',
                    userId: 0,
                    userNameSurname: '',
                    title: '',
                    description: '',
                    userComment: '',
                    isActive: true
                };
                this.isTodoModalActive = true;
                this.todoModalTitle = "Yapılacak Ekle";
                this.lookupCategoryLoad();
                this.lookupUsersLoad();
            } else if (this.currentTab == 2) {
                this.todoCategory = {
                    id: 0,
                    name: '',
                    isActive: true
                };
                this.isTodoCategoryModalActive = true;
                this.todoCategoryModalTitle = "Kategori Ekle";
            } else if (this.currentTab == 3) {
                this.todoStatus = {
                    id: 0,
                    todoCategoryId: 0,
                    name: '',
                    isActive: true
                };
                this.isTodoStatusModalActive = true;
                this.todoStatusModalTitle = "Durum Ekle";
                this.todoCategoryLoad();
                this.lookupCategoryLoad();
            }
        },
        edit(e) {
            if (this.currentTab == 1) {
                this.todoModalTitle = "Yapılacak Düzenle";
                this.todo = e;
                this.isTodoModalActive = true;
                this.lookupCategoryLoad();
                this.lookupStatusLoad(this.todo.todoCategoryId);
                this.lookupUsersLoad();
            } else if (this.currentTab == 2) {
                this.todoCategoryModalTitle = "Kategori Düzenle";
                this.todoCategory = e;
                this.isTodoCategoryModalActive = true;
            } else if (this.currentTab == 3) {
                this.todoCategoryModalTitle = "Durum Düzenle";
                this.todoStatus = e;
                this.isTodoStatusModalActive = true;
                this.lookupCategoryLoad();
            }
        },
        save() {
            if (this.currentTab == 1) {
                axios.post("/api/Todo/CreateOrUpdate", this.todo)
                    .then(res => {
                        this.todoLoad();
                        this.isTodoModalActive = false;
                        this.alertSave();
                    })
                    .catch(e => {
                        this.alertErrors(e.response.data.exceptions);
                    });
            } else if (this.currentTab == 2) {
                axios.post("/api/TodoCategory/CreateOrUpdate", this.todoCategory)
                    .then(res => {
                        this.todoCategoryLoad();
                        this.isTodoCategoryModalActive = false;
                        this.alertSave();
                    })
                    .catch(e => {
                        this.alertErrors(e.response.data.exceptions);
                    });
            } else if (this.currentTab == 3) {
                axios.post("/api/TodoStatus/CreateOrUpdate", this.todoStatus)
                    .then(res => {
                        this.todoStatusLoad();
                        this.isTodoStatusModalActive = false;
                        this.alertSave();
                    })
                    .catch(e => {
                        this.alertErrors(e.response.data.exceptions);
                    });
            }
        },
        remove(e) {
            if (this.currentTab == 1) {
                this.$buefy.dialog.confirm({
                    message: 'Silmek istiyor musunuz?',
                    title: 'Silme Onayı',
                    cancelText: 'Hayır',
                    confirmText: 'Evet',
                    onConfirm: () => {
                        axios.delete("/api/Todo/" + e.id)
                            .then(res => {
                                this.todoLoad();
                                this.alertDelete();
                            });
                    }
                });
            } else if (this.currentTab == 2) {
                this.$buefy.dialog.confirm({
                    message: 'Silmek istiyor musunuz?',
                    title: 'Silme Onayı',
                    cancelText: 'Hayır',
                    confirmText: 'Evet',
                    onConfirm: () => {
                        axios.delete("/api/TodoCategory/" + e.id)
                            .then(res => {
                                this.todoCategoryLoad();
                                this.alertDelete();
                            });
                    }
                });
            } else if (this.currentTab == 3) {
                this.$buefy.dialog.confirm({
                    message: 'Silmek istiyor musunuz?',
                    title: 'Silme Onayı',
                    cancelText: 'Hayır',
                    confirmText: 'Evet',
                    onConfirm: () => {
                        axios.delete("/api/TodoStatus/" + e.id)
                            .then(res => {
                                this.todoStatusLoad();
                                this.alertDelete();
                            });
                    }
                });
            }
        },
        tabChanged(e) {
            this.currentTab = e;
            if (e == 1) {
                this.todoLoad();
            } else if (e == 2) {
                this.todoCategoryLoad();
            } else if (e == 3) {
                this.todoStatusLoad();
            }
        },
        hideModal() {
            this.isTodoModalActive = false;
            this.isTodoCategoryModalActive = false;
            this.isTodoStatusModalActive = false;
        }
    }
});