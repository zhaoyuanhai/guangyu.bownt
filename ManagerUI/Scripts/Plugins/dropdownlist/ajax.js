var dropdownloadajax = 1;
var Ajax = function () {
    function request(url, opt) {
        function fn() { }
        var async = opt.async !== false,
            method = opt.method || 'GET',
            type = opt.type || 'text',
            encode = opt.encode || 'UTF-8',
            data = opt.data || null,
            success = opt.success || fn,
            failure = opt.failure || fn;
        method = method.toUpperCase();
        if (data && typeof data == 'object') {//对象转换成字符串键值对  
            data = _serialize(data);
        }
        if (method == 'GET' && data) {
            url += (url.indexOf('?') == -1 ? '?' : '&') + data;
            data = null;
        }

        var xhr = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
        xhr.onreadystatechange = function () {
            _onStateChange(xhr, type, success, failure);
        };
        xhr.open(method, url, async);
        if (method == 'POST') {
            xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded;charset=' + encode);
        }
        xhr.send(data);
        return xhr;
    }

    function _serialize(obj) {
        var a = [];
        for (var k in obj) {
            var val = obj[k];
            if (val.constructor == Array) {
                for (var i = 0, len = val.length; i < len; i++) {
                    a.push(k + '=' + encodeURIComponent(val[i]));
                }
            } else {
                a.push(k + '=' + encodeURIComponent(val));
            }
        }
        return a.join('&');
    }

    function _onStateChange(xhr, type, success, failure) {
        if (xhr.readyState == 4) {
            var s = xhr.status, result;
            if (s >= 200 && s < 300) {
                switch (type) {
                    case 'text':
                        result = xhr.responseText;
                        break;
                    case 'json':
                        result = function (str) {
                            return (new Function('return ' + str))();
                        } (xhr.responseText);
                        break;
                    case 'xml':
                        result = xhr.responseXML;
                        break;
                }
                success(result);
            } else {
                failure(xhr);
            }
        } else { }
    }
    return (function () {
        var Ajax = { request: request }, types = ['text', 'json', 'xml'];
        for (var i = 0, len = types.length; i < len; i++) {
            Ajax[types[i]] = function (i) {
                return function (url, opt) {
                    opt = opt || {};
                    opt.type = types[i];
                    return request(url, opt);
                };
            } (i);
        }
        return Ajax;
    })();
} ();