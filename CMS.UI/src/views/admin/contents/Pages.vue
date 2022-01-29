<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h4>{{ title }}</h4>
        </div>
        <div class="col-6">
          <Button
            v-if="showGrid"
            icon="pi pi-plus"
            class="p-button-primary p-button-sm float-end"
            @click="add()"
          />
          <Button
            v-if="showForm"
            icon="pi pi-arrow-left"
            class="p-button-primary p-button-sm float-end"
            @click="reset()"
          />
        </div>
      </div>
    </div>
    <div class="card-body"></div>
  </div>
</template>

<script>
export default {
  name: "name",
  data() {
    return {
      title: "",
      showForm: false,
      showGrid: true,
    };
  },
  created() {
    this.reset();
  },
  methods: {
    add() {
      this.title = "Sayfa Ekle";
       this.showForm = true;
      this.showGrid = false;
    },
    reset() {
      this.title = "Sayfalar";
      this.showForm = false;
      this.showGrid = true;
    },
  },
};
</script>

<style lang="scss" scoped>
</style>