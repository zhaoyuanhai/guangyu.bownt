$.fn.menuList = function (options) {
    $(this).click(function () {
        if (typeof options.click === "function")
            options.click.call(this);
        var self = this;
        $(".menu").removeClass("menu-active");
        $(this).addClass("menu-active");
        $.getJSON("/Home/GetProdectByNormId", { id: $(this).data("id") }, function (data) {
            var html = "";
            for (var i = 0; i < data.Data.length; i++) {
                var template = $("#list_temp").html()
                    .replace("{Id}",data.Data[i].Id)
                    .replace("{Title}", data.Data[i].Title)
                    .replace("{Icon}", data.Data[i].Icon);
                html += template;
            }
            html += '<div style="clear:left;"></div>';
            $("#lb").html(html);
            options.ondataed.call(self, data);
        })
    });
}