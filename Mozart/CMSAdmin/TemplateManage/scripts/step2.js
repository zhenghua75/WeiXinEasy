//步骤2
var Step2 = {
    NoRefresh: function () {
        $(document).bind("keydown", function (e) {
            e = window.event || e;
            if (e.keyCode == 116) {
                e.keyCode = 0;
                return false; //屏蔽F5刷新键   
            }
            if ((e.altKey) &&
                ((e.keyCode == 37) ||   //屏蔽 Alt+ 方向键 ←   
                (e.keyCode == 39)))   //屏蔽 Alt+ 方向键 →   
            {
                e.keyCode = 0;
                return false;
            }
            if (e.keyCode == 8) {
                e.keyCode = 0;
                return false; //屏蔽退格删除键    
            }
            if (e.keyCode == 116) {
                e.keyCode = 0;
                return false; //屏蔽F5刷新键   
            }
            if ((e.ctrlKey) && (e.keyCode == 82)) {
                e.keyCode = 0;
                return false; //屏蔽alt+R   
            }
        });
    },
    //获取生成次数（不需要了）
    getResidueGenerateCount: function () {
        $('#ResidueGenerateCount').click(function () {
            $('#pop-search').val('');
            $('#pop-tjgn .p-nav li').show();
        });
    },
    //发布
    GoToStep3: function () {
        var weixinStatus = $('#generate_task_weixin').attr("status");
        var iphoneStatus = $('#generate_task_iphone').attr("status");
        var androidStatus = $('#generate_task_android').attr("status");
        var iphone_ec_Status = $('#generate_task_iphone_ec').attr("status");
        if (weixinStatus == "" && iphoneStatus == "" && androidStatus == "" && iphone_ec_Status == "") {

            popAlert("您当前未生成应用，请生成后，点击“下一步”。");
            //            if (iphone_ec_Status != "") {
            //                popAlert("苹果企业版不能对外发布，请生成其他平台后，再行发布。");
            //            } else {
            //                popAlert("您当前未生成应用，请生成后，点击“下一步”。");
            //            }
            return false;
        }
        if ((weixinStatus != "" && weixinStatus != "4") || (androidStatus != "" && androidStatus != "4") || (iphone_ec_Status != "" && iphone_ec_Status != "4") || (iphoneStatus != "" && iphoneStatus != "4")) {
            popAlert("您当前应用未生成完成，请生成完成后，点击“下一步”。");
            return false
        }
        return true;
    },
    //初始化进度条
    InitProgressbar: function () {
        $('#generate_mp_progress,#generate_weixin_progress,#generate_android_progress,#generate_iphone_progress,#generate_iphone_ec_progress').each(function (index, element) {
            var platform_type = $(this).attr("id").replace('generate_', '').replace('_progress', '');
            var bar = $('#generate_' + platform_type + '_progress').progressbar({ width: '450px', height: '30px', padding: '0px' });
            //设置进度
            bar.progress(0);
            progresss[platform_type] = 0;
            progressbars[platform_type] = bar;
            $(this).hide();
        });
    },
    StartProgressbarRuning: function (platform_type) {
        if (runningProgress[platform_type] == undefined) {
            runningProgress[platform_type] = true;
            var m = 4600;
            if (platform_type == 'mp') {
                m = 100;
                $('#generate_task_' + platform_type).attr("status", 1);
                $('#show_message_' + platform_type).html('<i class="cg_ing_icon"></i><span id="w1_1">盆友，生着呢，马上就好了</span>');
                $('#generate_' + platform_type + '_progress').show();
            } else if (platform_type == 'weixin') {
                m = 100;
                $('#generate_task_' + platform_type).attr("status", 1);
                $('#show_message_' + platform_type).html('<i class="cg_ing_icon"></i><span id="w1_1">盆友，生着呢，马上就好了</span>');
                $('#generate_' + platform_type + '_progress').show();
            } else if (platform_type == 'android') {
                m = 2000;
            }
            $('#timer').everyTime(m, platform_type, function () {
                progresss[platform_type] = progresss[platform_type] + 1;

                if (platform_type == 'weixin' || platform_type == 'mp') {
                    if (progresss[platform_type] > 100) {
                        $('#generate_task_' + platform_type).attr("status", 4);
                        $('#timer').stopTime(platform_type);
                        $('#generate_' + platform_type + "_progress").hide();
                        $('#show_message_' + platform_type).html('<i class="cg_success_icon"></i><span id="w1_1">恭喜，生成成功！</span>');
                        $('#hint_message_' + platform_type).html('F妹友情提醒：暂无法预览，请发布后查看');
                        return;
                    }
                }

                if (progresss[platform_type] > 99) {
                    return;
                }
                progressbars[platform_type].progress(progresss[platform_type]);
            });
        }
    },
    //绑定生成按钮
    StartGenerateBind: function () {
        $('#generate_mp_start,#generate_weixin_start,#generate_android_start,#generate_iphone_start,#generate_iphone_ec_start').click(function () {
            var platform_type = $(this).attr("id").replace('generate_', '').replace('_start', '');
            Step2_Request.StartGenerate(ecid, template_id, platform_type, tag);
            if (platform_type == 'weixin' || platform_type == 'mp') {
                Step2.StartProgressbarRuning(platform_type);
            }
            return false;
        });
    },
    //重新生成
    ReGenerateBind: function () {
        $('#generate_mp_regenerate,#generate_weixin_regenerate,#generate_android_regenerate,#generate_iphone_regenerate,#generate_iphone_ec_regenerate').click(function () {
            var platform_type = $(this).attr("id").replace('generate_', '').replace('_regenerate', '');
            var task_id = $("#generate_task_" + platform_type).val();


            Step2_Request.ReGenerate(ecid, task_id, platform_type, tag);
            return false;
        });
    },
    //定时获取状态
    GetGenerateTaskStatusTimer: function (run) {
        if (run) {
            $(this).everyTime(5000, function () {
                Step2_Request.BatchGetGenerateTask(ecid, template_id, tag);
            });
        } else {
            $(this).stopTime();
        }
    },
    InitGenerateStatus: function (task_id, status, wait_count, platform_type, generate_percent, regenerate_count, client_download_url, client_qr_url) {
        //设置ID
        var gtask = $('#generate_task_' + platform_type)
        gtask.val(task_id);
        if (gtask.attr("status") == 4 || gtask.attr("status") == 5) {
            return;
        }
        if (platform_type == "iphone") {
            //$('a[id^=generate_' + platform_type + "_]").hide();
            $('#generate_iphone_start').hide();
            $('#generate_iphone_regenerate').hide();
            $('#generate_iphone_priority').hide();
            $('#generate_iphone_feedback').hide();
            $('#generate_iphone_client_download_url').hide();
            $('#generate_iphone_qr').hide();
            $('#generate_iphone_progress').hide();
            $('#hint_message_iphone').hide();
        } else {
            $('a[id^=generate_' + platform_type + "_]").hide();
        }

        if (platform_type == 'weixin' || platform_type == 'mp') {
            if (runningProgress[platform_type] != undefined && progresss[platform_type] <= 100) {
                return;
            }
        }
        gtask.attr("status", status);


        //生成客户端状态， 0：等待 1：开始生成 2：资源准备就绪； 3：生成中；4：生成成功； 5：生成失败；
        if (status == 0) {
            //等待生成
            if (wait_count > 0) {
                $('#show_message_' + platform_type).html('<i class="cg_wait_icon"></i><span id="w1_1">盆友，稍安勿躁，还有' + wait_count + '个人等着呢...</span>');
                //$('#hint_message_' + platform_type).html('F妹友情提醒：优先可以更快哦');
                //$('#generate_' + platform_type + '_priority').show();
                $('#generate_' + platform_type + '_progress').hide();
            } else {
                //                $('#show_message_' + platform_type).html('<i class="cg_ing_icon"></i><span id="w1_1">盆友，马上就开始生成了，</span>');
                //                //$('#hint_message_' + platform_type).html('F妹今日心情：天气好啊');
                //  $('#generate_' + platform_type + '_progress').show();
                $('#show_message_' + platform_type).html('<i class="cg_wait_icon"></i><span id="w1_1">盆友，稍安勿躁，还有' + 1 + '个人等着呢...</span>');
                //$('#hint_message_' + platform_type).html('F妹友情提醒：优先可以更快哦');
                //$('#generate_' + platform_type + '_priority').show();
                $('#generate_' + platform_type + '_progress').hide();

            }
        } else if (status == 1 || status == 2 || status == 3) {
            //生成中，设置生成按钮隐藏，显示进度
            $('#show_message_' + platform_type).html('<i class="cg_ing_icon"></i><span id="w1_1">盆友，生着呢，马上就好了</span>');

            $('#generate_' + platform_type + '_progress').show();
            if (generate_percent > progresss[platform_type]) {
                //设置进度
                progresss[platform_type] = generate_percent;
            }
            //开始走进度
            Step2.StartProgressbarRuning(platform_type);
        } else if (status == 4) {

            //生成成功
            if (platform_type == 'weixin' || platform_type == 'mp') {
                if (runningProgress[platform_type] == undefined) {
                    $('#generate_' + platform_type + "_progress").hide();
                    $('#show_message_' + platform_type).html('<i class="cg_success_icon"></i><span id="w1_1">恭喜，生成成功！</span>');
                    $('#hint_message_' + platform_type).html('F妹友情提醒：暂无法预览，请发布后查看');
                }
            } else {
                $('#timer').stopTime(platform_type);
                $('#generate_' + platform_type + "_progress").hide();

                $('#show_message_' + platform_type).html('<i class="cg_success_icon"></i><span id="w1_1">恭喜，生成成功！</span>');
                $('#hint_message_' + platform_type).html('F妹友情提醒：扫描二维码或下载安装测试。该二维码图片仅作测试，请勿做他用');
                $('#generate_' + platform_type + '_client_download_url').attr("href", client_download_url);
                $('#generate_' + platform_type + '_client_download_url').show();
                //client_qr_url = encodeURIComponent(client_qr_url);
                $('#generate_' + platform_type + '_qr').html('<img width="140px" height="140px" src="' + client_qr_url + '"/>');
                $('#generate_' + platform_type + '_qr').show();

                //苹果正式版生成后提示不同以及隐藏二维码图片 update by hecq
                if (platform_type == 'iphone') {
                    $('#hint_message_' + platform_type).html('F妹友情提醒：请下载后，上传至苹果商店后再预览');
                    $('#generate_' + platform_type + '_qr').hide();
                }

            }

        }
        else if (status == 5) {
            //隐藏进度条
            $('#generate_' + platform_type + '_progress').hide();

            //生成失败
            if (regenerate_count > 3) {
                $('#show_message_' + platform_type).html('<i class="cg_error_icon"></i><span id="w1_1">盆友，已经尽力了，真的生不了了</span>');
                $('#hint_message_' + platform_type).html('F妹友情提醒：马上报告F妹。');
            } else {
                $('#show_message_' + platform_type).html('<i class="cg_fail_icon"></i><span id="w1_1">盆友，邪门了，竟然失败了。</span>');
                $('#hint_message_' + platform_type).html('F妹友情提醒：失败了，就重新来过吧。</span>');
            }
            $('#generate_' + platform_type + '_regenerate').show();
        }
    }
}

