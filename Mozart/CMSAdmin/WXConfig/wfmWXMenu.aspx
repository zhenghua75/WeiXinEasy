<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmWXMenu.aspx.cs" Inherits="Mozart.CMSAdmin.WXConfig.wfmWXMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>微信菜单管理</title>
    <link rel="stylesheet" href="../style/demo.css" type="text/css" />
	<link rel="stylesheet" href="../style/zTreeStyle/zTreeStyle.css" type="text/css" />
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script type="text/javascript" src="../script/jquery-1.4.4.min.js"></script>
	<script type="text/javascript" src="../script/jquery.ztree.core-3.5.js"></script>   
    <style type="text/css">
        .ztree li span.button.switch.level0 {visibility:hidden; width:1px;}
        .ztree li ul.level0 {padding:0; background:none;}
	</style>
</head>
<body style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px; background-color:#F5F5F5">
    <form id="form1" runat="server">
        <div style = "padding:20px,0,0,20px; ">
		    <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
			    <h2>微信菜单管理</h2>
		    </div>
            <div class="content_wrap">
                <div class="zTreeDemoBackground left">
                    <ul id="tree" class="ztree">
                    </ul>
                </div>
                <div class="right" style="margin-top: 10px;">
                    <table style="height: 100px;width:818px ;border:solid 1px;">
                    <tr>
                        <td style="text-align:right; width:100px">菜单名称：</td>
                        <td style="text-align:left;">
                            <asp:TextBox ID="txtButtonName" name="txtButtonName" runat="server"></asp:TextBox>
                        </td>
                    </tr>   
                    <tr>
                        <td style="text-align:right;" >菜单类别：</td>
                        <td style="text-align:left">
                            <asp:DropDownList ID="ddlMeneType" name="ddlMeneType" Width="180px" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                 
                    <tr>
                        <td style="text-align:right;">上级菜单：</td>
                        <td style="text-align:left;">
                            <asp:TextBox id="txtParenMenu" name="txtParenMenu" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;" >按钮类别：</td>
                        <td style="text-align:left">
                            <asp:DropDownList ID="ddlButtonType" Name="ddlButtonType" Width="180px" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">定向范围：</td>
                        <td style="text-align:left">
                            <asp:DropDownList ID="txtRedirectScope" Name="txtRedirectScope" runat="server">
                                <asp:ListItem>否</asp:ListItem>
                                <asp:ListItem>是</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">定向值：</td>
                        <td style="text-align:left">
                            <asp:TextBox id="txtRedirectState" name="txtRedirectState" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">顺序号：</td>
                        <td style="text-align:left">
                             <asp:TextBox onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" ID="txtOrder" Name="txtOrder" runat="server" width="20px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;"></td>
                        <td>
                            <asp:Button ID="btnAdd" name="btnAdd" OnClientClick="return addupdatemenu('add');" runat="server" Text="添加"/> 
                            <asp:Button ID="btnSave" runat="server" OnClientClick="return addupdatemenu('update');" Text="保存"/>
                            <asp:Button ID="btnDel" runat="server" OnClientClick="return addupdatemenu('del');" Text="删除"/>
						    <asp:Button ID="btnReset" runat="server" Text="重置" />
                        </td>
                    </tr>
                </table>
	            </div>
            </div>
        </div>
    </form>
