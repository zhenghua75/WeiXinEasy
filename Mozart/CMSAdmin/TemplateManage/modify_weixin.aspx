<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modify_weixin.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.modify_weixin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <title>微信</title>
    <link rel="stylesheet" href="css/bootstrap.min.css"/>
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/yypz.css"/>
    <link href="css/ui.css" rel="stylesheet" type="text/css" />
    <link href="css/weixin.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Request.js"></script>
    <script src="scripts/Global.js"></script>
    <script src="scripts/Step1.js"></script>
    <script src="scripts/weixin.js"></script>
    <script src="scripts/ajaxupload2.3.6.js"></script>
    <script>
        $(function () {

            var ecid = Request.GetQuery('ecid');
            var template_id = Request.GetQuery('template_id');
            var shell_type = Request.GetQuery('shell_type');
            var doc = null;
            $('#ylFrame').load(function () {
                doc = $(this.contentDocument);
            });

            //限制输入字数
//            $(document).on('keyup', '.wx-menuMain', function () {
//                CheckLength(this, 8);
//            });

//            $(document).on('keyup', '.wx-menuItem', function () {
//                CheckLength(this, 14);
//            });


            //tab切换
            $('#rTab > li > a').click(function () {

                $(this).parent().addClass('active').siblings().removeClass('active');

                var i = $('#rTab > li > a').index(this);

                $('#rTabCont > li').removeClass('active').eq(i).addClass('active');

                return false;
            });

            //组合框
            $(document).on('change', '#wx-table select', function () {

                if ($(this).val() == '链接') {
                    $(this).siblings('input').show().siblings('select[type=sKeyWord]').hide();
                    $(this).parents('tr').attr('type', 2);
                }
                else {
                    $(this).siblings('select[type=sKeyWord]').show().siblings('input').hide();
                    $(this).parents('tr').attr('type', 1);
                }

            });

            //删除菜单项
            $(document).on('click', '#wx-table .btn_del', function () {

                var currentBtnDel = $(this);
                popConfirm("确认删除该项？", function (res) {
                    if (res) {
                        var tr = currentBtnDel.parents('tr');

                        //如果是一级菜单,并且有子菜单
                        if (tr.attr('menulevel') == 1) {
                            if (tr.next().attr('menulevel') == 2) {
                                alert('必须先删除子菜单，才能删除主菜单!');
                                return;
                            }
                            else {
                                //没有子菜单.左侧预览区改变
                                var trIndex = $('#wx-table tr[menulevel=1]').index(tr);
                                doc.find('#menu li').eq(trIndex + 1).empty();
                            }
                        }
                        else {
                            //如果是二级菜单
                            //一直往上找主菜单
                            var parent = tr.prev();
                            while (parent.attr('menulevel') != 1) {
                                parent = parent.prev();
                            }
                            //找到后判断其儿子是否就一个(这里判断主菜单后面的后面是不是另一个主菜单)
                            if (parent.next().next().attr('menulevel') == 1) {
                                //如果其父级只有这一个子菜单了，这个菜单被删除的时候,需要把父级的隐藏项目显示
                                var select = parent.find('td:nth-child(2) > select').show();
                                select.find('*').show();
                                if (select.val() == '链接') {
                                    select.siblings('input').show().siblings('a,span').hide();
                                }
                                else {
                                    select.siblings('input').hide().siblings('a,span').show();
                                }
                            }
                            //预览区改变
                            var trIndex = $('#wx-table tr[menulevel=2]').index(tr);
                            doc.find('.content li').eq(trIndex).remove();

                        }
                        tr.remove();
                    }
                });

            });

            //添加子菜单
            $(document).on('click', '#wx-table .btn-add', function () {

                var tr = $(this).parents('tr');

                var trIndex = $('#wx-table tr[menulevel=1]').index(tr);

                tr.find('td:nth-child(2) *').hide();

                var last = false;
                var count = -1;
                do {
                    count++;
                    if (tr.is('#wx-table tr:last')) {
                        last = true;
                        break;
                    }
                    tr = tr.next();
                } while (tr.attr('menulevel') == 2);

                if (count == 5) {
                    popAlert('子菜单上限为5个!');
                    return;
                }

                var ecid = Request.GetQuery('ecid');
                var resultKeyWord = Request.Sync('/CMSAdmin/TemplateManage/GetKeyword.ashx?ecid=' + ecid, {
                    'ecid': ecid
                });

                var nTr = $(Request.format('<tr bizmoduleid="{BizModuleID}" menulevel="{MenuLevel}" moduleid="{ModuleID}" moduletypeid="{ModuleTypeId}" type="{Type}"><td><span class="q4"></span><input class="wx-menuItem" type="text" value="{MenuName}"></td><td><select type="fselect"><option selected>链接</option><option>关键字</option></select><input class="wx-menuLink" type="text" value="{LinkUrl}" placeholder="请输入以http://开头的链接地址"  style="margin-left:10px;"><select type="sKeyWord"  style="display:none; margin-left:50px;">' + Request.formats('<option value="{MsgID}">{KeyWord}</option>', resultKeyWord.data) + '</select></td><td style="width:60px;"><a class="btn_del"></a></td></tr>', {
                    'BizModuleID': '',
                    'MenuLevel': 2,
                    'ModuleID': '',
                    'ModuleTypeId': '',
                    'Type': 2,
                    'MenuName': '',
                    'LinkUrl': ''
                }));

                tr[last ? 'after' : 'before'](nTr);
                //              tr.find('td > select[type=sKeyWord]').html(Request.formats('<option value="{MsgID}">{KeyWord}</option>', resultKeyWord.data));
                //trIndex
                doc.find('#submenu' + (trIndex + 1)).append('<li>&nbsp;</li>');

                /*
                if(count == 0){
                //添加第一个子菜单,带父级的配置过来
                var pTd = $(this).parents('tr').find('td:nth-child(2)');
                if(pTd.find('select').val() == '链接'){
                nTr.find('select').siblings('input').val(pTd.find('input').val()).show().siblings('a,span').hide();
                }
                else{
                nTr.find('select > option').removeAttr('selected').last().attr('selected', 'selected');
                nTr.find('td:nth-child(2) > input').hide().siblings('a').show().siblings('span').show().html(pTd.find('td:nth-child(2) > span').html());
				
				
                }
                }*/

            });

            //添加主菜单
            $('#addMenu').click(function () {

                if ($('#wx-table tr[menulevel=1]').length == 3) {
                    popAlert('主菜单上限为3个!');
                    return;
                }

                var ecid = Request.GetQuery('ecid');
                var resultKeyWord = Request.Sync('/CMSAdmin/TemplateManage/GetKeyword.ashx?ecid=' + ecid, {
                    'ecid': ecid
                });

                //$('#wx-table > tbody').append('<tr bizmoduleid="0" menulevel="1" moduleid="0" moduletypeid="0" type="1"><td style="width:200px;"><input class="wx-menuMain" type="text" value=""><a class="btn-add"></a></td><td><select><option selected>链接</option><option>业务模块</option></select><input class="wx-menuLink" type="text" value="" style="margin-left:10px;"><a class="btn-add2" style="display:none;"></a><span style="display:none; margin-left:50px;"></span></td><td style="width:60px;"><a class="btn_del"></a></td></tr>');
                $('#wx-table > tbody').append('<tr ServiceMsgId="0"  menulevel="1" moduleid="0" moduletypeid="0" type="2"><td style="width:200px;"><input class="wx-menuMain" type="text" value=""><a class="btn-add"></a></td><td><select type="fselect"><option selected>链接</option><option>关键字</option></select><input class="wx-menuLink" type="text" value="" style="margin-left:10px;" placeholder="请输入以http://开头的链接地址" ><select type="sKeyWord"  style="display:none; margin-left:50px;">' + Request.formats('<option value="{MsgID}">{KeyWord}</option>', resultKeyWord.data) + '</select></td><td style="width:60px;"><a class="btn_del"></a></td></tr>');

                doc.find('#menu li:empty').html('<img src="images/icon_wx_prev.png" style="width:10px; height:10px; display:inline-block; vertical-align:middle; padding-right:2px;"><span></span>');

            });

            //添加
            $('#addFunc').click(function () {

                if (!vaildKeyWord()) {
                    return;
                }

                $('#func_table input,#func_table a').removeClass("border_red"); //移除输入标红边框的
                if (!$('#tjgnFrame').attr('src')) {
                    $('#tjgnFrame').attr('src', $('#pop-tjgn .p-nav > li.active:first ul a').attr('href'));
                }
                $('#pop-tjgn').show();

                Request.GetBizModuleList(ecid, 'weixin');
            });




            var curBtnAdd = null;
            //添加功能
            $(document).on('click', '.btn-add2', function () {

                curBtnAdd = $(this);

                if (!$('#tjgnFrame').attr('src')) {
                    $('#tjgnFrame').attr('src', $('#pop-tjgn .p-nav > li.active:first ul a').attr('href'));
                }
                $('#pop-tjgn').show();

                Request.GetBizModuleList(ecid, 'weixin');

            });

            $('#pop-tjgn b').click(function () {
                $('#pop-tjgn').hide();
            });




            //弹出层 - 添加功能 - 点击导航切换导航效果
            Global.change('#pop-tjgn li a', function () {
                Request.GetBizModuleListDetail(ecid, template_id, $(this).attr('bizmoduleid'));
            });

            //弹出层 - 添加功能 - 导航展开 收缩效果
            Step1.tjgnNavSlide();
            Step1.tjgnSetup();

            $('.bcyl').click(function () {

                if (Request.SaveWeiXinMenu(ecid, template_id)) {

                    var mbFrame = $(window.opener.document).find('#mbFrame');

                    mbFrame.attr('src', mbFrame.attr('src'));

                    popAlert('保存成功!', function (res) {
                        if (res) {
                            window.close();
                        }
                    });
                }
                else {
                    popAlert('保存失败');
                }

            });

            function inputChanged() {
                var i = $('.wx-menuMain').index(this);
                doc.find('#menu li:eq(' + (i + 1) + ') span').html(this.value);
            }

            $(document).on('propertychange', '.wx-menuMain', inputChanged);
            $(document).on('input', '.wx-menuMain', inputChanged);

            function inputChanged2() {
                var i = $('.wx-menuItem').index(this);
                doc.find('.content li').eq(i).html(this.value);
            }

            $(document).on('propertychange', '.wx-menuItem', inputChanged2);
            $(document).on('input', '.wx-menuItem', inputChanged2);


            $('#txtAttReply').bind('propertychange, input', function () {
                doc.find('#firstreply').html(this.value);
            });

            //Request.GetWeiXinMenu(ecid, template_id);


            //取消搜索
            Step1.cancelSearch();

            //回车搜索
            Step1.enterSearch();

        });
    </script>
