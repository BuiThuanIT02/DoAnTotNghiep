
var orderAd = {
    init: function () {
        orderAd.registerEvents();
    },
    registerEvents: function () {
        //pending order
        $('.orderPending').off("click").on("click", function () {
            let orderId = $(this).data('id');
            $.ajax({
                url: '/Admin/Order/ChangeStatusPending',
                type: "POST",
                data: { id: orderId },
                dataType: "json",
               
                success: function (res) {
                    if (res.status) {
                        alert("Duyệt đơn hàng thành công!");
                        $("#row_" + orderId).remove();
                    }
                    else {
                        alert("Duyệt đơn hàng thất bại!");
                    }
                }


            });// end ajax
        });
        //pending order
        //pack order
        $('.orderPackAd').off("click").on("click", function () {
            let orderId = $(this).data('id');
            $.ajax({
                url: '/Admin/Order/ChangeStatusPack',
                type: "POST",
                data: { id: orderId },
                dataType: "json",

                success: function (res) {
                    if (res.status) {
                        alert("Đóng gói đơn hàng thành công!");
                        $("#row_" + orderId).remove();
                    }
                    else {
                        alert("Đóng gói đơn hàng thất bại!");
                    }
                }


            });// end ajax
        });
        //pack order
    }
}
orderAd.init();