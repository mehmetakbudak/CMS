<template>
  <div>
    <div class="row mb-2">
      <div class="col-6">
        <h5>Kullanıcılar</h5>
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
        :value="users"
        :paginator="true"
        :rows="5"
        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
        :rowsPerPageOptions="[5, 10, 20, 50]"
        responsiveLayout="scroll"
        currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
      >
        <Column header="">
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
        <Column field="name" header="Adı"></Column>
        <Column field="surname" header="Soyadı"></Column>
        <Column field="emailAddress" header="Email Adresi"></Column>
        <Column field="userTypeName" header="Kullanıcı Tipi"></Column>
        <Column field="isActive" header="Aktif">
          <template #body="slotProps">
            <div>
              {{ slotProps.data.isActive ? "Aktif" : "Pasif" }}
            </div>
          </template>
        </Column>
      </DataTable>
    </div>
  </div>
  <Dialog
    :header="modalTitle"
    v-model:visible="displayModal"
    :modal="true"
    :breakpoints="{ '960px': '75vw' }"
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
</template>

<script>
import userService from "../services/UserService";

export default {
  data() {
    return {
      displayModal: false,
      modalTitle: "",
      users: [],
      exceptions: [],
      userTypes: [
        {
          value: 1,
          name: "Süper Admin",
        },
        {
          value: 2,
          name: "Admin",
        },
        {
          value: 3,
          name: "Kullanıcı",
        },
      ],
      user: {
        id: 0,
        name: "",
        surname: "",
        emailAddress: "",
        userType: 0,
        userTypeName: "",
        isActive: true,
      },
    };
  },
  created() {
    this.getUsers();
  },
  methods: {
    getUsers() {
      userService.getAll().then((res) => {
        this.users = res.data;
      });
    },
    add() {
      this.modalTitle = "Kullanıcı Ekle";
      this.displayModal = true;
      this.user = {
        id: 0,
        name: "",
        surname: "",
        emailAddress: "",
        userType: 0,
        userTypeName: "",
        isActive: true,
      };
    },
    edit(e) {
      this.modalTitle = "Kullanıcı Düzenle";
      this.displayModal = true;
      this.user = e;
    },
    remove(e, data) {
      this.$confirm.require({
        target: e.currentTarget,
        message: "Silmek istediğinize emin misiniz?",
        icon: "pi pi-exclamation-triangle",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        accept: () => {
          userService.delete(data.id).then(() => {
            this.getUsers();
          });
        },
      });
    },
    save() {
      if (this.user.id == 0) {
        userService
          .post(this.user)
          .then(() => {
            this.getUsers();
            this.displayModal = false;
            // this.alertSave();
          })
          .catch((e) => {
            this.exceptions = e.response.data.exceptions;
          });
      } else {
        userService
          .put(this.user)
          .then(() => {
            this.getUsers();
            this.displayModal = false;
            // this.alertSave();
          })
          .catch((e) => {
            this.exceptions = e.response.data.exceptions;
          });
      }
    },
  },
};
</script>

<style>
</style>