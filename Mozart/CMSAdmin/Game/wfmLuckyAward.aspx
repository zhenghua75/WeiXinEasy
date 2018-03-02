﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmLuckyAward.aspx.cs" Inherits="Mozart.CMSAdmin.Game.wfmLuckyAward" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>奖项管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Calendar/WdatePicker.js"></script>
	<script type="text/javascript">
	    function AddFunction() {
	        $.dialog({
	            id: 'wfmJCQuizAdd',
	            title: '添加奖项',
	            top: '20px',
	            width: '1000px',
	            height: '600px',
	            content: 'url:Game/wfmLuckyAwardAdd.aspx',
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }

	    function UpdateArticle(strid) {
	        $.dialog({
	            id: 'wfmJCQuizUpdate',
	            title: '奖项修改',
	            top: '20px',
	            width: '1000px',
	            height: '600px',
	            content: 'url:Game/wfmLuckyAwardUpdate.aspx?action=update&id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }

	    function ShowArticle(strid) {
	        $.dialog({
	            id: 'wfmJCQuizUpdate',
	            title: '奖项详细',
	            top: '20px',
	            width: '1000px',
	            height: '600px',
	            content: 'url:Game/wfmLuckyAwardUpdate.aspx?action=show&id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }
	    function AdminBizControlConfirm(strURL) {
	        $.dialog({
	            content: '确定删除该信息？',
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
    <style>
        ul {list-style:none;padding:0px 0px 0px 0px;margin:0px 0px 0px 0px;}
        ul li {float:left;padding:0px 0px 0px 0px;margin:0px 0px 0px 0px;}
    </style>
</head>
<body bgcolor="#F5F5F5" style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = "padding:20px,0,0,20px; ">
		<div>
			<h2>奖项管理</h2>
		</div>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>名称：</td>
					<td>
						<asp:TextBox ID="txtName" runat="server" Width="80px"></asp:TextBox>
					</td>
                        <td align="right">活动名称：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlactlist"  runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />&nbsp;
                        <input type="button" value="添加奖项" onclick="AddFunction()" />
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
                        <th>名称</th>
                        <th>活动名称</th>
						<th>中奖数量</th>
                        <th>中奖几率</th>
                        <th>添加时间</th>
                        <th>管理</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC"> 
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td>               
						<td><%#Eval("Award") %></td>
                        <td><%#Eval("ActTitle") %></td>
                        <td><%#Eval("AwardNum") %></td>
                        <td><%#Eval("AwardPro") %></td>
                        <td><%#Eval("AddTime") %></td>
						<td>
                            <ul>
                                <li><a href="#" onclick="ShowArticle('<%#Eval("ID") %>');">详细内容</a>&nbsp;|&nbsp;</li>
                                <li><a href="#" onclick="UpdateArticle('<%#Eval("ID") %>');">修改</a>&nbsp;|&nbsp;</li>
                                <li><a href="#" onclick="AdminBizControlConfirm('wfmLuckyAwardAdmin.aspx?action=del&id=<%#Eval("ID") %>');">删除</a></li>
                            </ul>
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
		CustomInfoHTML="第<font color='red'><b>%currentPageIndex%</b></font>页，共%PageCount%页，每页%PageSize%条记录，共%RecordCount% 条记录"         
		>
	</webdiyer:AspNetPager>  
	</div>
	</form>
</body>
</html>