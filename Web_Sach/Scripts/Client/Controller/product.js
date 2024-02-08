var productDetail = {
    init: function () {
        productDetail.releaseEvents();
    },
    releaseEvents: function () {

        $('#addCart').off("click").on("click", function () {
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
        // bình luận
        $('#review_submit').off("click").on("click", function (e) {
            e.preventDefault();
            let MaSach = $('#maSachComment').val();
            let MaKH = $('#maUserComment').val();
            let Rate = $('#txtRate').val();
            //let FullName = $('#review_name').val();
            //let Email = $('#review_email').val();
            let Content = $('#review_message').val();
            debugger;
            if (Content == "") {
                debugger;
                alert("Nội dung bình luận trống!");
                return;
            }
            $.ajax({
                url: '/ReView/PostReView',
                type: "POST",
                data: {
                    MaSach,
                    MaKH,
                    Rate,
                    parentId: 0,
                    //FullName,
                    //Email,
                    Content,
                },
                dataType: "json",
                success: function (res) {
                    if (res.success) {
                        $('#container_comment').load('/ReView/GetComment?productId=' + MaSach);
                        /*$('#review_form')[0].reset();*/
                        Content = "";
                        alert("Đã thêm bình luận thành công !");
                    }
                    else {
                        alert("Thêm bình luận thất bại !");
                    }

                },
            });// end ajax
        })// submit
        //reply bình luận
       
        $('.abcdefghkj').off("click").on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            let MaSach = btn.data('productid');
            let MaKH = btn.data('userid');
            var parenid = btn.data('parentid');
            
           /* var commentmsg = btn.data('commentmsg');*/
            let Content = btn.data('commentmsg');
            var commentmsgvalue = document.getElementById(Content);
            if (commentmsgvalue.value == "") {

              alert("Chưa nhập nội dung bình luận");
                return;
            }
            $.ajax({
                url: '/ReView/PostReView',
                data: {
                    MaSach,
                    MaKH,
                    Rate:0,
                    parentId: parenid,       
                    Content :commentmsgvalue.value,   
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.success) {
                        commentmsgvalue.value = "";
                        alert("Đã thêm bình luận thành công !");
                        $('#container_comment').load('/ReView/GetComment?productId=' + MaSach);
                    }
                    else {
                       alert("Thêm bình luận lỗi");
                    }
                }
            });
        });


    }
}
productDetail.init();