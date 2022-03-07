<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h4>{{ title }}</h4>
        </div>
        <div class="col-6">
          <Button
            v-if="showGrid"
            icon="pi pi-plus"
            class="p-button-primary p-button-sm float-end"
            @click="add()"
          />
          <Button
            v-if="showForm"
            icon="pi pi-arrow-left"
            class="p-button-primary p-button-sm float-end"
            @click="reset()"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <div v-if="showGrid" class="py-3">
        <DataTable
          showGridlines
          :value="blogCategories"
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
                class="p-button-rounded p-button-info p-button-sm"
                @click="toggleGridMenu($event, slotProps.data)"
              />
              <Menu ref="menu" :model="gridMenuItems" :popup="true" />
            </template>
          </Column>
          <Column field="name" header="Kategori Adı"></Column>
          <Column field="url" header="Url"></Column>
          <Column field="isShowHome" header="Anasayfada Gösterilsin">
            <template #body="slotProps">
              <div>
                {{ slotProps.data.isShowHome ? "Evet" : "Hayır" }}
              </div>
            </template>
          </Column>
          <Column field="isActive" header="Aktif">
            <template #body="slotProps">
              <div>
                {{ slotProps.data.isActive ? "Aktif" : "Pasif" }}
              </div>
            </template>
          </Column>
        </DataTable>
      </div>
      <div v-if="showForm">
        <div class="row">
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Kategori Adı</label>
              <InputText
                type="text"
                v-model="blogCategory.name"
                placeholder="Adı"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Url</label>
              <InputText
                type="text"
                v-model="blogCategory.url"
                placeholder="Url"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Anasayfada Gösterilsin</label>
              <div>
                <InputSwitch v-model="blogCategory.isShowHome" />
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label">Aktif</label>
              <div>
                <InputSwitch v-model="blogCategory.isActive" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="card-footer bg-white py-3" v-if="showForm">
      <Button label="Kaydet" @click="save()" />
      <Button
        label="Vazgeç"
        @click="reset()"
        class="ms-2 p-button-outlined p-button-secondary"
      />
    </div>
  </div>
</template>

<script>
import AlertService from "../../../services/AlertService";
import { Endpoints } from "../../../services/Endpoints";
import GlobalService from "../../../services/GlobalService";

export default {
  name: "name",
  mixins: [AlertService],
  data() {
    return {
      blogCategories: [],
      showGrid: true,
      showForm: false,
      selectedBlogCategory: {},
      title: "",
      blogCategory: {
        deleted: false,
        id: 0,
        isActive: true,
        isShowHome: true,
        name: "",
        url: "",
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Blog Düzenle";
            this.showForm = true;
            this.showGrid = false;
            this.blogCategory = this.selectedBlogCategory;
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
                  Endpoints.Admin.BlogCategory,
                  this.selectedBlogCategory.id
                )
                  .then((res) => {
                    this.getAll();
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
    this.reset();
  },
  methods: {
    getAll() {
      GlobalService.GetByAuth(Endpoints.Admin.BlogCategory).then((res) => {
        this.blogCategories = res.data;
      });
    },
    add() {
      this.showForm = true;
      this.showGrid = false;
      this.title = "Yeni Blog Kategorisi Ekle";
      this.blogCategory = {
        deleted: false,
        id: 0,
        isActive: true,
        isShowHome: true,
        name: "",
        url: "",
      };
    },
    toggleGridMenu(event, data) {
      this.selectedBlogCategory = data;
      this.$refs.menu.toggle(event);
    },
    save() {
      if (this.blogCategory.id == 0) {
        GlobalService.PostByAuth(
          Endpoints.Admin.BlogCategory,
          this.blogCategory
        )
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.BlogCategory, this.blogCategory)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      }
    },
    reset() {
      this.showForm = false;
      this.showGrid = true;
      this.title = "Blog Kategorileri";
    },
  },
};
</script>
