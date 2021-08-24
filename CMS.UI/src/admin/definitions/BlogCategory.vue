<template>
  <div id="name">
    <div class="row mb-2">
      <div class="col-6">
        <h5>Blog Kategorileri</h5>
      </div>
      <div class="col-6">
        <Button
          label="Ekle"
          icon="pi pi-plus"
          class="p-button-primary p-button-sm float-end"
          @click="add()"
        />
      </div>
    </div>
    <div class="border border-top-0">
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
        <Column header="" class="w-120">
          <template #body="slotProps">
            <Button
              icon="pi pi-pencil"
              class="p-button-rounded p-button-info p-button-sm"
              @click="edit(slotProps.data)"
            />
            <Button
              icon="pi pi-trash"
              class="p-button-rounded p-button-danger p-button-sm ms-2"
              @click="remove($event, slotProps.data)"
            />
          </template>
        </Column>
        <Column field="name" header="Kategori Adı"></Column>
        <Column field="url" header="Url"></Column>
        <Column field="isShowHomePage" header="Anasayfada Gösterilsin">
          <template #body="slotProps">
            <div>
              {{ slotProps.data.isShowHomePage ? "Evet" : "Hayır" }}
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

    <Dialog
      :header="modalTitle"
      v-model:visible="displayModal"
      :modal="true"
      :breakpoints="{ '960px': '95vw' }"
      :style="{ width: '50vw' }"
    >
      <div>
        <Message v-for="item of exceptions" severity="error" :key="item">{{
          item
        }}</Message>
      </div>
      <div class="mb-3">
        <label>Adı</label>
        <InputText
          type="text"
          v-model="user.name"
          placeholder="Adı"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label>Soyadı</label>
        <InputText
          type="text"
          v-model="user.surname"
          placeholder="Soyadı"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label>Email Adresi</label>
        <InputText
          type="email"
          v-model="user.emailAddress"
          placeholder="Email Adresi"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label>Tipi</label>
        <Dropdown
          class="w-100"
          v-model="user.userType"
          :options="userTypes"
          optionLabel="name"
          optionValue="value"
          placeholder="Kullanıcı tipi seçiniz."
        />
      </div>
      <div class="mb-3">
        <label>Aktif</label>
        <div>
          <InputSwitch v-model="user.isActive" />
        </div>
      </div>
      <template #footer>
        <Button
          label="Kapat"
          @click="displayModal = false"
          class="p-button-outlined p-button-secondary"
        />
        <Button label="Kaydet" @click="save()" autofocus />
      </template>
    </Dialog>
  </div>
</template>

<script>
import blogCategoryService from "../../services/BlogCategoryService";

export default {
  name: "name",
  data() {
    return {
      blogCategories: [],
      exceptions: [],
      displayModal: false,
      modalTitle: "",
      todoStatus: {
        id: 0,
        title: "",
      },
    };
  },
  created() {
    this.getBlogCategories();
  },
  methods: {
    getBlogCategories() {
      blogCategoryService.getAll().then((res) => {
        this.blogCategories = res.data;
      });
    },
    add() {
      this.displayModal = true;
      this.modalTitle = "Yeni Blog Kategorisi Ekle";
    },
    edit() {},
    remove() {},
  },
};
</script>
