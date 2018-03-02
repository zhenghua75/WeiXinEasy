<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfmKeywordMsgVoicePlay.aspx.cs" Inherits="Mozart.CMSAdmin.WeiXin.wfmKeywordMsgVoicePlay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <embed  src='<% = mediaFile %>'  id="wavTest"  hidden="false"  height="200"  width="450"  autostart="false"  type="audio/wav"  loop="true"> 
        </embed>
    </div>
    </form>
</body>
</html>
