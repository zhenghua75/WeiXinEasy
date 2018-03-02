<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mozart.HPPrint.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/style.css" />
    <script type="text/javascript" src="3Djs/jquery.min.js"></script>
    <script type="text/javascript" src="3Djs/flux.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="box">
			<div class="top">
				<img src="img/logo.png"/>
			</div>
			<div class="center">
				<div class="left">
					  <div id="slider">
		                <%=printimglist %>
                    </div>
				</div>
				<div class="right">
				    <div class="erweima">
				    	<%=codeimg %>
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
    <script type="text/javascript">
        $(function () {
            if (!flux.browser.supportsTransitions) {
                alert("该浏览器不支持CSS3"); $("#slider").hide();
            }
            window.f = new flux.slider('#slider', {
                pagination: false,//图片页码导航
                controls: false,//上一张下一张
                delay: 5000,//图片切换时间
                width: 770,//设置图片高度
                height: 588,//设置图片宽度
                transitions: ['blocks2', 'bars', 'blinds', 'blocks', 'concentric', 'dissolve', 'slide', 'warp', 'swipe', 'zip',
                    'bars3d', 'blinds3d', 'cube', 'tiles3d', 'turn', 'explode'], //后面6种为3D效果，前面为2D效果
                autoplay: true//自动播放
            });
        });
</script>
</html>
