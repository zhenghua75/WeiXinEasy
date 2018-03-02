<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoMe.aspx.cs" Inherits="Mozart.MicroSite.PhotoMe" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <title>照片裁剪</title>
    <link rel="stylesheet" type="text/css" href="Themes/Default/style/base.css" />
    <link rel="stylesheet" type="text/css" href="Themes/PhotoWall/style/jquery.Jcrop.min.css" />
    <script type="text/javascript" src="Themes/Default/script/jquery.min.js"></script>
    <script type="text/javascript" src="Themes/PhotoWall/script/jquery.Jcrop.min.js"></script>
</head>
<body>
    <script type="text/javascript">
            jQuery(function ($) {
                var api;
                $('#target').Jcrop({
                    // start off with jcrop-light class
                    bgOpacity: 0.6,
                    bgColor: 'white',
                    addClass: 'jcrop-light'
                    //onSelect: updateCoords
                }, function () {
                    api = this;
                    api.setSelect([20, 20, 20 + 260, 20 + 300]);
                    api.setOptions({ bgFade: true });
                    api.setOptions({ allowSelect: false });
                    api.setOptions({ allowResize: false });
                    api.ui.selection.addClass('jcrop-selection');
                });
            });

            function updateCoords(c) {
                $('#x').val(c.x);
                $('#y').val(c.y);
            };
    </script>
    <form id="form1" runat="server">
        <input type="hidden" id="x" name="x" runat="server" value="" />
		<input type="hidden" id="y" name="y" runat="server" value="" />
    <div>
        <div id="top">
        <div class="back-ui-a">
            <a href="javascript:history.back(1)">返回</a>
        </div>
        <div class="header-title">照片裁剪</div>
        <div class="site-nav">
            <ul class="fix">
               <!-- <li class="mycart"><a href="MyCart.aspx">购物车</a></li>-->
            </ul>
        </div>
    </div>
    <div class="line"></div>
    <div style="margin:0 auto; width:280px;text-align:center"><img id="target" runat="server" style="width:100%" src="\USER_PHOTO\$pDetail.FilePath" alt="" /></div>
    <div class="line"></div>
    <div class="layout" style="margin-top:10px">
        <div class="btn-ui-b mt10">
 <%--           <a href="MyThumbPrintTwo.aspx?ThumbID=$pDetail.ID" style="color:#fff">下一步</a>--%>
            <a href="#" id="aSave" runat="server" style="color:#fff" onserverclick="btnSave_Click">下一步</a>
            <%--<asp:Button ID="btnSave" style="color:#fff" runat="server" Text="下一步" OnClick="btnSave_Click" />--%>
        </div>         
    </div>
    <div id="footer">
        <div class="copyright">版权所有 &#169; 2012-2014 奥琦微商易</div>
    </div>
    </div>
    </form>
</body>
</html>
