<template>
  <div>
    <Header></Header>
    <div>
      <router-view :key="$route.fullPath"></router-view>
    </div> 
    <Footer></Footer>
  </div>
</template>

<script>
import Header from "./Header.vue";
import Footer from "./Footer.vue";

export default {
  components: {
    Header,
    Footer,
  },
};
</script>

<style>
</style>