﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmLuckyAwardUsers.aspx.cs" EnableEventValidation="false" Inherits="Mozart.CMSAdmin.Game.wfmLuckyAwardUsers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>用户管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Calendar/WdatePicker.js"></script>
	<script type="text/javascript">
	    function AdminBizControlConfirm(strURL) {
	        $.dialog({
	            content: '确定删除该信息？',
	            ok: function () {
	                AdminBizControl(strURL);
	                return true;
	            },
	            cancel: function () {
	                return true;
	            }
	        });
	    }

	    function AdminBizControl(strURL) {//我们就是通过这个函数来异步获取信息的
	        var xmlHttpReq = null; //声明一个空对象用来装入XMLHttpRequest
	        if (window.XMLHttpRequest) {//除IE5 IE6 以外的浏览器XMLHttpRequest是window的子对象
	            xmlHttpReq = new XMLHttpRequest(); //我们通常采用这种方式实例化一个XMLHttpRequest
	        }
	        else if (window.ActiveXObject) {//IE5 IE6是以ActiveXObject的方式引入XMLHttpRequest的
	            xmlHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
	            //IE5 IE6是通过这种方式
	        }
	        if (xmlHttpReq != null) {//如果对象实例化成功 我们就可以干活啦
	            xmlHttpReq.open("get", strURL, true);
	            //调用open()方法并采用异步方式
	            xmlHttpReq.onreadystatechange = RequestCallBack; //设置回调函数
	            xmlHttpReq.send(null); //因为使用get方式提交，所以可以使用null参调用
	        }
	        function RequestCallBack() {//一旦readyState值改变，将会调用这个函数
	            if (xmlHttpReq.readyState == 4) {
	                alert(xmlHttpReq.responseText);
	                //将xmlHttpReq.responseText的值赋给iptText控件
	                __doPostBack('btnQuery', '');
	            }
	        }
	    }
	</script>
    <style>
        ul {list-style:none;padding:0px 0px 0px 0px;margin:0px 0px 0px 0px;}
        ul li {float:left;padding:0px 0px 0px 0px;margin:0px 0px 0px 0px;}
    </style>
</head>
<body bgcolor="#F5F5F5" style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = "padding:20px,0,0,20px; ">
		<div>
			<h2>用户管理</h2>
		</div>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>名称：</td>
					<td>
						<asp:TextBox ID="txtName" runat="server" Width="80px"></asp:TextBox>
					</td>
                    <td align="right">活动名称：</td>
                        <td align="left">
                            <asp:DropDownList ID="ddlactlist" onchange="GetOption(this.id);" runat="server">
                        </asp:DropDownList>
                        </td>
                    <td>奖项：</td>
					<td>
						<asp:DropDownList ID="ddlawardlist" onchange="setvalue(this.id,'setoptionvalue')" runat="server">
                        </asp:DropDownList>
                        <input type="text" name="setoptionvalue" runat="server" style="display:none;" id="setoptionvalue" />
					</td>
                    <td>奖项发放状态：</td>
					<td>
						<asp:DropDownList ID="ddlsendaward"  runat="server">
                            <asp:ListItem Text="--全部--" Value=""></asp:ListItem>
                            <asp:ListItem Text="未发放" Value="0"></asp:ListItem>
                            <asp:ListItem Text="已发放" Value="1"></asp:ListItem>
                        </asp:DropDownList>
					</td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />&nbsp;
                    </td>
				</tr>
			</table>
		</asp:Panel>
	</div>
		<br />
		<div style = "background-color:#EDF3FD; width:100%;">
		<asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" >
			<HeaderTemplate>
				<table width="100%" border="1" cellspacing="0" cellpadding="4" style="border-collapse:collapse">
					<tr>
						<th>序号</th>
                        <th>用户昵称</th>
                        <th>活动名称</th>
						<th>奖项名称</th>
                        <th>用户电话</th>
                        <th>SN码</th>
                        <th>是否发放奖品</th>
                        <th>中奖时间</th>
                        <th>管理</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC"> 
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td>  
                        <td><%#Eval("NickName") %></td> 
                        <td><%#Eval("ActTitle") %></td>                
						<td><%#Eval("Award") %></td>
                        <td><%#Eval("Phone") %></td>
                        <td><%#Eval("SNCode") %></td>
                        <td><asp:Label ID="sendaward" runat="server" Text='<%#Eval("SendAward") %>'></asp:Label></td>
                        <td><%#Eval("AddTime") %></td>
						<td>
                            <ul>
                                <li><a href="#" onclick="AdminBizControlConfirm('wfmLuckyAwardUsersAdmin.aspx?action=del&id=<%#Eval("ID") %>');">删除</a></li>
                            </ul>
						</td>
					</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
		</div>
		<br />
		<webdiyer:AspNetPager 
		id="AspNetPager1" 
		runat="server" 
		PageSize="12" 
		AlwaysShow="True" 
		OnPageChanged="AspNetPager1_PageChanged" 
		ShowCustomInfoSection="Left" 
		CustomInfoSectionWidth="24%" 
		ShowPageIndexBox="always"         
		TextAfterPageIndexBox="页" 
		TextBeforePageIndexBox="转到第" 
		FirstPageText="【首页】" 
		LastPageText="【尾页】" 
		NextPageText="【后页】" 
		PrevPageText="【前页】" 
		NumericButtonTextFormatString="{0}"         
		CustomInfoHTML="第<font color='red'><b>%currentPageIndex%</b></font>页，共%PageCount%页，每页%PageSize%条记录，共%RecordCount% 条记录"         
		>
	</webdiyer:AspNetPager>  
	</div>
	</form>
</body>
    <script>
        $(document).ready(function () {
            $("#ddlawardlist").empty();
            document.getElementById("ddlawardlist").options.add(new Option("--全部--", ""));
        })
        function GetOption(id)
        {
            $("#ddlawardlist").empty();
            document.getElementById("ddlawardlist").options.add(new Option("--全部--", ""));
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value == 0 || value == "0") { }
            else {
                $.getJSON("wfmLuckyAwardUsers.aspx", { "action": "getoption", "subid": value }, function (json) {
                    $.each(json, function (i, item) {
                        var optionID = item.ID;
                        var optionTitle = item.Award;
                        document.getElementById("ddlawardlist").options.add(new Option("" + optionTitle + "", "" + optionID + ""));
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
    </script>
</html>