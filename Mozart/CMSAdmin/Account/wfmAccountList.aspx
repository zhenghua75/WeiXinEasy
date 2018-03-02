<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmAccountList.aspx.cs" Inherits="Mozart.CMSAdmin.Account.wfmAccountList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>用户管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	    function AddAccount() {
	        $.dialog({
	            id: 'wfmAccountAdd',
	            title: '添加账户',
	            width: '1000px',
	            height: '600px',
	            content: 'url:Account/wfmAccountAdd.aspx',
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }
	    function UpdateAccount(strid) {
	        $.dialog({
	            id: 'wfmAccountAdd',
	            title: '修改账户',
	            width: '1000px',
	            height: '600px',
	            content: 'url:Account/wfmAccountUpdate.aspx?id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }
	    function ChangePD(strid) {
	        $.dialog({
	            id: 'wfmChangeAgentPD',
	            title: '重置密码',
	            width: '246px',
	            height: '160px',
	            content: 'url:Account/wfmChangeAgentPD.aspx?id=' + strid,
	            lock: true
	        });
	    }
	</script>
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = "padding:20px,0,0,20px; ">
<%--		<div>
			<h2>网点信息明细查询</h2>
		</div>--%>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>账号：</td>
					<td>
                        <asp:TextBox ID="txtLoginName" runat="server" Width="80px" />
					</td>
					<td>账户：</td>
					<td>
						<asp:TextBox ID="txtName" runat="server" Width="80px"></asp:TextBox>
					</td>
					<td>状态：</td>
					<td>
						<asp:DropDownList ID="ddlState" runat="server" Width="84px">
						</asp:DropDownList>
					</td>
					<td>
						<asp:Button ID="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click" />
                        <input type="button" value ="添加账户" onclick="AddAccount()"/>
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
						<th>用户账号</th>
						<th>用户名称</th>
                        <th>所属代理商</th>
                        <th>所属角色</th>
                        <th>联系邮箱</th>
                        <th>联系地址</th>
                        <th>联系电话</th>
                        <th>联系固话</th>
                        <th>创建时间</th>
                        <th>站点代码</th>
                        <th>用户状态</th>
						<th>重置密码</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC">
						<%--<td><%#DataBinder.Eval(Container.DataItem, "ID")%></td> --%>    
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td>                   
						<td><%#DataBinder.Eval(Container.DataItem, "LoginName")%></td>
						<td><%#DataBinder.Eval(Container.DataItem, "Name")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "AgentID")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "RoleID")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Email")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Address")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Mobile")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Telphone")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "CreateTime")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "SiteCode")%></td>
						<td><%#DataBinder.Eval(Container.DataItem, "Status")%></td>				
						<td><a href="#" onclick="UpdateAccount('<%#DataBinder.Eval(Container.DataItem, "ID")%>');">修改</a></td>
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