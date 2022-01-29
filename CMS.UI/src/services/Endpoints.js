export const API_URL = process.env.VUE_APP_BASEURL;

export const Endpoints = {
    Admin: {
        Blog: "AdminBlog",
        BlogCategory: "AdminBlogCategory",
        User: "AdminUser",
        Todo: "AdminTodo",
        TodoStatus: "AdminTodoStatus",
        AccessRight: "AdminAccessRight",
        UserAccessRight: "AdminUserAccessRight",
        TodoCategory: "AdminTodoCategory",
        Author: "AdminAuthor",
        Menu: "AdminMenu",
        Lookup: {
            Users: "AdminLookup/Users",
            TodoCategories: "AdminLookup/TodoCategories",
        }
    },
    Blog: "Blog",
    BlogCategory: "BlogCategory",
    Author: "Author",
    Menu: "Menu",
    Lookup: {
        Users: "Lookup/Users",
        TodoCategories: "Lookup/TodoCategories",
    }
}