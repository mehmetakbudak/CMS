<template>
  <div class="row">
    <div class="col-md-3">
      <div class="card">
        <div class="card-header bg-white py-3">
          <h5>
            <i class="pi pi-user pe-2"></i>
            {{ fullName }}
          </h5>
        </div>
        <div class="card-body">
          <ul class="list-group list-group-flush">
            <li class="list-group-item ps-0 pb-3">
              <router-link
                class="text-dark text-decoration-none"
                to="/uye/hesabim"
                >Hesabım</router-link
              >
            </li>
            <li class="list-group-item ps-0 py-3">
              <router-link
                class="text-dark text-decoration-none"
                to="/uye/sifre-degistir"
              >
                Şifre Değiştir
              </router-link>
            </li>
            <li class="list-group-item ps-0 py-3">
              <router-link
                class="text-dark text-decoration-none"
                to="/uye/yorumlarim"
              >
                Yorumlarım
              </router-link>
            </li>
            <li class="list-group-item cursor-pointer ps-0 pt-3" @click="logout()">
              Çıkış Yap
            </li>
          </ul>
        </div>
      </div>
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
</style>