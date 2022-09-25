import {
    defineStore
} from 'pinia';
import {
    Endpoints
} from "../services/Endpoints";
import GlobalService from "../services/GlobalService";

export const useAuthStore = defineStore({
    id: 'auth',
    state: () => ({
        // initialize state from local storage to enable user to stay logged in
        user: JSON.parse(localStorage.getItem('user')),
        loggedIn: false,
        returnUrl: null
    }),
    actions: {
        login(data) {
            GlobalService.Post(Endpoints.Account.Login, data)
                .then((res) => {
                    // update pinia state
                    this.user = res.data.data;
                    this.loggedIn = true;
                    // store user details and jwt in local storage to keep user logged in between page refreshes
                    localStorage.setItem('user', JSON.stringify(this.user));

                    // redirect to previous url or default to home page
                    window.location.href = this.returnUrl || '/';
                });
        },
        logout() {
            this.user = null;
            localStorage.removeItem('user');
            window.location.href = "/";
        }
    }
});