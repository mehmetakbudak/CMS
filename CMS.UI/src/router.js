import {
    createWebHistory,
    createRouter
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
import Users from "./admin/Users.vue";
import UserAuthorization from "./admin/UserAuthorization.vue";
import AdminMenu from "./admin/AdminMenu.vue";

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
                component: Dashboard,
                name: "Dashboard"
            },
            {
                path: "kullanicilar",
                component: Users,
                name: "User"
            },
            {
                path: "kullanici-yetkilendirme",
                component: UserAuthorization,
                name: "UserAuthorization"
            },
            {
                path: "menuler",
                name: "Menus",
                component: AdminMenu,
                children: [{
                    path: "admin-menu",
                    component: AdminMenu
                }]
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