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
            BlogCategories: "AdminLookup/BlogCategories"
        }
    },
    Blog: "Blog",
    BlogCategory: "BlogCategory",
    Author: "Author",
    Menu: "Menu",
    Account: {
        Login: "Account/Login",
        ForgotPassword: "Account/ForgotPassword",
        Profile: "Account/Profile",
        AddMember: "Account/AddMember",
        ChangePassword: "Account/ChangePassword",
        MemberComments: "Account/MemberComments",
        EmailVerified: "Account/EmailVerified",
        ResetPassword: "Account/ResetPassword"
    },
    Lookup: {
        Users: "Lookup/Users",
        TodoCategories: "Lookup/TodoCategories",
    }
}