<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmMenuAdmin.aspx.cs" Inherits="Mozart.CMSAdmin.WXConfig.wfmMenuAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="txtAccessToken" runat="server" Width="729px"></asp:TextBox>
        <asp:Button ID="btnCreateMenu" runat="server" OnClick="btnCreateMenu_Click" Text="创建菜单" />
    
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="库生成菜单" />
    
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
    
    </div>
        <div>
            <asp:TextBox ID="txtResult" runat="server" Width="729px" Height="220px" TextMode="MultiLine"></asp:TextBox>
        </div>
    </form>
</body>
</html>