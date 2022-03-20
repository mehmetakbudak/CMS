import {
    Endpoints
} from './Endpoints';
import GlobalService from "./GlobalService";

class AuthService {
    login(user) {
        return GlobalService.Post(Endpoints.Account.Login, {
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
}

export default new AuthService();