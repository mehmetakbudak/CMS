import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

export class TodoStatusService {

    getAll() {
        return axios.get(Endpoints.TodoStatus, {
            headers: authHeader()
        });
    }

    getByTodoCategoryId(todoCategoryId) {
        return axios.get(Endpoints.TodoStatus + "/GetByTodoCategoryId/" + todoCategoryId, {
            headers: authHeader()
        });
    }
}

export default new TodoStatusService();