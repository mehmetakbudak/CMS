<template>
  <div class="card">
    <div class="card-body">
      <div class="col-md-4 offset-md-4 py-100">
        <form @submit="login">
          <div class="card shadow" v-if="visibleLogin">
            <div class="card-header py-3 bg-white">
              <h4>Giriş Yap</h4>
            </div>
            <div class="card-body">
              <div class="my-3" v-if="visibleError">
                <div class="alert alert-danger">{{ message }}</div>
              </div>

              <div class="mb-3">
                <label class="form-label">Email Adresi</label>
                <DxTextBox v-model:value="loginFormData.emailAddress">
                  <DxValidator>
                    <DxRequiredRule message="Email adresi gereklidir." />
                    <DxEmailRule message="Email adresi geçersiz." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="mb-3">
                <label class="form-label">Şifre</label>
                <DxTextBox
                  v-model:value="loginFormData.password"
                  mode="password"
                >
                  <DxValidator>
                    <DxRequiredRule message="Şifre gereklidir." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="row">
                <div class="col-6"></div>
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
                <DxButton
                  class="w-100"
                  text="Giriş Yap"
                  type="default"
                  :use-submit-behavior="true"
                />
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
        </form>
        <form @submit="forgotPassword">
          <div class="card shadow" v-if="visibleForgotPassword">
            <div class="card-header py-3 bg-white">
              <div class="row">
                <div class="col-9">
                  <h4>Şifremi Unuttum</h4>
                </div>
                <div class="col-3">
                  <a
                    @click="showLogin()"
                    class="btn btn-outline-primary float-end btn-sm"
                    ><i class="dx-icon-arrowleft"></i
                  ></a>
                </div>
              </div>
            </div>
            <div class="card-body">
              <div class="mb-3">
                <label class="form-label">Email Adresi</label>
                <DxTextBox v-model:value="forgotPasswordFormData.emailAddress">
                  <DxValidator>
                    <DxRequiredRule message="Email adresi gereklidir." />
                    <DxEmailRule message="Email adresi geçersiz." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="mb-3">
                <DxButton
                  class="w-100"
                  text="Gönder"
                  type="default"
                  :use-submit-behavior="true"
                />
              </div>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { DxButton } from "devextreme-vue/button";
import { DxTextBox } from "devextreme-vue/text-box";
import {
  DxValidator,
  DxRequiredRule,
  DxEmailRule,
} from "devextreme-vue/validator";
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";

export default {
  mixins: [AlertService],
  components: {
    DxButton,
    DxTextBox,
    DxValidator,
    DxRequiredRule,
    DxEmailRule,
  },
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
    login(e) {
      e.preventDefault();
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
    forgotPassword(e) {
      e.preventDefault();
      GlobalService.Post(
        Endpoints.Account.ForgotPassword,
        this.forgotPasswordFormData
      ).then((res) => {
        this.forgotPasswordFormData = {};
        this.successMessage( res.data.message);
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
