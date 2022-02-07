<template>
  <div class="card mt-3">
    <div class="card-body">
      <div class="row">
        <div class="col-md-8">
          <div class="row">
            <div class="col-md-8">
              <h3 class="p-2">Blog</h3>
            </div>
            <div class="col-md-4">
              <div class="p-inputgroup pe-2">
                <InputText placeholder="Ara" v-model="searchText" />
                <Button
                  icon="pi pi-search"
                  class="p-button-default"
                  @click="getAll()"
                />
              </div>
            </div>
          </div>
          <div class="row">
            <DataView
              :value="blogs"
              :layout="layout"
              :paginator="true"
              :rows="8"
            >
              <template #grid="slotProps">
                <div class="col-md-6 p-2 mb-3">
                  <div class="card">
                    <img
                      class="card-img-top"
                      src="http://via.placeholder.com/268x180"
                      :alt="slotProps.data.title"
                    />
                    <div class="card-body">
                      <div class="badge bg-secondary mt-2">
                        {{ slotProps.data.category }}
                      </div>
                      <h5 class="card-title mt-2 mb-2">
                        {{ slotProps.data.title }}
                      </h5>
                      <p class="card-text mt-2 mb-2">
                        {{ slotProps.data.description }}
                      </p>
                      <div class="mt-3"></div>
                    </div>
                    <div class="card-footer bg-white">
                      <router-link
                        :to="slotProps.data.url"
                        class="btn btn-primary float-end"
                        >Detaylar</router-link
                      >
                    </div>
                  </div>
                </div>
              </template>
            </DataView>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card">
            <div class="card-header bg-white py-3">
              <h4>Çok Okunanlar</h4>
            </div>
            <div class="card-body"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
export default {
  data() {
    return {
      layout: "grid",
      blogs: [],
      searchText: null,
    };
  },
  created() {
    this.getAll();
  },
  methods: {
    getAll() {
      var url = `${Endpoints.Blog}/GetByText`;
      if (this.searchText) {
        url = `${url}?text=${this.searchText}`;
      }
      GlobalService.Get(url).then((res) => {
        this.blogs = res.data;
      });
    },
  },
};
</script>
