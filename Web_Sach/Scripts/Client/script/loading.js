$(document).ready(function () {
$(function () {
    $(".submitbtn").click(function () {
        $("#preloader").show();
    });
    $(window).on('pageshow', function () {
        $('#preloader').hide();
    });//mỗi khi trang hiển thị thì ẩn loading
});

})