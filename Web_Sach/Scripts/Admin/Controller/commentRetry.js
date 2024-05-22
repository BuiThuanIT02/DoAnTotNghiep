
var retry = {
    init: function () {
        retry.registerEvents();

    },
    registerEvents: function () {
     
        $('#reTryAdmin').off("click").on("click", function () {   
            let content = $('textarea[name="txtRetryAdmin"]').val();
            if (content == "") {
                alert("Nội dung phản hồi trống!");
                return;
            }
            let parentId = $('#reTryAdmin').data('parentid');
            let maSach = $('#reTryAdmin').data('masach'); 
            $.ajax({
                url: '/Admin/Comment/RetryPost',
                type: 'POST',
                data: {
                    content,
                    parentId,
                    maSach
                },
                dataType: 'json',
                success: function (res) {
                    if (res.Status) {
                        $('textarea[name="txtRetryAdmin"]').val("");
                        alert("Phản hồi thành công!!")
                    }
                    else {
                        alert("Phản hồi thất bại");
                    }
                },
            });//end ajax
        })// end retry
    },

}
retry.init();