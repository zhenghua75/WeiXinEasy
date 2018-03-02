var imgcount = 8; var tid = "";
try {
    tid = Request("tid");
    if (tid == "D3019417B0FF446AA4D83819391617DD") {
        imgcount = 2;
    }
} catch (e) {
    imgcount = 8;
}
var form_pics = (function () {
    var fp = function () {
        this.length = 1;
    }
    fp.prototype = {
        addImg: function (thi) {
            if (thi.files && thi.files[0]) {
                var img = thi.nextSibling;
                var filename = thi.value;
                var strtype = "";
                try {
                    strtype = filename;
                } catch (e) {
                }
                strtype = filename.substring(filename.length - 3, filename.length);
                strtype = strtype.toLowerCase();
                if (strtype == "jpg" || strtype == "gif" || strtype == "jpeg" ||
                    strtype == "bmp" || strtype == "png") {
                    var URL = window.URL || webkitURL;
                    var url = URL.createObjectURL(thi.files[0]);
                    img.src = url;
                    img.onload = function (e) {
                        window.URL.revokeObjectURL(this.src); //图片加载后，释放object URL
                    }
                    thi.parentNode.setAttribute("type", "image");
                }
                else {
                    $.dialog({ time: 2, fixed: true, icon: 'error', content: '只能上传jpg、png格式的图片！' });
                    return;
                }
                this.createImgFile(thi);
                this.length++;
                thi.setAttribute("style", "display:none;");
                return this;
            }
        },
        removeImg: function (thi) {
            var type = $(thi).closest("dd").remove().attr("type");
            this.length--;
            this.createImgFile(thi);
            return this;
        },
        createImgFile: function (thi) {
            if (this.length >= imgcount) {
                this.length = imgcount;
                return this;
            }
            var TPL = '<dd><input type="file" accept="image/jpg, image/jpeg, image/png"' +
                '  onchange="form_pics.addImg(this);" name="pics' + this.length + '" />' +
                '<img src="images/upload.png"/>\
							<span onclick="form_pics.removeImg(this);">&nbsp;</span></dd>';
            $(thi).closest("dl").append($(TPL));
            return this;
        }
    }
    return new fp();
})();
function Request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}