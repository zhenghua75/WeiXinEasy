<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecHandList.aspx.cs" Inherits="Mozart.PalmShop.ShopCode.SecHandList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
		<title><%=cname %></title>
		<link rel="stylesheet" href="css/bootstrap.min.css" />
		<link id="changecss" rel="stylesheet" type="text/css" href="css/base.css"/>
		<link rel="stylesheet" href="css/bootstrap-theme.min.css" />
		<script type="text/javascript" src="js/jquery-1.8.3.min.js" ></script>
		<script type="text/javascript" src="js/bootstrap.js" ></script>
		<script type="text/javascript" src="js/common.js" ></script>
    <script src="artDialog/jquery.artDialog.js?skin=dialogred" type="text/javascript" charset="gbk"></script>
		<!--[if lt IE 9]>
	      <script src="http://cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
	      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
	    <![endif]-->
</head>
<body style="background: #EFEFF3;">
    <form id="form1" runat="server">
    <div id="nav" class="container-fluid">
			<div class="row">
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                    <a class="back_ico" onclick="javascript:history.back();">返回</a></div>
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center"><%=cname %></div>
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-right">
                    <a class="next_ico" href="PubSecHand.aspx">添加</a></div>
			</div>
		</div>
        <div class="container-fluid">
			<div class="row" id="search_box">
				  <input type="text" id="searchinput" value="请输入要查询的内容" />
				  <input type="button" value="搜索" id="search_btn" />
			</div>
		</div>
		<div class="container-fluid Product_List">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                <div class="row clear_margin">
				<div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                    <a href='Product_detail.aspx?pid=<%#Eval("ID") %>'>
                        <img src='<%#Eval("PimgUrl") %>' class="img-responsive"/></a></div>
				<div class="col-lg-9 col-md-9 col-sm-9 col-xs-8">
					<h4><a href='Product_detail.aspx?pid=<%#Eval("ID") %>'><%#Eval("ptitle") %></a></h4>
					<p>¥<%#Eval("price") %>元</p>
				</div>
			</div>
             </ItemTemplate>
            </asp:Repeater>
            <div style="text-align:center">
                <webdiyer:AspNetPager id="AspNetPager1" runat="server" PageSize="12" AlwaysShow="True"
                 OnPageChanged="AspNetPager1_PageChanged" CustomInfoSectionWidth="24%" CurrentPageButtonPosition="Center">
	</webdiyer:AspNetPager>
            </div> 
		</div>
    </form>
</body>
    <script>
        <%=errorscript%>
        $(document).ready(function () {
            $("#searchinput").attr("onfocus", "if(this.value==defaultValue) {this.value='';this.type='text';this.style.color='#000'}");
            $("#searchinput").attr("onblur", "if(!value) {value=defaultValue; this.type='text';this.style.color='#999'}");
        })
        $(".clear_margin").click(function ()
        {
            var con = $(this).find("a").attr('href');
            if (con != null && con != "" && con.toLowerCase() != "undefined") {
                location.href = con;
            }
        })
        $("#search_btn").click(function () {
            var searchinfo = $("#searchinput").val();
            if (searchinfo != null && searchinfo != "" && searchinfo != "请输入要查询的内容") {
                var url = "";
                url = Request("scid");
                if (url != null && url != "") {
                    url = "?scid=" + url + "&";
                } else {
                    url = "?";
                }
                location.href = "SecHandList.aspx" + url+searchinfo;
            } else {
                $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入要查询的内容！' });
                $("#searchinput")[0].focus(); return false;
            }
        })
        function Request(paras) {
            var url = location.href;
            var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
            var paraObj = {}
            for (i = 0; j = paraString[i]; i++) {
                paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
            }
            var returnValue = paraObj[paras.toLowerCase()];
            if (typeof (returnValue) == "undefined") {
                return "";
            } else {
                return returnValue;
            }
        }
    </script>
</html>
