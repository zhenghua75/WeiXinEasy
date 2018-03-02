<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmPhotoSubmitUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.MiniShop.wfmPhotoSubmitUpdate" %>

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
</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:HiddenField ID="hd_content" runat="server" />
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">订单号：</td>
                        <td align="left">
                            <asp:TextBox ID="ordernum" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">订单照片：</td>
                        <td align="left">
                            <a target="_blank" href="<%=imgsrc1 %>">
                                <img id="img1"
                                    width="150" height="150" runat="server" />
                            </a>
                            <a target="_blank" href="<%=imgsrc2 %>">
                            <img id="img2"
                                width="150" height="150" runat="server" />
                            </a>
                            <div>
                                <span style="float:left;">照片1</span><span style="float:right;">照片2</span>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td align="right">审核情况：</td>
                        <td align="left">
                            <asp:RadioButton id="ryes" Text="通过" GroupName="Reivew" runat="server" />
                            <asp:RadioButton ID="rno" Text="未通过" GroupName="Reivew" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">打印照片：</td>
                        <td align="left">
                            <asp:DropDownList id="printimg" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">文字描述：</td>
                        <td align="left">
                            <asp:TextBox ID="AttachText" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="提交"
                                 OnClick="btnSave_Click" CssClass="cssbtn" /> 
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>