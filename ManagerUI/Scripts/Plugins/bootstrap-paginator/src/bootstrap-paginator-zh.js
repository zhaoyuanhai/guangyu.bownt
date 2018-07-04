$.fn.bootstrapPaginator.defaults.itemTexts = function (type, page, current) {
    switch (type) {
        case "first":
            return "首页";
        case "prev":
            return "上一页";
        case "next":
            return "下一页";
        case "last":
            return "尾页";
        case "page":
            return page;
    }
    return '';
}