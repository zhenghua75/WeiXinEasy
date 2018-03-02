<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Step2.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.Step2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<title>生成测试</title>
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/yypz.css" />
    <link rel="stylesheet" href="css/sccs.css" />
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/jquery.progressbar.js"></script>
    <script src="scripts/jquery.timers.js"></script>
    <script src="scripts/ajaxupload2.3.6.js"></script>
    <script src="scripts/global.js?t=635545811951597150"></script>
    <script src="scripts/request.js?t=635545811951597150"></script>
    <script src="scripts/step2_request.js?t=635545811951597150"></script>
    <script src="scripts/step2.js?t=635545811951597150"></script>
    <script src="scripts/Helper.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="http://ec.f3.cn/PageGuider/guider.css" />
    <script type="text/javascript" src="http://ec.f3.cn/PageGuider/guider.ashx"></script>--%>
    <script>
        var ecid = 608989;
        var template_id =  62;
        var tag = 1418955634802;
        var progressbars = {};
        var progresss = {};
        var runningProgress = {};
        var errorCount = 0;
      
        $(function () {
            Step2.NoRefresh();
            //初始化页面数据
            Step2_Request.GetAppName(ecid, template_id);
            //初始化进度条
            Step2.InitProgressbar();
            //初始化生成数据
            Step2_Request.InitGenerateStatus(ecid, template_id, tag);
            //绑定生成按钮
            Step2.StartGenerateBind();
            //定时获取生成状态
            Step2.GetGenerateTaskStatusTimer(true);
            //重新生成
            Step2.ReGenerateBind();
             //判断证书版是否有证书
            Step2_Request.HaveCre(ecid);
            //发布应用
            $('#btn_Release').click(function () {
                //必须全部生成完成才可以发布
                if(Step2.GoToStep3()){
                    $(this).attr("href",'Step3.aspx?ecid='+ecid+'&template_id='+ template_id+'&tag='+tag+'&type='+Request.GetQuery('type')+ '&agent_ecid=' + Request.GetQuery('agent_ecid')); 
                    return true;
                }else{
                    return false;
                }

           });

            //上传证书版证书
	        $('#generate_uplaod_iphone').click(function(){
		        Step2_Request.UploadUploadCertificateFile('#generate_uplaod_iphone', ecid, function(rs){
			        if(rs.code == 1){
				        $('#generate_iphone_start').show();
                        $('#generate_uplaod_iphone').hide();
			        }
			        else{
                        popAlert(rs.notice);	
			        }	
		        });	
	        }).click(); 
            $('.t1>div').css("cursor", "pointer").click(function(){
                goto_step1();
	        });
            Helper('#help_iphone', '提示：苹果正式版用于上传至苹果商店，未通过审核无法安装；生成时需要准备证书，如需帮助请联系管理员。');
            Helper('#help_iphone_ec', '提示：苹果企业版仅作测试，无法使用信息推送功能，且无法正式发布。');

            showPlatform();
        });

        
            function goto_step1()
            {
                var type = Request.GetQuery('type');
                window.location.href = 'step1.aspx?ecid='+ecid+'&force=true&type='+type+ '&agent_ecid=' + Request.GetQuery('agent_ecid');
                return false;
            }

            //隐藏或显示平台
            function showPlatform()
            {
                var type = Request.GetQuery('type');
                if(type!='')
                {
                    if(type=="app"){
                        $(".state4").hide();
                    }else if(type=="weixin"){
                        $(".state1").hide();
                        $(".state2").hide();
                        $(".state3").hide();

                        $(".state4").css({"float":"left","margin-left":"200px","margin-top":"30px"});
                        $("#generate_weixin_start").css("margin-left","150px");
                        $(".state4 > h3").hide();

                        $("#app_name").hide();//隐藏appname
                    }
                }
            }
    </script>
