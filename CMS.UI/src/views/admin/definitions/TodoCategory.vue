<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h5>{{ title }}</h5>
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
          :loading="loading"
          showGridlines
          :value="todoCategories"
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
                v-model="todoCategory.name"
                placeholder="Kategori Adı"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Aktif</label>
              <div>
                <InputSwitch v-model="todoCategory.isActive" />
              </div>
            </div>
          </div>
        </div>
        <div class="footer-button">
          <Button label="Kaydet" @click="save()" autofocus />
          <Button
            label="Vazgeç"
            @click="reset()"
            class="ms-2 p-button-outlined p-button-secondary"
          />
        </div>
      </div>
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
      loading: false,
      showGrid: true,
      showForm: false,
      todoCategories: [],
      exceptions: [],
      selectedTodoCategory: {},
      title: "",
      todoCategory: {
        id: 0,
        title: "",
        isActive: true,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Yapılacak Kategorileri Düzenle";
            this.showForm = true;
            this.showGrid = false;
            this.todoCategory = this.selectedTodoCategory;
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
                  Endpoints.Admin.TodoCategory,
                  this.selectedTodoCategory.id
                )
                  .then((res) => {
                    this.getAll();
                    this.successMessage( res.data.message);
                  })
                  .catch((e) => {
                    this.errorMessage( e.response.data.message);
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
      GlobalService.GetByAuth(Endpoints.Admin.TodoCategory).then((res) => {
        this.todoCategories = res.data;
        this.loading = false;
      });
    },
    toggleGridMenu(event, data) {
      this.selectedTodoCategory = data;
      this.$refs.menu.toggle(event);
    },
    add() {
      this.showGrid = false;
      this.showForm = true;
      this.title = "Yeni Yapılacak Kategorisi Ekle";
      this.todoCategory = {
        id: 0,
        title: "",
        isActive: true,
      };
    },
    reset() {
      this.showForm = false;
      this.showGrid = true;
      this.title = "Yapılacak Kategorileri";
    },
    save() {
      if (this.todoCategory.id == 0) {
        GlobalService.PostByAuth(
          Endpoints.Admin.TodoCategory,
          this.todoCategory
        )
          .then((res) => {
            this.successMessage( res.data.message);
            this.reset();
            this.getAll();
          })
          .catch((e) => {
            this.errorMessage( e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(
          Endpoints.Admin.TodoCategory,
          this.todoCategory
        )
          .then((res) => {
            this.successMessage( res.data.message);
            this.reset();
            this.getAll();
          })
          .catch((e) => {
            this.errorMessage( e.response.data.message);
          });
      }
    },
  },
};
</script>
