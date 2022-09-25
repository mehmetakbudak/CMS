<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Hizmetler</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>Hizmetler</li>
          </ol>
        </div>
      </div>
    </section>

    <section id="services" class="services section-bg">
      <div class="container">
        <div class="card mb-3" v-for="item in list" :key="item.id">
          <div class="card-body">
            <div class="row">
              <div class="col-md-6">
                <div class="">
                  <img :src="item.imageUrl" class="w-100" />
                </div>
              </div>
              <div class="col-md-6">
                <h4 class="py-2 text-black border-bottom">{{ item.name }}</h4>
                <p class="text-black py-3" v-html="item.content"></p>
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
      GlobalService.Get(Endpoints.Service).then((res) => {
        this.list = res.data;
      });
    },
  },
};
</script>

<style>
</style>