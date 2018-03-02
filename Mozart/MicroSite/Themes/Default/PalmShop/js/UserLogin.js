var openid = "";
$(document).ready(function () {
    $(".codeimg").attr("src", "ValidateCode.ashx?num=4");
    //$("#imgcodeimg").attr("src", "GetImgCode.ashx");
    $("#logphone").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='tel';this.style.color='#000'}");
    $("#logphone").attr("onblur", "if(!value) {value=defaultValue; this.type='tel';this.style.color='#999'}");
    $("#logphone").attr("maxlength", "13");
    $("#logphone").attr("onkeyup", "this.value=this.value.replace(/[^0-9]/g,'')");
    $("#logpwd").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='password';this.style.color='#000'}");
    $("#logpwd").attr("onblur", "if(!value) {value=defaultValue; this.type='password';this.style.color='#999'}");
    $("#logcode").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='tel';this.style.color='#000'}");
    $("#logcode").attr("onblur", "if(!value) {value=defaultValue; this.type='tel';this.style.color='#999'}");
    //$("#logcode").attr("onkeyup", "this.value=this.value.replace(/[^A-Za-z0-9]/g,'')");
    $("#logcode").attr("onkeyup", "this.value=this.value.replace(/[^0-9]/g,'')");
    $("#logcode").attr("maxlength", "4");

    $("#inputphone").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='tel';this.style.color='#000'}");
    $("#inputphone").attr("onblur", "if(!value) {value=defaultValue; this.type='tel';this.style.color='#999'}");
    $("#inputphone").attr("maxlength", "13");
    $("#inputphone").attr("onkeyup", "this.value=this.value.replace(/[^0-9]/g,'')");
    $("#inputpwd").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='password';this.style.color='#000'}");
    $("#inputpwd").attr("onblur", "if(!value) {value=defaultValue; this.type='password';this.style.color='#999'}");
    $("#inputpwdagain").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='password';this.style.color='#000'}");
    $("#inputpwdagain").attr("onblur", "if(!value) {value=defaultValue; this.type='password';this.style.color='#999'}");
    $("#inputvalidcode").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='text';this.style.color='#000'}");
    $("#inputvalidcode").attr("onblur", "if(!value) {value=defaultValue; this.type='tel';this.style.color='#999'}");
    //$("#inputvalidcode").attr("onkeyup", "this.value=this.value.replace(/[^A-Za-z0-9]/g,'')");
    $("#inputvalidcode").attr("onkeyup", "this.value=this.value.replace(/[^0-9]/g,'')");
    $("#inputvalidcode").attr("maxlength", "6");

    //$("#imgcodvalide").attr("onkeyup", "this.value=this.value.replace(/[^A-Za-z0-9]/g,'')");
    //$("#imgcodvalide").attr("maxlength", "6");

    openid = $("div[data-openid]").attr("data-openid");
    if (openid == null || openid == "" || openid == "undefined")
    {
        openid = "";
    }
    var action = Request("action");
    if (action != null && action != "") {
        action = action.toLocaleLowerCase();
        if (action == "login") {
            $("#pagetitle").html("登录");
            $(".loginbox").addClass("loginbox on");
            $(".regbox").addClass("regbox off");
        } else {
            $("#pagetitle").html("注册");
            $(".loginbox").addClass("loginbox off");
            $(".regbox").addClass("regbox on");
        }
    } else {
        $("#pagetitle").html("登录");
        $(".loginbox").addClass("loginbox on");
        $(".regbox").addClass("regbox off");
    }
})
$('.codeimg').click(function () {
    $(".codeimg").attr("src", "ValidateCode.ashx?num=4&t=" + (new Date()).valueOf());
});
$("#btnLog").click(function () {
    var code = getCookie("validate");
    if (code != null && code != "") {
        code = code.toLowerCase(); delCookie("validate");
    } else {
        $.dialog({
            time: 1, fixed: true, icon: 'error', content: '操作失败，请核对后再操作！'
        });
        return;
    }
    var logphone = $("#logphone").val();
    var logpwd = $("#logpwd").val();
    var logcode = $("#logcode").val().toLowerCase();
    if (logphone == null || logphone == "" || logphone == "手机号码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入手机号码！' });
        $("#logphone").val(''); ("#logphone")[0].focus(); return;
    } else {
        if (checkTel(logphone) == false) {
            $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的电话！' });
            $("#logphone").val(''); $("#logphone")[0].focus(); return;
        }
    }
    if (logpwd == null || logpwd == "" || logpwd == "登录密码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入账户密码！' });
        $("#logpwd").val(''); ("#logpwd")[0].focus(); return;
    }
    if (logcode == null || logcode == "" || logcode == "验证码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入验证码！' });
        $("#logcode").val(''); ("#logcode")[0].focus(); return;
    }
    if (logcode != code) {
        $(".codeimg").attr("src", "ValidateCode.ashx?num=4&t=" + (new Date()).valueOf());
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的验证码！' });
        $("#logcode").val('');
        // ("#logcode")[0].focus();
        return;
    }
    $.getJSON("UserLogin.aspx", {
        "action": "userlogin", "logphone": logphone, "logpwd": logpwd
    }, function (json) {
        if (json.error == true) {
            $.dialog({ time: 1, fixed: true, icon: 'error', content: json.msg }); return;
        }
        if (json.success == true) {
            var pageurl = getCookie("pageurl");
            delCookie("pageurl");
            if (pageurl != null && pageurl != "") {
                if (pageurl.toLowerCase().indexOf("applyshop") > 0) {
                    location.href = "Index.aspx?openid=" + openid;
                }
                if (pageurl.indexOf("?") > 0) {
                    pageurl = pageurl + "&openid=" + openid;
                } else {
                    pageurl = pageurl + "?openid=" + openid;
                }
                location.href = pageurl;
            } else {
                location.href = "Index.aspx?openid=" + openid;
            }
        }
    })
})
$("#btnReg").click(function () {
    var inputphone = $("#inputphone").val();
    var logpwd = $("#inputpwd").val();
    var logpwdagain = $("#inputpwdagain").val();
    var logcode = $("#inputvalidcode").val().toLowerCase();
    var code = getCookie("msgcode");
    if (inputphone == null || inputphone == "" || inputphone == "手机号码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入手机号码！' });
        $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
    } else {
        if (checkTel(inputphone) == false) {
            $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的电话！' });
            $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
        }
    }
    if (logpwd == null || logpwd == "" || logpwd == "注册密码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入账户密码！' });
        $("#inputpwd").val(''); ("#inputpwd")[0].focus(); return;
    }
    if (logpwdagain == null || logpwdagain == "" || logpwdagain == "重复密码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请再次输入账户密码！' });
        $("#inputpwdagain").val(''); ("#inputpwdagain")[0].focus(); return;
    }
    if (logpwd != logpwdagain) {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '两次密码不正确！' });
        $("#inputpwdagain").val(''); ("#inputpwdagain")[0].focus(); return;
    }

    //var ckcode = getCookie("imgcode");
    //var imgcode = $("#imgcodvalide").val();
    //if (ckcode != null && ckcode != "") {
    //    ckcode = ckcode.toLowerCase(); delCookie("imgcode");
    //} else {
    //    $.dialog({ time: 1, fixed: true, icon: 'error', content: '操作失败，请核对后再操作！' });
    //    return false;
    //}
    //if (imgcode != null && imgcode != "" && imgcode != "") {
    //    $.dialog({ time: 1, fixed: true, icon: 'error', content: '操作失败，请输入验证码！' });
    //    $("#imgcodvalide").val(''); ("#imgcodvalide")[0].focus(); return false;
    //}
    //if (imgcode != ckcode) {
    //    $(".codeimg").attr("src", "ValidateCode.ashx?t=" + (new Date()).valueOf());
    //    $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的验证码！' });
    //    $("#imgcodvalide").val(''); ("#imgcodvalide")[0].focus(); return false;
    //}

    if (logcode == null || logcode == "") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入短信验证码！' });
        $("#inputvalidcode").val(''); ("#inputvalidcode")[0].focus(); return;
    }
    if (code != null && code != "") {
        code = code.toLowerCase();
    } else {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '验证码已过期，请重新获取后再操作！' });
        $("#inputvalidcode").val(''); ("#inputvalidcode")[0].focus();
        return;
    }
    if (logcode != code) {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的短信验证码！' });
        $("#inputvalidcode").val(''); ("#inputvalidcode")[0].focus(); return;
    }
    $.getJSON("UserLogin.aspx", {
        "action": "save", "inputphone": inputphone, "logpwd": logpwd
    }, function (json) {
        if (json.error == true) {
            $.dialog({ time: 2, fixed: true, icon: 'error', content: json.msg }); return;
        }
        if (json.success == true) {
            delCookie("msgcode");
            $.dialog({
                time: 3, fixed: true, icon: 'succeed', content: '恭喜！您已注册成功',
                close: function () { location.href = "UserLogin.aspx"; }
            });
        }
    })
})
$("#regmsgbtn").click(function () {
    var thi = this;
    var reguserphone = $("#inputphone").val();
    if (reguserphone == null || reguserphone == "" || reguserphone == "手机号码") {
        $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入手机号码！' });
        $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
    } else {
        if (checkTel(reguserphone) == false) {
            $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的电话！' });
            $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
        }
    }

    $.getJSON("UserLogin.aspx", {
        "action": "wxphone", "inputphone": reguserphone
    }, function (json) {
        if (json.error == true) {
            $.dialog({ time: 3, fixed: true, icon: 'error', content: json.msg });
            $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
        }
        if (json.success == true) {
            //var ckcode = getCookie("imgcode");
            //var imgcode = $("#imgcodvalide").val();
            //if (ckcode != null && ckcode != "") {
            //    ckcode = ckcode.toLowerCase(); delCookie("imgcode");
            //} else {
            //    $.dialog({ time: 1, fixed: true, icon: 'error', content: '操作失败，请核对后再操作！' });
            //    return false;
            //}
            //if (imgcode == null || imgcode == "" || imgcode == "") {
            //    $.dialog({ time: 1, fixed: true, icon: 'error', content: '操作失败，请输入验证码！' });
            //    $("#imgcodvalide").val(''); ("#imgcodvalide")[0].focus(); return false;
            //}
            //if (imgcode != ckcode) {
            //    $(".codeimg").attr("src", "ValidateCode.ashx?t=" + (new Date()).valueOf());
            //    $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的验证码！' });
            //    $("#imgcodvalide").val(''); ("#imgcodvalide")[0].focus(); return false;
            //}

            var code = getCookie("msgcode");
            $.getJSON("UserLogin.aspx", {
                "action": "getmsg", "inputphone": reguserphone, "imgcode": code
            }, function (json) {
                if (json.error == true) {
                    $.dialog({ time: 3, fixed: true, icon: 'error', content: json.msg }); return false;
                }
                if (json.success == true) {
                    var step = 120;
                    var msg = "";
                    var _res = setInterval(function () {
                        msg = '重新发送' + step;
                        $(thi).attr("disabled", true);
                        $(thi).attr("title", msg);
                        $(thi).html(msg);
                        if (step <= 0) {
                            $(thi).removeAttr("disabled");
                            msg = "获取手机验证";
                            $(thi).attr("title", msg);
                            $(thi).html(msg);
                            clearInterval(_res);
                        }
                        step -= 1;
                    }, 1000);
                }
            })
        }
    })


})
$("#inputphone").mouseleave(function () {
    setmsgcode();
}).blur(function () {
    setmsgcode();
})
function setmsgcode() {
    var reguserphone = $("#inputphone").val();
    if (reguserphone != null && reguserphone != "" && reguserphone != "手机号码") {
        if (checkTel(reguserphone) == false) {
            $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的电话！' });
            $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
        }
    }
    $.getJSON("UserLogin.aspx", {
        "action": "validatephone", "inputphone": reguserphone
    }, function (json) {
        if (json.error == true) {
            $.dialog({ time: 3, fixed: true, icon: 'error', content: json.msg });
            $("#inputphone").val(''); $("#inputphone")[0].focus(); return;
        }
        if (json.success == true) {
            $(".codeimg").attr("src", "ValidateCode.ashx?num=4&codname=msgcode&minute=5");
        }
    })
}
//读取cookies 
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}
//删除cookies 
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}
function checkTel(tel) {
    var mobile = /^1[3|5|8]\d{9}$/, phone = /^0\d{2,3}-?\d{7,8}$/;
    return mobile.test(tel) || phone.test(tel);
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