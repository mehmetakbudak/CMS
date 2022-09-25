<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Referanslar</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>Referanslar</li>
          </ol>
        </div>
      </div>
    </section>

    <section id="testimonials" class="testimonials section-bg">
      <div class="container">
        <div class="row">
          <div class="col-lg-6 mb-3" v-for="item in list" :key="item.id">
            <div class="testimonial-item">
              <img :src="item.imageUrl" class="testimonial-img" />
              <h3>{{ item.name }} {{ item.surname }}</h3>
              <h4><span v-if="item.corporateName">{{ item.corporateName }} / </span>{{ item.title }}</h4>
              <p v-html="item.description"></p>
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
      GlobalService.Get(Endpoints.Testimonial).then((res) => {
        this.list = res.data;
      });
    },
  },
};
</script>

<style>
</style>