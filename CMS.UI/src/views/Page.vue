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
          <router-link
            class="text-secondary text-decoration-none"
            to="/sayfalar/hakkinda"
            >Hakkında</router-link
          >
        </li>
      </ol>
    </nav>
  </div>
  <div class="row mb-3 mt-3">
    <div class="col-md-3">
      <div class="card">
        <h5 class="card-header">Kurumsal</h5>
        <div class="card-body">
          <Tree :value="menuDatasource">
            <template #default="menu">
              <router-link
                class="text-dark text-decoration-none"
                :to="menu.node.url"
              >
                {{ menu.node.title }}
              </router-link>
            </template>
          </Tree>
        </div>
      </div>
    </div>
    <div class="col-md-9">
      <h5>Hakkında</h5>
      <div class="mb-3 mt-3">
        Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quas vitae
        earum et excepturi error quibusdam, nam, unde, quisquam assumenda ab
        sapiente? Numquam consequuntur excepturi quam vel sequi ipsam
        dignissimos odio. Lorem, ipsum dolor sit amet consectetur adipisicing
        elit. Quas vitae earum et excepturi error quibusdam, nam, unde, quisquam
        assumenda ab sapiente? Numquam consequuntur excepturi quam vel sequi
        ipsam dignissimos odio. Lorem, ipsum dolor sit amet consectetur
        adipisicing elit. Quas vitae earum et excepturi error quibusdam, nam,
        unde, quisquam assumenda ab sapiente? Numquam consequuntur excepturi
        quam vel sequi ipsam dignissimos odio.
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      menuDatasource: [
        {
          title: "Hakkımızda",
          url: "/sayfalar/hakkimizda",
        },
        {
          title: "Haberler",
          url: "/haberler",
        },
        {
          title: "Şirket Profili",
          url: "/sayfalar/sirket-profili",
          children: [
            {
              title: "Mehmet Akbudak",
              url: "safsdf fsdfds fsdf fdsfdsf",
            },
            {
              title: "Güler Akbudak",
              url: "fsdf dsfdsf sdfds",
            },
          ],
        },
      ],
    };
  },
};
</script>

<style scoped>
::v-deep(.p-tree .p-tree-container .p-treenode .p-treenode-content) {
  padding: unset;
}
::v-deep(.p-tree) {
  padding: unset;
  border: unset;
}
</style>