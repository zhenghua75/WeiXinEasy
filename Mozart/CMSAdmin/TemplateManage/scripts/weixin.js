$(function () {

    loadMenuPreview();
    getKeyWord(); //第一步 获取关键字

});

function CloseWin() //这个不会提示是否关闭浏览器    
{
    window.opener = null;
    //window.opener=top;    
    window.open("", "_self");
    window.close();
}

/*--------微信回复新版-------------------*/

$(function () {

    //文本框获得焦点
    function ui_input_text() {
        $('input:text,textarea').focus(function () {
            if ($(this).val() == $(this).attr('tips')) {
                $(this).val('');
            }
        }).blur(function () {
            if ($(this).val() == '') {
                $(this).val($(this).attr('tips'));
            }
        });
    }
    //详情页1. tab栏切换
    function ui_tabs() {
        var tabs = $('.detail1-tabs-title > li');
        var tabs_cont = $('.detail1-tabs-content > li');
        tabs.children('a').click(function () {
            if (!$(this).parent().hasClass('active')) {
                tabs.removeClass('active');
                $(this).parent().addClass('active');
                var i = tabs.index($(this).parent());
                tabs_cont.removeClass('active');
                $(tabs_cont[i]).addClass('active');
            }
            return false;
        });
    }

    //详情页2 .上一步 下一步切换
    function ui_tabs2() {
        var tabs = $('.huodong > li');
        var index = 0;
        var tabs_cont = $('.detail1-tabs-content > li');
        //dhl test
        //$(tabs_cont[2]).addClass('active');
        //dhl end
        $('#prev').click(function () {
            index--;
            if (index == 0) {
                $('#prev').addClass('disabled');
                $('#prev').hide();
                getKeyWord();
                loadKeyWordPreview();
            }
            $('#next').show();
            $('#send').hide();
            tabs.removeClass('active');
            if (index == 1) {
                $(tabs[index]).addClass('active');
                $(tabs[0]).addClass('active');
            }
            $(tabs[index]).addClass('active');
            tabs_cont.removeClass('active');
            $(tabs_cont[index]).addClass('active');
        });
        $('#next').click(function () {
            index++;
            console.log("index:" + index);
            if (index == 1) {

                if (!vaildKeyWord()) {
                    index--
                    return;
                }
                if (!vaildRepeatKeyWord()) {
                    alert("存在相同的关键字名称！");
                    index--
                    return;
                }
                if (!saveKeyWord()) {
                    index--
                    alert("保存关键字失败");
                    return;
                }

                $('#prev').show();
                showMenuPreview();
                getAttReply();
                getAutoReply();
            }
            if (index == 2) {

                if (!vaildReplyLength()) {
                    index--;
                    return;
                }
                if (!saveReply()) {
                    alert("保存回复失败");
                    index--;
                    return;
                }
                $('#next').hide();
                $('#send').show();
                $('#prev').show();
                getMenu();
            }
            else if (index == 3) {
                return false;

            }
            $('#prev').removeClass('disabled');
            //tabs.removeClass('active');
            $(tabs[index]).addClass('active');
            tabs_cont.removeClass('active');
            $(tabs_cont[index]).addClass('active');

        });


        $('#send').click(function () {

            if (!vaildMenuLength())
                return;

            if (saveMenu()) {
                popAlert('保存成功!', function (res) {
                    var mbFrame = $(window.opener.document).find('#mbFrame');
                    mbFrame.attr('src', mbFrame.attr('src'));
                    if (res) {
                        window.close();
                    }
                });
            } else {
                popAlert('保存失败!');
            }
        });
    }

    ui_tabs2();

    $('.pop-hf b').click(function () {
        $('.pop-hf').hide();
    });

    $('.hf-cancel-btnGreen').click(function () {
        $('.pop-hf').hide();
    });


    //上传微信图片
    $('#upload_weixincover').click(function () {

        var ecid = Request.GetQuery('ecid');
        Request.UploadWeixinFile('#upload_weixincover', 'png', ecid, function (rs) {
            if (rs.code == 1) {
                $('#img_weixincover').attr({
                    src: rs.data.image_url,
                    imageid: rs.data.imageid
                }).show();
            }
            else {
                popAlert(rs.notice);
            }
        });
    }).click();

    //点击添加关键字功能弹出框
    $('#btn-ok-closePop').click(function () {

        var activeLink = $('#pop-tjgn a.active');

        var moduleId = activeLink.attr('moduleid');
        var replytype = 1;
        var servicetype = 1;

        if (moduleId == 5007) {
            servicetype = 2; //自定义文字回复：
            replytype = 2;
        }
        //自定义链接
        var moduleurl = '';
        if (moduleId == 24) {
            moduleurl = $('#moduleurl').val();
            $('#moduleurl').val('');
        }

        //        var ordernum = $('#rTabCont > li.active table tr').length;

        //        var isFunc = !!$('#rTabCont > li.active').has('#func_table').length;

        //        var moduleid = $('#ModuleList a.active').attr('moduleid');

        //        var moduletypeid = $('#ModuleList a.active').attr('moduletypeid');


        //        if ($('.tjgn-h-title').attr('permissions') == 'false') {
        //            popAlert('您暂未购买该功能，如需帮助，请联系管理员。');
        //            return false;
        //        }

        var exit = false;
        if ($('#onlyone').val() == 'true') {

            //判断是否可以重复添加
            $('#func_table tr').each(function (index, element) {
                if ($(this).attr('ModuleID') == moduleId) {
                    exit = true;
                    popAlert('该功能已经配置过， 请勿重复添加！');
                    return false;
                }
            });
        }

        if (exit) {
            return false;
        }

        var tmp = '<tr style="" MsgID="0" LinkUrl="' + moduleurl + '" replytype="' + replytype + '" ServiceType="' + servicetype + '"  ModuleID="' + activeLink.attr('moduleid') + '"  ModuleType="' + activeLink.attr('moduletypeid') + '" BizModuleId="' + activeLink.attr('bizmoduleid') + '"  keyword="' + activeLink.html() + '"   BizModuleName="' + activeLink.html() + '" ><td style="width: 90px;"><input type="text" style="width: 110px;" value="' + activeLink.html() + '"/></td><td style="width: 393px; text-align: center;"> <input type="text" value="" sign="sign" /></td><td style="width: 112px;"><a class="btn_replySet" replytype="' + replytype + '">回复设置</a> </td> <td style="width: 112px;"><a class="btn_del mt-6px"></a></td></tr>';

        //表格
        $('#func_table > tbody').append(tmp);
        $('#pop-tjgn').hide();
    });

    //删除关键字
    $(document).on('click', '#func_table .btn_del', function () {
        var currentBtnDel = $(this);
        var tr = currentBtnDel.parents('tr');
        popConfirm("确认删除该关键字？", function (res) {
            if (res) {
                var msgid = tr.attr("msgid");
                var ecid = Request.GetQuery('ecid');
                if (msgid != 0) {
                    if (confirm("删除关键字同时删除关联该关键字的菜单，是否继续？")) {
                        Request.Sync('/WeiXin/DeleteKeyword?ecid=' + ecid + '&msgid=' + msgid, {
                            'msgid': msgid
                        });
                        tr.remove();
                    }
                } else {
                    tr.remove();
                }
            }
        });
    });

    //输入关键字
    $(document).on('keyup', '#func_table input[sign=sign]', function () {
        $(this).parents('tr').attr("keyword", $(this).val());
    });

    //添加回复
    var tbCurrWxhf = null;
    $(document).on('click', '.btn_replySet', function () {
        var replyType = $(this).attr("replytype");
        if (!$('#tjgnFrame').attr('src')) {
            $('#tjgnFrame').attr('src', $('#pop-tjgn .p-nav > li.active:first ul a').attr('href'));
        }

        var tr = $(this).parents('tr');
        tbCurrWxhf = tr;
        if (replyType == "1") {
            $("#txtTwReplyTitle").val(tr.attr('ReplyImageTitle'));
            $("#txtTwReplySummary").val(tr.attr('Summary'));
            var imgid = tr.attr('ReplyImageID');
            if (imgid != undefined && imgid != 0) {
                $("#img_weixincover").attr("src", tr.attr('ImageUrl')).attr("imageid", tr.attr('ReplyImageID'));
            } else {
                $("#img_weixincover").attr("src", "images/c_bg.jpg").attr("imageid", "");
            }
            $('#pop-twhf').show();
        } else {
            $("#txtWzReplyContent").val(tr.attr('ReplyTxt'));
            $('#pop-wzhf').show();
        }
    });


    //点击“确认”图文回复
    $('#btnTwhfConfirm').click(function () {

        var title = $("#txtTwReplyTitle").val();
        var summary = $("#txtTwReplySummary").val();
        var img = $("#img_weixincover").attr("imageid");
        var imgSrc = $("#img_weixincover").attr("src");
        if (title == "") {
            popAlert('请输入标题!');
            return;
        }
        if (summary == "") {
            popAlert('请输入摘要!');
            return;
        }
        if (img == undefined || img == "") {
            popAlert('请上传图片!');
            return;
        }
        //表格属性值改变
        $(tbCurrWxhf).attr({
            'ReplyImageTitle': title,
            'Summary': summary,
            'ReplyImageID': img,
            'imageurl': imgSrc
        });
        $('#pop-twhf').hide();

        $('#func_table input,#func_table a').removeClass("border_red"); //移除输入标红边框的

    });

    //点击“确认”文字回复
    $('#btnWzhfConfirm').click(function () {

        var content = $("#txtWzReplyContent").val();
        if (content == "") {
            popAlert('请输入回复内容!');
            return;
        }
        //表格属性值改变
        $(tbCurrWxhf).attr({
            'ReplyTxt': content
        });

        $('#pop-wzhf').hide();

        $('#func_table input,#func_table a').removeClass("border_red"); //移除输入标红边框的
    });


    //关注时回复字数输入提示
    $(document).on('keyup', '#txtAttReply', function () {
        wordTip(1);
    });
    //自动回复字数输入提示
    $(document).on('keyup', '#txtAutoReply', function () {
        wordTip(2);
    });
});



