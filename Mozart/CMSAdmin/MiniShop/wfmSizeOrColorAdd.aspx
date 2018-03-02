<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmSizeOrColorAdd.aspx.cs" EnableEventValidation="false" Inherits="Mozart.CMSAdmin.MiniShop.wfmSizeOrColorAdd" %>
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
                        <td align="right">选择颜色或尺寸：</td>
                        <td align="left">
                            <asp:DropDownList id="sizeorcolor" onchange="setshohide(this.id);" runat="server">
                                <asp:ListItem Text="颜色" Value="-1" /><asp:ListItem Text="尺寸" Value="1" />
                                <asp:ListItem Text="没有" Value="0" />
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">选择店铺：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlshoplist" onchange="GetSortOption(this.id);" runat="server">
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">选择类别：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlsortlist" onchange="GetProductOption(this.id,'ddlshoplist');setvalue(this.id,'setsortvalue');" runat="server">
                        </asp:DropDownList>
                        <input type="text" runat="server" style="display:none;" id="setsortvalue" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">选择产品：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlproductlist" onchange="setvalue(this.id,'setpvalue')" runat="server">
                        </asp:DropDownList>
                        <input type="text" name="setoptionvalue" runat="server" style="display:none;" id="setpvalue" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">尺寸/颜色名称：</td>
                        <td align="left">
                            <asp:TextBox ID="scname" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="colortr">
                        <td align="right">颜色图像：</td>
                        <td align="left">
                            <div style="overflow: hidden;">
                                <input type="file" id="file0" runat="server" />&nbsp;建议尺寸40x40
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
                        <td align="right">库存：</td>
                        <td align="left">
                            <asp:TextBox ID="stock" runat="server" maxlength="10" 
                                 onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^0-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" 
            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^0-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"></asp:TextBox>
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
    <script>
        $(document).ready(function () {
            $("#ddlsortlist").empty();
            document.getElementById("ddlsortlist").options.add(new Option("--全部--", ""));
            $("#ddlproductlist").empty();
            document.getElementById("ddlproductlist").options.add(new Option("--全部--", ""));
        })
        function GetSortOption(id) {
            $("#ddlsortlist").empty();
            document.getElementById("ddlsortlist").options.add(new Option("--全部--", ""));
            $("#ddlproductlist").empty();
            document.getElementById("ddlproductlist").options.add(new Option("--全部--", ""));
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value == 0 || value == "0") { }
            else {
                $.getJSON("wfmSizeOrColorAdd.aspx", { "action": "getcateoption", "subid": value }, function (json) {
                    $.each(json, function (i, item) {
                        var optionID = item.ID;
                        var optionTitle = item.Cname;
                        document.getElementById("ddlsortlist").options.add(new Option("" + optionTitle + "", "" + optionID + ""));
                    })
                })
            }
        }

        function GetProductOption(id, subid) {
            $("#ddlproductlist").empty();
            document.getElementById("ddlproductlist").options.add(new Option("--全部--", ""));
            var subselect = document.getElementById(subid);
            var subindex = subselect.selectedIndex;
            var subtext = subselect.options[subindex].text;
            var subvalue = subselect.options[subindex].value;

            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value == 0 || value == "0") { }
            else {
                $.getJSON("wfmSizeOrColorAdd.aspx", { "action": "getpoption", "subid": subvalue, "catid": value }, function (json) {
                    $.each(json, function (i, item) {
                        var optionID = item.ID;
                        var optionTitle = item.Ptitle;
                        document.getElementById("ddlproductlist").options.add(new Option("" + optionTitle + "", "" + optionID + ""));
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
        function setshohide(id)
        {
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value == -1 || value == "-1") {
                $("#colortr").show();
            } else {
                $("#colortr").hide();
            }
        }
    </script>
</html>