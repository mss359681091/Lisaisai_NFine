var currpage = 1;//默认当前第一页
(function ($) {
    $.piclist_imgmanager = {
        init: function () {
            $.piclist_imgmanager.loadcategory();
            $.piclist_imgmanager.fnimglist(true);
            $.piclist_imgmanager.fnagain();
            $.piclist_imgmanager.bind();

        },
        bind: function () {
            $("#btnChange").on("click", function () {
                var $this = $(this);
                var $i = $this.children("i");
                if ($i.hasClass("fa-th-large")) {
                    $this.attr("title", "切换到列表模式")
                    $i.removeClass("fa-th-large").addClass("fa-th-list");
                    $("#tab2").show();
                    $("#tab1").hide();

                }
                else {
                    $this.attr("title", "切换到缩略图模式")
                    $i.removeClass("fa-th-list").addClass("fa-th-large");
                    $("#tab1").show();
                    $("#tab2").hide();
                }
            });
            $("#btnSearch,#NF_search").on("click", function () {
                $.piclist_imgmanager.fnimglist(true);
            });
            $(".file-control").on("click", function () {
                $(".file-control").removeClass("active");
                $(this).addClass("active");
                $.piclist_imgmanager.fnimglist(true);
            });
            $("#NF-add").on("click", function () {
                $.piclist_imgmanager.btn_add();
            });
            $("#NF-edit").on("click", function () {
                $.piclist_imgmanager.btn_edit();
            });
            $('.mail-box').slimScroll({
                height: '700px'
            }).on('slimscroll', function (e, pos) {
                if (pos == 'bottom') {
                    currpage += 1;
                    $.piclist_imgmanager.fnimglist(false);
                }
            });
            $('#ulCategory').slimScroll({
                height: '480px'
            });
            $("#imglst").on("click", "tr", function () {
                var $chk = $(this).find('input[type="checkbox"]');
                var flag = 'check';
                if ($chk.is(':checked')) {
                    flag = 'uncheck';
                }
                $chk.iCheck(flag);
            });
        },
        loadcategory:function(){
                $.ajax({
                    url: "/SystemManage/ItemsData/GetSelectJson",
                    data: { enCode: "Category" },
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        var strhtml = '';
                        strhtml += '<li data-id=""><a href="#"><i class="fa fa-folder"></i>查看全部</a></li>';
                        $.each(data, function (i) {
                            strhtml += '<li data-id="' + data[i]["id"] + '"><a href="#"><i class="fa fa-folder"></i>' + data[i]["text"] + '</a></li>';
                        });
                        $("#ulCategory").empty().append(strhtml);

                        $("#ulCategory li").bind("click", function () {
                            $("#ulCategory li").removeClass("li_active");
                            $(this).addClass("li_active");
                            $("#ulCategory li i").each(function () {
                                if ($(this).hasClass("fa-folder-open")) {
                                    $(this).removeClass("fa-folder-open").addClass("fa-folder");
                                }
                            });
                            $(this).find("i").removeClass("fa-folder").addClass("fa-folder-open");
                            $.piclist_imgmanager.fnimglist(true);
                        });
                    }
                });
            },
        fnimglist: function (isEmpty) {
            var crows = 20;//默认20一页
            if (isEmpty) {
                currpage = 1;
            }
            var loadindex = null;
            var category = $(".li_active").data("id");
            var txt_keyword = $("#txt_keyword").val();
            var filetype = $(".active").data("id");
            var strhtm = '';
            var strhtml = '';
            $.ajax({
                type: "post",
                url: "/Canchong/PicList/GetImglstJson",
                data: { category: category, page: currpage, keyword: txt_keyword, rows: crows, filetype: filetype },
                beforeSend: function () {
                    loadindex = layer.load(1, {
                        shade: [0.1, '#fff'] //0.1透明度的白色背景
                    });
                },
                complete: function (data) {
                    layer.close(loadindex);
                },
                dataType: "json",
                async: true,
                success: function (result) {

                    if (!result.state) {
                        $.modalMsg('拉取失败', 'error');
                        return;
                    }
                    if (result.data == null || result.data == 'null') {
                        $.modalMsg('暂无数据', 'warning');
                        return;
                    }


                    $("#allcount").text(result.message);//总数
                    $.each(result.data, function (idx, obj) {
                        var category = top.clients.dataItems.Category[obj.F_Category] == undefined ? "" : top.clients.dataItems.Category[obj.F_Category];
                        //var date = $.FormatDate(obj.F_CreatorTime, 'yyyy-M-d');
                        var date = new Date(obj.F_CreatorTime).Format("yyyy-MM-dd");
                        var enable = obj.F_EnabledMark == 1 ? '<i class=\"fa fa-toggle-on\"></i>' : '<i class=\"fa fa-toggle-off\"></i>';

                        strhtm += '<tr class="read">';
                        strhtm += '<td class="check-mail">';
                        strhtm += '<input type="checkbox" data-id="' + obj.F_Id + '" class="i-checks">';
                        strhtm += '</td>';
                        strhtm += '<td class="text-center">' + (((parseInt(currpage) - 1) * crows) + (parseInt(idx) + 1)) + '</td>';
                        strhtm += '<td class="text-left">';
                        strhtm += '<a href="#" title="' + obj.F_Nick + '">' + subString1(obj.F_Nick, 40) + '</a>';
                        strhtm += '</td>';
                        strhtm += '<td class="text-left">';
                        strhtm += '<a href="#" title="' + obj.F_ThumbnailPath + '">' + subString1(obj.F_ThumbnailPath, 40) + '</a>';
                        strhtm += '</td>';
                        strhtm += '<td class="text-center">' + category + '</td>';
                        strhtm += '<td class="text-center">' + obj.F_FileSize + '</td>';
                        strhtm += '<td class="text-center">' + date + '</td>';
                        strhtm += '<td class="text-center">';
                        strhtm += enable;
                        strhtm += '</td>';
                        strhtm += '</tr>';

                        strhtml += '<div class="file-box">';
                        strhtml += '<div class="file">';
                        strhtml += '<a href="' + obj.F_ThumbnailPath + '" title="' + obj.F_Nick + '" data-gallery="">';
                        strhtml += '<span class="corner"></span>';

                        if ($.trim(obj.F_ThumbnailPath) == '' || obj.F_ThumbnailPath == null || obj.F_ThumbnailPath == 'null') {
                            strhtml += '<div class="icon">';
                            strhtml += '<i class="fa fa-image"></i>';
                            strhtml += '</div>';
                        }
                        else {
                            strhtml += '<div class="image">';
                            strhtml += '<img alt="" class="img-responsive" src="' + obj.F_ThumbnailPath + '" >';
                            strhtml += '</div>';
                        }

                        strhtml += '<div class="file-name">';
                        strhtml += obj.F_Nick;
                        strhtml += '<br />';
                        strhtml += '<small>' + date + '</small>';
                        strhtml += '</div>';
                        strhtml += '</a>';
                        strhtml += '</div>';
                        strhtml += '</div>';

                    });


                    if (isEmpty) {
                        $('#chkall').iCheck('uncheck');
                        $("#imglst").empty().append(strhtm);
                        $("#piclst").empty().append(strhtml);
                        if ($.trim(strhtm) == '') {
                            $.modalMsg('暂无图片', 'warning');
                            return;
                        }
                    }
                    else {
                        if ($.trim(strhtm) == '') {
                            //$.modalMsg('没有更多了', 'warning');
                            return;
                        }
                        $("#imglst").append(strhtm);
                        $("#piclst").append(strhtml);
                    }
                    $.piclist_imgmanager.fnagain();//二次加载
                }
            });
        },
        fnagain:function(){
            $(".i-checks").iCheck({
                handle: 'checkbox',
                checkboxClass: 'icheckbox_minimal-green',
                radioClass: 'iradio_minimal-green'
            });
            $('#chkall').on('ifChanged', function (event) {
                var flag = 'uncheck';
                if ($(this).is(':checked')) {
                    flag = 'check';
                }
                $('#imglst input[type="checkbox"]').iCheck(flag);
            });
        },
        fngetids: function () {
            var strIds = '';
            $("#imglst :checkbox:checked").each(function () {
                strIds += $(this).data("id") + ",";
            });
            if (strIds != '') {
                strIds = strIds.substring(0, strIds.length - 1);
            }
            return strIds;
        },
        btn_delete: function () {
            if ($("#imglst :checkbox:checked").length <= 0) {
                $.modalMsg('请选择图片', 'warning');
                return;
            }
            var ids = $.piclist_imgmanager.fngetids();
            $.deleteForm({
                url: "/Canchong/PicList/DeleteForm",
                param: { keyValue: ids },
                success: function () {
                    $.modalMsg('删除成功', 'success');
                    $.piclist_imgmanager.fnimglist(true);
                }
            })
        },
        btn_disabled: function () {
            var Ids = $.piclist_imgmanager.fngetids();
            if (Ids.length == 0) {
                $.modalMsg("请选择禁用项", "warning");
                return;
            }
            $.modalConfirm("注：您确定要【启用】选中图片吗？", function (r) {
                if (r) {
                    $.submitForm({
                        url: "/Canchong/PicList/DisabledConents",
                        param: { keyValue: Ids },
                        success: function () {
                            $.modalMsg('操作成功', 'success');
                            $.piclist_imgmanager.fnimglist(true);
                        }
                    })
                }
            });
        },
        btn_enabled: function () {
            var Ids = $.piclist_imgmanager.fngetids();
            if (Ids.length == 0) {
                $.modalMsg("请选择启用项", "warning");
                return;
            }
            $.modalConfirm("注：您确定要【启用】选中图片吗？", function (r) {
                if (r) {
                    $.submitForm({
                        url: "/Canchong/PicList/EnabledConents",
                        param: { keyValue: Ids },
                        success: function () {
                            $.modalMsg('操作成功', 'success');
                            $.piclist_imgmanager.fnimglist(true);
                        }
                    })
                }
            });
        },
        btn_download: function () {
            var Ids = $.piclist_imgmanager.fngetids();
            if (Ids.length == 0) {
                $.modalMsg("请选择下载项", "warning");
                return;
            }
            $.download("/Canchong/PicList/DownloadBackup", "keyValue=" + Ids, 'post');
        },
        btn_add: function () {
            $.modalOpen({
                id: "Form",
                title: "新增",
                url: "/Canchong/PicList/Form",
                width: "80%",
                height: "90%",
                callBack: function (iframeId) {
                    top.frames[iframeId].submitForm();
                }
            });
        },
        btn_edit: function () {
            var selectedRowIds = $.piclist_imgmanager.fngetids();
            if (selectedRowIds.length == 0) {
                $.modalMsg("请选择编辑项", "warning");
                return;
            }
            if ($("#imglst :checkbox:checked").length > 1) {
                $.modalMsg("编辑项不能批量操作", "warning");
                return;
            }
            var keyValue = selectedRowIds;

            $.modalOpen({
                id: "Form",
                title: "修改图片",
                url: "/Canchong/PicList/Form?keyValue=" + keyValue,
                width: "80%",
                height: "90%",
                callBack: function (iframeId) {
                    top.frames[iframeId].submitForm();
                }
            });
        },
        btn_upload: function () {
            var selectedRowIds = $.piclist_imgmanager.fngetids();
            if (selectedRowIds.length == 0) {
                $.modalMsg("请选择同步项", "warning");
                return;
            }
            $.modalConfirm("注：您确定要【同步】选中图片吗？", function (r) {
                if (r) {
                    $.submitForm({
                        url: "/Canchong/PicList/UploadConents",
                        param: { keyValue: selectedRowIds.toString() },
                        success: function () {
                            $.currentWindow().$("#NF_search").trigger("click");
                        }
                    })
                }
            });
        }
    };
    $(function () {
        $.piclist_imgmanager.init();//初始化
    });
})(jQuery);