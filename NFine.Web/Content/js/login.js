
(function ($) {
    "use strict";
    var lrPage = {
        init: function () {
            if (window.location.href != top.window.location.href) {
                top.window.location.href = window.location.href;
            }
            var isIE = !!window.ActiveXObject;
            var isIE6 = isIE && !window.XMLHttpRequest;
            if (isIE6) {
                window.location.href = $.rootUrl + "/Error/ErrorBrowser";
            }
            lrPage.bind();
            if ($.cookie("xuantu_rmbUser") == "true") {
                $("#rememberMe").attr("checked", true);
                $("#lr_username").val($.cookie("xuantu_username"));
                $("#lr_password").val($.cookie("xuantu_password"));
            }
            var login_error = top.$.cookie('nfine_login_error');
            if (login_error != null) {
                switch (login_error) {
                    case "overdue":
                        lrPage.tip("系统登录已超时,请重新登录", true);
                        break;
                    case "OnLine":
                        lrPage.tip("您的帐号已在其它地方登录,请重新登录", true);
                        break;
                    case "-1":
                        lrPage.tip("系统未知错误,请重新登录", true);
                        break;
                }
                top.$.cookie('nfine_login_error', '', { path: "/", expires: -1 });
            }
            $('#errornum').val('0');
        },
        bind: function () {
            // 回车键
            document.onkeydown = function (e) {
                e = e || window.event;
                if ((e.keyCode || e.which) == 13) {
                    $('#lr_login_btn').trigger('click');
                }
            }
            //输入框获取焦点
            $('.lr-input-item input').on('focus', function () {
                var $item = $(this).parent();
                $item.addClass('focus');
            }).on('blur', function () {
                var $item = $(this).parent();
                $item.removeClass('focus');
            });

            // 点击切换验证码
            $("#lr_verifycode_img").click(function () {
                $("#lr_verifycode_input").val('');
                $("#lr_verifycode_img").attr("src", "/Login/GetAuthCode?time=" + Math.random());
            });

            // 登录按钮事件
            $("#lr_login_btn").on('click', function () {
                lrPage.login();

                var errornum = parseInt($('#errornum').val());
                if (errornum >= 3) {
                    $('#lr_verifycode_input').parent().show();
                    $("#lr_verifycode_img").trigger('click');
                }
            });
        },
        login: function () {
            lrPage.tip();

            var $username = $("#lr_username"), $password = $("#lr_password"), $verifycode = $("#lr_verifycode_input");
            var username = $.trim($username.val()), password = $.trim($password.val()), verifycode = $.trim($verifycode.val());

            if (username == "") {
                lrPage.tip('请输入账户。');
                $username.focus();
                return false;
            }
            if (password == "") {
                lrPage.tip('请输入密码。');
                $password.focus();
                return false;
            }

            if ($("#lr_verifycode_input").is(":visible") && verifycode == "") {
                lrPage.tip('请输入验证码。');
                $verifycode.focus();
                return false;
            }

            if ($("#rememberMe").prop("checked")) {
                $.cookie("xuantu_rmbUser", "true", { expires: 7 }); //存储一个带7天期限的cookie  
                $.cookie("xuantu_username", username, { expires: 7 });
                $.cookie("xuantu_password", password, { expires: 7 });
            }
            else {
                $.cookie("xuantu_rmbUser", "false", { expire: -1 });
                $.cookie("xuantu_username", "", { expires: -1 });
                $.cookie("xuantu_password", "", { expires: -1 });
            }

            password = $.md5(password);
            var xuantu_errornum = $.cookie("xuantu_errornum");
            lrPage.logining(true);
            $.ajax({
                url: $.rootUrl + "/Login/CheckLogin",
                data: { username: username, password: password, code: verifycode, errornum: xuantu_errornum },
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.state == "success") {
                        window.location.href = "/Home/Index";
                    }
                    else {
                        lrPage.logining(false);
                        lrPage.tip(data.message, true);
                        var errornum = parseInt($('#errornum').val()) + 1;
                        $('#errornum').val(errornum)

                    }
                }
            });
        },
        logining: function (isShow) {
            if (isShow) {
                $('input').attr('disabled', 'disabled');
                $("#lr_login_btn").addClass('active').attr('disabled', 'disabled').find('span').hide();
            }
            else {
                $('input').removeAttr('disabled');
                $("#lr_login_btn").removeClass('active').removeAttr('disabled').find('span').show();
            }
        },
        tip: function (msg) {
            var $tip = $('#lr_tips');
            $tip.hide();
            if (!!msg) {
                $tip.html('<b></b>' + msg);
                $tip.show();
            }
        }
    };
    $(function () {
        lrPage.init();
    });
})(window.jQuery)