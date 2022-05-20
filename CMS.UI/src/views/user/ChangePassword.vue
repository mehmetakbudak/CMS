<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <h3>Şifre Değiştir</h3>
    </div>
    <div class="card-body">
      <div class="row p-3">
        <div class="col-md-6">
          <form @submit="save">
            <div class="mb-3">
              <label class="form-label">Mevcut Şifre</label>
              <DxTextBox v-model:value="data.oldPassword" mode="password">
                <DxValidator>
                  <DxRequiredRule message="Şifre gereklidir." />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="mb-3">
              <label class="form-label">Yeni Şifre</label>
              <DxTextBox v-model:value="data.newPassword" mode="password">
                <DxValidator>
                  <DxRequiredRule message="Yeni şifre gereklidir." />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="mb-3">
              <label class="form-label">Yeni Şifre Tekrar</label>
              <DxTextBox v-model:value="data.reNewPassword" mode="password">
                <DxValidator>
                  <DxRequiredRule message="Yeni şifre tekrar gereklidir." />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="mb-3">
              <DxButton
                text="Kaydet"
                type="default"
                :use-submit-behavior="true"
              />
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
import { DxButton } from "devextreme-vue/button";
import { DxTextBox } from "devextreme-vue/text-box";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";

export default {
  mixins: [AlertService],
  components: {
    DxButton,
    DxTextBox,
    DxValidator,
    DxRequiredRule,
  },
  data() {
    return {
      message: "",
      visibleError: false,
      data: {
        oldPassword: "",
        newPassword: "",
        reNewPassword: "",
      },
    };
  },
  methods: {
    save(e) {
      e.preventDefault();
      GlobalService.PutByAuth(Endpoints.Account.ChangePassword, this.data)
        .then(() => {
          this.successMessage("Şifre başarıyla güncellendi.");
          this.data = {
            oldPassword: "",
            newPassword: "",
            reNewPassword: "",
          };
        })
        .catch((error) => {
          this.errorMessage(error.response.data.message);
        });
    },
  },
};
</script>

<style></style>
