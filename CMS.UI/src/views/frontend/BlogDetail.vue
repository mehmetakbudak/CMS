<template>
  <main id="main">
    <section id="breadcrumbs" class="breadcrumbs">
      <div class="container">
        <div class="d-flex justify-content-between align-items-center">
          <h2>{{ blog.title }}</h2>
          <ol>
            <li><router-link to="/">Anasayfa</router-link></li>
            <li><router-link to="/blog">Blog</router-link></li>
            <li>{{ blog.title }}</li>
          </ol>
        </div>
      </div>
    </section>

    <section id="blog" class="blog">
      <div class="container">
        <div class="row">
          <div class="col-lg-8 entries">
            <article class="entry entry-single">
              <div class="entry-img">
                <img :src="blog.imageUrl" alt="" class="img-fluid" />
              </div>

              <h2 class="entry-title">
                {{ blog.title }}
              </h2>

              <div class="entry-meta">
                <ul>
                  <li class="d-flex align-items-center">
                    <i class="pi pi-user"></i>
                    {{ blog.userName }}
                  </li>
                  <li class="d-flex align-items-center">
                    <i class="pi pi-clock"></i>
                    <time>
                      {{ shortDateFormatValue(blog.insertedDate) }}
                    </time>
                  </li>
                  <li class="d-flex align-items-center">
                    <i class="pi pi-comments"></i>
                    {{ comments.length }} Yorum
                  </li>
                </ul>
              </div>

              <div class="entry-content" v-html="blog.content"></div>

              <div class="entry-footer">
                <i class="pi pi-folder"></i>
                <ul
                  class="cats ps-2"
                  v-for="blogCategory in blog?.blogCategories"
                  :key="blogCategory?.id"
                >
                  <li>
                    <router-link :to="`/blog/${blogCategory.url}`">{{
                      blogCategory.name
                    }}</router-link>
                  </li>
                </ul>

                <i class="pi pi-tags"></i>
                <ul class="tags ps-2">
                  <li><a href="#">Creative</a></li>
                  <li><a href="#">Tips</a></li>
                  <li><a href="#">Marketing</a></li>
                </ul>
              </div>
            </article>

            <div class="card box-shadow border-0 mb-5">
              <div class="card-header py-3 bg-white">
                <h4><i class="pi pi-pencil"></i> Yorum Ekle</h4>
              </div>
              <div class="card-body">
                <div class="py-3">
                  <label class="form-label">Yorumunuz:</label>
                  <Textarea
                    v-model="comment.description"
                    :autoResize="true"
                    rows="5"
                    class="w-100"
                  />
                  <Button
                    @click="commentSave"
                    label="Kaydet"
                    class="bg-green float-end mt-2"
                  />
                </div>
              </div>
            </div>

            <div class="card box-shadow border-0">
              <div class="card-header py-3 bg-white">
                <h4>
                  <i class="pi pi-comments"></i> {{ comments.length }} Yorum
                </h4>
              </div>
              <div class="card-body">
                
              </div>
            </div>
          </div>

          <div class="col-lg-4">
            <div class="sidebar">
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
                  <time> {{ shortDateFormatValue(item.insertedDate) }}</time>
                </div>
              </div>

              <h3 class="sidebar-title">Etiketler</h3>
              <div class="sidebar-item tags">
                <ul>
                  <li><a href="#">App</a></li>
                  <li><a href="#">IT</a></li>
                  <li><a href="#">Business</a></li>
                  <li><a href="#">Mac</a></li>
                  <li><a href="#">Design</a></li>
                  <li><a href="#">Office</a></li>
                  <li><a href="#">Creative</a></li>
                  <li><a href="#">Studio</a></li>
                  <li><a href="#">Smart</a></li>
                  <li><a href="#">Tips</a></li>
                  <li><a href="#">Marketing</a></li>
                </ul>
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
import { Enums } from "../../models/Enums";
import AlertService from "../../services/AlertService";
import dateFormat from "../../infrastructure/DateFormat";

export default {
  mixins: [AlertService],
  computed: {
    sourceId() {
      return parseInt(this.$route.params.id);
    },
    loggedIn() {
      return this.$store.state.auth.status.loggedIn;
    },
  },
  data() {
    return {
      visibleAnswered: false,
      searchText: "",
      answerText: "",
      blog: {},
      blogCategories: [],
      mostReadList: [],
      loading: true,
      comment: {
        id: 0,
        description: "",
        parentId: null,
        sourceType: Enums.SourceTypes.Blog,
        sourceId: 0,
      },
      comments: [],
    };
  },
  created() {
    this.getBlogDetail();
    this.seen();
    this.getBlogCategories();
    this.mostRead();
    this.getBlogComments();
  },
  methods: {
    getBlogDetail() {
      GlobalService.Get(`${Endpoints.Blog}/${this.$route.params.id}`).then(
        (res) => {
          this.blog = res.data;
          this.loading = false;
        }
      );
    },
    getBlogComments() {
      var data = {
        sourceId: this.sourceId,
        sourceType: Enums.SourceTypes.Blog,
      };
      GlobalService.PostByAuth(
        `${Endpoints.Comment}/GetSourceComments`,
        data
      ).then((res) => {
        this.comments = res.data;
      });
    },
    seen() {
      GlobalService.Put(
        `${Endpoints.Blog}/Seen/${this.$route.params.id}`,
        null
      );
    },
    getBlogCategories() {
      GlobalService.Get(Endpoints.BlogCategory).then((res) => {
        this.blogCategories = res.data;
      });
    },
    mostRead() {
      GlobalService.Get(`${Endpoints.Blog}/MostRead`).then((res) => {
        this.mostReadList = res.data;
      });
    },
    commentSave() {
      this.comment.sourceId = this.sourceId;
      GlobalService.PostByAuth(Endpoints.Comment, this.comment)
        .then(() => {
          this.comment = {};
          this.getBlogComments();
          this.successMessage(
            this,
            "Yorum başarıyla kaydedildi. Onay sürecinden sonra yayınlanabilecektir."
          );
        })
        .catch((e) => {
          this.errorMessage(e.response.data.message);
        });
    },
    dateFormatValue(value) {
      return dateFormat.convert(value);
    },
    shortDateFormatValue(value) {
      return dateFormat.convertShortDate(value);
    },
    answerComment(e) {
      this.visibleAnswered = true;
      this.answerText = e.userFullName;
      this.comment.parentId = e.id;
      this.comment.description = "";
    },
    cancelAnswer() {
      this.visibleAnswered = false;
      this.answerText = "";
      this.comment.parentId = null;
      this.comment.description = "";
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

<style  scoped lang="css">
::v-deep(.p-treenode-label) {
  width: 100% !important;
}

::v-deep(.p-tree) {
  padding: unset;
  border: unset;
}

.box-shadow {
  box-shadow: 0 4px 16px rgb(0 0 0 / 10%) !important;
}
</style>