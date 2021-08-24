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
}

export default new AuthorService();