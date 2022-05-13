<template>
  <div class="row">
    <div class="col-md-3">
      <DxAccordion
        :collapsible="true"
        item-title-template="title"
      >
        <template #title>
          <h3 class="py-3">
            <i class="dx-icon dx-icon-card h3"></i> {{ fullName }}
          </h3>
        </template>
        <DxItem>
          <template #default>
            <div class="border-top">
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
            </div>
          </template>
        </DxItem>
      </DxAccordion>

      <!-- <Panel class="mb-3" :toggleable="true">
        <template #header>
          <h5 class="my-3">
            <i class="pi pi-user pe-2"></i>
            {{ fullName }}
          </h5>
        </template>
      </Panel> -->
    </div>
    <div class="col-md-9">
      <router-view :key="$route.fullPath"></router-view>
    </div>
  </div>
</template>

<script>
import DxAccordion, { DxItem } from "devextreme-vue/accordion";
export default {
  components: {
    DxAccordion,
    DxItem,
  },
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
.p-panel-header {
  background-color: white !important;
}
</style>