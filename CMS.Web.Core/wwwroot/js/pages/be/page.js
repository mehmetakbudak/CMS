new Vue({
    el: '#page',
    mixins: [alert],
    data: {
        pages: [],
        menus: [],        
        pageId: 0,
        page: {
            id: 0,
            menuId: 0,
            name: '',
            title: '',
            url: '',
            content: ''
        }
    },
    mounted: function () {
        this.$nextTick(function () {
            this.pageId = document.getElementById("pageVal").value;
            this.pageLoad();
            this.lookupMenuLoad();
            $('#summernote').summernote({
                lang: 'tr-TR',
                height: '300px',
                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['color', ['color']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['insert', ['link', 'picture', 'video']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ]
            });

        });
    },
    methods: {
        pageLoad() {
            if (this.pageId != 0) {
                axios.get("/api/Page/" + this.pageId)
                    .then(res => {
                        this.page = res.data;
                        $('#summernote').summernote('code', res.data.content);
                    });
            }
        },
        lookupMenuLoad() {
            axios.get("/api/Lookup/Menus")
                .then(res => {
                    this.menus = res.data;
                });
        },
        save() {
            this.page.content = $('#summernote').summernote('code');
            axios.post("/api/Page/CreateOrUpdate", this.page)
                .then(res => {
                    this.alertSave();
                    setTimeout(function () {
                        window.location.href = "/Admin/Page";
                    }, 1000);
                })
                .catch(e => {
                    this.alertErrors(e.response.data.exceptions);
                });
        },
        clearMenu() {
            this.page.menuId = null;
        }
    }
});