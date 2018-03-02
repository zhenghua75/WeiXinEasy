﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HPPrint.aspx.cs" Inherits="Mozart.HPPrint.HPPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/style.css" />
		 <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
         <script type="text/javascript" src="js/swfobject.js"></script>
    <script>
        var flashvars = {};
        flashvars.xml ="<%=sitecode%>config.xml";
        var params = {};
        params.allowscriptaccess = "always";
        params.allownetworking = "all";
        params.wmode = "transparent";
        var attributes = {};
        attributes.id = "slider";
        swfobject.embedSWF("16sucai.swf", "cu3er_swf", "770", "588", "9", flashvars, params, attributes);
    </script>
    <!--[if lte IE 6]>
       <script type="text/javascript" src="js/DD_belatedPNG_0.0.8a.js"></script>
       <script type="text/javascript">
       DD_belatedPNG.fix('*');
       </script>
     <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div class="box">
			<div class="top">
				<img src="img/logo.png"/>
			</div>
			<div class="center">
				<div class="left">
					   <div id="cu3er_swf">
						    <a href="http://www.adobe.com/go/getflashplayer">
						        <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" />
						    </a>
						</div>
				</div>
				<div class="right">
				    <div class="erweima">
				    	<%=strcodeimg %>
				    	<h4>微信扫一扫关注我</h4>
				    </div>
					<div class="tiaoma">
					    <strong>打印验证码:</strong>
						<label>1111</label>
					</div>
					<div class="zhuyi">
						<h3>“微商易”照片打印操作流程</h3>
					<ol>
						<li><label>第一步</label>，关注公众账号，发送打印图片；<br/>发送关键字“我的相册”。</li>
						<li><label>第二步</label>，点击进入“我的相册”；<br/> 选择照片，进行裁剪，输入需要打印的心情文字。 </li>
						<li><label>第三步</label>，输入上面打印验证码号码提交，等待打印。打印完成取出相片，完成打印。</li>
						
					</ol>
					</div>
				</div>
			</div>
			<div class="bottom"><img src="img/hezuo.png"/>
                    <span>
                    电话：0874-3368666     传真：0874-3368111  邮编：655000<br/>

集团总部地址：云南省曲靖市麒麟区官坡寺街168号</span>
			</div>
		</div>
    </form>
</body>
</html>
