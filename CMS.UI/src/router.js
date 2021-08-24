import {
    createWebHistory,
    createRouter,
    RouterView
} from "vue-router";
import Layout from "./views/Layout.vue";
import Home from "./views/Home.vue";
import Page from "./views/Page.vue";
import News from "./views/News.vue";
import NewsDetail from "./views/NewsDetail.vue";
import Contact from "./views/Contact.vue";
import Login from "./views/Login.vue";
import FAQ from "./views/FAQ.vue";
import store from '@/store';

// admin routes
import AdminLayout from "./admin/AdminLayout.vue";
import Dashboard from "./admin/Dashboard.vue";
import Users from "./admin/definitions/Users.vue";
import UserAuthorization from "./admin/definitions/UserAuthorization.vue";
import AdminMenu from "./admin/definitions/menus/AdminMenu.vue";
import BlogCategory from './admin/definitions/BlogCategory.vue';
import TodoCategory from './admin/definitions/TodoCategory.vue';
import TodoStatus from './admin/definitions/TodoStatus.vue';
import Author from './admin/definitions/Author.vue';

import WaitingApproveComment from './admin/comments/WaitingApproveComment.vue';
import ApprovedComment from './admin/comments/ApprovedComment.vue';
import RejectedComment from './admin/comments/RejectedComment.vue';
import Announcement from './admin/contents/Announcement.vue';
import Article from './admin/contents/Article.vue';
import Blog from './admin/contents/Blog.vue';
import ContactMessages from './admin/contents/ContactMessages.vue';
import Pages from './admin/contents/Pages.vue';
import PhotoGallery from './admin/contents/PhotoGallery.vue';
import VideoGallery from './admin/contents/VideoGallery.vue';

import MyTodos from "./admin/tools/MyTodos.vue";
import Todos from "./admin/tools/Todos.vue";

const routes = [{
        path: "/",
        component: Layout,
        children: [{
                path: "",
                component: Home
            },
            {
                path: "/sayfalar/:url",
                name: "Page",
                component: Page,
            },
            {
                path: "/haberler",
                name: "News",
                component: News,
            },
            {
                path: "/haberler/:url",
                name: "NewsDetail",
                component: NewsDetail,
            },
            {
                path: '/iletisim',
                name: "Contact",
                component: Contact
            },
            {
                path: '/giris',
                name: "Login",
                component: Login
            },
            {
                path: '/sss',
                name: "FAQ",
                component: FAQ
            },
            {
                path: '/sss/:categoryUrl',
                name: "FAQWithCategory",
                component: FAQ
            }
        ]
    },
    {
        path: "/admin",
        component: AdminLayout,
        meta: {
            requireAuth: true,
            isAdmin: true
        },
        children: [{
                path: "",
                component: Dashboard
            },
            {
                path: "icerikler",
                component: RouterView,
                children: [{
                        path: "bloglar",
                        component: Blog
                    },
                    {
                        path: "sayfalar",
                        component: Pages
                    },
                    {
                        path: "duyurular",
                        component: Announcement
                    },
                    {
                        path: "iletisim-mesajlari",
                        component: ContactMessages
                    },
                    {
                        path: "blog",
                        component: Blog
                    },
                    {
                        path: "yazar-yazilari",
                        component: Article
                    },
                    {
                        path: "sayfalar",
                        component: Pages
                    },
                    {
                        path: "foto-galeri",
                        component: PhotoGallery
                    },
                    {
                        path: "video-galeri",
                        component: VideoGallery
                    }
                ]
            },
            {
                path: "tanimlar",
                component: RouterView,
                children: [{
                        path: "kullanicilar",
                        component: Users
                    },
                    {
                        path: "kullanici-yetkilendirme",
                        component: UserAuthorization
                    },
                    {
                        path: "blog-kategorileri",
                        component: BlogCategory
                    },
                    {
                        path: "yapilacak-kategorileri",
                        component: TodoCategory
                    },
                    {
                        path: "yapilacak-durumlari",
                        component: TodoStatus
                    },
                    {
                        path: "yazarlar",
                        component: Author
                    },
                    {
                        path: "menuler",
                        component: RouterView,
                        children: [{
                            path: "admin-menu",
                            component: AdminMenu
                        }, ]
                    },
                ]
            },
            {
                path: "yorumlar",
                component: RouterView,
                children: [{
                        path: "onay-bekleyenler",
                        component: WaitingApproveComment
                    },
                    {
                        path: "onaylananlar",
                        component: ApprovedComment
                    },
                    {
                        path: "reddedilenler",
                        component: RejectedComment
                    }
                ],
            },
            {
                path: "araclar",
                component: RouterView,
                children: [{
                        path: "yapilacaklar",
                        component: Todos
                    },
                    {
                        path: "bana-atananlar",
                        component: MyTodos
                    }
                ]
            }
        ]
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    const currentUser = JSON.parse(localStorage.getItem('user'));
    if (currentUser) {
        const result = currentUser.menuAccessRights.filter(x => x == to.fullPath);
        if (result.length > 0) {
            next();
        } else {
            store.dispatch("auth/logout");
            next('/giris');
        }
    } else {
        if (to.meta.requireAuth) {
            next('/giris');
        } else {
            next();
        }
    }
})

export default router;