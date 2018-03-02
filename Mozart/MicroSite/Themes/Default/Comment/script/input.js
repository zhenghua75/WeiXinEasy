var myInput = (function () {
    var mi = function () {
        this.maxLength = 500,
		this.currentLength = 0
    }
    mi.prototype = {
        listen: function (thi, evt) {
            var that = this;
            if ("/:del" == evt.value) {
                thi = evt.srcElement;
                var imgs = thi.querySelectorAll("img");
                if (imgs.length) {
                    imgs[imgs.length - 1].remove();
                }
                return;
            }
            if ("paste" == evt.type) {
                var text = evt.clipboardData.getData('text/plain');
                if (text.length > (that.maxLength - that.currentLength)) {
                    evt.preventDefault();
                    evt.stopPropagation();
                    return false;
                }
            }
            if (evt.keyCode && evt.keyCode > 0 && that.currentLength >= that.maxLength) {
                if (evt.keyCode == 8 || evt.keyCode == 46) {

                } else {
                    evt.preventDefault();
                    evt.stopPropagation();
                    return false;
                }
            }
            if (evt.keyCode && -10 == evt.keyCode) {
                if (evt.value.length > (that.maxLength - that.currentLength)) {
                    return that;
                }
                thi = evt.srcElement;
                var img = new Image();
                img.src = evt.imgUrl;
                img.innerHTML = evt.value;
                img.setAttribute("data-emotion", evt.value);
                thi.appendChild(img);
            }
            var imgs = thi.querySelectorAll("img");
            var em_count = 0;
            for (var i = 0, ci; ci = imgs[i]; i++) {
                em_count += ci.getAttribute("data-emotion").length;
            }
            var fc = $("#fontcount");
            that.currentLength = thi.innerText.length + em_count;
            if (that.maxLength < that.currentLength) {
                thi.innerHTML = thi.innerHTML.slice(0, that.maxLength);
                that.currentLength = that.maxLength;
            }
            fc.html(that.maxLength - that.currentLength + " 字");
        },
        active: function (thi, evt) {
            return;
            var that = this;
            that.curPos = getCaretCharacterOffsetWithin(thi);
            console.log(that.curPos);
            return that;
        }
    }
    return new mi();
})();