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
            class="p-button-primary p-button-sm float-end p-button-sm"
            @click="add()"
          />
          <Button
            v-if="showForm"
            icon="pi pi-arrow-left"
            class="p-button-primary p-button-sm float-end p-button-sm"
            @click="reset()"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <div v-if="showGrid">
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
import { Endpoints } from "../../../services/Endpoints";

export default {
  data() {
    return {
      showGrid: true,
      showForm: false,
      title: "",
      users: [],
      exceptions: [],
      selectedUser: {},
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
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.title = "Kullanıcı Düzenle";
            this.showForm = true;
            this.showGrid = false;
            this.user = this.selectedUser;
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
                  Endpoints.Admin.User,
                  this.selectedUser.id
                ).then(() => {
                  this.getUsers();
                });
              },
            });
          },
        },
      ],
    };
  },
  created() {
    this.getUsers();
    this.reset();
  },
  methods: {
    getUsers() {
      GlobalService.GetByAuth(Endpoints.Admin.User).then((res) => {
        this.users = res.data;
      });
    },
    toggleGridMenu(event, data) {
      this.selectedUser = data;
      this.$refs.menu.toggle(event);
    },
    add() {
      this.reset();
      this.title = "Kullanıcı Ekle";
      this.showForm = true;
      this.showGrid = false;
    },
    save() {
      if (this.user.id == 0) {
        GlobalService.PostByAuth(Endpoints.Admin.User)
          .then(() => {
            this.getUsers();
            this.reset();
          })
          .catch((e) => {
            this.exceptions = e.response.data.exceptions;
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.User)
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
