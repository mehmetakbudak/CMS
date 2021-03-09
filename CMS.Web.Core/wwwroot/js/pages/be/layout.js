const app = new Vue({
    data: {
        treeData: [],
        openSidebar: false,
        screenWidth: 0,
        defaultProps: {
            children: 'items',
            label: 'title'
        }
    },
    created: function () {
        this.screenWidth = window.screen.width;
        this.loadData();

    },
    methods: {
        loadData() {
            axios.get("/api/menu/backend")
                .then(res => {
                    this.treeData = res.data;
                });
        },
        filterNode(value, data) {
            if (!value) return true;
            return data.label.indexOf(value) !== -1;
        },
        selectMenu(e) {
            if (e) {
                window.location.href = e.url;
            }
        }
    }
});

app.$mount('#app')