var filterProduct = {
    init: function () {
        filterProduct.registerEvents();
    },
    registerEvents: function () {
        $('.sidebar_sort input[type="checkbox"]').change(function () {
            let selectedFilter = [];

            $('.sidebar_sort input[type="checkbox"]:checked').each(function () {
                selectedFilter.push($(this).val());
            })// theo dõi input khi đc chọn
            $.ajax({
                type: "GET",
                url: '/ViewAll/_FillerProductTopHot',
                data:
                {
                    filterOb: JSON.stringify(selectedFilter)
                }
                ,

                success: function (res) {
                    $('#product-tophot').html(res);


                }

            })// end ajax
        })// theo dõi all input

        $('#select_Sort').change(function () {

            let selectedFilter = [];
            $('.sidebar_sort input[type="checkbox"]:checked').each(function () {
                selectedFilter.push($(this).val());
            })// theo dõi input khi đc chọn

            let sortChoose = $('#select_Sort').val();

            $.ajax({
                type: "GET",
                url: '/ViewAll/_FillerProductTopHot',
                data:
                {
                    filterOb: JSON.stringify(selectedFilter),
                    sortBy: sortChoose
                }
                ,

                success: function (res) {
                    $('#product-tophot').html(res);


                }

            })// end ajax
        })// theo dõi thay đổi của selection
    }
}
filterProduct.init();