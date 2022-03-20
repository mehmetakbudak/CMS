<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h5>Erişim Hakkı Tanımları</h5>
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
      <page-loading :loading="loading" />
      <TabView v-model:activeIndex="activeIndex" v-if="!loading">
        <TabPanel header="Admin Menü Tanımları">
          <Tree :value="menus" class="p-3">
            <template #default="menu">
              <div class="pb-2 mb-2">
                <span class="me-2">
                  <Button
                    icon="pi pi-cog"
                    class="p-button-rounded p-button-info p-button-sm"
                    @click="toggleGridMenu($event, menu.node)"
                  />
                  <Menu ref="menu" :model="gridMenuItems" :popup="true" />
                </span>
                <span class="m-2 pb-3">
                  {{ menu.node.label }}
                </span>
              </div>
            </template>
          </Tree>
        </TabPanel>
        <TabPanel header="İşlem Yetki Tanımları">
          <Tree :value="operations" class="p-3">
            <template #default="operation">
              <div class="pb-2 mb-2">
                <span class="me-2">
                  <Button
                    icon="pi pi-cog"
                    class="p-button-rounded p-button-info p-button-sm"
                    @click="toggleGridOperation($event, operation.node)"
                  />
                  <Menu
                    ref="operation"
                    :model="gridOperationItems"
                    :popup="true"
                  />
                </span>
                <span class="m-2 pb-3">
                  {{ operation.node.label }}
                </span>
              </div>
            </template>
          </Tree>
        </TabPanel>
      </TabView>
    </div>
  </div>

  <Dialog
    :header="menuModalTitle"
    v-model:visible="displayMenuModal"
    :modal="true"
    :breakpoints="{ '760px': '90vw' }"
    :style="{ width: '30vw' }"
  >
    <page-loading :loading="modalLoading" />
    <div v-if="!modalLoading">
      <div class="mb-3">
        <label class="form-label">Bağlı Menü</label>
        <TreeSelect
          ref="parentMenu"
          v-model="parentMenu"
          :options="menus"
          placeholder="Menü seçiniz"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Adı</label>
        <InputText
          type="text"
          v-model="menu.name"
          placeholder="Menü Adı"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Url</label>
        <InputText
          type="text"
          v-model="menu.endpoint"
          placeholder="Url"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Sıra</label>
        <InputNumber
          v-model="menu.displayOrder"
          placeholder="Sıra"
          :min="1"
          :step="1"
          inputStyle="width: 100px !important; margin-left: 10px;"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Menüde Göster</label>
        <div>
          <InputSwitch v-model="menu.isShowMenu" />
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label">Aktif</label>
        <div>
          <InputSwitch v-model="menu.isActive" />
        </div>
      </div>
    </div>
    <template #footer>
      <Button
        label="Kapat"
        @click="displayMenuModal = false"
        class="p-button-outlined p-button-secondary"
      />
      <Button label="Kaydet" @click="saveMenu()" autofocus />
    </template>
  </Dialog>

  <Dialog
    :header="operationModalTitle"
    v-model:visible="displayOperationModal"
    :modal="true"
    :breakpoints="{ '760px': '90vw' }"
    :style="{ width: '30vw' }"
  >
    <page-loading :loading="modalLoading" />
    <div v-if="!modalLoading">
      <div class="mb-3">
        <label class="form-label">Bağlı İşlem Yetkisi</label>
        <TreeSelect
          ref="parentOperation"
          v-model="parentOperation"
          :options="operations"
          placeholder="İşlem seçiniz"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">İşlem Yetkisi Adı</label>
        <InputText
          type="text"
          v-model="operation.name"
          placeholder="İşlem Yetkisi Adı"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Url</label>
        <InputText
          type="text"
          v-model="operation.endpoint"
          placeholder="Url"
          class="w-100"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Metod</label>
        <Dropdown
          class="w-100"
          v-model="operation.method"
          :options="methodTypes"
          optionLabel="name"
          optionValue="name"
          placeholder="Metod seçiniz."
          :showClear="true"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Sıra</label>
        <InputNumber
          v-model="operation.displayOrder"
          placeholder="Sıra"
          :min="1"
          :step="1"
          inputStyle="width: 100px !important; margin-left: 10px;"
        />
      </div>
      <div class="mb-3">
        <label class="form-label">Aktif</label>
        <div>
          <InputSwitch v-model="operation.isActive" />
        </div>
      </div>
    </div>
    <template #footer>
      <Button
        label="Kapat"
        @click="displayOperationModal = false"
        class="p-button-outlined p-button-secondary"
      />
      <Button label="Kaydet" @click="saveOperation()" autofocus />
    </template>
  </Dialog>
</template> 

<script>
import AlertService from "../../../services/AlertService";
import { Endpoints } from "../../../services/Endpoints";
import GlobalService from "../../../services/GlobalService";

