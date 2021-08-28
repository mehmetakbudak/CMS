import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

export class AuthorService {

    getAll() {
        return axios.get(Endpoints.Author, {
            headers: authHeader()
        });
    }

    post(data) {
        return axios.post(Endpoints.Author, data, {
            headers: authHeader()
        });
    }

    put(data) {
        return axios.put(Endpoints.Author, data, {
            headers: authHeader()
        });
    }

    delete(id) {
        return axios.delete(Endpoints.Author + "/" + id, {
            headers: authHeader()
        });
    }
}

export default new AuthorService();