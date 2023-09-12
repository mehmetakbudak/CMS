const appModule = angular.module('app', ['dx']);
appModule.controller('HeaderController', ($scope, $http) => {
    $scope.languages = [{ name: "TR", value: "tr-TR" }, { name: "EN", value: "en-US" }];

    getMenu();
    getCurrentLanguage();

    $scope.menuOptions = {
        displayExpr: 'title',
        itemsExpr: 'children',
        orientation: 'horizontal',
        itemTemplate: 'itemTemplate',
        showFirstSubmenuMode: 'onHover',
        hideSubmenuOnMouseLeave: true,
        onItemClick(data) {
            const item = data.itemData;

        },
        bindingOptions: {
            dataSource: 'menuData',
        },
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

var app = {};

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

    var menuDataSource = new kendo.data.HierarchicalDataSource({
        transport: {
            read: {
                url: "/api/menu/frontend",
                dataType: "json"
            }
        },
        schema: {
            model: {
                children: "items"
            }
        }
    });

    $("#topMenu").kendoMenu({
        dataSource: menuDataSource,
        dataUrlField: "url",
        dataTextField: "title"
    });

    $("#profileMenu").kendoPanelBar({});

    var validatorNewsletter = $("#formNewsletter").kendoValidator().data("kendoValidator");

    $("#btnNewsletter").click((e) => {
        e.preventDefault();
        if (validatorNewsletter.validate()) {
            var data = {
                emailAddress: $("#txtEmailAddress").val()
            };

        }
    });

    var notification = $("#notification").kendoNotification({
        position: {
            pinned: true,
            top: 30,
            right: 30
        },
        autoHideAfter: 5000,
        stacking: "down",
        templates: [{
            type: "error",
            template: $("#errorTemplate").html()
        },
        {
            type: "success",
            template: $("#successTemplate").html()
        }]
    }).data("kendoNotification");

    app.successNotification = function successNotification(title, message) {
        notification.show({
            title: title,
            message: message
        }, "success");
    }

    app.errorNotification = function errorNotification(title, message) {
        notification.show({
            title: title,
            message: message
        }, "error");
    }

})

function dateFormat(value) {
    if (value) {
        var formattedDate = new Date(value);
        var year = formattedDate.getFullYear();
        var month = formattedDate.getMonth() + 1;
        var day = formattedDate.getDate();
        if (day < 10) {
            day = "0" + day;
        }
        if (month < 10) {
            month = "0" + month;
        }
        return (day + "." + month + "." + year);
    } else {
        return '';
    }
}

function dateTimeFormat(value) {
    if (value) {
        var formattedDate = new Date(value);
        var year = formattedDate.getFullYear();
        var month = formattedDate.getMonth() + 1;
        var day = formattedDate.getDate();
        var hour = formattedDate.getHours();
        var minute = formattedDate.getMinutes();
        var second = formattedDate.getSeconds();

        if (day < 10) {
            day = "0" + day;
        }
        if (month < 10) {
            month = "0" + month;
        }
        if (hour < 10) {
            hour = "0" + hour;
        }
        if (minute < 10) {
            minute = "0" + minute;
        }
        if (second < 10) {
            second = "0" + second;
        }
        return (day + "." + month + "." + year + " " + hour + ":" + minute + ":" + second);
    } else {
        return '';
    }
}
