<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxpayDemo.aspx.cs" Inherits="Mozart.Payment.wxpay.wxpayDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
<script type="text/javascript">
    function callpay(){
        WeixinJSBridge.invoke('getBrandWCPayRequest',
            <% =JSAPIParameters %>,function(res){
                WeixinJSBridge.log(res.err_msg);
                //if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                //    alert(res.err_code+res.err_desc+res.err_msg);
                //}
                alert(res.err_code+res.err_desc+res.err_msg);
                // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg 将在用户支付成功后返回 ok，但并不保证它绝对可靠。
                //get_brand_wcpay_request:ok,支付成功
                //get_brand_wcpay_request:cancel,支付过程中用户取消
                //get_brand_wcpay_request:fail，支付失败
            });
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        手机号码：<asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="直接支付" OnClick="Button1_Click" />
        <br />
        <br />
        <asp:Button ID="btnUnifiedOrder" runat="server" OnClick="btnUnifiedOrder_Click" Text="生成预订单处理" />
        <asp:Label ID="lblDesc" runat="server"></asp:Label>
        <br />
        <asp:Button ID="Button2" runat="server" OnClientClick="callpay()" Text="支付" />
        
    </div>
    </form>
</body>
</html>
