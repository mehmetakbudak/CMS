<template>
  <div>
    <div>
      <Toast></Toast>
      <ConfirmDialog></ConfirmDialog>
    </div>

    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: "App",
  components: {},
};
</script>
