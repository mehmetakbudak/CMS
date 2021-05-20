import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

class UserService {

    getAll() {
        return axios.get(Endpoints.User, {
            headers: authHeader()
        });
    }

    post(data) {
        return axios.post(Endpoints.User, data, {
            headers: authHeader()
        });
    }

    put(data) {
        return axios.put(Endpoints.User, data, {
            headers: authHeader()
        });
    }

    delete(id) {
        return axios.delete(Endpoints.User + id, {
            headers: authHeader()
        });
    }
}

export default new UserService();