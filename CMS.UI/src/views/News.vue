<template>
  <div class="card mt-3">
    <div class="card-body">
      <div class="row">
        <div class="col-md-6">
          <h3 class="p-2">Haberler</h3>
        </div>
        <div class="col-md-6"></div>
      </div>
      <div class="row">
        <DataView :value="news" :layout="layout" :paginator="true" :rows="9">
          <template #grid="slotProps">
            <div class="col-md-4 p-2">
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
                <div class="card-footer">
                  <router-link
                    to="/haberler/detay"
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
};
</script>
