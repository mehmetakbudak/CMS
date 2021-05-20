<template>
  <div class="card">
    <div class="card-body content">
      <div class="col-md-4 offset-4">
        <div class="card">
          <h3 class="card-header">Giriş Yap</h3>
          <div class="card-body">
            <Message v-for="item of exceptions" severity="error" :key="item">{{
              item
            }}</Message>

            <div class="p-fluid">
              <div class="p-field">
                <label>Email Adresi</label>
                <InputText type="email" v-model="data.emailAddress" />
              </div>
              <div class="p-field">
                <label>Şifre</label>
                <InputText type="password" v-model="data.password" />
              </div>
              <div class="p-field">
                <Button type="submit" label="Giriş Yap" @click="login"></Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
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
  },
};
</script>

<style scoped>
.content {
  padding: 50px 0;
}
</style>