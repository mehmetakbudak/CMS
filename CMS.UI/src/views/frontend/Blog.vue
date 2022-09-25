<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Blog</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li>Blog</li>
          </ol>
        </div>
      </div>
    </section>

    <section id="blog" class="blog">
      <div class="container">
        <div class="row">
          <div class="col-lg-8 entries">
            <DataView :value="blogs" layout="grid" :paginator="true" :rows="5">
              <template #grid="slotProps">
                <article class="entry">
                  <div class="entry-img">
                    <router-link
                      :to="`/blog/${slotProps.data.url}/${slotProps.data.id}`"
                    >
                      <img
                        :src="slotProps.data.imageUrl"
                        alt=""
                        class="img-fluid"
                      />
                    </router-link>
                  </div>

                  <h2 class="entry-title">
                    <router-link
                      :to="`/blog/${slotProps.data.url}/${slotProps.data.id}`"
                    >
                      {{ slotProps.data.title }}
                    </router-link>
                  </h2>

                  <div class="entry-meta">
                    <ul>
                      <li class="d-flex align-items-center">
                        <i class="pi pi-user"></i>
                        {{ slotProps.data.userName }}
                      </li>
                      <li class="d-flex align-items-center">
                        <i class="pi pi-clock"></i>
                        <time>
                          {{ dateFormatValue(slotProps.data.insertedDate) }}
                        </time>
                      </li>
                      <li class="d-flex align-items-center">
                        <i class="pi pi-comments"></i>
                        {{ slotProps.data.commentCount }} Yorum
                      </li>
                    </ul>
                  </div>

                  <div class="entry-content">
                    <p>
                      {{ slotProps.data.description }}
                    </p>
                    <div class="read-more">
                      <router-link
                        :to="`/blog/${slotProps.data.url}/${slotProps.data.id}`"
                        >Daha Fazlası &nbsp;
                        <i class="pi pi-arrow-circle-right"></i
                      ></router-link>
                    </div>
                  </div>
                </article>
              </template>
              <template #empty
                ><p class="my-3 alert alert-info">Kayıt bulunamadı.</p>
              </template>
            </DataView>
          </div>
          <div class="col-lg-4">
            <div class="sidebar border">
              <h3 class="sidebar-title">Ara</h3>
              <div class="sidebar-item search-form">
                <div class="p-inputgroup">
                  <InputText placeholder="Ara" v-model="searchText" />
                  <Button
                    icon="pi pi-search"
                    class="bg-green"
                    @click="search()"
                  />
                </div>
              </div>
              <h3 class="sidebar-title">Kategoriler</h3>
              <div class="sidebar-item categories">
                <ul>
                  <li
                    class="py-2"
                    v-for="blogCategory in blogCategories"
                    :key="blogCategory.id"
                  >
                    <router-link :to="`/blog/${blogCategory.url}`"
                      >{{ blogCategory.name }}
                      <span>({{ blogCategory.blogCount }})</span></router-link
                    >
                  </li>
                </ul>
              </div>
              <h3 class="sidebar-title">Çok Okunanlar</h3>
              <div class="sidebar-item recent-posts">
                <div
                  class="post-item clearfix py-2"
                  v-for="item in mostReadList"
                  :key="item.id"
                >
                  <router-link :to="`/blog/${item.url}/${item.id}`">
                    <img :src="item.imageUrl" alt="" />
                  </router-link>
                  <h4>
                    <router-link :to="`/blog/${item.url}/${item.id}`">{{
                      item.title
                    }}</router-link>
                  </h4>
                  <time> {{ dateFormatValue(item.insertedDate) }}</time>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import GlobalService from "../../services/GlobalService";
import dateFormat from "../../infrastructure/DateFormat";

export default {
  data() {
    return {
      title: "",
      loading: true,
      blogs: [],
      blogCategories: [],
      mostReadList: [],
      searchText: null,
      visibleError: false,
      message: "",
    };
  },
  created() {
    if (this.$route.query.ara) {
      this.searchText = this.$route.query.ara;
    }

    this.getAll();
    this.getBlogCategories();
    this.mostRead();
  },
  methods: {
    getAll() {
      var url = `${Endpoints.Blog}/GetBlogs`;
      if (this.searchText) {
        url = `${url}?text=${this.searchText}`;
      }
      GlobalService.Get(url)
        .then((res) => {
          this.blogs = res.data;
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
    dateFormatValue(value) {
      return dateFormat.convertShortDate(value);
    },
    mostRead() {
      GlobalService.Get(`${Endpoints.Blog}/MostRead`).then((res) => {
        this.mostReadList = res.data;
      });
    },
    search() {
      if (this.searchText) {
        this.$router.push(`/blog?ara=${this.searchText}`);
      } else {
        this.$router.push("/blog");
      }
    },
  },
};
</script>
