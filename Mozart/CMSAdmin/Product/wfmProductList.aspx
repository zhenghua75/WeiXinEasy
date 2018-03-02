<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmProductList.aspx.cs" Inherits="Mozart.CMSAdmin.Product.wfmProductList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>商品管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	    function AddProduct() {
	        $.dialog({
	            id: 'wfmProductAdd',
	            title: '添加商品',
	            top: '20px',
	            width: '1000px',
	            height: '640px',
	            content: 'url:Product/wfmProductAdd.aspx',
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }

	    function UpdateProduct(strid) {
	        $.dialog({
	            id: 'wfmProductUpdate',
	            title: '商品修改',
	            top: '20px',
	            width: '1000px',
	            height: '640px',
	            content: 'url:Product/wfmProductUpdate.aspx?action=update&id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }

	    function ShowProduct(strid) {
	        $.dialog({
	            id: 'wfmProductShow',
	            title: '商品详细内容',
	            top: '20px',
	            width: '1000px',
	            height: '640px',
	            content: 'url:Product/wfmProductUpdate.aspx?action=show&id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }

	    function AdminBizControlConfirm(strURL) {
	        $.dialog({
	            content: '确定删除此商品？',
	            ok: function () {
	                AdminBizControl(strURL);
	                return true;
	            },
	            cancel: function () {
	                return true;
	            }
	        });
	    }

	    function AdminBizControl(strURL) {//我们就是通过这个函数来异步获取信息的
	        var xmlHttpReq = null; //声明一个空对象用来装入XMLHttpRequest
	        if (window.XMLHttpRequest) {//除IE5 IE6 以外的浏览器XMLHttpRequest是window的子对象
	            xmlHttpReq = new XMLHttpRequest(); //我们通常采用这种方式实例化一个XMLHttpRequest
	        }
	        else if (window.ActiveXObject) {//IE5 IE6是以ActiveXObject的方式引入XMLHttpRequest的
	            xmlHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
	            //IE5 IE6是通过这种方式
	        }
	        if (xmlHttpReq != null) {//如果对象实例化成功 我们就可以干活啦
	            xmlHttpReq.open("get", strURL, true);
	            //调用open()方法并采用异步方式
	            xmlHttpReq.onreadystatechange = RequestCallBack; //设置回调函数
	            xmlHttpReq.send(null); //因为使用get方式提交，所以可以使用null参调用
	        }
	        function RequestCallBack() {//一旦readyState值改变，将会调用这个函数
	            if (xmlHttpReq.readyState == 4) {
	                alert(xmlHttpReq.responseText);
	                //将xmlHttpReq.responseText的值赋给iptText控件
	                __doPostBack('btnQuery', '');
	            }
	        }
	    }
	</script>
</head>
<body bgcolor="#F5F5F5" style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = "padding:20px,0,0,20px; ">
		<div>
			<h2>商品管理</h2>
		</div>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>标题：</td>
					<td>
						<asp:TextBox ID="txtTitle" runat="server" Width="80px"></asp:TextBox>
					</td>
					<td>类别：</td>
					<td>
					    <asp:DropDownList ID="ddlCategory" Width="180px" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
					</td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />&nbsp;
                        <input type="button" value="添加商品" onclick="AddProduct()" />
                    </td>
				</tr>
			</table>
		</asp:Panel>
	</div>
		<br />
		<div style = "background-color:#EDF3FD; width:100%;">
		<asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" >
			<HeaderTemplate>
				<table width="100%" border="1" cellspacing="0" cellpadding="4" style="border-collapse:collapse">
					<tr>
						<th>序号</th>
						<th>标题</th>
                        <th>图标</th>
                        <th>类别</th>
                        <th>单位</th>
                        <th>普通价格</th>
                        <th>会员价格</th>
                        <th>上架时间</th>
                        <th>可兑积分</th>
                        <th>排序号</th>
                        <th>管理</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC">  
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td>                   
						<td><%#DataBinder.Eval(Container.DataItem, "Name")%></td>
						<td><img src="../../<%#DataBinder.Eval(Container.DataItem, "Photo")%>" alt="图标" width="20px",height="20px"/></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "CatName")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Unit")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "NormalPrice")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "MemberPrice")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Pdate","{0:yyyy-M-d}")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Credits")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Order")%></td>
						<td>
                            <a href="#" onclick="ShowProduct('<%#DataBinder.Eval(Container.DataItem, "ID")%>');">详细内容</a>&nbsp;|&nbsp;
                            <a href="#" onclick="UpdateProduct('<%#DataBinder.Eval(Container.DataItem, "ID")%>');">修改</a>&nbsp;|&nbsp;
                            <a href="#" onclick="AdminBizControlConfirm('wfmProductAdmin.aspx?action=del&id=<%#DataBinder.Eval(Container.DataItem, "ID")%>');">删除</a>
						</td>
					</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
		</div>
		<br />
		<webdiyer:AspNetPager 
		id="AspNetPager1" 
		runat="server" 
		PageSize="12" 
		AlwaysShow="True" 
		OnPageChanged="AspNetPager1_PageChanged" 
		ShowCustomInfoSection="Left" 
		CustomInfoSectionWidth="24%" 
		ShowPageIndexBox="always"         
		TextAfterPageIndexBox="页" 
		TextBeforePageIndexBox="转到第" 
		FirstPageText="【首页】" 
		LastPageText="【尾页】" 
		NextPageText="【后页】" 
		PrevPageText="【前页】" 
		NumericButtonTextFormatString="{0}"         
		CustomInfoHTML="第<font color='red'><b>%currentPageIndex%</b></font>页，共%PageCount%页，每页%PageSize%条记录"         
		>
	</webdiyer:AspNetPager>  
	</div>
	</form>
</body>
</html>