<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>İletişim</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>İletişim</li>
          </ol>
        </div>
      </div>
    </section>

    <div class="map-section">
      <iframe
        style="border: 0; width: 100%; height: 350px"
        src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d12097.433213460943!2d-74.0062269!3d40.7101282!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0xb89d1fe6bc499443!2sDowntown+Conference+Center!5e0!3m2!1smk!2sbg!4v1539943755621"
        frameborder="0"
        allowfullscreen
      ></iframe>
    </div>

    <section id="contact" class="contact">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-10">
            <div class="info-wrap bg-white">
              <div class="row">
                <div class="col-lg-4 info">
                  <i class="pi pi-map-marker"></i>
                  <h4>Location:</h4>
                  <p>A108 Adam Street<br />New York, NY 535022</p>
                </div>

                <div class="col-lg-4 info mt-4 mt-lg-0">
                  <i class="pi pi-envelope"></i>
                  <h4>Email:</h4>
                  <p>info@example.com<br />contact@example.com</p>
                </div>

                <div class="col-lg-4 info mt-4 mt-lg-0">
                  <i class="pi pi-phone"></i>
                  <h4>Call:</h4>
                  <p>+1 5589 55488 51<br />+1 5589 22475 14</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="row mt-5 justify-content-center">
          <div class="col-lg-10">
            <div class="contact-form">
              <div class="row py-3">
                <div class="col-md-6 offset-3">
                  <div class="mb-3">
                    <label class="form-label fw-bold">Konusu</label>
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
                    <label class="form-label fw-bold">Adı</label>
                    <InputText class="w-100" type="text" v-model="data.name" />
                  </div>
                  <div class="mb-3">
                    <label class="form-label fw-bold">Soyadı</label>
                    <InputText
                      class="w-100"
                      type="text"
                      v-model="data.surname"
                    />
                  </div>
                  <div class="mb-3">
                    <label class="form-label fw-bold">Email Adresi</label>
                    <InputText
                      class="w-100"
                      type="email"
                      v-model="data.emailAddress"
                    />
                  </div>
                  <div class="mb-3">
                    <label class="form-label fw-bold">Mesajı</label>
                    <Textarea
                      class="w-100"
                      v-model="data.message"
                      rows="5"
                    ></Textarea>
                  </div>
                  <div class="mb-3">
                    <Button type="submit" label="Gönder" @click="save"></Button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
    <!-- End Contact Section -->
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
          this.errorMessage(e.response.data.message);
        });
    },
  },
};
</script>
