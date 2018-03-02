var dialogTemplate = '<div  title=""></div>';

var confirmButton = '<div style="clear:both;"></div><div style="float:right;" id="confirmButton" ><div></div></div>';
var okA = '<a href="javascript:void(0);" id="ok" onclick="ZDialog.Close(true);">确定</a>';
var cancelA = '&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);" id="cancel" onclick="ZDialog.Close(false);">取消</a>';
var dialogId = 'dialog-base'; 	//默认div ID

var defaultTitle = "提示";
var defaultWidth = 300;
var defaultHeight = 'auto';
var defaultShowTime = 1000;

var options = {
    autoOpen: true,           //初始化之后，是否立即显示对话框，默认为 true
    closeOnEscape: true,      //当用户按 Esc 键之后，是否应该关闭对话框，默认为 true
    draggable: false,        // 是否允许拖动，默认为 true
    width: 300,
    height: 'auto',
    minHeight: 100,
    minWidth: 150,
    modal: true,    		//是否模式对话框，默认为 false
    title: "提示",    		//对话框的标题，可以是 html串，例如一个超级链接。
    bgiframe: false,
    resizable: false,
    //show : 'puff',
    //hide : 'puff',  //blind,clip,drop,explode,fold,puff,slide,scale,size,pulsate
};

var ZDialog = {
    //div加到载页面中
    init: function () {

        //初始化弹出参数
        options.title = this.title || defaultTitle;
        options.width = this.width || defaultWidth;
        options.height = this.height || defaultHeight;
        options.dialogClass = this.dialogClass || options.dialogClass;//添加样式

        $("body").css("overflow", "hidden");//禁用滚动条
        if (this.dialogDiv[0]) {
            this.dialogDiv.remove();//清除div
        }
        $(dialogTemplate).appendTo("body").attr("id", dialogId).css("display", "none");

        this.dialogDiv = $("#" + dialogId);
        this.dialogDiv.append(this.msg || "提示信息");//如果没有内容则显示   '提示信息'

        if (this.type == 'confirm') {//确定
            this.dialogDiv.append(confirmButton);
            $("#confirmButton").append(okA).append(cancelA);
        } else if (this.type == 'alert') {//提示
            this.dialogDiv.append(confirmButton);
            $("#confirmButton").append(okA);//添加确定按钮
        } else if (this.type == 'children') {
            options.draggable = true;//如果是子页面则弹出层可以拖动
            $("body").css("overflow-y", "auto");//启用y方向滚动条
        } else if (this.type == 'tip') {
            $("body").css("overflow-y", "auto");//启用y方向滚动条
        }

    },

    //打开
    open: function (msg, type, callback, width, height, title, showTime) {
        this.msg = msg;
        this.type = type;
        this.callback = callback;
        this.width = width;
        this.height = height;
        this.title = title;

        this.dialogDiv = $("#" + dialogId);

        ZDialog.init();//初始化

        this.dialogDiv.css("display", "block");//显示div
        this.dialogDiv.dialog(options);//弹框

        //提示框一秒消失
        if (type == 'tip') {
            $(".ui-dialog-titlebar").css("display", "none");
            $(".ui-dialog").css("background", "none");
            $(".ui-dialog").css("box-shadow", "none");
            $(".ui-widget-overlay").css("display", "none");
            setTimeout("ZDialog.Close(true)", showTime || defaultShowTime);
        }
    },

    //关闭窗口
    Close: function (flag) {
        this.dialogDiv.dialog("close");
        $("body").css("overflow", "auto");//启用滚动条
        if (this.callback) {
            this.callback(flag);//回调函数
        }
        if (this.type == 'tip') {
            $(".ui-dialog-titlebar").css("display", "block");
            $(".ui-dialog").css("background", "#F4F4F4");
            $(".ui-dialog").css("box-shadow", "0 0 15px #000000");
            $(".ui-widget-overlay").css("display", "block");
        }
    }

};
//提示消息
var ZMessage = {

    //提示
    Alert: function (msg, width, height, title) {
        ZDialog.open(msg, "alert", null, width, height, title);
    },

    //确定
    Confirm: function (msg, callback, width, height, title) {
        ZDialog.open(msg, "confirm", callback, width, height, title);
    },

    //打开子页面
    Page: function (msg, width, height, title) {
        ZDialog.open(msg, "children", null, width, height, title);
    },

    //提示后消失
    Tip: function (successMsg, failMsg, flag, showTime, callback) {
        if (flag) {
            ZDialog.open('<div class="xk"><p class="bcgg_a"></p><p>' + successMsg + '</p></div>', "tip", callback, null, 200, null, showTime);
        } else {
            ZDialog.open('<div class="xk"><p class="bcgg_b"></p><p>' + failMsg + '</p></div>', "tip", callback, null, 200, null, showTime);
        }
    }

};

