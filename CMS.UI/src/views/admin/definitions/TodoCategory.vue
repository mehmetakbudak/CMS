<template>
  <Card>
    <template #title>
      <div class="row mb-2">
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
    </template>
    <template #content>
      <div class="border border-top-0" v-if="showGrid">
        <DataTable
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
    </template>
  </Card>
</template>

<script>
import { Endpoints } from "../../../services/Endpoints";
import GlobalService from "../../../services/GlobalService";

export default {
  name: "name",
  data() {
    return {
      showGrid: true,
      showForm: false,
      todoCategories: [],
      exceptions: [],
      selectedTodoCategory: {},
      title: "",
      todoCategory: {
        id: 0,
        title: "",
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
    this.getTodoCategories();
    this.reset();
  },
  methods: {
    getTodoCategories() {
      GlobalService.GetByAuth(Endpoints.Admin.TodoCategory).then((res) => {
        this.todoCategories = res.data;
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
      this.todoCategory = {};
    },
    reset() {
      this.showForm = false;
      this.showGrid = true;
      this.title = "Yapılacak Kategorileri";
    },
  },
};
</script>
