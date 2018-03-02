$(document).ready(function () {
    if (!isWeiXin()) {
        location.href = "Themes/Default/message.html";
    }
})
function isWeiXin() {
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
        return true;
    } else {
        return false;
    }
}