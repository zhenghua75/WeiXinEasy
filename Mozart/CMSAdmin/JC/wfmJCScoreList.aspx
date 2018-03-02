<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmJCScoreList.aspx.cs" Inherits="Mozart.CMSAdmin.JC.wfmJCScoreList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>竞猜管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
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
			<h2>竞猜结果信息管理</h2>
		</div>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>赛事名称：</td>
					<td>
						<asp:DropDownList ID="ddlselectName"  runat="server" >
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
					</td>
                    <td>是否猜对输赢：</td>
					<td>
						<asp:DropDownList ID="ddlselectlosewin"  runat="server" >
                            <asp:ListItem Text ="--全部--" Value ="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text ="是" Value ="1"></asp:ListItem>
                            <asp:ListItem Text ="否" Value ="2"></asp:ListItem>
                        </asp:DropDownList>
					</td>
                    <td>是否猜对比分：</td>
					<td>
						<asp:DropDownList ID="ddlselectscore"  runat="server">
                            <asp:ListItem Text ="--全部--" Value ="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text ="是" Value ="1"></asp:ListItem>
                            <asp:ListItem Text ="否" Value ="2"></asp:ListItem>
                        </asp:DropDownList>
					</td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />
                        <asp:Button ID="btnCoupon" runat="server" Text="奖励发放" OnClick="btnCoupon_Click" />
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
                        <th>OpenID</th>
						<th>赛事名称</th>
                        <th>开始时间</th>
                        <th>主队</th>
                        <th>客队</th>
                        <th>竞猜比分</th>
                        <th>正确比分</th>
                        <th>胜负</th>
                        <th>是否猜对比分</th>
                        <th>竞猜时间</th>
                        <th>奖励发放</th>
                        <th>管理</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC">  
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td> 
                        <td><%#Eval("OpenID") %></td>                  
						<td><%#Eval("Name") %></td>
                        <td><%#Eval("StartTime") %></td>
                        <td>
                            <ul>
                                <li><asp:Label ID="HomeTeamImg" Text='<%#Eval("HomeTeamImg") %>' runat="server" /></li>
                                <li><%#Eval("HomeTeam") %></li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li><asp:Label ID="VisitingTeamImg" Text='<%#Eval("VisitingTeamImg") %>' runat="server" /></li>
                                <li><%#Eval("VisitingTeam") %></li>
                            </ul>
                        </td>
                        <td><asp:Label Text='<%#Eval("GuessScore") %>' ID="GuessScore" runat="server" /></td>
                        <td><asp:Label Text='<%#Eval("RightScore") %>' ID="RightScore" runat="server" /></td>
                        <td><asp:Label ID="losewin"  runat="server" /></td>
                        <td><asp:Label ID="score"  runat="server" /></td>
                        <td><%#Eval("AddTime") %></td>
                        <td><%#Eval("CouponState") %></td>                      
						<td>
                            <a href="#" onclick="AdminBizControlConfirm('wfmJCScoreAdmin.aspx?action=del&id=<%#Eval("ID") %>');">删除</a>
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
</html>