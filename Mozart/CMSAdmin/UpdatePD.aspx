<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatePD.aspx.cs" Inherits="Mozart.CMSAdmin.UpdatePD" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>网点管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#F5F5F5"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = " padding:20px,0px,0px,20px; ">
		<div>
			<h2>修改密码</h2>
		</div>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">  
			<asp:Panel ID="Panel1" runat="server" Height="122px" HorizontalAlign="Center">
				<table>
					<tr>
						<td align="right" class="style1">原密码：</td>
						<td><asp:TextBox ID="txtOldPD" runat="server"></asp:TextBox></td>
						
					</tr>
					<tr>
						<td align="right" class="style1">新密码：</td>
						<td><asp:TextBox ID="txtNewPD" runat="server"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right" class="style1">重复密码：</td>
						<td><asp:TextBox ID="txtSecPD" runat="server"></asp:TextBox></td>
					</tr>                    
					<tr>
						<td align="center" class="style1"><asp:Button ID="btnOK" runat="server" Text="确定" 
								onclick="btnOK_Click" /></td>
						<td align="center"><asp:Button ID="btnCancel" runat="server" Text="重填" 
								onclick="btnCancel_Click" /></td>
					</tr>
				</table>
			</asp:Panel>
		</div>
	</div>
	</form>
</body>
</html>
