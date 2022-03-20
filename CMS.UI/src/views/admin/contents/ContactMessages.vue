<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <h5>İletişim Mesajları</h5>
    </div>
    <div class="card-body">
      <DataTable
        :loading="loading"
        showGridlines
        :value="contactMessages"
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
        <Column field="contactCategory.name" header="Konusu"></Column>
        <Column field="name" header="Adı"></Column>
        <Column field="surname" header="Soyadı"></Column>
        <Column field="emailAddress" header="Email Adres"></Column>
        <Column field="insertedDate" header="Kayıt Tarihi" dataType="date">
          <template #body="{ data }">
            {{ dateFormatValue(data.insertedDate) }}
          </template>
        </Column>
        <template #expansion="slotProps">
          <div class="bg-light p-3">
            <div>{{ slotProps.data.message }}</div>
          </div>
        </template>
        <template #empty> Kayıt bulunamadı. </template>
      </DataTable>
    </div>
  </div>
</template>

<script>
import DateFormat from "../../../infrastructure/DateFormat";
import AlertService from "../../../services/AlertService";
import { Endpoints } from "../../../services/Endpoints";
import GlobalService from "../../../services/GlobalService";
export default {
  name: "name",
  mixins: [AlertService],
  data() {
    return {
      loading: true,
      contactMessages: [],
      expandedRows: [],
      selectedMessage: {},
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
                  Endpoints.Admin.Contact,
                  this.selectedMessage.id
                )
                  .then((res) => {
                    this.getAll();
                    this.successMessage(this, res.data.message);
                  })
                  .catch((e) => {
                    this.errorMessage(this, e.response.data.message);
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
      this.loading = true;
      GlobalService.GetByAuth(`${Endpoints.Admin.Contact}`).then((res) => {
        this.contactMessages = res.data;
        this.loading = false;
      });
    },
    dateFormatValue(value) {
      return DateFormat.convert(value);
    },
    changeType() {
      this.getAll();
    },
    toggleGridMenu(event, data) {
      this.selectedMessage = data;
      this.$refs.menu.toggle(event);
    },
  },
  created() {
    this.getAll();
  },
};
</script>

<style lang="scss" scoped>
</style>