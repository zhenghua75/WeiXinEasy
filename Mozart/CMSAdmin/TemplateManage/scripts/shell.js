$('.ctrls > ul > li').click(function () {
    $(this).addClass('on').siblings().removeClass('on');
    $('.pages > ul').removeClass('on').eq($(this).index()).addClass('on');
});
var template_id = Request.GetQuery('template_id');
var ecid = Request.GetQuery('ecid');
var moveFlag = false;

//判断来源为Step1页
if (Request.GetQuery('referer') == 'Step1') {
    var result = Request.Sync('/CMSAdmin/TemplateManage/GetGenerateResource.ashx', {
        'ecid': ecid,
        'template_id': template_id
    });

    if (result.code == 1) {
        try {
            $('#top').attr('src', result.data.nav[0].image_url);
        } catch (e) { }
        try {
            $('#top_txt').attr('src', result.data.nav[1].image_url);
        } catch (e) {
            $('#top_txt').hide();
        }

        if (result.data.fun.length > 0) {
            $.each(result.data.fun, function () {
                $('#func' + this.ordernum)
                    .attr({ 'src': this.image_url, 'wap_url': this.wap_url })
                    .click(function () {
                        if (!moveFlag) {
                            window.location.href = $(this).attr('wap_url');
                        }
                    }).mousemove(function () {
                        moveFlag = true;
                    }).mousedown(function () {
                        moveFlag = false;
                    })
                    .siblings('p').html(this.text)
                    .css('color', this.textcolor);
            });
        }
        if (result.data.menu.length > 0) {
            $.each(result.data.menu, function () {
                $('#menu' + this.ordernum)
                 .attr({ 'src': this.image_url, 'wap_url': this.wap_url })
                 .click(function () {
                     window.location.href = $(this).attr('wap_url');
                 })
                .siblings('p').html(this.text)
                .css('color', this.textcolor);
            });

            $('#menu').siblings().find('li').width(100 / (result.data.menu.length - 1) + '%');
            $('#menu').attr('src', result.data.menu[0].image_url);
        }

        $('.phone').css('background', 'url(' + result.data.bg.image_url + ')').css("background-size", "cover");
    }
}


