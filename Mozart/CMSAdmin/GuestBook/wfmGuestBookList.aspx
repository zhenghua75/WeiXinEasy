﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmGuestBookList.aspx.cs" Inherits="Mozart.CMSAdmin.GuestBook.wfmGuestBookList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>活动管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	    function ShowGuestBookList(strid) {
	        $.dialog({
	            id: 'wfmBBSGuestBookUpdate',
	            title: '留言信息查看',
	            top: '20px',
	            width: '600px',
	            height: '400px',
	            content: 'url:GuestBook/wfmBBSGuestBookUpdate.aspx?action=show&id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }
	    function UpdateRepaly(strid) {
	        $.dialog({
	            id: 'wfmCategoryUpdate',
	            title: '留言回复',
	            width: '1000px',
	            height: '640px',
	            content: 'url:GuestBook/wfmBBSGuestBookUpdate.aspx?action=update&id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }
	    function AdminBizControlConfirm(strURL) {
	        $.dialog({
	            content: '确定删除？',
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
<body bgcolor="#F5F5F5"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = "padding:20px,0,0,20px; ">
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>姓名：</td>
					<td>
						<asp:TextBox ID="txtTitle" runat="server" Width="80px"></asp:TextBox>
					</td>
                    <td>信息状态：</td>
					<td>
						<asp:DropDownList ID="ddlisdel"  runat="server">
                            <asp:ListItem Text="--全部--" Value=""></asp:ListItem>
                            <asp:ListItem Text="已删" Value="1"></asp:ListItem>
                            <asp:ListItem Text="未删" Value="0"></asp:ListItem>
                        </asp:DropDownList>
					</td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />
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
						<th>姓名</th>
                        <th>手机</th>
                        <th>留言时间</th>
                        <th>回复状态</th>
                        <th>管理</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC">   
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td>                   
						<td><%#Eval("UserName")%></td>
                        <td><%#Eval("UserMobile")%></td>
                        <td><%#Eval("CreateTime")%></td>
                        <td><asp:Label ID="isreplay" Text='<%#Eval("Replay")%>' runat="server" /> </td>
						<td>
                            <asp:Label  ID="isview" runat="server" />&nbsp;|&nbsp;
                            <a href="#" onclick="ShowGuestBookList('<%#Eval("ID")%>');">留言详细</a>&nbsp;|&nbsp;
                            <asp:Label Text='<%#Eval("State") %>' ID="state" runat="server" />
                            <a href="#" onclick="AdminBizControlConfirm('wfmGuestBookAdmin.aspx?action=del&id=<%#Eval("ID")%>');">删除</a>
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