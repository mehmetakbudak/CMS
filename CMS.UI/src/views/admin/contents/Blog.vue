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
      <div v-if="showGrid">
        <DataTable
          showGridlines
          :value="blogs"
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
          <Column field="title" header="Başlık"></Column>
          <Column field="url" header="Url"></Column>
          <Column field="numberOfView" header="Okunma Sayısı"></Column>
          <Column field="displayOrder" header="Sıra"></Column>
          <Column field="insertedDate" header="Kayıt Tarihi" dataType="date">
            <template #body="{ data }">
              {{ dateFormatValue(data.insertedDate) }}
            </template>
          </Column>
          <Column
            field="updatedDate"
            header="Güncelleme Tarihi"
            dataType="date"
          >
            <template #body="{ data }">
              {{
                data.updatedDate != null
                  ? dateFormatValue(data.updatedDate)
                  : ""
              }}
            </template>
          </Column>
          <Column field="published" header="Yayında">
            <template #body="slotProps">
              <div>
                {{ slotProps.data.published ? "Yayında" : "Yayında Değil" }}
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
          <div class="col-md-12">
            <div class="mb-3">
              <label class="form-label">Başlık</label>
              <InputText
                type="text"
                v-model="blog.title"
                placeholder="Başlık"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Kısa Açıklama</label>
              <Textarea
                type="text"
                rows="3"
                v-model="blog.description"
                placeholder="Kısa Açıklama"
                class="w-100"
              ></Textarea>
            </div>
            <div class="mb-3">
              <label class="form-label">İçerik</label>
              <Editor v-model="blog.content" editorStyle="height: 320px" />
            </div>
            <div class="mb-3">
              <label class="form-label">Url</label>
              <InputText
                type="text"
                v-model="blog.url"
                placeholder="Url"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <div class="row">
                <div class="col-md-2">
                  <label class="form-label">Sıra</label>
                  <InputNumber
                    v-model="blog.displayOrder"
                    placeholder="Sıra"
                    class="w-100"
                  />
                </div>
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label">Yayında</label>
              <div>
                <InputSwitch v-model="blog.published" />
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label">Aktif</label>
              <div>
                <InputSwitch v-model="blog.isActive" />
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
import GlobalService from "../../../services/GlobalService";
import dateFormat from "../../../infrastructure/DateFormat";
import { Endpoints } from "../../../services/Endpoints";
import AlertService from "../../../services/AlertService";

export default {
  name: "name",
  mixins: [AlertService],
  data() {
    return {
      title: "",
      showGrid: true,
      showForm: false,
      selectedBlog: {},
      blogs: [],
      blog: {
        id: 0,
        url: "",
        title: "",
        description: "",
        content: "",
        published: true,
        isActive: true,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Blog Düzenle";
            this.showForm = true;
            this.showGrid = false;
            this.blog = this.selectedBlog;
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
                  Endpoints.Admin.Blog,
                  this.selectedBlog.id
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
      GlobalService.GetByAuth(Endpoints.Admin.Blog).then((res) => {
        this.blogs = res.data;
      });
    },
    add() {
      this.showForm = true;
      this.showGrid = false;
      this.title = "Yeni Blog Ekle";
      this.blog = {
        id: 0,
        url: "",
        title: "",
        content: "",
        published: true,
        isActive: true,
      };
    },
    toggleGridMenu(event, data) {
      this.selectedBlog = data;
      this.$refs.menu.toggle(event);
    },
    dateFormatValue(value) {
      return dateFormat.convert(value);
    },
    reset() {
      this.showForm = false;
      this.showGrid = true;
      this.title = "Bloglar";
    },
    save() {
      if (this.blog.id == 0) {
        GlobalService.PostByAuth(Endpoints.Admin.Blog, this.blog)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.Blog, this.blog)
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
  },
};
</script>

<style lang="scss" scoped></style>
