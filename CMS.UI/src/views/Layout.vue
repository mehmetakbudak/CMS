<template>
  <div class="container">
    <Header />
    <div class="mt-3">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import Header from "../components/Header";

export default {
  components: {
    Header,
  },
};
</script>

<style>
</style>