export default {
  mixins: [AlertService],
  data() {
    return {
      loading: true,
      activeIndex: 0,
      modalLoading: false,
      methodTypes: [],
      menus: [],
      operations: [],
      displayMenuModal: false,
      displayOperationModal: false,
      menuModalTitle: "",
      operationModalTitle: "",
      selectedMenu: {},
      parentMenu: {},
      selectedOperation: {},
      parentOperation: {},
      selectedTab: null,
      menu: {
        id: 0,
        parentId: null,
        name: "",
        endpoint: "",
        displayOrder: 1,
        isShowMenu: true,
        isActive: true,
      },
      operation: {
        id: 0,
        parentId: null,
        name: "",
        endpoint: "",
        displayOrder: 1,
        isShowMenu: false,
        isActive: true,
        method: "",
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.menuModalTitle = "Admin Menü Düzenle";
            this.displayMenuModal = true;
            this.modalLoading = true;
            GlobalService.GetByAuth(
              `${Endpoints.Admin.AccessRight}/${this.selectedMenu.key}`
            )
              .then((res) => {
                this.menu = res.data;
                this.parentMenu = {};
                if (this.menu.parentId) {
                  this.parentMenu[this.menu.parentId] = true;
                } else {
                  this.parentMenu = {};
                }
                this.modalLoading = false;
              })
              .catch((e) => {
                this.errorMessage(e.response.data.message);
              });
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
                  Endpoints.Admin.AccessRight,
                  this.selectedMenu.key
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
      gridOperationItems: [
        {
          label: "Düzenle",
          command: () => {
            this.operationModalTitle = "İşlem Yetkisi Düzenle";
            this.displayOperationModal = true;
            this.modalLoading = true;
            this.getMethodTypes();
            GlobalService.GetByAuth(
              `${Endpoints.Admin.AccessRight}/${this.selectedOperation.key}`
            )
              .then((res) => {
                this.operation = res.data;
                this.parentOperation = {};
                if (this.operation.parentId) {
                  this.parentOperation[this.operation.parentId] = true;
                } else {
                  this.parentOperation = {};
                }
                this.modalLoading = false;
              })
              .catch((e) => {
                this.errorMessage(e.response.data.message);
              });
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
                  Endpoints.Admin.AccessRight,
                  this.selectedOperation.id
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
    this.getAll();
  },
  methods: {
    getAll() {
      this.loading = true;
      GlobalService.GetByAuth(Endpoints.Admin.AccessRight).then((res) => {
        this.menus = res.data.menuAccessRights;
        this.operations = res.data.operationAccessRights;
        this.loading = false;
      });
    },
    getMethodTypes() {
      GlobalService.GetByAuth(`${Endpoints.Admin.Lookup.MethodTypes}`).then(
        (res) => {
          this.methodTypes = res.data;
        }
      );
    },
    add() {
      if (this.activeIndex == 0) {
        this.displayMenuModal = true;
        this.menuModalTitle = "Admin Menü Ekle";
        this.parentMenu = {};
        this.menu = {
          id: 0,
          parentId: null,
          name: "",
          endpoint: "",
          displayOrder: 1,
          isShowMenu: true,
          isActive: true,
        };
      } else {
        this.displayOperationModal = true;
        this.operationModalTitle = "İşlem Yetkisi Ekle";
        this.getMethodTypes();
        this.parentOperation = {};
        this.operation = {
          id: 0,
          parentId: null,
          name: "",
          endpoint: "",
          displayOrder: 1,
          isShowMenu: false,
          isActive: true,
          method: "",
        };
      }
    },
    toggleGridMenu(event, data) {
      this.selectedMenu = data;
      this.$refs.menu.toggle(event);
    },
    toggleGridOperation(event, data) {
      this.selectedOperation = data;
      this.$refs.operation.toggle(event);
    },
    saveMenu() {
      if (this.$refs.parentMenu.selectedNodes.length > 0) {
        this.menu.parentId = this.$refs.parentMenu.selectedNodes[0].key;
      } else {
        this.menu.parentId = null;
      }
      if (this.menu.id === 0) {
        GlobalService.PostByAuth(
          `${Endpoints.Admin.AccessRight}/Menu`,
          this.menu
        )
          .then((res) => {
            this.getAll();
            this.activeIndex = 0;
            this.successMessage(this, res.data.message);
            this.displayMenuModal = false;
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(
          `${Endpoints.Admin.AccessRight}/Menu`,
          this.menu
        )
          .then((res) => {
            this.getAll();
            this.activeIndex = 0;
            this.successMessage(this, res.data.message);
            this.displayMenuModal = false;
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      }
    },
    saveOperation() {
      if (this.$refs.parentOperation.selectedNodes.length > 0) {
        this.operation.parentId =
          this.$refs.parentOperation.selectedNodes[0].key;
      } else {
        this.operation.parentId = null;
      }
      if (this.operation.id === 0) {
        GlobalService.PostByAuth(
          `${Endpoints.Admin.AccessRight}/Operation`,
          this.operation
        )
          .then((res) => {
            this.getAll();
            this.activeIndex = 1;
            this.successMessage(this, res.data.message);
            this.displayOperationModal = false;
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(
          `${Endpoints.Admin.AccessRight}/Operation`,
          this.operation
        )
          .then((res) => {
            this.getAll();
            this.activeIndex = 1;
            this.successMessage(this, res.data.message);
            this.displayOperationModal = false;
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      }
    },
  },
};
</script>

<style>
</style>