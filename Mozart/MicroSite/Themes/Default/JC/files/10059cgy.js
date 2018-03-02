/**
 * @author luwenbin@live.com
 */
var _={
	bt:10059,
	playName:"冠军竞猜",
	playTypes:function(a){
		var html=[],v1='',v2='';
		if(a=="a0"){
			v1='class="onn"';
		}else if(a=="a1"){
			v2='class="onn"';
		}
		html.push('<a data="a0" href="'+WebAppUrl.HOME_APP_URL+'/caiguanya?type=a0" '+v1+' data2="世界杯">世界杯</a>');
		html.push('<a data="a1" href="'+WebAppUrl.HOME_APP_URL+'/caiguanya?type=a1" '+v2+' data2="欧冠杯">欧冠杯</a>');
		return html.join('');
	}
};
_.a0={
	playName:_.playName,
	playType:"世界杯",
	pass:"冠亚军",
	input:"WC",
	bet:function(){
		$.ajax({
			url:WebAppUrl.HOME_APP_URL+"/lottery/worldcupgj",
			beforeSend:function(){cp2y.dialog.loading();},
			success:function(data){
				cp2y.dialog.clearLoading();
				var html0=[],i=0,len=data.datalist.length,len2=0;
				for(i;i<len;i++){
					//html0.push('<li onclick="cp2y.buy.select(this)" data_id="'+i+'" data_t="'+data.datalist[i].teamNameG+'" data_sp="'+data.datalist[i].sp+'" ><i>'+(i+1).addZero()+'</i><div>'+data.datalist[i].teamNameG+'<span>'+data.datalist[i].sp+'</span></div></li>');
					if(data.datalist[i].status == 0  && data.datalist[i].sellStatus == 0){
						html0.push('<li onclick="cp2y.buy.select(this)" data_id="'+i+'" data_t="'+data.datalist[i].teamNameG+'" data_sp="'+data.datalist[i].sp+'" ><i>'+(i+1).addZero()+'</i><div>'+data.datalist[i].teamNameG+'<span>'+data.datalist[i].sp+'</span></div></li>');
						len2++;
					}else{
						var wo;
						if(data.datalist[i].sellStatus==1){
							wo='停售';
						}else if(data.datalist[i].sellStatus==2){
							wo='已出局';
						}else if(data.datalist[i].sellStatus==3){
							wo='冠军';
						}else{
							wo='';
						}
						html0.push('<li class="out"><i>'+(i+1).addZero()+'</i><div>'+data.datalist[i].teamNameG+'<span>['+wo+']</span></div></li>');
					}
				}
				jcDom.choose.html(html0.join(''));
				cp2y.buy.betType=data.betType;
				cp2y.buy.iid=data.issues.id;
				cp2y.buy.issue=data.issues.issue;
				jcDom.jcTo.html("共"+len2+"支");
				jcDom.jcTime.html("截止:"+data.sellEndTime);
			},
			error:function(){cp2y.dialog.clearLoading();}
		});
	},
	select:function(_this){
		$(_this).toggleClass("sel");
		this.count();
	},
	count:function(){
		var b=this.units();
		var units = b.length;
		jcDom.curBets.html(units);
		jcDom.curMoney.html('￥'+(units*2));
	},
	units:function(){
		var rb=[],i=0,o=this.getBall(),len=o.length,s=1;
		for(i;i<len;i++){
			if(o.eq(i).hasClass("sel")){
				rb.push(o.eq(i));
			}
		}
		return rb;
	},
	addContent:function(){
		var b=this.units(),o=[],i=0,len=b.length,c,d,sp;
		if(len==0){return cp2y.dialog.alert('请至少选择一项');}
		for(i;i<len;i++){
			c=$(b)[i].attr('data_t');
			d=$(b)[i].attr('data_id');
			sp=$(b)[i].attr('data_sp');
			o.push('<li data_input="'+this.input+'" data_id="'+d+'" data_sp="'+sp+'" data_bets="1" data_code="'+c+'"><div>'+c+'</div><p>'+this.playType+'：1注2元</p><i class="delI" title=\"删除\" onclick="cp2y.buy.del(this)"></i></li>');
		}
		this.addRecord(o.join(''),len);
		cp2y.buy.step2();
	}
};
_.a1={
	playName:_.playName,
	playType:"欧冠杯",
	pass:"冠军",
	input:"ECC",
	bet:function(){
		$.ajax({
			url:WebAppUrl.HOME_APP_URL+"/lottery/ogbgj",
			beforeSend:function(){cp2y.dialog.loading();},
			success:function(data){
				cp2y.dialog.clearLoading();
				var html0=[],i=0,len=data.datalist.length,len2=0;
				for(i;i<len;i++){
					if(data.datalist[i].status == 0  && data.datalist[i].sellStatus == 0){
						html0.push('<li onclick="cp2y.buy.select(this)" data_id="'+i+'" data_t="'+data.datalist[i].teamNameG+'" data_sp="'+data.datalist[i].sp+'" ><i>'+(i+1).addZero()+'</i><div>'+data.datalist[i].teamNameG+'<span>'+data.datalist[i].sp+'</span></div></li>');
						len2++;
					}else{
						var wo;
						if(data.datalist[i].sellStatus==1){
							wo='停售';
						}else if(data.datalist[i].sellStatus==2){
							wo='已出局';
						}else if(data.datalist[i].sellStatus==3){
							wo='冠军';
						}else{
							wo='';
						}
						html0.push('<li class="out"><i>'+(i+1).addZero()+'</i><div>'+data.datalist[i].teamNameG+'<span>['+wo+']</span></div></li>');
					}
				}
				jcDom.choose.html(html0.join(''));
				cp2y.buy.betType=data.betType;
				cp2y.buy.iid=data.issues.id;
				cp2y.buy.issue=data.issues.issue;
				jcDom.jcTo.html("共"+len2+"支");
				jcDom.jcTime.html("截止:"+data.sellEndTime);
			},
			error:function(){cp2y.dialog.clearLoading();}
		});
	},
	select:_.a0.select,
	count:_.a0.count,
	units:_.a0.units,
	addContent:_.a0.addContent
};
