<template>
  <div class="container mt-3 mb-3">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item">
          <router-link to="/">Anasayfa</router-link>
        </li>
        <li class="breadcrumb-item active" aria-current="page">İletişim</li>
      </ol>
    </nav>
    <div class="card">
      <div class="card-body">
        <b-form @submit="save">
          <div class="row">
            <div class="col-md-6">
              <div class="form-group">
                <label>Adı Soyadı</label>
                <b-form-input
                  v-model="contact.nameSurname"
                  type="text"
                  placeholder="Adı Soyadı"
                  required
                >
                </b-form-input>
              </div>
              <div class="form-group">
                <label>Email Adresi</label>
                <b-form-input
                  v-model="contact.emailAddress"
                  type="email"
                  placeholder="Email Adresi"
                  required
                >
                </b-form-input>
              </div>
              <div class="form-group">
                <label>Konu</label>
                <b-form-select
                  v-model="contact.subject"
                  :options="contactCategories"
                  value-field="id"
                  text-field="name"
                  required
                >
                  <template #first>
                    <b-form-select-option :value="null"
                      >-- Konu seçiniz --</b-form-select-option
                    >
                  </template>
                </b-form-select>
              </div>
              <div class="form-group">
                <label>Konu</label>
                <b-textarea v-model="contact.message" required></b-textarea>
              </div>
              <div>
                <b-button type="submit" variant="primary">Kaydet</b-button>
              </div>
            </div>
          </div>
        </b-form>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  created() {
    this.loadContactCategories();
  },
  components: {},
  data() {
    return {
      contact: {
        nameSurname: "",
        emailAddress: "",
        subject: null,
        message: "",
      },
      contactCategories: [],
    };
  },
  methods: {
    loadContactCategories() {
      axios
        .get(`${process.env.VUE_APP_BASEURL}Lookup/ContactCategories`)
        .then((res) => {
          this.contactCategories = res.data;
        });
    },
    save(e) {
      e.preventDefault();
      axios
        .post(`${process.env.VUE_APP_BASEURL}contact`, this.contact)
        .then((res) => {
          this.contact.nameSurname = "";
          this.contact.emailAddress = "";
          this.contact.subject = null;
          this.contact.message = "";

          this.$bvToast.toast("Mesajınız başarıyla kaydedildi.", {
            title: "İşlem Başarılı",
            solid: true,
          });
        });
    },
  },
};
</script>