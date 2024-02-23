var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
     
        $('#txtkeyword').autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/ListName",
                    type: "GET",
                  
                    dataType: "json",
                    data: {
                        term: request.term
                    },
                    success: function (res) {
                        debugger;
                     
                        response(res.data);
                    }
                });
            }, 
            focus: function (event, ui) {
                debugger;
                $('#txtkeyword').val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                debugger;
                $('#txtkeyword').val(ui.item.label);    
                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div>" + item.label + "</div>")
                    .appendTo(ul);
            };

    }
}
common.init();