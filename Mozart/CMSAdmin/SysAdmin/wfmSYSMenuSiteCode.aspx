<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmSYSMenuSiteCode.aspx.cs" Inherits="Mozart.CMSAdmin.SysAdmin.wfmSYSMenuSiteCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>定制菜单管理</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	    <div style = "padding:20px,0,0,20px; ">
        <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#b5d6e6">
                <tr>
                    <td bgcolor="#FFFFFF" width="30%">用户选择</td>
                    <td bgcolor="#FFFFFF" width="30%">菜单列表</td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" width="30%">
                        <asp:DropDownList ID="ddlSiteCode" runat="server" Height="20px" Width="148px"  AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" ></asp:DropDownList>
                    </td>
                    <td bgcolor="#FFFFFF" width="30%">
                        <asp:TreeView ID="tvModel" runat="server" ShowCheckBoxes="All" onclick="postBackByObject(event)"></asp:TreeView>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF">
                        <asp:Label ID="lbNodes" runat="server"></asp:Label>
                    </td>
                    <td bgcolor="#FFFFFF">
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
        </div>
    </form>
</body>
    
<script type="text/javascript">

    function postBackByObject(e) {  //兼容FireFox的写法，FireFox没有window.event.srcElement;
        var o = e.target || window.event.srcElement;
        if (o == null) {
            return;
        }
        if (o.tagName == "INPUT" && o.type == "checkbox") //点击treeview的checkbox是触发
        {
            var d = o.id; //获得当前checkbox的id;
            var e = d.replace("CheckBox", "Nodes"); //通过查看脚本信息,获得包含所有子节点div的id
            var div = window.document.getElementById(e); //获得div对象
            if (div != null)  //如果不为空则表示,存在自节点
            {
                var check = div.getElementsByTagName("INPUT"); //获得div中所有的已input开始的标记
                for (i = 0; i < check.length; i++) {
                    if (check[i].type == "checkbox") //如果是checkbox
                    {
                        check[i].checked = o.checked; //字节点的状态和父节点的状态相同,即达到全选
                    }
                }
                PostParentNode(o);
            }
            else  //点子节点的时候,使父节点的状态改变,即不为全选
            {
                PostParentNode(o);
            }
        }
    }

    function PostParentNode(o) {
        var divid = o.parentElement.parentElement.parentElement.parentElement.parentElement; //子节点所在的div

        var id = divid.id.replace("Nodes", "CheckBox"); //获得根节点的id
        var vCheckBox = window.document.getElementById(id); //父CheckBox,新增递归调用 add bywfz

        var checkbox = divid.getElementsByTagName("INPUT"); //获取所有子节点数
        var s = 0;
        for (i = 0; i < checkbox.length; i++) {
            if (checkbox[i].checked)  //判断有多少子节点被选中
            {
                s++;
            }
        }
        if (s > 0)  //如果全部选中 或者 选择的是另外一个根节点的子节点 ，
        {                               //    则开始的根节点的状态仍然为选中状态
            window.document.getElementById(id).checked = true;
            if (vCheckBox.tagName == "INPUT" && vCheckBox.type == "checkbox") {
                PostParentNode(vCheckBox); //递归调用
            }
        }
        else {                               //否则为没选中状态
            window.document.getElementById(id).checked = false;
            if (vCheckBox.tagName == "INPUT" && vCheckBox.type == "checkbox") {
                PostParentNode(vCheckBox); //递归调用
            }
        }

    }

</script>
</html>