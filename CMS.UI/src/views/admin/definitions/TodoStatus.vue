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
          :value="todoStatuses"
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
          <Column field="todoCategory.name" header="Kategori Adı"></Column>
          <Column field="name" header="Durum Adı"></Column>
          <Column field="displayOrder" header="Sıra"></Column>
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
              <label class="form-label">Yapılacak Durum Adı</label>
              <InputText
                type="text"
                v-model="todoStatus.name"
                placeholder="Yapılacak Durum Adı"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Yapılacak Kategori Adı</label>
              <Dropdown
                class="w-100"
                v-model="todoStatus.todoCategoryId"
                :options="todoCategories"
                optionLabel="name"
                optionValue="id"
                placeholder="Yapılacak Kategori seçiniz."
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Sıra</label>
              <InputNumber
                v-model="todoStatus.displayOrder"
                placeholder="Sıra"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Aktif</label>
              <div>
                <InputSwitch v-model="todoStatus.isActive" />
              </div>
            </div>
          </div>
        </div>
        <div class="footer-button">
          <Button label="Kaydet" @click="save()" />
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
      todoStatuses: [],
      todoCategories: [],
      showGrid: true,
      showForm: false,
      title: "",
      selectedTodoStatus: {},
      todoStatus: {
        id: 0,
        todoCategoryId: 0,
        title: "",
        displayOrder: 0,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Yapılacak Kategorileri Düzenle";
            this.showForm = true;
            this.showGrid = false;
            this.todoStatus = this.selectedTodoStatus;
            this.getTodoCategories();
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
                  Endpoints.Admin.TodoStatus,
                  this.selectedTodoStatus.id
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
    this.getTodoStatuses();
    this.reset();
  },
  methods: {
    getTodoStatuses() {
      GlobalService.GetByAuth(Endpoints.Admin.TodoStatus).then((res) => {
        this.todoStatuses = res.data;
      });
    },
    getTodoCategories() {
      GlobalService.GetByAuth(Endpoints.Lookup.TodoCategories).then((res) => {
        this.todoCategories = res.data;
      });
    },
    add() {
      this.showGrid = false;
      this.showForm = true;
      this.title = "Yeni Yapılacak Durumu Ekle";
      this.todoStatus = {};
    },
    toggleGridMenu(event, data) {
      this.selectedTodoStatus = data;
      this.$refs.menu.toggle(event);
    },
    reset() {
      this.showForm = false;
      this.showGrid = true;
      this.title = "Yapılacak Durumları";
    },
  },
};
</script>
