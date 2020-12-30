function login() {
    utils.loading();

    $.ajax({
        type: 'POST',
        url: '/account/checkLogin',
        dataType: 'json',
        data: { userName: $('#Username').val(), passWord: $('#Password').val() },
        success: function (response) {
            if (response.status) {
                utils.done();
                Lobibox.alert("success", {
                    msg: response.mess,
                    beforeClose: function () {
                        var rtUrl = $('#rtUrl').val();
                        if (rtUrl === "") {
                            rtUrl = "/";
                        }
                        window.location.href = rtUrl;
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
        }
    });
};
$(document).ready(function () {
    $('input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            login();
            return false;
        }
    });
    $('#btnLogin').click(function (e) {
        e.preventDefault();
        login();
    });

    $('#frmLogin').submit(function (e) {
        e.preventDefault();
    });

    $('#frmReg').submit(function (e) {
        e.preventDefault();
        utils.loading();
        var formData = new FormData(this);
        $.ajax({
            url: '/account/register',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.status) {
                    utils.done();
                    Lobibox.alert("success", {
                        msg: response.mess,
                        beforeClose: function () {
                            var rtUrl = $('#rtUrl').val();
                            if (rtUrl === "") {
                                rtUrl = "/";
                            }
                            window.location.href = rtUrl;
                        }
                    });
                    
                } else {
                    utils.done();
                    Lobibox.alert("error", {
                        msg: response.mess
                    });
                }
            },
            cache: false,
            contentType: false,
            processData: false
        });
    });
});