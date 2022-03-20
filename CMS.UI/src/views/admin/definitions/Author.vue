<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h5>{{ title }}</h5>
        </div>
        <div class="col-6">
          <Button
            icon="pi pi-plus"
            class="p-button-primary p-button-sm float-end"
            @click="add()"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <div class="border border-top-0" v-if="showGrid">
        <DataTable
          :loading="loading"
          showGridlines
          :value="authors"
          :paginator="true"
          :rows="5"
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
          :rowsPerPageOptions="[5, 10, 20, 50]"
          responsiveLayout="scroll"
          currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
        >
          <Column header="" class="w-50px">
            <template #body="slotProps">
              <Button
                icon="pi pi-cog"
                class="p-button-rounded p-button-primary p-button-sm"
                @click="toggleGridMenu($event, slotProps.data)"
              />
              <Menu ref="menu" :model="gridMenuItems" :popup="true" />
            </template>
          </Column>
          <Column field="nameSurname" header="Yazar Adı"></Column>
          <Column field="isActive" header="Aktif">
            <template #body="slotProps">
              <div>
                {{ slotProps.data.isActive ? "Aktif" : "Pasif" }}
              </div>
            </template>
          </Column>
        </DataTable>
      </div>

      <div class="mb-3 pb-3" v-if="showForm">
        <div class="row">
          <div class="col-md-6">
            <div class="mb-3">
              <label>Adı Soyadı</label>
              <InputText
                type="text"
                v-model="author.nameSurname"
                placeholder="Adı"
                class="w-100"
              />
            </div>
            <div class="mb-3" v-if="author.fileUrl != ''">
              <FileUpload
                ref="file"
                chooseLabel="Dosya Seç"
                mode="basic"
                accept="image/*"
                @change="onUpload()"
              />
            </div>
          </div>
          <div class="col-md-6">
            <img :src="author.fileSrc" class="img-thumbnail" />
          </div>
        </div>
        <div class="mb-3">
          <label>Hayatı</label>
          <Editor v-model="author.resume" editorStyle="height: 320px" />
        </div>
        <div class="mb-3">
          <label>Aktif</label>
          <div>
            <InputSwitch v-model="author.isActive" />
          </div>
        </div>

        <div class="float-end">
          <Button
            label="Vazgeç"
            @click="reset()"
            class="p-button-outlined p-button-secondary"
          />
          <Button class="ms-2" label="Kaydet" @click="save()" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import GlobalService from "../../../services/GlobalService";
import alertService from "../../../services/AlertService";
import { Endpoints } from "../../../services/Endpoints";

export default {
  name: "name",
  mixins: [alertService],
  data() {
    return {
      loading: false,
      authors: [],
      exceptions: [],
      showGrid: true,
      showForm: false,
      selectedAuthor: {},
      title: "Yazarlar",
      author: {
        id: 0,
        nameSurname: "",
        resume: "",
        file: new FormData(),
        fileSrc: "",
        isActive: true,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Yapılacak Düzenle";
            this.showForm = true;
            this.showGrid = false;
            var e = this.selectedAuthor;
            this.author = {
              id: e.id,
              nameSurname: e.nameSurname,
              resume: e.resume,
              isActive: e.isActive,
              fileSrc: e.fileUrl,
            };
          },
        },
        {
          label: "Sil",
          command: () => {
            this.$confirm.require({
              message: "Silmek istediğinize emin misiniz?",
              header: "Silme Onayı",
              icon: "pi pi-exclamation-triangle",
              acceptLabel: "Evet",
              rejectLabel: "Hayır",
              accept: () => {
                GlobalService.DeleteByAuth(
                  Endpoints.Admin.Author,
                  this.selectedAuthor.id
                )
                  .then((res) => {
                    this.getAuthors();
                    this.successMessage(this, res.data.message);
                  })
                  .catch((e) => {
                    this.errorMessage(this, e.response.data.message);
                  });
              },
            });
          },
        },
      ],
    };
  },
  created() {
    this.getAll();
  },
  methods: {
    getAll() {
      this.loading = true;
      GlobalService.GetByAuth(Endpoints.Admin.Author).then((res) => {
        this.authors = res.data;
        this.loading = false;
      });
    },
    toggleGridMenu(event, data) {
      this.selectedAuthor = data;
      this.$refs.menu.toggle(event);
    },
    add() {
      this.showForm = true;
      this.showGrid = false;
      this.title = "Yeni Yazar Ekle";
    },
    edit() {},
    remove() {},
    onUpload() {
      console.log(this.$refs.file.files[0]);
      this.author.file = this.$refs.file.files[0];
    },
    save() {
      console.log(this.author);
      if (this.author.id == 0) {
        GlobalService.PostByAuth(Endpoints.Admin.Author, this.author)
          .then((res) => {
            this.getAuthors();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.Author, this.author)
          .then((res) => {
            this.getAuthors();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      }
    },
    reset() {
      this.author = {
        id: 0,
        nameSurname: "",
        resume: "",
        file: new FormData(),
        fileUrl: "",
        isActive: true,
      };
      this.showForm = false;
      this.showGrid = true;
      this.title = "Yazarlar";
    },
  },
};
</script>
