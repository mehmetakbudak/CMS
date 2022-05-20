<template>
  <div class="card" v-if="showGrid">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6"><h3>Kullanıcılar</h3></div>
        <div class="col-6">
          <DxButton
            v-if="showGrid"
            class="float-end"
            type="default"
            icon="plus"
            @click="add"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <div class="my-3">
        <DxDataGrid
          :showRowLines="true"
          :show-borders="true"
          :data-source="users"
          :allow-column-resizing="true"
          :column-auto-width="true"
          :loadPanel="{ showIndicator: false, showPane: false, text: '' }"
          key-expr="id"
          height="calc(100vh - )"
          no-data-text="Kayıt bulunamadı."
        >
          <DxPaging :page-size="10" />
          <DxScrolling mode="standard" row-rendering-mode="standard" />
          <DxColumnFixing :enabled="true" />
          <DxHeaderFilter :visible="true" />
          <DxFilterRow :visible="true" apply-filter="auto" />
          <DxColumn
            :allowFiltering="false"
            :allow-sorting="false"
            data-field="id"
            caption=""
            width="70"
            alignment="center"
            cell-template="cellTemplate"
          />
          <DxColumn data-field="name" caption="Adı" />
          <DxColumn data-field="surname" caption="Soyadı" />
          <DxColumn data-field="emailAddress" caption="Email Adresi" />
          <DxColumn data-field="userTypeName" caption="Kullanıcı Tipi" />
          <DxColumn data-field="status" caption="Durumu" />
          <DxColumn data-field="isActive" caption="Aktif" />
          <template #cellTemplate="{ data }">
            <DxDropDownButton
              :items="menuItems"
              :drop-down-options="{ width: 100 }"
              icon="preferences"
              @item-click="onItemClick($event, data)"
            />
          </template>
        </DxDataGrid>
      </div>
    </div>
  </div>
  <form form @submit="save">
    <div class="card" v-if="showForm">
      <div class="card-header bg-white py-3">
        <div class="row">
          <div class="col-6">
            <h3>{{ title }}</h3>
          </div>
          <div class="col-6">
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
        <div class="row">
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Adı</label>
              <DxTextBox v-model:value="user.name" mode="text" placeholder="Adı">
                <DxValidator>
                  <DxRequiredRule message="Adı gereklidir." />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Soyadı</label>
              <DxTextBox v-model:value="user.surname" mode="text" placeholder="Soyadı">
                <DxValidator>
                  <DxRequiredRule message="Soyadı gereklidir." />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Email Adresi</label>
              <DxTextBox
                v-model:value="user.emailAddress"
                mode="text"
                placeholder="Email Adresi"
              >
                <DxValidator>
                  <DxRequiredRule message="Email Adresi gereklidir." />
                  <DxEmailRule message="Email adresi geçersiz." />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Kullanıcı Tipi</label>
              <DxSelectBox
                v-model:value="user.userType"
                :data-source="userTypes"
                display-expr="name"
                value-expr="value"
                placeholder="Kullanıcı tipi seçiniz."
              >
                <DxValidator>
                  <DxRequiredRule message="Kullanıcı Tipi gereklidir." />
                </DxValidator>
              </DxSelectBox>
            </div>
          </div>
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">Aktif</label>
              <div>
                <InputSwitch v-model="user.isActive" />
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="card-footer bg-white py-3">
        <DxButton text="Kaydet" :useSubmitBehavior="true" type="default" />
        <DxButton text="Vazgeç" @click="reset" class="ms-2" />
      </div>
    </div>
  </form>
</template>

<script>
import { DxSelectBox } from "devextreme-vue/select-box";
import { DxValidator, DxRequiredRule, DxEmailRule } from "devextreme-vue/validator";
import { DxTextBox } from "devextreme-vue/text-box";
import { DxDropDownButton } from "devextreme-vue/drop-down-button";
import {
  DxDataGrid,
  DxPaging,
  DxScrolling,
  DxColumnFixing,
  DxHeaderFilter,
  DxFilterRow,
  DxColumn,
} from "devextreme-vue/data-grid";
import { DxButton } from "devextreme-vue/button";
import GlobalService from "../../../services/GlobalService";
import { Endpoints } from "../../../services/Endpoints";
import { Constants } from "../../../models/Constants";
import AlertService from "../../../services/AlertService";

export default {
  mixins: [AlertService],
  components: {
    DxSelectBox,
    DxValidator,
    DxRequiredRule,
    DxEmailRule,
    DxTextBox,
    DxDropDownButton,
    DxButton,
    DxDataGrid,
    DxPaging,
    DxScrolling,
    DxColumnFixing,
    DxHeaderFilter,
    DxFilterRow,
    DxColumn,
  },
  data() {
    return {
      loading: true,
      showGrid: true,
      showForm: false,
      title: "",
      users: [],
      exceptions: [],
      selectedUser: {},
      userTypes: Constants.UserTypes,
      menuItems: [
        { key: "edit", text: "Düzenle" },
        { key: "delete", text: "Sil" },
      ],
      user: {
        id: 0,
        name: "",
        surname: "",
        emailAddress: "",
        userType: 0,
        isActive: true,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {},
        },
        {
          label: "Sil",
          command: () => {},
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
      this.loading = true;
      GlobalService.GetByAuth(Endpoints.Admin.User).then((res) => {
        this.users = res.data;
        this.loading = false;
      });
    },
    onItemClick(e, item) {
      if (e.itemData.key == "edit") {
        this.title = "Kullanıcı Düzenle";
        this.showForm = true;
        this.showGrid = false;
        this.user = item.data;
      } else if (e.itemData.key == "delete") {
        this.$confirm.require({
          message: "Silmek istediğinize emin misiniz?",
          header: "Silme Onayı",
          icon: "pi pi-exclamation-triangle",
          acceptLabel: "Evet",
          rejectLabel: "Hayır",
          accept: () => {
            GlobalService.DeleteByAuth(Endpoints.Admin.User, this.selectedUser.id).then(
              () => {
                this.getUsers();
              }
            );
          },
        });
      }
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
    save(e) {
      e.preventDefault();
      if (this.user.id == 0) {
        GlobalService.PostByAuth(Endpoints.Admin.User, this.user)
          .then((res) => {
            this.getUsers();
            this.reset();
            this.successMessage(res.data.message);
          })
          .catch((e) => {
            this.errorMessage(e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.User, this.user)
          .then((res) => {
            this.getUsers();
            this.reset();
            this.successMessage(res.data.message);
          })
          .catch((e) => {
            this.errorMessage(e.response.data.message);
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
