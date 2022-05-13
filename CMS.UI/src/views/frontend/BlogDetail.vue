<template>
  <div class="card">
    <div class="card-body">
      <div class="row">
        <div class="col-md-9">
          <page-loading :loading="loading" />
          <div v-if="!loading">
            <div class="d-flex flex-row bd-highlight mb-3">
              <div class="p-1 bd-highlight">
                <router-link
                  class="text-secondary text-decoration-none small"
                  to="/"
                  ><i class="pi pi-home"></i> Anasayfa</router-link
                >
              </div>
              <div class="p-1 bd-highlight">
                <i class="pi pi-angle-right text-secondary"></i>
              </div>
              <div class="p-1 bd-highlight">
                <div class="dropdown">
                  <div
                    class="
                      btn btn-outline-secondary btn-sm
                      dropdown-toggle
                      small
                    "
                    type="button"
                    id="blogCategoryList"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                  >
                    Kategoriler
                  </div>
                  <ul class="dropdown-menu" aria-labelledby="blogCategoryList">
                    <li v-for="item in blog.blogCategories" :key="item.id">
                      <router-link
                        class="
                          dropdown-item
                          text-secondary text-decoration-none
                        "
                        :to="`/blog/${item.url}`"
                      >
                        {{ item.name }}</router-link
                      >
                    </li>
                  </ul>
                </div>
              </div>
              <div class="p-1 bd-highlight">
                <i class="pi pi-angle-right text-secondary"></i>
              </div>
              <div class="p-1 bd-highlight text-secondary small">
                {{ blog.title }}
              </div>
            </div>
            <h5 class="py-3">
              {{ blog.title }}
            </h5>
            <img
              class="img-fluid w-100 my-3"
              src="https://www.murekkephaber.com/images/haberler/2021/04/tiyatro-kooperatifi-2-olagan-genel-kurulu-nu-gerceklestirdi.jpg"
            />
            <div class="my-4" v-html="blog.content"></div>
            <div class="d-flex flex-column flex-sm-row bd-highlight mt-4 pb-3">
              <div class="p-pr-1 p-pb-1">
                <router-link
                  to="/haberler/etiketler/detay"
                  class="btn btn-outline-secondary text-dark"
                >
                  tiyatro kooperatifi
                </router-link>
              </div>
              <div class="p-pr-1 p-pb-1">
                <router-link
                  to="/haberler/etiketler/detay"
                  class="btn btn-outline-secondary"
                >
                  tiyatro kooperatifi girişimi
                </router-link>
              </div>
              <div class="p-pr-1 p-pb-1">
                <router-link
                  to="/haberler/etiketler/detay"
                  class="btn btn-outline-secondary"
                >
                  izmir tiyatro kooperatifi
                </router-link>
              </div>
            </div>
            <div class="mt-3 mb-3">
              <Accordion :activeIndex="0" icon="comment">
                <AccordionTab>
                  <template #header>
                    <i class="pi pi-plus"></i>
                    <span>&nbsp; Yorum Ekle</span>
                  </template>
                  <div v-if="!loggedIn">
                    <div class="alert alert-info">
                      Yorum eklemek için
                      <router-link to="/giris" class="btn btn-outline-primary">
                        Giriş Yap</router-link
                      >
                    </div>
                  </div>
                  <div class="mt-3" v-if="loggedIn">
                    <div class="w-100 mb-3" v-if="visibleAnswered">
                      Cevaplanıyor:
                      <span class="fw-bold"> {{ answerText }} </span>
                      <a
                        class="btn btn-outline-secondary btn-sm ms-3"
                        @click="cancelAnswer()"
                        >Vazgeç</a
                      >
                    </div>
                    <div class="p-inputgroup mb-3">
                      <Textarea
                        class="w-100"
                        v-model="comment.description"
                        placeholder="Yorumunuz"
                        :autoResize="true"
                        rows="5"
                        cols="30"
                      />
                    </div>
                    <div class="pb-5">
                      <button
                        @click="commentSave"
                        type="submit"
                        class="btn btn-primary float-end"
                      >
                        Kaydet
                      </button>
                    </div>
                  </div>
                </AccordionTab>
              </Accordion>
            </div>
            <div class="mt-3 mb-3">
              <Accordion :activeIndex="0">
                <AccordionTab>
                  <template #header>
                    <i class="pi pi-comments"></i>
                    <span>&nbsp; Yorumlar</span>
                  </template>
                  <div>
                    <div v-if="comments.length == 0">
                      Henüz yorum yapılmamış.
                    </div>
                    <Tree :value="comments" :paginator="true" :rows="1">
                      <template #default="comment">
                        <div class="card">
                          <div class="card-header">
                            <div class="row">
                              <div class="col-9">
                                <i class="pi pi-user me-1"></i>
                                {{ comment.node.userFullName }}
                              </div>
                              <div class="col-3">
                                <small class="float-end"
                                  ><i class="pi pi-clock me-1"></i>
                                  {{
                                    dateFormatValue(comment.node.insertedDate)
                                  }}</small
                                >
                              </div>
                            </div>
                          </div>
                          <div class="card-body">
                            <div class="my-2">
                              {{ comment.node.description }}
                            </div>
                            <div class="my-1">
                              <a
                                class="btn btn-outline-secondary btn-sm"
                                @click="answerComment(comment.node)"
                                >Cevap Ver</a
                              >
                            </div>
                          </div>
                        </div>
                      </template>
                    </Tree>
                  </div>
                </AccordionTab>
              </Accordion>
            </div>
            <div class="mt-5">
              <h5 class="mb-3">Bu Haberler de ilgilinizi çekebilir</h5>
              <!-- <Galleria
              :value="images"
              :numVisible="5"
              :circular="true"
              :showItemNavigators="true"
              :showItemNavigatorsOnHover="true"
            >
              <template #item="slotProps">
                <router-link>
                  <img
                    :src="slotProps.item.itemImageSrc"
                    :alt="slotProps.item.alt"
                    style="width: 100%; display: block"
                  />
                </router-link>
              </template>
              <template #thumbnail="slotProps">
                <router-link>
                  <img
                    :src="slotProps.item.thumbnailImageSrc"
                    :alt="slotProps.item.alt"
                    style="display: block"
                  />
                </router-link>
              </template>
              <template #caption="{ item }">
                <router-link
                  class="text-white text-decoration-none"
                >
                  <h4 style="margin-bottom: 0.5rem">{{ item.title }}</h4>
                  <p>{{ item.alt }}</p>
                </router-link>
              </template>
            </Galleria> -->
            </div>
          </div>
        </div>
        <div class="col-md-3">
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
      answerText: "",
      blog: {},
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
      images: [
        {
          itemImageSrc: "http://via.placeholder.com/268x180",
          thumbnailImageSrc: "http://via.placeholder.com/268x180",
          alt: "Description for Image 1",
          title: "Title 1",
        },
        {
          itemImageSrc: "http://via.placeholder.com/268x180",
          thumbnailImageSrc: "http://via.placeholder.com/268x180",
          alt: "Description for Image 2",
          title: "Title 2",
        },
        {
          itemImageSrc: "http://via.placeholder.com/268x180",
          thumbnailImageSrc: "http://via.placeholder.com/268x180",
          alt: "Description for Image 3",
          title: "Title 3",
        },
      ],
    };
  },
  created() {
    this.getBlogDetail();
    this.seen();
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
          this.errorMessage( e.response.data.message);
        });
    },
    dateFormatValue(value) {
      return dateFormat.convert(value);
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
</style>