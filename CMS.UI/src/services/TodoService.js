import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

export class TodoService {

    getAll() {
        return axios.get(Endpoints.Todo, {
            headers: authHeader()
        });
    }

    post(data) {
        return axios.post(Endpoints.Todo, data, {
            headers: authHeader()
        });
    }

    put(data) {
        return axios.put(Endpoints.Todo, data, {
            headers: authHeader()
        });
    }

    delete(id) {
        return axios.delete(Endpoints.Todo + "/" + id, {
            headers: authHeader()
        });
    }
}

export default new TodoService();