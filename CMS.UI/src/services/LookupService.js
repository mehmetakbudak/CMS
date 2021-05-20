import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

class LookupService {

    getUsers() {
        return axios.get(Endpoints.Lookup.Users, {
            headers: authHeader()
        });
    }
}

export default new LookupService();