<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h4>Admin Menü</h4>
        </div>
        <div class="col-6">
          <Button
            icon="pi pi-plus"
            class="p-button-primary p-button-sm float-end p-button-sm"
            @click="add()"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <Tree :value="menus" class="p-3">
        <template #default="menu">
          <div class="pb-2 mb-2">
            <span>
              <Button
                @click="edit(menu.node)"
                class="p-button-secondary p-button-sm"
                icon="pi pi-pencil"
              />
            </span>
            <span class="m-2">
              <Button
                @click="remove(menu.node)"
                class="p-button-danger p-button-sm"
                icon="pi pi-trash"
              />
            </span>
            <span class="m-2 pb-3">
              {{ menu.node.label }}
            </span>
          </div>
        </template>
      </Tree>
    </div>
  </div>

  <Dialog
    :header="modalTitle"
    v-model:visible="displayModal"
    :modal="true"
    :breakpoints="{ '760px': '90vw' }"
    :style="{ width: '40vw' }"
  >
    <div class="mb-3">
      <label>Bağlı Menü</label>
      <TreeSelect
        v-model="accessRight.parentId"
        :options="menus"
        placeholder="Menü seçiniz"
        class="w-100"
      />
    </div>
    <div class="mb-3">
      <label>Adı</label>
      <InputText
        type="text"
        v-model="accessRight.name"
        placeholder="Menü Adı"
        class="w-100"
      />
    </div>
    <div class="mb-3">
      <label>Url</label>
      <InputText
        type="text"
        v-model="accessRight.endpoint"
        placeholder="Url"
        class="w-100"
      />
    </div>
    <div class="mb-3">
      <label>Sıra</label>
      <InputNumber
        v-model="accessRight.order"
        placeholder="Sıra"
        :min="1"
        :step="1"
        class="w-100"
      />
    </div>
    <div class="mb-3">
      <label>Menüde Göster</label>
      <div>
        <InputSwitch v-model="accessRight.isShowMenu" />
      </div>
    </div>
    <div class="mb-3">
      <label>Aktif</label>
      <div>
        <InputSwitch v-model="accessRight.isActive" />
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
import { Endpoints } from "../../../../services/Endpoints";
import GlobalService from "../../../../services/GlobalService";

export default {
  data() {
    return {
      menus: [],
      displayModal: false,
      modalTitle: "",
      accessRight: {
        id: 0,
        parentId: null,
        name: "",
        endpoint: "",
        order: 1,
        isShowMenu: true,
        isActive: true,
      },
    };
  },
  created() {
    this.getMenus();
  },
  methods: {
    getMenus() {
      GlobalService.GetByAuth(Endpoints.Admin.AccessRight).then((res) => {
        this.menus = res.data.menuAccessRights;
      });
    },
    add() {
      this.displayModal = true;
      this.modalTitle = "Admin Menü Ekle";
    },
    edit(e) {
      console.log(e);
    },
    remove(e) {
      console.log(e);
    },
    save() {
      // if (this.accessRight.id == 0) {
      // } else {
      // }
    },
  },
};
</script>

<style>
</style>