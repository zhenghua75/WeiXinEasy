<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="Mozart.Distributor.UserLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="script/jquery-1.8.0.min.js" type="text/javascript" ></script>le>
<link rel="stylesheet" type="text/css" href="style/register.css"/>
    <title></title>
</head>
<body>
    <div class='signup_container'>
    <h1 class='signup_title'>用户登陆</h1>
    <img src='images/people.png' id='admin'/>
    <div id="signup_forms" class="signup_forms clearfix">
            <form class="signup_form_form" id="signup_form" method="post" >
                    <div class="form_row first_row">
                        <%--<div class='tip ok'></div>--%>
                        <input type="text" placeholder="请输入用户名" id="signup_name"/>
                    </div>
                    <div class="form_row">
                        <%--<div class='tip error'></div>--%>
                        <input type="password" placeholder="请输入密码" id="signup_password"/>
                    </div>
                    <div class="form_row">
                        <input type="tel" placeholder="请输入验证码" id="signup_codeinput"/>
                        <div class="codediv">
                            <img title="看不清楚?点我换一张" class="codeimg">
                        </div>
                    </div>
           </form>
    </div>
    <div class="login-btn-set"><a href='javascript:void(0)' class='login-btn'></a></div>
    <p class='copyright'>版权所有 </p>
</div>
</body>
<script type="text/javascript">
    $(".codeimg").attr("src", "../PalmShop/ShopCode/ValidateCode.ashx?num=4");
    $('.codeimg').click(function () {
        $(this).attr("src", "../PalmShop/ShopCode/ValidateCode.ashx?num=4&t=" + (new Date()).valueOf());
    });
</script>
</html>