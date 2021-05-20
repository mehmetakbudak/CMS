export const API_URL = process.env.VUE_APP_BASEURL;

export const Endpoints = {
    User: API_URL + "User",
    AccessRight: API_URL + "AccessRight",
    UserAccessRight: API_URL + "UserAccessRight",
    Lookup: {
        Users: API_URL + "Lookup/Users",
    }
}