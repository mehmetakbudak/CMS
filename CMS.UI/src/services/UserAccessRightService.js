import axios from 'axios';
import authHeader from "./AuthHeader";
import {
    Endpoints
} from "./GlobalService";

class UserAccessRightService {

    get(userId) {
        return axios.get(Endpoints.UserAccessRight + "/" + userId, {
            headers: authHeader()
        });
    }

    createOrUpdate(data) {
        return axios.put(Endpoints.UserAccessRight + "/CreateOrUpdate", data, {
            headers: authHeader()
        });
    }

}

export default new UserAccessRightService();