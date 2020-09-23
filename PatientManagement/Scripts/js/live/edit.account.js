$(document).ready(function () {
    $('#frmEdit').submit(function (e) {
        e.preventDefault();
        utils.loading();
        var formData = new FormData(this);
        $.ajax({
            url: '/account/update',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.status) {
                    utils.done();
                    Lobibox.alert("success", {
                        msg: response.mess,
                        beforeClose: function () {
                            window.location.href = '/account';
                        }
                    });
                } else {
                    utils.done();
                    Lobibox.alert("error", {
                        msg: response.mess
                    });
                }
            },
            error: function (jqXHR) {
                utils.done();
                Lobibox.alert("error", {
                    msg: "ERR"
                });
            },
            cache: false,
            contentType: false,
            processData: false
        });
    });

    $('#updatePass').submit(function (e) {
        e.preventDefault();
        utils.loading();
        var formData = new FormData(this);
        $.ajax({
            url: '/account/updatepass',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.status) {
                    utils.done();
                    Lobibox.alert("success", {
                        msg: response.mess,
                        beforeClose: function () {
                            window.location.href = '/account';
                        }
                    });
                } else {
                    utils.done();
                    Lobibox.alert("error", {
                        msg: response.mess
                    });
                }
            },
            error: function (jqXHR) {
                utils.done();
                Lobibox.alert("error", {
                    msg: "ERR"
                });
            },
            cache: false,
            contentType: false,
            processData: false
        });
    });
});