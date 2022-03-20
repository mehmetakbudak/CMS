<template>
  <div class="row">
    <div class="col-md-3">
      <Panel class="mb-3" :toggleable="true">
        <template #header>
          <h5 class="my-3">
            <i class="pi pi-user pe-2"></i>
            {{ fullName }}
          </h5>
        </template>

        <ul class="list-group list-group-flush">
          <li class="list-group-item ps-0 pb-3">
            <router-link
              class="text-dark text-decoration-none"
              to="/kullanici/hesabim"
              >Hesabım</router-link
            >
          </li>
          <li class="list-group-item ps-0 py-3">
            <router-link
              class="text-dark text-decoration-none"
              to="/kullanici/sifre-degistir"
            >
              Şifre Değiştir
            </router-link>
          </li>
          <li class="list-group-item ps-0 py-3">
            <router-link
              class="text-dark text-decoration-none"
              to="/kullanici/yorumlarim"
            >
              Yorumlarım
            </router-link>
          </li>
          <li
            class="list-group-item cursor-pointer ps-0 pt-3"
            @click="logout()"
          >
            Çıkış Yap
          </li>
        </ul>
      </Panel>
    </div>
    <div class="col-md-9">
      <router-view :key="$route.fullPath"></router-view>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      fullName: "",
    };
  },
  created() {
    this.fullName = this.$store.state.auth.user.fullName;
  },
  methods: {
    logout() {
      this.$store.dispatch("auth/logout");
      location.href = "/";
    },
  },
};
</script>

<style>
.p-panel-header{
  background-color: white !important;
}
</style>