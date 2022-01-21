<template>
  <div class="container">
    <Header />
    <div class="my-3">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import Header from "./Header.vue";

export default {
  components: {
    Header,
  },
};
</script>

<style>
</style>