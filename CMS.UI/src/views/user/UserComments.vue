<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-md-6">
          <h3>Yorumlarım</h3>
        </div>
        <div class="col-md-6">
          
          <Dropdown
            class="float-end"
            v-model="type"
            :options="sourceTypes"
            optionLabel="name"
            optionValue="id"
            @change="changeType"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <DataTable
        :value="comments"
        dataKey="id"
        :paginator="true"
        :rows="5"
        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
        :rowsPerPageOptions="[5, 10, 20, 50]"
        responsiveLayout="scroll"
        currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
        v-model:expandedRows="expandedRows"
      >
        <Column :expander="true" headerStyle="width: 3rem" />
        <Column header="" class="w-50px">
          <template #body="slotProps">
            <Button
              icon="pi pi-cog"
              class="p-button-rounded p-button-info p-button-sm"
              @click="toggleGridMenu($event, slotProps.data)"
            />
            <Menu ref="menu" :model="gridMenuItems" :popup="true" />
          </template>
        </Column>
        <Column field="source" header="Tipi"></Column>
        <Column field="status" header="Durumu"></Column>
        <Column field="insertedDate" header="Kayıt Tarihi" dataType="date">
          <template #body="{ data }">
            {{ dateFormatValue(data.insertedDate) }}
          </template>
        </Column>
        <Column field="updatedDate" header="Güncelleme Tarihi" dataType="date">
          <template #body="{ data }">
            {{
              data.updatedDate != null ? dateFormatValue(data.updatedDate) : ""
            }}
          </template>
        </Column>
        <template #expansion="slotProps">
          <div class="bg-light p-3">
            <div>{{ slotProps.data.description }}</div>
          </div>
        </template>
        <template #empty> Kayıt bulunamadı. </template>
      </DataTable>
    </div>
  </div>
</template>

<script>
import { Endpoints } from "../../services/Endpoints";
import { Constants } from "../../models/Constants";
import GlobalService from "../../services/GlobalService";
import dateFormat from "../../infrastructure/DateFormat";
import AlertService from "../../services/AlertService";

export default {
  mixins: [AlertService],
  data() {
    return {
      type: 0,
      comments: [],
      expandedRows: [],
      selectedComment: {},
      sourceTypes: Constants.SourceTypes,
      gridMenuItems: [
        {
          label: "Sil",
          command: () => {
            this.$confirm.require({
              message: "Silmek istediğinize emin misiniz?",
              header: "Silme Onayı",
              icon: "pi pi-exclamation-triangle",
              acceptLabel: "Evet",
              rejectLabel: "Hayır",
              accept: () => {
                GlobalService.DeleteByAuth(
                  Endpoints.Comment,
                  this.selectedComment.id
                )
                  .then((res) => {
                    this.getAll();
                    this.successMessage( res.data.message);
                  })
                  .catch((e) => {
                    this.errorMessage( e.response.data.message);
                  });
              },
            });
          },
        },
      ],
    };
  },
  methods: {
    getAll() {
      GlobalService.GetByAuth(
        `${Endpoints.Comment}/GetUserComments/${this.type}`
      ).then((res) => {
        this.comments = res.data;
      });
    },
    dateFormatValue(value) {
      return dateFormat.convert(value);
    },
    changeType() {
      this.getAll();
    },
    toggleGridMenu(event, data) {
      this.selectedComment = data;
      this.$refs.menu.toggle(event);
    },
  },
  created() {
    this.getAll();
  },
};
</script>

<style>
</style>