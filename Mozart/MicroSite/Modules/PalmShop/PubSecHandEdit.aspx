<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PubSecHandEdit.aspx.cs" Inherits="Mozart.PalmShop.ShopCode.PubSecHandEdit" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <meta name="viewport" content="width=device-width, initial-scale=1">
		<title>产品编辑</title>
		<link rel="stylesheet" href="css/bootstrap.min.css" />
		<link id="changecss" rel="stylesheet" type="text/css" href="css/base.css"/>
		<link rel="stylesheet" href="css/bootstrap-theme.min.css" />
		<script type="text/javascript" src="js/jquery-1.8.3.min.js" ></script>
		<script type="text/javascript" src="js/bootstrap.js" ></script>
		<script type="text/javascript" src="js/common.js" ></script>
    <script src="artDialog/jquery.artDialog.js?skin=dialogred" type="text/javascript" charset="gbk"></script>
		<!--[if lt IE 9]>
	      <script src="http://cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
	      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
	    <![endif]-->
</head>
<body>
    <form id="form1" method="post" runat="server" action="" enctype="multipart/form-data">
        <asp:Label id="errowmsg" runat="server" />
    <div id="nav" class="container-fluid">
			<div class="row">
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4">
                    <a class="back_ico" onclick="javascript:history.back()">返回</a>
				</div>
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center">产品发布</div>
				<div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-right">
                    <asp:LinkButton Text="完成" class="next_ico" id="uploadbtn" 
                        runat="server" OnClick="uploadbtn_Click" OnClientClick="return validateform();" />
				</div>
			</div>
		</div>
		<div class="container-fluid">
			<div class="row">
				<div class="col-lg-12">
        <ul class="page_pic_emotion clear" id="nav_page">
			<li class="page_pic on">
				<dl>
                    <%=atlaslist %>
				</dl>
			</li>
		</ul>
            <div id="chosestyle">
					<label>商品类别:</label>
                        <asp:DropDownList ID="ddlbigcategorylist" onchange="GetSortOption(this.id);" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlsmallcategorylist"  onchange="setvalue(this.id,'setpvalue')" runat="server">
                        </asp:DropDownList>
                        <input type="text" name="setoptionvalue" runat="server" style="display:none;" id="setpvalue" />
				</div>
				<div class="table-responsive" id="product_info">
				  <table class="table">
				    <tr>
				    	<td class="col-xs-3 text-center">商品标题：</td>
				    	<td class="col-xs-9">
                            <input type="text" name="name" id="ptitle" runat="server" value=" " />
				    	</td>
				    </tr>
				     <tr>
				    	<td class="col-xs-3 text-center">商品描述：</td>
				    	<td class="col-xs-9">
                            <textarea id="pdesc" runat="server"></textarea>
				    	</td>
				    </tr>
				    <tr>
				    	<td class="col-xs-3 text-center">商品价格：</td>
				    	<td class="col-xs-9">
                            <input type="text" name="name" value=" " id="price" runat="server" onkeyup="clearNoNum(this)" />
				    	</td>
				    </tr>
                      <tr>
				    	<td class="col-xs-3 text-center">联系人：</td>
				    	<td class="col-xs-9">
                            <input type="text" id="UserName" onkeyup="onlychinese()" runat="server" />
				    	</td>
				    </tr>
                      <tr>
				    	<td class="col-xs-3 text-center">联系电话：</td>
				    	<td class="col-xs-9">
                            <input type="text" id="UserPhone" runat="server" />
				    	</td>
				    </tr>
				  </table>
				</div>
				</div>
			</div>
		</div>
    </form>
