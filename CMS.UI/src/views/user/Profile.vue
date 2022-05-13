<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <h3>Hesabım</h3>
    </div>
    <div class="card-body">
      <div class="row p-3">
        <div class="col-md-6">
          <form @submit="save">
            <div class="mb-3">
              <label class="form-label">Email Adresi</label>
              <DxTextBox
                v-model:value="user.emailAddress"
                mode="text"
                :disabled="true"
              ></DxTextBox>
            </div>
            <div class="mb-3">
              <label class="form-label">Adı</label>
              <DxTextBox v-model:value="user.name" mode="text"></DxTextBox>
            </div>
            <div class="mb-3">
              <label class="form-label">Soyadı</label>
              <DxTextBox v-model:value="user.surname" mode="text"></DxTextBox>
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
import { DxButton } from "devextreme-vue/button";
import { DxTextBox } from "devextreme-vue/text-box";
import AlertService from "../../services/AlertService";
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
export default {
  mixins: [AlertService],
  components: {
    DxButton,
    DxTextBox,
  },
  data() {
    return {
      user: {
        emailAddress: "",
        name: "",
        surname: "",
      },
    };
  },
  created() {
    GlobalService.GetByAuth(Endpoints.Account.Profile).then((res) => {
      this.user = res.data;
    });
  },
  methods: {
    save(e) {
      e.preventDefault();
      GlobalService.PutByAuth(Endpoints.Account.Profile, this.user)
        .then((res) => {
          this.successMessage( res.data.message);
        })
        .catch((error) => {
          this.errorMessage( error.response.data.message);
        });
    },
  },
};
</script>