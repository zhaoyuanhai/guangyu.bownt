$(function () {
    $(".item-list .btns button").click(function () {
        var id = $(this).data("id");
        if (id == 0) {
            $(".item-list .types").show();
        } else {
            $(".item-list .types").hide();
            $("#type_" + id).show();
        }

        $(".item-list .btns button").removeClass("active");
        $(this).addClass("active");
    });
});