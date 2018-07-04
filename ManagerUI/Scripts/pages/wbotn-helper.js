function Delete(id, url) {
    layer.confirm('是否删除?', function (index) {
        layer.close(index);
        location.href = url + "?id=" + id;
    });
}