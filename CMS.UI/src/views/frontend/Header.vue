<template>
  <header id="header" class="fixed-top">
    <div class="container d-flex align-items-center">
      <h1 class="logo me-auto">
        <router-link to="/"><span>Com</span>pany</router-link>
      </h1>
      <Menubar :model="menuDatasource" class="py-0 text-dark">
        <template #end>
          <Button
            type="button"
            icon="pi pi-user"
            class="p-button-rounded p-button-primary"
            @click="toggleRightMenu"
          />
          <Menu
            id="rightMenu"
            class="ps-3"
            ref="menu"
            :model="rightMenuItems"
            :popup="true"
          />
        </template>
      </Menubar>
    </div>
  </header>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
import { useAuthStore } from "../../store";

export default {
  setup() {
    const authStore = useAuthStore();
    return { authStore };
  },
  data() {
    return {
      menuDatasource: [],
      rightMenuItems: [],
    };
  },
  created() {
    this.loadMenu();
    console.log(this.authStore.user);
    const currentUser = this.authStore.user;
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
              this.authStore.logout();
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
              this.authStore.logout();
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

::v-deep(.p-menubar .p-menuitem-link .p-menuitem-text) {
  color: black !important;
}
</style>
