<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmSiteActivityUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.Coupon.wfmSiteActivityUpdate" %>
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
     <script type="text/javascript" src="../Calendar/WdatePicker.js"></script>

</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:HiddenField ID="hd_content" runat="server" Value="" />
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">活动名称：</td>
                        <td align="left">
                            <asp:TextBox ID="txtName" runat="server" width="800px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">图标：</td>
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
                            <asp:TextBox ID="txtSummary" runat="server" width="800px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">活动内容：</td>
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
                        <td align="right">折扣说明：</td>
                        <td align="left">
                            <asp:TextBox ID="Discount"  runat="server" width="800px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">开始时间：</td>
                        <td align="left">
                            <asp:TextBox ID="starttime" runat="server"  Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">结束时间：</td>
                        <td align="left">
                            <asp:TextBox ID="endtime"  runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">优惠券到期时间：</td>
                        <td align="left">
                            <asp:TextBox ID="cutTime"  runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">开启时间：</td>
                        <td align="left">
                            <asp:TextBox ID="opentime"  runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">关闭时间：</td>
                        <td align="left">
                            <asp:TextBox ID="closetime"  runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">可领限额：</td>
                        <td align="left">
                            <asp:TextBox ID="daylimit" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" MaxLength="9"  runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">是否生效：</td>
                        <td align="left">
                            <asp:RadioButton ID="ActStatusyes" GroupName="ActStatus" Text="是"  runat="server" />
                            <asp:RadioButton ID="ActStatusno" GroupName="ActStatus" Text="否"  runat="server" />
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