<template>
  <Panel header="Admin Menü">
    <template #icons>
      <button class="p-panel-header-icon p-link p-mr-2" @click="add">
        <span class="pi pi-plus"></span>
      </button>
    </template>
    <div>
      <Tree :value="menus">
        <template #default="menu">
          <div class="border-bottom pb-2 mb-2">
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
            <span class="m-2">
              {{ menu.node.label }}
            </span>
          </div>
        </template>
      </Tree>
    </div>
  </Panel>

  <Dialog
    :header="modalTitle"
    v-model:visible="displayModal"
    :modal="true"
    :breakpoints="{ '960px': '75vw' }"
    :style="{ width: '50vw' }"
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
import AccessRightService from "../services/AccessRightService";

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
      AccessRightService.get().then((res) => {
        this.menus = res.data.menuAccessRights;
      });
    },
    add() {
      this.displayModal = true;
      this.modalTitle = "Admin Menü Ekle";
    },
    edit(e) {},
    remove(e) {},
    save() {
      if (this.accessRight.id == 0) {
      } else {
      }
    },
  },
};
</script>

<style>
</style>