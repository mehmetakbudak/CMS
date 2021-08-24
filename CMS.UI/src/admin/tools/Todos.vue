<template>
  <div id="name">
    <div class="row mb-2">
      <div class="col-6">
        <h5>Yapılacaklar</h5>
      </div>
      <div class="col-6">
        <Button
          label="Ekle"
          icon="pi pi-plus"
          class="p-button-primary p-button-sm float-end"
          @click="add()"
        />
      </div>
    </div>
    <div class="mt-3 mb-3">
      <Accordion>
        <AccordionTab>
          <template #header>
            <div class="row w-100">
              <div class="col-md-6">
                <div class="mt-5px">Filtrele</div>
              </div>
              <div class="col-md-6">
                <Button
                  label="Filtrele"
                  icon="pi pi-search"
                  class="p-button-info p-button-sm float-end"
                  @click="filterTodos()"
                />
              </div>
            </div>
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
                  @change="getTodoStatuses(filter.todoCategoryId)"
                />
              </div>
            </div>
            <div class="col-md-4">
              <div class="pt-3">
                <div class="p-float-label">
                  <InputText type="text" v-model="filter.title" class="w-100" />
                  <label>Başlık</label>
                </div>
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
        </AccordionTab>
      </Accordion>
    </div>
    <div class="border border-top-0">
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
              class="p-button-rounded p-button-primary p-button-sm"
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
      </DataTable>
    </div>

    <Dialog
      :header="modalTitle"
      v-model:visible="displayModal"
      :modal="true"
      :breakpoints="{ '960px': '95vw' }"
      :style="{ width: '50vw' }"
      :autoZIndex="false"
    >
      <div class="mb-3">
        <label>Kategori Adı</label>
        <Dropdown
          class="w-100"
          v-model="todo.todoCategoryId"
          :options="todoCategories"
          optionLabel="name"
          optionValue="id"
          placeholder="Kategori seçiniz."
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
        <label>Atanan Kullanıcı Yorumu</label>
        <Textarea
          v-model="todo.userComment"
          :disabled="true"
          placeholder="Atanan Kullanıcı Yorumu"
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
      <template #footer>
        <Button
          label="Kapat"
          @click="displayModal = false"
          class="p-button-outlined p-button-secondary"
        />
        <Button label="Kaydet" @click="save()" autofocus />
      </template>
    </Dialog>
  </div>
</template>

<script>
import todoService from "../../services/TodoService";
import userService from "../../services/UserService";
import todoCategoryService from "../../services/TodoCategoryService";
import todoStatusService from "../../services/TodoStatusService";
import dateFormat from "../../infrastructure/DateFormat";

export default {
  name: "name",
  data() {
    return {
      todos: [],
      todoCategories: [],
      todoStatuses: [],
      users: [],
      selectedTodo: {},
      displayModal: false,
      modalTitle: "",
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
        todoStatusId: 0,
        startDate: null,
        endDate: null,
        isActive: true,
      },
      gridMenuItems: [
        {
          label: "Düzenle",
          command: () => {
            this.modalTitle = "Yapılacak Düzenle";
            this.displayModal = true;
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
                    this.$toast.add({
                      severity: "success",
                      summary: "İşlem Başarılı",
                      detail: res.data.message,
                    });
                  })
                  .catch((e) => {
                    this.$toast.add({
                      severity: "error",
                      summary: "İşlem Başarısız",
                      detail: e.response.data.message,
                    });
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
      todoService.getAll().then((res) => {
        this.todos = res.data;
      });
    },
    filterTodos() {
      console.log(this.filter);
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
      this.displayModal = true;
      this.modalTitle = "Yeni Yapılacak Ekle";
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
    },
    save() {
      if (this.todo.id == 0) {
        todoService
          .post(this.todo)
          .then((res) => {
            this.getAll();
            this.displayModal = false;
            this.$toast.add({
              severity: "success",
              summary: "İşlem Başarılı",
              detail: res.data.message,
            });
          })
          .catch((e) => {
            console.log(e);
            this.$toast.add({
              severity: "error",
              summary: "İşlem Başarısız",
              detail: e.response.data.message,
            });
          });
      } else {
        todoService
          .put(this.todo)
          .then((res) => {
            this.getAll();
            this.displayModal = false;
            this.$toast.add({
              severity: "success",
              summary: "İşlem Başarılı",
              detail: res.data.message,
            });
          })
          .catch((e) => {
            this.$toast.add({
              severity: "error",
              summary: "İşlem Başarısız",
              detail: e.response.data.message,
            });
          });
      }
    },
    dateFormatValue(value) {
      return dateFormat.convert(value);
    },
  },
};
</script>
