<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MsgWall.aspx.cs" Inherits="Mozart.WXWall.MsgWall" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=walltitle %></title>
    <script src="script/jquery-2.1.0.min.js"></script>
    <link href="style/Themes.css" rel="stylesheet" />
    <link rel="stylesheet" href="style/ImgBox.css" />
<script type="text/javascript" src="script/ImgBox.js"></script>
    <script src="script/weixinwall.js"></script>
</head>
<body class="ui-mobile-viewport ui-overlay-a" <%=bgimgurl %>>
    <form id="form1" runat="server">
        <div class="chrometips chromeTip" style="display:none;">
    <a href="javascript:void(0);" class="btntips-close" id="chromeTipCloseBtn">×</a>
    <div class="inner-chrometips">
      <p class="chrm-word1">由于你正在使用非Chrome浏览器，大屏幕的体验处于不佳状态，建议您立刻更换浏览器，以获得更好的大屏幕产品用户体验</p>
      <p class="chrm-word2"><span>安装Chrome浏览器：</span><a href="https://www.google.com/intl/zh-CN/chrome/browser/" target="_blank">chrome</a></p>
    </div>
  </div>

    <div data-role="header" data-position="fixed" class="divChatsBanner ui-header ui-header-fixed"  data-tap-toggle="false" role="banner">
	<h1 class="bannerHeader" role="heading" aria-level="1"><%=walltitle %></h1>
        <div class="num-t">
                <p><em><%=msgcount %></em></p><span>条信息</span>
            </div>
    <a href="javascript:" class="ui-btn-right" onclick="ShowRightPanel();"><img src='<%=codimg %>'/></a>
</div>

        <%=msglist %>

    <div data-role="panel" id="rightPanel" data-dismissible="false" data-position-fixed="true" data-position="right" 
        data-display="overlay" data-theme="none" class="ui-panel ui-panel-position-right ui-panel-display-overlay
         ui-body-none ui-panel-animate ui-panel-closed ui-panel-fixed">
        <div class="ui-panel-inner">
            <div class="divQRCode">
                <img src='<%=codimg %>'/>
	<%--<span>扫描二维码添加公众账号，发送 <font class="spRed">用户名+#+消息</font>参与活动</span>--%>
                <span>扫描二维码添加公众账号，发送 <font class="spRed">#1</font>参与活动</span>
	<div class="divUserCount">
        <a id="btnHideRightPanel" href="javascript:" title="隐藏" onclick="HideRightPanel();"></a>
        共有<span><font class="spRed"><%=strUserCount %></font></span>人加入
	</div>
	</div>
        </div>
     </div>
    <div data-role="footer" data-position="fixed" class="divChatsFooter ui-footer ui-footer-fixed" data-tap-toggle="false" role="contentinfo">
        <div class="divTabLink">
            <a href="<%=urlparm %>action=msg" class="on ui-link">微信墙</a>
            <a href="<%=urlparm %>action=photo" class="ui-link">图片集</a>
            <a href="<%=urlparm %>action=lottery" class="ui-link">抽奖</a>
            <a href="javascript:" onclick="ShowCodeimgdiv();" class="ui-link">如何加入</a>
            <a href="javascript:" onclick="GotoFullscreen()" class="ui-link">进入全屏</a>
            <a href="javascript:" class="ui-link">技术支持：微商易</a>
        </div>
    </div>
        <div class="divCodeimg">
<div class="scanjoin">
    <img src='<%=codimg %>'/>
    <div class="scanjoinright">
        <a href="javascript:void(0);" class="flbtn-close" title="关闭"></a>
        <div>
            <%--扫描二维码添加公众账号，发送<span class="redfont">用户名+#+消息</span>参与活动<br />--%>
            扫描二维码添加公众账号，发送<span class="redfont">#1</span>参与活动<br />
            <%=appid %>
        </div>
    </div>
</div>
</div>
        <script>
            var timer;
            var clickTimes = 0;
            var randnum;
            var lotterynum=0;
            var linum=0;
            var areaServer =<%=script%>;
            var num = areaServer.length-1;
            var timecount;
            function getRandNum() {
                num = parseInt(num);
                var y = GetRnd(0, num);
                $("#rockname").html(areaServer[y]["nickname"]);
                $("#rockname").attr("data-id",areaServer[y]["id"]);
                $("#rockname").attr("data-rmid",areaServer[y]["rmid"]);
            }
            function start() {
                clearInterval(timer);
                timer = setInterval('change()', 50);
            }
            function ok() {
                clearInterval(timer);
            }
            function GetRnd(m, n) {
                randnum = parseInt(Math.random() * (n - m + 1));
                return randnum;
            }
            function setTimer() {
                if (num <= 0||$(".lotteryUserNum").html()=="1") {
                    alert("用户数量少于2个"); return;
                } else {
                    time = setInterval("getRandNum()", 50);
                    $("span[id='startLottery']").css('display', 'none');
                    $("span[id='endLottery']").css('display', 'inline');
                }
            }
            function clearTimer() {
                noDupNum();
                clearInterval(time);
                $("span[id='endLottery']").css('display', 'none');
                $("span[id='startLottery']").css('display', 'inline');
                setValues();
                if($('#prize-list').html().length>0)
                {
                    $(".und-btn").css('display', 'inline');
                }
                timecount--;
                if (num > 0&&timecount >0)
                {
                    $("span[id='endLottery']").removeAttr("onclick");
                    $("span[id='endLottery']").unbind("onclick");
                    $("#endLottery").html("正在抽奖("+timecount+")");
                    setTimer();
                    countDown(5);
                }else
                {
                    $("#endLottery").html("停止");
                }
            }
            function noDupNum() {
                areaServer.removeEleAt(randnum);
                var o = 0;
                for (p = 0; p < areaServer.length; p++) {
                    if (typeof areaServer[p] != "undefined") {
                        areaServer[o] = areaServer[p];
                        o++;
                    } else {
                        areaServer[p] = "";
                    }
                }
                num = areaServer.length - 1;
            }
            Array.prototype.removeEleAt = function (dx) {
                if (isNaN(dx) || dx > this.length) { return false; }
                this.splice(dx, 1);
            }
        </script>
    </form>
</body>
</html>