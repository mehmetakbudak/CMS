<template>
  <div class="border-bottom">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item">
          <router-link class="text-secondary text-decoration-none" to="/"
            >Anasayfa</router-link
          >
        </li>
        <li class="breadcrumb-item active" aria-current="page">
          <router-link class="text-secondary text-decoration-none" to="/sss"
            >Sıkça Sorulan Sorular</router-link
          >
        </li>
      </ol>
    </nav>
  </div>
  <div class="row">
    <div class="col-md-3">
      <div class="pt-4">
        <Listbox
          v-model="selectedCategory"
          :options="categories"
          optionLabel="label"
          optionValue="id"
          :filter="true"
          @change="selectCategory($event)"
        />
      </div>
    </div>
    <div class="col-md-9">
      <h5 class="mt-4">{{ title }}</h5>
      <div class="mt-3">
        <Accordion>
          <AccordionTab
            :key="item.id"
            v-for="item in questions"
            :header="item.question"
          >
            {{ item.answer }}
          </AccordionTab>
        </Accordion>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      title: "",
      selectedCategory: null,
      categories: [
        {
          id: 1,
          label: "Popüler Sorular",
          to: "/sss/populer-sorular",
        },
        {
          id: 2,
          label: "Kolay İade ve Değişim",
          to: "/sss/kolay-iade-ve-degisim",
        },
        {
          id: 3,
          label: "Siparişler ve Teslimat",
          to: "/sss/siparisler-ve-teslimat",
        },
        {
          id: 4,
          label: "Üyelik",
          to: "/sss/uyelik",
        },
        {
          id: 5,
          label: "Ödeme Seçenekleri",
          to: "/sss/odeme-secenekleri",
        },
      ],
      questions: [
        {
          id: 1,
          question: "1 fdsdes dsfdsds fdsfsdfds dsfdsfds sfdsfds ",
          answer:
            "1 ffdsfds fsdfdsfdsfds fdsfdsfds fdfdfds fsdfdsf fsdafdsf fdsfdsf fdsfdsfds fdsfdsfd",
        },
        {
          id: 2,
          question: "2 fdsdes dsfdsds fdsfsdfds dsfdsfds sfdsfds ",
          answer:
            " 2 ffdsfds fsdfdsfdsfds fdsfdsfds fdfdfds fsdfdsf fsdafdsf fdsfdsf fdsfdsfds fdsfdsfd",
        },
        {
          id: 3,
          question: "3 fdsdes dsfdsds fdsfsdfds dsfdsfds sfdsfds ",
          answer:
            "3 ffdsfds fsdfdsfdsfds fdsfdsfds fdfdfds fsdfdsf fsdafdsf fdsfdsf fdsfdsfds fdsfdsfd",
        },
        {
          id: 4,
          question: "4 fdsdes dsfdsds fdsfsdfds dsfdsfds sfdsfds ",
          answer:
            "4 ffdsfds fsdfdsfdsfds fdsfdsfds fdfdfds fsdfdsf fsdafdsf fdsfdsf fdsfdsfds fdsfdsfd",
        },
      ],
    };
  },
  created() {
    if (this.categories.length > 0) {
      this.selectedCategory = this.categories[0].id;
      this.title = this.categories[0].label;
      this.$router.push(this.categories[0].to);
    }
  },
  methods: {
    selectCategory(e) {
      console.log(e);
    },
  },
};
</script>

<style>
</style>