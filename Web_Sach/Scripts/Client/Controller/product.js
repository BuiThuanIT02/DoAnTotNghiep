$(document).ready(function () {
    $('#addCart').on("click", function () {
        var id = $(this).data('id');
        var amountValue = ($("#amount").val());
        $.ajax({
            url: "/Cart/AddCart",
            type: "POST",
            data: {
                productId: id,
                Quantity: amountValue,
            },
            dataType: "json",
            success: function (res) {
                if (res.status) {
                    alert("Thêm sản phẩm vào giỏ hàng thành công!");
                }
                else if (!res.status && res.role == 0) {
                    window.location.href = "/them-gio-hang";
                }
            },
            error: function (res) {
                alert("Đã xảy ra lỗi trong quá trình thêm sản phẩm");
            },
        });
        //var url = "/them-gio-hang?productId=" + id + "&Quantity=" + amountValue;
        //window.location.href = url;

    });
    // lưu voucher

    $('.btnAddVoucher').each(function () {
        $(this).off("click").on("click", function () {
            let id = $(this).data('id');
            let $btn = $(this);
            $.ajax({
                url: "/Vouchers/AddVoucher",
                type: "POST",
                data: {
                    voucherId: id
                },
                dataType: "json",
                success: function (res) {
                    if (res.status) {
                        $btn.addClass('disabled');
                        alert("Lưu voucher thành công!");
                    }
                    else if (!res.status && res.voucherNotEmpty == 1) {
                        alert("Voucher này đã tồn tại!");
                    }

                    else if (!res.status && res.role == 0) {
                        window.location.href = "/add-voucher";
                    }
                },
                error: function (res) {
                    alert("Đã xảy ra lỗi trong quá trình lưu voucher");
                },
            });
        })

        //var url = "/add-voucher?voucherId=" + id + "&detailIds=" + prId;
        ///* $('.btnCheckOut').attr("disabled", "disabled");*/
        //window.location.href = url;

    });


    $('#buyNow').on("click", function () {
        var id = $(this).data('id');
        var amountValue = ($("#amount").val());

        var url = "/mua-ngay?id=" + id + "&Quantity=" + amountValue;
        window.location.href = url;

    });

    $('#amount').on("blur", function () {
        var amountValue = $('#amount').val();// giá trị input
        var max = $('#amount').attr("max");
        if (isNaN(amountValue) || amountValue == 0) {
            $('#amount').val(1);

        }
        else if (parseInt(amountValue) > parseInt(max)) {
            alert("Số lượng vượt quá " + max);
            $('#amount').val(1);

        }
    });// sự kiện khi nhập giá trị vào input nhấp chuột ra

    $('.plus-btn').on("click", function () {
        // lấy giá trị input
        var amountValue = parseInt($('#amount').val());// giá trị input
        var max = $('#amount').attr("max");
        if (parseInt(amountValue) < parseInt(max)) {
            amountValue++;
            $('#amount').val(amountValue);// val(1): hiện 1 chứ không phải nó tăng

        }
        else {
            alert("Số lượng vượt quá " + max);
        }


    }); // sk click tăng

    $('.minus-btn').on("click", function () {
        var amountValue = parseInt($('#amount').val())// giá trị input

        if (parseInt(amountValue) > 1) {

            amountValue--;
            $('#amount').val(amountValue);
        }

    })// sk giảm

    $('#buyBook').on("click", function () {
        var id = $(this).data('id');
        var amountValue = ($("#amount").val());

        var url = "/thanh-toan?id=" + id + "&Quantity=" + amountValue;
        window.location.href = url;

    });

});