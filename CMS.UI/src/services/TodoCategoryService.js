import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

export class TodoCategoryService {

    getAll() {
        return axios.get(Endpoints.TodoCategory, {
            headers: authHeader()
        });
    }
}

export default new TodoCategoryService();