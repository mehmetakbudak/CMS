import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

export class BlogCategoryService {

    getAll() {
        return axios.get(Endpoints.BlogCategory, {
            headers: authHeader()
        });
    }
}

export default new BlogCategoryService();