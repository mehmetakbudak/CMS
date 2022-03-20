<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h4>Yorum Detayları</h4>
        </div>
        <div class="col-6">
          <Button
            icon="pi pi-arrow-left"
            class="p-button-primary p-button-sm float-end"
            @click="reset()"
          />
        </div>
      </div>
    </div>
    <div class="card-body mx-3">
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Adı Soyadı</div>
        <div class="col-md-10">{{ comment.userFullName }}</div>
      </div>
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Tipi</div>
        <div class="col-md-10">{{ comment.source }}</div>
      </div>
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Durumu</div>
        <div class="col-md-10">{{ comment.status }}</div>
      </div>
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Yorumu</div>
        <div class="col-md-10">{{ comment.description }}</div>
      </div>
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Kaynağı</div>
        <div class="col-md-10">
          <a target="_blank" v-bind:href="comment.sourceUrl">{{
            comment.sourceTitle
          }}</a>
        </div>
      </div>
      <div class="row my-3 pb-3 border-bottom" v-if="comment.parentId">
        <div class="col-md-2 fw-bold">
          <a class="text-dark" v-bind:href="`/admin/icerikler/yorumlar/${comment.parentId}`">
            Bağlı Yorumu</a
          >
        </div>
        <div class="col-md-10">{{ comment.parentDescription }}</div>
      </div>
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Kayıt Tarihi</div>
        <div class="col-md-10">{{ dateFormatValue(comment.insertedDate) }}</div>
      </div>
      <div class="row my-3 pb-3 border-bottom">
        <div class="col-md-2 fw-bold">Güncelleme Tarihi</div>
        <div class="col-md-10">{{ dateFormatValue(comment.updatedDate) }}</div>
      </div>
      <div class="my-3">
        <Button
          v-if="comment.commentStatus !== commentStatus.Approved"
          label="Onayla"
          icon="pi pi-check"
          class="p-button-success p-button-sm"
          @click="approve()"
        />
        <Button
          v-if="comment.commentStatus !== commentStatus.Rejected"
          label="Reddet"
          icon="pi pi-times"
          class="p-button-warning p-button-sm ms-2"
          @click="reject()"
        />
        <Button
          label="Sil"
          icon="pi pi-trash"
          class="p-button-danger p-button-sm ms-2"
          @click="deleteComment()"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { Endpoints } from "../../../../services/Endpoints";
import GlobalService from "../../../../services/GlobalService";
import DateFormat from "../../../../infrastructure/DateFormat";
import { Enums } from "../../../../models/Enums";
import AlertService from "../../../../services/AlertService";

export default {
  mixins: [AlertService],
  computed: {
    id() {
      return parseInt(this.$route.params.id);
    },
  },
  data() {
    return {
      comment: {},
      commentStatus: Enums.CommentStatus,
    };
  },
  created() {
    this.getById();
  },
  methods: {
    getById() {
      GlobalService.GetByAuth(
        `${Endpoints.Admin.Comment}/GetDetail/${this.$route.params.id}`
      ).then((e) => {
        this.comment = e.data;
      });
    },
    dateFormatValue(value) {
      if (value) {
        return DateFormat.convert(value);
      }
    },
    approve() {
      this.$confirm.require({
        message: "Onaylamak istediğinize emin misiniz?",
        header: "Yorum Onaylama",
        icon: "pi pi-exclamation-triangle",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        accept: () => {
          GlobalService.PutByAuth(Endpoints.Admin.Comment, {
            id: this.id,
            commentStatus: Enums.CommentStatus.Approved,
          })
            .then(() => {
              this.$router.push(`/admin/icerikler/yorumlar`);
              this.successMessage(this, "Yorum başarıyla onaylandı.");
            })
            .catch((e) => {
              this.errorMessage(this, e.response.data.message);
            });
        },
      });
    },
    reject() {
      this.$confirm.require({
        message: "Reddetmek istediğinize emin misiniz?",
        header: "Yorum Reddet",
        icon: "pi pi-exclamation-triangle",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        accept: () => {
          GlobalService.PutByAuth(Endpoints.Admin.Comment, {
            id: this.id,
            commentStatus: Enums.CommentStatus.Rejected,
          })
            .then(() => {
              this.$router.push(`/admin/icerikler/yorumlar`);
              this.successMessage(this, "Yorum başarıyla reddedildi.");
            })
            .catch((e) => {
              this.errorMessage(this, e.response.data.message);
            });
        },
      });
    },
    deleteComment() {
      this.$confirm.require({
        message: "Silmek istediğinize emin misiniz?",
        header: "Silme Onayı",
        icon: "pi pi-exclamation-triangle",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        accept: () => {
          GlobalService.DeleteByAuth(Endpoints.Admin.Comment, this.id)
            .then((res) => {
              this.$router.push(`/admin/icerikler/yorumlar`);
              this.successMessage(this, res.data.message);
            })
            .catch((e) => {
              this.errorMessage(this, e.response.data.message);
            });
        },
      });
    },
    reset() {
      this.$router.push(`/admin/icerikler/yorumlar`);
    },
  },
};
</script>

<style>
</style>