//获取关键字列表
function getKeyWord() {

    var ecid = Request.GetQuery('ecid');
    var result = Request.Sync('/CMSAdmin/TemplateManage/GetKeyword.ashx?ecid=' + ecid, {
        'ecid': ecid
    });

    if (result.code == 1) {
        var tmp = [
					'<tr title="{showbizmodulename}"  style="" LinkUrl="{LinkUrl}" ServiceType="{ServiceType}" MsgID="{MsgID}" ModuleID="{ModuleID}" ModuleType="{ModuleType}" BizModuleId="{BizModuleId}" replyimageid="{ReplyImageID}" keyword="{KeyWord}" ReplyImageTitle="{ReplyImageTitle}" ReplyType="{ReplyType}"  Summary="{Summary}"  ReplyTxt="{ReplyTxt}" ImageUrl="{ImageUrl}" BizModuleName="{BizModuleName}" ><td style="width: 90px;"><input funcname="funcname" type="text" style="width: 110px;" value="{BizModuleName}" /></td><td style="width: 393px; text-align: center;"> <input sign="sign" type="text" value="{KeyWord}" /></td><td style="width: 112px;"><a class="btn_replySet" replytype="{ReplyType}">回复设置</a> </td> <td style="width: 112px;"><a class="btn_del mt-6px"></a></td></tr>'
        ];

        //表格
        $('#func_table > tbody').html(
				Request.formats(tmp[0], result.data)
			);
    }

    loadKeyWordPreview();
}

