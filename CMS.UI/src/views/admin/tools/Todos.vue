<template>
  <div class="card">
    <div class="card-header bg-white py-3">
      <div class="row">
        <div class="col-6">
          <h5>{{ title }}</h5>
        </div>
        <div class="col-6">
          <Button
            v-if="showGrid"
            icon="pi pi-plus"
            class="p-button-primary p-button-sm float-end"
            @click="add()"
          />
          <Button
            v-if="showForm"
            icon="pi pi-arrow-left"
            class="p-button-primary p-button-sm float-end"
            @click="reset()"
          />
        </div>
      </div>
    </div>
    <div class="card-body">
      <div v-if="showGrid">
        <div class="mb-3">
          <Panel
            header="Filtrele"
            :toggleable="true"
            :collapsed="false"
            class="bg-white"
          >
            <template #icons>
              <Button
                icon="pi pi-times"
                class="p-button-raised p-button-rounded p-button-help ms-5"
                @click="filterReset()"
              />

              <Button
                icon="pi pi-search"
                class="p-button-raised p-button-rounded ms-3 mr-20px"
                @click="filterTodos()"
              />
            </template>
            <div class="row pb-0">
              <div class="col-md-4">
                <div class="pt-3">
                  <Dropdown
                    class="w-100"
                    v-model="filter.todoCategoryId"
                    :options="todoCategories"
                    optionLabel="name"
                    optionValue="id"
                    placeholder="Kategori seçiniz."
                    :showClear="true"
                    @change="getTodoStatuses(filter.todoCategoryId)"
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="pt-3">
                  <InputText
                    type="text"
                    v-model="filter.title"
                    placeholder="Başlık"
                    class="w-100"
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="pt-3">
                  <Dropdown
                    class="w-100"
                    v-model="filter.userId"
                    :options="users"
                    optionLabel="name"
                    optionValue="id"
                    :showClear="true"
                    placeholder="Kullanıcı seçiniz."
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="pt-3">
                  <Dropdown
                    class="w-100"
                    v-model="filter.todoStatusId"
                    :options="todoStatuses"
                    optionLabel="name"
                    optionValue="id"
                    :showClear="true"
                    placeholder="Durum seçiniz."
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="pt-3">
                  <Calendar
                    class="w-100"
                    placeholder="Başlangıç Tarihi"
                    v-model="filter.startDate"
                    :showIcon="true"
                    dateFormat="dd.mm.yy"
                    :showButtonBar="true"
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="pt-3">
                  <Calendar
                    class="w-100"
                    placeholder="Bitiş Tarihi"
                    v-model="filter.endDate"
                    :showIcon="true"
                    dateFormat="dd.mm.yy"
                    :showButtonBar="true"
                  />
                </div>
              </div>
              <div class="col-md-4">
                <div class="pt-3">
                  <div class="row row-cols-auto">
                    <div class="col">
                      <InputSwitch v-model="filter.isActive" />
                    </div>
                    <div class="col">Aktif</div>
                  </div>
                </div>
              </div>
            </div>
          </Panel>
        </div>

        <DataTable
          :loading="loading"
          showGridlines
          :value="todos"
          :paginator="true"
          :rows="5"
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
          :rowsPerPageOptions="[5, 10, 20, 50]"
          responsiveLayout="scroll"
          currentPageReportTemplate="Showing {first} to {last} of {totalRecords}"
        >
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
          <Column field="todoCategoryName" header="Kategori Adı"></Column>
          <Column field="title" header="Başlık"></Column>
          <Column field="userNameSurname" header="Atanan Kullanıcı"></Column>
          <Column field="todoStatusName" header="Durumu"></Column>
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
              {{
                data.updatedDate != null
                  ? dateFormatValue(data.updatedDate)
                  : ""
              }}
            </template>
          </Column>
          <Column field="isActive" header="Aktif">
            <template #body="slotProps">
              <div>
                {{ slotProps.data.isActive ? "Aktif" : "Pasif" }}
              </div>
            </template>
          </Column>
          <template #empty> Kayıt yok. </template>
        </DataTable>
      </div>

      <div v-if="showForm">
        <TabView>
          <TabPanel header="Bilgiler" key="1">
            <div class="row pt-3">
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">Kategori Adı</label>
                  <Dropdown
                    autofocus
                    class="w-100"
                    v-model="todo.todoCategoryId"
                    :options="todoCategories"
                    optionLabel="name"
                    optionValue="id"
                    placeholder="Kategori seçiniz."
                    :showClear="true"
                    @change="getTodoStatuses(todo.todoCategoryId)"
                    :loading="loading"
                  />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">Başlık</label>
                  <InputText
                    type="text"
                    v-model="todo.title"
                    placeholder="Başlık"
                    class="w-100"
                  />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">Atanan Kullanıcı</label>
                  <Dropdown
                    class="w-100"
                    v-model="todo.userId"
                    :options="users"
                    optionLabel="name"
                    optionValue="id"
                    :showClear="true"
                    placeholder="Kullanıcı seçiniz."
                  />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">Durumu</label>
                  <Dropdown
                    class="w-100"
                    v-model="todo.todoStatusId"
                    :options="todoStatuses"
                    optionLabel="name"
                    optionValue="id"
                    :showClear="true"
                    placeholder="Durum seçiniz."
                    :loading="loading"
                  />
                </div>
              </div>
              <div class="col-md-12">
                <div class="mb-3">
                  <label class="form-label">Açıklama</label>
                  <Editor
                    v-model="todo.description"
                    editorStyle="height: 200px"
                    class="w-100"
                  />
                </div>
              </div>
              <div class="col-md-12">
                <div class="mb-3">
                  <label class="form-label">Aktif</label>
                  <div>
                    <InputSwitch v-model="todo.isActive" />
                  </div>
                </div>
              </div>
            </div>
            <div class="footer-button">
              <Button label="Kaydet" @click="save()" />
              <Button
                label="Vazgeç"
                @click="reset()"
                class="ms-2 p-button-outlined p-button-secondary"
              />
            </div>
          </TabPanel>
          <TabPanel key="2" header="Yorumlar" v-if="todo?.id != 0">
            <div class="mb-3">
              <label class="form-label">Yorumunuz</label>
              <Textarea
                v-model="comment"
                placeholder="Yorumunuz"
                rows="3"
                class="w-100"
              />
              <Button
                label="Kaydet"
                class="p-button-primary p-button-sm float-end mt-2"
                @click="addComment()"
              />
            </div>
          </TabPanel>
        </TabView>
      </div>
    </div>
  </div>
