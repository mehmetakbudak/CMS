<template>
  <div class="card">
    <div class="card-header bg-white">
      <h4 class="my-3">E-mail Adresi Doğrulama</h4>
    </div>
    <div class="card-body">
      <div v-if="isSuccess">
        E-mail adresi başarıyla doğrulandı. Giriş yapmak için
        <router-link to="/giris" class="btn btn-outline-primary"
          >Tıklayınız</router-link
        >
      </div>
      <div v-if="!isSuccess">
        <div class="mt-3 alert alert-danger">
          {{ message }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";

export default {
  data() {
    return {
      isSuccess: false,
      message: "",
    };
  },
  created() {
    GlobalService.Put(
      `${Endpoints.Account.EmailVerified}/${this.$route.params.code}`
    )
      .then(() => {
        this.isSuccess = true;
      })
      .catch((e) => {
        this.message = e.response.data.message;
      });
  },
};
</script>

<style>
</style>