//保存关键字
function saveKeyWord() {

    var ecid = Request.GetQuery('ecid');
    var keywordlist = [];
    $('#func_table tr').each(function (index, element) {

        keywordlist.push({
            'MsgID': $(this).attr('MsgID'),
            'BizModuleId': $(this).attr('BizModuleId') || 0,
            'ECID': ecid,
            'ModuleID': $(this).attr('ModuleID') || 0,
            'ModuleType': $(this).attr('ModuleType') || 0,
            'ServiceType': $(this).attr('ServiceType'),
            'KeyWord': $(this).find("input[type=text]").last().val(),
            'ReplyType': $(this).attr('ReplyType'),
            'ReplyTxt': $(this).attr('ReplyTxt'),
            'ReplyImageID': $(this).attr('ReplyImageID') || 0,
            'ReplyImageTitle': $(this).attr('ReplyImageTitle') || "",
            'Summary': $(this).attr('Summary') || "",
            'BizModuleName': $(this).find("input[type=text]").first().val(),
            'LinkUrl': $(this).attr('LinkUrl') || ""
        });
    });

    //    var ecid = Request.GetQuery('ecid');
    //    $('#func_table > tbody > tr').attr("ecid", ecid);
    //    var keyWord = Request.GetAttrMaps($('#func_table > tbody > tr'));

    var result = Request.Sync('/CMSAdmin/TemplateManage/SaveKeyWord.ashx?ecid=' + ecid, keywordlist);

    if (result.code == 1) {
        return true;
    } else {
        return false;
    }
}