</template>

<script>
import GlobalService from "../../../services/GlobalService";
import dateFormat from "../../../infrastructure/DateFormat";
import alertService from "../../../services/AlertService";
import { Endpoints } from "../../../services/Endpoints";

export default {
  name: "name",
  mixins: [alertService],
  data() {
    return {
      loading: false,
      todos: [],
      todoCategories: [],
      todoStatuses: [],
      users: [],
      selectedTodo: {},
      showForm: false,
      showGrid: true,
      title: "",
      comment: "",
      showNewComment: false,
      todo: {
        id: 0,
        todoCategoryId: 0,
        todoStatusId: 0,
        userId: null,
        title: "",
        description: "",
        userComment: "",
        isActive: true,
      },
      filter: {
        todoCategoryId: null,
        title: null,
        userId: null,
        todoStatusId: null,
        startDate: null,
        endDate: null,
        isActive: null,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.showGrid = false;
            this.title = "Yapılacak Düzenle";
            this.showForm = true;
            var e = this.selectedTodo;
            this.todo = {
              id: e.id,
              todoCategoryId: e.todoCategoryId,
              todoStatusId: e.todoStatusId,
              userId: e.userId,
              title: e.title,
              description: e.description,
              userComment: e.userComment,
              isActive: e.isActive,
            };
            this.getUsers();
            this.getTodoCategories();
            this.getTodoStatuses(e.todoCategoryId);
          },
        },
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
                  Endpoints.Admin.Todo,
                  this.selectedTodo.id
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
  created() {
    this.getAll();
    this.getUsers();
    this.getTodoCategories();
    this.reset();
  },
  methods: {
    getAll() {
      this.loading = true;
      GlobalService.PostByAuth(
        `${Endpoints.Admin.Todo}/GetWithFilter`,
        this.filter
      ).then((res) => {
        this.todos = res.data;
        this.loading = false;
      });
    },
    filterTodos() {
      this.getAll();
    },
    toggleGridMenu(event, data) {
      this.selectedTodo = data;
      this.$refs.menu.toggle(event);
    },
    getUsers() {
      GlobalService.GetByAuth(Endpoints.Admin.Lookup.Users).then((res) => {
        this.users = res.data;
      });
    },
    getTodoCategories() {
      this.loading = true;
      GlobalService.GetByAuth(Endpoints.Admin.TodoCategory).then((res) => {
        this.todoCategories = res.data;
        this.loading = false;
      });
    },
    getTodoStatuses(categoryId) {
      if (categoryId != 0) {
        this.loading = true;
        GlobalService.GetByAuth(
          `${Endpoints.Admin.TodoStatus}/GetByTodoCategoryId/${categoryId}`
        ).then((res) => {
          this.todoStatuses = res.data;
          this.loading = false;
        });
      }
    },
    add() {
      this.showGrid = false;
      this.showForm = true;
      this.title = "Yeni Yapılacak Ekle";
      this.todo = {
        id: 0,
        todoCategoryId: 0,
        todoStatusId: 0,
        userId: null,
        title: "",
        description: "",
        userComment: "",
        isActive: true,
      };
      this.getUsers();
      this.getTodoCategories();
      this.todoStatuses = [];
    },
    save() {
      if (this.todo.id == 0) {
        GlobalService.PostByAuth(Endpoints.Admin.Todo, this.todo)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        GlobalService.PutByAuth(Endpoints.Admin.Todo, this.todo)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      }
    },
    dateFormatValue(value) {
      return dateFormat.convert(value);
    },
    reset() {
      this.showForm = false;
      this.showGrid = true;
      this.title = "Yapılacaklar";
    },
    filterReset() {
      this.filter = {
        todoCategoryId: null,
        title: null,
        userId: null,
        todoStatusId: null,
        startDate: null,
        endDate: null,
        isActive: null,
      };
      this.getAll();
    },
    addComment() {
      this.showNewComment = true;
    },
  },
};
</script>