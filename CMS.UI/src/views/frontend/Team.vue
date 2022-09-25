<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Ekibimiz</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>Ekibimiz</li>
          </ol>
        </div>
      </div>
    </section>

    <section id="team" class="team section-bg">
      <div class="container">
        <div class="row">
          <div
            class="col-lg-3 col-md-6 d-flex align-items-stretch"
            v-for="item in list"
            :key="item.id"
          >
            <div class="member">
              <div class="member-img">
                <img :src="item.imageUrl" class="img-fluid" alt="" />
                <div class="social">
                  <a target="_blank" :href="item.twitterUrl"
                    ><i class="pi pi-twitter"></i
                  ></a>
                  <a target="_blank" :href="item.facebookUrl"
                    ><i class="pi pi-facebook"></i
                  ></a>
                  <a target="_blank" :href="item.instagramUrl"
                    ><i class="pi pi-instagram"></i
                  ></a>
                  <a target="_blank" :href="item.linkedinUrl"
                    ><i class="pi pi-linkedin"></i
                  ></a>
                </div>
              </div>
              <div class="member-info">
                <h4>{{ item.name }} {{ item.surname }}</h4>
                <span
                  ><i class="pi pi-envelope"></i> {{ item.emailAddress }}</span
                >
                <span><i class="pi pi-phone"></i> {{ item.phone }}</span>
                <span>{{ item.title }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";

export default {
  data() {
    return {
      list: [],
    };
  },
  created() {
    this.getAll();
  },
  methods: {
    getAll() {
      GlobalService.Get(Endpoints.Team).then((res) => {
        this.list = res.data;
      });
    },
  },
};
</script>

<style>
</style>