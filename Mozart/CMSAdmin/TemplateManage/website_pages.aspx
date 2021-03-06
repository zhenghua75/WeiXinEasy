﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="website_pages.aspx.cs" Inherits="Mozart.CMSAdmin.TemplateManage.website_pages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>企业微站管理系统</title>
    <link rel="stylesheet" type="text/css" href="css/css.css" />
    <link rel="stylesheet" href="css/jquery-ui-1.10.3.custom-south-street.css" />
    <link rel="stylesheet" href="css/drop-down-menu.css" />
    <link rel="stylesheet" href="css/loop.css" />
    <link rel="stylesheet" href="css/album.css" type="text/css" />
    <script type="text/javascript" src="scripts/swfupload.js"></script>
    <script type="text/javascript" src="scripts/handlers.js"></script>

    <link rel="stylesheet" href="css/validate.css" />
    <script type="text/javascript" src="scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui-1.10.3.custom.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui-CN.js"></script>
    <script type="text/javascript" src="scripts/superfish.js"></script>
    <script type="text/javascript" src="scripts/tablesorter.js"></script>
    <script type="text/javascript" src="scripts/tablesorter-pager.js"></script>
    <script type="text/javascript" src="scripts/cookie.js"></script>
    <script type="text/javascript" src="scripts/jquery.form.js"></script>
    <script type="text/javascript" src="scripts/jquery.validate.js"></script>
    <script type="text/javascript" src="scripts/jquery.metadata.js"></script>
    <script type="text/javascript" src="scripts/jquery_validate_methods.js"></script>

    <script type="text/javascript" src="scripts/custom.js"></script>
    <script type="text/javascript" src="scripts/zDialog.js"></script>
    <script type="text/javascript" src="scripts/datePicker.js"></script>
    <script type="text/javascript" src="scripts/ajaxRequest.js"></script>
    <script type="text/javascript" src="scripts/autoComplete.js"></script>
    <script type="text/javascript" src="scripts/pager.js"></script>
    <script charset="utf-8" src="scripts/kindeditor.js"></script>
    <script type="text/javascript" src="scripts/jquery.form.wizard.js"></script>
    <script charset="utf-8" src="scripts/swipe.js"></script>
    <script type="text/javascript" src="scripts/jquery-ui-timepicker-addon.js"></script>
    <!-- 上传图片 -->
    <link href="css/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="scripts/swfobject.js"></script>
    <script type="text/javascript" src="scripts/jquery.uploadify.v2.1.0.min.js"></script>
    <style>
        #initset {
            width: 100%;
            height: 1329px;
            background: rgba(0,0,0,0.5);
            position: absolute;
            z-index: 9999;
            display: none;
        }

        .ooBoxbj {
            width: 928px;
            height: 100%;
            margin: 0 auto;
            position: relative;
        }

            .ooBoxbj span {
                cursor: pointer;
            }

        #ts_one {
            width: 496px;
            height: 221px;
            background: url(images/one.png) no-repeat;
            position: absolute;
            z-index: 99999;
            left: 0;
            top: 107px;
            position: relative;
        }

            #ts_one a {
                position: absolute;
                width: 136px;
                height: 22px;
                display: block;
                bottom: 19px;
                right: 14px;
            }

            #ts_one span {
                position: absolute;
                right: 20px;
                top: 17px;
            }



        #ts_two {
            width: 498px;
            height: 221px;
            background: url(images/two.png) no-repeat;
            position: absolute;
            z-index: 99999;
            left: 0;
            top: 219px;
            position: relative;
            display: none;
        }

            #ts_two a {
                position: absolute;
                width: 81px;
                height: 22px;
                display: block;
                bottom: 19px;
                right: 14px;
            }


            #ts_two span {
                position: absolute;
                right: 20px;
                top: 17px;
            }



        #ts_three {
            width: 333px;
            height: 497px;
            background: url(images/three.png) no-repeat;
            position: absolute;
            z-index: 99999;
            left: 0;
            top: 84px;
            position: relative;
            display: none;
        }

            #ts_three a {
                position: absolute;
                width: 120px;
                height: 22px;
                display: block;
                top: 175px;
                left: 27px;
            }

            #ts_three span {
                position: absolute;
                right: 74px;
                top: 17px;
            }

        #ts_fore {
            width: 495px;
            height: 356px;
            background: url(images/fore.png) no-repeat;
            position: absolute;
            z-index: 99999;
            left: 0;
            bottom: 393px;
            display: none;
        }

            #ts_fore a {
                position: absolute;
                width: 87px;
                height: 22px;
                display: block;
                bottom: 155px;
                right: 14px;
            }


            #ts_fore span {
                position: absolute;
                right: 20px;
                top: 17px;
            }


        #ts_five {
            width: 495px;
            height: 271px;
            background: url(images/five.png) no-repeat;
            position: absolute;
            z-index: 99999;
            left: 0;
            bottom: 121px;
            display: none;
        }

            #ts_five a {
                position: absolute;
                width: 133px;
                height: 22px;
                display: block;
                bottom: 70px;
                right: 14px;
            }


            #ts_five span {
                position: absolute;
                right: 20px;
                top: 17px;
            }
    </style>
    <script type="text/javascript">
		window.onload=function(){
			var parinitset = 'null';
			var initset=document.getElementById("initset");
			if(parinitset == "true"){
				initset.style.display="block";
				$('.mengbangs').css({background:"none"})
			
			}
			 
		}
		function resetSubmenuHeight(){
			var height=$("#page-right").height();
			height = height<=1130?1130:height;

			$(".submenu").height(height);
		}
		function loading_open(){
		    $(".mengbangs").css({'display':'block'});
		    $("#loading").css({'display':'block'});
		   // $("#right").css({'display':'none'})
		}
		
		function loading_close(){
		    $(".mengbangs").css({'display':'none'});
		    $("#loading").css({'display':'none'});
		    //$("#right").css({'display':'block'})
		}
		$(function(){
			$("#ts_one a").click(function(){
				$("#ts_one").css({display:"none"});
				$("#ts_two").css({display:"block"});	
			})
			
			$("#ts_two a").click(function(){
				$("#ts_two").css({display:"none"});
				$("#ts_three").css({display:"block"});	
			})
			
			$("#ts_three a").click(function(){
				window.location.hash = "#pamenu";
				$("#ts_three").css({display:"none"});
				$("#ts_fore").css({display:"block"});	
			})
			
			$("#ts_fore a").click(function(){
				window.location.hash = "#album";
				$("#ts_five").css({display:"block"});
				$("#ts_fore").css({display:"none"});
			})
			
			$("#ts_five a").click(function(){
				$("#ts_three").css({display:"none"});
				$("#ts_five").css({display:"none"});
				$('#initset').css({display:"none"});
				refreshPage();
			})
			
			$(".ooBoxbj span").click(function(){
				//window.location.hash = "#index";
				//$('#initset').css({display:"none"});
				refreshPage();
			});
		})	;
		
		function scrollTop(){
			$('html,body').animate({scrollTop: $("#top").offset().top}, 500);
		
		}
		
		/**
		 * 刷新页面防止进入新手引导
		 */
		function refreshPage(){
			window.location.href="/manage.jsp?pageName=manage";
		}
    </script>



