<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmProductAdd.aspx.cs" Inherits="Mozart.CMSAdmin.Product.wfmProductAdd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>添加文章</title>
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
    <script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script src="../script/smart.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.config.js" type="text/javascript"></script>
	<script src="../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
</head>
<body style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:HiddenField ID="hd_content" runat="server" Value="" />
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">标题：</td>
                        <td align="left">
                            <asp:TextBox ID="txtArticleTitle" runat="server" width="800px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">图标：</td>
                        <td align="left">
                            <div style="overflow: hidden;">
                                <input type="file" id="file0" runat="server"/>
                            </div>
                            <div><img src="" id="img0" width="60" height="60" alt=""/></div>
                            <script type="text/javascript">
                                $("#file0").change(function () {
                                    var objUrl = getObjectURL(this.files[0]);
                                    console.log("objUrl = " + objUrl);
                                    if (objUrl) {
                                        $("#img0").attr("src", objUrl);
                                    }
                                });

                                function getObjectURL(file) {
                                    var url = null;
                                    if (window.createObjectURL != undefined) {
                                        url = window.createObjectURL(file);
                                    } else if (window.URL != undefined) {
                                        url = window.URL.createObjectURL(file);
                                    } else if (window.webkitURL != undefined) {
                                        url = window.webkitURL.createObjectURL(file);
                                    }
                                    return url;
                                }
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">内容：</td>
                        <td align="left">
                            <div id="myEditor" style="height:200px; width:800px;"></div>
	                        <script type="text/javascript">
	                            var temp = document.getElementById("<%=hd_content.ClientID %>").value;
	                            var ue = new baidu.editor.ui.Editor();
	                            ue.render("myEditor");   //这里填写要改变为编辑器的控件id
	                            ue.ready(function () { ue.setContent(temp); })
 	                        </script>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">单位：</td>
                        <td align="left">
                            <asp:TextBox ID="txtUnit" runat="server" width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">普通价格：</td>
                        <td align="left">
                            <asp:TextBox onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ID="txtNPrice" runat="server" width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">会员价格：</td>
                        <td align="left">
                            <asp:TextBox onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ID="txtMPrice" runat="server" width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">积分：</td>
                        <td align="left">
                            <asp:TextBox onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ID="txtCredits" runat="server" width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">类别：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlCategory" Width="180px" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">是否置顶：</td>
                        <td align="left">
                            <asp:DropDownList ID="txtArticleIsTop" runat="server">
                                <asp:ListItem>否</asp:ListItem>
                                <asp:ListItem>是</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">顺序号：</td>
                        <td align="left">
                             <asp:TextBox onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ID="txtOrder" runat="server" width="20px" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" OnClientClick="return getContent();" CssClass="cssbtn" /> 
                            <script type="text/javascript">
                                function getContent() {
                                    var temp = UE.getEditor('myEditor').getContent();
                                    //alert(temp);
                                    document.getElementById("<%=hd_content.ClientID %>").value = temp;
                                }
                            </script>
						    <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>