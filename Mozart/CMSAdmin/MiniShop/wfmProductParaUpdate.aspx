﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmProductParaUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.MiniShop.wfmProductParaUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>信息修改</title>
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
    <script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script src="../script/smart.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.config.js" type="text/javascript"></script>
	<script src="../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
    <script src="../script/region_select.js" type="text/javascript" charset="gb2312"></script>
    <style>
select{display: inline-block;font-size: 14px;line-height: 20px;color: #555555;
       -webkit-border-radius: 4px;-moz-border-radius: 4px;border-radius: 4px;vertical-align: middle;
}
input[name^="select"] {display:none;border:0px;}
</style>
</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">参数名称：</td>
                        <td align="left">
                            <asp:TextBox ID="paraname" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">参数值：</td>
                        <td align="left">
                            <asp:TextBox ID="paravalue" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" OnClientClick="return isEmail();" CssClass="cssbtn" /> 
						    <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>