import {
  createWebHistory,
  createRouter,
  RouterView
} from "vue-router";
import store from "@/store";

// frontend routes
import Layout from "./views/frontend/Layout.vue";
import Home from "./views/frontend/Home.vue";
import Page from "./views/frontend/Page.vue";
import Blog from "./views/frontend/Blog.vue";
import BlogDetail from "./views/frontend/BlogDetail.vue";
import Contact from "./views/frontend/Contact.vue";
import Login from "./views/frontend/Login.vue";
import Register from "./views/frontend/Register.vue";
import EmailVerify from './views/frontend/EmailVerify.vue';
import ResetPassword from './views/frontend/ResetPassword.vue';

// admin routes
import AdminLayout from "./views/admin/AdminLayout.vue";
import Dashboard from "./views/admin/Dashboard.vue";
import Users from "./views/admin/definitions/Users.vue";
import UserAuthorization from "./views/admin/definitions/UserAuthorization.vue";
import AccessRight from "./views/admin/definitions/AccessRight.vue";
import FrontendMenu from "./views/admin/definitions/FrontendMenu.vue";
import BlogCategory from "./views/admin/definitions/BlogCategory.vue";
import TodoCategory from "./views/admin/definitions/TodoCategory.vue";
import TodoStatus from "./views/admin/definitions/TodoStatus.vue";
import Author from "./views/admin/definitions/Author.vue";
import Comment from "./views/admin/contents/comment/Comment.vue";
import CommentDetail from "./views/admin/contents/comment/CommentDetail.vue";
import Announcement from "./views/admin/contents/Announcement.vue";
import Article from "./views/admin/contents/Article.vue";
import AdminBlog from "./views/admin/contents/Blog.vue";
import ContactMessages from "./views/admin/contents/ContactMessages.vue";
import Pages from "./views/admin/contents/Pages.vue";
import PhotoGallery from "./views/admin/contents/PhotoGallery.vue";
import VideoGallery from "./views/admin/contents/VideoGallery.vue";
import MyTodos from "./views/admin/tools/MyTodos.vue";
import Todos from "./views/admin/tools/Todos.vue";

// user routes
import UserLayout from "./views/user/UserLayout.vue";
import Profile from "./views/user/Profile.vue";
import ChangePassword from "./views/user/ChangePassword.vue";
import UserComments from "./views/user/UserComments.vue";

const routes = [{
    path: "/",
    component: Layout,
    meta: {
      requireAuth: false,
      isAccessAdminPanel: false,
    },
    children: [{
        path: "",
        component: Home,
      },
      {
        path: "/sayfalar/:url",
        name: "Page",
        component: Page,
      },
      {
        path: "/blog/:categoryUrl/:id",
        name: "BlogDetail",
        component: BlogDetail,
      },
      {
        path: "/blog/:categoryUrl",
        name: "Blog",
        component: Blog,
      },
      {
        path: "/iletisim",
        name: "Contact",
        component: Contact,
      },
      {
        path: "/giris",
        name: "Login",
        component: Login,
      },
      {
        path: "/uye-ol",
        name: "Register",
        component: Register
      },    
      {
        path: "/email-dogrulama/:code",
        name: "EmailVerify",
        component: EmailVerify
      },
      {
        path: "/sifre-belirle/:code",
        name: "ResetPassword",
        component: ResetPassword,
      },
      {
        path: "/kullanici",
        component: UserLayout,
        meta: {
          requireAuth: true,
          isAccessAdminPanel: false,
        },
        children: [{
            path: "hesabim",
            component: Profile,
          },
          {
            path: "sifre-degistir",
            component: ChangePassword
          },
          {
            path: "yorumlarim",
            component: UserComments
          }
        ]
      },
    ],
  },
  {
    path: "/admin",
    component: AdminLayout,
    meta: {
      requireAuth: true,
      isAccessAdminPanel: true,
    },
    children: [{
        path: "",
        component: Dashboard,
      },
      {
        path: "icerikler",
        component: RouterView,
        children: [{
            path: "blog",
            component: AdminBlog,
          },
          {
            path: "sayfalar",
            component: Pages,
          },
          {
            path: "duyurular",
            component: Announcement,
          },
          {
            path: "iletisim-mesajlari",
            component: ContactMessages,
          },
          {
            path: "blog",
            component: Blog,
          },
          {
            path: "yazar-yazilari",
            component: Article,
          },
          {
            path: "sayfalar",
            component: Pages,
          },
          {
            path: "foto-galeri",
            component: PhotoGallery,
          },
          {
            path: "video-galeri",
            component: VideoGallery,
          },
          {
            path: "yorumlar",
            name: "Comment",
            component: Comment
          },
          {
            path: "yorumlar/:id",
            name: "CommentDetail",
            component: CommentDetail
          },
        ],
      },
      {
        path: "tanimlar",
        component: RouterView,
        children: [{
            path: "kullanicilar",
            component: Users,
          },
          {
            path: "kullanici-yetkilendirme",
            component: UserAuthorization,
          },
          {
            path: "blog-kategorileri",
            component: BlogCategory,
          },
          {
            path: "yapilacak-kategorileri",
            component: TodoCategory,
          },
          {
            path: "yapilacak-durumlari",
            component: TodoStatus,
          },
          {
            path: "yazarlar",
            component: Author,
          },
          {
            path: "erisim-haklari",
            component: AccessRight
          },
          {
            path: "on-arayuz-menu",
            component: FrontendMenu
          }
        ],
      },
      {
        path: "araclar",
        component: RouterView,
        children: [{
            path: "yapilacaklar",
            component: Todos,
          },
          {
            path: "bana-atananlar",
            component: MyTodos,
          },
        ],
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  if (!to.meta.requireAuth && !to.meta.isAccessAdminPanel) {
    next();
  }
  if (to.meta.requireAuth) {
    const currentUser = JSON.parse(localStorage.getItem("user"));
    if (!currentUser) {
      store.dispatch("auth/logout");
      next("/giris");
    } else {
      if (to.meta.isAccessAdminPanel) {
        // const result = currentUser.menuAccessRights.filter((x) => x == to.fullPath);
        // if (result.length > 0) {
        next();
        // } else {
        //   store.dispatch("auth/logout");
        //   next("/giris");
        // }
      } else {
        next();
      }
    }
  }
});

export default router;