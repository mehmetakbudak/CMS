import Vue from "vue";
import VueRouter from 'vue-router';

import Layout from "./components/Layout";
import Home from "./components/Home";
import Grid from "./components/Grid";
import Login from "./components/Login";
import Contact from "./components/Contact";
import AdminLayout from "./components/admin/AdminLayout";


Vue.use(VueRouter);

export const router = new VueRouter({
    routes: [{
            path: "/",
            component: Layout,
            children: [{
                    path: "/",                    
                    component: Home
                },
                {
                    path: "/grid",
                    component: Grid
                },
                {
                    path: "/giris",
                    component: Login
                },
                {
                    path:"/iletisim",
                    component: Contact
                }
            ]
        },
        {
            path: "/admin",
            component: AdminLayout
        }
    ],
    mode: "history"
});