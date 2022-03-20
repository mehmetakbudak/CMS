<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <h5>Yorumlar</h5>
    </div>
    <div class="card-body">
      <TabView v-model:activeIndex="active" @click="getAll">
        <TabPanel header="Onay Bekliyor"></TabPanel>
        <TabPanel header="Onaylandı"></TabPanel>
        <TabPanel header="Reddedildi"></TabPanel>
      </TabView>
      <div class="alert alert-info mb-0">
        İşlem yapmak istediğiniz yorumu seçiniz.
      </div>
      <div class="my-3">
        <DataTable
          :loading="loading"
          showGridlines
          :value="comments"
          dataKey="id"
          :paginator="true"
          :rows="5"
          v-model:selection="selectedComment"
          v-model:expandedRows="expandedRows"
          selectionMode="single"
          @rowSelect="onRowSelect"
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
          :rowsPerPageOptions="[5, 10, 20, 50]"
          responsiveLayout="scroll"
          currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
        >
          <Column :expander="true" headerStyle="width: 3rem" />
          <Column field="source" header="Tipi"></Column>
          <Column field="status" header="Durumu"></Column>
          <Column field="userFullName" header="Adı Soyadı"></Column>
          <Column field="insertedDate" header="Kayıt Tarihi" dataType="date">
            <template #body="{ data }">
              {{ dateFormatValue(data.insertedDate) }}
            </template>
          </Column>
          <Column
            field="updatedDate"
            header="Güncelleme Tarihi"
            dataType="date"
          >
            <template #body="{ data }">
              {{ dateFormatValue(data.updatedDate) }}
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
  </div>
</template>

<script>
import { Endpoints } from "../../../../services/Endpoints";
import GlobalService from "../../../../services/GlobalService";
import DateFormat from "../../../../infrastructure/DateFormat";
import AlertService from "../../../../services/AlertService";

export default {
  name: "name",
  mixins: [AlertService],
  data() {
    return {
      active: 0,
      loading: true,
      comments: [],
      selectedComment: {},
      expandedRows: [],
    };
  },
  created() {
    this.getAll();
  },
  methods: {
    getAll() {
      this.loading = true;
      GlobalService.GetByAuth(
        `${Endpoints.Admin.Comment}/${this.active + 1}`
      ).then((res) => {
        this.comments = res.data;
        this.loading = false;
      });
    },
    dateFormatValue(value) {
      if (value) {
        return DateFormat.convert(value);
      }
    },
    onRowSelect(e) {
      this.$router.push(`/admin/icerikler/yorumlar/${e.data.id}`);
    },
  },
};
</script>

<style>
.p-tabview .p-tabview-panels {
  padding: 10px;
}
</style>