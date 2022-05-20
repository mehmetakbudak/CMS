<template>
  <div class="card" v-if="showGrid">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h3>Bloglar</h3>
        </div>
        <div class="col-6">
          <DxButton icon="plus" @click="add" type="default" class="float-end" />
        </div>
      </div>
    </div>
    <div class="card-body">
      <div v-if="showGrid">
        <DataTable
          :loading="loading"
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
          <Column field="updatedDate" header="Güncelleme Tarihi" dataType="date">
            <template #body="{ data }">
              {{ data.updatedDate != null ? dateFormatValue(data.updatedDate) : "" }}
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
          <template #empty> Kayıt bulunamadı. </template>
        </DataTable>
      </div>
      <div v-if="showForm"></div>
    </div>
  </div>
  <form @submit="save" v-if="showForm">
    <div class="card">
      <div class="card-header bg-white py-3">
        <div class="row">
          <div class="col-6">
            <h3>{{ title }}</h3>
          </div>
          <div class="col-6">
            <DxButton icon="arrowleft" @click="reset" type="default" class="float-end" />
          </div>
        </div>
      </div>
      <div class="card-body">
        <div class="row">
          <div class="col-md-12 px-3">
            <div class="mb-3">
              <label class="form-label">Başlık</label>
              <DxTextBox v-model:value="blog.title" mode="text" placeholder="Başlık" />
            </div>
            <div class="mb-3">
              <label class="form-label">Kısa Açıklama</label>
              <DxTextArea v-model:value="blog.description" placeholder="Kısa Açıklama" />
            </div>
            <div class="mb-3">
              <label class="form-label">Blog Kategoriler</label>
              <DxTagBox
                v-model:value="blog.blogCategories"
                :data-source="blogCategories"
                display-expr="name"
                value-expr="id"
                placeholder="Kategori seçiniz."
              />
            </div>
            <div class="mb-3">
              <label class="form-label">İçerik</label>
              <DxHtmlEditor v-model:value="blog.content" height="300px">
                <DxMediaResizing :enabled="true" />
                <DxToolbar :multiline="true">
                  <DxItem name="undo" />
                  <DxItem name="redo" />
                  <DxItem name="separator" />
                  <DxItem :accepted-values="sizeValues" name="size" />
                  <DxItem :accepted-values="fontValues" name="font" />
                  <DxItem name="separator" />
                  <DxItem name="bold" />
                  <DxItem name="italic" />
                  <DxItem name="strike" />
                  <DxItem name="underline" />
                  <DxItem name="separator" />
                  <DxItem name="alignLeft" />
                  <DxItem name="alignCenter" />
                  <DxItem name="alignRight" />
                  <DxItem name="alignJustify" />
                  <DxItem name="separator" />
                  <DxItem name="orderedList" />
                  <DxItem name="bulletList" />
                  <DxItem name="separator" />
                  <DxItem :accepted-values="headerValues" name="header" />
                  <DxItem name="separator" />
                  <DxItem name="color" />
                  <DxItem name="background" />
                  <DxItem name="separator" />
                  <DxItem name="link" />
                  <DxItem name="image" />
                  <DxItem name="separator" />
                  <DxItem name="clear" />
                  <DxItem name="codeBlock" />
                  <DxItem name="blockquote" />
                  <DxItem name="separator" />
                  <DxItem name="insertTable" />
                  <DxItem name="deleteTable" />
                  <DxItem name="insertRowAbove" />
                  <DxItem name="insertRowBelow" />
                  <DxItem name="deleteRow" />
                  <DxItem name="insertColumnLeft" />
                  <DxItem name="insertColumnRight" />
                  <DxItem name="deleteColumn" />
                </DxToolbar>
              </DxHtmlEditor>
            </div>
            <div class="row">
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">Url</label>
                  <DxTextBox v-model:value="blog.url" :disabled="true" />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-3">
                  <div class="row">
                    <div class="col-md-3">
                      <label class="form-label">Sıra</label>
                      <DxNumberBox
                        v-model:value="blog.displayOrder"
                        :show-spin-buttons="true"
                        :min="1"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-md-4">
                <div class="mb-3">
                  <label class="form-label w-100">Yayınla</label>
                  <DxSwitch
                    width="100px"
                    v-model="blog.published"
                    switchedOffText="Yayında Değil"
                    switchedOnText="Yayında"
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="mb-3">
                  <label class="form-label w-100">Aktif</label>
                  <DxSwitch
                    v-model="blog.isActive"
                    switchedOffText="Pasif"
                    switchedOnText="Aktif"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="card-footer bg-white py-3" v-if="showForm">
        <DxButton text="Kaydet" type="default" :useSubmitBehavior="true" />
        <DxButton text="Vazgeç" @click="reset" class="ms-2" />
      </div>
    </div>
  </form>
