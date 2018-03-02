﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmSizeOrColorUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.MiniShop.wfmSizeOrColorUpdate" %>
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
    <script src="../script/region_select.js" type="text/javascript" charset="gb2312"></script>
    <style>
select{display: inline-block;font-size: 14px;line-height: 20px;color: #555555;
       -webkit-border-radius: 4px;-moz-border-radius: 4px;border-radius: 4px;vertical-align: middle;
}
input[name^="select"] {display:none;border:0px;}
</style>
</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">选择颜色或尺寸：</td>
                        <td align="left">
                            <asp:DropDownList id="sizeorcolor" onchange="setshohide(this.id);" runat="server">
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">名称：</td>
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
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" OnClientClick="return isEmail();" CssClass="cssbtn" /> 
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
            setshohide("sizeorcolor");
        })
        function setshohide(id) {
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