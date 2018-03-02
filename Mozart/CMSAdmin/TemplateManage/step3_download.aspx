<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="step3_download.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.step3_download" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" name="viewport"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <title></title>
    <link rel="stylesheet" href="css/1.css"/>
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script>
        $(function () {
            $('.platforms a').hide();
        });
    </script>
</head>
<body>
    <header>
        <h1>
            <span></span><i></i></h1>
        <div class="cf">
            <div class="download">
                <p></p>
                <div class="downloadBtn">
                    免费下载<i></i>
                </div>
                <div class="downloadBtn">
                    免费下载<i style="background: url('images/icon_an.png') no-repeat scroll 0 0 / contain rgba(0, 0, 0, 0);"></i>
                </div>
            </div>
            <div class="pics">
                <div class="picsInner">
                    <img class="pAn" src="images/an.png" width="64" height="118"> <img class="pIp" src="images/ip.png" width="73" height="153">
                </div>
            </div>
            <div class="sawtooth">
            </div>
        </div>
    </header>
    <div class="content">
        <div class="detail">
        </div>
        <div class="arrow_down">
            <i>
            </i>
        </div>
        <div class="platforms">
            <a href="javascript:void(0);">
                <div class="arrow">
                    <i>
                    </i>
                </div>
                <div class="icon">
                    <img src="images/weixin.png">
                </div>
                <div class="word">
                    微信
                </div>
            </a>
            <a href="javascript:void(0);">
                <div class="arrow">
                    <i>
                    </i>
                </div>
                <div class="icon">
                    <img src="images/mp.png">
                </div>
                <div class="word">
                    轻应用
                </div>
            </a>
            <a href="javascript:void(0);">
                <div class="arrow">
                    <i>
                    </i>
                </div>
                <div class="icon">
                    <img src="images/sms.png">
                </div>
                <div class="word">
                    快件
                </div>
            </a>
        </div>
    </div>
</body>
</html>
