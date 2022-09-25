<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>{{ page.title }}</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>{{ page.title }}</li>
          </ol>
        </div>
      </div>
    </section>

    <section id="pages">
      <div class="container">
        <div v-html="page.content"></div>
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
      page: {},
    };
  },
  created() {
    this.getPageInfo();
  },
  methods: {
    getPageInfo() {
      var url = this.$route.params.url;
      GlobalService.Get(`${Endpoints.Page}/GetByUrl/${url}`)
        .then((res) => {
          this.page = res.data;
        })
        .catch((e) => {
          this.visibleError = true;
          this.message = e.response.data.message;
        });
    },
  },
};
</script>

<style scoped>
::v-deep(.p-tree .p-tree-container .p-treenode .p-treenode-content) {
  padding: unset;
}
::v-deep(.p-tree) {
  padding: unset;
  border: unset;
}
</style>