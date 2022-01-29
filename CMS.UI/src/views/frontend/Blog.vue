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
                <InputText placeholder="Ara" />
                <Button icon="pi pi-search" class="p-button-default" />
              </div>
            </div>
          </div>
          <div class="row">
            <DataView
              :value="news"
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
                        to="/blog/detay"
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
export default {
  data() {
    return {
      layout: "grid",
      news: [
        {
          category: "Edebiyat",
          title: "Haber Başlık 1",
          description: "Haber İçerik 1",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 2",
          description: "Haber İçerik 2",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 3",
          description: "Haber İçerik 3",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 4",
          description: "Haber İçerik 4",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 5",
          description: "Haber İçerik 5",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 6",
          description: "Haber İçerik 6",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 7",
          description: "Haber İçerik 7",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 8",
          description: "Haber İçerik 8",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 9",
          description: "Haber İçerik 9",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 10",
          description: "Haber İçerik 10",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 11",
          description: "Haber İçerik 11",
        },
        {
          category: "Edebiyat",
          title: "Haber Başlık 12",
          description: "Haber İçerik 12",
        },
      ],
    };
  },
  created() {
    
  }
};
</script>
