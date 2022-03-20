<template>
  <div class="card mt-3">
    <div class="card-body">
      <div class="row">
        <div class="col-md-9 mb-3">
          <page-loading :loading="loading" />
          <div v-if="!loading">
            <div class="row">
              <div class="col-md-9">
                <h3 class="p-2">{{ title }}</h3>
              </div>
              <div class="col-md-3">
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
            <div class="row" v-if="!visibleError">
              <DataView
                :value="blogCategory.blogs"
                :layout="layout"
                :paginator="true"
                :rows="9"
              >
                <template #grid="slotProps">
                  <div class="col-md-4 p-2 mb-3">
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
                      <div class="card-footer bg-white border-top-0">
                        <router-link
                          :to="`${slotProps.data.url}/${slotProps.data.id}`"
                          class="btn btn-outline-primary float-end"
                          >Detaylar</router-link
                        >
                      </div>
                    </div>
                  </div>
                </template>
                <template #empty
                  ><p class="my-3 alert alert-info">Kayıt bulunamadı.</p>
                </template>
              </DataView>
            </div>
            <div v-if="visibleError">
              <div class="alert alert-danger mt-3">{{ message }}</div>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="mb-3">
            <h5 class="mb-3">Kategoriler</h5>
            <Listbox
              :options="blogCategories"
              :multiple="true"
              :filter="true"
              optionLabel="name"
              listStyle="max-height:250px"
              style="width: 100%"
              filterPlaceholder="Ara"
            >
              <template #option="slotProps">
                <router-link
                  class="text-dark text-decoration-none"
                  :to="`/blog/${slotProps.option.url}`"
                  >{{ slotProps.option.name }}</router-link
                >
              </template>
            </Listbox>
          </div>
          <div v-if="blogCategory?.blogs?.length > 0">
            <h5>Çok Okunanlar</h5>
            <div class="my-4" v-for="item in mostReadList" :key="item.id">
              <router-link
                class="text-decoration-none"
                :to="`/blog/${item.url}/${item.id}`"
              >
                <img
                  class="img-fluid w-100"
                  src="http://via.placeholder.com/268x180"
                />
                <div class="text-dark fw-bold mt-2">
                  {{ item.title }}
                </div>
              </router-link>
            </div>
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
      title: "",
      loading: true,
      blogCategory: {},
      blogCategories: [],
      mostReadList: [],
      searchText: null,
      visibleError: false,
      message: "",
    };
  },
  created() {
    this.getAll();
    this.getBlogCategories();
  },
  methods: {
    getAll() {
      var url = `${Endpoints.Blog}/GetByText?categoryUrl=${this.$route.params.categoryUrl}`;
      if (this.searchText) {
        url = `${url}&text=${this.searchText}`;
      }
      GlobalService.Get(url)
        .then((res) => {
          this.blogCategory = res.data;
          this.mostRead();
          this.title = "Blog - " + this.blogCategory.name;
          this.loading = false;
        })
        .catch((e) => {
          this.visibleError = true;
          this.message = e.response.data.message;
        });
    },
    getBlogCategories() {
      GlobalService.Get(Endpoints.BlogCategory).then((res) => {
        this.blogCategories = res.data;
      });
    },
    mostRead() {
      if (this.blogCategory) {
        GlobalService.Get(
          `${Endpoints.Blog}/MostReadByBlogCategoryId/${this.blogCategory.id}`
        ).then((res) => {
          this.mostReadList = res.data;
        });
      }
    },
  },
};
</script>