//获取关注时回复
function getAttReply() {

    var ecid = Request.GetQuery('ecid');
    var result = Request.Sync('/CMSAdmin/TemplateManage/GetAutoReply.ashx?service_type=3&ecid=' + ecid, {
        'ecid': ecid,
        'service_type': "3"
    });

    $("#txtAttReply").attr("msgid", "0");
    if (result.code == 1) {
        if (result.data != null) {
            $("#txtAttReply").val(result.data.ReplyTxt).attr("msgid", result.data.MsgID);
            wordTip(1);
        }

    }


}
//获取自动回复
function getAutoReply() {

    var ecid = Request.GetQuery('ecid');
    var result = Request.Sync('/CMSAdmin/TemplateManage/GetAutoReply.ashx?service_type=4&ecid=' + ecid, {
        'ecid': ecid,
        'service_type': "4"
    });

    $("#txtAutoReply").attr("msgid", "0");
    if (result.code == 1) {
        if (result.data != null) {
            $("#txtAutoReply").val(result.data.ReplyTxt).attr("msgid", result.data.MsgID);
            wordTip(2);
        }
    }
}

//保存回复
function saveReply() {

    var ecid = Request.GetQuery('ecid');

    var reply = [];
    reply.push({
        'MsgID': $("#txtAttReply").attr("msgid"),
        'ecid': ecid,
        'ServiceType': "3",
        'ReplyTxt': $("#txtAttReply").val()
    });
    reply.push({
        'MsgID': $("#txtAutoReply").attr("msgid"),
        'ecid': ecid,
        'ServiceType': "4",
        'ReplyTxt': $("#txtAutoReply").val()
    });

    var result = Request.Sync('/CMSAdmin/TemplateManage/SaveKeyWord.ashx?service_type=3&ecid=' + ecid, reply);

    return result.code == 1;
}


//保存菜单
function saveMenu() {

    var ecid = Request.GetQuery('ecid');
    var menulist = [];
    $('#wx-table tr').each(function (index, element) {
        var type = $(this).attr('type');
        menulist.push({
            'bizmoduleid': $(this).attr('bizmoduleid') || 0,
            'bizmodulename': type == 2 ? '' : $(this).find('.wx-menuLink').siblings('span').html(),
            'linkurl': type == 2 ? $(this).find('.wx-menuLink').val() : '',
            'menulevel': $(this).attr('menulevel'),
            'menuname': $(this).find('td:first input').val(),
            'moduleid': $(this).attr('moduleid') || 0,
            'moduletypeid': $(this).attr('moduletypeid') || 0,
            'type': type,
            'ServiceMsgId': type == 2 ? '0' : $(this).find('select[type=sKeyWord]').val()
        });
    });

    var result = Request.Sync('/CMSAdmin/TemplateManage/SaveMenu.ashx?ecid=' + ecid, menulist);

    return result.code == 1;
}


