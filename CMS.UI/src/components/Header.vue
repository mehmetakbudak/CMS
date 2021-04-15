<template>
  <nav class="navbar navbar-light bg-light border-bottom">
    <div class="container">
      <a class="navbar-brand" href="#">CMS</a>
      <div>
          <vue-tree-nav :items="menuDataSource"></vue-tree-nav>
      </div>
    </div>
  </nav>
</template>

<script>
import axios from "axios";
  import treeNav from 'vue-tree-nav'

export default {
  created() {
    this.loadMenu();
  },
  components: {
      'vue-tree-nav': treeNav
  },
  data: function () {
    return {
      menuDataSource: [],
    };
  },
  methods: {
    loadMenu() {
      axios.get(`${process.env.VUE_APP_BASEURL}menu/frontend`).then((res) => {
        this.menuDataSource = res.data;
      });
    },
    itemClick(e) {
      this.$router.push(e.itemData.url).catch(() => {});
    },
  },
};
</script>