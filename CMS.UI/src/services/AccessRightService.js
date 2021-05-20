import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

class AccessRightService {

    get() {
        return axios.get(Endpoints.AccessRight, {
            headers: authHeader()
        });
    }

}

export default new AccessRightService();