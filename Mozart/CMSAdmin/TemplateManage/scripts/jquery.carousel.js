/*
[轮播图插件] - 朱伟友
*/
; (function () {

    $.fn.extend({

        carousel: function (delay) {

            //初始化
            //----------------------------------------
            delay = delay || 2500;

            var self = this,

				pics = $(self).children(':eq(0)'),

				ctrls = $(self).children(':eq(1)'),

				ctrl = ctrls.children(),

				timer = null,

				index = 0,

				len = pics.children().length,

				width = $(self).width(),

				height = $(self).height()

            ;

            //置必要样式
            //----------------------------------------
            //父容器
            $(self).css({
                'position': 'relative',
                'overflow': 'hidden'
            });

            //图片容器
            pics.css({
                'position': 'absolute',
                'left': 0,
                'top': 0,
                'list-style': 'none',
                'width': width * len,
                'height': height
            });
            pics.children().css({
                'float': 'left',
                'width': width,
                'height': height
            });
            pics.find('img').css({
                'display': 'block',
                'width': width,
                'height': height
            });

            //控制按钮容器
            //用CSS自定义样式

            //轮播逻辑
            //----------------------------------------
            //执行一次切换
            function once() {

                if (++index == len) {
                    index = 0;
                }

                //切换图
                if (pics) {
                    pics.animate({
                        'left': -index * width
                    });
                }

                //切换控制按钮
                if (ctrls) {
                    ctrl.removeClass('active');
                    ctrl.eq(index).addClass('active');
                }
            }

            //开始轮播
            function start() {
                end();
                timer = window.setInterval(once, delay);
            }

            //停止轮播
            function end() {
                timer && window.clearInterval(timer);
                timer = null;
            }

            //监听控制按钮事件
            if (ctrls) {
                ctrl.click(function () {
                    end();
                    index = $(this).index() - 1;
                    once();
                    start();
                });
            }

            start();

            return this;
        }


    });


})(jQuery);