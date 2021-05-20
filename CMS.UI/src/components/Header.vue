<template>
  <Menubar :model="menuDatasource" class="border">
    <template #start>
      <router-link
        class="h4 text-secondary text-decoration-none mr-3"
        to="/"
        style="padding-right: 30px"
        >CMS</router-link
      >
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