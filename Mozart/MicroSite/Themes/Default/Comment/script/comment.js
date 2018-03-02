$().ready(function () {
    form_emotion.rend();
    myInput.maxLength = 500
    $("#fontcount").html(myInput.maxLength + " 字");
});
function switchPage(thi, index) {
    $("#nav_page li").removeClass("on").eq(index).addClass("on");
}
var form_emotion = (function () {
    var fe = function () {
        this.values = ["/::)", "/::~", "/::B", "/::|", "/:8-)", "/::<", "/::$", "/::X", "/::Z",
            "/::'(", "/::-|", "/::@", "/::P", "/::D", "/::O", "/::(", "/::+", "/:–b", "/::Q",
            "/::T", "/:,@P", "/:,@-D", "/::d", "/:,@o", "/::g", "/:|-)", "/::!", "/::L", "/::>",
            "/::,@", "/:,@f", "/::-S", "/:?", "/:,@x", "/:,@@", "/::8", "/:,@!", "/:!!!", "/:xx",
            "/:bye", "/:wipe", "/:dig", "/:handclap", "/:&-(", "/:B-)", "/:<@", "/:@>", "/::-O",
            "/:>-|", "/:P-(", "/::'|", "/:X-)", "/::*", "/:@x", "/:8*", "/:pd", "/:<W>", "/:beer",
            "/:basketb", "/:oo", "/:coffee", "/:eat", "/:pig", "/:rose", "/:fade", "/:showlove",
            "/:heart", "/:break", "/:cake", "/:li", "/:bome", "/:kn", "/:footb", "/:ladybug",
            "/:shit", "/:moon", "/:sun", "/:gift", "/:hug", "/:strong", "/:weak", "/:share",
            "/:v", "/:@)", "/:jj", "/:@@", "/:bad", "/:lvu", "/:no", "/:ok", "/:love", "/:<L>",
            "/:jump", "/:shake", "/:<O>", "/:circle", "/:kotow", "/:turn", "/:skip", "[挥手]",
            "/:#-0", "[街舞]", "/:kiss", "/:<&", "/:&>"].slice(0, -7);
        this.spearate = 20
    }
    fe.prototype = {
        rend: function () {
            var that = this;
            var TPL = '{seprateDiv}<dd><span data-key="{k}_{page}_{v}" style="background-position:{xPos}px 0;">' +
                '</span></dd>{delHTML}';
            var res = iTemplate.makeList(TPL, that.values, function (k, v) {
                return {
                    k: k,
                    v: v,
                    page: Math.floor(k / that.spearate),
                    xPos: -24 * k,
                    seprateDiv: (0 == k % that.spearate && 0 != k && k != that.values.length) ? "</div><div>" : "",
                    delHTML: (19 == k % that.spearate || k == (that.values.length - 1)) ? '<dd>' +
                        '<span data-key="-1_-1_/:del" class="del"></span></dd>' : ''
                }
            });
            $("#list_emotion").html('<div>' + res + '</div>');
            var nav_span = new Array(Math.ceil(that.values.length / that.spearate));
            $("#nav_emotion").html('<span class="on">' + nav_span.join("</span><span>") + '</span>');
            that.bind();
            window.swiper = new Swipe(document.getElementById('page_emotion'), {
                speed: 500,
                callback: function () {
                    $("#nav_emotion span").removeClass("on").eq(this.index).addClass("on");
                }
            });
            return that;
        },
        bind: function () {
            $("#list_emotion").on("click", function (evt) {
                if ("SPAN" == evt.target.tagName) {
                    var val = evt.target.getAttribute("data-key").split('_');
                    myInput.listen(this, {
                        keyCode: -10,
                        srcElement: document.getElementById("topicdesc"),
                        value: val[2],
                        imgUrl: 'emotion/' + val[0] + ".gif"
                    });
                    this.focus();
                }
            });
        }
    }
    return new fe();
})();