$(document).ready(function () {
    var animation = function () {
        var p = $('div.banner');
        if (p.hasClass('banner_game')) {
            p.fadeTo('normal', 0.2, function () {
                p.removeClass('banner_game');
                p.addClass('banner_video');
                $('div.menu > ul > li:eq(1)').removeClass('active');
                $('div.menu > ul > li:eq(2)').addClass('active');
            });
        } else if (p.hasClass('banner_video')) {
            p.fadeTo('normal', 0.2, function () {
                p.removeClass('banner_video');
                p.addClass('banner_novel');
                $('div.menu > ul > li:eq(2)').removeClass('active');
                $('div.menu > ul > li:eq(3)').addClass('active');
            });
        } else if (p.hasClass('banner_novel')) {
            p.fadeTo('normal', 0.2, function () {
                p.removeClass('banner_novel');
                p.addClass('banner_music');
                $('div.menu > ul > li:eq(3)').removeClass('active');
                $('div.menu > ul > li:eq(4)').addClass('active');
            });
        } else if (p.hasClass('banner_music')) {
            p.fadeTo('normal', 0.2, function () {
                p.removeClass('banner_music');
                $('div.menu > ul > li:eq(4)').removeClass('active');
                $('div.menu > ul > li:eq(0)').addClass('active');
            });
        } else {
            p.fadeTo('normal', 0.2, function () {
                p.addClass('banner_game');
                $('div.menu > ul > li:eq(0)').removeClass('active');
                $('div.menu > ul > li:eq(1)').addClass('active');
            });
        }
        p.fadeTo('normal', 1);
    };
    var id = setInterval(animation, 5000);
    var time_id = null;
    $('div.menu > ul > li').click(function () {
        id && clearInterval(id);
        $('div.menu > ul > li').removeClass('active');
        $(this).addClass('active');
        var p = $('div.banner');
        var a = $(this).find('a:first');
        if (a.hasClass('menu_home')) {
            p.attr('class', 'banner');
        } else if (a.hasClass('menu_game')) {
            p.attr('class', 'banner banner_game');
        } else if (a.hasClass('menu_video')) {
            p.attr('class', 'banner banner_video');
        } else if (a.hasClass('menu_novel')) {
            p.attr('class', 'banner banner_novel');
        } else if (a.hasClass('menu_music')) {
            p.attr('class', 'banner banner_music');
        }
        time_id && clearTimeout(time_id);
        time_id = setTimeout(function () {
            id = setInterval(animation, 5000);
        }, 5000);
    });
});