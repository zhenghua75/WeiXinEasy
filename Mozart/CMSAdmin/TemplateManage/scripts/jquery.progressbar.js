/*!
* jQuery Progress Bar
* version: 1.0.0
* @requires jQuery v1.6 or later
* Copyright (c) 2013 Ravishanker Kusuma 
* http://hayageek.com/ 
*/
(function ($) {
    $.fn.progressbar = function (options) {
        var settings = $.extend({
            width: '300px',
            height: '25px',
            color: '#b1da98',
            padding: '0px',
            border: '1px solid #ddd'
        }, options);

        //Set css to container
        $(this).css({
            'width': settings.width,
            'border': settings.border,
            'border-radius': '5px',
            'overflow': 'hidden',
            'display': 'inline-block',
            'padding': settings.padding,
            'margin': '0px 10px 5px 5px',
            'background-color': '#e6ebef'
        });


        // add progress bar to container
        var progressbar = $("<div></div>");
        progressbar.css({
            'height': settings.height,
            'text-align': 'center',
            'vertical-align': 'middle',
            'color': '#e6ebef',
            'width': '0px',
            'border-radius': '3px',
            'background-color': settings.color
        });

        $(this).html(progressbar);

        this.progress = function (value) {
            var width = 0;
            if (value != 0) {
                width = $(this).width() * value / 100;
            }
            var html = '<div style="color:#ff0000;width: 450px;vertical-align:middle;padding-top:3px;">' + value + '%</div>';
            progressbar.width(width).html(html);
        }
        return this;
    };

}(jQuery));