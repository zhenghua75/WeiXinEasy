<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="step1.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.step1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/bootstrap.min.css?t=635544536969112323"/>
    <link rel="stylesheet" href="css/yypz.css?t=635544536969112323"/>
    <link href="css/site.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/ajaxupload2.3.6.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/global.js?t=635544536969112323"></script>
    <script src="scripts/request.js?t=635544536969112323"></script>
    <script src="scripts/Step1.js?t=635544536969112323"></script>
    <%--<link rel="stylesheet" type="text/css" href="guider.css" />
    <script type="text/javascript" src="guider.ashx"></script>--%>
    <script>
   $(function(){
	
	//数据请求
	//[-------------------
	 var ecid = 608989;
	  //---------------------]
    Step1.gotostep2_onclick(ecid);

	//获取行业
	Request.GetBizType(ecid);
	
	//根据行业取对应的模板
    
    var bizid=$(".ulBizType>li").first().attr("value");
	Request.GetAppTemplate(ecid,bizid,function(){
		Request.GetBaseConfig(ecid);
		//Step1.getImgs(ecid);
		
		//设置模板名称
		$('.lTitle').html( $('#AppTemplate > li.active').attr('template_name') );

	});
	
	
	//模板图片被选择
	Step1.mbPic_onclick(ecid, function(){
		//获取基础配置
		//Request.GetBaseConfig(ecid);
		//Step1.getImgs(ecid);	
		//设置模板名称
		$('.lTitle').html( $('#AppTemplate > li.active').attr('template_name') );

	});
	
	//基础配置 选图片
	Step1.imgs_onclick();
	
	//切换行业 ，换模板
	$('#BizType').change(function(){
		Request.GetAppTemplate(ecid,$(this).val());
	});
	
	//获取基础配置
	/*
	$('#jcpz').click(function(){
		Request.GetBaseConfig(ecid);
		Step1.getImgs(ecid);
	});*/
	
	//生成测试.保存配置项.并到下一个操作
	$('#btn_sccs').click(function(){
		Request.SaveBaseConfig(ecid);	
	});
	
	//---------------------]
	
	
	
	//------------------
	
	//让A链接没有虚线边框
	Global.noDashed();
	
	//左侧4个纵向按钮点击的时候,切换选中效果.
	Global.tabs('#lTab','');
	
	//左侧4个纵向按钮点击的时候,切换iframe内容
	Global.iframeLocation('#mbFrame','#lTab > li > a');
	
	//右侧tab栏切换效果
	Global.tabs('#rTab','#rTabCont');
	
	//界面模板 和 基础配置 点击操作
	//Step1.jmmb_onclick();http://ci.f3.cn/d/600281
	//Step1.jcpz_onclick();
	
	
	//新手引导 和 封面 checkbox
	//Step1.checkbox_onclick();
	

	//上传处理
	//Request.AjaxUpload(id, ext, ecid, template_id, resourcetype);
	
	
	

	$(document).on('click','#AppTemplate a',function(e){
		
		window.open(this.href,'newwindow','height=760,width=1060,top=0,left=0,toolbar=no,menubar=no,scrollbars=auto,location=no, status=no');
		
		return false;	
	});
	
	
	//左侧点击切换右侧模板数据
	
	$('#lTab > li > a').click(function(){
		
		//获取平台类别
		var type = $(this).parent().attr('type');
		
		//更换模板图片
		$('#AppTemplate img').each(function(index, element) {
            $(this).attr('src', $(this).attr(type + '_image_url') );
        });
		
		//更换修改页链接
		$('#AppTemplate a').each(function(index, element) {
			var href = this.href.replace(/modify_\w*?\.aspx/i, 'modify_' + type + '.aspx');
          	$(this).attr('href', href);
            /*
            $(this).click(function(){
                //判断是否微信tag
                if($("#lTab li.active").attr('type') == 'weixin'){
                    popConfirm("<input type='radio' name='radiobutton' value='dingyuehao' id='radio1' checked><label for='radio1'>我是订阅号</label>&nbsp;&nbsp;&nbsp;<input type='radio' name='radiobutton' value='fuwuhao' id='radio2'><label for='radio2'>我是服务号</label>",function(res){
                            if (res == true) {
                            if($('input[name="radiobutton"]:checked').val()=='fuwuhao'){
                                href=href.replace(/modify_\w*?\.aspx/i, 'modify_weixin_fuwu.aspx');
                                window.open(href,'newwindow','height=760,width=1060,top=0,left=0,toolbar=no,menubar=no,scrollbars=auto,location=no, status=no');
                            }else {
                                href=href.replace(/modify_\w*?\.aspx/i, 'modify_weixin_dingyue.aspx');
                                window.open(href,'newwindow','height=760,width=1060,top=0,left=0,toolbar=no,menubar=no,scrollbars=auto,location=no, status=no');
                            }
                        }
                    });
                    return false;
                }
            });
            */
        });

         //刘迎修改，不切换微信和微站模板
        $('#AppTemplate li').each(function(index, element) {
            if(type == "app"){
                $(this).show();
                if($(this).attr('app_select') == "true"){
                    $(this).attr('class', "active");
                }else{
                    $(this).removeClass('active');
                }
                return;
            }
            if(index == 0){
                $(this).attr('class', "active");
            }else{
                $(this).removeClass('active');
                $(this).hide();
            }
        });
	});
	
	
	$('.lbtm a').click(function(){
		$('#mbFrame').attr('src', $('#mbFrame').attr('src'));	
	});
	
	$(document).on('mouseover', '#AppTemplate img', function(){

		$(this).attr('title', $(this).parents('li').attr('description'));
		
	}).on('mouseout', '#AppTemplate img', function(){
		
		$(this).removeAttr('title');
		
	});


    $('.ulBizType >li').click(function(){
        if($(this).attr("template_count")=="0"){
            return;
        }
        $(this).siblings("li[class!=ulBizTypeUnline]").removeClass("ulBizTypeSelect").end().addClass("ulBizTypeSelect");
		Request.GetAppTemplate(ecid,$(this).attr("value"));
	});

    showPlatform();
});

