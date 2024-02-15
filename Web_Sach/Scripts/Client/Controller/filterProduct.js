var filterProduct = {
    init: function () {
        filterProduct.removeEventListener();
    },
    removeEventListener: function () {
        $('.sidebar_sort input[type="checkbox"]').change(function () {
            let selectedFilter = [];
         
            $('.sidebar_sort input[type="checkbox"]:checked').each(function () {
                selectedFilter.push($(this).val());
            })// theo dõi input khi đc chọn
            $.ajax({
                type: "GET",
                url: '/ViewAll/_FillerProductViewAll',
                data:
                {
                    filterOb: JSON.stringify(selectedFilter)
                }
                ,

                success: function (res) {
                    $('#products-container-New').html(res);


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
                url: '/ViewAll/_FillerProductViewAll',
                data:
                {
                    filterOb: JSON.stringify(selectedFilter),
                    sortBy: sortChoose
                }
                ,

                success: function (res) {
                    $('#products-container-New').html(res);


                }

            })// end ajax
        })// theo dõi thay đổi của selection
    }
}
filterProduct.init();