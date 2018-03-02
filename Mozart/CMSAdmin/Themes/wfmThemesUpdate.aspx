<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmThemesUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.Themes.wfmThemesUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>修改添加</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script src="../script/smart.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
    <style>
        .td1 {
        width:80px;
        }
    </style>
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
    <form id="form1" runat="server">
    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			<table border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td align="right" class="td1">名称：</td>
					<td align="left"><asp:TextBox ID="themesname" runat="server"></asp:TextBox></td>                    
				</tr>
                <tr>
					<td align="right" class="td1">文件夹名：</td>
					<td align="left"><asp:TextBox ID="themesvalue" runat="server"></asp:TextBox></td>                    
				</tr>
                <tr>
					<td align="right" class="td1">是否生效：</td>
					<td align="left">
                        <asp:RadioButton Text="是" ID="isstateyes" GroupName="isstate" runat="server" />
                        <asp:RadioButton ID="isstateno" Text="否" GroupName="isstate" runat="server" />
					</td>                    
				</tr>
                <tr>
					<td align="right"></td>	
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnReset" runat="server" Text="重置" onclick="btnReset_Click" />
                    </td>
                    </tr>
      </table>
    </div>
    </form>
</body>
</html>