</head>
<body>
    <div id="initset">
        <div class="ooBoxbj">
            <div id="ts_one">
                <a href="###"></a>
                <span>
                    <img src="images/gban_a.png" /></span>
            </div>
            <div id="ts_two">
                <a href="###"></a>
                <span>
                    <img src="images/gban_a.png" /></span>
            </div>
            <div id="ts_three">
                <a href="###"></a>
                <span>
                    <img src="images/gban_a.png" /></span>
            </div>
            <div id="ts_fore">
                <a href="###"></a>
                <span>
                    <img src="images/gban_a.png" /></span>
            </div>
            <div id="ts_five">
                <a href="###"></a>
                <span>
                    <img src="images/gban_a.png" /></span>
            </div>
        </div>
    </div>


    <div class="container" id="container" style="width: 100%;">

        <div id="page-right" class="page-right" style="width: 100%;">
            <!-- <div class="mengbangs"></div> -->
            <div id="loading" class="loading" style="display: none">
                <img src="images/loading42.gif" /><br />
            </div>

            <div id="right" style="overflow: hidden;">


                <link rel="stylesheet" href="css/tem.css" />
                <link rel="stylesheet" href="css/tem12.css" />
                <link rel="stylesheet" href="css/evol.colorpicker.css" />
                <script type="text/javascript" src="scripts/evol.colorpicker.js"></script>
                <script type="text/javascript" src="scripts/pager1.js"></script>


                <script type="text/javascript">