</body>
    <script>
        <%=errorscript%>
        $(document).ready(function () {
            $("body").css("background", "#FFFFFF");
            $("#product_info table td:even").css("border-right", "#ddd 1px solid");
            var pid = Request("pid");
            if (pid == null || pid == "") {
                $("#ddlsmallcategorylist").empty();
                document.getElementById("ddlsmallcategorylist").options.add(new Option("--请选择类别--", ""));
            }
        })
        var form_pics = (function () {
            var fp = function () {
                var rowcount=$(".page_pic dl").find("dd[datacount]").attr("datacount");
                if(rowcount==null||rowcount<=0||rowcount=="undefined")
                {
                    rowcount=1;
                }
                this.length = rowcount;
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
                    var img = $(thi).prev("img[dataid]").attr("dataid");
                    if (img != null && img != "" && img != "undefined")
                    {
                        var pid = Request("pid");
                        if (confirm("您确定要删除该信息？")) {
                            $.getJSON("PubSecHandEdit.aspx", { "action": "delimg", "pid": pid, "img": img },
                                function (json) {
                                    if (json.error == true) {
                                        $.dialog({ time: 2, fixed: true, icon: 'error', content: json.msg });
                                        return false;
                                    }
                                    if (json.success == true) {

                                    }
                                })
                        } else {
                            return false;
                        }
                    }
                    var type = $(thi).closest("dd").remove().attr("type");
                    this.length--;
                    this.createImgFile(thi);
                    return this;
                },
                createImgFile: function (thi) {
                    if (this.length >= 8) {
                        this.length = 8;
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
        function GetSortOption(id) {
            $("#ddlsmallcategorylist").empty();
            document.getElementById("ddlsmallcategorylist").options.add(new Option("--请选择类别--", ""));
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value == 0 || value == "0") { }
            else {
                $.getJSON("PubSecHand.aspx", { "action": "getcateoption", "subid": value }, function (json) {
                    $.each(json, function (i, item) {
                        var optionID = item.ID;
                        var optionTitle = item.Cname;
                        document.getElementById("ddlsmallcategorylist").options.add(
                            new Option("" + optionTitle + "", "" + optionID + ""));
                    })
                })
            }
        }
        function setvalue(id, setvalueid) {
            var select = document.getElementById(id);
            var index = select.selectedIndex;
            var text = select.options[index].text;
            var value = select.options[index].value;
            if (value != null && value != "") {
                document.getElementById(setvalueid).value = value;
            }
        }
        function validateform() {
            var category = $("#setpvalue").val();
            var imgcount = $('.page_pic').find("dd[type=\"image\"]").length;
            var ptitle = $("#ptitle").val();
            var pdesc = $("#pdesc").val();
            var price = $("#price").val();
            var uphone = $("#UserPhone").val();
            if (category == null || category == "") {
                $.dialog({ time: 2, fixed: true, icon: 'error', content: '请选择相应的类别后再操作！' });
                return false;
            }
            if (imgcount < 1) {
                $.dialog({ time: 2, fixed: true, icon: 'error', content: '请至少选择一张产品图！' });
                return false;
            }
            if (pdesc == null || pdesc == "") {
                $.dialog({ time: 2, fixed: true, icon: 'error', content: '请对该信息作一些描述！' });
                $("#pdesc").val(""); $("#pdesc")[0].focus();
                return false;
            }
            if (ptitle == null || ptitle == "") {
                $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入信息标题！' });
                $("#ptitle").val(""); $("#ptitle")[0].focus();
                return false;
            }
            if (price == null || price == "") {
                $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入价格！' });
                $("#price").val(""); $("#price")[0].focus();
                return false;
            }
            if (uphone == null || uphone == "") {
                $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入手机号码！' });
                $("#UserPhone").val(''); ("#UserPhone")[0].focus(); return false;
            } else {
                if (checkTel(uphone) == false) {
                    $.dialog({ time: 1, fixed: true, icon: 'error', content: '请输入正确的电话！' });
                    $("#UserPhone").val(''); $("#UserPhone")[0].focus(); return false;
                }
            }
            return true;
        }
        function clearNoNum(obj) {
            obj.value = obj.value.replace(/[^\d.]/g, "");
            obj.value = obj.value.replace(/^\./g, "");
            obj.value = obj.value.replace(/\.{2,}/g, ".");
            obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
        }
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
        function checkTel(tel) {
            var mobile = /^1[3|5|8]\d{9}$/, phone = /^0\d{2,3}-?\d{7,8}$/;
            return mobile.test(tel) || phone.test(tel);
        }
        function onlychinese() {
            if ((window.event.keyCode >= 32) && (window.event.keyCode <= 126)) {
                window.event.keyCode = 0;
            }
        }
    </script>
</html>