<template>
  <nav class="navbar navbar-light bg-light border fixed-top">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/admin">CMS Admin Paneli</router-link>
      <div class="d-flex">
        <div class="float-end">
          <Button
            class="me-3 p-button-rounded p-button-primary p-button-outlined p-button-sm"
            icon="pi pi-th-large"
            @click="visibleLeft = true"
            v-if="windowWidth < 768"
            :label="windowWidth > 768 ? 'Menü' : ''"
          />
          <Button
            :label="currentUser?.fullName"
            @click="toggleRightMenu"
            class="p-button-rounded p-button-primary p-button-outlined p-button-sm"
            icon="pi pi-user"
          />
          <Menu ref="menu" :model="rightMenuItems" :popup="true" />
        </div>
      </div>
    </div>
  </nav>
  <div class="container-fluid position-relative" style="top: 50px">
    <Sidebar v-model:visible="visibleLeft" :baseZIndex="10000">
      <h3>Menü</h3>
      <div>
        <Tree :value="leftMenu" :filter="true" filterMode="lenient">
          <template #default="menu">
            <a class="text-dark text-decoration-none" v-if="menu.node.to == null">{{
              menu.node?.label
            }}</a>
            <router-link
              v-if="menu.node.to != null"
              class="text-dark text-decoration-none"
              :to="menu.node?.to"
              @click="selectTree"
            >
              {{ menu.node?.label }}
            </router-link>
          </template>
        </Tree>
      </div>
    </Sidebar>
    <div class="mt-3">
      <div class="container-fluid px-0 px-md-2">
        <div class="row">
          <div class="col-md-3 mb-3" v-if="windowWidth > 768">
            <div class="card">
              <div class="card-body">
                <DxTreeView
                  class="py-2"
                  :items="leftMenu"
                  itemsExpr="items"
                  :search-enabled="true"
                  searchExpr="title"
                  search-mode="contains"
                  item-template="left-menu-template"
                >
                  <DxSearchEditorOptions placeholder="Menüde Ara..." />
                  <template #left-menu-template="leftMenuItem">
                    <div v-if="leftMenuItem.data.to != null">
                      <router-link
                        class="text-dark text-decoration-none"
                        :to="leftMenuItem?.data?.to"
                        >{{ leftMenuItem?.data?.title }}</router-link
                      >
                    </div>
                    <div v-if="leftMenuItem.data.to == null">
                      {{ leftMenuItem?.data?.title }}
                    </div>
                  </template>
                </DxTreeView>
              </div>
            </div>
          </div>
          <div class="col-md-9">
            <div class="mb-3">
              <router-view></router-view>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
import DxTreeView, { DxSearchEditorOptions } from "devextreme-vue/tree-view";

export default {
  components: {
    DxTreeView,
    DxSearchEditorOptions,
  },
  data() {
    return {
      currentUser: "",
      visibleLeft: false,
      windowWidth: 0,
      leftMenu: [],
      rightMenuItems: [
        {
          label: "Siteye Git",
          to: "/",
        },
        {
          label: "Çıkış Yap",
          command: () => {
            this.$store.dispatch("auth/logout");
            this.$router.push("/");
          },
        },
      ],
    };
  },
  created() {
    this.loadMenu();
    this.windowWidth = window.innerWidth;
    if (this.windowWidth > 768) {
      this.currentUser = this.$store.state.auth.user;
    }
  },
  methods: {
    toggleRightMenu(event) {
      this.$refs.menu.toggle(event);
    },
    loadMenu() {
      GlobalService.GetByAuth(`${Endpoints.Admin.Menu}/GetUserMenu`).then((res) => {
        this.leftMenu = res.data;
      });
    },
    selectTree() {
      this.visibleLeft = false;
    },
  },
};
</script>

<style scoped>
::v-deep(.p-tree .p-tree-container .p-treenode .p-treenode-content) {
  padding: unset;
}
::v-deep(.p-tree) {
  padding: unset;
  border: unset;
}
.bg-pi {
  background: #efefef !important;
}
</style>
