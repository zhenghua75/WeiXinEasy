<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmBBSGuestBookUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.GuestBook.wfmBBSGuestBookUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>信息阅览</title>
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
    <script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script src="../script/smart.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
    <style>
        .tdname {width:80px;}
        table {background:black;border:1px;}
        table  td{background:#EDF3FD;}
    </style>
</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:0px;" id="showwddiv" runat="server">
			    <table style="height: 300px;text-align:left;width:100%"  cellspacing="1" >
                    <tr>
                        <td align="right" class="tdname">留言人：</td>
                        <td align="left">
                            <asp:Label id="UserName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdname">电话/手机：</td>
                        <td align="left">
                            <asp:Label id="UserMobile" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdname">留言内容：</td>
                        <td align="left">
                            <asp:Label id="Content" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">留言时间：</td>
                        <td align="left">
                            <asp:Label id="CreateTime" runat="server" />
                        </td>
                    </tr>
                    <tr id="repcontr" runat="server">
                        <td align="right" class="tdname">回复内容：</td>
                        <td align="left">
                            <asp:Label id="repcontent" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button Text="回复" ID="repbtn" runat="server" OnClick="repbtn_Click" />
                        </td>
                    </tr>
                    </table>
                </div>
            <div style = "background-color:#BED3FE;border:0px" id="repinputdiv" runat="server">
                <table style="height:300px;" align="center"  cellspacing="1" >
                    <tr>
                        <td align="right" class="tdname">回复内容：</td>
                        <td align="left">
                            <asp:TextBox ID="repcontentinfo" style="border:0px;" width="280px" Height="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"  CssClass="cssbtn" />&nbsp;&nbsp;
						    <asp:Button ID="btnReset" runat="server" Text="取消" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>