function getMenu(ecid, template_id) {

    var ecid = Request.GetQuery('ecid');
    var result = Request.Sync('/CMSAdmin/TemplateManage/GetMenu.ashx?ecid=' + ecid, {
        'ecid': ecid
    });


    if (result.code == 1) {

        var html = [];
        $.each(result.data, function () {

            if (this.MenuLevel == 1) {
                //html.push(Request.format('<tr bizmoduleid="{BizModuleID}" menulevel="{MenuLevel}" moduleid="{ModuleID}" moduletypeid="{ModuleTypeId}" type="{Type}"><td style="width:200px;"><input class="wx-menuMain" type="text" value="{MenuName}"><a class="btn-add"></a></td><td><select type="fselect"><option selected>链接</option><option>业务模块</option></select><input class="wx-menuLink" type="text" value="{LinkUrl}" style="margin-left:10px;"><a class="btn-add2" style="display:none;"></a><span style="display:none; margin-left:50px;">{BizModuleName}</span></td><td style="width:60px;"><a class="btn_del"></a></td></tr>', this));
                html.push(Request.format('<tr ServiceMsgId="{ServiceMsgId}"  bizmoduleid="{BizModuleID}" menulevel="{MenuLevel}" moduleid="{ModuleID}" moduletypeid="{ModuleTypeId}" type="{Type}"><td style="width:200px;"><input class="wx-menuMain" type="text" value="{MenuName}"><a class="btn-add"></a></td><td><select type="fselect"><option selected>链接</option><option>关键字</option></select><input class="wx-menuLink" type="text" value="{LinkUrl}" style="margin-left:10px;" placeholder="请输入以http://开头的链接地址" ><select type="sKeyWord"  style="display:none; margin-left:50px;"></select></td><td style="width:60px;"><a class="btn_del"></a></td></tr>', this));
            }
            else {
                //html.push(Request.format('<tr bizmoduleid="{BizModuleID}" menulevel="{MenuLevel}" moduleid="{ModuleID}" moduletypeid="{ModuleTypeId}" type="{Type}"><td><span class="q4"></span><input class="wx-menuItem" type="text" value="{MenuName}"></td><td><select id="fselect"><option selected>链接</option><option>业务模块</option></select><input class="wx-menuLink" type="text" value="{LinkUrl}" style="margin-left:10px;"><a class="btn-add2" style="display:none;"></a><span style="display:none; margin-left:50px;">{BizModuleName}</span></td><td style="width:60px;"><a class="btn_del"></a></td></tr>', this));
                html.push(Request.format('<tr ServiceMsgId="{ServiceMsgId}" bizmoduleid="{BizModuleID}" menulevel="{MenuLevel}" moduleid="{ModuleID}" moduletypeid="{ModuleTypeId}" type="{Type}"><td><span class="q4"></span><input class="wx-menuItem" type="text" value="{MenuName}"></td><td><select type="fselect"><option selected>链接</option><option>关键字</option></select><input class="wx-menuLink" type="text" value="{LinkUrl}" style="margin-left:10px;" placeholder="请输入以http://开头的链接地址" ><select type="sKeyWord"  style="display:none; margin-left:50px;"></select></td><td style="width:60px;"><a class="btn_del"></a></td></tr>', this));
            }

        });

        var trs = $('#wx-table > tbody').html(html).children('tr');

        var resultKeyWord = Request.Sync('/CMSAdmin/TemplateManage/GetKeyword.ashx?ecid=' + ecid, {
            'ecid': ecid
        });

        for (var i = 0, len = trs.length; i < len; i++) {

            var tr = $(trs[i]);
            tr.find('td > select[type=sKeyWord]').html(Request.formats('<option value="{MsgID}">{KeyWord}</option>', resultKeyWord.data));
            //根据type来选择选中
            if (tr.attr('type') == 1) {
                //选组合框第二个值
                tr.find('td > select[type=fselect] > option').removeAttr('selected').last().attr('selected', 'selected');
                //tr.find('td > select[type=sKeyWord]').val(tr.attr("ServiceMsgId"));
                tr.find('td > select[type=sKeyWord]').find("option[value='" + tr.attr("ServiceMsgId") + "']").attr("selected", true);
                tr.find('td:nth-child(2) > input').hide().siblings().show();
                //                if (tr.attr('menulevel') != 1) {//子才显示
                //                    tr.find('td:nth-child(2) > input').hide().siblings().show();
                //                }
            }

            //如果当前level=1，并且下一级为level=2 或 无 ，那么说明有子菜单
            if (tr.attr('menulevel') == 1 && i + 1 != len) {

                if ($(trs[i + 1]).attr('menulevel') != 1) {
                    //隐藏第二个td
                    tr.find('td:nth-child(2) > *').hide();
                }

            }
        }




    }

}

