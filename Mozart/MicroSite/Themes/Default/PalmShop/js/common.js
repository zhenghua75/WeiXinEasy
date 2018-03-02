$(function(){
	$("#money_text,#search").focus(function(event) {
		if($(this).val()==this.defaultValue)
		{
			$(this).val("");
		}
	}).blur(function(event) {
		if($(this).val()=='')
		{
			$(this).val(this.defaultValue);
		}
	});
})

//$(document).ready(function(){
//	$(".pihu_list").css("left",($(window).width()-$(".pihu_list").width())/2);
//})
$(window).resize(function(){
	$(".box").css("height",$(document).height());
})

function show()
{
	var $boxshadow=$('<div class="box"></div>');
	var $shadoemain=$('<div class="choseMain"></div>');
	var $choselist=$('<ul class="list-unstyled"><h4 class="text-center">请选择您喜欢的皮肤</h4><li class="one"><img src="img/youfu.png"  /><p>马上有福</></li><li class="two"><img src="img/mucai.png"  /><p>清新木质</></li><li class="three"><img src="img/haibian.png" class="" /><p>清凉海边</></li></ul>')
	$("body").append($boxshadow);
	$boxshadow.append($shadoemain);
	$shadoemain.append($choselist);
	$(".box").css("height",$(document).height());
	var $anintop=($(window).height()-$(".choseMain").height())/2;
	$(".choseMain").animate({top:$anintop},500);
	$(".one").click(function(){
		$("#changecss").attr("href","css/base.css");
		$(".box").fadeTo(0).hide();
	})
	$(".two").click(function(){
		$("#changecss").attr("href","css/mood.css");
		$(".box").fadeTo(0).hide();
	})
	$(".three").click(function(){
		$("#changecss").attr("href","css/haian.css");
		$(".box").fadeTo(0).hide();
	})
}

$(function(){
	$(".choseMain ul li img").hover(function(){
		$(this).animate({"margin-top":"-10px"},2000);
	})
})


$(function(){
	$(".Product_List:gt(0)").hide();
	var $tabmenu=$(".Product_Menu li");
	$tabmenu.click(function(){
		$(this).addClass("item_red").siblings('li').removeClass("item_red");
		$(".Product_List").eq($tabmenu.index(this)).show().siblings(".Product_List").hide();
	})
})

$(function(){
	$(".OrderList:gt(0)").hide();
	var $tabmenu1=$(".OrderMenu li");
	$tabmenu1.click(function(){
		$(this).addClass("item_red").siblings('li').removeClass("item_red");
		$(".OrderList").eq($tabmenu1.index(this)).show().siblings(".OrderList").hide();
	})
})

$(function(){
	$(".ctTab_box:gt(0)").hide();
	var $tabmenu2=$(".ctTab_menu li");
	$tabmenu2.click(function(){
		$(this).addClass("item2").siblings('li').removeClass("item2");
		$(".ctTab_box").eq($tabmenu2.index(this)).show().siblings(".ctTab_box").hide();
	})
})
