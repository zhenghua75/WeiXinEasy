var Global = {
    //去除元素获得焦点后的虚线边框
    noDashed: function () {
        $('a,:checkbox,:radio').focus(function () {
            $(this).blur();
        });
    },
    //监听A链接事件，跳转iframe
    iframeLocation: function (iframe, links) {
        $(links).click(function () {
            $(iframe).attr('src', this.href);
            return false;
        });
    },
    //TAB栏效果
    tabs: function (tNav, tCont, callback) {

        var tNav = $(tNav + ' > li');
        var tCont = $(tCont + ' > li');
        tNav.children('a').click(function () {

            if (!$(this).parent().hasClass('active')) {
                tNav.removeClass('active');
                $(this).parent().addClass('active');
                var i = tNav.index($(this).parent());
                tCont.removeClass('active');
                $(tCont[i]).addClass('active');
                //隐藏颜色选择
                var isShellD = /^shelld$/i.test(Request.GetQuery('shell_type'));
                var isShellE = /^shelle$/i.test(Request.GetQuery('shell_type'));
                var isShellF = /^shellf$/i.test(Request.GetQuery('shell_type'));
                var isShellB = /^shellb$/i.test(Request.GetQuery('shell_type'));
                var isShellC = /^shellc$/i.test(Request.GetQuery('shell_type'));
                var isShellG = /^shellg$/i.test(Request.GetQuery('shell_type'));
                var isShellR = /^shellr$/i.test(Request.GetQuery('shell_type'));
                var isShellP = /^shellp$/i.test(Request.GetQuery('shell_type'));
                var isShellS = /^shells$/i.test(Request.GetQuery('shell_type'));

                $('.wzys2').show(); //弹出框文章颜色
                $('.wzys3').show(); //弹出背景颜色
                $('.wzys').show(); //文字颜色

                if (i == 2) {
                    if (isShellD || isShellP || isShellS) {
                        $('.wzys').hide();
                        //$('.wzys2').hide();
                    } else if (isShellE) {
                        $('.wzys').hide();
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellF) {
                        $('.wzys').hide();
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellB) {
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellC) {
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellG) {
                        $('.wzys').hide();
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellR) {
                        $('.wzys').hide();
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                } else if (i == 3) {
                    if (isShellD) {

                    } else if (isShellE) {
                        $('.wzys3').hide();
                    }
                    else if (isShellF) {
                        $('.wzys3').hide();
                    }
                    else if (isShellB) {
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellC) {
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellG) {
                        $('.wzys3').hide();
                    }
                    else if (isShellR) {
                        $('.wzys').hide();
                        $('.wzys2').hide();
                        $('.wzys3').hide();
                    }
                    else if (isShellP) {
                        $('.wzys3').hide();
                    } else if (isShellS) {
                        $('.wzys3').hide();
                    }
                }
            }
            return false;
        });
    },
    //切换
    change: function (obj, callback) {
        $(document).on('click', obj, function () {
            $(obj).removeClass('active');
            $(this).addClass('active');
            if (callback) {
                callback.call(this);
            }
        });
    },
    //切换选中和正常状态
    toggleActive: function (obj) {
        $(obj).click(function () {
            $(this).parent().toggleClass('active');
        });
    },
    //input tips切换
    inputTips: function (elem) {
        $(elem).focus(function () {
            if ($(this).val() == $(this).attr('tips')) {
                $(this).val('');
            }
        });
    }
}

// 取字符串长度(中文、全角占2个长度)
String.prototype.getLength = function () {
    var len = 0;
    for (var i = 0; i < this.length; i++) {
        ++len && this.charCodeAt(i) > 0x7f && ++len;
    }
    return len;
}


//限制input输入的字符数
function CheckLength(input, length) {
    var value = input.value;
    var rs = value;

    var charLen = 0;
    for (var i = 0, c = 0; i < value.length; i++) {
        if (value.charCodeAt(i) > 0x7f) {
            if (c + 2 > length) {
                rs = value.substr(0, i);
                break;
            }
            c += 2;
        }
        else {
            if (c + 1 > length) {
                rs = value.substr(0, i);
                break;
            }
            c++;
        }
        charLen = c;
    }
    if (input.value != rs) {
        input.value = rs;
        //alert("已超过 " + length + " 个字符限制");
        //input.value = value.substr(0, charLen);
    }

}


//提示框
function popAlert(msg, callBack) {
    PopUpWindow("alert", msg, callBack);
}
//确认框
function popConfirm(msg, callBack) {
    PopUpWindow("confirm", msg, callBack);
}
// 弹出遮罩窗口(元素选择器, 显示文字, 回调函数);
function PopUpWindow(val, text, handler) {

    handler = handler || function (bool) { };

    var mark = true;

    if (val == "alert") {

        $(".PopUpCancel").hide();

    } else {

        $(".PopUpCancel").show();

        $('.PopUpCancel').one('click', function () {
            if (mark) {
                mark = false;
                $('#layer, #PopUpWindow').hide();
                handler(false);
                return false;
            }
        });
    }

    $('.PopUpOk').one('click', function () {
        if (mark) {
            mark = false;
            $('#layer, #PopUpWindow').hide();
            handler(true);
            return true;
        }
    });

    $('.PopUpWord').html(text);
    $('#layer, #PopUpWindow').show();
    var top = $(window).scrollTop();
    $(window).scrollTop(99999).scrollTop(top);
}