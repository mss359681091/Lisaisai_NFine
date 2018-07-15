// JavaScript Document
//搜索分类代码
$('#searchvalue').toggle(
function(){
	$('#valuelist').show();
	$('#boult').text('▲');
},function(){
	$('#valuelist').hide();
	$('#boult').text('▼');
});

$('#boult').toggle(
function(){
	$('#valuelist').show();
	$('#boult').text('▲');
},function(){
	$('#valuelist').hide();
	$('#boult').text('▼');
});
$('#valuelist li').click(function(){
	var v = $(this).attr('title');
	var name = $(this).text();
	$('#typeid').val(v);
	$('#searchvalue').text(name);
	$('#boult').text('▼');
	$('#valuelist').hide();
})

//搜索热门关键字代码
function fbbox(fbboxID,ObjID){
	$(fbboxID).click(function(){
		$(ObjID).show();
	});
	$(ObjID).hover('',function(){
		$(ObjID).hide();
	});
	$(fbboxID).keydown(function(){
		$(ObjID).hide();
	});
	$(ObjID).find('li').click(function(){
		var text = $(this).find('h1').text();
		$(fbboxID).val(text);
		$(ObjID).hide();
	});
}
fbbox('#search-keyword','#hotwords');

//搜索框检测代码
function checkinput(){
	var search = $('#search-keyword');
	var isNull = /^[\s' ']*$/;
	if(search.val() == '输入关键词，回车搜索全国设计素材资源' || search.val().length <= 0 || isNull.test(search.val())){
		search.focus();
		window.alert("请输入关键词，搜索关键词不能为空");
		return false;
	}
}

//导航代码
$('#navMenu dl dt').hover(function(){
	$(this).find('.dropMenu').show();
	if($(this).attr("class") != 'clear-btn'){
	$(this).addClass('hover');
	}
},function(){
	$(this).find('.dropMenu').hide();
	$(this).removeClass('hover');
})

//时间差计算
function dayBetween(lasttime)
{
	$('.daybetween').each(function(){
		var timestamp = Date.parse(new Date())/1000;	
		timestamp = timestamp - $(this).text();
		//timestamp = timestamp+'/'+$(this).text();
		var daytime = timestamp/86400;
		var htime = timestamp/3600;
		var mtmie = timestamp/60;
		var stime = timestamp/1;
		if(daytime>=1)
		{
			daytime = Math.floor(daytime);
			var t = daytime+'天前';
			$(this).text(t);
		}
		else if(htime>=1)
		{
				daytime = Math.floor(htime);
			var t = daytime+'小时前';
			$(this).text(t);
		}
		else if(mtmie>=1)
		{
			daytime = Math.floor(mtmie);
			var t = daytime+'分钟前';
			$(this).text(t);
		}	
		else
		{
			if(timestamp<=0){
				daytime = 1;
			}else{
				daytime = timestamp;
				}
			var t = daytime+'秒前';
			$(this).text(t);
		}	
		})
}