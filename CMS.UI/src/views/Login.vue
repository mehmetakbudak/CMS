<template>
  <Card>
    <template #content>
      <div class="col-md-4 offset-4" style="padding: 100px 0;">
        <Panel v-if="showLogin">
          <template #header>
            <h4>Giriş Yap</h4>
          </template>
          <div class="p-fluid">
            <div class="p-field">
              <label>Email Adresi</label>
              <InputText type="email" v-model="data.emailAddress" />
            </div>
            <div class="p-field">
              <label>Şifre</label>
              <InputText type="password" v-model="data.password" />
            </div>
            <div>
              <a class="float-start pb-3" @click="showForgotPassword()"
                >Şifremi Unuttum</a
              >
            </div>
            <div class="p-field">
              <Button type="submit" label="Giriş Yap" @click="login"></Button>
            </div>
          </div>
        </Panel>
        <Panel v-if="showForgotPassword">
          <template #header>
            <h4>Giriş Yap</h4>
          </template>
          <div class="p-fluid">
            <div class="p-field">
              <label>Email Adresi</label>
              <InputText type="email" v-model="data.emailAddress" />
            </div>
            <div class="p-field">
              <label>Şifre</label>
              <InputText type="password" v-model="data.password" />
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
                  class="float-end pb-3"
                  @click="showForgotPassword()"
                >
                  Şifremi Unuttum
                </a>
              </div>
            </div>
            <div class="p-field">
              <Button type="submit" label="Giriş Yap" @click="login"></Button>
            </div>
          </div>
        </Panel>
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
      this.$router.push("/admin");
    }
  },
  data() {
    return {
      exceptions: [],
      data: {
        emailAddress: "",
        password: "",
      },
    };
  },
  methods: {
    login() {
      this.exceptions = [];
      this.$store.dispatch("auth/login", this.data).then(
        () => {
          this.$router.push("/admin");
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
    showForgotPassword() {},
  },
};
</script>

<style scoped>
.content {
  padding: 50px 0;
}
</style>
