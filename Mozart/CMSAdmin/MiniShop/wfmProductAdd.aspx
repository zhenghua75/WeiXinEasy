<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="wfmProductAdd.aspx.cs" Inherits="Mozart.CMSAdmin.MiniShop.wfmProductAdd" %>
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
                        <td align="right">所属店铺：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlshoplist" onchange="GetOption(this.id);" runat="server">
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">所在类别：</td>
                        <td align="left">
                           <asp:DropDownList ID="ddlsortlist" onchange="setvalue(this.id,'setoptionvalue')" runat="server">
                        </asp:DropDownList>
                        <input type="text" name="setoptionvalue" runat="server" style="display:none;" id="setoptionvalue" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">商品名称：</td>
                        <td align="left">
                           <asp:TextBox ID="pname" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">商品价格：</td>
                        <td align="left">
                            ￥<asp:TextBox ID="price" runat="server" onkeyup="clearNoNum(this)"></asp:TextBox>元
                        </td>
                    </tr>
                    <tr>
					<td align="right" class="td1">是否二手：</td>
					<td align="left">
                        <asp:RadioButton Text="是" ID="isstateyes" GroupName="isstate" runat="server" />
                        <asp:RadioButton ID="isstateno" Text="否" Checked="true" GroupName="isstate" runat="server" />
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
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" OnClientClick="return getContent();" CssClass="cssbtn" /> 
                            <script type="text/javascript">
                                function getContent() {
                                    var temp = UE.getEditor('myEditor').getContent();
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
    <script>
        $(document).ready(function () {
            $("#ddlsortlist").empty();
            document.getElementById("ddlsortlist").options.add(new Option("--请选择相应的店铺--", ""));
        })
        function GetOption(id) {
            $("#ddlsortlist").empty();
            document.getElementById("ddlsortlist").options.add(new Option("--请选择相应的类别--", ""));
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value == 0 || value == "0") { }
            else {
                $.getJSON("wfmProductAdd.aspx", { "action": "getoption", "subid": value }, function (json) {
                    $.each(json, function (i, item) {
                        var optionID = item.ID;
                        var optionTitle = item.Cname;
                        document.getElementById("ddlsortlist").options.add(new Option("" + optionTitle + "", "" + optionID + ""));
                    })
                })
            }
        }
        function setvalue(id, setvalueid) {
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value != null && value != "") {
                document.getElementById(setvalueid).value = value;
            }
        }
        function clearNoNum(obj) {
            //先把非数字的都替换掉，除了数字和.
            obj.value = obj.value.replace(/[^\d.]/g, "");
            //必须保证第一个为数字而不是.
            obj.value = obj.value.replace(/^\./g, "");
            //保证只有出现一个.而没有多个.
            obj.value = obj.value.replace(/\.{2,}/g, ".");
            //保证.只出现一次，而不能出现两次以上
            obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
        }
    </script>
</body>
</html>