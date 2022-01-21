<template>
  <Card>
    <template #title>
      <div class="row mb-2">
        <div class="col-md-12">
          <h5>Kullanıcı Yetkilendirme</h5>
        </div>
      </div>
    </template>
    <template #content>
      <div class="row">
        <div class="col-md-6">
          <div class="mb-3">
            <label>Kullanıcı</label>
            <Dropdown
              class="w-100"
              v-model="userId"
              :options="users"
              optionLabel="name"
              optionValue="id"
              placeholder="Kullanıcı seçiniz."
              @change="changeUser($event)"
            />
          </div>
        </div>
      </div>
      <div>
        <TabView>
          <TabPanel header="Sayfa Yetkilendirme">
            <div>
              <Tree
                ref="treeMenu"
                :value="menus"
                selectionMode="checkbox"
                v-model:selectionKeys="selectedMenus"
              ></Tree>
            </div>
          </TabPanel>
          <TabPanel header="İşlem Yetkilendirme">
            <Tree
              :value="operations"
              selectionMode="checkbox"
              v-model:selectionKeys="selectedOperations"
              :metaKeySelection="false"
            ></Tree>
          </TabPanel>
        </TabView>
      </div>
      <div class="row mt-3">
        <div class="col-md-12">
          <Button class="float-end" label="Kaydet" @click="save()" />
        </div>
      </div>
    </template>
  </Card>
</template>

<script>
import lookupService from "../../services/LookupService";
import accessRightService from "../../services/AccessRightService";
import userAccessRightService from "../../services/UserAccessRightService";

export default {
  data() {
    return {
      userId: 0,
      users: [],
      menus: [],
      selectedMenus: [],
      selectMenuList: [],
      operations: [],
      selectedOperations: [],
      selectOperationList: [],
    };
  },
  created() {
    this.getUsers();
    this.getAccessRights();
  },
  methods: {
    getUsers() {
      lookupService.getUsers().then((res) => {
        this.users = res.data;
      });
    },
    getAccessRights() {
      accessRightService.get().then((res) => {
        this.menus = res.data.menuAccessRights;
        this.operations = res.data.operationAccessRights;
      });
    },
    changeUser(e) {
      this.selectedMenus = [];
      this.selectedOperations = [];
      userAccessRightService.get(e.value).then((res) => {
        if (res.data.menuUserAccessRights) {
          res.data.menuUserAccessRights.forEach((e) => {
            this.selectedMenus[e] = {
              checked: true,
            };
          });
        }
        if (res.data.operationUserAccessRights) {
          res.data.operationUserAccessRights.forEach((e) => {
            this.selectedOperations[e] = {
              checked: true,
            };
          });
        }
      });
    },
    selectMenuNode(node) {
      if (this.selectedMenus[node.key]) {
        this.selectMenuList.push(node.key);
      }
      if (node.children && node.children.length) {
        for (let child of node.children) {
          if (this.selectedMenus[child.key]) {
            this.selectMenuList.push(child.key);
          }
          this.selectMenuNode(child);
        }
      }
    },
    selectOperationNode(node) {
      if (this.selectedOperations[node.key]) {
        this.selectOperationList.push(node.key);
      }

      if (node.children && node.children.length) {
        for (let child of node.children) {
          if (this.selectedOperations[child.key]) {
            this.selectOperationList.push(child.key);
          }
          this.selectOperationNode(child);
        }
      }
    },
    save() {
      this.selectMenuList = [];
      this.selectOperationList = [];
      this.menus.forEach((node) => {
        this.selectMenuNode(node);
      });
      this.operations.forEach((node) => {
        this.selectOperationNode(node);
      });

      var data = {
        userId: this.userId,
        menuUserAccessRights: this.selectMenuList,
        operationUserAccessRights: this.selectOperationList,
      };
      userAccessRightService.createOrUpdate(data).then((res) => {
        console.log(res);
      });
    },
  },
};
</script>

<style></style>
