<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmAlbumTypeAdd.aspx.cs" Inherits="Mozart.CMSAdmin.Album.wfmAlbumTypeAdd" %>

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
			    <table style="height: 345px">
                    <tr>
                        <td align="right">名称：</td>
                        <td align="left">
                            <asp:TextBox ID="AlbumTypeName" runat="server" Width="800px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">封面：</td>
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
                        <td align="right">简要说明：</td>
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
					<td align="right" class="td1">是否生效：</td>
					<td align="left">
                        <asp:RadioButton Text="是" ID="isstateyes" Checked="true" GroupName="isstate" runat="server" />
                        <asp:RadioButton ID="isstateno" Text="否" GroupName="isstate" runat="server" />
					</td>                    
				</tr>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" OnClientClick="return getContent();" CssClass="cssbtn" /> 
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