/*
**此类仅用于一行图片翻页使用

data = 待分页数组
size = 一页几条
active = 取选中图片的选择器
img = 取一行图片父容器的选择器
prevBtn = 上一页按钮的选择器
nextBtn = 下一页按钮的选择器
format = 生成图片的HTML格式
*/
function Paging(data, size, active, imgs, prevBtn, nextBtn, format) {

    var self = this;

    self.active = null;

    var loop = 0;

    var len = parseInt(data.length / size);

    if (data.length % size != 0) {
        len++;
    }

    $(prevBtn).hide();
    $(nextBtn).show();

    if (data.length <= size) {
        $(nextBtn).hide();
    }

    //上一页
    this.prev = function () {

        $(nextBtn).show();

        loop--;
        //如果到了第一页,还往上,就啥也不干
        if (loop == 0) {
            $(prevBtn).hide();
        }
        else if (loop == -1) {
            loop = 0;
            return;
        }

        this.current();
    };

    //下一页
    this.next = function () {

        $(prevBtn).show();


        loop++;

        console.log("next beging------->" + loop + " len=" + len);

        if (loop == len - 1) {
            console.log("next btn hide------->" + loop);
            $(nextBtn).hide();
        }
        else if (loop == len) {
            loop = len - 1;
            console.log("next loop == len------->" + loop);
            return;
        }

        console.log("next end------->" + loop);


        this.current();

    };

    //当前页
    this.current = function () {

        //取选中的图片标识
        var _active = $(active).attr('imageid');
        if (_active) { //如果有
            self.active = _active; //保存
        }

        //换图
        $(imgs).html(
			Request.formats(format, data.slice(loop * size, (loop + 1) * size))
		)
        //如果选中的在items中,则设置选中状态
		.find('img').each(function () {
		    if ($(this).attr('imageid') == self.active) {
		        $(this).addClass('active');
		        return false;
		    }
		});
        /*
        if(_mark){
        $(imgs).find('img:first').addClass('active');
        }
        */
    };

    //上传后调用此方法,插入一个img
    this.insert = function (isAdd, o) {
        //去掉原有选中的img
        $(imgs).find('img').removeClass('active');

        //插入元素

        data.splice(loop * size, isAdd ? 0 : 1, o);
        self.active = o.imageid;

        this.current();

        //$(imgs).find('img:first').addClass('active');
        /*
        if($(imgs).find('img').length < size){
        $(imgs).prepend(
        Request.format(format, o)
        ).find('img:first').addClass('active');
        }
        else {
        this.current();
        }*/

        //重新计算页码
        var _len = parseInt(data.length / size);

        if (data.length % size != 0) {
            _len++;
        }

        if (_len != len) {
            len = _len;
            $(nextBtn).show();
        }


    }

    this.getPages = function () {
        return len;
    }

    $(prevBtn).unbind("click").click(function () {
        self.prev();
    });

    $(nextBtn).unbind("click").click(function () {
        self.next();
    });

}