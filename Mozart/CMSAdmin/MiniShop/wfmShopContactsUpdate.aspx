<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmShopContactsUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.MiniShop.wfmShopContactsUpdate" %>
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
    <script src="../script/region_select.js" type="text/javascript" charset="gb2312"></script>
    <style>
select{display: inline-block;font-size: 14px;line-height: 20px;color: #555555;
       -webkit-border-radius: 4px;-moz-border-radius: 4px;border-radius: 4px;vertical-align: middle;
}
</style>
</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">所属店铺：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlcontactlist"  runat="server">
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系人姓名：</td>
                        <td align="left">
                            <asp:TextBox ID="nickname" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系人电话：</td>
                        <td align="left">
                            <asp:TextBox ID="phone" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系人QQ：</td>
                        <td align="left">
                            <asp:TextBox ID="qqnum" runat="server" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^0-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^0-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">电子邮箱：</td>
                        <td align="left">
                            <asp:TextBox ID="email" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">详细地址：</td>
                        <td align="left">
                            <%--<select name="location_p" id="location_p"></select>
                           <select name="location_c" id="location_c"></select>
                           <select name="location_a" id="location_a"></select>
                            <script type="text/javascript">
                                new PCAS('location_p', 'location_c', 'location_a', '', '', '');
                          </script>--%>
                            <input type="text" id="address" style="width:450px;" runat="server"/><font color="red">*</font>
                        </td>
                    </tr>
                    <tr>
					<td align="right" class="td1">是否为默认地址：</td>
					<td align="left">
                        <asp:RadioButton Text="是" ID="isyes"  GroupName="isstate" runat="server" />
                        <asp:RadioButton ID="isno" Text="否" GroupName="isstate" runat="server" />
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
    <script type="text/javascript">
        function isEmail() {
            //var select_p = document.getElementById("location_p");
            //var index_p = select_p.selectedIndex;
            //var text_p = select_p.options[index_p].text;
            //var value_p = select_p.options[index_p].value;
            //var select_c = document.getElementById("location_c");
            //var index_c = select_c.selectedIndex;
            //var text_c = select_c.options[index_c].text;
            //var value_c = select_c.options[index_c].value;
            //var select_a = document.getElementById("location_a");
            //var index_a = select_a.selectedIndex;
            //var text_a = select_a.options[index_a].text;
            //var value_a = select_a.options[index_a].value;
            var address = $("#address").val();
            if (address != null && address != "") {
                //document.getElementById("address").value = "";
                //$("#address").val(value_p + "-" + value_c + "-" + value_a + "-" + address);
            } else {
                document.getElementById("address").value = "";
                document.getElementById("address").focus();
                alert("请输入您店铺的详细地址");
                return false;
            }
            var strEmail = document.getElementById("email").value;
            if (strEmail != null && strEmail != "") {
                if (strEmail.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) == -1) {
                    alert("Email格式不正确！");
                    document.getElementById("email").value = "";
                    document.getElementById("email").focus();
                    return false;
                }
            }
        }
 </script>
</body>
</html>