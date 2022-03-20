<template>
  <nav
    class="navbar navbar-expand-lg navbar-light bg-white mt-2 border"
    style="border-radius: 5px"
  >
    <div class="container-fluid">
      <div class="w-100">
        <Menubar :model="menuDatasource" class="py-0 text-dark">
          <template #start>
            <router-link class="navbar-brand" to="/">CMS</router-link>
          </template>
          <template #end>
            <Button
              type="button"
              icon="pi pi-user"
              class="p-button-rounded p-button-primary"
              @click="toggleRightMenu"
            />
            <Menu ref="menu" :model="rightMenuItems" :popup="true" />
          </template>
        </Menubar>
      </div>
    </div>
  </nav>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
export default {
  data() {
    return {
      menuDatasource: [],
      rightMenuItems: [],
    };
  },
  created() {
    this.loadMenu();
    const currentUser = JSON.parse(localStorage.getItem("user"));
    if (currentUser) {
      if (currentUser.isAccessAdminPanel) {
        this.rightMenuItems = [
          {
            label: "Yönetim Paneli",
            to: "/admin",
          },
          {
            label: "Hesabım",
            to: "/kullanici/hesabim",
          },
          {
            label: "Çıkış Yap",
            command: () => {
              this.$store.dispatch("auth/logout");
              location.href = "/";
            },
          },
        ];
      } else {
        this.rightMenuItems = [
          {
            label: "Hesabım",
            to: "/kullanici/hesabim",
          },
          {
            label: "Çıkış Yap",
            command: () => {
              this.$store.dispatch("auth/logout");
              location.href = "/";
            },
          },
        ];
      }
    } else {
      this.rightMenuItems = [
        {
          label: "Giriş Yap",
          to: "/giris",
        },
        {
          label: "Üye Ol",
          to: "/uye-ol",
        },
      ];
    }
  },
  methods: {
    loadMenu() {
      GlobalService.Get(`${Endpoints.Menu}/Frontend`).then((res) => {
        this.menuDatasource = res.data;
      });
    },
    toggleRightMenu(event) {
      this.$refs.menu.toggle(event);
    },
  },
};
</script>

<style scoped>
@media screen and (max-width: 768px) {
  ::v-deep(.p-menubar-start) {
    width: 75%;
  }
}

.p-menubar {
  background: unset !important;
}
</style>
