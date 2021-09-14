<template>
  <div id="name">
    <div class="row mb-2">
      <div class="col-6">
        <h4>{{ title }}</h4>
      </div>
      <div class="col-6">
        <Button
          v-if="showGrid"
          label="Ekle"
          icon="pi pi-plus"
          class="p-button-primary p-button-sm float-end"
          @click="add()"
        />
        <Button
          v-if="showForm"
          label="Geri"
          icon="pi pi-arrow-left"
          class="p-button-primary p-button-sm float-end"
          @click="reset()"
        />
      </div>
    </div>

    <div class="shadow p-2 border" v-if="showGrid">
      <div class="mt-3 mb-3">
        <Panel header="Filtrele" :toggleable="true" :collapsed="true">
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
                  optionLabel="fullName"
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
        <Column field="insertDate" header="Kayıt Tarihi" dataType="date">
          <template #body="{ data }">
            {{ dateFormatValue(data.insertDate) }}
          </template>
        </Column>
        <Column field="updateDate" header="Güncelleme Tarihi" dataType="date">
          <template #body="{ data }">
            {{
              data.updateDate != null ? dateFormatValue(data.updateDate) : ""
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

    <div class="card shadow" v-if="showForm">
      <div class="card-body">
        <div class="row">
          <div class="col-md-6">
            <h5>Bilgiler</h5>
            <div class="border-bottom mb-3"></div>
            <div class="mb-3">
              <label>Kategori Adı</label>
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
              />
            </div>
            <div class="mb-3">
              <label>Başlık</label>
              <InputText
                type="text"
                v-model="todo.title"
                placeholder="Başlık"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label>Atanan Kullanıcı</label>
              <Dropdown
                class="w-100"
                v-model="todo.userId"
                :options="users"
                optionLabel="fullName"
                optionValue="id"
                :showClear="true"
                placeholder="Kullanıcı seçiniz."
              />
            </div>
            <div class="mb-3">
              <label>Durumu</label>
              <Dropdown
                class="w-100"
                v-model="todo.todoStatusId"
                :options="todoStatuses"
                optionLabel="name"
                optionValue="id"
                :showClear="true"
                placeholder="Durum seçiniz."
              />
            </div>
            <div class="mb-3">
              <label>Açıklama</label>
              <Textarea
                v-model="todo.description"
                placeholder="Açıklama"
                rows="5"
                class="w-100"
              />
            </div>
            <div class="mb-3">
              <label>Aktif</label>
              <div>
                <InputSwitch v-model="todo.isActive" />
              </div>
            </div>

            <div class="float-end">
              <Button
                label="Vazgeç"
                @click="reset()"
                class="p-button-outlined p-button-secondary"
              />
              <Button class="ms-2" label="Kaydet" @click="save()" />
            </div>
          </div>
          <div class="col-md-6">
            <div class="row">
              <div class="col-md-6">
                <h5>Yorumlar</h5>
              </div>
              <div class="col-md-6">
                <div class="">
                  <Button
                    label="Ekle"
                    icon="pi pi-plus"
                    class="p-button-primary p-button-sm float-end"
                    @click="addComment()"
                  />
                </div>
              </div>
            </div>
            <div class="border-bottom mb-3"></div>
            <div class="mb-3" v-if="showNewComment">
              <label>Yorumunuz</label>
              <Textarea
                v-model="comment"
                placeholder="Yorumunuz"
                rows="3"
                class="w-100"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import todoService from "../../services/TodoService";
import userService from "../../services/UserService";
import todoCategoryService from "../../services/TodoCategoryService";
import todoStatusService from "../../services/TodoStatusService";
import dateFormat from "../../infrastructure/DateFormat";
import alertService from "../../services/AlertService";

export default {
  name: "name",
  mixins: [alertService],
  data() {
    return {
      todos: [],
      todoCategories: [],
      todoStatuses: [],
      users: [],
      selectedTodo: {},
      showForm: false,
      showGrid: true,
      title: "Yapılacaklar",
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
            this.title = "Yapılacak Düzenle";
            this.showForm = true;
            this.showGrid = false;
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
                todoService
                  .delete(this.selectedTodo.id)
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
  },
  methods: {
    getAll() {
      todoService.getAll(this.filter).then((res) => {
        this.todos = res.data;
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
      userService.getAll().then((res) => {
        this.users = res.data;
      });
    },
    getTodoCategories() {
      todoCategoryService.getAll().then((res) => {
        this.todoCategories = res.data;
      });
    },
    getTodoStatuses(categoryId) {
      if (categoryId != 0) {
        todoStatusService.getByTodoCategoryId(categoryId).then((res) => {
          this.todoStatuses = res.data;
        });
      }
    },
    add() {
      this.showForm = true;
      this.showGrid = false;
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
        todoService
          .post(this.todo)
          .then((res) => {
            this.getAll();
            this.reset();
            this.successMessage(this, res.data.message);
          })
          .catch((e) => {
            this.errorMessage(this, e.response.data.message);
          });
      } else {
        todoService
          .put(this.todo)
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
