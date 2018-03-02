<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmVotesubjectUpdate.aspx.cs" Inherits="Mozart.CMSAdmin.Vote.wfmVotesubjectUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>类别修改</title>
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
    <script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script src="../script/smart.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../script/jq.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Calendar/WdatePicker.js"></script>

    <script type="text/javascript">
        var i = 0, j = 0;     //行号与列号
        var oNewRow;    //定义插入行对象
        var oNewCell1, oNewCell2;     //定义插入列对象
        //添加条件行
        function AddRow(msg, id, order, del) {
            if (msg == null || msg == "" || msg == "undefined") {
                msg = "";
            }
            if (del == null || del == "" || del == "undefined") {
                del = "";
            } else {
                if (id == null || id == "" || id == "undefined") {
                    del = "<input type='button' name='Del" + j + "' id='Del" + j + "0'  value='删除' onClick='DelCurrentRow(" + j + ",this.id);'>";
                } else {
                    del = "<input type='button' name='Del" + j + "' id='" + id + "' value='删除' onClick='DelCurrentRow(" + j + ",this.id);'>";
                }
            }

            if (j < 6) {
                i = document.all.MyOption.rows.length;
                oNewRow = document.all.MyOption.insertRow(i);
                oNewRow.id = j;
                //添加第一列
                oNewCell1 = document.all.MyOption.rows[i].insertCell(0);
                oNewCell1.innerHTML += "名称：<input type='text' id='Option" + j + "' name='" + id + "' size='14' value=\"" + msg + "\">" +
                    "序号：<input type='text' id='Order" + j + "' name='" + id + "' size='2' value=\"" + order + "\">" + del;
                j++;
            } else {
                j = 6;
                alert("最多能添加6个选项");
            }
        }
        //删除行
        function DelCurrentRow(n, opid) {
            var jj = j - 1;
            var ss = "Del" + jj + "0";
            with (document.all.MyOption) {
                if (rows.length == 2) {
                    j = rows.length;
                    alert('最少保持两个选项');
                } else {
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i].id == n) {
                            if (opid == null || opid == "" || opid == "undefined" || opid == ss || opid == "Del" + n + "0") {
                                deleteRow(i);
                                j--;
                            } else {
                                if (confirm("你确定要删除该项？")) {
                                    $.getJSON("wfmVotesubjectUpdate.aspx", { "action": "delopid", "delopid": opid }, function (json) {
                                        if (json.success == 'true' || json.success == true) {
                                            deleteRow(i);
                                            j--;
                                        }
                                        else {
                                            alert(json.success);
                                        }
                                    });
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        function suboptionlist() {
            var strData = "";
            var list = ""; var listlength = "";
            //对表单中所有的input进行遍历
            var temp = UE.getEditor('myEditor').getContent();//获取编辑器的内容
            var txtName = document.getElementById("txtName");
            var starttime = document.getElementById("starttime");
            var endtime = document.getElementById("endtime");
            try {
                list = document.getElementById("MyOption").getElementsByTagName("input");
                listlength = list.length;
                for (var i = 0; i < listlength && list[i]; i++) {
                    //判断是否为文本框
                    if (list[i].type == "text") {
                        if (list[i].value == null || list[i].value == "" || list[i].value == "undefined") {
                            alert("请输入相应的信息后再操作！"); list[i].id.focus(); break;
                        } else {
                            if (i == 0) {
                                strData += "?";
                            } else {
                                strData += "&";
                            }
                            strData += list[i].id + "ID" + "=" + list[i].name;
                            strData += "&" + list[i].id + "=" + list[i].value;
                        }
                    }
                }
            } catch (e) {
            }

            if (txtName.value == null || txtName.value == "") {
                alert("请输入相关名称"); document.getElementById("txtName").focus(); return false;
            } else {
                $.getJSON("wfmVotesubjectUpdate.aspx" + strData, { "action": "save", "upId": "<%=Mozart.Common.Common.NoHtml(Request.QueryString["id"])%>", "txtName": txtName.value, "Content": temp, "BeginTime": starttime.value, "endtime": endtime.value, "listlength": listlength}, function (json) {
                    if (json.success == 'true' || json.success == true) {
                        alert('修改成功');
                    } else {
                        alert(json.success);
                    }
                })
            }
            return false;
        }
    </script>

</head>
<body bgcolor="#EDF3FD" style="margin-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:HiddenField ID="hd_content" runat="server" Value="" />
        <div style="padding: 20px,0,0,20px;">
            <input id="txtID" type="hidden" runat="server" value="" />
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <table style="height: 345px">
                    <tr>
                        <td align="right">投票名称：</td>
                        <td align="left">
                            <asp:TextBox ID="txtName" runat="server" width="800px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">详细说明：</td>
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
                    <%if (Session["strSiteCode"].ToString().ToLower() != "qjtv")
                      { %>
                    <tr>
                        <td align="right">投票选项：</td>
                        <td align="left">
                            <%if (Mozart.Common.Common.NoHtml(Request["action"])!= "show"){ %>
                            <input type="button" value="添加" name"addFieldBT" onclick="AddRow('','','0','del');"/><br />
                            <%} %>
                            <%=optionlist %>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td align="right"></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return suboptionlist();" CssClass="cssbtn" />
                            <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>