</body>
    <script>
        //初始化树的参数
        var zTreeObj,
        setting = {
            view: {
                expandSpeed: "slow",
                dblClickExpand: dblClickExpand,
                selectedMulti: false
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: 0
                }
            },
            callback: {
                beforeClick: beforeClick,
                onClick: onClick
            }
        };
        var log, className = "dark";
        function beforeClick(treeId, treeNode, clickFlag) {
            //className = (className === "dark" ? "" : "dark");
            //showLog("[ " + getTime() + " beforeClick ]&nbsp;&nbsp;" + treeNode.name + "&nbsp;&nbsp;" + treeNode.id);
            $("#txtButtonName").attr("value", treeNode.name); 
            $("#txtButtonName").attr("data-id", treeNode.id);
            if (treeNode.pId != null && treeNode.pId != "" && treeNode.pId != "0") {
                $("#txtParenMenu").attr("data-id", treeNode.pId);
            } else {
                $("#txtParenMenu").attr("data-id", treeNode.id);
            }
            var menuid = $("#txtButtonName").attr("data-id");
            if (menuid != null && menuid != "") {
                $("#btnDel").attr("disabled", false);
            } else {
                $("#btnDel").attr("disabled", true);
            }
            if (treeNode.level > 1) {
                $("#btnAdd").attr("disabled", true);
            }
            else {
                $("#btnAdd").attr("disabled", false);
            }
            //return (treeNode.click != false);
        }
        function onClick(event, treeId, treeNode, clickFlag) {
            //showLog("[ " + getTime() + " onClick ]&nbsp;&nbsp;clickFlag = " + clickFlag + " (" + (clickFlag === 1 ? "普通选中" : (clickFlag === 0 ? "<b>取消选中</b>" : "<b>追加选中</b>")) + ")");            
        }

        function dblClickExpand(treeId, treeNode) {
            return treeNode.level > 0;
        }
        //页面加载的事件
        $(document).ready(function () {
            GetDatas();
            $("#btnDel").attr("disabled", true);
        });

        //从服务器获取数据
        function GetDatas() {
            $.ajax({
                url: "getWXMenu.ashx", //该页面为处理信息页面，要求页面文件为空。（实际项目中建议用HttpHandler）
                type: "post",                //请求发送方式
                dataType: "json",       //返回值有： text 为纯文本，json对象，html页面
                data: "",   //传递的参数，传递过去后用 Request.Params["name1"]) 取出。
                //请求成功执行
                success: function (data, textStatus) {
                    //data 为返回值,初始化树形控件
                    zTreeObj = $.fn.zTree.init($("#tree"), setting, data);
                    //CheckedNodes();
                }
            });
        }
        $("#txtButtonName").val('');
        function addupdatemenu(action)
        {
            var menuid = $("#txtButtonName").attr("data-id");
            var menuname = $("#txtButtonName").val();
            var selectmenutypevalue = ""; var buttontypevalue = ""; var Redirectvalue = "";
            try {
                var selectmenutypeID = document.getElementById("ddlMeneType");
                var index = selectmenutypeID.selectedIndex;
                var selectmenutypetext = selectmenutypeID.options[index].text;
                selectmenutypevalue = selectmenutypeID.options[index].value;
            } catch (e) {}
            try {
                var buttontypeID = document.getElementById("ddlButtonType");
                var index = buttontypeID.selectedIndex;
                var buttontypetext = buttontypeID.options[index].text;
                buttontypevalue = buttontypeID.options[index].value;
            } catch (e) {}
            try {
                var RedirectID = document.getElementById("txtRedirectScope");
                var index = RedirectID.selectedIndex;
                var Redirecttext = RedirectID.options[index].text;
                Redirectvalue = RedirectID.options[index].value;
            } catch (e) {}
            var Redirectstate = $("#txtRedirectState").val();
            var Ordernum = $("#txtOrder").val();
            var parentMenu = $("#txtParenMenu").attr("data-id");
            if (parentMenu == "0")
            {
                parentMenu = "";
            }
            if (menuid == "0")
            {
                menuid = "";
            }
            if (menuname != null && menuname != "")
            {
                $.getJSON("wfmWXMenu.aspx", {
                    "action": action, "menuname": menuname, "menutype": selectmenutypevalue,
                    "parentmenuid": parentMenu, "buttonmenutype": buttontypevalue, "redirectscope": Redirectvalue,
                    "redirectstate": Redirectstate, "ordernum": Ordernum, "menuid": menuid
                }, function (json) {
                    alert(json.message); return;
                })
            } else {
                if (action == "del") {
                    alert("请选择您要删除的菜单");
                } else {
                    alert("请输入菜单名称"); $("#txtButtonName")[0].focus();
                }
                $("#txtButtonName").val(''); return;
            }
        }
    </script>
</html>
