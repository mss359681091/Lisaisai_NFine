// 
$(function(){
	var pc= $("#pc"),
	    wx = $("#wx"),
		login_box = $("#loginbox"),
		wx_box = $("#wxsm"),
		qrcode = true;
		wx.on('click', function (event) {
			$(this).hide();
			if (qrcode){
				$("#wxlogo").append('<img src="images/liantu.png" alt="xycms二维码">');
				qrcode = false;
			}
			pc.show();
			login_box.hide();
			
			wx_box.show();
		});
		pc.on('click', function (event) {
			$(this).hide();
			login_box.show();
			wx.show();
			wx_box.hide();
		});
		

})