var fillter = {
    init: function () {
        fillter.removeEventListener();
    },
    removeEventListener: function () {
        $('sidebar_sort input[type="checkbox"]').change(function () {
            let selectedFilter = [];
            debugger;
            $('sidebar_sort input[type="checkbox"]:checked').each(function () {
                selectedFilter.push($(this).val());
            })// theo dõi input khi đc chọn
            $.ajax({
                type: "POST",
                url: 'ProductCategory/ProductCategory',
                data: {
                    filterOb: selectedFilter
                },
                success: function (res) {
                    $('#products-container').html(res);
                }

            })// end ajax
        })// theo dõi all input

    }
}
fillter.init();