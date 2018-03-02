
var AutoComplete = {
    bind: function (inputId, sqlId, length, row) {
        $("#" + inputId).autocomplete(
            {
                minLength: length || 2,
                source: function (request, response) {
                    $.ajax({
                        url: "/auto/auto_complete.action",
                        dataType: "json",

                        data: {
                            maxRows: row || 12,
                            sqlId: sqlId,
                            term: request.term
                        },
                        success: function (data) {
                            response($.map(data.data, function (item) {
                                return {
                                    label: item.label,
                                    value: item.value
                                }
                            }));
                        }
                    });
                }
            });
    }


}