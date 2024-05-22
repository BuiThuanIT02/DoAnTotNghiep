var getOrder = {
    init: function () {
        getOrder.registerEvents();
    },
    registerEvents: function () {
        $('.getOrer').off("click").on("click", function () {
            let orderId = $(this).data('orderid');
            $.ajax({
                url: '/Order/GetOrder',
                type: "POST",
                data: { id: orderId },
                dataType: "json",
                success: function (res) {
                    if (res.status) {
                        alert("Đơn hàng đã giao thành công!");
                        $('#orderRow_' + orderId).remove();
                    }
                    else {

                        alert("Đơn hàng đã giao thất bại!");
                    }
                }


            });// end ajax
        });// end click
    }
}
getOrder.init();