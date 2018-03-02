<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmCreatePrintCode.aspx.cs" Inherits="Mozart.CMSAdmin.HappyPhoto.wfmCreatePrintCode" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>打印码管理</title>
	<link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
	<script src="../script/jquery-1.7.2.min.js" type="text/javascript"></script>
	<script src="../script/lhgdialog.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	    function CreatePrintCode(strid) {
	        $.dialog({
	            id: 'CreatePrintCode',
	            title: '生成打印码',
	            width: '1000px',
	            height: '600px',
	            content: 'url:wfmCreateCodePrint.aspx?id=' + strid,
	            lock: true,
	            close: function () {
	                __doPostBack('btnQuery', '');
	            }
	        });
	    }

	    function AdminBizControlConfirm(strURL) {
	        $.dialog({
	            content: '确定重置？',
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
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
	<form id="form1" runat="server">
	<div style = "padding:20px,0,0,20px; ">
<%--		<div>
			<h2>网点信息明细查询</h2>
		</div>--%>
		<div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
		<asp:Panel ID="Panel1" runat="server" Height="31px" HorizontalAlign="Center">
			<table>
				<tr>
					<td>打印码：</td>
					<td>
                        <asp:TextBox ID="txtLoginName" runat="server" Width="80px" />
					</td>
					<td>状态：</td>
					<td>
						<asp:DropDownList ID="ddlState" runat="server" Width="84px">
						</asp:DropDownList>
					</td>
					<td>
						<asp:Button ID="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click" />
					</td>
                    <td>
                        &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                    </td>
                    <td>打印终端编码：</td>
					<td>
						<asp:DropDownList ID="ddlClientID" runat="server" Width="84px">
						</asp:DropDownList>
					</td>
                    <td>打印码生成数量：</td>
					<td>
                        <asp:TextBox ID="txtAmount" runat="server" Width="80px" >0</asp:TextBox>
					</td>
					<td>
                        <asp:Button ID="btnCreatePrintCode" runat="server" Text="生成打印码" OnClick="btnCreatePrintCode_Click" />
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
						<th>打印码</th>
                        <th>终端ID</th>
                        <th>开始时间</th>
                        <th>结束时间</th>
                        <th>生成时间</th>
                        <th>状态</th>
						<th>操作</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
					<tr style="background-color:#FAF3DC">
						<%--<td><%#DataBinder.Eval(Container.DataItem, "ID")%></td> --%>   
                        <td><asp:Label ID="no" runat="server" Text=""></asp:Label></td>                   
						<td><%#DataBinder.Eval(Container.DataItem, "PrintCode")%></td>
						<td><%#DataBinder.Eval(Container.DataItem, "ClientID")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Start")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "End")%></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Create")%></td>
						<td><%#DataBinder.Eval(Container.DataItem, "State")%></td>				
						<td><a href="#" onclick="AdminBizControlConfirm('wfmPrintCodeAdmin.aspx?action=reset&id=<%#Eval("ID")%>');">重置</a></td>
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
		CustomInfoHTML="第<font color='red'><b>%currentPageIndex%</b></font>页，共%PageCount%页，每页%PageSize%条记录"         
		>
	</webdiyer:AspNetPager>  
	</div>
	</form>
</body>
</html>