</head>
<body>
    <div id="layer">
    </div>
    <div id="PopUpWindow">
        <div class="PopUpWindow">
            <h1>
                提示<i></i></h1>
            <p class="PopUpWord">
            </p>
            <p class="PopUpBtns">
                <a class="PopUpOk">确定</a> <a class="PopUpCancel">取消</a>
            </p>
        </div>
    </div>
    <div class="content">
        <div class="left cl2">
            <div class="lTitle modify-lt" style="display:none;">
                模板名称</div>
            <div class="ylq">
                <iframe id="ylFrame" src="weixin.html" frameborder="0" scrolling="no" width="100%"
                    height="100%"></iframe>
            </div>
        </div>
        <div class="right r2">
            <ul class="huodong clearfix">
                <li class="active"><span class="hd1">关键字回复</span></li>
                <li><span class="hd2">关注自动回复</span></li>
                <li><span class="hd3">微信菜单</span></li>
            </ul>
            <div class="detail1-tabs panel panel-default">
                <ul class="detail1-tabs-content">
                    <li class="active">
                        <div class="tw">
                            <div class="clearfix">
                                <a id="addFunc" class="u-btnBlue pull-right tjgn">添加</a>
                            </div>
                            <div  class="rr2 r-weixin" style=" height:460px;">
                                <div style="font-weight: bold;">
                                    <p style="float: left; width: 230px;">
                                        功能名称</p>
                                    <p style="float: left;">
                                        关键字</p>
                                    <table id="func_table" class="table table-responsive table-striped func-table">
                                        <tbody class="ui-sortable" style="">
                  
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </li>
                    <!--2第二步-->
                    <li>
                        <div class="tw">
                            <div>
                                <div class="r5 clearfix">
                                    <div>
                                        关注时回复
                                    </div>
                                    <hr class="weixin_hr" />
                                    <div style="display: none;">
                                        <label>
                                            回复类型</label>
                                        <select id="selectReplyType">
                                            <option value="1">文字回复 </option>
                                            <option value="2">关键字 </option>
                                        </select>
                                    </div>
                                    <div style="display: none;">
                                        <label>
                                            关键字</label>
                                        <select id="selectKeyWord">
                                        </select>
                                    </div>
                                    <div id="bizModuleNameReply" style="display: none;">
                                        <label>
                                            业务模块</label>
                                        <span></span>
                                    </div>
                                    <div>
                                        <div class="txtArea">
                                            <div class="editArea">
                                                <textarea id="txtAttReply" maxlength="300" style="height: 130px; width: 99%; border: 0px;"></textarea>
                                            </div>
                                            <div class="functionBar">
                                                <div class="opt">
                                                    <a>
                                                      </a></div>
                                                <div class="tip">
                                                    还可以输入<span id="wordAttReplyCount">300</span>字</div>
                                                <div class="clr">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="margin-top: 10px;">
                                        自动回复
                                    </div>
                                    <hr class="weixin_hr" />
                                    <div>
                                        <div class="txtArea">
                                            <div class="editArea">
                                                <textarea id="txtAutoReply" maxlength="300" style="height: 130px; width: 99%; border: 0px;"></textarea>
                                            </div>
                                            <div class="functionBar">
                                                <div class="opt">
                                                    <a>
                                                       </a></div>
                                                <div class="tip">
                                                    还可以输入<span id="wordAutoReplyCount">300</span>字</div>
                                                <div class="clr">
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                    </li>
                    <li>
                        <div class="tw">
                            <div class="r4 clearfix">
                                <ul id="rTabCont" class=" rr2 r-weixin">
                                    <li class="active">
                                        <div class="clearfix">
                                            <a id="addMenu" class="u-btnBlue pull-left tjgn">添加主菜单</a>
                                        </div>
                                        <div style="font-weight: bold; margin-top: 20px;" class="clearfix">
                                            <p style="float: left; width: 200px;">
                                                菜单名称</p>
                                            <p style="float: left;">
                                                回复内容</p>
                                        </div>
                     <div class="func-table-wrap-noheight" style="margin-top: 10px;">
                        <table id="wx-table" class="table table-responsive table-striped">
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
            <div style="padding-bottom: 10px; text-align: center;">
                <a id="prev" class="btn-green2" style="line-height: 33px; height: 33px; box-sizing: content-box;
                    display: none;">上一步</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id="send" style="display: none;"
                        class="btn-green2">保存</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a id="next" class="btn-green2">
                            下一步</a>
            </div>
        </div>
    </div>
    <!-- 添加功能弹出DIV窗 -->
    <div id="pop-tjgn">
        <div class="p-outer">
            <div class="p-inner">
                <h1>
                    <b></b>添加功能</h1>
                <div class="p-main">
                    <input type="hidden" id="onlyone">
                    <div class="p-left">
                        <div class="p-searchBox">
                            <input id="pop-search" type="text" value="搜索..." tips="搜索...">
                            <span></span>
                        </div>
                        <ul id="ModuleList" class="p-nav">
                        </ul>
                    </div>
                    <div class="p-right">
                        <div id="tjgnCont">
                            <div class="tjgn-header">
                                <h1 class="tjgn-h-title">
                                </h1>
                                <div class="tjgn-h-icons">
                                    <span id="ticon-iphone" class="tjgn-h-icon1"></span><span id="ticon-android" class="tjgn-h-icon2">
                                    </span><span id="ticon-weixin" class="tjgn-h-icon3"></span><span id="ticon-mp" class="tjgn-h-icon4">
                                    </span><span id="ticon-sms" class="tjgn-h-icon5"></span>
                                </div>
                                <a class="tjgn-u-btnGreen" id="btn-ok-closePop">确认</a>
                            </div>
                            <div class="tjgn-content">
                                <p id="bizdescription">
                                </p>
                                <input id="moduleurl" placeholder="请输入以http://开头的链接地址" name="moduleurl" type="text"
                                    value="" style="width: 300px; margin-bottom: 5px;" />
                                <div class="tjgn-images" style="text-align: center;">
                                    <img id="timage1" width="150" height="266">
                                    <img id="timage2" width="150" height="266">
                                    <img id="timage3" width="150" height="266">
                                </div>
                            </div>
                        </div>
                        <div class="setupbar" style="display: none;">
                            <span><i></i>设置</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 添加图文回复弹出DIV窗 -->
    <div id="pop-twhf" class="pop-hf">
        <div class="p-outer">
            <div class="p-inner">
                <h1>
                    <b></b>回复内容</h1>
                <div class="p-main">
                    <input type="hidden" id="Hidden1">
                    <p>
                        标题</p>
                    <p>
                        <input type="text" id="txtTwReplyTitle" placeholder="请输入标题" /></p>
                    <p>
                        封面 <span style="color: #BBB; margin-left: 120px;">建议尺寸：720*400px 格式：jpg或png</span></p>
                    <div style="float: left; height: 80px;">
                        <a class="upload-btn" id="upload_weixincover">上传图片</a>
                    </div>
                    <div style="float: left; margin-left: 10px; height: 80px;">
                        <img height="80px" id="img_weixincover" src="images/c_bg.jpg" />
                    </div>
                    <p style="clear: both;">
                        摘要</p>
                    <p>
                        <textarea id="txtTwReplySummary" style="width: 420px; height: 60px;" placeholder="请输入摘要"></textarea></p>
                    <div style="text-align: center">
                        <input type="button" class="hf-cancel-btnGreen" value="取消" />
                        <input type="button" id="btnTwhfConfirm" class="hf-confirm-btnGreen" value="确认" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 添加文字回复弹出DIV窗 -->
    <div id="pop-wzhf" class="pop-hf">
        <div class="p-outer">
            <div class="p-inner">
                <h1>
                    <b></b>回复内容</h1>
                <div class="p-main">
                    <textarea id="txtWzReplyContent"></textarea>
                    <div style="text-align: center">
                        <input type="button" class="hf-cancel-btnGreen" value="取消" />
                        <input type="button" id="btnWzhfConfirm" class="hf-confirm-btnGreen" value="确认" />
                        
                    </div>
                </div>
            </div>
        </div>
</body>
</html>
