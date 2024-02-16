var order = {
    init: function () {
        order.releaseEvents();
    },
    releaseEvents: function () {
        $('.moveOrder').off("click").on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('order-id');
            if (confirm("Bạn có muốn hủy đơn hàng này không!!")) {
                $.ajax({
                    url: '/Admin/Order/RemoveOrder',
                    data: { id: id },
                    type: "POST",
                    dataType: "json",
                    success: function (res) {
                        if (res.status) {
                            $('#row_' + id).remove();
                            alert("Hủy đơn hàng thành công");
                        }
                        else {
                            alert("Hủy đơn hàng thất bại");
                        }
                    }
                })
            }

        }) // end
    }
}
order.init();