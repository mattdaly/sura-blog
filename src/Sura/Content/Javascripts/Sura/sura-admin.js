$(document).ready(function () {
    var page = window.location.pathname.split('admin/')[1];
    var sub = page.indexOf('/');
    if (sub !== -1) {
        page = page.substr(0, sub) + '-' + page.substr(sub + 1);
    }

    $('li.nav-' + page).addClass('active');
});

function redirect(url) {
    window.location.href = url;
}