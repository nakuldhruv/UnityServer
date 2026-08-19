// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// 表单提交失败（例如应用/容器重启后防伪令牌过期，服务器返回 400）时，
// 自动刷新页面获取新令牌并让用户重新提交，避免停留在错误页面。
$(function () {
    var submitting = false;
    $("form[action$='/Home/Submit']").on("submit", function (e) {
        e.preventDefault();
        if (submitting) {
            return;
        }
        submitting = true;
        var $form = $(this);
        $.ajax({
            url: $form.attr("action"),
            method: "POST",
            data: $form.serialize()
        }).always(function () {
            window.location.reload();
        });
    });
});
