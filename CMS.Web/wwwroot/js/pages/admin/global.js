const appModule = angular.module('app', ['dx']);
appModule.controller('menuController', ($scope, $http) => {
    $scope.adminDataSource;

    getAdminMenu();

    $scope.adminMenuOptions = {
        bindingOptions: {
            dataSource: 'adminDataSource',
        },
        searchEnabled: true,
        searchMode: "contains",
        searchEditorOptions: {
            placeholder: "Ara",
            stylingMode: 'outlined'
        },
        itemsExpr: 'items',
        keyExpr: 'id',
        displayExpr: 'title',
        itemTemplate: 'itemTemplate',
        dataStructure: 'plain',
        parentIdExpr: 'parentId',
    };

    function getAdminMenu() {
        $http.get("/admin/menu/user-admin-menu").then((res) => {
            $scope.adminDataSource = res.data;
        });
    }

});


function adjustSize() {
    if (screen.width < 800 || screen.height < 600) {
        this.maximize();
    }
}
