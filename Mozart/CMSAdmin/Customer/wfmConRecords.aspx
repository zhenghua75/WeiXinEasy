<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmConRecords.aspx.cs" Inherits="Mozart.CMSAdmin.Customer.wfmConRecords" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户消费明细查询</title>
    <script src="../script/rl.js" type="text/javascript"></script>
    <link href="../style/MainCss.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#EDF3FD"  style="margin-left: 20px;margin-top: 20px;margin-right: 20px;margin-bottom: 20px;">
    <form id="form1" runat="server">
    <div style = "padding:20px,0px,0px,20px; ">
        <div>
            <h2>用户消费明细查询</h2>
        </div>
        <div style = "background-color:#BED3FE;border:solid 1px #809FB5;">
        <asp:Panel ID="Panel1" runat="server" Height="30px" HorizontalAlign="Left" >
            <table>
                <tr>
                    <td>开始时间：</td>
                    <td>
                        <asp:TextBox ID="txtStartDate" runat="server" onClick="SelectDate(this.name)" Width="80px" >2012-07-10</asp:TextBox>
                    </td>
                    <td>结束时间：</td>
                    <td>
                        <asp:TextBox ID="txtEndDate" runat="server"  onClick="SelectDate(this.name)" width="80px" >2012-07-10</asp:TextBox>
                    </td>                   
                    <td>会员账号：</td>
                    <td>
                        <asp:TextBox ID="txtMemNo" runat="server" Width="80px">13769100359</asp:TextBox>
                    </td>
                    <td>消费金额：</td>
                    <td>
                        <asp:TextBox ID="txtConFee" runat="server" Width="80px">13769100359</asp:TextBox>
                    </td> 
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click" />
                        <asp:Button ID="Button1" runat="server" Text="导出" onclick="btnExport_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
        <br />
        <div style = "background-color:#EDF3FD; width:100%; text-align:left; ">
        <asp:GridView ID="GridView1" runat="server" CssClass="GridViewStyle" 
            BorderStyle="Solid">
            <RowStyle BackColor="#E3EFFF" ForeColor="#4A3C8C" CssClass="gvRow" />
            <HeaderStyle BackColor="#CFE4FF" Font-Bold="True" ForeColor="#000000" CssClass="gvHeader" />
            <AlternatingRowStyle BackColor="#FFFFFF" CssClass="gvAlternatingRow" />
        </asp:GridView>
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
