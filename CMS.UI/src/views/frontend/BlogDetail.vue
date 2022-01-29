<template>
  <div class="card">
    <div class="card-body">
      <div class="row">
        <div class="col-md-8">
          <nav style="--bs-breadcrumb-divider: '>'" class="small" aria-label="breadcrumb">
            <ol class="breadcrumb">
              <li class="breadcrumb-item">
                <router-link class="text-secondary text-decoration-none" to="/"
                  ><i class="pi pi-home"></i> Anasayfa</router-link
                >
              </li>
              <li class="breadcrumb-item">
                <router-link
                  class="text-secondary text-decoration-none"
                  to="/blog"
                >
                  Blog</router-link
                >
              </li>
              <li class="breadcrumb-item active" aria-current="page">
                Blog Detay
              </li>
            </ol>
          </nav>
          <h5 class="py-3">Tiyatro Kooperatifi, 2. Olağan Genel Kurulu’nu gerçekleştirdi</h5>
          <img
            class="img-fluid w-100 my-3"
            src="https://www.murekkephaber.com/images/haberler/2021/04/tiyatro-kooperatifi-2-olagan-genel-kurulu-nu-gerceklestirdi.jpg"
          />
          <div class="my-4">
            Mayıs 2018’de 13 özel tiyatronun girişimiyle çalışmalarına başlayan
            ve bugün 64 özel tiyatronun dahil olduğu büyük ve güçlü bir yapı
            olarak yola devam eden Tiyatro Kooperatifi, 6 Nisan 2021 Salı günü
            2. Olağan Genel Kurulu’nu gerçekleştirdi. Pandemi koşulları
            gözetilerek DasDas’ın açık avlusunda gerçekleştirilen toplantıda,
            Kasım 2019’dan beri gönüllü olarak görev yapan Yönetim Kurulu
            Başkanı Iraz Yöntem, Başkan Yardımcısı Yeşim Özsoy, Yönetim Kurulu
            Üyeleri Ersin Umut Güler, Mert Fırat ve Muharrem Uğurlu Yönetim
            Kurulu’na yeniden seçilerek bir yıl için daha aynı sorumluluğu
            üstlendi. Tiyatro Kooperatifi, pandemi sürecinde büyük yara alan ve
            kapanma tehlikesiyle karşı karşıya olan özel tiyatrolar için acil
            çözüm çağrısını sürdürüyor. Sektörün ayakta kalmasının ancak somut
            ve sürdürülebilir çözümlerle mümkün olduğunu vurgulayan kooperatif,
            telafisi belki de on yıllar alacak kayıpların yaşanmaması için
            ilgilileri sorumlu davranmaya davet ediyor.
          </div>
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
                <div class="mt-3">
                  <div class="p-inputgroup mb-3">
                    <span class="p-inputgroup-addon">
                      <i class="pi pi-user"></i>
                    </span>
                    <InputText
                      v-model="comment.nameSurname"
                      type="text"
                      placeholder="Adı Soyadı"
                    />
                  </div>

                  <div class="p-inputgroup mb-3">
                    <span class="p-inputgroup-addon">
                      <i class="pi pi-envelope"></i>
                    </span>
                    <InputText
                      v-model="comment.emailAddress"
                      type="email"
                      placeholder="Email Adresi"
                    />
                  </div>

                  <div class="p-inputgroup mb-3">
                    <span class="p-inputgroup-addon">
                      <i class="pi pi-pencil"></i>
                    </span>
                    <Textarea
                      v-model="comment.comment"
                      placeholder="Yorum"
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
                  <Tree :value="comments">
                    <template #default="comment">
                      <div class="card">
                        <div class="card-header">
                          <div class="row">
                            <div class="col-9">
                              {{ comment.node.userName }}
                            </div>
                            <div class="col-3">
                              <small class="float-end p-text-italic"
                                >09.04.2021</small
                              >
                            </div>
                          </div>
                        </div>
                        <div class="card-body">
                          {{ comment.node.detail }}
                        </div>
                      </div>
                    </template>
                  </Tree>
                </div>
              </AccordionTab>
            </Accordion>
          </div>
          <div class="mt-3">
            <h5 class="mb-3">Bu Haberler de ilgilinizi çekebilir</h5>
            <Galleria
              :value="images"
              :numVisible="5"
              :circular="true"
              :showItemNavigators="true"
              :showItemNavigatorsOnHover="true"
            >
              <template #item="slotProps">
                <router-link to="/haberler/detay">
                  <img
                    :src="slotProps.item.itemImageSrc"
                    :alt="slotProps.item.alt"
                    style="width: 100%; display: block"
                  />
                </router-link>
              </template>
              <template #thumbnail="slotProps">
                <router-link to="/haberler/detay">
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
                  to="/haberler/detay"
                >
                  <h4 style="margin-bottom: 0.5rem">{{ item.title }}</h4>
                  <p>{{ item.alt }}</p>
                </router-link>
              </template>
            </Galleria>
          </div>
        </div>
        <div class="col-md-4">
          <h5>Çok Okunan Haberler</h5>
          <div>
            <router-link class="text-decoration-none" to="./haberler/detay">
              <img
                class="img-fluid my-3 w-100"
                src="https://www.murekkephaber.com/images/haberler/2021/04/semaver-ve-kumpanya-online-izlenebilecek_t.jpg"
              />
              <div class="text-dark fw-bold mt-2">
                Semaver ve Kumpanya online izlenebilecek
              </div>
            </router-link>
          </div>
          <div class="mt-4 mb-4">
            <router-link class="text-decoration-none" to="./haberler/detay">
              <img
                class="img-fluid w-100"
                src="https://www.murekkephaber.com/images/haberler/2021/04/14-egitimde-edebiyat-semineri-10-nisan-da_t.jpg"
              />
              <div class="text-dark fw-bold mt-2">
                14. Eğitimde Edebiyat Semineri, 10 Nisanda
              </div>
            </router-link>
          </div>
          <div class="mt-4 mb-4">
            <router-link class="text-decoration-none" to="./haberler/detay">
              <img
                class="img-fluid w-100"
                src="https://www.murekkephaber.com/images/haberler/2021/04/black-widow-un-yeni-fragmani-yayinlandi_t.jpg"
              />
              <div class="text-dark fw-bold mt-2">
                Black Window yeni fragmanı yayınlandı
              </div>
            </router-link>
          </div>
          <div class="mt-4 mb-4">
            <router-link class="text-decoration-none" to="./haberler/detay">
              <img
                class="img-fluid w-100"
                src="https://www.murekkephaber.com/images/haberler/2021/04/dunyanin-en-pahali-tablosu-nft-olarak-satista_t.jpg"
              />
              <div class="text-dark fw-bold mt-2">
                Dünyanın en pahalı tablosu NFT olarak satışta
              </div>
            </router-link>
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
      comment: {
        nameSurname: "",
        emailAddress: "",
        comment: "",
      },
      comments: [
        {
          userName: "Yorum Başlık 1",
          detail: "Yorum İçerik 1",
          children: [
            {
              userName: "Elif Demirtaş",
              detail: "dasfsd fdsf fdsfsd fdsfsd",
            },
            {
              userName: "Ali Demirtaş",
              detail: "fdsf dsgdg gdfgfd gfdgdf",
              children: [
                {
                  userName: "Mehmet Akbudak",
                  detail: "safsdf fsdfds fsdf fdsfdsf",
                },
                {
                  userName: "Güler Akbudak",
                  detail: "fsdf dsfdsf sdfds",
                },
              ],
            },
          ],
        },
        {
          userName: "Sezen Aksu",
          detail: "sdasd dsadas dsadasd",
          children: [
            {
              userName: "Akif Sarı",
              detail: "fdfsd fdsfsdf dsfsdfd fdsfsd",
            },
            {
              userName: "Hediye Kara",
              detail: "dsdads dsgfdgd fdgfdg",
            },
          ],
        },
      ],
      images: [
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria1.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria1s.jpg",
          alt: "Description for Image 1",
          title: "Title 1",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria2.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria2s.jpg",
          alt: "Description for Image 2",
          title: "Title 2",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria3.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria3s.jpg",
          alt: "Description for Image 3",
          title: "Title 3",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria4.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria4s.jpg",
          alt: "Description for Image 4",
          title: "Title 4",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria5.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria5s.jpg",
          alt: "Description for Image 5",
          title: "Title 5",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria6.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria6s.jpg",
          alt: "Description for Image 6",
          title: "Title 6",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria7.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria7s.jpg",
          alt: "Description for Image 7",
          title: "Title 7",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria8.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria8s.jpg",
          alt: "Description for Image 8",
          title: "Title 8",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria9.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria9s.jpg",
          alt: "Description for Image 9",
          title: "Title 9",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria10.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria10s.jpg",
          alt: "Description for Image 10",
          title: "Title 10",
        },
        {
          itemImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria11.jpg",
          thumbnailImageSrc:
            "https://primefaces.org/primevue/showcase/demo/images/galleria/galleria11s.jpg",
          alt: "Description for Image 11",
          title: "Title 11",
        },
      ],
    };
  },
  methods: {
    commentSave() {
      console.log(this.comment);
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