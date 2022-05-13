<template>
  <div class="card">
    <div class="card-header bg-white">
      <h4 class="my-3">İletişim</h4>
    </div>
    <div class="card-body">
      <div class="row p-3">
        <div class="col-md-6">
          <div class="mb-3">
            <label class="form-label">Konusu</label>
            <Dropdown
              class="w-100"
              v-model="data.contactCategoryId"
              :options="contactCategories"
              optionLabel="name"
              optionValue="id"
              placeholder="Konu seçiniz."
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
            <label class="form-label">Email Adresi</label>
            <InputText class="w-100" type="email" v-model="data.emailAddress" />
          </div>
          <div class="mb-3">
            <label class="form-label">Mesajı</label>
            <Textarea class="w-100" v-model="data.message" rows="5"></Textarea>
          </div>
          <div class="mb-3">
            <Button type="submit" label="Kaydet" @click="save"></Button>
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
export default {
  mixins: [AlertService],
  data() {
    return {
      contactCategories: [],
      data: {
        contactCategoryId: 0,
        name: "",
        surname: "",
        emailAddress: "",
        message: "",
      },
    };
  },
  created() {
    GlobalService.Get(Endpoints.Lookup.ContactCategories).then((res) => {
      this.contactCategories = res.data;
    });
  },
  methods: {
    save() {
      GlobalService.Post(Endpoints.Contact, this.data)
        .then(() => {
          this.data = {};
          this.successMessage(
            this,
            "Mesajınız başarıyla kaydedildi. En kısa zamanda dönüş sağlanacaktır."
          );
        })
        .catch((e) => {
          this.errorMessage( e.response.data.message);
        });
    },
  },
};
</script>
