export const API_URL = process.env.VUE_APP_BASEURL;

export const Endpoints = {
    User: addApiUrl("User"),
    AccessRight: addApiUrl("AccessRight"),
    UserAccessRight: addApiUrl("UserAccessRight"),
    Todo: addApiUrl("Todo"),
    TodoCategory: addApiUrl("TodoCategory"),
    TodoStatus: addApiUrl("TodoStatus"),
    BlogCategory: addApiUrl("BlogCategory"),
    Author: addApiUrl("Author"),
    Lookup: {
        Users: addApiUrl("Lookup/Users"),
    }
}

function addApiUrl(endPoint) {
    return API_URL + endPoint;
}