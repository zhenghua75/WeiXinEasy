<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alipayreturn.aspx.cs" Inherits="Mozart.PalmShop.ShopCode.Alipayreturn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
         <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
		<title>支付成功</title>
		<link rel="stylesheet" href="css/bootstrap.min.css" />
		<link id="changecss" rel="stylesheet" type="text/css" href="css/base.css"/>
		<link rel="stylesheet" href="css/bootstrap-theme.min.css" />
		<script type="text/javascript" src="js/jquery-1.8.3.min.js" ></script>
		<script type="text/javascript" src="js/bootstrap.js" ></script>
        <script src="artDialog/jquery.artDialog.js?skin=dialogred" type="text/javascript" charset="gbk"></script>
		<!--[if lt IE 9]>
	      <script src="http://cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
	      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
	    <![endif]-->
</head>
<body style="background: #EEEEEE;">
        <div id="nav" class="container-fluid">
			<div class="row">
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4"></div>
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center">支付成功</div>
			</div>
		</div>
		<div class="container-fluid active_box" style="margin:20px 0px 0px 0px;">
            <div class="container-fluid">
			<div class="row">
				<div class="table-responsive" id="product_info">
				  <table class="table">
                      <tr>
				    	<td class="col-xs-3 text-center">产品订单：</td>
				    	<td class="col-xs-9">
                            <%=oid %>
                            &nbsp;&nbsp;
                            <a href="OrderDetail.aspx?oid=<%=oid %>">订单详细</a>
				    	</td>
				    </tr>
                      <tr>
				    	<td class="col-xs-3 text-center">支付宝交易号：</td>
				    	<td class="col-xs-9">
                           <%=payid %>
				    	</td>
				    </tr>
				  </table>
			</div>
            </div>
		</div>
        </div>
</body>
</html>
