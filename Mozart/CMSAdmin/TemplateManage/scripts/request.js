/* 页面忽略报错,  继续执行. */
//window.onerror = function () { return true; };

var Request = {

    Sync: function (url, param) {
        ///	<summary>
        /// 同步请求,约定为json类型
        ///	</summary>
        ///	<param name="param" type="Object">
        /// {key:"value"}
        ///	</param>
        var resultObj = null;

        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(param),
            async: false,
            processData: false,
            timeout: 10000,
            cache: false,
            success: function (data, textStatus) {
                resultObj = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                XMLHttpRequest = null;
            },
            complete: function (XMLHttpRequest, textStatus) {
                XMLHttpRequest = null;
            },
            dataType: 'json'
        });

        return resultObj;
    },

    Async: function (url, param, callBack) {
        ///	<summary>
        /// 异步请求,约定为json类型
        ///	</summary>
        ///	<param name="param" type="Object">
        /// {key:"value"}
        ///	</param>

        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(param),
            async: true,
            processData: false,
            timeout: 10000,
            cache: false,
            success: callBack,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                XMLHttpRequest = null;
            },
            complete: function (XMLHttpRequest, textStatus) {
                XMLHttpRequest = null;
            },
            dataType: 'json'
        });
    },
    // jquery css()取出来的颜色是rgb(x,x,x)的形式,转成16进制的
    RGB2Hex: function (rgb) {
        if (typeof rgb == 'undefined') {
            return '';
        }
        if (!/^[#t]/.test(rgb)) {// IE返回的是16进制，其它浏览器是rgb()形式,需要转为16进制
            rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/i);
            function hex(x) { return ("0" + parseInt(x).toString(16)).slice(-2); };
            if (rgb) {
                rgb = "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
            }
            else {
                return '';
            }
        }
        return rgb;
    },

    // 格式化占位
    format: function (text, data) {
        for (var prop in data) {
            text = text.replace(new RegExp('{' + prop + '}', 'g'), data[prop]);
        }
        return text.replace(/ (.*?)="\{\1\}"/g, '');   //text.replace(/\{.*?\}/g, '');  //text.replace(/ (.*?)="\{\1\}"/g, '');  //.replace(/\{.*?\}/g, '');
    },

    formats: function (text, data) {
        var ret = [];
        $.each(data, function () {
            ret.push(Request.format(text, this));
        });
        return ret.join('');
    },

    //1.1	获取行业类别
    ///AppTemplate/GetBizType


    GetBizType: function (ecid) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetBizType.ashx', { 'ecid': ecid });

        if (result.code == 1) {
            $('.ulBizType').html(
				Request.formats('<li value="{biz_id}" self_biz_id="{self_biz_id}" template_count="{template_count}">{biz_name}</li>', result.data)
			);
            $(".ulBizType>li").each(function () {
                if ($(this).attr("template_count") == "0") {
                    $(this).addClass("ulBizTypeUnline");
                }
                if ($(this).attr("value") == $(this).attr("self_biz_id")) {
                    $(this).addClass("ulBizTypeSelect");
                }
            });

            $('#BizType').html(
				'<option value="-1" selected>全部</option>' +

				Request.formats('<option value="{biz_id}">{biz_name}</option>', result.data)

			);
        }

    },

    //1.2	获取模版
    // /AppTemplate/GetAppTemplate

    GetAppTemplate: function (ecid, biz_id, callback) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/GetAppTemplate.ashx', { 'ecid': ecid, 'biz_id': biz_id, 'page_index': 1, 'page_size': 9999 });

        //console.log(result);

        if (result.code == 1) {
            $('#AppTemplate').html(

				Request.formats('<li template_id="{template_id}" description="{description}" app_url="{shell_type}.html" weixin_url="weixin.html" mp_url="http://mod.f3.cn/mpbwap/homea.aspx" querystring="?template_id={template_id}&ecid=' + ecid + '&referer=Step1" enabled_flag="{enabled_flag}" template_name="{template_name}"><p><img width="128" height="227"  app_image_url="{app_image_url}" weixin_image_url="{weixin_image_url}" mp_image_url="{mp_image_url}"></p><a href="modify_.aspx?template_id={template_id}&shell_type={shell_type}&ecid=' + ecid + '"></a></li>', result.data.items)
			);
            //置选中模板
            var t = $('#AppTemplate > li[enabled_flag=1]');

            if (t.length) {
                t.addClass('active');
                t.attr('app_select', 'true');
            }
            else {
                $('#AppTemplate > li:first').addClass('active');
                $('#AppTemplate > li:first').attr('app_select', 'true');
            }

            //根据左侧选中的平台.来给模板设置不同的图片和modify链接
            var type = $('#lTab > li.active').attr('type');

            //更换模板图片
            $('#AppTemplate img').each(function (index, element) {
                $(this).attr('src', $(this).attr(type + '_image_url'));
            });

            //更换修改页链接
            $('#AppTemplate a').each(function (index, element) {

                var href = this.href.replace(/modify_\w*?\.aspx/i, 'modify_' + type + '.aspx');

                $(this).attr('href', href);
            });

            var activeMb = $('#AppTemplate > li.active');

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
        else if (result.code == 4) {
            $('#AppTemplate').empty();
        }


    },

    //2.1	获取基础配置
    ///ResourceConfig/GetBaseConfig

    GetBaseConfig: function (ecid, template_id) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetBaseConfig.ashx', {
            'ecid': ecid,
            'template_id': template_id
        });


        //console.log( result );

        //应用名称
        $('#input_app_name').val(result.data ? result.data.app_name : '');
        $('.lTitle').html($('#input_app_name').val());

        //先隐藏基础配置中的图片
        $('#tJcpz .imgs, #tJcpz img').hide();

        //隐藏上传按钮
        $('#tJcpz .upload-btn:gt(1)').hide();

        //取消勾选
        $('#tJcpz input[type=checkbox]').prop('checked', false);

        if (result.code == 1) {

            //遍历,有返回的,就赋值显示
            $.each(result.data.listgenerateimages, function () {
                //图片
                $('#img_' + this['resourcetype']).attr({
                    'src': this['image_url'],
                    'imageid': this['imageid'],
                    'themesimagesremark': this['themesimagesremark']
                }).show().addClass('active').parent().show();
                //选框  .上传按钮
                $('#input_' + this['resourcetype']).prop('checked', true).siblings('.upload-btn').show();
            });


        }

    },

    //3.1	获取资源图片
    // / ResourceAdmin/GetResourceImages
    GetResourceImages: function (ecid, template_id, resourcetype, page_index, page_size) {
        return Request.Sync('/CMSAdmin/TemplateManage/GetResourceImages.ashx', {
            'ecid': ecid,
            'template_id': template_id,
            'resourcetype': resourcetype,
            'page_index': page_index,
            'page_size': page_size
        });
    },

    GetQuery: function (name) {
        return window.location.search.match(new RegExp('(?:\\?|&)' + name + '=([^&]*)(?:&|$)', 'i')) ? RegExp.$1 : '';
    },

    //2.3	获取资源配置项
    ///ResourceConfig /GetGenerateResource
    GetGenerateResource: function (ecid) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetGenerateResource.ashx', {
            'ecid': ecid,
            'template_id': Request.GetQuery('template_id')
        });

        //console.log('result:',result);


        var tmp = [
					'<img width="33" height="33" function_id="{function_id}" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="{image_url}" imageid="{imageid}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="{resourcetype}" shelltype="{shelltype}" textcolor="{textcolor}" themesimagesremark="{themesimagesremark}" partflag="{partflag}" ',

					'<tr title="{showbizmodulename}"><td><img height="50" function_id="{function_id}" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="{image_url}" imageid="{imageid}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="{resourcetype}" shelltype="{shelltype}" textcolor="{textcolor}" themesimagesremark="{themesimagesremark}" partflag="{partflag}" text="{text}" ishome="{ishome}"></td><td><input type="text" value="{text}"></td><td ishome="{ishome}"><a class="btn_del mt-6px"></a></td></tr>'

        ];

        if (result.code == 1) {

            //Array copy
            var rs = [];
            $.extend(true, rs, result);

            //加载iframe资源
            $('#ylFrame').one('load', function (e) {

                var doc = $(this.contentWindow.document);

                try {
                    doc.find('.phone').css('background', 'url(' + rs.data.bg.image_url + ')').css("background-size", "cover");
                } catch (e) { }
                //导航背景						
                try {
                    doc.find('#top').attr('src', rs.data.nav[0].image_url);
                } catch (e) { };
                //导航文字
                try {
                    doc.find('#top_txt').attr('src', rs.data.nav[1].image_url);
                } catch (e) {
                    doc.find('#top_txt').hide();
                };
                //功能区
                //文字颜色
                try {
                    doc.find('.func_p p').css('color', rs.data.fun[0]['textcolor']);
                } catch (e) { };


                var isShellD = /ShellD.html/i.test($(this).attr('src'));
                var isShellP = /ShellP.html/i.test($(this).attr('src'));
                var isShellS = /ShellS.html/i.test($(this).attr('src'));


                //图标
                $.each(rs.data.fun, function () {

                    doc.find('#func' + this.ordernum).attr('src', this.image_url).siblings().html(this.text);
                    //ShellD 功能区设置背景
                    if (isShellD || isShellP) {
                        //设置底座背景色
                        doc.find('#func' + this.ordernum).attr('src', this.image_url).siblings().html(this.text).css('color', this.textcolor);
                        doc.find('#func' + this.ordernum).parent().css('background-color', this.backgroundcolor);

                    }
                    if (isShellS) {
                        doc.find('#func' + this.ordernum).attr('src', this.image_url).siblings().html(this.text).css('color', this.textcolor);
                        $('#ylFrame')[0].contentWindow.updateBgColor(this.ordernum, this.backgroundcolor);
                    }
                });

                if (isShellD || isShellP || isShellS) {
                    for (var i = 0, len = rs.data.fun.length; i < len; i++) {
                        doc.find('.func_p p').eq(i).css('color', rs.data.fun[i].textcolor);
                    }
                }


                //菜单区
                //文字颜色
                if (rs.data.menu != '') {
                    //背景
                    try {
                        doc.find('.menu_p p').css('color', rs.data.menu[1]['textcolor']);
                    } catch (e) { };

                    doc.find('#menu').attr('src', rs.data.menu[0]['image_url']);
                    if (/shellb.html/i.test($(this).attr('src'))) {
                        doc.find('#menu').siblings().find('li').width(100 / (rs.data.menu.length - 1) + '%');
                    }

                }
                rs.data.menu.shift(); //shift获取第一个值不准确.所以上一句用[0]

                //图标
                $.each(rs.data.menu, function () {
                    var orderNum = this.ordernum;
                    if (rs.data.menu.length == 2) {
                        orderNum = 2;
                    }
                    doc.find('#menu' + orderNum).attr('src', this.image_url).siblings().html(this.text);
                });
            });


            //导航区第一个
            var navFirst = result.data.nav[0];
            $('.bj-imgs').html(
				Request.format(tmp[0] + 'class="active">', result.data.nav[0])
			);

            $.each(result.data.nav, function () {
                if (this.resourcetype == 'top_txt') {
                    //导航区底部
                    $('#top_txt_div').html(
						Request.format(tmp[0] + '>', this)
					).find('img').attr({
					    'width': 180,
					    'height': 24
					});


                    if (this.text == '') {
                        $('.wz-radios input[name=wz][value=1]').attr('checked', true);
                        $('.wz-cont > li').removeClass('active').eq(1).addClass('active');
                    }
                    else {
                        $('.wz-radios input[name=wz][value=0]').attr('checked', true);
                        $('.wz-cont > li').removeClass('active').eq(0).addClass('active');
                        $('#nav_txt').val(this.text);

                    }


                    return false;
                }
            });


            var fTc = '';
            try {
                fTc = result.data.fun[0]['textcolor'];
            } catch (e) { }

            //功能区
            //生成一个选中textcolor色	
            $('#func_textcolor').prepend('<span class="active"><i style="background:' + fTc + '"></i></span>');

            var isShellD = /^shelld$/i.test(Request.GetQuery('shell_type'));
            var isShellE = /^shelle$/i.test(Request.GetQuery('shell_type'));
            var isShellF = /^shellf$/i.test(Request.GetQuery('shell_type'));
            var isShellR = /^shellf$/i.test(Request.GetQuery('shell_type'));
            //表格
            $('#func_table > tbody').html(
				Request.formats(tmp[1], result.data.fun)
			);
            if (result.data.menu != '') {
                //菜单
                //生成一个选中textcolor色	
                var mTc = '';
                try {
                    mTc = result.data.menu[1]['textcolor'];
                } catch (e) { }

                $('#menu_textcolor').prepend('<span class="active"><i style="background:' + mTc + '"></i></span>');

                //菜单背景 图
                var menuBgResourceType = result.data.menu[0]['resourcetype'];
                //console.log(result.data.menu[0]);
                $('.cd-imgs').html(
					Request.format(tmp[0] + 'class="active">', result.data.menu[0])
				);

                result.data.menu.shift(); //shift获取第一个值不准确.所以上一句用[0]
                //console.log(result.data.menu);
                //菜单区 表格
                $('#menu_table > tbody').html(
					Request.formats(tmp[1], result.data.menu)
				);

            }
            if (/^ShellD$/i.test(Request.GetQuery('shell_type')) || /^ShellR$/i.test(Request.GetQuery('shell_type'))) {
                $('#rTab > li:nth-child(4)').hide();
            }
            else {
                $('#rTab > li:nth-child(4)').show();
            }
            //ishome 没有删除按钮
            $('#menu_table td[ishome=true] a.btn_del').hide();

        }


        //导航区预选资源获取
        result = Request.GetResourceImages(
			ecid,
			Request.GetQuery('template_id'),
			'top',
			1,
			9
		);

        //console.log( result );

        if (result.code == 1) {

            result.data.items.unshift(navFirst);

            navPaging = new Paging(
				result.data.items,
				10,
				'.bj-imgs > img.active',
				'.bj-imgs',
				'#navPrev',
				'#navNext',
				'<img width="33" height="33" function_id="{function_id}" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="{image_url}" imageid="{imageid}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="{resourcetype}" shelltype="{shelltype}" textcolor="{textcolor}" themesimagesremark="{themesimagesremark}" partflag="2">'
			);

            navPaging.current();


        }
        //--------

        //菜单区预选资源获取

        result = Request.GetResourceImages(
			ecid,
			Request.GetQuery('template_id'),
			menuBgResourceType,
			1,
			10
		);

        //console.log( result );

        if (result.code == 1) {
            $('.cd-imgs').append(
				Request.formats(tmp[0] + '>', result.data.items)
			);
        }





        //result = tmp = null;
    },

    //把标签属性转为json对象形式
    GetAttrMap: function (elem) {
        var map = {};
        $.each(elem.attributes || elem[0].attributes, function () {
            map[this.name] = this.value;
        });
        return map;
    },
    //把一组标签属性转为json对象数组形式
    GetAttrMaps: function (elems) {
        var maps = [];
        $.each(elems, function () {
            maps.push(Request.GetAttrMap(this));
        });
        return maps;
    },

    //2.4	保存资源配置项
    ///ResourceConfig /SaveGenerateResource
    SaveGenerateResource: function (ecid, template_id) {
        //资源配置
        //选中的所有选择器,组合在一起提交
        //导航区背景 .bj-imgs > img.active
        //导航区文字 .wz-cont > li.active img
        //功能区表格数据 #func_table img
        //菜单区背景 .cd-imgs > img.active
        //菜单区表格数据 #menu_table img
        var listgenerateimages = Request.GetAttrMaps($('.bj-imgs > img.active, #func_table img, .cd-imgs > img.active, #menu_table img'));

        //'.wz-cont li.active img'
        if (parseInt($('.wz-radios input[name=wz]:checked').val()) < 2) {
            listgenerateimages.push(Request.GetAttrMap($('#top_txt_div > img')));
        }

        //基础配置 partflag = 1
        listgenerateimages.push({
            'imageid': $('#img_icon').attr('imageid'),
            'resourcetype': 'icon',
            'themesimagesremark': $('#img_icon').attr('themesimagesremark'),
            'partflag': 1
        });

        listgenerateimages.push({
            'imageid': $('#img_loading').attr('imageid'),
            'resourcetype': 'loading',
            'themesimagesremark': $('#img_loading').attr('themesimagesremark'),
            'partflag': 1
        });

        //可选项. 只提交选中的
        var type = ['help', 'cover', 'bg'];
        for (var i = 0; i < type.length; i++) {
            var curr = type[i];
            if ($('#input_' + curr).prop('checked')) {
                var tmp = $('#input_' + curr).siblings('.imgs').find('img.active');
                listgenerateimages.push({
                    'imageid': tmp.attr('imageid'),
                    'resourcetype': curr,
                    'themesimagesremark': tmp.attr('themesimagesremark'),
                    'partflag': 1
                });
            }
        }



        var param = {
            'ecid': ecid,
            'template_id': template_id,
            'app_name': $('#input_app_name').val(),
            'listgenerateimages': listgenerateimages
        }


        //console.log(param);
        var result = Request.Sync('/CMSAdmin/TemplateManage/SaveGenerateResource.ashx', param);

        //console.log(result);
        return result.code;

    },

    //2.5	获取业务模块菜单
    /// ResourceConfig /GetBizModuleList
    GetBizModuleListMark: true,
    GetBizModuleList: function (ecid, platform, callback) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetBizModuleList.ashx', { 'ecid': ecid, 'template_id': Request.GetQuery('template_id'), 'platform': platform });

        //console.log(result);

        if (result.code == 1) {

            //ModuleList
            var html = [];

            for (var i = 0, len = result.data.length; i < len; i++) {

                var curr = result.data[i];

                var nextLevel = i + 1 == len ? 1 : result.data[i + 1].level;

                //父
                if (curr.level == 1) {
                    //加入
                    html.push('<li><h2><i></i>' + curr.bizmodulename + '</h2>');
                    //判断下一个是父
                    if (nextLevel == 1) {
                        html.push('</li>');
                    }
                    else if (nextLevel == 2) {
                        html.push('<ul>');
                    }

                }
                else if (curr.level == 2) {

                    var deleteHtml = "";
                    //已有功能并发布的加上删除处理
                    if (curr.published == 0 && curr.biztypeid == -1)
                        deleteHtml = '<div class="delete-btn" onclick="deleteModuleType(this,' + curr.moduletypeid + ',\'' + curr.bizmodulename + '\')">删除</div>';

                    html.push('<li> ' + deleteHtml + ' <a class="modulenode" title="' + curr.moduletypeid + curr.showbizmodulename + '" moduletypeid=' + curr.moduletypeid + ' published=' + curr.published + ' wapurl="' + curr.wapurl + '" moduleid="' + curr.moduleid + '" bizmoduleid="' + curr.bizmoduleid + '">' + curr.bizmodulename + '</a> </li>');

                    if (nextLevel == 1) {
                        html.push('</ul></li>');
                    }

                }
            }


            $('#ModuleList').html(html.join(' '));

            if (Request.GetBizModuleListMark) {
                $('.tjgn-content').css('visibility', 'hidden');
                Request.GetBizModuleListMark = false;
            }
            //展开最近的一个子菜单
            //$('#ModuleList > li > ul').first().show().parent().addClass('active').find('a:first').addClass('active');
            /*
            Request.GetBizModuleListDetail(ecid,
            $('#ModuleList > li > ul').first().show().parent().addClass('active').find('a:first').addClass('active').attr('bizmoduleid')
            );*/
            /*
            Request.GetBizModuleListDetail(ecid,
            $('#ModuleList > li a:first').addClass('active').attr('bizmoduleid')
            );*/


            //隐藏非active的子菜单
            $('#pop-tjgn .p-nav > li:not(.active) ul').hide();

            if (callback) {
                callback();
            }

        }

    },

    //2.6	获取业务模块详细信息
    /// ResourceConfig / GetBizModuleListDetail

    GetBizModuleListDetail: function (ecid, template_id, bizmoduleid) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetBizModuleListDetail.ashx', {
            'ecid': ecid,
            'bizmoduleid': bizmoduleid,
            'template_id': template_id
        });


        if (result.code == 1) {

            $('.tjgn-content').css('visibility', 'visible');

            $('.tjgn-h-title').attr('permissions', result.data.permissions);

            //设置图标的亮和暗
            $.each('iphone|android|weixin|mp|sms'.split('|'), function () {
                $('#ticon-' + this)[result.data[this] ? 'addClass' : 'removeClass']('active');
            });

            //设置详情标题
            //$('.tjgn-h-title').html(result.data.bizmodulename);
            $('.tjgn-h-title').html(
				$('#ModuleList a.active').html()
			);


            //设置详情内容
            $('#bizdescription').html(result.data.bizdescription);

            //设置三张图片
            //先清空
            for (var i = 1; i <= 3; i++) {
                $('#timage' + i).attr('src', '').hide();
            }

            for (var i = 1; i <= 3; i++) {
                if (result.data['image' + i + '_url']) {
                    $('#timage' + i).attr('src', result.data['image' + i + '_url']).show();
                }
            }

            //设置是否带文本框 1=有,0=无
            $('#moduleurl')[result.data.customurlflag ? 'show' : 'hide']();
            if (result.data.customurlflag) {
                //显示自定义链接地址
                var wapurl = $('#pop-tjgn .p-nav li a[class=active]').attr("wapurl");
                console.log($('#pop-tjgn .p-nav li a[class=active]'));
                $('#moduleurl').val(wapurl);
            }
            //onlyone
            $('#onlyone').val(result.data.onlyone);
        }

    },
    //1.3	模版切换检查
    // / ResourceConfig/SwitchTemplateCheck
    //1.4	删除模版配置
    /// ResourceConfig/ DeleteTemplateConfig
    SwitchTemplateCheck: function (ecid, template_id) {

        var param = {
            'ecid': ecid,
            'template_id': template_id
        };

        var result = Request.Sync('/CMSAdmin/TemplateManage/SwitchTemplateCheck.ashx', param);

        if (result.code == 6) {

            if (confirm('您之前配置过模板，如果更换模版将清空之前的模版配置。\r\n是否继续？')) {

                result = Request.Sync('/AppTemplate/DeleteTemplateConfig', param);

                //console.log('B:', result );

            }
            else {
                return false;
            }

        }

        return true;

    },

    //上传资源
    UploadResourceFile: function (id, ext, ecid, template_id, resourcetype, callback) {

        new AjaxUpload($(id), {

            action: '/CMSAdmin/TemplateManage/UploadResourceFile.ashx',

            name: '__' + id.substr(1),

            onSubmit: function (file, e) {
                if (!new RegExp('^' + ext + '$', 'i').test(e)) {
                    popAlert("错误：无效的文件扩展名！");
                    return false;
                }

                this.setData({
                    "ecid": ecid,
                    'template_id': template_id,
                    "resourcetype": resourcetype
                });

                this.disable();
            },

            onComplete: function (file, response) {
                this.enable();
                if (callback) {
                    callback($.parseJSON(response));
                }
            }

        });

    },
    //上传微信资源
    UploadWeixinFile: function (id, ext, ecid, callback) {

        new AjaxUpload($(id), {

            action: '/CMSAdmin/TemplateManage/UploadWeiXinImages.ashx',

            name: '__' + id.substr(1),

            onSubmit: function (file, e) {
                //                if (!new RegExp('^' + ext + '$', 'i').test(e)) {
                //                    popAlert("错误：无效的文件扩展名！");
                //                    return false;
                //                }

                this.setData({
                    "ecid": ecid
                });

                this.disable();
            },

            onComplete: function (file, response) {
                this.enable();
                if (callback) {
                    callback($.parseJSON(response));
                }
            }

        });

    },

    //3.3	上传整包资源
    /// ResourceAdmin /UploadResourcePackage
    UploadResourcePackage: function (id, ecid, template_id, callback) {

        new AjaxUpload($(id), {

            action: '/CMSAdmin/TemplateManage/UploadResourcePackage.ashx',

            name: '__' + id.substr(1),

            onSubmit: function (file, e) {
                if (!/^zip$/i.test(e)) {
                    popAlert("错误：无效的文件扩展名！");
                    return false;
                }

                this.setData({
                    "ecid": ecid,
                    'template_id': template_id
                });

                this.disable();
            },

            onComplete: function (file, response) {
                this.enable();
                console.log('response:', response);
                if (callback) {
                    callback($.parseJSON(response));
                }
            }

        });

    },

    /// ClientSetting /GetReleaseSettings

    GetReleaseSettings: function (ecid) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetReleaseSettings.ashx', { 'ecid': ecid });



        //console.log( result );


    },

    ProcessGenerateResource: function (ecid, template_id) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/ProcessGenerateResource.ashx', {
            'ecid': ecid,
            'template_id': template_id
        });

        return result.code;

    },

    //获取轮播图
    //ResourceConfig/GetCarouselImages

    GetCarouselImages: function (ecid, shelltype) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetCarouselImages.ashx', {
            'ecid': ecid,
            'shelltype': shelltype
        });

        return result.code == 1 ? result.data : [];

    },

    //http://localhost:22421/WeiXin/GetWeiXinMenu


    GetWeiXinMenu: function (ecid, template_id) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetWeiXinMenu.ashx', {
            'ecid': ecid,
            'template_id': template_id
        });


        if (result.code == 1) {

            $('#scgz').val(result.data.firstreply);

            var html = [];

            $.each(result.data.menulist, function () {

                if (this.menulevel == 1) {

                    html.push(Request.format('<tr bizmoduleid="{bizmoduleid}" menulevel="{menulevel}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" type="{type}"><td style="width:200px;"><input class="wx-menuMain" type="text" value="{menuname}"><a class="btn-add"></a></td><td><select type="fselect"><option selected>链接</option><option>业务模块</option></select><input class="wx-menuLink" type="text" value="{linkurl}" style="margin-left:10px;"><a class="btn-add2" style="display:none;"></a><span style="display:none; margin-left:50px;">{bizmodulename}</span></td><td style="width:60px;"><a class="btn-del"></a></td></tr>', this));
                }
                else {

                    html.push(Request.format('<tr bizmoduleid="{bizmoduleid}" menulevel="{menulevel}" moduleid="{moduleid}" moduletypeid="{moduletypeid}" type="{type}"><td><span class="q4"></span><input class="wx-menuItem" type="text" value="{menuname}"></td><td><select type="fselect"><option selected>链接</option><option>业务模块</option></select><input class="wx-menuLink" type="text" value="{linkurl}" style="margin-left:10px;"><a class="btn-add2" style="display:none;"></a><span style="display:none; margin-left:50px;">{bizmodulename}</span></td><td style="width:60px;"><a class="btn-del"></a></td></tr>', this));

                }

            });

            var trs = $('#wx-table > tbody').html(html).children('tr');

            for (var i = 0, len = trs.length; i < len; i++) {

                var tr = $(trs[i]);



                //根据type来选择选中
                if (tr.attr('type') == 1) {
                    //选组合框第二个值
                    tr.find('select > option').removeAttr('selected').last().attr('selected', 'selected');
                    if (tr.attr('menulevel') != 1) {//子才显示
                        tr.find('td:nth-child(2) > input').hide().siblings().show();
                    }
                }

                //如果当前level=1，并且下一级为level=2 或 无 ，那么说明有子菜单
                if (tr.attr('menulevel') == 1 && i + 1 != len) {

                    if ($(trs[i + 1]).attr('menulevel') != 1) {
                        //隐藏第二个td
                        tr.find('td:nth-child(2) > *').hide();
                    }

                }
            }


            $('#ylFrame').load(function () {

                var doc = $(this.contentDocument);
                //首次关注
                doc.find('#firstreply').html(result.data.firstreply);

                //设置菜单
                var count = 0;
                for (var i = 0, len = result.data.menulist.length; i < len; i++) {
                    var x = result.data.menulist[i];
                    //主菜单	
                    if (x.menulevel == 1) {
                        count++;
                        doc.find('#menu li:eq(' + count + ') span').html(x.menuname);
                    }
                    else {
                        doc.find('#submenu' + count).append('<li>' + x.menuname + '</li>');
                    }
                }

            }).attr('src', 'weixin.html');




        }


    },


    SaveWeiXinMenu: function (ecid, template_id) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/SaveWeiXinMenu.ashx', {
            'template_id': template_id,
            'ecid': ecid,
            'firstreply': $('#scgz').val(),
            'menulist': (function () {
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
                        'type': type
                    });
                });
                return menulist;
            })()
        });

        return result.code == 1;

    },

    //3.1	获取获取微站图片
    // / ResourceAdmin/GetMPResourceImages
    GetMPResourceImages: function (ecid, template_id, resourcetype, page_index, page_size) {
        return Request.Sync('/CMSAdmin/TemplateManage/GetMPResourceImages.ashx', {
            'ecid': ecid,
            'template_id': template_id,
            'resourcetype': resourcetype,
            'page_index': page_index,
            'page_size': page_size
        });
    },


    //上传资源
    UploadMPResourceFile: function (id, ext, ecid, template_id, resourcetype, callback) {

        new AjaxUpload($(id), {

            action: '/CMSAdmin/TemplateManage/UploadMPResourceFile.ashx',

            name: '__' + id.substr(1),

            onSubmit: function (file, e) {
                if (!new RegExp('^' + ext + '$', 'i').test(e)) {
                    popAlert("错误：无效的文件扩展名！");
                    return false;
                }

                this.setData({
                    "ecid": ecid,
                    'template_id': template_id,
                    "resourcetype": resourcetype
                });

                this.disable();
            },

            onComplete: function (file, response) {
                this.enable();
                if (callback) {
                    callback($.parseJSON(response));
                }
            }

        });

    },
    /*
    //获取手机门户用户模板功能 
    //shark 2014-02-06
    */
    GetMPResource: function (ecid, template_id) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/GetMPResource.ashx', {
            'ecid': ecid,
            'template_id': template_id
        });


        if (result.code == 1) {
            var html = [];
            $.each(result.data.fun, function () {
                html.push(Request.format('<tr><td><img style="display:none;" height="50" backgroundcolor="{backgroundcolor}" bizmoduleid="{bizmoduleid}" src="/ResourceAdmin/GetResourceFile?FileID=0" imageid="0" moduleid="{moduleid}" moduletypeid="{moduletypeid}" moduleurl="{moduleurl}" ordernum="{ordernum}" resourcetype="mpa_icon" shelltype="mpa" textcolor="{textcolor}" themesimagesremark="{themesimagesremark}" partflag="{partflag}" function_id="{function_id}" text="{text}"></td><td><input type="text" value="{text}" onblur="$(this).parent().siblings().find(\'img\').attr(\'text\',this.value);"></td><td><a class="btn_del mt-6px"></a></td></tr>', this));

            });

            $('#rTabCont > li.active table').html(html);

            $('#ylFrame').load(function () {

                var doc = $(this.contentDocument);
                $('#func_table tbody').find('tr img').each(function (index, element) {
                    $(this).attr('ordernum', index + 1);
                    doc.find("#fun" + (index + 1)).attr("href", $(this).attr("moduleurl"));
                    doc.find("#fun" + (index + 1) + " div").html($(this).attr("text").replace(/,/, "<br/>").replace(/，/, "<br/>").replace(/-/, "<br/>"));
                });
                doc.find('body').css('background', 'url(' + result.data.bg.image_url + ') no-repeat scroll 0 0').css("background-size", "310px 540px");

                var img = $('<img>').attr({
                    src: result.data.bg.image_url,
                    imageid: result.data.bg.imageid,
                    bizmoduleid: "0",
                    moduleid: "0",
                    moduletypeid: "0",
                    moduleurl: "",
                    ordernum: "0",
                    partflag: "0",
                    resourcetype: "bg",
                    shelltype: "mpa",
                    textcolor: "",
                    'class': 'active',
                    width: 33,
                    height: 33,
                    partflag: 0
                }).show();
                $('.cd-imgs > img:first').remove();
                $('.cd-imgs').prepend(img);
                //选中触发左侧生效
                $('.cd-imgs > img.active').click();

            }).attr('src', 'mp_shellb.html');

        }


    },

    /*
    //保存手机门户模板功能 
    //shark 2014-02-06
    //资源配置
    //选中的所有选择器,组合在一起提交
    //导航区背景 .bj-imgs > img.active
    //导航区文字 .wz-cont > li.active img
    //功能区表格数据 #func_table img
    //菜单区背景 .cd-imgs > img.active
    //菜单区表格数据 #menu_table img
    */
    SaveMPFunction: function (ecid, template_id) {

        var listgenerateimages = Request.GetAttrMaps($('.bj-imgs > img.active, #func_table img, .cd-imgs > img.active, #menu_table img'));

        //'.wz-cont li.active img'
        if (parseInt($('.wz-radios input[name=wz]:checked').val()) < 2) {
            listgenerateimages.push(Request.GetAttrMap($('#top_txt_div > img')));
        }



        var param = {
            'ecid': ecid,
            'template_id': template_id,
            'app_name': $('#input_app_name').val(),
            'listgenerateimages': listgenerateimages
        }


        //console.log(param);
        var result = Request.Sync('/ResourceConfig/SaveMPResourceConfig', param);

        //console.log(result);
        return result.code;

    },
    //ResourceAdmin/UploadTxtImages
    /*
    请求参数 ecid:企业编号，   template_id:模版id，   text:文字

    返回  imageid 
    image_url
    resourcetype 
    themesimagesremark 

    */

    UploadTxtImages: function (ecid, template_id, text) {

        var result = Request.Sync('/CMSAdmin/TemplateManage/UploadTxtImages.ashx', {

            'ecid': ecid,

            'template_id': template_id,

            'text': text

        });

        if (result.code == 1) {

            var attrs = {

                'src': result.data.image_url,

                'imageid': result.data.imageid,

                'themesimagesremark': result.data.themesimagesremark,

                'partflag': 2,

                'resourcetype': 'top_txt'

            };

            $('#top_txt_div').html($('<img>').attr(attrs));

            $('#ylFrame').contents().find('#top_txt').attr('src', attrs.src);


        }
        else {
            popAlert('生成失败');
        }

    }

    ,
    //删除已有功能
    DeleteModuleType: function (ecid, moduletype) {

        var param = {
            'ecid': ecid,
            'module_type': moduletype
        };

        var result = Request.Sync('/CMSAdmin/TemplateManage/DeleteModuleType.ashx', param);

        if (result.code == 1) {
            return true;
        } else {
            return false;
        }
    }

}
function deleteModuleType(self, moduletype, name) {
    if (confirm("您确定要删除" + name + "？")) {
        Request.DeleteModuleType(Request.GetQuery('ecid'), moduletype);
        $(self).parent().remove();
    }
}





