function vaildKeyWord() {
    var lasttr = $('#func_table tr').last();

    $('#func_table input,#func_table a').removeClass("border_red");//移除输入标红边框的

    if (lasttr.length != 0) {
        var replyType = lasttr.attr("replytype");
        var bizmodulename = lasttr.find("input[type=text]").first().val();
        var keyword = lasttr.find("input[type=text]").last().val();

        if (bizmodulename == undefined || bizmodulename == "") {
            popAlert("请输入已添加项的功能名称！");
            lasttr.find("input[type=text]").first().addClass("border_red");
            return false;
        }

        if (keyword == undefined || keyword == "") {
            popAlert("请输入已添加项的关键字！");
            lasttr.find("input[type=text]").last().addClass("border_red");
            return false;
        }

        if (replyType == 1) {
            if (lasttr.attr("replyimagetitle") == undefined || lasttr.attr("replyimagetitle") == "") {
                popAlert("请点击'回复设置' 输入已添加项的回复内容！");
                lasttr.find("a[class=btn_replySet]").last().addClass("border_red");
                return false;
            }
        } else {
            if (lasttr.attr("ReplyTxt") == undefined || lasttr.attr("ReplyTxt") == "") {
                popAlert("请点击'回复设置' 输入已添加项的回复内容！");
                lasttr.find("a[class=btn_replySet]").last().addClass("border_red");
                return false;
            }
        }

    }

    return true;
}

//关键字重复验证
function vaildRepeatKeyWord() {
    var isVaild = true;
    var hash = {};
    $('#func_table input').removeClass("border_red");

    $('#func_table tr').each(function (index, element) {
        var keyword = $(this).find('input[sign=sign]').val();
        if (hash[keyword]) {
            $(this).find('input[sign=sign]').addClass("border_red");
            isVaild = false;
            return;
        }
        hash[keyword] = true;
    });

    return isVaild;
}
//回复文字长度限制
function vaildReplyLength() {
    var attreply = $("#txtAttReply").val();
    var autoreply = $("#txtAutoReply").val();

    if (attreply == "") {
        popAlert("关注时回复不能为空！");
        return false;
    }

    if (GetWordLength(attreply) > 300) {
        popAlert("关注时回复内容不能大于300字符！");
        return false;
    }

    if (autoreply != "" && GetWordLength(autoreply) > 300) {
        popAlert("自动回复内容不能大于300字符！");
        return false;
    }

    return true;
}
//验证菜单输入长度
function vaildMenuLength() {


    $('#wx-table input').removeClass("border_red")
    var isVaild = true;
    $('#wx-table tr').each(function (index, element) {
        var menuLevel = $(this).attr('menulevel');
        var menuName = $(this).find('td:first input').val();
        var link = $(this).find('.wx-menuLink');

        if (link.is(":visible")) {
            if ($.trim(link.val()) == "") {
                popAlert("链接地址不能为空！");
                isVaild = false;
                link.addClass("border_red");
                return false;
            }
        }

        if (menuName == "") {
            popAlert("菜单名称不能为空！");
            isVaild = false;
            $(this).find('td:first input').addClass("border_red");
            return false;
        }
        var wordLen = GetWordLength(menuName);
        if (menuLevel == 1) {
            if (wordLen > 8) {
                popAlert("一级菜单名称不能超过8个字符！");
                isVaild = false;
                $(this).find('td:first input').addClass("border_red");
                return false;
            }
        } else {
            if (wordLen > 14) {
                popAlert("二级菜单名称不能超过14个字符！");
                isVaild = false;
                $(this).find('td:first input').addClass("border_red");
                return false;
            }
        }
    });

    return isVaild;
}


