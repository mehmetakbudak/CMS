<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <h5>Hesabım</h5>
    </div>
    <div class="card-body">
      <div class="row p-3">
        <div class="col-md-6">
          <div class="mb-3">
            <label class="form-label">Email Adresi</label>
            <InputText
              disabled
              class="w-100"
              type="email"
              v-model="data.emailAddress"
            />
          </div>
          <div class="mb-3">
            <label class="form-label">Adı</label>
            <InputText class="w-100" type="text" v-model="data.name" />
          </div>
          <div class="mb-3">
            <label class="form-label">Soyadı</label>
            <InputText class="w-100" type="text" v-model="data.surname" />
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
import { useBreadcrumbStore } from '../../store';

export default {
  mixins: [AlertService],
  setup() {
    const breadcrumbStore = useBreadcrumbStore();
    return { breadcrumbStore };
  },
  data() {
    return {
      data: {
        emailAddress: "",
        name: "",
        surname: "",
      },
    };
  },
  created() {
    this.breadcrumbStore.title = "Hesabım";
    GlobalService.GetByAuth(Endpoints.Account.Profile).then((res) => {
      this.data = res.data;
    });
  },
  methods: {
    save() {
      GlobalService.PutByAuth(Endpoints.Account.Profile, this.data)
        .then((res) => {
          this.successMessage(this, res.data.message);
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