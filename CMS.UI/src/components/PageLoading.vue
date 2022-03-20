<template>
  <div class="text-center my-5" v-if="loading">
    <div>Yükleniyor...</div>
    <i class="pi pi-spin pi-spinner" style="font-size: 2rem"></i>
  </div>
</template>

<script>
export default {
  props: {
    loading: Boolean,
  },
};
</script>

<style>
</style>