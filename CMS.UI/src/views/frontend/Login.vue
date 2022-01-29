<template>
  <Card>
    <template #content>
      <div class="col-md-4 offset-4 my-5">
        <div class="card" v-if="visibleLogin">
          <div class="card-header py-3">
            <h4>Giriş Yap</h4>
          </div>
          <div class="card-body">
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
            <h4>Şifremi Unuttum</h4>
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
            <div class="p-field">
              <a
                class="float-end pb-3 text-decoration-none cursor-pointer"
                @click="showLogin()"
              >
                Giriş Yap
              </a>
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
export default {
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
      exceptions: [],
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
      this.exceptions = [];
      this.$store.dispatch("auth/login", this.loginFormData).then(
        () => {
          location.href="/";
        },
        (error) => {
          this.loading = false;
          this.message =
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString();
        }
      );
    },
    forgotPassword() {},
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
