//步骤1
var Step1 = {
    //进入第二步骤被点击
    gotostep2_onclick: function (ecid) {
        $('#goToStep2_1').click(function () {
            //old
            //var template_id = $('#AppTemplate > li.active').attr('template_id');
            //刘迎修改，不切换微信和微站模板 start
            var template_id = $('#AppTemplate > li.active').attr('template_id');
            //end
            var code = Request.ProcessGenerateResource(ecid, template_id);

            if (code == 1) {
                var type = Request.GetQuery('type');
                var agent_ecid = Request.GetQuery('agent_ecid');
                $(this).attr("href", 'Step2.aspx?ecid=' + ecid + '&tag=' + new Date().getTime() + '&template_id=' + template_id + '&type=' + type + '&agent_ecid=' + agent_ecid);
                return true;
            }
            else if (code == 6) {
                popAlert('您暂未购买套餐，或套餐中的产品不足，如需帮助，请联系管理员。');
                return false;
            } else {
                popAlert("模板保存失败");
                return false;
            }
        });
    },
    //界面模板被点击
    jmmb_onclick: function () {
        $('#jmmb').click(function () {
            $('#lTab > li').show();
            $('#lTabCont > li').removeClass('active').first().addClass('active');
        });
    },
    //基础配置被点击
    jcpz_onclick: function () {
        $('#jcpz').click(function () {
            $('#lTab > li').hide().first().show();
            $('#lTabCont > li').removeClass('active').last().addClass('active');
        });
    },
    //模板被选择
    mbPic_onclick: function (ecid, callback) {
        //        $(document).on('click', '#AppTemplate > li > p', function () {
        //            if (Request.SwitchTemplateCheck(ecid, $('#AppTemplate > li.active').attr('template_id'))) {
        //                $('#AppTemplate > li').removeClass('active');

        //                var activeMb = $(this).parent().addClass('active');

        //                var type = $('#lTab > li.active').attr('type');

        //                var querystring = activeMb.attr('querystring');

        //                //左侧4个链接赋值
        //                $.each(['app', 'weixin', 'mp', ''], function () {

        //                    $('#lTab > li[type=' + this + '] > a').attr('href', activeMb.attr(this + '_url') + querystring);

        //                });

        //                $('#mbFrame').attr('src', activeMb.attr(type + '_url') + querystring);


        //                if (callback) {
        //                    callback();
        //                }
        //            }
        //        });

        $(document).on('click', '#AppTemplate > li > p', function () {

            //点击已选择节点不作处理
            if ($(this).parent().attr('class') == 'active') {
                return;
            }

            var template_id = $('#AppTemplate > li.active').attr('template_id');
            var param = {
                'ecid': ecid,
                'template_id': template_id
            };

            var type = $('#lTab > li.active').attr('type');

            //刘迎修改，不切换微信和微站模板 start
            if (type != 'app') {
                return;
            }
            //lyy end
            var result = Request.Sync('/AppTemplate/SwitchTemplateCheck', param);

            var currentTemplate = $(this);
            if (result.code == 6) {
                popConfirm('您之前配置过模板，如果更换模版将清空之前的模版配置。\r\n是否继续？', function (res) {
                    if (res == true) {
                        result = Request.Sync('/AppTemplate/DeleteTemplateConfig', param);

                        $('#AppTemplate > li').removeClass('active');
                        var activeMb = currentTemplate.parent().addClass('active');
                        var type = $('#lTab > li.active').attr('type');
                        $('#AppTemplate > li').removeAttr('app_select');
                        $(this).parent().attr('app_select', 'true');
                        var querystring = activeMb.attr('querystring');

                        //左侧4个链接赋值
                        $.each(['app', 'weixin', 'mp', ''], function () {

                            $('#lTab > li[type=' + this + '] > a').attr('href', activeMb.attr(this + '_url') + querystring);

                        });

                        $('#mbFrame').attr('src', activeMb.attr(type + '_url') + querystring);

                        if (callback) {
                            callback();
                        }
                    }
                });
            } else {
                $('#AppTemplate > li').removeClass('active');
                var activeMb = $(this).parent().addClass('active');


                var querystring = activeMb.attr('querystring');

                //左侧4个链接赋值
                $.each(['app', 'weixin', 'mp', ''], function () {

                    $('#lTab > li[type=' + this + '] > a').attr('href', activeMb.attr(this + '_url') + querystring);

                });

                $('#mbFrame').attr('src', activeMb.attr(type + '_url') + querystring);


                if (callback) {
                    callback();
                }
            }
        });
    },
    //新手引导和封面checkbox选择
    checkbox_onclick: function () {
        $('#tJcpz > div > input[type=checkbox]').click(function () {
            if (this.checked) {
                $(this).siblings('.upload-btn, .imgs').show();
                //如果没有默认的，就将第一个为默认
                var tmp = $(this).siblings('.imgs').find('img.active');
                if (tmp.length == 0 || tmp.is(':hidden')) {
                    $(this).siblings('.imgs').find('img').removeClass('active');
                    $(this).siblings('.imgs').find('img:visible:first').addClass('active');
                }
            }
            else {
                $(this).siblings('.upload-btn, .imgs').hide();
            }
        });
    },
    //文字 - 已有，图片，隐藏
    wordSwitch: function () {
        $('.wz-radios input').click(function () {
            var i = $('.wz-radios input').index(this);
            $('.wz-cont > *').removeClass('active').eq(i).addClass('active');

            var doc = $($('#ylFrame')[0].contentWindow.document);
            if (i == 2) {
                doc.find('#top_txt').hide();
            } else {
                doc.find('#top_txt').show();
            }
        });
    },
    //弹出层 - 添加功能 - 导航展开 收缩效果
    tjgnNavSlide: function () {
        //隐藏非active的子菜单
        $('#pop-tjgn .p-nav > li:not(.active) ul').hide();

        $(document).on('click', '#pop-tjgn .p-nav h2', function () {

            //当前子菜单展开,收缩其它子菜单.一级菜单箭头转向
            $(this).siblings().slideToggle('fast').parent().toggleClass('active').siblings().removeClass('active').find('ul').slideUp('fast');

        });
    },
    //点击添加功能，弹出模态层
    popTjgn: function (callback) {
        $('a.tjgn').click(function () {
            if (!$('#tjgnFrame').attr('src')) {
                $('#tjgnFrame').attr('src', $('#pop-tjgn .p-nav > li.active:first ul a').attr('href'));
            }
            $('#pop-tjgn').show();
            callback();
        });
        $('#pop-tjgn b').click(function () {
            $('#pop-tjgn').hide();
        });
    },
    //小图片选择切换
    imgs_onclick: function () {
        $(document).on('click', '#tJcpz > div > .imgs > img, .bj-imgs > img, .cd-imgs > img, #modifyIcon .imgs > img', function () {
            $(this).addClass('active').siblings().removeClass('active');
        });
    },
    //获取小图片资源
    getImgs: function (ecid, template_id) {
        var type = ['help', 'cover', 'bg'];
        $.each(type, function () {
            var result = Request.GetResourceImages(
				ecid,
				template_id,
				this,
				1,
				5
			);
            //console.log(result);
            if (result.code == 1) {
                var tmp = $('#tJcpz > div > .img_' + this);
                var first = tmp.find('img:first');
                tmp.empty().append(first).append(
					Request.formats('<img src="{image_url}" imageid="{imageid}" themesimagesremark="{themesimagesremark}" height="80">', result.data.items)
				);
            }
            else {
                //第二张开始清除
                $('#tJcpz > div > .img_' + this + ' > img:gt(0)').empty();
            }
        });
    },
    //取消搜索
    cancelSearch: function () {
        $('#pop-tjgn .p-searchBox span').click(function () {
            $('#pop-search').val('');
            $('#pop-tjgn .p-nav li').show();
        });
    },
    //回车搜索
    enterSearch: function () {
        $('#pop-search').keyup(function (e) {
            if (e.keyCode == 13) {
                $('#pop-tjgn .p-nav li').show();
                var text = $.trim($(this).val());
                $('#pop-tjgn .p-nav > li > ul li a').each(function (index, element) {
                    if ($(this).text().indexOf(text) == -1) {
                        $(this).parent().hide();
                    }
                });

                $('#pop-tjgn .p-nav > li').each(function (index, element) {

                    var count = 0;

                    $(this).find('ul li').filter(function (index) {
                        if ($(this).css('display') == 'list-item') {
                            count++;
                        }
                    });;

                    if (count == 0) {
                        $(this).hide();
                    }

                });

                $('#pop-tjgn .p-nav > li').each(function (index) {
                    if ($(this).css('display') == 'list-item') {
                        if (!$(this).hasClass('active')) {
                            $(this).find('h2').click();
                        }
                        return false;
                    }
                });

            }
        });
    },
    tjgnSetup: function () {
        //折叠展开面板
        $('#pop-tjgn .setupbar').click(function () {
            $(this).siblings('.setupbox').toggle();
        });
        //开关,显示隐藏后面元素
        $('#pop-tjgn .setupcell3').click(function () {
            $(this).toggleClass('on').nextAll().toggle();
        });
        //遍历开关,显示隐藏后面元素
        $('#pop-tjgn .setupcell3').each(function (index, element) {
            if ($(this).hasClass('on')) {
                $(this).nextAll().show();
            }
            else {
                $(this).nextAll().hide();
            }
        });
    }

}