<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h5>Ön Arayüz Menü Tanımları</h5>
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
      <div v-if="!loading">
        <Tree :value="menus" class="p-3">
          <template #default="menu">
            <div class="pb-2 mb-2">
              <span class="me-2 p-0">
                <Button
                  icon="pi pi-cog"
                  class="p-button-rounded p-button-info p-button-sm"
                  @click="toggleGridMenu($event, menu.node)"
                />
                <Menu ref="menu" :model="menuItems" :popup="true" />
              </span>
              <span class="m-2 pb-3">
                {{ menu.node.label }}
              </span>
            </div>
          </template>
        </Tree>
      </div>
    </div>
  </div>

  <Dialog
    :header="modalTitle"
    v-model:visible="displayModal"
    :modal="true"
    :breakpoints="{ '760px': '90vw' }"
    :style="{ width: '30vw' }"
  >
    <div class="mb-3">
      <label class="form-label">Bağlı Menü</label>
      <TreeSelect
        selectionMode="single"
        v-model="parentMenu"
        ref="parentMenu"
        :options="menus"
        placeholder="Menü seçiniz"
        class="w-100"
      />
    </div>
    <div class="mb-3">
      <label class="form-label">Adı</label>
      <InputText
        type="text"
        v-model="menu.label"
        placeholder="Menü Adı"
        class="w-100"
      />
    </div>
    <div class="mb-3">
      <label class="form-label">Url</label>
      <InputText
        type="text"
        v-model="menu.to"
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
      <label class="form-label">Aktif</label>
      <div>
        <InputSwitch v-model="menu.isActive" />
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
import PageLoading from '../../../components/PageLoading.vue';
import AlertService from "../../../services/AlertService";
import { Endpoints } from "../../../services/Endpoints";
import GlobalService from "../../../services/GlobalService";
export default {
  components: { PageLoading },
  mixins: [AlertService],
  data() {
    return {
      menus: [],
      loading: true,
      modalTitle: "",
      displayModal: false,
      selectedMenu: {},
      parentMenu: {},
      menu: {
        key: 0,
        parentId: {},
        label: "",
        to: "",
        displayOrder: 0,
        isActive: true,
        children: [],
      },
      menuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.modalTitle = "Admin Menü Düzenle";
            this.displayModal = true;
            this.menu = this.selectedMenu;
            this.parentMenu = {};
            if (this.selectedMenu.parentId) {
              this.parentMenu[this.selectedMenu.parentId] = true;
            } else {
              this.parentMenu = {};
            }
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
                  `${Endpoints.Admin.Menu}/FrontendMenu`,
                  this.selectedMenu.key
                )
                  .then((res) => {
                    this.getAll();
                    this.successMessage( res.data.message);
                  })
                  .catch((e) => {
                    this.errorMessage( e.response.data.message);
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
      GlobalService.GetByAuth(`${Endpoints.Admin.Menu}/FrontendMenu`).then(
        (res) => {
          this.loading = false;
          this.menus = res.data;
        }
      );
    },
    add() {
      this.displayModal = true;
      this.modalTitle = "Ön Arayüz Menü Ekle";
      this.parentMenu = {};
      this.menu = {
        key: 0,
        parentId: {},
        label: "",
        to: "",
        displayOrder: 0,
        isActive: true,
        children: [],
      };
    },
    toggleGridMenu(event, data) {
      this.selectedMenu = data;
      this.$refs.menu.toggle(event);
    },
    save() {
      if (this.$refs.parentMenu.selectedNodes.length > 0) {
        this.menu.parentId = this.$refs.parentMenu.selectedNodes[0].key;
      } else {
        this.menu.parentId = null;
      }
      if (this.menu.key === 0) {
        GlobalService.PostByAuth(
          `${Endpoints.Admin.Menu}/FrontendMenu`,
          this.menu
        )
          .then((res) => {
            this.getAll();
            this.successMessage( res.data.message);
            this.displayModal = false;
          })
          .catch((e) => {
            this.errorMessage( e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(
          `${Endpoints.Admin.Menu}/FrontendMenu`,
          this.menu
        )
          .then((res) => {
            this.getAll();
            this.successMessage( res.data.message);
            this.displayModal = false;
          })
          .catch((e) => {
            this.errorMessage( e.response.data.message);
          });
      }
    },
  },
};
</script>