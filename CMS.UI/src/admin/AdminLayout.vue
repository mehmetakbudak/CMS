<template>
  <div class="container-fluid">
    <div class="card bg-pi">
      <div class="card-body">
        <router-link
          class="h5 text-secondary text-decoration-none mr-3"
          to="/admin"
          style="padding-right: 30px"
          >CMS Admin Paneli</router-link
        >
        <div class="float-end">
          <Button
            class="me-3 p-button-sm"
            icon="pi pi-th-large"
            @click="visibleLeft = true"
            label="Menü"
          />
          <Button
            type="button"
            icon="pi pi-user"
            class="p-button-rounded p-button-primary p-button-sm"
            :label="currentUser?.fullName"
            @click="toggleRightMenu"
          />
          <Menu ref="menu" :model="rightMenuItems" :popup="true" />
        </div>
      </div>
    </div>
    <Sidebar v-model:visible="visibleLeft" :baseZIndex="1000">
      <h3>Menü</h3>
      <div>
        <Tree :value="leftMenu" :filter="true" filterMode="lenient">
          <template #default="menu">
            <a
              class="text-dark text-decoration-none"
              v-if="menu.node.to == null"
              >{{ menu.node?.label }}</a
            >
            <router-link
              v-if="menu.node.to != null"
              class="text-dark text-decoration-none"
              :to="menu.node?.to"
            >
              {{ menu.node?.label }}
            </router-link>
          </template>
        </Tree>
      </div>
    </Sidebar>
    <div class="mt-3">
      <div class="container-fluid">
        <div class="row">
          <div class="col-md-3 border">
            <Tree
              class="p-2"
              :value="leftMenu"
              :filter="true"
              filterMode="lenient"
            >
              <template #default="menu">
                <a
                  class="text-dark text-decoration-none"
                  v-if="menu.node.to == null"
                  >{{ menu.node?.label }}</a
                >
                <router-link
                  v-if="menu.node.to != null"
                  class="text-dark text-decoration-none"
                  :to="menu.node?.to"
                >
                  {{ menu.node?.label }}
                </router-link>
              </template>
            </Tree>
          </div>
          <div class="col-md-9 pe-0">
            <div class="">
              <router-view></router-view>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import authHeader from "../services/AuthHeader";

export default {
  data() {
    return {
      currentUser: "",
      visibleLeft: false,
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
            this.$router.push("/giris");
          },
        },
      ],
    };
  },
  created() {
    this.loadMenu();
    if (window.innerWidth > 768) {
      this.currentUser = this.$store.state.auth.user;
    }
  },
  methods: {
    toggleRightMenu(event) {
      this.$refs.menu.toggle(event);
    },
    loadMenu() {
      this.axios
        .get(process.env.VUE_APP_BASEURL + "Menu/Backend", {
          headers: authHeader(),
        })
        .then((res) => {
          this.leftMenu = res.data;
        });
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