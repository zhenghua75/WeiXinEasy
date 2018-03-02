var Step2_Request = {
    //获取应用名称
    GetAppName: function (ecid, template_id) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/GetBaseConfig.ashx', {
            'ecid': ecid,
            'template_id': template_id
        });
        if (result.code == 1) {
            //应用名称
            $('#app_name').html(result.data.app_name);
        }
    },

    //初始化页面状态
    InitGenerateStatus: function (ecid, template_id, tag) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/BatchGetGenerateTaskByTag.ashx', {
            'ecid': ecid,
            'template_id': template_id,
            'tag': tag
        });
        //console.log("InitGenerateStatus------------------------>");
        //console.log(result);
        if (result.code == 1) {
            $.each(result.data, function () {
                Step2.InitGenerateStatus(this["task_id"], this["status"], this["wait_count"], this["platform_type"], this["generate_percent"], this["regenerate_count"], this["client_download_url"], this["client_qr_url"]);
            });
        }
    },
    //判断是否有证书
    HaveCre: function (ecid) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/HaveCre.ashx', {
            'ecid': ecid,
            'platform_type': 'iphone'
        });
        //console.log("HaveCre------------------------>");
        //console.log(result);
        if (result.data == false) {
            $("#generate_iphone_start").hide();
            $("#generate_uplaod_iphone").show();
        }

    },
    //开始生成应用
    StartGenerate: function (ecid, template_id, platform_type, tag) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/StartGenerate.ashx', {
            'ecid': ecid,
            'template_id': template_id,
            'platform_type': platform_type,
            'tag': tag
        });
        //console.log("StartGenerate------------------------>");
        //console.log(result);

        if (result.code == 1) {
            $("generate_task_" + platform_type).attr("status", 0);
            //设置属性
            Step2.InitGenerateStatus(result.data["task_id"], result.data["status"], result.data["wait_count"], platform_type, result.data["generate_percent"], result.data["regenerate_count"], result.data["client_download_url"], result.data["client_qr_url"]);
        } else if (result.code == 3 && platform_type == "iphone") {
            //需要上传证书
            $("#generate_uplaod_iphone").show();
        } else {
            popAlert(result.notice);
        }
    },
    ReGenerate: function (ecid, task_id, platform_type, tag) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/ReGenerate.ashx', {
            'ecid': ecid,
            'task_id': task_id,
            'platform_type': platform_type,
            'tag': tag
        });
        //console.log("ReGenerate------------------------>");
        //console.log(result);
        if (result.code == 1) {
            //设置属性
            $('#generate_task_' + platform_type).attr("status", "");
            progresss[platform_type] = 0;
            Step2.InitGenerateStatus(result.data["task_id"], result.data["status"], result.data["wait_count"], platform_type, result.data["generate_percent"], result.data["regenerate_count"], result.data["client_download_url"], result.data["client_qr_url"]);
        } else if (result.code == 3 && platform_type == "iphone") {
            //需要上传证书
            $("#generate_uplaod_iphone").show();
        } else {
            popAlert(result.notice);
        }
    },
    //批量获取生成状态
    BatchGetGenerateTask: function (ecid, template_id, tag) {
        var result = Request.Sync('/CMSAdmin/TemplateManage/BatchGetGenerateTaskByTag.ashx', {
            'ecid': ecid,
            'template_id': template_id,
            'tag': tag
        });
        if (result.code == 1) {
            $.each(result.data, function () {
                Step2.InitGenerateStatus(this["task_id"], this["status"], this["wait_count"], this["platform_type"], this["generate_percent"], this["regenerate_count"], this["client_download_url"], this["client_qr_url"]);
            });
        } else {
            errorCount++;
            if (errorCount > 5) {
                //console.log('==============>>> stop');
                Step2.GetGenerateTaskStatusTimer(false);
            }
        }
    },
    //上传资源
    UploadUploadCertificateFile: function (id, ecid, callback) {
        new AjaxUpload(id, {
            action: '/CMSAdmin/TemplateManage/UploadCertificate.ashx',
            name: '__' + id.substr(1),
            onSubmit: function (file, e) {
                if (!new RegExp('^mobileprovision$', 'i').test(e)) {
                    popAlert("错误：无效的文件扩展名！");
                    return false;
                }
                this.setData({
                    "ecid": ecid,
                    'type': 1
                });
                this.disable();
            },
            onComplete: function (file, response) {
                this.enable();
                if (callback) {
                    callback($.parseJSON(response));
                }
            }
        });
    }
}