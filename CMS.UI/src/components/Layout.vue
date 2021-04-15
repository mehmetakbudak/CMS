<template>
  <div>
    <div><Header/></div>
    <div>
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import Header from "./Header"

export default {
  components: {
    Header
  },
};
</script>