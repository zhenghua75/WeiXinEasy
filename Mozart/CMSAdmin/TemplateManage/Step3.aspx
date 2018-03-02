<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Step3.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.Step3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>绑定微信参数</title>
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/yypz.css" />
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Request.js"></script>
    <script src="scripts/global.js"></script>
    <script src="scripts/step3.js"></script>
    <script src="scripts/step3_request.js"></script>
    <script src="scripts/iColorPicker.js"></script>
    <script src="scripts/Helper.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="http://ec.f3.cn/PageGuider/guider.css" />
    <script type="text/javascript" src="http://ec.f3.cn/PageGuider/guider.ashx"></script>--%>
    <script>

        var ecid = Request.GetQuery('ecid');
        var template_id = Request.GetQuery('template_id');
        var tag = Request.GetQuery('tag');

        $(function () {
            //document.domain = 'f3.cn';

            $(document).on('input propertychange', 'div.infoItem:nth-child(1) > input:nth-child(2)', function () {
                CheckLength(this, 40); //20个汉字，40个数字和字母		
            });

            Global.noDashed();
            //Global.toggleActive('.icons > span > a');
            //页面初始化
            //取配置
            Step3_Request.GetReleaseSettings(ecid);
            $("#btn_release").click(function () {
                Step3_Request.SetReleaseSettings(ecid);
            });


            var toStep1Link = 'step1.aspx?ecid=' + ecid + '&template_id=' + template_id + '&force=true&type=' + Request.GetQuery('type') + '&agent_ecid=' + Request.GetQuery('agent_ecid');
            var toStep2Link = 'step2.aspx?ecid=' + ecid + '&tag=' + tag + '&template_id=' + template_id + '&force=true&type=' + Request.GetQuery('type') + '&agent_ecid=' + Request.GetQuery('agent_ecid');

            $('.t1>div>span').wrap('<a href="' + toStep1Link + '"></a>');
            $('.t2>div>span').wrap('<a href="' + toStep2Link + '"></a>');

            Helper('#weixinbind_question', '提示：须绑定微信公众账户，微信平台才能生效');
            Helper('#weixinmenu_question', '提示：未认证的订阅号无需绑定');
            Helper('#iphone_question', '提示：登录苹果网站（<a href="http://itunesconnect.apple.com/" target="_blank">http://itunesconnect.apple.com/</a>），获取审核通过的APP下载地址。');
            //            $('.t1>div').css("cursor", "pointer").click(function () {
            //                window.location.href = 'step1.aspx?ecid=' + ecid + '&force=true';
            //            });
            //            $('.t2>div').css("cursor", "pointer").click(function () {
            //                goto_step2();
            //            });

            showPlatform();
        });
        function goto_step2() {
            var type = Request.GetQuery('type');
            window.location.href = 'step2.aspx?ecid=' + ecid + '&tag=' + tag + '&template_id=' + template_id + '&force=true&type=' + type + '&agent_ecid=' + Request.GetQuery('agent_ecid');
        }

        //隐藏或显示平台
        function showPlatform() {
            var type = Request.GetQuery('type');

            if (type != '') {
                if (type == "app") {
                    $(".weixin").hide();
                } else if (type == "weixin") {
                    $(".weixin").addClass("active");
                    $(".binds div:lt(2)").show();
                    $(".iphone").hide();
                    $(".android").hide();
                }
            }
        }
    </script>
    <style>
        .p-main {
            text-align: center;
            height: 220px;
        }

            .p-main p {
                margin: 0;
                padding: 0 10px;
            }

            .p-main input[type=text] {
                width: 360px;
                height: 24px;
            }
    </style>
