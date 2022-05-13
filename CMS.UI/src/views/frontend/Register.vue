<template>
  <div class="card">
    <div class="card-body">
      <div class="col-md-4 offset-md-4 py-50">
        <div class="card shadow">
          <div class="card-header py-3 bg-white">
            <h4>Üye Ol</h4>
          </div>
          <div class="card-body">
            <form @submit="register">
              <div class="mb-3">
                <label class="form-label">Adı</label>
                <DxTextBox v-model:value="user.name" mode="text">
                  <DxValidator>
                    <DxRequiredRule message="Adı gereklidir." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="mb-3">
                <label class="form-label">Soyadı</label>
                <DxTextBox v-model:value="user.surname" mode="text">
                  <DxValidator>
                    <DxRequiredRule message="Soyadı gereklidir." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="mb-3">
                <label class="form-label">Email Adresi</label>
                <DxTextBox v-model:value="user.emailAddress" mode="email">
                  <DxValidator>
                    <DxRequiredRule message="Email adresi gereklidir." />
                    <DxEmailRule message="Email adresi geçersiz." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="mb-3">
                <label class="form-label">Şifre</label>
                <DxTextBox v-model:value="user.password" mode="password">
                  <DxValidator>
                    <DxRequiredRule message="Şifre gereklidir." />
                  </DxValidator>
                </DxTextBox>
              </div>
              <div class="mb-3">
                <label class="form-label">Şifre Yeniden</label>
                <DxTextBox v-model:value="user.rePassword" mode="password">
                  <DxValidator>
                    <DxRequiredRule message="Şifre Yeniden gereklidir." />
                  </DxValidator>
                </DxTextBox>
              </div>

              <div class="mb-3">
                <DxButton
                  class="w-100"
                  text="Kaydet"
                  type="default"
                  :use-submit-behavior="true"
                />
              </div>
            </form>
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
    </div>
  </div>
</template>

<script>
import { DxButton } from "devextreme-vue/button";
import {
  DxValidator,
  DxRequiredRule,
  DxEmailRule,
} from "devextreme-vue/validator";
import { DxTextBox } from "devextreme-vue/text-box";
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";

export default {
  mixins: [AlertService],
  components: { DxTextBox, DxButton, DxValidator, DxRequiredRule, DxEmailRule },
  data() {
    return {
      user: {
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
      GlobalService.Post(Endpoints.Account.AddMember, this.user)
        .then((res) => {
          this.user = {
            name: "",
            surname: "",
            emailAddress: "",
            password: "",
            rePassword: "",
          };
          this.successMessage( res.user.message);
        })
        .catch((e) => {
          this.errorMessage( e.response.user.message);
        });
    },
  },
};
</script>
