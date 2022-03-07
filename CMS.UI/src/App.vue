<template>
  <div>
    <div>
      <Toast></Toast>
      <ConfirmDialog></ConfirmDialog>
    </div>

    <router-view :key="$route.fullPath"></router-view>
  </div>
</template>

<script>
export default {
  name: "App",
  components: {},
};
</script>
