<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoveRace.aspx.cs" Inherits="Mozart.WXWall.MoveRace" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="style/race.css" type="text/css" media="screen" />
    <script src="script/jquery-2.1.0.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="raceman">
        <div class="startrace">
    <div class="headdiv">微信摇一摇活动</div>
    <div class="centerdesc"></div>
        <div class="startbtn">开始</div>
      </div>
        <div class="racecontent">
            <div class="headdiv">微信摇一摇活动</div>
            <div class="moveracediv">
            <ul class="list" id="list"></ul>
        </div>
        </div>
    </div>
        <div class="mobile">
            <div class="head">拿起您的手机，尽情的摇吧！！！</div>
            <div class="movespeed"></div>
        </div>
        <div id="inputdiv">
            <div>
                <input type="text" class="inputNickName" title="请输入您的称呼" value="请输入您的称呼" onfocus="if(value=='请输入您的称呼') {value=''}" 
                    onblur="if(value=='') {value='请输入您的称呼'}"/> 
                <input type="button" class="inputbtn" value=" 确 定 " />      
            </div>
        </div>
    </form>
    <script src="script/race.js"></script>
</body>
</html>