//隐藏或显示平台选项卡
function showPlatform()
{
    $("#lTab").show();
    var type = Request.GetQuery('type');
    if(type!='')
    {
        $("#lTab").hide();


        $("#mbFrame").attr('src',$('#lTab > li[type='+type+']').find('a').attr('href'));

        //更换模板图片
		$('#AppTemplate img').each(function(index, element) {
            $(this).attr('src', $(this).attr(type + '_image_url') );
        });
		
		//更换修改页链接
		$('#AppTemplate a').each(function(index, element) {
			var href = this.href.replace(/modify_\w*?\.aspx/i, 'modify_' + type + '.aspx');
          	$(this).attr('href', href);
        });

         //刘迎修改，不切换微信和微站模板
        $('#AppTemplate li').each(function(index, element) {
            if(type == "app"){
                $(this).show();
                if($(this).attr('app_select') == "true"){
                    $(this).attr('class', "active");
                }else{
                    $(this).removeClass('active');
                }
                return;
            }
            if(index == 0){
                $(this).attr('class', "active");
            }else{
                $(this).removeClass('active');
                $(this).hide();
            }
        });

        if(type=="weixin")
        {
            $(".lTitle").hide();//隐藏模版名称
        }
    }
}
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
    <ol class="top clearfix">
        <li class="t1 active">
            <div>
                <i></i><span>应用配置</span>
            </div>
        </li>
        <li class="t2">
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
    <div class="content">
        <div class="left">
            <div class="lTitle">
                模板名称</div>
            <ul id="lTab" class="lTab">
                <li type="app" class="t1 active"><a href=""></a></li>
                <li type="weixin" class="t2"><a href=""></a></li>
                <li type="mp" class="t3"><a href=""></a></li>
                <!-- <li class="t4"><a href="sms.html"></a></li>-->
            </ul>
            <ul id="lTabCont" class="lTabCont">
                <li class="active">
                    <iframe id="mbFrame" frameborder="0" width="305" height="541" scrolling="no" src="">
                    </iframe>
                </li>
                <li>123 </li>
            </ul>
            <div style="clear: both;">
            </div>
            <div class="lbtm">
                <a href="#"></a>
            </div>
        </div>
        <div class="right">
            <ul id="rTabCont" class="rTabCont">
                <li class="active">
                    <div class="rHead clearfix">
                        <label class="pull-left">
                            选择行业</label>
                        <select id="BizType">
                        </select>
                        <ul class='ulBizType'>
                        </ul>
                        <a class="u-btnGreen btn_right" id="goToStep2_1" href="#">下一步</a>
                    </div>
                    <div class="rCont">
                        <ul id="AppTemplate" class="rContPic clearfix">
                        </ul>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</body>
</html>
