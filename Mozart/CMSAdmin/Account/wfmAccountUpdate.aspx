<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmAccountUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.Account.wfmAccountUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>账户修改</title>
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
        ul {list-style:none;margin:0px;padding:0px;}
        ul  li{float:left;margin:0px;padding:0px;}
        ul  li input{width:200px;margin:0px;padding:0px;}
    </style>
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
        <asp:HiddenField ID="hd_remark" runat="server" Value="" />
	<div style = " padding:20px,0,0,20px; ">
<%--		<div>
			<h2>账户添加</h2>
		</div>--%>
        <input id="txtID" type="hidden" runat="server" value="" />
        <input id="txtDO" type="hidden" runat="server" value="" />
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			<table border="0" cellpadding="0" cellspacing="0" width="880px">
                <tr><td colspan="2" align="left" style="font-size:14pt;">账户信息：</td></tr>
				<tr>
					<td align="right" class="td1">账号：</td>
					<td align="left"><asp:TextBox ID="txtAccountID" runat="server"></asp:TextBox></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">名称：</td>
					<td align="left"><asp:TextBox ID="txtAccountName" runat="server"></asp:TextBox></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">所属代理商：</td>                    
					<td align="left"><asp:DropDownList ID="ddlAgent" runat="server" Height="20px" Width="148px" >
					</asp:DropDownList></td>                    
				</tr>
                <tr>
					<td align="right" class="td1">所属角色：</td>                    
					<td align="left"><asp:DropDownList ID="ddlRole" runat="server" Height="20px" Width="148px" >
					</asp:DropDownList></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">状态：</td>                    
					<td align="left"><asp:DropDownList ID="ddlState" runat="server" Height="20px" Width="148px" >
					</asp:DropDownList></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">地址：</td>
					<td align="left"><asp:TextBox ID="txtAddress" runat="server" Width="300px" ></asp:TextBox></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">手机：</td>
					<td align="left"><asp:TextBox ID="txtMobile" runat="server" ></asp:TextBox></td>                    
				</tr>
                <tr>
					<td align="right" class="td1">电话：</td>
					<td align="left"><asp:TextBox ID="txtTel" runat="server" ></asp:TextBox></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">EMail：</td>
					<td align="left"><asp:TextBox ID="txtEmail" runat="server" ></asp:TextBox></td>                    
				</tr>
				<tr>
					<td align="right" class="td1">站点代码：</td>
					<td align="left"><asp:TextBox ID="txtSitCode" runat="server" ></asp:TextBox></td>                    
				</tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="880px" runat="server" id="SummaryTb">
                <tr><td colspan="2" align="left" style="font-size:14pt;">站点信息：</td></tr>
                    <tr>
					<td align="right" class="td1">站点图标：</td>
					<td align="left">
                        <div style="overflow: hidden;">
                                <input type="file" id="file0" runat="server" />
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
					<td align="right" class="td1">站点类别：</td>
					<td align="left">
                        <asp:DropDownList ID="ddlsitecategory" Width="180px" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
					</td>                    
				</tr>
                    <tr>
					<td align="right" class="td1">站点摘要：</td>
					<td align="left">
                        <asp:TextBox ID="summary" Width="790px" runat="server" />
					</td>                    
				</tr>
                    <tr>
					<td align="right" class="td1">备注说明：</td>
					<td align="left">
                        <div id="myEditor" style="height:200px; width:800px;"></div>
	                        <script type="text/javascript">
	                            var temp = document.getElementById("<%=hd_remark.ClientID %>").value;
	                            var ue = new baidu.editor.ui.Editor();
	                            ue.render("myEditor");   //这里填写要改变为编辑器的控件id
	                            ue.ready(function () { ue.setContent(temp); })
 	                        </script>
					</td>                    
				</tr>
                    <tr>
					<td align="right" class="td1">站点皮肤：</td>
					<td align="left">
                        <asp:DropDownList ID="ddlthemeslist" Width="180px" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
					</td>                    
				</tr>
                    <tr>
					<td align="right" class="td1">二维码图标：</td>
					<td align="left">
                        <div style="overflow: hidden;">
                                <input type="file" id="file1" runat="server" />
                            </div>
                            <div>
                                <img src="" id="img1" width="60" height="60" alt="" runat="server" />
                            </div>
                            <script type="text/javascript">
                                $("#file1").change(function () {
                                    var objUrl = getObjectURL(this.files[0]);
                                    console.log("objUrl = " + objUrl);
                                    if (objUrl) {
                                        $("#img1").attr("src", objUrl);
                                    }
                                });
                            </script>
					</td>                    
				</tr>
                <tr>
					<td align="right" class="td1">打印机预留照片：</td>
					<td align="left">
                        <ul>
                            <li>
                                <div style="overflow: hidden;">
                                <input type="file" id="file2" runat="server" />
                            </div>
                            <div>
                                <img src="" id="img2" width="60" height="60" alt="" runat="server" />
                            </div>
                            <script type="text/javascript">
                                $("#file2").change(function () {
                                    var objUrl = getObjectURL(this.files[0]);
                                    console.log("objUrl = " + objUrl);
                                    if (objUrl) {
                                        $("#img2").attr("src", objUrl);
                                    }
                                });
                            </script>
                            </li>
                            <li>
                                <div style="overflow: hidden;">
                                <input type="file" id="file3" runat="server" />
                            </div>
                            <div>
                                <img src="" id="img3" width="60" height="60" alt="" runat="server" />
                            </div>
                            <script type="text/javascript">
                                $("#file3").change(function () {
                                    var objUrl = getObjectURL(this.files[0]);
                                    console.log("objUrl = " + objUrl);
                                    if (objUrl) {
                                        $("#img3").attr("src", objUrl);
                                    }
                                });
                            </script>
                            </li>
                            <li>
                                <div style="overflow: hidden;">
                                <input type="file" id="file4" runat="server" />
                            </div>
                            <div>
                                <img src="" id="img4" width="60" height="60" alt="" runat="server" />
                            </div>
                            <script type="text/javascript">
                                $("#file4").change(function () {
                                    var objUrl = getObjectURL(this.files[0]);
                                    console.log("objUrl = " + objUrl);
                                    if (objUrl) {
                                        $("#img4").attr("src", objUrl);
                                    }
                                });
                            </script>
                            </li>
                            <li>
                                <div style="overflow: hidden;">
                                <input type="file" id="file5" runat="server" />
                            </div>
                            <div>
                                <img src="" id="img5" width="60" height="60" alt="" runat="server" />
                            </div>
                            <script type="text/javascript">
                                $("#file5").change(function () {
                                    var objUrl = getObjectURL(this.files[0]);
                                    console.log("objUrl = " + objUrl);
                                    if (objUrl) {
                                        $("#img5").attr("src", objUrl);
                                    }
                                });
                            </script>
                            </li>
                        </ul>
					</td>                    
				</tr>
               </table>
            <table border="0" cellpadding="0" cellspacing="0" width="880px">
				<tr>
					<td align="right" class="td1">&nbsp;</td>					
					<td><asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" OnClientClick="return getContent();" CssClass="cssbtn" /> 
                            <script type="text/javascript">
                                function getContent() {
                                    var temp = UE.getEditor('myEditor').getContent();
                                    document.getElementById("<%=hd_remark.ClientID %>").value = temp;
                                }
                            </script>
						<asp:Button ID="btnReset" runat="server" Text="重置" onclick="btnReset_Click" /></td>                    
				</tr>
			</table>		
	</div>
	</div>
	</form>
</body>
</html>