</template>

<script>
import { DxButton } from "devextreme-vue/button";
import { DxSwitch } from "devextreme-vue/switch";
import { DxNumberBox } from "devextreme-vue/number-box";
import { DxTextArea } from "devextreme-vue/text-area";
import { DxTextBox } from "devextreme-vue/text-box";
import { DxTagBox } from "devextreme-vue/tag-box";
import {
  DxHtmlEditor,
  DxToolbar,
  DxMediaResizing,
  DxItem,
} from "devextreme-vue/html-editor";
import GlobalService from "../../../services/GlobalService";
import dateFormat from "../../../infrastructure/DateFormat";
import { Endpoints } from "../../../services/Endpoints";
import AlertService from "../../../services/AlertService";

export default {
  components: {
    DxButton,
    DxSwitch,
    DxNumberBox,
    DxTextArea,
    DxTextBox,
    DxTagBox,
    DxHtmlEditor,
    DxToolbar,
    DxMediaResizing,
    DxItem,
  },
  mixins: [AlertService],
  data() {
    return {
      sizeValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"],
      fontValues: [
        "Arial",
        "Courier New",
        "Georgia",
        "Impact",
        "Lucida Console",
        "Tahoma",
        "Times New Roman",
        "Verdana",
      ],
      headerValues: [false, 1, 2, 3, 4, 5],
      title: "",
      loading: true,
      showGrid: true,
      showForm: false,
      selectedBlog: {},
      blogs: [],
      blogCategories: [],
      blog: {
        id: 0,
        url: "",
        title: "",
        description: "",
        content: "",
        published: true,
        isActive: true,
        blogCategories: [],
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Blog Düzenle";
            this.showForm = true;
            this.showGrid = false;
            this.getBlogCategories();
            GlobalService.GetByAuth(
              `${Endpoints.Admin.Blog}/${this.selectedBlog.id}`
            ).then((res) => {
              this.blog = res.data;
            });
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
                GlobalService.DeleteByAuth(Endpoints.Admin.Blog, this.selectedBlog.id)
                  .then((res) => {
                    this.getAll();
                    this.successMessage(res.data.message);
                  })
                  .catch((e) => {
                    this.errorMessage(e.response.data.message);
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
      this.loading = true;
      GlobalService.GetByAuth(Endpoints.Admin.Blog).then((res) => {
        this.blogs = res.data;
        this.loading = false;
      });
    },
    getBlogCategories() {
      GlobalService.GetByAuth(Endpoints.Admin.Lookup.BlogCategories).then((res) => {
        this.blogCategories = res.data;
      });
    },
    add() {
      this.showForm = true;
      this.showGrid = false;
      this.title = "Yeni Blog Ekle";
      this.getBlogCategories();
      this.blog = {
        id: 0,
        url: "",
        title: "",
        description: "",
        content: "",
        published: true,
        isActive: true,
        blogCategories: [],
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
    save(e) {
      e.preventDefault();
      if (this.blog.id == 0) {
        GlobalService.PostByAuth(Endpoints.Admin.Blog, this.blog)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(res.data.message);
          })
          .catch((e) => {
            this.errorMessage(e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.Blog, this.blog)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(res.data.message);
          })
          .catch((e) => {
            this.errorMessage(e.response.data.message);
          });
      }
    },
  },
};
</script>

<style lang="scss" scoped></style>
