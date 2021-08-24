<template>
  <div class="mt-3">
    <div class="mt-3 mb-3">
      <div>
        <Toast></Toast>
        <ConfirmDialog></ConfirmDialog>
      </div>

      <router-view></router-view>
    </div>
  </div>
</template>

<script>
export default {
  name: "App",
  components: {},
};
</script>

