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
            label="Ekle"
            icon="pi pi-plus"
            class="p-button-primary p-button-sm float-end p-button-sm"
            @click="add()"
          />
          <Button
            v-if="showForm"
            label="Geri"
            icon="pi pi-arrow-left"
            class="p-button-primary p-button-sm float-end p-button-sm"
            @click="reset()"
          />
        </div>
      </div>
    </template>
    <template #content>
      <div v-if="showGrid">
        <div class="border border-top-0">
          <DataTable
            showGridlines
            :value="users"
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
      <div v-if="showForm">
        <div class="row">
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Adı</label>
              <InputText
                type="text"
                v-model="user.name"
                placeholder="Adı"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Soyadı</label>
              <InputText
                type="text"
                v-model="user.surname"
                placeholder="Soyadı"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Email Adresi</label>
              <InputText
                type="email"
                v-model="user.emailAddress"
                placeholder="Email Adresi"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Tipi</label>
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
              <label class="form-label">Aktif</label>
              <div>
                <InputSwitch v-model="user.isActive" />
              </div>
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col-12">
            <div class="float-end my-3">
              <Button
                label="Kapat"
                @click="reset()"
                class="p-button-outlined p-button-secondary"
              />
              <Button class="ms-2" label="Kaydet" @click="save()" />
            </div>
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>

<script>
import userService from "../../services/UserService";

export default {
  data() {
    return {
      showGrid: true,
      showForm: false,
      title: "",
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
    this.reset();
  },
  methods: {
    getUsers() {
      userService.getAll().then((res) => {
        this.users = res.data;
      });
    },
    add() {
      this.reset();
      this.title = "Kullanıcı Ekle";
      this.showForm = true;
      this.showGrid = false;
    },
    edit(e) {
      this.title = "Kullanıcı Düzenle";
      this.showForm = true;
      this.showGrid = false;
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
            this.reset();
          })
          .catch((e) => {
            this.exceptions = e.response.data.exceptions;
          });
      } else {
        userService
          .put(this.user)
          .then(() => {
            this.getUsers();
            this.reset();
          })
          .catch((e) => {
            this.exceptions = e.response.data.exceptions;
          });
      }
    },
    reset() {
      this.user = {
        id: 0,
        name: "",
        surname: "",
        emailAddress: "",
        userType: 0,
        userTypeName: "",
        isActive: true,
      };
      this.showForm = false;
      this.showGrid = true;
      this.title = "Kullanıcılar";
    },
  },
};
</script>

<style></style>
