const appModule = angular.module('app', ['dx', 'ngSanitize']);
appModule.controller('HeaderController', ($scope, $http) => {
    $scope.languages = [{ name: "TR", value: "tr-TR" }, { name: "EN", value: "en-US" }];
    $scope.menuData = [];

    getMenu();
    getCurrentLanguage();

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

    $scope.treeMenuOptions = {
        itemsExpr: 'children',
        itemTemplate: 'itemTemplate',
        bindingOptions: {
            dataSource: 'menuData',
        }
    };
    function getMenu() {
        $http.get("/menu/frontend").then((res) => {
            $scope.menuData = res.data;
        });
    }

    function getCurrentLanguage() {
        $http.get("/language/currentlanguage").then((res) => {
            $scope.item = $scope.languages.find(x => x.value == res.data);
        });
    }

    $scope.selectLanguage = function () {
        window.location.href = `/?culture=${$scope.item.value}`;
    }
});

$(() => {
    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    const on = (type, el, listener, all = false) => {
        let selectEl = select(el, all)
        if (selectEl) {
            if (all) {
                selectEl.forEach(e => e.addEventListener(type, listener))
            } else {
                selectEl.addEventListener(type, listener)
            }
        }
    }

    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    on('click', '.mobile-nav-toggle', function (e) {
        select('#navbar').classList.toggle('navbar-mobile')
        this.classList.toggle('bi-list')
        this.classList.toggle('bi-x')
    })

    on('click', '.navbar .dropdown > a', function (e) {
        if (select('#navbar').classList.contains('navbar-mobile')) {
            e.preventDefault()
            this.nextElementSibling.classList.toggle('dropdown-active')
        }
    }, true)

    let heroCarouselIndicators = select("#hero-carousel-indicators")
    let heroCarouselItems = select('#heroCarousel .carousel-item', true)

    heroCarouselItems.forEach((item, index) => {
        (index === 0) ?
            heroCarouselIndicators.innerHTML += "<li data-bs-target='#heroCarousel' data-bs-slide-to='" + index + "' class='active'></li>" :
            heroCarouselIndicators.innerHTML += "<li data-bs-target='#heroCarousel' data-bs-slide-to='" + index + "'></li>"
    });
});