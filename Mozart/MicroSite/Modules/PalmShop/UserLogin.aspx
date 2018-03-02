<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="Mozart.PalmShop.ShopCode.UserLogin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户登录/注册</title>
    <meta name="viewport" content="width=320" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <script src="js/jquery-2.1.0.min.js"></script>
    <link href="css/ShopMain.css" rel="stylesheet" />
    <script src="artDialog/jquery.artDialog.js?skin=dialogred" type="text/javascript" charset="gbk"></script>
    <style>
        #regmsgbtn {background-color:#fff;border: 1px solid #babac0;}
    </style>
</head>
<body style="background: #ebeced;">
    <div class="twoHead">
    <ul>
        <li>
            <a href="javascript:history.go(-1);" id="head_back" class="c-btn c-btn-aw">
            <span class="goback">返回</span></a></li>
        <li id="pagetitle"></li> 
        <%--<li>
            <a href="index.aspx" class="c-btn-home">
            <img src="images/icon_index_08.png" style="width: 25px;" alt="主页" />
            </a>
        </li>--%>
    </ul>
</div>
<div class="clear">
</div><div class="Content">
        <div class="mainCon">
            <div class="loginCon">
                <div class="loginbox">
                    <dl>
                        <dd>
                            <span>电话号码：</span><input type="tel" id="logphone" value="手机号码"/>
                        </dd>
                        <dd>
                            <span>登录密码：</span><input type="password" value="" id="logpwd"/>
                        </dd>
                        <dd>
                            <span>&nbsp;&nbsp;&nbsp;&nbsp;验证码：</span>
                            <input type="tel" value="验证码" id="logcode"/>
                            <img title="看不清楚?点我换一张" class="codeimg">
                        </dd>
                       </dl>
                    <div class="clear">
                    </div>
                    <div class="button-Red foo_bt" id="btnLog">登&nbsp;&nbsp;&nbsp;&nbsp;录</div>
                    <div class="regbottom">
                        <a href="?action=reg" title="新用户注册">新用户注册</a> 
                        <a href="EditeLogPwd.aspx" title="修改密码">修改密码</a>
                    </div>
                </div>
                <div class="regbox">
                    <dl>
                        <dd>
                            <span>电话号码：</span><input type="tel" id="inputphone" value="手机号码"/>
                        </dd>
                        <dd>
                            <span>注册密码：</span><input type="password" value="" id="inputpwd"/>
                        </dd>
                        <dd>
                            <span>重复密码：</span><input type="password" value="" id="inputpwdagain"/>
                        </dd>
                        <%--<dd>
                            <span>&nbsp;&nbsp;&nbsp;&nbsp;验证码：</span>
                            <input type="text" value="验证码" id="imgcodvalide"/>
                            <img title="看不清楚?点我换一张" id="imgcodeimg" class="codeimg">
                        </dd>--%>
                        <dd>
                            <span>短信验证：</span>
                            <input type="text" value="短信验证码" id="inputvalidcode"/>
                            <button id="regmsgbtn" title="获取手机验证码">获取短信验证码</button>
                        </dd>
                    </dl>
                    <div class="clear">
                    </div>
                    <div class="button-Red foo_bt" id="btnReg">注&nbsp;&nbsp;&nbsp;&nbsp;册</div>
                    <div class="regbottom">
                        <a href="?action=login" title="已注册用户登录">已注册用户登录</a> 
                        <a href="EditeLogPwd.aspx" title="修改密码">修改密码</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="js/UserLogin.js"></script>
</body>
</html>