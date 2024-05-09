const appModule = angular.module('app', ['dx']);
appModule.controller('menuController', ($scope, $http) => {
    $scope.adminDataSource;

    getAdminMenu();

    $scope.menuOptions = {
        displayExpr: 'title',
        itemsExpr: 'children',
        orientation: 'horizontal',
        itemTemplate: 'itemTemplate',
        showFirstSubmenuMode: 'onHover',
        hideSubmenuOnMouseLeave: true,
        bindingOptions: {
            dataSource: 'menuData',
        }
    };

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

appModule.controller('navbarController', ($scope, $http) => {
    $scope.languages = [{ name: "TR", value: "tr-TR" }, { name: "EN", value: "en-US" }];

    getCurrentLanguage();

    function getCurrentLanguage() {
        $http.get("/language/currentlanguage").then((res) => {
            $scope.item = $scope.languages.find(x => x.value == res.data);
        });
    }

    $scope.selectLanguage = function () {
        $http.get(`/language/ChangeLanguage/?culture=${$scope.item.value}`).then(() => {
            window.location.reload();
        });
    }
});

function adjustSize() {
    if (screen.width < 800 || screen.height < 600) {
        this.maximize();
    }
}
