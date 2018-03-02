<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlPanel.aspx.cs" Inherits="Mozart.CMSAdmin.ControlPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="script/jquery-1.2.6.pack.js"></script>
<script src="script/jquery.messager.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
       <script>
           var altmsg = '<%=msghtml%>';
           if (altmsg != null && altmsg != "")
           {
               $.messager.lays(300, 200);
               $.messager.show(0, altmsg);
           }
       </script>
    </form>
</body>
</html>
