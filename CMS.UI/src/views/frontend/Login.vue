<template>
  <Card>
    <template #content>
      <div class="col-md-4 offset-4 my-5">
        <div class="card" v-if="visibleLogin">
          <div class="card-header py-3">
            <h4>Giriş Yap</h4>
          </div>
          <div class="card-body">
            <div class="my-3" v-if="visibleError">
              <div class="alert alert-danger">{{ message }}</div>
            </div>

            <div class="mb-3">
              <label class="form-label">Email Adresi</label>
              <InputText
                class="w-100"
                type="email"
                v-model="loginFormData.emailAddress"
              />
            </div>
            <div class="mb-3">
              <label class="form-label">Şifre</label>
              <InputText
                class="w-100"
                type="password"
                v-model="loginFormData.password"
              />
            </div>
            <div class="row">
              <div class="col-6">
                <div class="p-field-checkbox float-start">
                  <Checkbox />
                  <label class="pe-2">Beni Hatırla</label>
                </div>
              </div>
              <div class="col-6">
                <a
                  class="float-end pb-3 text-decoration-none cursor-pointer"
                  @click="showForgot()"
                >
                  Şifremi Unuttum
                </a>
              </div>
            </div>
            <div class="mb-3">
              <Button
                class="w-100"
                type="submit"
                label="Giriş Yap"
                @click="login"
              ></Button>
            </div>
            <div class="text-center">
              Üye değil misiniz?
              <router-link
                class="text-decoration-none cursor-pointer"
                to="/uye-ol"
                >Üye Ol</router-link
              >
            </div>
          </div>
        </div>
        <div class="card" v-if="visibleForgotPassword">
          <div class="card-header py-3">
            <div class="row">
              <div class="col-9">
                <h4>Şifremi Unuttum</h4>
              </div>
              <div class="col-3">
                <Button
                  @click="showLogin()"
                  class="p-button-outlined float-end p-sm"
                  icon="pi pi-arrow-left"
                />
              </div>
            </div>
          </div>
          <div class="card-body">
            <div class="mb-3">
              <label class="form-label">Email Adresi</label>
              <InputText
                class="w-100"
                type="email"
                v-model="forgotPasswordFormData.emailAddress"
              />
            </div>
            <div class="mb-3">
              <Button
                class="w-100"
                type="submit"
                label="Gönder"
                @click="forgotPassword"
              ></Button>
            </div>
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>

<script>
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
export default {
  mixins: [AlertService],
  computed: {
    loggedIn() {
      return this.$store.state.auth.status.loggedIn;
    },
  },
  created() {
    if (this.loggedIn) {
      this.$router.push("/");
    }
  },
  data() {
    return {
      visibleLogin: true,
      visibleForgotPassword: false,
      visibleError: false,
      message: "",
      loginFormData: {
        emailAddress: "",
        password: "",
      },
      forgotPasswordFormData: {
        emailAddress: "",
      },
    };
  },
  methods: {
    login() {
      this.$store.dispatch("auth/login", this.loginFormData).then(
        () => {
          location.href = "/";
        },
        (error) => {
          this.visibleError = true;
          this.message = error.response.data.message;
        }
      );
    },
    forgotPassword() {
      GlobalService.Post(
        Endpoints.Account.ForgotPassword,
        this.forgotPasswordFormData
      ).then((res) => {
        this.forgotPasswordFormData = {};
        this.successMessage(this, res.data.message);
        this.showLogin();
      });
    },
    showForgot() {
      this.visibleLogin = false;
      this.visibleForgotPassword = true;
    },
    showLogin() {
      this.visibleLogin = true;
      this.visibleForgotPassword = false;
    },
  },
};
</script>

<style scoped>
.content {
  padding: 50px 0;
}
</style>
