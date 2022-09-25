<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Giriş Yap</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>Giriş Yap</li>
          </ol>
        </div>
      </div>
    </section>
    <div class="container my-3">
      <Card>
        <template #content>
          <div class="col-md-4 offset-md-4 my-5">
            <div class="card" v-if="visibleLogin">
              <div class="card-header py-3 bg-white">
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
                    <div class="p-field-checkbox float-start"></div>
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
                    class="w-100 bg-green"
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
              <div class="card-header py-3 bg-white">
                <div class="row">
                  <div class="col-9">
                    <h4>Şifremi Unuttum</h4>
                  </div>
                  <div class="col-3">
                    <Button
                      @click="showLogin()"
                      class="
                        p-button-outlined
                        float-end
                        p-sm
                        bg-green
                        text-white
                      "
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
                    class="w-100 bg-green"
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
    </div>
  </main>
</template>

<script>
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
import { useAuthStore } from "../../store";

export default {
  mixins: [AlertService],
  setup() {
    const authStore = useAuthStore();
    return { authStore };
  },
  computed: {
    // loggedIn() {
    //   return authStore.loggedIn;
    // },
  },
  created() {
    // if (this.loggedIn) {
    //   this.$router.push("/");
    // }
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
      this.authStore.login(this.loginFormData);
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