$(function() {

    $("#dialog").dialog( {
        title: "添加模块" ,
        autoOpen : false,
        width : 400,
        modal : true,
        buttons: {}
    });

    $("#shapedialog").dialog( {
        title: "选择形状" ,
        autoOpen : false,
        width : 400,
        modal : true
    });

    $("#changetmp").dialog( {
        title: "选择模板" ,
        autoOpen : false,
        width : 720,
        modal : true
    });
	
	
    $("#webCode").dialog( {
        title: "微网站二维码" ,
        autoOpen : false,
        width : 280,
        modal : true
    });

    // Link to open the dialog
    $("#dialog-link").click(function(event) {
        $("#dialog").dialog("open");
        event.preventDefault();
    });

    showEdit();

    footerMenubar();
	
	
    $( "#content1" ).sortable({
        stop: function( event, ui ) {
		
            //先存入数据库，后更新页面
            $(".mini").hide();
            fillform();
            var options = {
                type : 'POST',
                success : function(responseText, textStatus) {
                },
                error : function(XmlHttpRequest, textStatus, errorThrown) {
                    alert("error");
                }
            };
            $('#contentForm').ajaxSubmit(options);//ajax提交
        }
    }).disableSelection();
	
    var isSubPage ;
    if(isSubPage!=1){
    }else{
    }

});

                           function showEdit() {

                               var rgb = "";

                               $('#bo #content1').on('mouseover', 'dd', function(event) {
                                   var id = event.currentTarget.id;
                                   $("#" + id + " .mini").show();
                                   //	rgb = $("#" + id).css('background-color');
                                   //	$("#" + id).css("background-color", "#7FB221;");
                               });
                               $("#bo #content1").on("mouseout", 'dd', function(event) {
                                   var id = event.currentTarget.id;
                                   $("#" + id + " .mini").hide();
                                   //	$("#" + id).css("background-color", rgb);
                               })
                               /* $("#bo #content1").on( "click", '.ccolor', function(event) {
                                               var id = $(event.currentTarget).attr("date_id");
                                               $("#color" + id).colorpicker( {
                                                   showOn : 'none'
                                               });
                                               event.stopImmediatePropagation();
                                               $("#color" + id).colorpicker("showPalette").on(
                                                       "change.color",
                                                       function(event, color) {
                                                           $("#" + id).attr('style',
                                                                   'background-color:' + color);
                                                           $("#color" + id).colorpicker("hidePalette");
                                                           $(".mini").hide();
                                                           //修改颜色之后更新网站
                                                           $("#contentd").val($("#content1").html());
                                                           AjaxRequest.formRequest("contentForm", null);
                                                       })
                               
                                           })
                               
                                   $('#title1').on('click', 'div', function(event) {
                                       var id = event.currentTarget.id;
                                       edit(id, "title");
                                   });
                               */
                               $('#looppic1').on('click', 'div', function(event) {
                                   var id = event.currentTarget.id;
                                   edit(id, "looppic");
                               });

                               $('#menubar1').on('click', '.footer-nav', function(event) {

                                   edit("1", "menubar");
                               });
                           }

                           function copy(it) {

                               $("#deleteId").val("no");
                               var now = new Date();
                               var random = now.getTime();
                               var url = "/websitePages/website_pages!findDivId.action?ajax=true&className="+ it.id+ "&random=" + random;

                               $.post(url, function(s) {

                                   if (s == null) {
                                       alert("error");
                                   } else {
                                       //先存入数据库，后更新页面
                                       fillform();
                                       var con = $("#contentd").val();
                                       $("#contentd").val(con + s);
                                       var options = {
                                           type : 'POST',
                                           success : function(responseText, textStatus) {
                                               $("#content1").append(s);
                                           },
                                           error : function(XmlHttpRequest, textStatus, errorThrown) {
                                               alert("error");
                                           }
                                       };
                                       $('#contentForm').ajaxSubmit(options);//ajax提交
			
                                   }
		
                                   $("#dialog").dialog("close");
                               });
                           }

                           function fillform() {
	
                               $("#titled").val($("#title1").html());
                               $("#looppicd").val($("#looppic1").html());
                               $("#contentd").val($("#content1").html());
                           }

                           function edit(id, type) {
                               if (type == 'menubar') {
                                   AjaxRequest.urlRequest("/menubar/merchant_menubar!editMeunbar.action?ajax=true","bo1");
                               } else {
                                   var isSubPage = $("#isSubPage").val();
                                   var templateName = $("#templateName").val();
                                   var moduleName = $("#"+id).attr("class");
                                   AjaxRequest .urlRequest( "/websitePages/website_pages!beforeEdit.action?ajax=true&model.id="+ id+ "&model.isSubPage="+ isSubPage
                                                           + "&model.eidtType=" + type+"&websiteModule.templateName="+templateName+"&websiteModule.moduleName="+moduleName, "bo1");
                               }
                           }

                           function del(id) {
                               if(!confirm("确定删除？")){
                                   return;
                               }
	
                               $("#deleteId").val("yes");
                               $("#pageId").val(id);
                               $("#" + id).remove();
                               fillform();
                               var options = {
                                   type : 'POST',
                                   success : function(responseText, textStatus) {
                                       $("#bo1").empty();
                                   },
                                   error : function(XmlHttpRequest, textStatus, errorThrown) {
                                       alert("error");
                                   }
                               };
                               $('#contentForm').ajaxSubmit(options);//ajax提交
                           }

                           function changeshape(id) {
                               $("#chang_div_id").val(id);
                               $("#shapedialog").dialog("open");
                           }

                           function ch_shape(value) {
                               var id = $("#chang_div_id").val();
                               $("#" + id).attr("class", value);
                               $("#shapedialog").dialog("close");
                               //修改后更新数据库
                               $("#contentd").val($("#content1").html());
                               AjaxRequest.formRequest("contentForm", null);
                           }

                           function footerMenubar() {
                               AjaxRequest.urlRequest(
                                               "/CMSAdmin/TemplateManage/getMeunbar.ashx?ajax=true",
                                               "menubar1");
                           }

                           function changetmp(value) {
                               var isSubPage = $("#isSubPage").val();
                               if(isSubPage==1){
                                   isSubPage=3;
                               }
                               var url = "/CMSAdmin/TemplateManage/changetmp.ashx?ajax=true&query4Page.curPage=1&moban=" + isSubPage;
                               AjaxRequest.urlRequest(url, "changetmp");
                               $("#changetmp").dialog("open");

                               $("#settable").find("td").each(function() {
                                   $(this).attr("class", "");
                               });
                           }

                           function ctmplate(id) {
                               $("#changetmp").dialog("close");
                               var isSubPage = $("#isSubPage").val();
                               //二级页面
                               if (1 == isSubPage) {
                                   var modelid = $("#weid").val();
                                   AjaxRequest.urlRequest("/websitePages/website_pages!changSupTemp.action?ajax=true&temId="+ id + "&model.id=" + modelid+"&moban=1", "right",
                                       function(f) {
                                           if (f) {
                                               $("#isSubPage").val('1');
                                           }
                                       });
                               }
                               //一级页面
                               if (0 == isSubPage) {
                                   AjaxRequest.urlRequest("/CMSAdmin/TemplateManage/wapsiteEdit.ashx?ajax=true&temId=" + id, "right", function (ff) {
                                       if (ff) {
                                           $("#isSubPage").val('0');
                                       }
                                   });
                               };
                           }

                           function changeback(value, content) {
                               //0:一级页面；1：二级页面
                               var isSubPage = $("#isSubPage").val();
                               var weid = $("#weid").val();
                               $("#settable").find("td").each(function() {
                                   $(this).attr("class", "");
                               });
	 
                               AjaxRequest.urlRequest(
                                       "/CMSAdmin/TemplateManage/jump.ashx?ajax=true&content=" + content + "&model.isSubPage=" + isSubPage + "&model.websiteId=" + weid, "bo1", function (f) {
                                           $(value).attr("class", "hover");
                                       });
	
                           }


                           function backtopage(){
                               AjaxRequest.urlRequest("/CMSAdmin/TemplateManage/wapsiteEdit.ashx?ajax=true", "right");
                           }

                           function showCode(){
                               $.post('/CMSAdmin/TemplateManage/getCode.ashx?ajax=true', function (s) {
                                   if (s == "error") {
                                       alert("生成二维码错误，请联系管理员！");
                                   } else {
                                       $("#webCode").html('<img src="'+s+'" />');
                                       $("#webCode").dialog("open");
                                   }
                               });
                           }

                           function aleft(){
                               var left = $("#content1").css("left");
                               var a = left.substring(0, left.length-2);
                               var ll = parseInt(167)+parseInt(a);
                               if(parseInt(ll)>11){
                                   return;
                               }
                               $("#content1").css("left",ll+"px");
                           }

                           function aright(){
                               var left = $("#content1").css("left");
                               var a = left.substring(0, left.length-2);
                               var Oli=$('#content1 dd').length;
                               var Oli1=$($('#content1 dd'));
                               var ll = a-167;
                               $("#content1").css('width',190*Oli+'px')
                               $("#content1").css("left",ll+"px");
                           }

                           $(document).ready(function() {
                               var path = 'css/tem12.css';
                               if('css/tem7.css'==path){
                                   $("dd").removeAttr("style"); 
                               };
                               $("#fa_btn").click(function(){
                                   AjaxRequest.urlRequest("/websitePages/website_pages!fabuWeizhan.action?ajax=true", "", function(ff) {
                                       if (ff) {
                                           ZMessage.Tip("发布成功","发布成功" ,ff);
                                           setTimeout(function(){
                                               window.parent.location.href="http://ec.f3.cn/BackOffice/aspx/sysFrame/MyMain.aspx?product=67";
                                           },2000);
                                           //window.parent.location.href="http://192.168.21.10/BackOffice/aspx/sysFrame/MyMain.aspx?product=67";
                                           //window.parent.location.href="http://ec.f3.cn/BackOffice/aspx/sysFrame/MyMain.aspx?product=67";
                                       };
                                   });
			
			
                                   //window.location.href="http://192.168.21.10/BackOffice/aspx/sysFrame/MyMain.aspx?product=67";
                               });
                           });

                           /**
                           *过滤背景图片
                           */
                           function filterBGColor_pic(){
                               $("dd").each(function(index){
                                   var img=$(this).find(".picture img").get(0);
                                   if(img!=null){
                                       $(this).css("background","none");
                                   };
                               });
                           }
                           </script>


                <div class="form1" style="padding: 40px;">
                    <span style="padding-left: 176px; color: #666666; font-size: 14px;">微网站二维码：
                        <a href="javascript:void(0)" style="color: #92d050; text-decoration: underline" onclick="showCode()">点击查看</a>
                    </span>

                    <div class="form-img">
                        <div id="bo" class="template">
                            <div id="title1" style="margin-top: 5px;">
                                <div id="58812" class="template-b-00" onclick="edit('58812', 'title');" style="background-color: #0c0c0c"><span class="titlelogo">
                                    <img src="http://www.veshow.cn:80//uploadFiles/site/title/101/20131226162413.jpg" height="30px" width="30px"/></span>
                                    <span class="titlename" style="color: rgb(255, 255, 255);">道讯科技</span>
                                    <span class="titlecode"> </span>
                                </div>
                            </div>
                            <div id="looppic1"
                                style="margin: 5px 5px 5px 0; float: left; width: 100%; max-width: 290px">
                                <script type="text/javascript">
                                    var fullctx = "";
                                    $(document).ready(function () { $("#image").Swipe(); });
                                </script>
                                <div onclick="edit('id','looppic');">
                                    <!-- 图片轮播 -->
                                    <div style="visibility: visible;" id="image" class="swipe">
                                        <div class="swipe-wrap" style="width: 678px;">
                                            <div data-index="0" style="width: 226px; height: 113px; left: 0px; -webkit-transition: 1000ms; transition: 1000ms; -webkit-transform: translate(0px, 0px) translateZ(0px);"><a href="javascript:void(0)" onclick="showPic('图片','040BD1CA4AE5AB08E050973BAE0C1EA0')" style="width: 226px; height: 113px;">
                                                <img src="http://www.veshow.cn:80//uploadFiles/site/loop/101/20131226163102.jpg" align="absmiddle"/>
                                            </a><span class="bottom">图片</span>					<span class="dots"><b class="select"></b><b></b><b></b></span></div>
                                            <div data-index="1" style="width: 226px; height: 113px; left: -226px; -webkit-transition: 0ms; transition: 0ms; -webkit-transform: translate(226px, 0px) translateZ(0px);"><a href="javascript:void(0)" onclick="showPic('鍥剧墖','040BD1CA4AE6AB08E050973BAE0C1EA0')" style="width: 226px; height: 113px;">
                                                <img src="http://www.veshow.cn:80//uploadFiles/site/loop/101/20131226163102.jpg" align="absmiddle"/>
                                            </a><span class="bottom">图片</span>					<span class="dots"><b></b><b class="select"></b><b></b></span></div>
                                            <div data-index="2" style="width: 226px; height: 113px; left: -452px; -webkit-transition: 1000ms; transition: 1000ms; -webkit-transform: translate(-226px, 0px) translateZ(0px);"><a href="javascript:void(0)" onclick="showPic('','03E35C2494403409E050973BAE0C1EA4')" style="width: 226px; height: 113px;">
                                                <img src="http://59.151.12.174:8010/uploadFiles/site/loop/608989/20141008194555.jpg" align="absmiddle"/>
                                            </a><span class="bottom"></span><span class="dots"><b></b><b></b><b class="select"></b></span></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="content1Boxaa">
                                <div class="aLeft" onclick="aleft();"><a href="###"></a></div>
                                <div class="aRight" onclick="aright();"><a href="###"></a></div>
                                <div id="content1">

                                    <dd id="58814" class="template-b-01" style="background-color: ">
                                        <p class="text_1" style="color: rgb(12, 12, 12);">关于我们</p>
                                        <div id="mini58814" class="mini" style="display: none;">
                                            <a href="#" onclick="edit('58814','content')">
                                                <img alt="编辑" src="images/bj.png" style="padding-top: 4px;" height="12px" width="12px" />
                                            </a><a href="#" onclick="changeshape('58814')">
                                                <img alt="改变形状" src="images/xz.png" style="padding-top: 4px;" height="12px" width="12px" />
                                            </a>
                                            <a href="#" onclick="del('58814')">
                                                <img alt="删除" src="images/sc.png" style="padding-top: 4px;" height="12px" width="12px" />
                                            </a>
                                        </div>
                                        <div class="icon">
                                            <img src="images/13.png"/></div>
                                        <div class="picture">
                                            <img src=""/></div>
                                        <div class="bmini" style="color: rgb(12, 12, 12);">关于我们</div>
                                        <div class="news_xx" style="color: rgb(12, 12, 12);"></div>
                                    </dd>
                                    <dd id="58815" class="template-b-02" style="background-color: #ebf1dd">
                                        <p class="text_1" style="color: rgb(12, 12, 12);">联系我们</p>
                                        <div id="mini58815" class="mini" style="display: none;"><a href="#" onclick="edit('58815','content')">
                                            <img alt="编辑" src="images/bj.png" style="padding-top: 4px;" height="12px" width="12px"/>
                                        </a><a href="#" onclick="changeshape('58815')">
                                            <img alt="改变形状" src="images/xz.png" style="padding-top: 4px;" height="12px" width="12px"/>
                                        </a><a href="#" onclick="del('58815')">
                                            <img alt="删除" src="images/sc.png" style="padding-top: 4px;" height="12px" width="12px"/>
                                        </a></div>
                                        <div class="icon">
                                            <img src="images/18.png"/></div>
                                        <div class="picture">
                                            <img src=""/></div>
                                        <div class="bmini" style="color: rgb(12, 12, 12);">联系我们</div>
                                        <div class="news_xx" style="color: rgb(12, 12, 12);"></div>
                                    </dd>
                                    <dd id="58818" class="template-b-02" style="background-color: #dbeef3">
                                        <p class="text_1" style="color: rgb(12, 12, 12);">招募人才</p>
                                        <div id="mini58818" class="mini" style="display: none;"><a href="#" onclick="edit('58818','content')">
                                            <img alt="编辑" src="images/bj.png" style="padding-top: 4px;" height="12px" width="12px">
                                        </a><a href="#" onclick="changeshape('58818')">
                                            <img alt="改变形状" src="images/xz.png" style="padding-top: 4px;" height="12px" width="12px">
                                        </a>
                                            <!--a href="#" class="ccolor" date_id="58818" >						
                                                <img alt="颜色" src="/images/ys.png"  width="12px" height="12px" style="padding-top: 4px;">				
                                                </a -->
                                            <a href="#" onclick="del('58818')">
                                                <img alt="删除" src="images/sc.png" style="padding-top: 4px;" height="12px" width="12px"/>
                                            </a></div>
                                        <div class="icon">
                                            <img src="images/26.png"/></div>
                                        <div class="picture">
                                            <img src=""/></div>
                                        <div class="bmini" style="color: rgb(12, 12, 12);">招募人才</div>
                                        <div class="news_xx" style="color: rgb(12, 12, 12);"></div>
                                    </dd>
                                    <dd id="67590" class="template-b-01">
                                        <p class="text_1"></p>
                                        <div id="mini67590" class="mini" style="display: none;"><a href="javascript:void(0);" onclick="edit('67590','content')">
                                            <img alt="编辑" src="images/bj.png" width="12px" height="12px" style="padding-top: 4px;">
                                        </a><a href="javascript:void(0);" onclick="changeshape('67590')">
                                            <img alt="改变形状" src="images/xz.png" width="12px" height="12px" style="padding-top: 4px;">
                                        </a>
                                            <!--a href="#" class="ccolor" date_id="67590" >						
                                                <img alt="颜色" src="/images/ys.png"  width="12px" height="12px" style="padding-top: 4px;">				
                                                </a -->
                                            <a href="javascript:void(0);" onclick="del('67590')">
                                                <img alt="删除" src="images/sc.png" width="12px" height="12px" style="padding-top: 4px;"/>
                                            </a></div>
                                        <div class="icon">
                                            <img src="images/1.png">
                                        </div>
                                        <div class="picture"></div>
                                        <div class="bmini"></div>
                                        <div class="news_xx"></div>
                                    </dd>

                                </div>
                            </div>
                            <div id="menubar1" class="ds">
                            </div>
                        </div>

                    </div>


                    <div class="tab-box" style="position: absolute; top: 666px; left: 212px;">
                        <input id="fa_btn" type="button" value="发布微站" class="btn" style="background-position: 0 -214px; color: #fff; border: 0; margin: 50px auto; border: 1px solid #54be00; background: #7cc945; width: 140px; height: 30px" />
                    </div>

                    <table id="settable" style="width:100%;border:0;cellspacing:1;cellpadding:0" class="tab-nav">
                        <tr>
                            <td id="mob" onclick="changetmp(this)">
                                <a>更换模版</a>
                            </td>
                            <td id="backjpg" onclick="changeback(this,'back')">
                                <a>更换背景</a>
                            </td>
                            <td id="moreset" onclick="changeback(this,'more')">
                                <a>更多设置</a>
                            </td>
                        </tr>
                    </table>

                    <div id="bo1" class="form-box-mini">
                    </div>
                    <div class="tjbkBox">
                        <a href="#" id="dialog-link"></a>
                    </div>
                </div>

                <input type="hidden" id="isSubPage" value="0" />
                <input type="hidden" id="templateName" value="css/tem12.css" />

                <div style="display: none">

                    <form id="contentForm" name="contentForm" method="post"
                        action="/websitePages/website_pages!updateStie.action">

                        <input type="text" name="we.id" id="weid"
                            value="040BD1CA4AE2AB08E050973BAE0C1EA0" />

                        <input type="text" name="we.title" id="titled" value="" />
                        <input type="text" name="we.loopPic" id="looppicd" value="" />
                        <input type="text" name="we.content" id="contentd" value="" />
                        <input type="text" name="we.deleteId" id="deleteId" value="" />
                        <input type="text" name="we.pageId" id="pageId" value="" />

                    </form>

                </div>


                <!-- ui-dialog -->
                <div id="dialog">

                    <div id="template-b-01" class="template-01" onclick="copy(this)"></div>

                    <div id="template-b-02" class="template-02" onclick="copy(this)"></div>

                    <div id="template-b-03" class="template-03" onclick="copy(this)"></div>

                    <div id="template-b-04" class="template-04" onclick="copy(this)"></div>

                    <div id="template-b-05" class="template-05" onclick="copy(this)"></div>

                </div>


                <div id="shapedialog">

                    <input type="hidden" id="chang_div_id" value="" />

                    <div id="template-b-01" class="template-01" onclick="ch_shape(this.id)"></div>

                    <div id="template-b-02" class="template-02" onclick="ch_shape(this.id)"></div>

                    <div id="template-b-03" class="template-03" onclick="ch_shape(this.id)"></div>

                    <div id="template-b-04" class="template-04" onclick="ch_shape(this.id)"></div>

                    <div id="template-b-05" class="template-05" onclick="ch_shape(this.id)"></div>

                </div>


                <div id="changetmp">
                </div>

                <div id="webCode" style="text-align: center;">
                </div>






            </div>
        </div>
        <div style="clear: both;"></div>
    </div>

</body>
</html>
