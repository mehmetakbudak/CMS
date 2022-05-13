<template>
  <div class="card">
    <div class="card-header bg-white">
      <h4 class="my-3">Şifre Belirleme</h4>
    </div>
    <div class="card-body">
      <div v-if="showMessage">
        <div class="mt-3 alert alert-danger">
          {{ message }}
        </div>
      </div>
      <div v-if="visibleForm">
        <div class="my-3">
          Merhaba <b>{{ fullName }} </b>. Aşağıdaki bilgilerle yeni şifrenizi
          belirleyebilirsiniz.
        </div>
        <div class="row">
          <div class="col-md-4">
            <div class="mb-3">
              <label class="form-label">Yeni Şifre</label>
              <InputText
                class="w-100"
                type="password"
                v-model="data.newPassword"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Yeni Şifre Tekrar</label>
              <InputText
                class="w-100"
                type="password"
                v-model="data.reNewPassword"
              />
            </div>
            <div class="mb-3">
              <Button type="submit" label="Kaydet" @click="save"></Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
export default {
  mixins: [AlertService],
  data() {
    return {
      visibleForm: false,
      showMessage: false,
      fullName: "",
      message: "",
      data: {
        code: this.$route.params.code,
        newPassword: "",
        reNewPassword: "",
      },
    };
  },
  created() {
    GlobalService.Get(
      `${Endpoints.Account.ResetPassword}/${this.$route.params.code}`
    )
      .then((res) => {
        this.visibleForm = true;
        this.fullName = res.data.fullName;
      })
      .catch((e) => {
        this.visibleForm = false;
        this.showMessage = true;
        this.message = e.response.data.message;
      });
  },
  methods: {
    save() {
      GlobalService.Put(`${Endpoints.Account.ResetPassword}`, this.data)
        .then((res) => {
          this.successMessage( res.data.message);
          this.$router.push({ path: "/giris" });
        })
        .catch((e) => {
          this.message = e.response.data.message;
          this.visibleForm = true;
          this.showMessage = true;
        });
    },
  },
};
</script>

<style>
</style>