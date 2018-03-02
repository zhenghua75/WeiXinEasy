
function Helper(obj, msg) {

    var id = 'Helper' + (Math.random() + '').substr(2);

    var div = $('<div style="position:absolute; border:1px solid #ccc; width:200px; height:auto; padding:7px; z-index:10000; background:#fff; border-radius:4px; font-size:12px; color:#2b2b2b; word-break:break-all; word-wrap:break-word;"></div>')

	.attr('id', id)

	.html(msg)

	.append('<span style="position:absolute; top:-12px; left:-12px; width:30px; height:30px; font-size:35px; color:#ccc;">◆</span><span style="position:absolute; top:-12px; left:-10px; width:30px; height:30px; font-size:35px; color:#fff;">◆</span>');

    var mark = false;

    $(obj).click(function () {


        if (mark) {

            $('#' + id).remove();

        }
        else {

            div.css({

                'left': $(obj).offset().left + 35,

                'top': $(obj).offset().top - 3

            }).appendTo('body');

        }

        mark = !mark;

    });


    this.setMsg = function (msg) {
        div.html(msg).append('<span style="position:absolute; top:-12px; left:-12px; width:30px; height:30px; font-size:35px; color:#ccc;">◆</span><span style="position:absolute; top:-12px; left:-10px; width:30px; height:30px; font-size:35px; color:#fff;">◆</span>');
    }



    $('#rTab > li > a, .u-btnBlue').click(function () {
        $('#' + id).remove();
        mark = false;
    });


}






