//加载关键字预览页面
function loadKeyWordPreview() {
    $('#ylFrame').load(function () {
        var doc = $(this.contentDocument);
        //首次关注
        doc.find('.keywordreply').show();
        doc.find('.menucontent').hide();

    }).attr('src', 'weixin.html');
}

//显示菜单预览页面
function showMenuPreview() {
    $('#ylFrame').load(function () {
        var doc = $(this.contentDocument);
        //首次关注
        doc.find('.keywordreply').hide();
        doc.find('.menucontent').show();

    }).attr('src', 'weixin.html');
}
function loadReplyPreview() {

}

//加载菜单预览页面
function loadMenuPreview() {
    $('#ylFrame').load(function () {
        var doc = $(this.contentDocument);

        doc.find('.keywordreply').hide();
        doc.find('.menucontent').show();

        var ecid = Request.GetQuery('ecid');
        var resultLogo = Request.Sync('/CMSAdmin/TemplateManage/GetLogoImage.ashx?ecid=' + ecid, { 'ecid': ecid });
        if (resultLogo.code == 1) {
            doc.find('#imgWeixinLog').attr("src", resultLogo.data.image_url); //设置微信logo
        }

        doc.find('#firstreply').html($('#txtAttReply').val()); //设置首次关注
        var result = Request.Sync('/CMSAdmin/TemplateManage/GetMenu.ashx?ecid=' + ecid, {
            'ecid': ecid
        });
        //设置菜单
        var count = 0;
        for (var i = 0, len = result.data.length; i < len; i++) {
            var x = result.data[i];
            //主菜单	
            if (x.MenuLevel == 1) {
                count++;
                doc.find('#menu li:eq(' + count + ') span').html(x.MenuName);
            }
            else {
                doc.find('#submenu' + count).append('<li>' + x.MenuName + '</li>');
            }
        }

    }).attr('src', 'weixin.html');
}



//字数提示
function wordTip(replyType) {

    //replyType=1 关注时回复 replyType=2 自动回复
    var content = "";
    if (replyType == 1) {
        content = $("#txtAttReply").val();
        var wordLength = GetWordLength(content);
        $("#wordAttReplyCount").html(300 - parseInt(wordLength));
    } else {
        content = $("#txtAutoReply").val();
        var wordLength = GetWordLength(content);
        $("#wordAutoReplyCount").html(300 - parseInt(wordLength));
    }
}

function GetWordLength(sStr) {
    //    var i, strTemp;
    //    var iCount = 0;

    //    for (i = 0; i < sStr.length; i++) {
    //        strTemp = escape(sStr[i]);
    //        if (strTemp.indexOf("%u", 0) == -1) // 表示是汉字  
    //        {
    //            iCount = iCount + 1;
    //        }
    //        else {
    //            iCount = iCount + 1;
    //        }
    //    }
    //    return iCount;

    var len = 0;
    for (var i = 0; i < sStr.length; i++) {
        var c = sStr.charCodeAt(i);
        //单字节加1   
        if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
            len++;
        }
        else {
            len += 2;
        }
    }
    return len;
}