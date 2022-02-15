import axios from 'axios';
import { Endpoints } from './Endpoints';

const API_URL = process.env.VUE_APP_BASEURL;

class AuthService {
    login(user) {
        return axios.post(API_URL + Endpoints.Account.Login, {
            emailAddress: user.emailAddress,
            password: user.password
        }).then(response => {
            if (response.data) {
                localStorage.setItem('user', JSON.stringify(response.data.data));
            }
            return response.data.data;
        });
    }

    logout() {
        localStorage.removeItem('user');
    }

    register(user) {
        return axios.post(API_URL + 'signup', {
            nameSurname: user.nameSurname,
            emailAddress: user.emailAddress,
            password: user.password
        });
    }
}

export default new AuthService();