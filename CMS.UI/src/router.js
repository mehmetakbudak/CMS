import {
  createWebHistory,
  createRouter,
  RouterView
} from "vue-router";

// frontend routes
import Layout from "./views/frontend/Layout.vue";
import Home from "./views/frontend/Home.vue";
import Page from "./views/frontend/Page.vue";
import Blog from "./views/frontend/Blog.vue";
import BlogCategory from "./views/frontend/BlogCategory.vue";
import BlogDetail from "./views/frontend/BlogDetail.vue";
import Contact from "./views/frontend/Contact.vue";
import Login from "./views/frontend/Login.vue";
import Register from "./views/frontend/Register.vue";
import EmailVerify from './views/frontend/EmailVerify.vue';
import ResetPassword from './views/frontend/ResetPassword.vue';
import Service from "./views/frontend/Service.vue";
import Testimonial from "./views/frontend/Testimonial.vue";
import Team from "./views/frontend/Team.vue";
import Job from "./views/frontend/Job.vue";

// admin routes
import AdminLayout from "./views/admin/AdminLayout.vue";
import Dashboard from "./views/admin/Dashboard.vue";
import Users from "./views/admin/definitions/Users.vue";
import UserAuthorization from "./views/admin/definitions/UserAuthorization.vue";
import AccessRight from "./views/admin/definitions/AccessRight.vue";
import FrontendMenu from "./views/admin/definitions/FrontendMenu.vue";
import AdminBlogCategory from "./views/admin/definitions/BlogCategory.vue";
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
        component: Page,
      },
      {
        path: "/blog/:blogUrl/:id",
        component: BlogDetail,
      },
      {
        path: "/blog/:categoryUrl",
        component: BlogCategory,
      },
      {
        path: "/blog",
        component: Blog,
      },
      {
        path: "/iletisim",
        component: Contact,
      },
      {
        path: "/hizmetler",
        component: Service
      },
      {
        path: "/referanslar",
        component: Testimonial
      },
      {
        path: "/ekibimiz",
        component: Team
      },
      {
        path: "kariyer-firsatlari",
        component: Job,
      },
      {
        path: "/giris",
        component: Login,
      },
      {
        path: "/uye-ol",
        component: Register
      },
      {
        path: "/email-dogrulama/:code",
        component: EmailVerify
      },
      {
        path: "/sifre-belirle/:code",
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
            component: Profile
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
            component: AdminBlogCategory,
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

export const router = createRouter({
  history: createWebHistory(),
  routes,
  base: "/"
});

router.beforeEach((to, from, next) => {
  if (!to.meta.requireAuth && !to.meta.isAccessAdminPanel) {
    next();
  }
  if (to.meta.requireAuth) {

    const currentUser = JSON.parse(localStorage.getItem("user"));
    if (!currentUser) {
      // authStore.logout();
      next("/giris");
    } else {
      if (to.meta.isAccessAdminPanel) {
        next();
      } else {
        next();
      }
    }
  }
});

export default router;