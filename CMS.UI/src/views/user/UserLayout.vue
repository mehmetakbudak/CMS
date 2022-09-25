<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>{{ title }}</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>{{ title }}</li>
          </ol>
        </div>
      </div>
    </section>
    <div class="container">
      <div class="row py-3">
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
    </div>
  </main>
</template>

<script>
import { useAuthStore, useBreadcrumbStore } from "../../store";

export default {
  setup() {
    const authStore = useAuthStore();
    const breadcrumbStore = useBreadcrumbStore();
    return { authStore, breadcrumbStore };
  },
  computed: {
    fullName() {
      return this.authStore.user.fullName;
    },
    title() {
      return this.breadcrumbStore.title; 
    }
  },
  data() {
    return {};
  },
  methods: {
    logout() {
      this.authStore.logout();
    },
  },
};
</script>

<style>
.p-panel-header {
  background-color: white !important;
}
</style>