<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modify_app.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.modify_app" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/bootstrap.min.css"/>
    <link rel="stylesheet" href="css/yypz.css"/>
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <link href="css/colorpicker.css" rel="stylesheet"/>
    <script>
        var navPaging = null; //导航区分页
        var mdfPaging = null; //修改图标分页
	
    </script>
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/ajaxupload2.3.6.js"></script>
    <script src="scripts/colorpicker.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/global.js"></script>
    <script src="scripts/Request.js"></script>
    <script src="scripts/Step1.js"></script>
    <script src="scripts/jquery-ui.js"></script>
    <script src="scripts/Paging.js"></script>
    <script src="scripts/Helper.js"></script>
    <script>

        $(function () {


            var ecid = Request.GetQuery('ecid');

            var template_id = Request.GetQuery('template_id');

            var shell_type = Request.GetQuery('shell_type').toLowerCase();

            $('#ylFrame').attr('src', shell_type + '.html?ecid=' + ecid + "&template_id=" + template_id);


            Request.GetBaseConfig(ecid, template_id);

            Step1.getImgs(ecid, template_id);

            Request.GetGenerateResource(ecid);

            $('#input_zb,#input_bg,#input_help,#input_cover').click(function () {
                if (this.checked) {
                    $(this).siblings('div,a').show();

                    //选择第一张为默认图片
                    var tid = $(this).attr("id");
                    if ($("#img_" + tid.split('_')[1]).css("display") == "none") {
                        $("#img_" + tid.split('_')[1]).removeClass("active");
                        $("#img_" + tid.split('_')[1]).next().addClass("active");
                    }
                }
                else {
                    $(this).siblings('div,a').hide();
                }
            });

            var isRepeat = false;
            var tb_input = '#func_table td input[type=text], #menu_table td input[type=text]';
            //监听表格文本框焦点离开
            $(document).on('keyup', tb_input, function () {
                var value = this.value;
                var count = 0;
                $(tb_input).each(function (index, element) {
                    if (this.value == value) {
                        count++;
                    }
                });
                if (count > 1) {
                    isRepeat = true;
                    $('.textTips').html('*提示：名称不能重复,请修改.');
                    $(this).css('border-color', 'red'); //[0].focus();
                    $(this).attr("repeatname", 'true');
                }
                else {
                    //设置img的text属性
                    $('.textTips').empty();
                    $(this).parent().siblings().find('img').attr('text', value);
                    $(this).css('border-color', '#D8D8D8');
                    $(this).attr("repeatname", 'false');
                    var doc = $($('#ylFrame')[0].contentWindow.document);
                    var isFunc = !!$('#rTabCont > li.active').has('#func_table').length;
                    var ordernum = $(this).parent().siblings().find('img').attr('ordernum');
                    doc.find((isFunc ? '#func' : '#menu') + ordernum).siblings('p').html(value);
                    isRepeat = false;
                }
            });
            //            $(document).on('blur', tb_input, function () {
            //                var value = this.value;
            //                var count = 0;
            //                $(tb_input).each(function (index, element) {
            //                    if (this.value == value) {
            //                        count++;
            //                    }
            //                });
            //                if (count > 1) {
            //                    isRepeat = true;
            //                    $('.textTips').html('*提示：名称不能重复,请修改.');
            //                    $(this).css('border-color', 'red'); //[0].focus();
            //                    $(this).attr("repeatname", 'true');

            //                }
            //                else {
            //                    //设置img的text属性
            //                    $('.textTips').empty();
            //                    $(this).parent().siblings().find('img').attr('text', value);
            //                    $(this).css('border-color', '#D8D8D8');
            //                    $(this).attr("repeatname", 'false');
            //                    var doc = $($('#ylFrame')[0].contentWindow.document);
            //                    var isFunc = !!$('#rTabCont > li.active').has('#func_table').length;
            //                    var ordernum = $(this).parent().siblings().find('img').attr('ordernum');
            //                    doc.find((isFunc ? '#func' : '#menu') + ordernum).siblings('p').html(value);
            //                    isRepeat = false;
            //                }
            //            });



            //上传ICON
            $('#upload_icon').click(function () {
                Request.UploadResourceFile('#upload_icon', 'png', ecid, template_id, 'icon', function (rs) {
                    if (rs.code == 1) {
                        $('#img_icon').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark
                        }).show();
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //上传LOADING
            $('#upload_loading').click(function () {
                Request.UploadResourceFile('#upload_loading', 'png', ecid, template_id, 'loading', function (rs) {
                    if (rs.code == 1) {
                        $('#img_loading').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark
                        }).show();
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //上传HELP
            $('#upload_help').click(function () {
                Request.UploadResourceFile('#upload_help', 'zip', ecid, template_id, 'help', function (rs) {
                    if (rs.code == 1) {
                        var img = $('<img>').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark,
                            'class': 'active',
                            width: 55,
                            height: 80
                        }).show();
                        $('.img_help > img:first').remove();
                        $('.img_help').prepend(img);
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //上传COVER
            $('#upload_cover').click(function () {
                Request.UploadResourceFile('#upload_cover', 'png', ecid, template_id, 'cover', function (rs) {
                    if (rs.code == 1) {
                        var img = $('<img>').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark,
                            'class': 'active',
                            width: 55,
                            height: 80
                        }).show();
                        $('.img_cover > img:first').remove();
                        $('.img_cover').prepend(img);
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //上传BG
            $('#upload_bg').click(function () {
                Request.UploadResourceFile('#upload_bg', 'png', ecid, template_id, 'bg', function (rs) {
                    if (rs.code == 1) {
                        var img = $('<img>').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark,
                            'class': 'active',
                            width: 55,
                            height: 80
                        }).show();
                        $('.img_bg > img:first').remove();
                        $('.img_bg').prepend(img);
                        //$('.img_bg > img').removeClass('active').parent().prepend( img );
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            Step1.imgs_onclick();

            //保存预览
            $('.bcyl').click(function () {
                if (isRepeat) {
                    popAlert('名称不能重复,请修改后再保存');
                    return false;
                }
                var isReName = false;
                $(tb_input).each(function (index, element) {
                    if ($(this).attr("repeatname") == "true") {
                        isReName = true;
                    }
                });
                if (isReName == true) {
                    $('.textTips').html('*提示：名称不能重复,请修改.');
                    popAlert('名称不能重复,请修改后再保存');
                    return;
                }
                //提交检查
                var isShellC = /^shellc$/i.test(Request.GetQuery('shell_type'));
                var isShellS = /^shells$/i.test(Request.GetQuery('shell_type'));
                if (isShellC) {
                    if ($('#menu_table img').length != 5) {
                        popAlert('保存失败，请在“菜单区”配置5个功能');
                        return false;
                    }
                }
                if (isShellS) {
                    if ($('#func_table img').length != 7) {
                        popAlert('保存失败，请在“功能区”配置7个功能');
                        return false;
                    }
                    if ($('#menu_table img').length != 3) {
                        popAlert('保存失败，请在“菜单区”配置两个功能');
                        return false;
                    }
                }
                //保存数据
                var code = Request.SaveGenerateResource(ecid, template_id);

                if (code == 1) {
                    //刷新父窗体
                    try {
                        window.opener.location.reload();
                    } catch (e) { };
                    popAlert('保存成功，确定后，点击“下一步”就可以生成咯!', function (res) {
                        if (res) {
                            window.close();
                        }
                    });

                }
                else if (code == 6) {
                    popAlert('您暂未购买套餐，或套餐中的产品不足，如需帮助，请联系管理员。');
                }
                else {
                    popAlert('保存失败!');
                }
            });


            //-------------------

            //让A链接没有虚线边框
            Global.noDashed();

            //右侧tab栏切换效果
            Global.tabs('#rTab', '#rTabCont');

            //颜色选择切换
            Global.change('#func_textcolor > span', function () {
                var color = Request.RGB2Hex($(this).find('i').css('background-color'));
                $('#func_table img').attr('textcolor', color);
                //$('#func_table input').css('color', color);
                var doc = $($('#ylFrame')[0].contentWindow.document);
                doc.find('.func_p p').css('color', color);

            });
            Global.change('#menu_textcolor > span', function () {
                var color = Request.RGB2Hex($(this).find('i').css('background-color'));
                $('#menu_table img').attr('textcolor', color);
                //$('#func_table input').css('color', color);
                var doc = $($('#ylFrame')[0].contentWindow.document);
                doc.find('.menu_p p').css('color', color);

            });


            Global.change('#modifyIcon .bgColors > span', function () {
                $('#modifyIcon .imgs > img.active').attr('backgroundcolor',
				Request.RGB2Hex($(this).find('i').css('background-color'))
			);
            });


            Global.change('#modifyIcon .wzColors > span', function () {
                $('#modifyIcon .imgs > img.active').attr('textcolor',
				Request.RGB2Hex($(this).find('i').css('background-color'))
			);
            });

            //文字 已有、图片、隐藏的切换
            Step1.wordSwitch();

            //监听添加功能按钮事件,弹出模态层
            Step1.popTjgn(function () {
                Request.GetBizModuleList(ecid, 'app');
            });

            //取消搜索
            Step1.cancelSearch();

            //回车搜索
            Step1.enterSearch();

            //添加功能 - 设置区
            Step1.tjgnSetup();

            //搜索框,获得焦点,提示文本消失
            Global.inputTips('#pop-search');


            //弹出层 - 添加功能 - 点击导航切换导航效果
            Global.change('#pop-tjgn li a', function () {
                $('.tjgn-content').css('visibility', 'visible');
                Request.GetBizModuleListDetail(ecid, template_id, $(this).attr('bizmoduleid'));
            });

            //弹出层 - 添加功能 - 导航展开 收缩效果
            Step1.tjgnNavSlide();

            //点击弹出层的确定按钮，向表格插入数据
            $('#btn-ok-closePop').click(function () {

                var ordernum = $('#rTabCont > li.active table tr').length;

                var isFunc = !!$('#rTabCont > li.active').has('#func_table').length;

                var moduleid = $('#ModuleList a.active').attr('moduleid');

                var moduletypeid = $('#ModuleList a.active').attr('moduletypeid');

                var exit = false;

                if ($('.tjgn-h-title').attr('permissions') == 'false') {
                    popAlert('您暂未购买该功能，如需帮助，请联系管理员。');
                    return false;
                }

                if ($('#onlyone').val() == 'true') {

                    //判断是否可以重复添加
                    $('#rTabCont > li.active table img').each(function (index, element) {
                        if ($(this).attr('moduleid') == moduleid) {
                            exit = true;
                            popAlert('该功能已经配置过， 请勿重复添加！');
                            return false;
                        }
                    });
                }

                var isShellS = /^shells$/i.test(Request.GetQuery('shell_type'));
                //判断是否超过添加数量
                if (isFunc) {
                    if (
					(/^shelld$/i.test(Request.GetQuery('shell_type')) && $('#func_table tr').length >= 21)
					|| ($('#func_table tr').length >= 24)
				) {
                        exit = true;
                        popAlert('超过功能区数量上限');
                    }
                }
                else {
                    if ($('#menu_table tr').length >= 5) {
                        exit = true;
                        popAlert('超过菜单区数量上限');
                    }
                    if (isShellS && $('#menu_table tr').length >= 3) {
                        exit = true;
                        popAlert('超过菜单区数量上限');
                    }
                }

                var isShellP = /^shellp$/i.test(Request.GetQuery('shell_type'));
                if (isFunc) {
                    if ((/^shellg$/i.test(Request.GetQuery('shell_type')) && $('#func_table tr').length >= 20)) {
                        exit = true;
                        popAlert('超过功能区数量上限');
                    }
                    if ((/^shellr$/i.test(Request.GetQuery('shell_type')) && $('#func_table tr').length >= 20)) {
                        exit = true;
                        popAlert('超过功能区数量上限');
                    }

                    if (isShellP && $('#func_table tr').length >= 24) {
                        exit = true;
                        popAlert('超过功能区数量上限');
                    }
                    if (isShellS && $('#func_table tr').length >= 7) {
                        exit = true;
                        popAlert('超过功能区数量上限');
                    }
                }

                if (exit) {
                    return false;
                }
                var img_resourcetype_type = "_icon";
                var isShellD = /^shelld$/i.test(Request.GetQuery('shell_type'));
                if (isShellD) {
                    var m = ordernum + 1;
                    if (m == 1 || m == 2 || m == 8 || m == 9) {
                        //img_resourcetype_type = "_module_pic";//去掉shelld大图资源
                        img_resourcetype_type = "_icon";
                    }
                }

                var defaultTextColor = Request.RGB2Hex($('#rTabCont > li.active .colors2 > span.active > i').css('background-color'));
                if (isFunc) {
                    if (isShellP)
                        defaultTextColor = "#4b4b4b";
                }

                $('#rTabCont > li.active table').append(
			Request.format('<tr><td><img height="50" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="{image_url}" imageid="{imageid}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="{resourcetype}" shelltype="{shelltype}" textcolor="{textcolor}" themesimagesremark="{themesimagesremark}" partflag="{partflag}" text="{text}"></td><td><input type="text" value="{text}" onblur="$(this).parent().siblings().find(\'img\').attr(\'text\',this.value);"></td><td><a class="btn_del mt-6px"></a></td></tr>', {
			    /* 如果有自定义链接,就加上 */
			    'moduleurl': $('#moduleurl').is(':visible') ? $('#moduleurl').val() : '',
			    /* 取选中的颜色 */
			    'textcolor': defaultTextColor,
			    'moduleid': moduleid,
			    'bizmoduleid': $('#ModuleList a.active').attr('bizmoduleid'),
			    'partflag': isFunc ? 3 : 4,
			    'text': $('.tjgn-h-title').html(),
			    'ordernum': ordernum + 1,
			    'image_url': '/ResourceAdmin/GetResourceFile?FileID=27762',
			    'themesimagesremark': 'df29853c-c233-4200-a999-e40e39ef5b4f',
			    'imageid': '27762',
			    'resourcetype': Request.GetQuery('shell_type') + (isFunc ? img_resourcetype_type : '_bottom_tabbar_icon'),
			    'moduletypeid': moduletypeid,
			    'function_id': 0
			})
		).parent().scrollTop(9999); //滚动到底部 

                if (!isFunc) {
                    var img_ishome = $('#menu_table tr img[ishome=true]');
                    var index_ishome = img_ishome.attr('ordernum');
                    var shellIndex = 0;
                    $('#menu_table tr img').each(function (index, element) {
                        if ($(this).attr('ishome') == "true") {
                            //$(this).attr('ordernum', index_ishome);
                        } else {
                            shellIndex++;
                            if (index_ishome == shellIndex) {
                                shellIndex++;
                            }
                            $(this).attr('ordernum', shellIndex);
                        }
                    });
                    //排序
                    var tableObject = $('#menu_table'); //获取id为tableSort的table对象 
                    var tbBody = tableObject.children('tbody'); //获取table对象下的tbody 
                    var tbBodyTr = tbBody.find('tr'); //获取tbody下的tr
                    tbBodyTr.sort(function (a, b) {
                        var td1 = $(a).find("td img").attr('ordernum');
                        td1 = isNaN(Number(td1)) ? td1 : Number(td1);
                        var td2 = $(b).find("td img").attr('ordernum');
                        td2 = isNaN(Number(td2)) ? td2 : Number(td2);
                        var dir = 1;
                        if (td1 > td2) {
                            return dir;
                        } else if (td1 < td2) {
                            return -dir;
                        } else {
                            return 0;
                        }
                    });
                    $(tbBodyTr).each(function () {
                        $("tbody", tableObject).append($(this));
                    });

                }
                var doc = $($('#ylFrame')[0].contentWindow.document);

                //                doc.find(
                //			(isFunc ? '#func' : '#menu') + (ordernum + 1)
                //		).attr('src', '/ResourceAdmin/GetResourceFile?FileID=27762').siblings('p').html($('.tjgn-h-title').html());
                if (isFunc) {
                    $('#func_table tbody').find('tr img').each(function (index, element) {
                        doc.find('#func' + $(this).attr('ordernum')).attr('src', this.src).siblings().html($(this).parent('td').siblings().find('input').val());
                    });
                } else {
                    $('#menu_table tbody').find('tr img').each(function (index, element) {
                        doc.find('#menu' + $(this).attr('ordernum')).attr('src', this.src).siblings().html($(this).parent('td').siblings().find('input').val());
                    });
                }
                if (shell_type == 'shellb' && !isFunc) {
                    doc.find('#menu').siblings().find('li').width(100 / $('#rTabCont > li.active table tr').length + '%');
                }

                $('#pop-tjgn').hide();

                //$('#rTabCont > li.active table tr:last input').focus();

                $(tb_input).trigger('input');

                var isReName = false;
                $(tb_input).each(function (index, element) {
                    if ($(this).attr("repeatname") == "true") {
                        isReName = true;
                    }
                });
                if (isReName == true) {
                    $('.textTips').html('*提示：名称不能重复,请修改.');
                } else {
                    $('.textTips').empty();
                }
            });

            //点击删除 功能项目或菜单项目
            $(document).on('click', '.btn_del', function () {

                var currentBtnDel = $(this);
                popConfirm("确认删除该项目？", function (res) {

                    if (res) {
                        var doc = $($('#ylFrame')[0].contentWindow.document);

                        //排序id重新刷新
                        var tbody = currentBtnDel.parents('tbody');

                        var isFunc = !!tbody.parents('#func_table').length;

                        for (var i = 1; i <= 24; i++) {
                            if (isFunc) {
                                doc.find('#func' + i).attr('src', doc.find('#hidFuncBlankIcon').val()).siblings('p').empty();
                                doc.find('#func' + i).parent().css("background-color", "");
                            }
                            else {
                                doc.find('#menu' + i).attr('src', doc.find('#hidMenuBlankIcon').val()).siblings('p').empty();
                            }
                        }


                        if (isFunc) {
                            currentBtnDel.parents('tr').remove();

                            tbody.find('tr img').each(function (index, element) {

                                $(this).attr('ordernum', index + 1);

                                doc.find('#func' + (index + 1)).attr('src', this.src).siblings().html($(this).parent('td').siblings().find('input').val());

                            });
                        }
                        else {	//判断删除的元素是菜单区，特殊处理
                            currentBtnDel.parents('tr').remove();
                            var img_ishome = $('#menu_table tr img[ishome=true]');
                            var index_ishome = img_ishome.attr('ordernum');
                            var shellIndex = 0;
                            tbody.find('tr img').each(function (index, element) {
                                if ($(this).attr('ishome') == "true") {
                                    //doc.find('#menu' + index_ishome).attr('src', this.src).siblings().html($(this).parent('td').siblings().find('input').val());
                                } else {
                                    shellIndex++;
                                    if (index_ishome == shellIndex) {
                                        shellIndex++;
                                    }
                                    $(this).attr('ordernum', shellIndex);
                                    //doc.find('#menu' + shellIndex).attr('src', this.src).siblings().html($(this).parent('td').siblings().find('input').val());
                                }
                            });

                            //排序
                            var tableObject = $('#menu_table'); //获取id为tableSort的table对象 
                            var tbBody = tableObject.children('tbody'); //获取table对象下的tbody 
                            var tbBodyTr = tbBody.find('tr'); //获取tbody下的tr
                            tbBodyTr.sort(function (a, b) {
                                var td1 = $(a).find("td img").attr('ordernum');
                                td1 = isNaN(Number(td1)) ? td1 : Number(td1);
                                var td2 = $(b).find("td img").attr('ordernum');
                                td2 = isNaN(Number(td2)) ? td2 : Number(td2);
                                var dir = 1;
                                if (td1 > td2) {
                                    return dir;
                                } else if (td1 < td2) {
                                    return -dir;
                                } else {
                                    return 0;
                                }
                            });
                            $(tbBodyTr).each(function () {
                                $("tbody", tableObject).append($(this));
                            });
                            //重新显示
                            tbody.find('tr img').each(function (index, element) {
                                doc.find('#menu' + $(this).attr('ordernum')).attr('src', this.src).siblings().html($(this).parent('td').siblings().find('input').val());
                            });

                        }
                    }
                });

            });

            //导航背景
            $(document).on('click', '.bj-imgs > img', function () {
                var doc = $($('#ylFrame')[0].contentWindow.document);
                doc.find('#top').attr('src', this.src);
            })
            //菜单背景
            $(document).on('click', '.cd-imgs > img', function () {
                var doc = $($('#ylFrame')[0].contentWindow.document);
                doc.find('#menu').attr('src', this.src);
            })


            //上传导航区背景
            $('#upload_top').click(function () {
                Request.UploadResourceFile('#upload_top', 'png', ecid, Request.GetQuery('template_id'), 'top', function (rs) {
                    //console.log(rs);
                    if (rs.code == 1) {
                        navPaging.insert(false, {
                            image_url: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark,
                            width: 33,
                            height: 33,
                            partflag: 2,
                            resourcetype: 'top'
                        });
                        //选中触发左侧生效
                        $('.bj-imgs > img.active').click();
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //上传导航区文字图
            $('#upload_top_txt').click(function () {
                Request.UploadResourceFile('#upload_top_txt', 'png', ecid, Request.GetQuery('template_id'), 'top_txt', function (rs) {
                    if (rs.code == 1) {
                        var img = $('<img>').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark,
                            partflag: 2,
                            resourcetype: 'top_txt'
                        }).show();
                        $('#top_txt_div').html(img);
                        var doc = $($('#ylFrame')[0].contentWindow.document);
                        doc.find('#top_txt').attr('src', rs.data.image_url);
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //上传菜单区背景
            $('#upload_menu_bg').click(function () {
                Request.UploadResourceFile('#upload_menu_bg', 'png', ecid, Request.GetQuery('template_id'), $('.cd-imgs > img').attr('resourcetype'), function (rs) {
                    //console.log(rs);
                    if (rs.code == 1) {
                        var img = $('<img>').attr({
                            src: rs.data.image_url,
                            imageid: rs.data.imageid,
                            themesimagesremark: rs.data.themesimagesremark,
                            'class': 'active',
                            width: 33,
                            height: 33,
                            partflag: 4,
                            resourcetype: $('.cd-imgs > img').attr('resourcetype')
                        }).show();
                        $('.cd-imgs > img:first').remove();
                        $('.cd-imgs').prepend(img);
                        //选中触发左侧生效
                        $('.cd-imgs > img.active').click();
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });
            }).click();

            //uploadPackage
            $('#upload_package').click(function () {

                Request.UploadResourcePackage('#upload_package', ecid, Request.GetQuery('template_id'), function (rs) {
                    //console.log(rs);
                    if (rs.code == 1) {
                        popAlert('上传成功');
                    }
                    else {
                        popAlert(rs.notice);
                    }
                });

            }).click();

            var helperPanel = new Helper('#helper_panel', 'png格式; 建议尺寸124*124px');

            // 点击表格的图标 .弹出遮罩层【表格图标】
            var tbCurrIcon = null;
            $(document).on('click', '#func_table img', function () {

                var func_postion = $('#func_table img').index(this) + 1;

                if (shell_type == 'shellb') {
                    helperPanel.setMsg('png格式; 建议尺寸114*114px');
                }
                else if (shell_type == 'shellc') {
                    helperPanel.setMsg('png格式; 建议尺寸100*100px');
                }
                else if (shell_type == 'shelld') {
                    helperPanel.setMsg('png格式; 建议尺寸61*61px');
                    var index = $('#func_table img').index(this);
                }
                else if (shell_type == 'shelle') {
                    helperPanel.setMsg('png格式; 建议尺寸340*340');
                    var index = $('#func_table img').index(this);
                }
                else if (shell_type == 'shellf') {
                    helperPanel.setMsg('png格式; 建议尺寸288*144px');
                    var index = $('#func_table img').index(this);
                }
                else if (shell_type == 'shellg') {
                    if (func_postion == 1 || func_postion == 6 || func_postion == 11 || func_postion == 16 || func_postion == 21) {
                        helperPanel.setMsg('png格式; 建议尺寸160*160px');
                    } else if (func_postion == 2 || func_postion == 7 || func_postion == 12 || func_postion == 17 || func_postion == 22) {
                        helperPanel.setMsg('png格式; 建议尺寸432*160px');
                    } else if (func_postion == 3 || func_postion == 8 || func_postion == 13 || func_postion == 18 || func_postion == 23) {
                        helperPanel.setMsg('png格式; 建议尺寸608*160px');
                    } else if (func_postion == 4 || func_postion == 5 || func_postion == 14 || func_postion == 15
                        || func_postion == 19 || func_postion == 20 || func_postion == 24 || func_postion == 25) {
                        helperPanel.setMsg('png格式; 建议尺寸296*160px');
                    }
                } else if (shell_type == "shellr") {
                    helperPanel.setMsg('png格式; 建议尺寸308*196px');
                    if (func_postion == 3 || func_postion == 4 || func_postion == 9 || func_postion == 10 || func_postion == 13 || func_postion == 14 || func_postion == 19 || func_postion == 20) {
                        helperPanel.setMsg('png格式; 建议尺寸150*196px');
                    }
                }
                else if (shell_type == "shellp") {
                    if (func_postion <= 8) {
                        $(".wzys2").show();
                        helperPanel.setMsg('png格式; 建议尺寸64*64px');
                    }
                    else {
                        $(".wzys2").hide();
                        helperPanel.setMsg('png格式; 建议尺寸300*150px');
                    }
                } else if (shell_type == "shells") {
                    helperPanel.setMsg('png格式; 建议尺寸72*72px');
                    if (func_postion == 1) {
                        helperPanel.setMsg('png格式; 建议尺寸235*235px');
                    }
                }

                tbCurrIcon = this;

                upload_mIcon();

                var map = Request.GetAttrMap(this);

                map.image_url = map.src;

                $('#panel, #modifyIcon').show();


                //设置文字颜色
                $('#modifyIcon .wzColors span:first').html('<i style="background:' + Request.RGB2Hex(map.textcolor) + '"></i>').addClass('active');

                //设置背景颜色
                $('#modifyIcon .bgColors span:first').html('<i style="background:' + Request.RGB2Hex(map.backgroundcolor) + '"></i>').addClass('active');


                //$('#modifyIcon .item:nth-child(3)').show();

                var isShellD = /^shelld$/i.test(Request.GetQuery('shell_type'));
                var isShellP = /^shellp$/i.test(Request.GetQuery('shell_type'));
                var isShellS = /^shells$/i.test(Request.GetQuery('shell_type'));
                $('#modifyIcon').css('height', (isShellD || isShellP || isShellS) ? '450px' : '360px');
                // Request.RGB2Hex( $(this).find('i').css('background-color') );


                var isShellB = /^shellb$/i.test(Request.GetQuery('shell_type'));
                var isShellC = /^shellc$/i.test(Request.GetQuery('shell_type'));
                var bgItem = $('#modifyIcon .item:eq(2)');

                if (isShellB || isShellC) {

                    bgItem.hide();
                    $('#modifyIcon').height(270);
                }

                //获取预选资源
                //预选资源获取

                result = Request.GetResourceImages(
				ecid,
				Request.GetQuery('template_id'),
				map.resourcetype,
				1,
				500
			);

                //console.log( result );

                if (result.code == 1) {

                    result.data.items.unshift(map);

                    mdfPaging = new Paging(
					result.data.items,
					 8,
					 '#modifyIcon .imgs img.active',
					 '#modifyIcon .imgs',
					 '#modify_prev',
					 '#modify_next',
					 '<img height="50" width="50" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="{image_url}" imageid="{imageid}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="{resourcetype}" shelltype="{shelltype}" themesimagesremark="{themesimagesremark}" partflag="{partflag}">'
				);

                    mdfPaging.current();

                    $('#modifyIcon .imgs img:first').click();
                }


            });


            //菜单区列表点击事件
            $(document).on('click', '#menu_table img', function () {

                helperPanel.setMsg('png格式; 建议尺寸124*124px');
                if (shell_type == 'shellb') {
                    helperPanel.setMsg('png格式; 建议尺寸124*124px');
                }
                else if (shell_type == 'shellc') {
                    if (2 == $('#menu_table img').index(this)) {
                        helperPanel.setMsg('png格式; 建议尺寸112*100px');
                    }
                    else {
                        helperPanel.setMsg('png格式; 建议尺寸54*54px');
                    }

                } else if (shell_type == 'shellg' || shell_type == 'shellp') {
                    helperPanel.setMsg('png格式; 建议尺寸60*60px');
                } else if (shell_type == 'shells') {
                    if (1 == $('#menu_table img').index(this)) {
                        helperPanel.setMsg('png格式; 建议尺寸78*78px');
                    }
                    else {
                        helperPanel.setMsg('png格式; 建议尺寸67*67px');
                    }
                }

                tbCurrIcon = this;
                upload_mIcon();

                var map = Request.GetAttrMap(this);

                map.image_url = map.src;

                $('#panel, #modifyIcon').show();

                $('#modifyIcon .item:nth-child(3)').hide();

                $('#modifyIcon').css('height', '270px');

                result = Request.GetResourceImages(
				ecid,
				Request.GetQuery('template_id'),
				map.resourcetype,
				1,
				500
			);

                if (result.code == 1) {

                    result.data.items.unshift(map);

                    mdfPaging = new Paging(
					result.data.items,
					 8,
					 '#modifyIcon .imgs img.active',
					 '#modifyIcon .imgs',
					 '#modify_prev',
					 '#modify_next',
					 '<img height="50" width="50" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="{image_url}" imageid="{imageid}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="{resourcetype}" shelltype="{shelltype}" themesimagesremark="{themesimagesremark}" partflag="{partflag}">'
				);

                    mdfPaging.current();

                    $('#modifyIcon .imgs img:first').click();

                }


            });

            function upload_mIcon() {
                //上传图标.用于修改 
                $('#upload_mIcon').click(function () {
                    Request.UploadResourceFile('#upload_mIcon', 'png', ecid, Request.GetQuery('template_id'), $(tbCurrIcon).attr('resourcetype'), function (rs) {

                        if (rs.code == 1) {

                            mdfPaging.insert(true, {
                                src: rs.data.image_url,
                                image_url: rs.data.image_url,
                                imageid: rs.data.imageid,
                                themesimagesremark: rs.data.themesimagesremark,
                                width: 50,
                                height: 50,
                                partflag: $('#modifyIcon .imgs > img.active').attr('partflag')
                            });


                            //$('#modifyIcon .imgs > img').removeClass('active').parent().prepend(img);
                        }
                        else {
                            popAlert(rs.notice);
                        }
                    });
                }).click();
            }



            //点击确定按钮,执行修改图标操作
            $('#modifyIcon_ok').click(function () {

                var curr = $('#modifyIcon .imgs > img.active');

                var backgroundcolor = Request.RGB2Hex($('.bgColors > span.active > i').css('background-color') || curr.attr('backgroundcolor'));
                var shelld_textcolor = Request.RGB2Hex($('.wzColors > span.active > i').css('background-color') || curr.attr('textcolor'));
                var shellp_textcolor = shelld_textcolor;
                var shells_textcolor = shelld_textcolor;
                //var textcolor = Request.RGB2Hex($('.wzys > span.active > i').css('background-color') || curr.attr('textcolor'));
                var textcolor = Request.RGB2Hex($('#rTabCont > li.active .colors2 > span.active > i').css('background-color'))

                var ordernum = $(tbCurrIcon).attr('ordernum');

                //	console.log(ordernum, textcolor);
                var isShellD = /^shelld$/i.test(Request.GetQuery('shell_type'));
                var isShellP = /^shellp$/i.test(Request.GetQuery('shell_type'));
                var isShellS = /^shells$/i.test(Request.GetQuery('shell_type'));
                //表格图标 和 属性值改变
                $(tbCurrIcon).attr({
                    'imageid': curr.attr('imageid'),
                    'src': curr.attr('src'),
                    'themesimagesremark': curr.attr('themesimagesremark'),
                    'backgroundcolor': backgroundcolor,
                    'textcolor': ((isShellD || isShellP || isShellS) ? shelld_textcolor : textcolor)
                });

                //预览区图标文字改变
                var doc = $($('#ylFrame')[0].contentWindow.document);

                var isFunc = $(tbCurrIcon).attr('partflag') == 3;

                doc.find((isFunc ? '#func' : '#menu') + ordernum)
			    .attr('src', $(tbCurrIcon).attr('src')).siblings().html($(tbCurrIcon).attr('text'));

                if (isFunc && /ShellD.html/i.test($('#ylFrame').attr('src'))) {

                    doc.find('#func' + ordernum).siblings('p').css('color', shelld_textcolor).parent().css('background', backgroundcolor);
                }

                if (isFunc && /ShellP.html/i.test($('#ylFrame').attr('src'))) {

                    doc.find('#func' + ordernum).siblings('p').css('color', shelld_textcolor).parent().css('background', backgroundcolor);
                }
                if (isFunc && /ShellS.html/i.test($('#ylFrame').attr('src'))) {

                    doc.find('#func' + ordernum).siblings('p').css('color', shelld_textcolor);
                    $('#ylFrame')[0].contentWindow.updateBgColor(ordernum, backgroundcolor);
                    //                    var obj = self.frames[0];
                    //                    obj.updateBgColor(ordernum, backgroundcolor);
                }
                $('#panel, #modifyIcon').hide();

                $('#modifyIcon .colors2 span:gt(0)').removeClass('active');

            });


            $("#func_table tbody tr td:nth-child(1)").width(114);
            $("#func_table tbody tr td:nth-child(2)").width(377);
            $("#func_table tbody tr td:nth-child(3)").width(96);

            //拖动排序
            $("#func_table tbody").sortable({
                stop: function (event, ui) {
                    var doc = $($('#ylFrame')[0].contentWindow.document);
                    var bodyImg = $("#func_table tbody tr img");
                    for (var index = 0; index < bodyImg.length; index++) {
                        var shellOrdernum = index + 1;
                        //                        if ($(bodyImg.get(index)).attr("resourcetype") == "ShellD_module_pic") {
                        //                            if (shellOrdernum != 1 && shellOrdernum != 2 && shellOrdernum != 8 && shellOrdernum != 9) {
                        //                                alert("不允许挪动位置");
                        //                                return false;
                        //                            }
                        //                        }

                        $(bodyImg.get(index)).attr('ordernum', shellOrdernum);
                        doc.find('#func' + (index + 1)).attr('src', bodyImg.get(index).src).siblings('p').html($(bodyImg.get(index)).parent().siblings().find('input').val());
                    }
                },
                containment: 'parent'

            });

            //生成图片
            $('#createPic').click(function () {

                Request.UploadTxtImages(
				ecid,
				template_id,
				$('#nav_txt').val()
			)
            });

            Helper('#helper_icon', 'png格式; 建议尺寸114*114px<br>APP安装到手机上，显示在手机桌面的图标');

            Helper('#helper_loading', 'png格式; 建议尺寸640*1136px<br>点击APP图标打开APP，看到的第一张图片');

            Helper('#helper_help', '下载左侧示意包，按照示意包整理图片后，打包成ZIP上传<br><br>用途：用户第一次打开APP时，以图片的方式向用户进行简要介绍');

            Helper('#helper_cover', 'png格式; 建议尺寸640*1136px<br>开机画面后是封面，在“广告管理”处进行更新；此处的图片为默认加载中的图片。');

            Helper('#helper_bg', 'png格式; 建议尺寸640*1136px');

            Helper('#helper_zb', '下载左侧示意包，按照示意包整理皮肤后，打包成ZIP上传<br><br>用户：批量上传APP的皮肤');

            Helper('#helper_top', 'png格式; 建议尺寸640*88px');
            Helper('#helper_bottom', 'png格式; 建议尺寸640*88px');

            if (/^shellp$/i.test(Request.GetQuery('shell_type'))) Helper('#helper_bottom', 'png格式; 建议尺寸640*98px');
            if (/^shells$/i.test(Request.GetQuery('shell_type'))) Helper('#helper_bottom', 'png格式; 建议尺寸640*150px');
            Helper('#helper_top_txt', 'png格式; 建议尺寸550*74px');

            //限制输入字数
            //app名称
            $('#input_app_name').bind('input propertychange', function () {
                CheckLength(this, 12);
            });
            //导航文字
            $('#nav_txt').bind('input propertychange', function () {
                CheckLength(this, 20);
            });
            //功能区
            $(document).on('input propertychange', '#func_table input', function () {
                CheckLength(this, 12);
            });
            //菜单区
            $(document).on('input propertychange', '#menu_table input', function () {
                CheckLength(this, 8);
            });


            Global.noDashed();

        });


        (function ($) {

            var initLayout = function () {

                //功能区文字颜色
                ColorPicker('#func_textcolor_select', function (a, hex) {
                    hex = '#' + hex;
                    $('#func_textcolor span').removeClass('active');
                    $('#func_table img').attr('textcolor', hex);
                    $('#ylFrame').contents().find('#funcs p').css('color', hex);
                });

                //菜单区文字颜色
                ColorPicker('#menu_textcolor_select', function (a, hex) {
                    hex = '#' + hex;
                    $('#menu_textcolor span').removeClass('active');
                    $('#menu_table img').attr('textcolor', hex);
                    $('#ylFrame').contents().find('#menu').siblings().find('p').css('color', hex);

                });

                //弹出层文字颜色

                ColorPicker('#panel_textcolor_select', function (a, hex) {
                    hex = '#' + hex;
                    $('.wzColors span').removeClass('active');
                    $('#modifyIcon .imgs > img.active').attr('textcolor', hex);

                });

                //弹出层背景颜色
                ColorPicker('#panel_backgroundcolor_select', function (a, hex) {
                    hex = '#' + hex;
                    $('.bgColors span').removeClass('active');
                    $('#modifyIcon .imgs > img.active').attr('backgroundcolor', hex);

                });
            };

            EYE.register(initLayout, 'init');
        })(jQuery);



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
            <div class="lTitle modify-lt">
                模板名称</div>
            <div class="ylq">
                <iframe id="ylFrame" frameborder="0" scrolling="no" width="100%" height="100%"></iframe>
            </div>
        </div>
        <div class="right r2">
            <ul id="rTab" class="clearfix">
                <li class="active"><a href="#">基础设置</a></li>
                <li><a href="#">导航区</a></li>
                <li><a href="#">功能区</a></li>
                <li><a href="#">菜单区</a></li>
            </ul>
            <ul id="rTabCont" class="rTabCont rr2">
                <li class="tJcpz active" id="tJcpz">
                    <div class="yymc clearfix">
                        <label>
                            应用名称</label>
                        <input id="input_app_name" type="text" value="">
                    </div>
                    <div class="yytb clearfix">
                        <label>
                            应用图标</label>
                        <a class="upload-btn" id="upload_icon">自定义上传</a> <a id="helper_icon" class="yl" href="#">
                        </a>
                        <img id="img_icon" height="55">
                    </div>
                    <div class="kjhm clearfix">
                        <label>
                            开机画面</label>
                        <div class="upload-btn" id="upload_loading">
                            自定义上传</div>
                        <a id="helper_loading" class="yl" href="#"></a>
                        <img id="img_loading" height="80">
                    </div>
                    <div class="xsyd clearfix">
                        <input id="input_help" name="input_help" type="checkbox">
                        <label for="input_help">
                            新手引导</label>
                        <a class="upload-btn" id="upload_help">自定义上传</a> <a class="zbsc-link" href="../UploadSample/help.zip"
                            target="_top">示意包</a> <a id="helper_help" class="yl" href="#"></a>
                        <div class="imgs img_help">
                            <img class="active" id="img_help" height="80">
                        </div>
                    </div>
                    <div class="fm clearfix">
                        <input id="input_cover" name="input_cover" type="checkbox">
                        <label for="input_cover">
                            封 面</label>
                        <a class="upload-btn" id="upload_cover">自定义上传</a> <a id="helper_cover" class="yl"
                            href="#"></a>
                        <div class="imgs img_cover">
                            <img class="active" id="img_cover" height="80">
                        </div>
                    </div>
                    <div class="bj clearfix">
                        <input id="input_bg" name="input_bg" type="checkbox">
                        <label for="input_bg">
                            背 景</label>
                        <a class="upload-btn" id="upload_bg">自定义上传</a> <a id="helper_bg" class="yl" href="#">
                        </a>
                        <div class="imgs img_bg">
                            <img class="active" id="img_bg" height="80">
                        </div>
                    </div>
                    <div class="zbsc clearfix" style="display: none;">
                        <input id="input_zb" name="input_zb" type="checkbox">
                        <label for="input_zb">
                            整包上传</label>
                        <div class="upload-btn" id="upload_package">
                            整包上传</div>
                        <a href="#" style="display: none;" target="_blank" class="zbsc-link">示意包</a> <a id="helper_zb"
                            class="yl" style="display: none;" href="#"></a>
                    </div>
                    <a href="#" class="u-btnGreen bcyl">保存预览</a> </li>
                <li>
                    <div class="bj" style="position: relative;">
                        <div class="small-head">
                            背景</div>
                        <div class="bj-imgs" style="padding: 0 50px;">
                        </div>
                        <div class="b3 clearfix">
                            <div class="upload-btn" id="upload_top">
                                自定义上传</div>
                            <a id="helper_top" class="yl" href="#"></a>
                            <img height="80">
                        </div>
                        <div id="navPrev" class="prevPage" style="top: 39px;">
                        </div>
                        <div id="navNext" class="nextPage" style="top: 39px;">
                        </div>
                    </div>
                    <div class="wz">
                        <div class="small-head">
                            文字</div>
                        <div class="wz-radios">
                            <label>
                                <input name="wz" type="radio" value="0">文字</label>
                            <label>
                                <input name="wz" type="radio" value="1">图片</label>
                            <label>
                                <input name="wz" checked type="radio" value="2">隐藏</label>
                        </div>
                        <ul class="wz-cont">
                            <li>
                                <input id="nav_txt" type="text" value="无线自由 掌握成功">
                                <a id="createPic">生成图片</a> </li>
                            <li>
                                <div class="upload-btn" id="upload_top_txt" style="display: inline-block">
                                    自定义上传</div>
                                <a id="helper_top_txt" style="display: inline-block; vertical-align: top; margin-left: 1px;"
                                    class="yl" href="#"></a>
                                <div id="top_txt_div">
                                    <img id="top_txt">
                                </div>
                            </li>
                            <li class="active"></li>
                        </ul>
                    </div>
                    <a href="#" class="u-btnGreen bcyl" style="margin-top: 10px;">保存预览</a> </li>
                <li>
                    <div class="clearfix">
                        <a class="u-btnBlue pull-right tjgn">添加功能</a>
                    </div>
                    <div class="wzys clearfix">
                        <label>
                            文字颜色</label>
                        <div id="func_textcolor" class="colors2 clearfix">
                            <span class="colors2-1"><i></i></span><span class="colors2-2"><i></i></span><span
                                class="colors2-3"><i></i></span><span class="colors2-4"><i></i></span><span class="colors2-5">
                                    <i></i></span><span class="colors2-6"><i></i></span><span class="colors2-7"><i></i>
                            </span>
                        </div>
                        <img id="func_textcolor_select" src="images/color.png" class="color_select"/>
                    </div>
                    <div class="func-table-wrap" style="height: 440px;">
                        <div style="font-weight: bold;">
                            <p style="float: left; width: 130px;">
                                图标</p>
                            <p style="float: left;">
                                名称</p>
                        </div>
                        <table id="func_table" class="table table-responsive table-striped func-table">
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div class="textTips" style="text-align: center; color: #f60;">
                    </div>
                    <a href="#" class="u-btnGreen bcyl">保存预览</a> </li>
                <li>
                    <div class="clearfix">
                        <a class="u-btnBlue pull-right tjgn">添加功能</a>
                    </div>
                    <div class="wzys clearfix">
                        <label>
                            文字颜色</label>
                        <div id="menu_textcolor" class="colors2 clearfix">
                            <span class="colors2-1"><i></i></span><span class="colors2-2"><i></i></span><span
                                class="colors2-3"><i></i></span><span class="colors2-4"><i></i></span><span class="colors2-5">
                                    <i></i></span><span class="colors2-6"><i></i></span><span class="colors2-7"><i></i>
                            </span>
                        </div>
                        <img id="menu_textcolor_select" src="images/color.png" class="color_select">
                    </div>
                    <div class="wzys clearfix">
                        <label>
                            菜单背景</label>
                        <div class="cd-imgs">
                        </div>
                        <div class="cdbj-zdy clearfix">
                            <div class="upload-btn" id="upload_menu_bg" style="line-height: 22px;">
                                自定义上传</div>
                        </div>
                        <a id="helper_bottom" class="yl" href="#"></a>
                    </div>
                    <div class="func-table-wrap-noheight">
                        <div style="font-weight: bold;">
                            <p style="float: left; width: 130px;">
                                图标</p>
                            <p style="float: left;">
                                名称</p>
                        </div>
                        <table id="menu_table" class="table table-responsive table-striped func-table">
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <a href="#" class="u-btnGreen bcyl">保存预览</a> </li>
            </ul>
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
                        <div class="setupbox">
                            <div class="setuprow">
                                <span class="setupcell1">推送证书</span> <span class="setupcell2"></span><span class="setupcell3">
                                </span>
                                <div class="setupcell4">
                                    <input type="file"/>
                                    <button>
                                        上传证书</button>
                                </div>
                                <span class="setupcell5"></span>
                            </div>
                            <div class="setuprow" style="display: none;">
                                <span class="setupcell1">微信分享</span> <span class="setupcell2"></span><span class="setupcell3">
                                </span>
                                <div class="setupcell4">
                                    <input type="text">
                                </div>
                                <span class="setupcell5 success"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 修改图标 . 上传 -->
    <div id="panel">
        <div id="modifyIcon">
            <div class="modifyIcon">
                <div class="item" style="position: relative;">
                    <div class="head">
                        已有</div>
                    <div class="imgs" style="display: block; height: 60px; padding: 0 40px;">
                    </div>
                    <div class="prevPage" id="modify_prev" style="top: 47px; width: 40px; height: 50px;">
                    </div>
                    <div class="nextPage" id="modify_next" style="top: 47px; width: 40px; height: 50px;">
                    </div>
                </div>
                <div class="item">
                    <div class="head">
                        上传</div>
                    <div class="cont">
                        <div class="upload-btn" id="upload_mIcon">
                            自定义上传</div>
                        <div>
                            <a id="helper_panel" class="yl" href="#"></a>
                        </div>
                    </div>
                </div>
                <div class="item wzys2">
                    <div class="head">
                        文字颜色</div>
                    <div class="cont">
                        <div class="wzColors colors2 clearfix">
                            <span></span><span class="colors2-1"><i></i></span><span class="colors2-2"><i></i>
                            </span><span class="colors2-3"><i></i></span><span class="colors2-4"><i></i></span>
                            <span class="colors2-5"><i></i></span><span class="colors2-6"><i></i></span><span
                                class="colors2-7"><i></i></span>
                        </div>
                        <img id="panel_textcolor_select" src="images/color.png" class="color_select">
                    </div>
                </div>
                <div class="item wzys3">
                    <div class="head">
                        背景颜色</div>
                    <div class="cont">
                        <div class="bgColors colors2 clearfix">
                            <span></span><span class="colors2-1"><i></i></span><span class="colors2-2"><i></i>
                            </span><span class="colors2-3"><i></i></span><span class="colors2-4"><i></i></span>
                            <span class="colors2-5"><i></i></span><span class="colors2-6"><i></i></span><span
                                class="colors2-7"><i></i></span>
                        </div>
                        <img id="panel_backgroundcolor_select" src="images/color.png" class="color_select">
                    </div>
                </div>
                <div class="btns">
                    <div class="u-btnBlue" id="modifyIcon_ok">
                        确定</div>
                    <div class="u-btnBlue u-btnGray" onclick="$('#panel, #modifyIcon').hide();">
                        取消</div>
                </div>
            </div>
        </div>
</body>
</html>
