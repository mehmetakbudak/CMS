<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <h5>Şifre Değiştir</h5>
    </div>
    <div class="card-body">
      <div class="row p-3">
        <div class="col-md-6">
          <div class="mb-3">
            <label class="form-label">Mevcut Şifre</label>
            <InputText
              class="w-100"
              type="password"
              v-model="data.oldPassword"
            />
          </div>
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
            <Button
              class="bg-green"
              type="submit"
              label="Kaydet"
              @click="save"
            ></Button>
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
import { useBreadcrumbStore } from "../../store";

export default {
  mixins: [AlertService],
  setup() {
    const breadcrumbStore = useBreadcrumbStore();
    return { breadcrumbStore };
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
  created() {
    this.breadcrumbStore.title = "Şifre Değiştir";
  },
  methods: {
    save() {
      GlobalService.PutByAuth(Endpoints.Account.ChangePassword, this.data)
        .then(() => {
          this.successMessage(this, "Şifre başarıyla güncellendi.");
          this.data = {
            oldPassword: "",
            newPassword: "",
            reNewPassword: "",
          };
        })
        .catch((error) => {
          this.errorMessage(this, error.response.data.message);
        });
    },
  },
};
</script>

<style>
</style>