</head>
<body>
    <ol class="top clearfix">
        <li class="t1">
            <div>
                <i></i><span>应用配置</span>
            </div>
        </li>
        <li class="t2 active">
            <div>
                <i></i><span>生成测试</span>
            </div>
        </li>
        <li class="t3">
            <div>
                <i></i><span>应用发布</span>
            </div>
        </li>
    </ol>
    <div class="t2-outer">
        <div class="t2-header">
            <a id="btn_Next" onclick="goto_step1();" href="#" class="u-Black pull-left">上一步</a>
            <h2 id="app_name">
                应用名称</h2>
            <p style="display: none">
                剩余生成&nbsp;&nbsp;&nbsp;<span id="ResidueGenerateCount">0次</span></p>
            <a id="btn_Release" href="" class="u-btnGreen pull-right">下一步</a>
        </div>
        <div class="t2-cont">
            <ul class="clearfix">
                <li class="odd state1">
                    <h3>
                        <span>安卓版</span><i style="display: none" id="help_android"></i></h3>
                    <div class="word">
                        <p class="w1" id="show_message_android">
                            <i class="cg_ready_icon"></i><span>Hey，盆友，准备好了么，开生吧！</span></p>
                        <p class="w2" id="hint_message_android">
                        </p>
                    </div>
                    <div class="other">
                        <a href="" id="generate_android_start" class="u-btnBlue">生成</a> <a href="" id="generate_android_regenerate"
                            class="u-btnBlue" style="display: none">重试</a> <a href="" id="generate_android_priority"
                                class="u-btnBlue" style="display: none">优先</a> <a href="" id="generate_android_feedback"
                                    class="u-btnBlue" style="display: none">马上报告</a> <a href="" target="_blank" id="generate_android_client_download_url"
                                        class="u-btnBlue" style="display: none">下载</a> <a class="code" id="generate_android_qr"
                                            style="display: none"></a>
                        <div id="generate_android_progress" style="display: none">
                        </div>
                        <input type="hidden" id="generate_task_android" status="" />
                    </div>
                </li>
                <li class="even state2">
                    <h3>
                        <span>苹果正式版</span><i id="help_iphone"></i></h3>
                    <div class="word">
                        <p class="w1" id="show_message_iphone">
                            <i class="cg_ready_icon"></i><span>Hey，盆友，准备好了么，开生吧！</span></p>
                        <p class="w2" id="hint_message_iphone">
                            F妹友情提醒：苹果正式版需先上传证书</p>
                    </div>
                    <div class="other">
                        <a href="" id="generate_iphone_start" class="u-btnBlue">生成</a> <a href="" id="generate_iphone_regenerate"
                            class="u-btnBlue" style="display: none">重试</a> <a href="" id="generate_iphone_priority"
                                class="u-btnBlue" style="display: none">优先</a> <a href="" id="generate_iphone_feedback"
                                    class="u-btnBlue" style="display: none">马上报告</a> <a href="" target="_blank" id="generate_iphone_client_download_url"
                                        class="u-btnBlue" style="display: none">下载</a> <a class="code" id="generate_iphone_qr"
                                            style="display: none"></a>
                        <div id="generate_iphone_progress" style="display: none">
                        </div>
                        <input type="hidden" id="generate_task_iphone" status="" />
                        <div class="upload-btn-cre" id="generate_uplaod_iphone" style="display: none">
                            苹果正式版证书上传</div>
                        
                    </div>
                </li>
                <li class="odd state3">
                    <h3>
                        <span>苹果企业版</span><i id="help_iphone_ec"></i></h3>
                    <div class="word">
                        <p class="w1" id="show_message_iphone_ec">
                            <i class="cg_ready_icon"></i><span>Hey，盆友，准备好了么，开生吧！</span></p>
                        <p class="w2" id="hint_message_iphone_ec">
                        </p>
                    </div>
                    <div class="other">
                        <a href="" id="generate_iphone_ec_start" class="u-btnBlue">生成</a> <a href="" id="generate_iphone_ec_regenerate"
                            class="u-btnBlue" style="display: none">重试</a> <a href="" id="generate_iphone_ec_priority"
                                class="u-btnBlue" style="display: none">优先</a> <a href="" id="generate_iphone_ec_feedback"
                                    class="u-btnBlue" style="display: none">马上报告</a> <a href="" target="_blank" id="generate_iphone_ec_client_download_url"
                                        class="u-btnBlue" style="display: none">下载</a> <a class="code" id="generate_iphone_ec_qr"
                                            style="display: none"></a>
                        <div id="generate_iphone_ec_progress" style="display: none">
                        </div>
                        <input type="hidden" id="generate_task_iphone_ec" status="" />
                    </div>
                </li>
                <li class="even state4">
                    <h3>
                        <span>微信</span><i style="display: none" id="help_weixin"></i></h3>
                    <div class="word">
                        <p class="w1" id="show_message_weixin">
                            <i class="cg_ready_icon"></i><span>Hey，盆友，准备好了么，开生吧！</span></p>
                        <p class="w2" id="hint_message_weixin">
                        </p>
                    </div>
                    <div class="other">
                        <a href="" id="generate_weixin_start" class="u-btnBlue">生成</a> <a href="" id="generate_weixin_regenerate"
                            class="u-btnBlue" style="display: none">重试</a> <a href="" id="generate_weixin_priority"
                                class="u-btnBlue" style="display: none">优先</a> <a href="" id="generate_weixin_feedback"
                                    class="u-btnBlue" style="display: none">马上报告</a> <a class="code" id="generate_weixin_qr"
                                        style="display: none">1</a>
                        <div id="generate_weixin_progress" style="display: none">
                        </div>
                        <input type="hidden" id="generate_task_weixin" status="" />
                    </div>
                </li>
                <li class="odd state5" style="display: none">
                    <h3>
                        <span>轻应用</span><i style="display: none" id="help_mp"></i></h3>
                    <div class="word">
                        <p class="w1" id="show_message_mp">
                            <i class="cg_ready_icon"></i><span>Hey，盆友，准备好了么，开生吧！</span></p>
                        <p class="w2" id="hint_message_mp">
                        </p>
                    </div>
                    <div class="other">
                        <a href="#" id="generate_mp_start" class="u-btnBlue">生成</a> <a href="#" id="generate_mp_regenerate"
                            class="u-btnBlue" style="display: none">重试</a> <a href="#" id="generate_mp_priority"
                                class="u-btnBlue" style="display: none">优先</a> <a href="#" id="generate_mp_feedback"
                                    class="u-btnBlue" style="display: none">马上报告</a> <a class="code" id="generate_mp_qr"
                                        style="display: none">1</a>
                        <div id="generate_mp_progress">
                        </div>
                        <input type="hidden" id="generate_task_mp" status="" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
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
    <div id="timer" style="display: none" >
    </div>
</body>
</html>