</head>
<body>
    <div id="layer">
    </div>
    <div id="PopUpWindow">
        <div class="PopUpWindow">
            <h1>提示<i></i></h1>
            <p class="PopUpWord"></p>
            <p class="PopUpBtns"><a class="PopUpOk">确定</a> <a class="PopUpCancel">取消</a> </p>
        </div>
    </div>
    <ol class="top clearfix">
        <li class="t1">
            <div>
                <i></i>
                <span>应用配置</span>
            </div>
        </li>
        <li class="t2">
            <div>
                <i></i>
                <span>生成测试</span>
            </div>
        </li>
        <li class="t3 active">
            <div>
                <i></i>
                <span>应用发布</span>
            </div>
        </li>
    </ol>
    <div class="content">
        <div class="left">
            <div class="ylq" style="overflow: hidden;">
                <iframe id="iframe1" frameborder="0" width="305" height="541" scrolling="no" src="step3_download.aspx"></iframe>
            </div>
        </div>
        <div class="right3">
            <h3 style="line-height: 35px; padding: 0; position: relative;">
                <a id="btn_Next" onclick="goto_step2();" href="#" class="u-Black pull-left">上一步</a> <span style="position: absolute; width: 300px; height: 30px; left: 50%; margin-left: -150px; text-align: center;">发布平台
                </span><a id="btn_release" class="u-btnGreen btn_right" href="#">发布</a></h3>
            <div class="icons">
                <span class="weixin"><a></a></span><span class="iphone"><a></a></span><span class="android"><a></a></span><span class="mp" style="display: none"><a></a></span><span class="sms" style="display: none"><a></a>
                </span>
            </div>
            <div class="binds">
                <div class="bindItem" style="display: none;">
                    <input id="item1" type="checkbox" disabled checked/>
                    <label for="item1">
                        绑定微信公众号</label>
                    <i id="weixinbind_question"></i>
                    <input type="text" style="display: none;"/>
                    <a class="u-btnBlue" href="" id="bind1">绑定</a> <b>未绑定</b>
                </div>
                <div class="bindItem" style="display: none;">
                    <input id="item2" type="checkbox"/>
                    <label for="item2">
                        绑定自定义菜单</label>
                    <i id="weixinmenu_question"></i>
                    <input type="text" style="display: none;">
                    <input type="text" style="display: none;">
                    <a class="u-btnBlue" href="" id="bind2" style="display: none;">绑定</a> <b style="display: none;">未绑定</b>
                </div>
                <div class="bindItem" style="display: none;">
                    <input id="item3" type="checkbox">
                    <label for="item3">
                        绑定微支付菜单</label>
                    <i></i>
                    <input type="text" style="display: none; width: 76px;"/>
                    <input type="text" style="display: none; width: 76px;"/>
                    <input type="text" style="display: none; width: 78px;"/>
                    <a class="u-btnBlue" href="" id="bind3">绑定</a> <b>未绑定</b>
                </div>
                <div class="bindItem" style="display: none;">
                    <input id="item4" name="input_iphone" type="radio" checked/>
                    <label for="item4" style="width: 112px;">
                        苹果正式版</label>
                    <!--<input id="item5" name="input_iphone" type="radio" disabled>
                    <label for="item5">
                        苹果企业版</label>-->
                    <i id="iphone_question"></i>
                    <input id="appstoreurl" type="text" placeholder="请在此输入appstore链接地址" style="width: 300px;" />
                    <b style="display: none;">未上传</b>
                </div>
            </div>
            <h3>推广配置</h3>
            <div class="info">
                <div class="infoItem clearfix">
                    <label>
                        推广语</label>
                    <input type="text" placeholder="最专业的营销利器" style="width: 510px;">
                </div>
                <div class="infoItem clearfix">
                    <label>
                        应用介绍</label>
                    <textarea></textarea>
                </div>
                <div class="infoItem clearfix" style="display: none;">
                    <label>
                        二维码</label>
                    <img id="erweima" src="">
                    <div>
                        <p>
                            <span style="margin-top: 5px; float: left;">前景色</span><input id="txtQjsColor" name="txtQjsColor" style="width: 70px; display: none;" type="text" value="#666666" class="iColorPicker"/>
                        </p>
                        <p>
                            <span style="margin-top: 5px; float: left;">背景色</span><input id="txtBjsColor" name="txtBjsColor" style="width: 70px; display: none;" type="text" value="#666666" class="iColorPicker"/>
                        </p>
                        <p><a href="#" id="erweima_xiazai" target="_blank">下载二维码图片</a></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 添加功能弹出DIV窗 -->
    <div id="pop-tjgn">
        <div class="p-outer">
            <div class="p-inner" style="width: 400px; height: 260px; background: #FFF; border-radius: 0px; position: inherit; margin: 0 auto; top: 120px; left: 0; border: 4px solid #666;">
                <h1 style="border-radius: 0px; background: none repeat scroll 0 0 #f3f3f3; color: #111;">
                    <b style="background: url(../Images/btn_close.png) no-repeat scroll 0 0 rgba(0, 0, 0, 0);"></b><span></span>
                </h1>
                <div class="p-main">
                    <div id="div_w1" style="display: none;">
                        <p style="text-align: left;">请将下面的URL和Token填入您的微信公众账户进行绑定</p>
                        <p style="text-align: right; margin: 0px 10px 0 0;"><a href="weixinbind.html" target="_blank">怎么绑定？</a></p>
                        <p style="text-align: left;">URL:<input type="text" id="input_apiurl" style="border: none; margin: 0 0 0 10px;" /></p>
                        <p style="text-align: left;">Token:<input type="text" id="input_token" style="border: none; margin: 0 0 0 10px;" /></p>
                        <p id="p_bind1" style="width: 360px; height: 32px; line-height: 32px; background: #000033; border: 1px solid #eee; text-align: center; margin: 0 auto; margin-top: 10px; color: #fff; cursor: pointer; float: none;">
                            我绑好了
                        </p>
                    </div>
                    <div id="div_w2" style="display: none;">
                        <p style="text-align: left;">请将微信网站上您的AppID和AppSecret填入下面的输入框中</p>
                        <p style="text-align: right; margin: 0px 10px 0 0;"><a href="weixinmenubind.html" target="_blank">怎么绑定？</a></p>
                        <p style="text-align: left;">AppID:</p>
                        <input type="text" id="input_appid" />
                        <p style="text-align: left;">AppSecret:</p>
                        <input type="text" id="input_appsecret" />
                        <p id="p_bind2" style="width: 300px; height: 32px; line-height: 32px; background: #000033; border: 1px solid #eee; text-align: center; margin: 0 auto; margin-top: 10px; color: #fff; cursor: pointer; float: none;">
                            我绑好了
                        </p>
                    </div>
                    <div id="div_w3" style="display: none;">
                        <p style="float: right; margin: 0px 10px 0 0;"><a href="">怎么绑定？</a></p>
                        <p>Token:</p>
                        <input type="text" /><p>ApiUrl:</p>
                        <input type="text" />
                        <p id="p_bind3" style="width: 360px; height: 32px; line-height: 32px; background: #000033; border: 1px solid #eee; text-align: center; margin: 0 auto; margin-top: 10px; color: #fff; cursor: pointer; float: none;">
                            我绑好了
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--<div id="modal1" class="modal">
        <h1 id="bind_title">
        </h1>
        <a class="closeBtn btn" href="#">
        </a>
        <div id="div_bind1" style="display: none; padding-left: 20px;">
            <p>Token</p>
            <input type="text" /><p>ApiUrl</p>
            <input type="text" />
            <div style="float: right;">
                <a href="" class="u-btnGreen btn_right" style="width: 80px; margin: 20px 10px 0 10px;">
                    怎么绑定</a>
                <a id="a_bind1" href="" class="u-btnGreen btn_right" style="width: 80px; margin: 20px 0 0 0;">
                    我绑好了</a></div>
        </div>
        <div id="div_bind2" style="display: none;">
        </div>
        <div id="div_bind3" style="display: none;">
        </div>
    </div>-->
</body>
</html>
