var Step3_Request = {
    GetReleaseSettings: function (ecid) {
        var template_id = Request.GetQuery('template_id');
        var tag = Request.GetQuery('tag');
        var result = Request.Sync('/CMSAdmin/TemplateManage/GetReleaseSettings.ashx?template_id=' + template_id + '&tag=' + tag, { 'ecid': ecid });

        //var doc = $($('#iframe1')[0].contentWindow.document);
        var iframe_weixin_show = false;
        var iframe_mp_show = false;
        var iframe_sms_show = false;

        //$(".binds div:lt(4)").hide();
        if (result.code == 1) {
            //五个平台赋值
            $.each(['weixin', 'mp', 'iphone', 'iphone_ec', 'android', 'sms'], function () {
                $('.' + this)[result.data[this] && result.data.rights[this] ? 'addClass' : 'removeClass']('active');
                if (this == 'weixin' && result.data[this] && result.data.rights[this]) {
                    //$(".binds div:lt(3)").show();
                    $(".binds div:lt(2)").show();
                    iframe_weixin_show = true;
                    //doc.find('.platforms a:eq(0)').show();
                }
                if (this == 'iphone' && result.data[this] && result.data.rights[this]) {
                    $(".binds div:eq(3)").show();
                }
                if (this == 'iphone_ec' && result.data[this] && result.data.rights[this]) {
                    $('.iphone')[result.data[this] && result.data.rights[this] ? 'addClass' : 'removeClass']('active');
                }
                if (this == 'mp' && result.data[this] && result.data.rights[this]) {
                    iframe_mp_show = true;
                    //doc.find('.platforms a:eq(1)').show();
                }
                if (this == 'sms' && result.data[this] && result.data.rights[this]) {
                    iframe_sms_show = true;
                    //doc.find('.platforms a:eq(2)').show();
                }
                //document['getElementById']();
                //x = {}
                //x['?/5/6'] = 1 
                //$(".binds div:eq(1) *")
            });
            /*
            result.data.weixin && result.data.rights.weixin ? $(".weixin").addClass("active") : $(".weixin").removeClass("active");
            result.data.mp && result.data.rights.mp ? $(".mp").addClass("active") : $(".mp").removeClass("active");
            result.data.iphone && result.data.rights.iphone ? $(".iphone").addClass("active") : $(".iphone").removeClass("active");
            result.data.android && result.data.rights.android ? $(".android").addClass("active") : $(".android").removeClass("active");
            result.data.sms && result.data.rights.sms ? $(".sms").addClass("active") : $(".sms").removeClass("active");
            */

            $(".weixin").click(function () {
                if (result.data.weixin) {
                    $(this).addClass("active");
                    $(".binds div:lt(2)").show();
                    //if (this.attr("class").indexOf("active") > 0) {
                    //if (this.hasClass("active")) {
                    //                    if ($(this).hasClass("active")) {
                    //                        $(this).removeClass("active");
                    //                        //$(".binds div:lt(3)").hide();
                    //                        $(".binds div:lt(2)").hide();
                    //                    } else {
                    //                        $(this).addClass("active");
                    //                        //$(".binds div:lt(3)").show();
                    //                        $(".binds div:lt(2)").show();
                    //                    }
                } else {
                    popAlert('暂未生成，无法发布');
                }
                //doc.find('.platforms a:eq(0)').toggle();
            });

            //            //去掉微信 权限设置
            //            if (result.data.rights.weixin) {
            //   
            //            } else {
            //                $(".weixin").click(function () { popAlert('暂未购买此平台，如需购买请联系管理员'); });
            //            };
            //            result.data.rights.mp ? $(".mp").click(function () {
            //                if ($(this).hasClass("active")) {
            //                    $(this).removeClass("active");
            //                } else {
            //                    $(this).addClass("active");
            //                }
            //                //doc.find('.platforms a:eq(1)').toggle();
            //            }) : $(".mp").click(function () { popAlert('暂未购买此平台，如需购买请联系管理员'); });




            (result.data.rights.iphone || result.data.rights.iphone_ec) ? $(".iphone").click(function () {
                if (result.data.iphone || result.data.iphone_ec) {
                    if ($(this).hasClass("active")) {
                        $(this).removeClass("active");
                        $(".binds div:eq(3)").hide();
                    } else {
                        $(this).addClass("active");
                        //只生成企业版未生成正式版 不显示appstore参数填写
                        if (result.data.iphone_ec && !result.data.iphone) {
                        } else {
                            $(".binds div:eq(3)").show();
                        }
                    }
                } else {
                    popAlert('暂未生成，无法发布');
                }
            }) : $(".iphone").click(function () { popAlert('暂未购买此平台，如需购买请联系管理员'); });
            result.data.rights.android ? $(".android").click(function () {
                if (result.data.android) {
                    if ($(this).hasClass("active")) {
                        $(this).removeClass("active");
                    } else {
                        $(this).addClass("active");
                    }
                } else {
                    popAlert('暂未生成，无法发布');
                }
            }) : $(".android").click(function () { popAlert('暂未购买此平台，如需购买请联系管理员'); });
            //            result.data.rights.sms ? $(".sms").click(function () {
            //                if ($(this).hasClass("active")) {
            //                    $(this).removeClass("active");
            //                } else {
            //                    $(this).addClass("active");
            //                }
            //                //doc.find('.platforms a:eq(2)').toggle();
            //            }) : $(".sms").click(function () { popAlert('暂未购买此平台，如需购买请联系管理员'); });


            //微信配置赋值
            if (result.data.weixin1) {
                //$(".binds div:eq(0) input:eq(0)").attr("checked", true);
                $(".binds div:eq(0) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green");
            } else {

            }
            if (result.data.weixin2) {
                $("#item2").prop("checked", true);
                //$(".binds div:eq(1) :text:lt(2)").show();
                $(".binds div:eq(1) a").show();
                $(".binds div:eq(1) b").show();
                $(".binds div:eq(1) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green");
                $("#item2").click(function () {
                    if ($("#item2").is(":checked") == true) {
                        $(".binds div:eq(1) a").show();
                        $(".binds div:eq(1) b").show();
                    } else {
                        popConfirm('确定要取消绑定微信自定义菜单吗？', function (res) {
                            if (res == true) {
                                if (self.SetWeiXinMenuBind(ecid, '', '') > 0) {
                                    $("#item2").prop("checked", false);
                                    $(".binds div:eq(1) b").text("已绑定").css("background-image", "url('../images/danger.png')").css("color", "#6f6f6f");
                                    $(".binds div:eq(1) a").hide();
                                    $(".binds div:eq(1) b").hide();
                                    $("#input_appid").val("");
                                    $("#input_appsecret").val("");
                                    popAlert('取消绑定成功！');
                                } else {
                                    popAlert('取消绑定失败！');
                                }
                            } else {
                                $("#item2").prop("checked", true);
                            }
                        });
                    }
                });
            } else {
                $("#item2").prop("checked", false);
                $(".binds div:eq(1) a").hide();
                $(".binds div:eq(1) b").hide();
                $("#item2").click(function () {
                    if ($("#item2").is(":checked") == true) {
                        if ($(".binds div:eq(0) b").text() == "已绑定") {
                            $(".binds div:eq(1) a").show();
                            $(".binds div:eq(1) b").show();
                        } else {
                            popAlert('请先绑定微信公众账户！');
                            $("#item2").prop("checked", false);
                        }
                    } else {
                        if ($(".binds div:eq(1) b").text() == "已绑定") {
                            //if (confirm('确定要取消绑定微信自定义菜单吗?')) {
                            popConfirm('确定要取消绑定微信自定义菜单吗？', function (res) {
                                if (res == true) {
                                    if (self.SetWeiXinMenuBind(ecid, '', '') > 0) {
                                        $("#item2").prop("checked", false);
                                        $(".binds div:eq(1) b").text("已绑定").css("background-image", "url('../images/danger.png')").css("color", "#6f6f6f");
                                        $(".binds div:eq(1) a").hide();
                                        $(".binds div:eq(1) b").hide();
                                        $("#input_appid").val("");
                                        $("#input_appsecret").val("");
                                        popAlert('取消绑定成功！');
                                    } else {
                                        popAlert('取消绑定失败！');
                                    }
                                } else {
                                    $("#item2").prop("checked", true);
                                }
                            });
                        } else {
                            $(".binds div:eq(1) a").hide();
                            $(".binds div:eq(1) b").hide();
                        }
                    }
                });
            }
            if (result.data.weixin3) {
                $(".binds div:eq(2) input:eq(0)").attr("checked", true);
                //$(".binds div:eq(2) :text:lt(3)").show();
                $(".binds div:eq(2) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green");
            } else {
                $(".binds div:eq(2) input:eq(0)").attr("checked", false);
            }

            result.data.apiurl && $("#input_apiurl").val(result.data.apiurl);
            result.data.token && $("#input_token").val(result.data.token);

            result.data.appid && $("#input_appid").val(result.data.appid);
            result.data.appsecret && $("#input_appsecret").val(result.data.appsecret);

            //result.data.paysignkey && $("#input_paysignkey").val(result.data.paysignkey);
            //result.data.partnerid && $("#input_partnerid").val(result.data.partnerid);
            //result.data.partnerkey && $("#input_partnerkey").val(result.data.partnerkey);

            //result.data.clientcredentials ? $(".binds div:eq(3) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green") : "";
            //result.data.logourl && doc.find('header>h1>i').css('background-image', 'url(' + result.data.logourl + ')');
            //result.data.clientname ? doc.find('header>h1>span').text(result.data.clientname) : "";
            //result.data.clientdescription ? doc.find('.download>p').text(result.data.clientdescription) : "";
            //result.data.appdesc ? doc.find('.detail').text(result.data.appdesc) : "";

            result.data.clientdescription ? $("div.infoItem:nth-child(1) > input:nth-child(2)").val(result.data.clientdescription) : "";
            result.data.appdesc ? $('.infoItem.clearfix textarea').val(result.data.appdesc) : "";

            //$("#item2").click(function () {
            //$(".binds div:eq(1) :text:lt(2)").toggle();
            //});
            //$("#item3").click(function () {
            //$(".binds div:eq(2) :text:lt(3)").toggle();
            //});

            //$("div.infoItem:nth-child(1) > input:nth-child(2)").blur(function () {
            //    doc.find('.download>p').text(this.value);
            //});
            //$('.infoItem.clearfix textarea').blur(function () {
            //    doc.find('.detail').text(this.value);
            //});

            var self = this;
            //绑定微信公众号
            $("#bind1").click(function () {
                //self.SetWeiXinBind(ecid, result.data.token);
                //$(".p-inner > h1:nth-child(1) > span:nth-child(2)").css('font-size', '11px');
                //$(".p-inner > h1:nth-child(1) > span:nth-child(2)").text("请将下面的URL和Token填入您的微信公众账户进行绑定");
                $(".p-inner > h1:nth-child(1) > span:nth-child(2)").text("提示");
                $(".p-inner").css("height", "260px");
                $('#pop-tjgn').show();
                $('#div_w1').show();
                $('#div_w2').hide();
                $('#div_w3').hide();
                return false;
            });
            $('#pop-tjgn b').click(function () {
                $('#pop-tjgn').hide();
            });
            $('#p_bind1').click(function () {
                if (self.SetWeiXinBind(ecid, result.data.token) > 0) {
                    $('#pop-tjgn').hide();
                }
            });

            //绑定自定义菜单
            $("#bind2").click(function () {
                //self.SetWeiXinMenuBind(ecid);
                //$(".p-inner > h1:nth-child(1) > span:nth-child(2)").text("绑定微信自定义菜单");
                $(".p-inner > h1:nth-child(1) > span:nth-child(2)").text("提示");
                $(".p-inner").css("height", "270px");
                $('#pop-tjgn').show();
                $('#div_w1').hide();
                $('#div_w2').show();
                $('#div_w3').hide();
                return false;
            });
            $("#p_bind2").click(function () {
                var appid = $("#input_appid").val();
                var appsecret = $("#input_appsecret").val();
                if (!appid) { popAlert('请填写 appid'); return; }
                if (!appsecret) { popAlert('请填写 appsecret'); return; }
                //if (appid.length == 0 && appsecret.length == 0) {
                //$('#pop-tjgn').hide();
                //return;
                //}
                if (appid.length == 18 && appsecret.length == 32) {
                    if (self.SetWeiXinMenuBind(ecid, appid, appsecret) > 0) {
                        $('#pop-tjgn').hide();
                    }
                } else {
                    popAlert('请输入正确的appid和appsecret');
                }
            });
            //绑定微信支付
            $("#bind3").click(function () {
                //self.SetWeiXinPayBind(ecid);
                $(".p-inner > h1:nth-child(1) > span:nth-child(2)").text("绑定微信支付");
                $('#pop-tjgn').show();
                $('#div_w1').hide();
                $('#div_w2').hide();
                $('#div_w3').show();
                return false;
            });
            $("#p_bind3").click(function () {
                if (self.SetWeiXinPayBind(ecid) > 0) {
                    $('#pop-tjgn').hide();
                }
            });
            //二维码
            //var logourl = result.data.logourl && ('&logo=' + result.data.logourl);
            var erweima = 'http://qr.liantu.com/api.php?&bg=ffffff&fg=000000&w=128&el=h&text=http://ci.f3.cn/d/' + ecid;
            $('#erweima').attr('src', erweima);
            $('#erweima_xiazai').attr('href', erweima);

            $('#iframe1').one('load', function (e) {
                var doc = $($('#iframe1')[0].contentWindow.document);

                iframe_weixin_show && doc.find('.platforms a:eq(0)').show();
                //iframe_mp_show && doc.find('.platforms a:eq(1)').show();
                //iframe_sms_show && doc.find('.platforms a:eq(2)').show();

                result.data.logourl && doc.find('header>h1>i').css('background-image', 'url(' + result.data.logourl + ')');
                result.data.clientname ? doc.find('header>h1>span').text(result.data.clientname) : "";
                result.data.clientdescription ? doc.find('.download>p').text(result.data.clientdescription) : "";
                result.data.appdesc ? doc.find('.detail').text(result.data.appdesc) : "";

                result.data.rights.weixin && $(".weixin").click(function () {
                    doc.find('.platforms a:eq(0)').toggle();
                });
                result.data.rights.mp && $(".mp").click(function () {
                    //doc.find('.platforms a:eq(1)').toggle();
                });
                result.data.rights.sms && $(".sms").click(function () {
                    //doc.find('.platforms a:eq(2)').toggle();
                });

                $("div.infoItem:nth-child(1) > input:nth-child(2)").blur(function () {
                    doc.find('.download>p').text(this.value);
                });
                $('.infoItem.clearfix textarea').blur(function () {
                    doc.find('.detail').text(this.value);
                });
            });

        } else {
            popAlert(result.notice);
        }
    },
    SetReleaseSettings: function (ecid) {
        var template_id = Request.GetQuery('template_id');
        var tag = Request.GetQuery('tag');
        if (!template_id) {
            popAlert("参数不正确");
        }
        if (!tag) {
            popAlert("参数不正确");
        }
        var weixin = $(".weixin").hasClass("active");
        var mp = $(".mp").hasClass("active");
        var iphone = $(".iphone").hasClass("active");
        var android = $(".android").hasClass("active");
        var sms = $(".sms").hasClass("active");

        var weixinmenu = $("#item2").is(":checked");

        if (!weixin && !iphone && !android) {
            popAlert("请选择要发布的平台");
            return;
        }

        var appstoreurl = $("#appstoreurl").val();
        if (iphone) {
            //            if (appstoreurl && appstoreurl.length > 0) {

            //            } else {
            //                popAlert('请填写appstore地址');
            //                return;
            //            }
        }
        //var clientname = $("div.infoItem:nth-child(1) > input:nth-child(2)").val();
        var clientdescription = $("div.infoItem:nth-child(1) > input:nth-child(2)").val();
        if (!clientdescription) {
            clientdescription = "最专业的营销利器";
        }
        var appdesc = $('.infoItem.clearfix textarea').val();
        if (!appdesc) {
            appdesc = '';
        }
        var result = Request.Sync('/CMSAdmin/TemplateManage/SetReleaseSettings.ashx?template_id=' + template_id + '&tag=' + tag, { 'ecid': ecid, 'weixin': weixin, 'mp': mp, 'iphone': iphone, 'android': android, 'sms': sms, 'appstoreurl': appstoreurl, 'clientdescription': clientdescription, 'appdesc': appdesc, 'weixinmenu': weixinmenu });
        //console.log(result);
        if (result.code == 1) {

            popAlert("发布成功", function (res) {
                if (res) {
                    var agent_ecid = Request.GetQuery('agent_ecid');
                    if (agent_ecid != "" && agent_ecid.length == 6)
                    { } else {
                        var type = Request.GetQuery('type');
                        if (type != "") {
                            if (type == "app") {
                                window.parent.location.href = 'http://ec.f3.cn/BackOffice/aspx/sysFrame/MyMain.aspx?product=88';
                            } else if (type == "weixin") {
                                window.parent.location.href = 'http://ec.f3.cn/BackOffice/aspx/sysFrame/MyMain.aspx?product=66';
                            }
                        }
                        else {
                            window.parent.location.href = 'http://ec.f3.cn/BackOffice/aspx/sysFrame/F3ECMain.aspx';
                        }
                    }
                    //window.parent.location = 'http://bo.f3.cn/BackOffice/aspx/sysFrame/F3ECMain.aspx';
                    //window.parent.location.href = 'http://bo.f3.cn/BackOffice/ECWeb/AccountCenter/MyAccount.aspx'; ;
                    return false;
                }
            });
            //            if (window.parent != window) {
            //                window.parent.location = window.parent.location;
            //            }
        } else {
            popAlert(result.notice);
        }
    },
    SetWeiXinBind: function (ecid, token) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/SetWeiXinBind.ashx', { 'ecid': ecid, 'token': token });
        //console.log(result);
        if (result.code == 1) {
            $(".binds div:eq(0) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green");
            popAlert('绑定成功'); return 1;
        } else {
            popAlert(result.notice); return 2;
        }
    },
    SetWeiXinMenuBind: function (ecid, appid, appsecret) {
        //if (!appid) { popAlert('请填写 appid'); return -1; }
        //if (!appsecret) { popAlert('请填写 appsecret'); return -1; }
        var result = Request.Sync('/CMSAdmin/TemplateManage/SetWeiXinMenuBind.ashx', { 'ecid': ecid, 'appid': appid, 'appsecret': appsecret });
        //console.log(result);
        if (result.code == 1) {
            $(".binds div:eq(1) input:eq(0)").attr("checked", true);
            $(".binds div:eq(1) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green");
            popAlert('绑定成功');
            return 1;
        } else {
            popAlert(result.notice);
            return 2;
        }
    },
    SetWeiXinPayBind: function (ecid) {
        var paysignkey = $("#div_w3 > div:nth-child(4) > div:nth-child(3) > table:nth-child(1) > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(2) > input:nth-child(1)").val();
        var partnerid = $("#div_w3 > div:nth-child(4) > div:nth-child(3) > table:nth-child(1) > tbody:nth-child(1) > tr:nth-child(2) > td:nth-child(2) > input:nth-child(1)").val();
        var partnerkey = $("#div_w3 > div:nth-child(4) > div:nth-child(3) > table:nth-child(1) > tbody:nth-child(1) > tr:nth-child(3) > td:nth-child(2) > input:nth-child(1)").val();

        if (!paysignkey) { popAlert('请填写 paysignkey'); return -1; }
        if (!partnerid) { popAlert('请填写 partnerid'); return -1; }
        if (!partnerkey) { popAlert('请填写 partnerkey'); return -1; }
        var result = Request.Sync('/ClientSetting/SetWeiXinPayBind', { 'ecid': ecid, 'paysignkey': paysignkey, 'partnerid': partnerid, 'partnerkey': partnerkey });
        //console.log(result);
        if (result.code == 1) {
            $(".binds div:eq(2) input:eq(0)").attr("checked", true);
            $(".binds div:eq(2) b").text("已绑定").css("background-image", "url('../images/success.png')").css("color", "green");
            popAlert('绑定成功');
            return 1;
        } else {
            popAlert(result.notice);
            return 2;
        }
    }
}

