var url = location.href;
var stoptime;
if (url.indexOf("msg") > 0) {
    if (stoptime != -1)
    {
        stoptime = 5; urlhref();
    }
    
}
$(function () {
    $('.divChatPhotos').magnificPopup({
        delegate: 'a',
        type: 'image',
        tLoading: '正在加载图片 #%curr%...',
        mainClass: 'mfp-img-mobile',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        },
        image: {
            tError: '<a href="%url%">第 #%curr%</a> 张图片加载失败',
            titleSrc: function (item) {
                return item.el.attr('data-name') + '<small>' + item.el.attr('data-time') + '</small>';
            }
        }
    });
    try {
        $("span[id='startLottery']").click(function () {
            setTimer();
        });
        $("span[id='endLottery']").click(function () {
            clearTimer();
        });
        $('.flbtn-close').click(function () {
            $(".divCodeimg").css('display', 'none');
            $("#chatsPage").css('display', 'block');
            var url = location.href;
            if (url.indexOf("msg") > 0) {
                stoptime = 5; urlhref();
            }
        });
        $("div[class='und-btn']").click(function () {
            if (confirm("确定要重新操作？")) {
                var sitecode = Request("sitecode");
                var rmid = Request("rmid");
                $.getJSON("MsgWall.aspx?", {
                    "action": "delallwin", "rmid": rmid,"sitecode": sitecode
                }, function (json) {
                    if (json.success == 'true' || json.success == true) {
                        location.href = location.href;
                    } else {
                        alert(json.success); return false;
                    }
                })
            }
        });
    } catch (e) {
    }

    var isChrome = navigator.userAgent.indexOf('Chrome') != -1 && $.browser.webkit;
    if (!isChrome) {
        $('.chromeTip').show();
    }
    //
    $('#chromeTipCloseBtn').click(function () {
        $('.chromeTip').hide();
    });
    //10s后自动关闭
    setTimeout(function () {
        $('.chromeTip').hide();
    }, 10000);
});

function showdel(strid) {
    $('#' + strid + ' a').css('display', 'inline');
}
function hidedel(strid) {
    $('#' + strid + ' a').css('display', 'none');
}

function countDown(stoptime) {
    if (stoptime >= 1) {
        stoptime--;
        setTimeout("countDown(" + stoptime + ")", 1000);
    }
    else {
        clearTimer();
    }
}
function urlhref() {
    if (stoptime >= 1) {
        stoptime--;
        setTimeout("urlhref()", 1000);
    }
    else if (stoptime == -1)
    {
        return;
    }
    else {
        location.href = location.href;
    }
}
function setTimeCount() {
    timecount = $("#lotteryNumSel").val();
}


function ShowCodeimgdiv() {
    stoptime =-1;
    $(".divCodeimg").css('display', 'block');
    $("#chatsPage").css('display', 'none');
}
function ShowRightPanel() {
    try {
        stoptime = -1;
        $(".divContent").css('padding-right', '250px');
        $(".divCodeimg").css('padding-right', '250px');
        document.getElementById("rightPanel").className = "ui-panel ui-panel-position-right ui-panel-display-overlay ui-body-none ui-panel-animate ui-panel-open ui-panel-fixed";
    } catch (e) {
    }
}
function HideRightPanel() {
    try {
        document.getElementById("rightPanel").className = "ui-panel ui-panel-position-right ui-panel-display-overlay ui-body-none ui-panel-animate ui-panel-closed ui-panel-fixed";
        $(".divContent").css('padding-right', '10px');
        $(".divCodeimg").css('padding-right', '10px');
        var url = location.href;
        if (url.indexOf("msg") > 0) {
            stoptime = 5; urlhref();
        }
    } catch (e) {
    }
}
function GotoFullscreen() {
    var el = document.documentElement
    , rfs = // for newer Webkit and Firefox
           el.requestFullScreen
        || el.webkitRequestFullScreen
        || el.mozRequestFullScreen
        || el.msRequestFullScreen
    ;
    if (typeof rfs != "undefined" && rfs) {
        rfs.call(el);
    } else if (typeof window.ActiveXObject != "undefined") {
        // for Internet Explorer
        var wscript = new ActiveXObject("WScript.Shell");
        if (wscript != null) {
            wscript.SendKeys("{F11}");
        }
    }
}

function setValues() {
    var strUname = document.getElementById("rockname").innerText;
    var uid = $("#rockname").attr("data-id");
    var rmid = $("#rockname").attr("data-rmid");
    var htmllist = $('#prize-list').html();
    var sitecode = Request("sitecode");
    $.getJSON("MsgWall.aspx", { "action": "savewin", "uid": uid, "rmid": rmid, "sitecode": sitecode }, function (json) {
        if (json.success == 'true' || json.success == true) {
            lotterynum++; linum++;
            $(".winUserNum").html(lotterynum);
            $(".lotteryUserNum").html($(".lotteryUserNum").html() - 1);
            var str = "";
            str = "\r\n<li id=\"prizeli" + uid + "\" data-id=\"" + uid + "\"  data-rmid=\"" + rmid + "\" onmouseover=\"showdel(this.id)\" onmouseout=\"hidedel(this.id)\">" +
                    "\r\n<span class=\"num-p\"><em>" + linum + "</em></span>" +
            "\r\n<span class=\"nick-name\">" + strUname + "</span> " +
            "\r\n<a href=\"javascript:void(0);\" class=\"deluser\" title=\"删除\" onclick=\"delprizelist('prizeli" + uid + "')\">×</a>" +
            "\r\n</li>";
            if (htmllist != null && htmllist != "") {
                $('#prize-list').html(htmllist + str);
            } else {
                $('#prize-list').html(str);
            }
        } else {
            alert(json.success); return false;
        }
    })
}
function delprizelist(obj) {
    if (confirm("确定要删除该项？")) {
        var dataid; var datao; var datarmid; var uname; var imgurl;
        dataid = $("#" + obj).attr("data-id");
        datarmid = $("#" + obj).attr("data-rmid");
        uname = $("#" + obj + " span[class=\"nick-name\"]").html();
        var sitecode = Request("sitecode");
        var str = "{\"id\":\"" + dataid + "\",\"rmid\":\"" + datarmid + "\",\"nickname\":\"" + uname + "\"}";
        str = eval("(" + str + ")");
        $.getJSON("MsgWall.aspx?", {
            "action": "delwin", "uid": dataid, "rmid": datarmid,"sitecode": sitecode
        }, function (json) {
            if (json.success == 'true' || json.success == true) {
                areaServer.push(str);
                $("#" + obj).remove();
                num = areaServer.length + 1;
                lotterynum--; timecount++;
                var wincount = $(".winUserNum").html();
                if (wincount == 0) {
                    wincount = "";
                } else {
                    wincount--;
                }
                $(".winUserNum").html(wincount);
                var lotCount = $(".lotteryUserNum").html();
                if (lotCount == 0) {
                    lotCount = "";
                } else {
                    lotCount++;
                }
                $(".lotteryUserNum").html(lotCount);
            } else {
                alert(json.success); return false;
            }
        })
    }
}

function Request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}