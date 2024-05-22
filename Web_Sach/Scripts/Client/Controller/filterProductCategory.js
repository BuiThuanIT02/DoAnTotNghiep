var filterCate = {
    init: function () {
        filterCate.registerEvents();
    },
    registerEvents: function () {
        $('.sidebar_sort input[type="checkbox"], .review_sort input[type="checkbox"]').change(function () {
            let selectedFilter = [];
            let selectedFilterReView = [];
            let cateId = $('#filter1').data('cate');
            $('.sidebar_sort input[type="checkbox"]:checked').each(function () {
                selectedFilter.push($(this).val());
            })// theo dõi input khi đc chọn giá

            $('.review_sort input[type="checkbox"]:checked').each(function () {
                selectedFilterReView.push($(this).val());
            })// theo dõi input khi đc chọn số sao(đánh giá)
            $.ajax({
                type: "GET",
                url: '/ProductCategory/_FillerProductCategory',
                data:
                {
                    cateId: cateId,
                    filterOb: JSON.stringify(selectedFilter),
                    reviewOb: JSON.stringify(selectedFilterReView)
                }
                ,

                success: function (res) {
                    $('#products-container').html(res);


                }

            })// end ajax
        })// theo dõi all input

        $('#select_Sort').change(function () {
                  
                let selectedFilter = [];
                $('.sidebar_sort input[type="checkbox"]:checked').each(function () {
                    selectedFilter.push($(this).val());
                })// theo dõi input khi đc chọn
           
            let sortChoose = $('#select_Sort').val();
            let cateId = $('#select_Sort').data('selectsort');
            $.ajax({
                type: "GET",
                url: '/ProductCategory/_FillerProductCategory',
                data:
                {
                    cateId: cateId,
                    filterOb: JSON.stringify(selectedFilter),
                    sortBy: sortChoose
                }
                ,

                success: function (res) {
                    $('#products-container').html(res);


                }

            })// end ajax
        })// theo dõi thay đổi của selection
    }
}
filterCate.init();