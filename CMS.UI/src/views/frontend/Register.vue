<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Üye Ol</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>Üye Ol</li>
          </ol>
        </div>
      </div>
    </section>
    <div class="container my-3">
      <Card>
        <template #content>
          <div class="col-md-4 offset-md-4">
            <div class="card">
              <div class="card-header py-3 bg-white">
                <h4>Üye Ol</h4>
              </div>
              <div class="card-body">
                <div class="mb-3">
                  <label class="form-label">Adı</label>
                  <InputText type="email" class="w-100" v-model="data.name" />
                </div>
                <div class="mb-3">
                  <label class="form-label">Soyadı</label>
                  <InputText
                    type="email"
                    class="w-100"
                    v-model="data.surname"
                  />
                </div>
                <div class="mb-3">
                  <label class="form-label">Email Adresi</label>
                  <InputText
                    type="email"
                    class="w-100"
                    v-model="data.emailAddress"
                  />
                </div>
                <div class="mb-3">
                  <label class="form-label">Şifre</label>
                  <InputText
                    type="password"
                    class="w-100"
                    v-model="data.password"
                  />
                </div>
                <div class="mb-3">
                  <label class="form-label">Şifre Yeniden</label>
                  <InputText
                    type="password"
                    class="w-100"
                    v-model="data.rePassword"
                  />
                </div>

                <div class="mb-3">
                  <Button
                    class="w-100 bg-green"
                    type="submit"
                    label="Kaydet"
                    @click="register"
                  ></Button>
                </div>
                <div class="text-center">
                  Zaten üye misiniz?
                  <router-link
                    class="text-decoration-none cursor-pointer"
                    to="/giris"
                    >Giriş Yap</router-link
                  >
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

export default {
  mixins: [AlertService],
  data() {
    return {
      data: {
        name: "",
        surname: "",
        emailAddress: "",
        password: "",
        rePassword: "",
      },
    };
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
  methods: {
    register() {
      GlobalService.Post(Endpoints.Account.AddMember, this.data)
        .then((res) => {
          this.data = {
            name: "",
            surname: "",
            emailAddress: "",
            password: "",
            rePassword: "",
          };
          this.successMessage(this, res.data.message);
        })
        .catch((e) => {
          this.errorMessage(this, e.response.data.message);
        });
    },
  },
};
</script>

<style>
</style>