var rid = Request("rid");
Loading = $("#inputdiv");
Mobile = $(".mobile");
GetRaceDetail();
$(document).ready(function () {
    if (rid != null && rid != "") {
        if ((navigator.userAgent.match(/(iPhone|iPod|Android|ios)/i))) {
            $(".raceman").hide(); Loading.show(); Mobile.hide();
        } else {
            $(".raceman").show(); Loading.hide(); Mobile.hide();
            $(".startrace").show(); $(".racecontent").hide();
            $(".startbtn").click(function () {
                $(".startrace").hide(); $(".racecontent").show();
                GetRaceList();
                setInterval("GetRaceList()", 10000);//定时器
            });
        }
    } else {
        alert("请求失败，请重新操作！");
        history.back();
    }
    })
function GetRaceDetail() {
    $.getJSON("MoveRace.aspx", { "action": "detail", "rid": rid },
        function (json) {
            document.title = json[0]["Rtitle"];
            var codeimg = json[0]["CodeImg"];
            var appid = json[0]["AppID"];
            var htmlmsg = "";
            if (codeimg == null || codeimg == "") {
                codeimg = "images/photo-default.png";
            }
            htmlmsg = "\r\n<img src=\"" + codeimg + "\" alt=\"" + json[0]["Rtitle"] + "\" />\r\n";
            if (appid != null && appid != "") {
                htmlmsg  += "\r\n<div class=\"rightdiv\">\r\n扫描关注左侧微信二维码或添加公共号<br />" +
                        " \r\n<span class=\"SpanRed\">" + appid + "</span>\r\n进行关注<br />发送摇一摇参加活动" +
                         "\r\n</div>\r\n";
            } else {
                htmlmsg  += "\r\n<div class=\"rightdiv\">\r\n扫描关注左侧微信二维码<br />发送摇一摇参加活动" +
                         "\r\n</div>\r\n";
            }
            $(".centerdesc").html(htmlmsg);
        })
}
function GetRaceList() {
    $.getJSON("MoveRace.aspx", { "action": "raceuserlist", "rid": rid },
        function (json) {
            var htmlmsg = "\r\n<ul>"; 
            $.each(json, function (i, item) {
                //var headimgurl = item.HeadImgUrl;
                var speed = item.Speed;
                var nickname = item.OpenID;
                //htmlmsg += "\r\n<li title=\"" + nickname + "\" id=\"list" + i + "\">" +
                //    "\r\n<img class=\"headimg\" src=\"" + headimgurl + "\" alt=\"" + nickname + "\" />" +
                //    "\r\n<span class=\"nickspan\">" + nickname + "</span>" +
                //    "\r\n<div class=\"prbar\"><p class=\"prpos\"><img src=\"images/ajax-loader.gif\" /></p></div>\r\n</li>";
                htmlmsg += "\r\n<li title=\"" + nickname + "\" id=\"list" + i + "\">" +
                    "\r\n<span class=\"nickspan\">" + nickname + "</span>" +
                    "\r\n<div class=\"prbar\"><p class=\"prpos\"><img src=\"images/ajax-loader.gif\" /></p></div>\r\n</li>";
            })
            htmlmsg += "\r\n</ul>";
            $("#list").html(htmlmsg);
            var a = $(".list li");
            var speedmax = 10;//初始速度
            var progress =100;//进度  长度
            for (var i = 0; i < a.length; i++) {
                GetProgress(0, progress, speedmax, a[i]);
                progress = progress - 10; speedmax = speedmax + 10;
            }
        })
}
function GetProgress(v, p,speedin, el) {
    if (v <= p)
        {
        setSB(v, el);
        window.setTimeout("GetProgress(" + (++v) + "," + p + "," + speedin + ", document.all['" + el.id + "'])", speedin);
    }
}
function setSB(v, el) {
    var ie5 = (document.all && document.getElementsByTagName);
    if (ie5 || document.readyState == "complete") {
        $("#" + el.id + " p").css("width", v + "%");
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

///////////Mobile/////////////////////////

var SHAKE_THRESHOLD;
var last_update = 0;
var x = y = z = last_x = last_y = last_z = 0;
function mobileinit() {
    $.getJSON("MoveRace.aspx", { "action": "getmovenum", "rid": rid}, function (json) {
        if (json.movenum != '' && json.movenum != null) {
            SHAKE_THRESHOLD = json.movenum;
        } else {
            SHAKE_THRESHOLD = 3000;
        }
    })
    last_update = new Date().getTime();
    if (window.DeviceMotionEvent) {
        window.addEventListener('devicemotion', deviceMotionHandler, false);
    } else {
        alert('该手机不支持');
    }
}
var NickName; var countnum=0;
function deviceMotionHandler(eventData) {
    var acceleration = eventData.accelerationIncludingGravity;
    var curTime = new Date().getTime();
    if ((curTime - last_update) > 100) {
        var diffTime = curTime - last_update;
        last_update = curTime;
        x = acceleration.x;
        y = acceleration.y;
        z = acceleration.z;
        var speed = Math.abs(x + y + z - last_x - last_y - last_z) / diffTime * 10000;
        $(".movespeed").html(speed);
        if (speed > SHAKE_THRESHOLD) {
            if (NickName != null && NickName != "") {
           $.getJSON("MoveRace.aspx", { "action": "newrace", "rid": rid, "speed": speed,"nickname":NickName},function (json) {
               if (json.success == 'true' || json.success == true) {
                   countnum++;//记录次数
                   $(".movespeed").html("这是第"+countnum+"次了，加油哦！！！<br/>"+speed);
               } 
            })}
        }
        last_x = x;
        last_y = y;
        last_z = z;
    }
}

$(".inputbtn").click(function () {
    var inputNickName = $(".inputNickName").val();
    if (inputNickName == null || inputNickName == "") {
        alert("请输入您的昵称后再操作！！"); return false;
    } else {
        NickName = inputNickName;
        $.getJSON("MoveRace.aspx", { "action": "newrace", "rid": rid,"nickname":NickName},function (json) {
            if (json.success == 'true' || json.success == true) {
                Loading.hide(); Mobile.show(); mobileinit();
            } else {
                alert(json.success); return false;
            }
       })
    }
})