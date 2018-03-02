<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXJSAPIPayDemo.aspx.cs" Inherits="Mozart.Payment.Demo.WXJSAPIPayDemo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <script type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        手机号码：<asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="直接支付" OnClick="Button1_Click" />
        <br />
        <asp:Label ID="lblDesc" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
