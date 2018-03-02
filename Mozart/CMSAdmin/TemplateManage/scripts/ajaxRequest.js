
var AjaxRequest = {

    //异步刷新表单token,防止重复提交
    getToken: function () {
        AjaxRequest.urlRequest("/token.action", "token-session");
    },

    //url请求
    urlRequest: function (requestUrl, updatePanel, callback, reset) {
        AjaxRequest.beforeRequest();
        $.ajax({
            url: requestUrl,
            type: "POST",
            success: function (responseText, textStatus) {
                AjaxRequest.onSuccess(responseText, textStatus, updatePanel, callback, reset);
            }
        });
    },

    //打开子页面
    openUrl: function (requestUrl, width, height, title) {
        AjaxRequest.beforeRequest();
        $.ajax({
            url: requestUrl,
            type: "POST",
            success: function (responseText) {
                AjaxRequest.afterRequest();
                ZMessage.Page(responseText, width, height, title);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                AjaxRequest.onFailure(XMLHttpRequest);
            }

        });
    },


    //表单请求
    formRequest: function (form, updatePanel, callback, reset) {
        AjaxRequest.beforeRequest();
        var options = {
            type: "POST",
            success: function (responseText, textStatus) {
                AjaxRequest.onSuccess(responseText, textStatus, updatePanel, callback, reset);
            }
        };
        $("#" + form).ajaxSubmit(options);//ajax提交
    },


    //请求前操作
    beforeRequest: function () {
        loading_open();
    },

    //请求后操作
    afterRequest: function (data) {
        loading_close();
    },


    //form请求   成功处理
    onSuccess: function (responseText, textStatus, updatePanel, callback, reset) {//成功
        if (responseText.errorMsg) {//是否存在错误信息
            AjaxRequest.onComplete("error", callback);
            AjaxRequest.onFailure(responseText);
        } else {
            if (updatePanel && responseText) {
                $("#" + updatePanel).html(responseText);
            }
            AjaxRequest.onComplete(textStatus, callback, responseText);
        }
    },

    //失败处理
    onFailure: function (data) {
        AjaxRequest.afterRequest();
        if (data.errorShowType == "local") {
            if (data.errorTag) {
                $("#" + data.errorTag).html("");//清除错误消息
                $("#" + data.errorTag).append(data.errorMsg);//添加错误
            } else {
                return;
            }

        } else {
            ZMessage.Alert(data.errorMsg);
        }

    },

    //完成处理,主要用于操作回调函数
    onComplete: function (textStatus, callback, responseText) {
        AjaxRequest.afterRequest();
        resetSubmenuHeight();
        if (textStatus == "success") {
            flag = true;
        } else {
            flag = false;
        }
        if (callback) {
            callback(flag, responseText);//回调函数
        }
    }
};