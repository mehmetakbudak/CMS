
$("#leftMenu").kendoTreeView();

function adjustSize() {
    if (screen.width < 800 || screen.height < 600) {
        this.maximize();
    }
}

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

function successNotification(title, message) {
    notification.show({
        title: title,
        message: message
    }, "success");
}

function errorNotification(title, message) {
    notification.show({
        title: title,
        message: message
    }, "error");
}

window.addEventListener('DOMContentLoaded', event => {
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});
