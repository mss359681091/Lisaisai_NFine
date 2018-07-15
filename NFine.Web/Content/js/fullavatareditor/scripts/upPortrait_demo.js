swfobject.addDomLoadEvent(function () {

    var webcamAvailable = false;
    var currentTab = 'upload';
    var sourcePic1Url = $.Cookie('swf1');
    var sourcePic2Url = $.Cookie('swf2');
    if (sourcePic2Url == null) {
        sourcePic2Url = "http://www.baidu.com/img/bdlogo.png";
    }
    var callback = function (json) {
        var id = this.id;
        switch (json.code) {
            case 2:
                //如果加载原图成功，说明进入了编辑面板，显示保存和取消按钮，隐藏拍照按钮
                if (json.type == 0) {
                    $('#webcamPanelButton').hide();
                    if (id == "swf1") $('#editorPanelButtons').show();
                }
                    //否则会转到上传面板，隐藏所有按钮
                else {
                    if (id == "swf1") $('#editorPanelButtons,#webcamPanelButton').hide();
                }
                break;
            case 3:
                //如果摄像头已准备就绪且用户已允许使用，显示拍照按钮。
                if (json.type == 0) {
                    if (id == "swf1") {
                        $('.button_shutter').removeClass('Disabled');
                        $('#webcamPanelButton').show();
                        webcamAvailable = true;
                    }
                }
                else {
                    if (id == "swf1") {
                        webcamAvailable = false;
                        $('#webcamPanelButton').hide();
                    }
                    //如果摄像头已准备就绪但用户已拒绝使用。
                    if (json.type == 1) {
                        alert('用户拒绝使用摄像头!');
                    }
                        //如果摄像头已准备就绪但摄像头被占用。
                    else {
                        alert('摄像头被占用!');
                    }
                }
                break;
            case 4:
                layer.msg("您选择的原图片文件大小（" + json.content + "）超出了指定的值(2MB)", { icon: 2, time: 2000 });
                break;
            case 5:
                if (json.content.msg == "Success") {
                    layer.msg("图像上传成功！", { icon: 1, time: 2000 }, function () {
                        var fPath = json.content.filePath;
                        //为保存全部信息暂时保存图片的相对路径
                        parent.$("#img_portrait").val(fPath);
                        //展示图片
                        parent.$(".Portrait").attr("src", fPath);
                        $.Cookie(id, null);

                    });
                }
                else {
                    layer.msg(json.content.msg, { icon: 2, time: 2000 });
                }
                break;
        }
    };
    var swf1 = new fullAvatarEditor('swf1', 335, {
        id: 'swf1',
        upload_url: actionURL,   //后台上传方法
        src_url: sourcePic1Url,			//默认加载的原图片的url
        src_size: "2MB",//选择的本地图片文件所允许的最大值
        src_size_over_limit: "文件大小超出2MB，请重新选择图片。",//当选择的原图片文件的大小超出指定最大值时的提示文本。
        tab_visible: false,				//不显示选项卡，外部自定义
        button_visible: false,				//不显示按钮，外部自定义
        src_upload: 2,						//是否上传原图片的选项：2-显示复选框由用户选择，0-不上传，1-上传
        checkbox_visible: false,			//不显示复选框，外部自定义
        avatar_sizes: '258*200',			//定义单个头像
        avatar_sizes_desc: '258*200像素',	   //头像尺寸的提示文本。
        avatar_intro: '     最终会生成下面这个尺寸的头像', //头像简介
        isShowUploadResultIcon: false,      //是否显示上传结果
        avatar_tools_visible: true,			//是否显示颜色调整工具
    }, callback);
   
    //选项卡点击事件
    $('#avatar-tab li').click(function () {
        if (currentTab != this.id) {
            currentTab = this.id;
            $(this).addClass('active');
            $(this).siblings().removeClass('active');
            //如果是点击“相册选取”
            if (this.id === 'albums') {
                //隐藏flash
                hideSWF();
                showAlbums();
            }
            else {
                hideAlbums();
                showSWF();
                if (this.id === 'webcam') {
                    $('#editorPanelButtons').hide();
                    if (webcamAvailable) {
                        $('.button_shutter').removeClass('Disabled');
                        $('#webcamPanelButton').show();
                    }
                }
                else {
                    //隐藏所有按钮
                    $('#editorPanelButtons,#webcamPanelButton').hide();
                }
            }
            swf1.call('changepanel', this.id);
        }
    });
    //复选框事件
    $('#src_upload').change(function () {
        swf1.call('srcUpload', this.checked);
    });
    //点击上传按钮的事件
    $('.button_upload').click(function () {
        swf1.call('upload');
    });
    //点击取消按钮的事件
    $('.button_cancel').click(function () {
        var activedTab = $('#avatar-tab li.active')[0].id;
        if (activedTab === 'albums') {
            hideSWF();
            showAlbums();
        }
        else {
            swf1.call('changepanel', activedTab);
            if (activedTab === 'webcam') {
                $('#editorPanelButtons').hide();
                if (webcamAvailable) {
                    $('.button_shutter').removeClass('Disabled');
                    $('#webcamPanelButton').show();
                }
            }
            else {
                //隐藏所有按钮
                $('#editorPanelButtons,#webcamPanelButton').hide();
            }
        }
    });
    //点击拍照按钮的事件
    $('.button_shutter').click(function () {
        if (!$(this).hasClass('Disabled')) {
            $(this).addClass('Disabled');
            swf1.call('pressShutter');
        }
    });
    //从相册中选取
    $('#userAlbums a').click(function () {
        var sourcePic = this.href;
        swf1.call('loadPic', sourcePic);
        //隐藏相册
        hideAlbums();
        //显示flash
        showSWF();
        return false;
    });
    //隐藏flash的函数
    function hideSWF() {
        //将宽高设置为0的方式来隐藏flash，而不能使用将其display样式设置为none的方式来隐藏，否则flash将不会被加载，隐藏时储存其宽高，以便后期恢复
        $('#flash1').data({
            w: $('#flash1').width(),
            h: $('#flash1').height()
        })
		.css({
		    width: '0px',
		    height: '0px',
		    overflow: 'hidden'
		});
        //隐藏所有按钮
        $('#editorPanelButtons,#webcamPanelButton').hide();
    }
    function showSWF() {
        $('#flash1').css({
            width: $('#flash1').data('w'),
            height: $('#flash1').data('h')
        });
    }
    //显示相册的函数
    function showAlbums() {
        $('#userAlbums').show();
    }
    //隐藏相册的函数
    function hideAlbums() {
        $('#userAlbums').hide();
    }

});