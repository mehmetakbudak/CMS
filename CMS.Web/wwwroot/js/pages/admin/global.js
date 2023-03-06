var app = {};

$(() => {
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

    $("#leftMenu").kendoPanelBar({
    });

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


    app.dateFormat = function dateFormat(value) {
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

    app.dateTimeFormat = function dateTimeFormat(value) {
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
});

function adjustSize() {
    if (screen.width < 800 || screen.height < 600) {
        this.maximize();
    }
}
