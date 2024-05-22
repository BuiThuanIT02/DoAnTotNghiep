


var product = {
    init: function () {
        product.registerEvents();

    },
    registerEvents: function () {
        // sk trangj thai
        $('.btn-active').off('click').on("click", function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: '/Admin/Product/ChangeStatus',
                data: { id: id },
                dataType: 'json',
                type: 'POST',
                success: function (res) {

                    if (res.status == true) {
                        btn.text("Kích hoạt");
                        btn.addClass('btn-start-text text-success');
                        btn.removeClass('btn-block-text text-danger')
                    }

                    else {
                        btn.text("Khóa");
                        btn.addClass('btn-block-text text-danger');
                        btn.removeClass('btn-start-text text-success');
                    }
                }
            });


        });


        //end su kien trang thai

        //sk quản lý nhiều ảnh
        $('.btn-images').off('click').on("click", function (e)  { // kích nút quản lý ảnh
            e.preventDefault();
            $('#imagesManage').modal('show');
            $('#hidProductID').val($(this).data('id')); // input trong hộp modal
            product.loadImages();
        }); // hiện hộp modal
        // sk chọn ảnh từ hộp modal
        $('.btnChooImages').off('click').on('click', function (e) {// jquery
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {// function này là call back của selectActionFunction
                $('.imageList').append('<div style="float:left;"><img src=" ' + url + ' "  width="100" /><a href="#" class="btn-delImage"><i class="fa fa-times"></i></a>         </div>');

                $('.btn-delImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                })
            };
            finder.popup(); // hiện thỉ cửa sổ lấy ảnh
        })

        // save
        $('.btnSaveImages').off('click').on('click', function () {
            var images = [];
            var id = $('#hidProductID').val();
            $.each($('.imageList img'), function (i, item) {
                images.push($(item).prop('src'));
            })

            $.ajax({
                url: '/Admin/Product/SaveImages',
                type: 'POST',
                data: {
                    id:id,
                    images: JSON.stringify(images)
                },
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        $('#imagesManage').modal('hide');
                        $('.imageList').html('');
                        alert("Save thành công!!");
                    }
                    else {
                        alert("Thất bại!!");
                    }
                   
                }
            });







        })





    },
    loadImages: function () {
        $.ajax({
            url: '/Admin/Product/LoadImages',
            type: 'GET',
            data: {
                id: $('#hidProductID').val()
               
            },
            dataType: 'json',
            success: function (res) {
               
                    var data = res.data;
                    var html = '';
                    $.each(data, function (i, item) {
                        html += '<div style="float:left;"><img src=" ' + item + ' "  width="100" /><a href="#" class="btn-delImage"><i class="fa fa-times"></i></a>         </div>'
                    });
                $('.imageList').html(html);
                $('.btn-delImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                })
                product.registerEvents();

            }
        });
    }
}
product.init();