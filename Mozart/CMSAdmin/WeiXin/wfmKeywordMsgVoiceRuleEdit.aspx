﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmKeywordMsgVoiceRuleEdit.aspx.cs" Inherits="Mozart.CMSAdmin.WeiXin.wfmKeywordMsgVoiceRuleEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>信息添加</title>
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
    <script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script src="../script/smart.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.config.js" type="text/javascript"></script>
	<script src="../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:HiddenField ID="hd_content" runat="server" />
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table>
                    <tr>
                        <td style="width:70px;" align="right">关键字：</td>
                        <td align="left">
                            <asp:TextBox ID="keyword" runat="server" Width="300px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">回复语音：</td>
                        <td align="left">
                            &nbsp;<asp:FileUpload ID="fudVoice" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">文件小于2M，播放长度不超过60s，支持AMR\MP3格式 </td>
                    </tr>
                    <tr>
                        <td align="right">排序：</td>
                        <td align="left">
                            <asp:TextBox ID="sort" runat="server" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" Width="30px" >0</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:CheckBox ID="cboIsSyn" Text="立即同步到微信服务器"  runat="server" /><br />
                            只有同步到微信服务器的语音文件，才能正确自动回复。默认每5分钟检测一次，未同步的自动同步。
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"  CssClass="cssbtn" />
						    <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>