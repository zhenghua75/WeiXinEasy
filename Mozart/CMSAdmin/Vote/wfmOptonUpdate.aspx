<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmOptonUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.Vote.wfmOptonUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>选项修改</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script src="../script/smart.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
    <style>
        .td1 {
        width:80px;
        }
    </style>
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hd_content" runat="server" Value="" />
    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			<table border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td align="right" class="td1">名称：</td>
					<td align="left"><asp:TextBox ID="optitle" runat="server"></asp:TextBox></td>                    
				</tr>
                <tr>
                        <td align="right">图像：</td>
                        <td align="left">
                            <div style="overflow: hidden;">
                                <input type="file" id="file0" runat="server" />&nbsp;
                            </div>
                            <div>
                                <img src="" id="img0" width="60" height="60" alt="" runat="server" />
                            </div>
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
                        <td align="right">说明内容：</td>
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
					<td align="right" class="td1">序号：</td>
					<td align="left"><asp:TextBox ID="oporder" MaxLength="2" runat="server" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^0-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^0-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"></asp:TextBox></td>                    
				</tr>
                <tr>
					<td align="right"></td>	
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return getContent();" onclick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnReset" runat="server" Text="重置" onclick="btnReset_Click" />
                        <script type="text/javascript">
                            function getContent() {
                                var temp = UE.getEditor('myEditor').getContent();
                                //alert(temp);
                                document.getElementById("<%=hd_content.ClientID %>").value = temp;
                                }
                            </script>
                    </td>
                    </tr>
      </table>
    </div>
    </form>
</body>
</html>