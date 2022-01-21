<template>
  <div class="row mb-3">
    <div class="col-md-9">
      <Card>
        <template #content>
          <div>
            <Galleria
              :value="carouselList"
              :responsiveOptions="responsiveOptions"
              :numVisible="5"
              :circular="true"
              :showItemNavigators="true"
              :showThumbnails="false"
              :showItemNavigatorsOnHover="true"
              :showIndicators="true"
            >
              <template #item="slotProps">
                <img
                  :src="slotProps.item.itemImageSrc"
                  :alt="slotProps.item.alt"
                  style="height: 400px; width: 100%; display: block"
                />
              </template>
            </Galleria>
          </div>
        </template>
      </Card>

      <Card class="mt-3">
        <template #content>
          <div>
            <Galleria
              :value="products"
              :responsiveOptions="responsiveOptions"
              :numVisible="3"
              :circular="true"
              :showItemNavigators="true"
              :showThumbnails="false"
              :showItemNavigatorsOnHover="true"
              :showIndicators="true"
            >
              <template #item="slotProps">
                <div class="product-item">
                  <div class="product-item-content">
                    <div class="p-mb-3">
                      <img
                        :src="slotProps.item.image"
                        :alt="slotProps.item.name"
                        class="product-image"
                      />
                    </div>
                    <div>
                      <h4 class="p-mb-1">{{ slotProps.item.name }}</h4>
                      <h6 class="p-mt-0 p-mb-3">${{ slotProps.item.price }}</h6>
                      <span
                        :class="
                          'product-badge status-' +
                            slotProps.item.inventoryStatus.toLowerCase()
                        "
                        >{{ slotProps.item.inventoryStatus }}</span
                      >
                      <div class="car-buttons p-mt-5">
                        <Button
                          icon="pi pi-search"
                          class="p-button p-button-rounded p-mr-2"
                        />
                        <Button
                          icon="pi pi-star"
                          class="p-button-success p-button-rounded p-mr-2"
                        />
                        <Button
                          icon="pi pi-cog"
                          class="p-button-help p-button-rounded"
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </template>
            </Galleria>
          </div>
        </template>
      </Card>
    </div>
    <div class="col-md-3">
      <Card>
        <template #title>
          <h5>Yazarlar</h5>
        </template>
        <template #content>
          <Carousel
            :value="articles"
            :numVisible="1"
            :numScroll="1"
            :responsiveOptions="responsiveOptions"
            :circular="true"
            :autoplayInterval="3000"
          >
            <template #item="slotProps">
              <div class="product-item">
                <div class="product-item-content">
                  <div class="p-mb-3">
                    <img
                      src="https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png"
                      :alt="slotProps.data.name"
                    />
                  </div>
                  <div>
                    <h4 class="p-mb-1">{{ slotProps.data.name }}</h4>
                  </div>
                </div>
              </div>
            </template>
          </Carousel>
        </template>
      </Card>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      carouselList: [
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
      ],
      products: [
        {
          id: "1000",
          code: "f230fh0g3",
          name: "Bamboo Watch",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/bamboo-watch.jpg",
          price: 65,
          category: "Accessories",
          quantity: 24,
          inventoryStatus: "INSTOCK",
          rating: 5,
        },
        {
          id: "1001",
          code: "nvklal433",
          name: "Black Watch",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/black-watch.jpg",
          price: 72,
          category: "Accessories",
          quantity: 61,
          inventoryStatus: "INSTOCK",
          rating: 4,
        },
        {
          id: "1002",
          code: "zz21cz3c1",
          name: "Blue Band",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/blue-band.jpg",
          price: 79,
          category: "Fitness",
          quantity: 2,
          inventoryStatus: "LOWSTOCK",
          rating: 3,
        },
        {
          id: "1003",
          code: "244wgerg2",
          name: "Blue T-Shirt",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/blue-t-shirt.jpg",
          price: 29,
          category: "Clothing",
          quantity: 25,
          inventoryStatus: "INSTOCK",
          rating: 5,
        },
        {
          id: "1004",
          code: "h456wer53",
          name: "Bracelet",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/bracelet.jpg",
          price: 15,
          category: "Accessories",
          quantity: 73,
          inventoryStatus: "INSTOCK",
          rating: 4,
        },
        {
          id: "1005",
          code: "av2231fwg",
          name: "Brown Purse",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/brown-purse.jpg",
          price: 120,
          category: "Accessories",
          quantity: 0,
          inventoryStatus: "OUTOFSTOCK",
          rating: 4,
        },
        {
          id: "1006",
          code: "bib36pfvm",
          name: "Chakra Bracelet",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/chakra-bracelet.jpg",
          price: 32,
          category: "Accessories",
          quantity: 5,
          inventoryStatus: "LOWSTOCK",
          rating: 3,
        },
        {
          id: "1007",
          code: "mbvjkgip5",
          name: "Galaxy Earrings",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/galaxy-earrings.jpg",
          price: 34,
          category: "Accessories",
          quantity: 23,
          inventoryStatus: "INSTOCK",
          rating: 5,
        },
        {
          id: "1008",
          code: "vbb124btr",
          name: "Game Controller",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/game-controller.jpg",
          price: 99,
          category: "Electronics",
          quantity: 2,
          inventoryStatus: "LOWSTOCK",
          rating: 4,
        },
        {
          id: "1009",
          code: "cm230f032",
          name: "Gaming Set",
          description: "Product Description",
          image:
            "https://primefaces.org/primevue/showcase/demo/images/product/gaming-set.jpg",
          price: 299,
          category: "Electronics",
          quantity: 63,
          inventoryStatus: "INSTOCK",
          rating: 3,
        },
      ],
      articles: [
        { name: "Sait Faik Abasıyanık" },
        { name: "Ziya Gökalp" },
        { name: "Cahit Sıtkı Tarancı" },
        { name: "Ömer Seyfettin" },
      ],
      responsiveOptions: [
        {
          breakpoint: "1024px",
          numVisible: 5,
        },
        {
          breakpoint: "768px",
          numVisible: 3,
        },
        {
          breakpoint: "560px",
          numVisible: 1,
        },
      ],
    };
  },
};
</script>
<style>
.product-item-content {
  border: 1px solid var(--surface-d);
  border-radius: 3px;
  margin: 0.3rem;
  text-align: center;
  padding: 2rem 0;
}

.product-image {
  width: 50%;
  box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
}
</style>
