<template>
  <nav
    class="navbar navbar-expand-lg navbar-light bg-light mt-2 border"
    style="border-radius: 5px;"
  >
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/">CMS</router-link>
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link class="nav-link active" to="/">Anasayfa</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/haberler">Haberler</router-link>
          </li>
            <li class="nav-item">
            <router-link class="nav-link" to="/iletisim">İletişim</router-link>
          </li>
          <li class="nav-item dropdown">
            <a
              class="nav-link dropdown-toggle"
              href="#"
              id="navbarDropdown"
              role="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              Blog
            </a>
            <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
              <li><a class="dropdown-item" href="#">Action</a></li>
              <li><a class="dropdown-item" href="#">Another action</a></li>
              <li><hr class="dropdown-divider" /></li>
              <li><a class="dropdown-item" href="#">Something else here</a></li>
            </ul>
          </li>
          <li class="nav-item">
            <a
              class="nav-link disabled"
              href="#"
              tabindex="-1"
              aria-disabled="true"
              >Disabled</a
            >
          </li>
        </ul>
        <div class="d-flex">
          <Button
            type="button"
            icon="pi pi-user"
            class="p-button-rounded p-button-primary"
            @click="toggleRightMenu"
          />
          <Menu ref="menu" :model="rightMenuItems" :popup="true" />
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
export default {
  created() {
    this.loadMenu();
  },
  data() {
    return {
      menuDatasource: [],
      rightMenuItems: [
        {
          label: "Giriş Yap",
          to: "/giris",
        },
        {
          label: "Üye Ol",
          to: "/uye-ol",
        },
      ],
    };
  },
  methods: {
    loadMenu() {
      this.axios
        .get(process.env.VUE_APP_BASEURL + "menu/frontend")
        .then((res) => {
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
