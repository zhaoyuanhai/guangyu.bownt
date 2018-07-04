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


var AutoComplite = function (el, option) {
    /// <summary>文本框的扩展，使文本框能弹出与输入内容相似的信息列表</summary>
    /// <param name="el" type="Object">文本框对像（不能是jquery加过$符号的变量）</param>
    /// <param name="option" type="Object">参数列表，具体参数：{
    /// <para name="panel" type="Object">panel: 列表的父容器（默认为Body）</para>
    /// <para>elId:列表的ID（默认为文本框ID）</para>
    /// <para>url: 列表生成文件（ajax后台处理文件，为空列表将不处理并不显示）</para>
    /// <para>method: Ajax提交方式，分两种（GET | POST）</para>
    /// <para>xmlmaintag: 返回的xml的主标签（默认为data,例如：<data></data>)</para>
    /// <para>xmlchildtag: 返回的xml的实际节点标签（默认为node,例如：<node></node>)</para>
    /// <para>width: 信息列表宽度（默认为文本框宽度）</para>
    /// <para>field: 对象类型（默认为{}）返回的字段类型列表，字段名称与xmlchildtag节点中的属性一致，字值为页面中的控件ID（例如：{value:valueObjId}）</para>
    /// <para>args: Ajax附加参数</para>
    /// <para>onfocus: 当信息列表得到焦点时要执行的事件</para>
    /// <para>onchange: 当信息列表选择时执行的事件</para>
    ///</param>
    this.SelectedIndex = -1;
    var me = this;
    var userAgent = navigator.userAgent.toLowerCase();
    var is_webtv = userAgent.indexOf('webtv') != -1;
    var is_kon = userAgent.indexOf('konqueror') != -1;
    var is_mac = userAgent.indexOf('mac') != -1;
    var is_saf = userAgent.indexOf('applewebkit') != -1 || navigator.vendor == 'Apple Computer, Inc.';
    var is_opera = userAgent.indexOf('opera') != -1 && opera.version();
    var is_moz = (navigator.product == 'Gecko' && !is_saf) && userAgent.substr(userAgent.indexOf('firefox') + 8, 3);
    var is_ns = userAgent.indexOf('compatible') == -1 && userAgent.indexOf('mozilla') != -1 && !is_opera && !is_webtv && !is_saf;
    var is_ie = (userAgent.indexOf('msie') != -1 && !is_ie6 && !is_opera && !is_saf && !is_webtv) && userAgent.substr(userAgent.indexOf('msie') + 5, 3);
    var is_ie6 = false;
    if (is_ie) {
        var re = /msie\s(\d{1,})\.\d{1,}/ig;
        var arr = re.exec(userAgent);
        is_ie6 = RegExp.$1 == 6;
    }
    this.isIE = is_ie;
    this.isIE6 = is_ie6;
    this.isOpera = is_opera;
    this.enabled = true;

    if (!el) { return; }

    this.AutoComplite_Source = window.AutoComplite_Source;
    el.setAttribute("autocomplete", "off"); //设置输入框的自动完成功能为关闭状态
    el.setAttribute("isautocomplete", true); //设置输入框的自动完成功能为关闭状态

    if (el.type == "text") {
        addEvent(el, "blur", function () {
            if (this.getAttribute("cleartext_dropdownlist") == "true" && me.enabled) {
                this.value = "";
            }
        });
    }


    // 参数对象 //
    this.oP = option ? option : {};
    
    // 参数默认值
    //
    var k, def = { panel: document.body, elId: undefined, url: "", notfound: "", method: "GET", xmlmaintag: "data", xmlchildtag: "node", width: "", field: {},cleartext:true, args: {}, onfocus: 0, onchange: function (ev, a) { }, onload: function (ev, ul) { } };
    for (k in def) {
        if (this.oP[k] == undefined)
            this.oP[k] = def[k];
    }

    if (!this.oP.elId) {
        this.oP.elId = el.id;
    }

    if (this.oP.panel) {
        addEvent(this.oP.panel, "scroll", function (e) {

        });
    }

    el.elId = this.oP.elId;

    this.oP.el = el;
    this.oP.me = me;

    if (!this.oP.field.text) {
        this.oP.field.text = el.id;
    }

    if (!this.oP.field.value) {
        this.oP.field.value = el.id + "_Sel_value";
    }

    if (el.className != "") {
        if (el.className.indexOf("AutoSelect") == -1) {
            el.className += " AutoSelect";
        }
    }
    else {
        el.className = "AutoSelect";
    }

    ///此类公用
    this.ge = function (id) {
        return document.getElementById(id);
    };

    this.getPos = function (el, sProp) {
        var iPos = 0;
        while (el != null) {
            iPos += el["offset" + sProp];

            el = el.offsetParent;
            if (el) {
                if (sProp == "Top") {
                    if (el.scrollTop > 0) {
                        iPos -= el.scrollTop;
                    }
                }
                else {
                    if (el.scrollLeft > 0) {
                        iPos -= el.scrollTop;
                    }
                }
            }

        }
        return iPos;
    };


    this.ResetPanelOffset = function (div) {
        if (this.oP.panel) {
            div.style.left = this.getPos(el, "Left") - this.getPos(this.oP.panel, "Left") * 1 + this.oP.panel.scrollLeft * 1 + "px";
            div.style.top = this.getPos(el, "Top") * 1 - this.getPos(this.oP.panel, "Top") * 1 + this.oP.panel.scrollTop * 1 + el.offsetHeight * 1 + "px"; // (this.getPos(el, "Top") * 1 +.scrollTop * 1 + el.offsetHeight) + "px";
        }
        else {
            div.style.left = this.getPos(el, "Left") + document.body.scrollLeft * 1 + "px";
            div.style.top = (this.getPos(el, "Top") + el.offsetHeight) + "px";
        }

        v = div.style.left;
        if ($(div).position().left + $(div).width() > $(this.oP.panel).width()) {
            div.style.left = ($(div).position().left - $(div).width() + $(el).width() + 5) + "px";
        }

        if ($(div).position().left < 0) { div.style.left = v; }
    }

    this.CreatePanel = function (id) {
        var div = document.createElement("DIV");
        div.id = this.oP.elId + "_AC_Panel";
        div.className = "AC_Panel"

        div.style.width = this.oP.width == "" ? (this.isIE ? el.offsetWidth : el.offsetWidth - 2) + "px" : parseInt(this.oP.width) + "px";

        var divi = document.createElement("DIV");
        divi.id = this.oP.elId + "_AC_Panel_Inner";
        divi.className = "AC_Inner"


        div.appendChild(divi);

        if (this.oP.panel) {
            this.oP.panel.appendChild(div);
        }
        else {
            document.body.appendChild(div);
        }
        me.ResetPanelOffset(div);
    }

    this.ReadData = function (obj) {
        if (this.oP.xmlmaintag == "" || this.oP.xmlchildtag == "") { return; }

        var data = obj.getElementsByTagName(this.oP.xmlmaintag);
        if (data.length < 1) { return; }
        var ul = document.createElement("UL");
        ul.rel = el.id;
        me.SelectedIndex = -1;
        if (this.oP.title != undefined) {
            var liTitle = document.createElement("LI");
            liTitle.innerHTML = "<span class=\"rednote\">" + this.oP.title + "</span>";
            ul.appendChild(liTitle);
        }
        for (var d = 0; d < 1; d++) {

            //如果查询数据记录为0，则显示指定的记录为空的显示内容，首先判断返回的xml中有没有<notfound text="" text_info="" />节点
            //  如果有此节点，则显示text中的内容，title提示内容自动引用text + text_info中的内容
            //  如果没有此节点将判断在创建此下拉框对像时是否传入了notfound这个属性值，
            //  如果传过来了将显示notfound的值
            //  如果没有传过来，将不会显示数据为空的提示信息 ----------------------------------
            if (data[d].getElementsByTagName(this.oP.xmlchildtag).length < 1) {
                var liNotfoundHtml = "";
                if (data[d].getElementsByTagName("notfound").length > 0) {
                    var node = data[d].getElementsByTagName("notfound")[0];
                    liNotfoundHtml = "<span class=\"note\" style=\"line-height:30px;\" title=\"" + node.getAttribute("text") + " " + node.getAttribute("text_info") + "\">" + node.getAttribute("text") + "</span>";
                }
                else {
                    liNotfoundHtml = "<span class=\"note\" style=\"line-height:30px;\">" + this.oP.notfound + "</span>";
                }
                if (liNotfoundHtml != "") {
                    var liNotfound = document.createElement("LI");
                    liNotfound.innerHTML = liNotfoundHtml;
                    ul.appendChild(liNotfound);
                }

            }
            // --------------------------------------------------------------------------------

            for (var i = 0; i < data[d].getElementsByTagName(this.oP.xmlchildtag).length; i++) {
                var node = data[d].getElementsByTagName(this.oP.xmlchildtag)[i];
                var li = document.createElement("LI");
                li.rel = el.id;

                if (i == 0 && el.value != node.getAttribute("text")) {
                    this.Clear();
                }

                var A = document.createElement("A");
                A.rel = el.id;
                A.onmouseover = function (e) { me.move(e, 0, this); };
                A.setAttribute("selIndex", i);
                if (me.isIE6) {
                    try {
                        A.style.width = ((me.isIE ? el.offsetWidth : el.offsetWidth - 2) - 12) + "px";
                    } catch (e) {
                    }
                }

                for (var k in this.oP.field) {
                    A.setAttribute("v_" + k, node.getAttribute(k));
                }

                A.setAttribute("v_text", node.getAttribute("text_context") ? node.getAttribute("text_context") : node.getAttribute("text"));

                var aInnerHTML = "<nobr rel='" + el.id + "'>" + (el.value!="" ? node.getAttribute("text").replace(eval("/(" + formatRegExp(el.value) + ")/ig"), "<b rel=\"" + el.id + "\">$1</b>") : node.getAttribute("text"));
                A.title = node.getAttribute("text");
                if (node.getAttribute("text_info") != null && node.getAttribute("text_info") != "") {
                    aInnerHTML += "&nbsp;<span class='text_info' rel='";
                    aInnerHTML += el.id;
                    aInnerHTML += "'>";
                    aInnerHTML += node.getAttribute("text_info");
                    aInnerHTML += "</span>";
                    A.title = node.getAttribute("text") + " " + node.getAttribute("text_info");
                }
                aInnerHTML += "</nobr>"
                A.innerHTML = aInnerHTML;
                //A.innerHTML = "<nobr rel='" + el.id + "'>" + node.getAttribute("text").replace(eval("/" + formatRegExp(el.value) + "/g"), "<b rel=\"" + el.id + "\">" + el.value + "</b>") + "</nobr>";
                A.href = "javascript:void(1);";
                A.title = node.getAttribute("text_title") ? node.getAttribute("text_title") : node.getAttribute("text_info") ? node.getAttribute("text_info") : node.getAttribute("text");
                A.onclick = function (e) {
                    me.SelOK(e);
                };
                li.appendChild(A);
                ul.appendChild(li);
            }
        }
        var div = this.ge(this.oP.elId + "_AC_Panel");
        if (ul.childNodes.length > 0) {
            try {
                if (this.ge(this.oP.elId + "_AC_Panel")) {
                    this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "";
                    this.ge(this.oP.elId + "_AC_Panel_Inner").appendChild(ul);
                    div = this.ge(this.oP.elId + "_AC_Panel");

                    me.ResetPanelOffset(div);
                    div.style.display = "";
                }
                else {
                    this.CreatePanel(el.id);
                    this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "";
                    this.ge(this.oP.elId + "_AC_Panel_Inner").appendChild(ul);
                }
            } catch (e) { }

        }
        else {
            if (this.ge(this.oP.elId + "_AC_Panel")) {
                this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "";
                div = this.ge(this.oP.elId + "_AC_Panel");

                me.ResetPanelOffset(div);
                div.style.display = "";
            }
            else {
                this.CreatePanel(el.id);
                this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "";
            }
        }


        this.oP.onload(this.oP.el, ul);
        window.AutoComplite_Source = el;


    }

    this.Failure = function (obj) {
        this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "<span class='failure'>出现异常，结果未找到 [" + obj.status + "]</span>";
        var div = this.ge(this.oP.elId + "_AC_Panel");

        me.ResetPanelOffset(div);
        div.style.display = "";
    }

    this.SendData = function (e) {
        try {
            if (this.oP.cleartext) el.setAttribute("cleartext_dropdownlist", "true");
            for (var k in this.oP.field) {
                var o = this.ge(this.oP.field[k]);
                if (o) {
                    if (o.id == el.id) { continue; }
                    if (o.value != undefined) {
                        o.value = o.initval ? o.initval : "";
                    }
                    else {
                        o.innerHTML = o.initval ? o.initval : "";
                    }
                }
            }

            var d = new Date();
            var urlStr = this.oP.url;
            if (urlStr.indexOf("?") >= 0) { urlStr += "&tmprandtime=" + d.valueOf(); } else { urlStr += "?tmprandtime=" + d.valueOf(); }
            if (Ajax) {
                //document.write(this.oP.url.replace(/\$1/ig, encodeURIComponent(el.value)));
                var argStr = "";
                for (var arg in this.oP.args) {
                    argStr += "&"
                    argStr += arg;
                    argStr += "=";
                    argStr += this.oP.args[arg];
                }
                if (argStr != "") {
                    urlStr += argStr;
                }
                var v = el.value == el.getAttribute("placeholder") ? "" : el.value;
                Ajax.xml(urlStr.replace(/\$1/ig, encodeURIComponent(v)), { success: function (obj) { me.ReadData(obj); }, failure: function (obj) { me.Failure(obj); } });
            }
        } catch (e) {
            alert("显示列表出错，错误信息：" + e.message);
        }
    }

    this.ShowPanel = function (e) {
        if (!this.enabled) { return false; }
        dorpdownlist_doup(e);
        if (this.oP.url != "") {
            if (el.value != el.getAttribute("upval") || el.readOnly || this.oP.showall) {
                el.setAttribute("upval", el.value);
                try {
                    if (this.ge(this.oP.elId + "_AC_Panel")) {
                        this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "<span class='loading'>查找中...</span>";
                        var div = this.ge(this.oP.elId + "_AC_Panel");

                        me.ResetPanelOffset(div);
                        div.style.display = "";
                    }
                    else {
                        this.CreatePanel(el.id);
                        this.ge(this.oP.elId + "_AC_Panel_Inner").innerHTML = "<span class='loading'>查找中...</span>";
                    }
                } catch (e) { }
                this.SendData(e);
            }
        }
    };

    this.move = function (e, index, o) {

        if (!this.enabled) { return false; }
        var div = this.ge(this.oP.elId + "_AC_Panel_Inner");

        var tempIndex = me.SelectedIndex + index;
        if (o) {
            tempIndex = o.getAttribute("selIndex") * 1 + index;
        }

        if (tempIndex >= 0 && tempIndex < div.getElementsByTagName("A").length) {
            var A = null
            if (me.SelectedIndex >= 0 && me.SelectedIndex < div.getElementsByTagName("A").length) {
                A = div.getElementsByTagName("A")[me.SelectedIndex];
                A.className = "";
            }

            A = div.getElementsByTagName("A")[tempIndex];
            A.className = "sel";
            me.SelectedIndex = tempIndex;
        }

    }

    this.SelOK = function (e) {

        if (!this.enabled) { return false; }
        var div = this.ge(this.oP.elId + "_AC_Panel_Inner");
        var A = div.getElementsByTagName("A")[me.SelectedIndex];
        if (A) {
            for (var k in this.oP.field) {
                var Bsplit = false;
                if (this.oP.field[k].indexOf("$") >= 0) {                    
                    var fieldArray = this.oP.field[k].split("$");
                    if (fieldArray.length > 1) {
                        Bsplit = true;
                        var o = this.ge(fieldArray[0]);
                        if (o) {
                            o.setAttribute(fieldArray[1], A.getAttribute("v_" + k));
                        }
                    }

                }
                if (!Bsplit){
                    var o = this.ge(this.oP.field[k]);
                    if (o) {
                        if (o.value != undefined) {
                            o.value = A.getAttribute("v_" + k);
                        }
                        else {
                            o.innerHTML = A.getAttribute("v_" + k);
                        }
                    }
                }
            
            }
            try {
                if (this.oP.cleartext) el.setAttribute("cleartext_dropdownlist", "false");
                if (el.type == "text") { el.value = A.getAttribute("v_text"); }
            }
            catch (e) { }
        }
        this.Hidden(e);
        this.oP.onchange(this.oP.el, A);
    }

    this.Hidden = function () {
        if (window.AutoComplite_Source) {
            var panel = document.getElementById(window.AutoComplite_Source.elId + "_AC_Panel");
            if (panel) {
                panel.style.display = "none";
            }
            window.AutoComplite_Source = null;
        }
    }

    this.Clear = function (e) {
        for (var k in this.oP.field) {
            if (k != "text") {
                var o = this.ge(this.oP.field[k]);
                if (o) {
                    if (o.value != undefined) {
                        o.value = o.getAttribute("initval") != null ? o.getAttribute("initval") : "";
                    }
                    else {
                        o.innerHTML = o.getAttribute("initval") != null ? o.getAttribute("initval") : "";
                    }
                }
            }
        }
    }




    if (this.isOpera) {
        this.Interval = null;
        addEvent(el, "focus", function (e) {

            if (me.Interval != null) {
                clearTimeout(me.Interval)
                me.Interval = null;
            }
            this.setAttribute("upval", "");
            me.Interval = setInterval(function (e) { me.ShowPanel(e); }, 100)
        });
        //el.onfocus = function (e) {

        //    if (me.Interval != null) {
        //        clearTimeout(me.Interval)
        //        me.Interval = null;
        //    }
        //    this.setAttribute("upval", "");
        //    me.Interval = setInterval(function (e) { me.ShowPanel(e); }, 100)
        //};
        addEvent(el, "blur", function (e) {
            try {
                if (me.Interval != null) {
                    clearInterval(me.Interval)
                    me.Interval = null;
                }
                me.Interval = window.setTimeout(function (e) { me.Hidden(e); }, 500);
            } catch (e) { alert(e); }
        });
        //el.onblur = function (e) {
        //    try {
        //        if (me.Interval != null) {
        //            clearInterval(me.Interval)
        //            me.Interval = null;
        //        }
        //        me.Interval = window.setTimeout(function (e) { me.Hidden(e); }, 500);
        //    } catch (e) { alert(e); }
        //};

    }

    if (!this.isOpera) {
        addEvent(el,"keyup",function (e) {
            e = e ? e : window.event;
            if (e.keyCode != 13 && e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 9)
                me.ShowPanel(e);

        });

        /////文本框事件
        //el.onkeyup = function (e) {
        //    e = e ? e : window.event;
        //    if (e.keyCode != 13 && e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 9)
        //        me.ShowPanel(e);

        //};

        if (el.readOnly || this.oP.showall) {
            //el.onfocus = function (e) {
            //    me.ShowPanel(e);
            //};
            addEvent(el,"focus",function (e) {
                me.ShowPanel(e);
            });
        }

    }

    addEvent(el, "keydown", function (e) {
        try {
            e = e ? e : window.event;
            switch (e.keyCode) {
                case 13:    //回车
                    me.SelOK(e);

                    break;
                case 40:    //向下
                    me.move(e, 1);
                    break;
                case 38:    //向上
                    me.move(e, -1);
                    break;
                case 9:
                    me.Hidden(e);
                    break;
                case 8:
                case 46:
                    this.setAttribute("upval", this.value);
                    break;
                default:
                    break;
            }
            if (e.keyCode === 13 || e.keyCode === 40 || e.keyCode === 38) {
                try { e.keyCode = 0; } catch (e) { }
                return false;
            }
        }
        catch (e) {
            alert(e + " | dropdownlist_onkeydown");
        }
    });

    //el.onkeydown = function (e) {
    //    try {
    //        e = e ? e : window.event;
    //        switch (e.keyCode) {
    //            case 13:    //回车
    //                me.SelOK(e);

    //                break;
    //            case 40:    //向下
    //                me.move(e, 1);
    //                break;
    //            case 38:    //向上
    //                me.move(e, -1);
    //                break;
    //            case 9:
    //                me.Hidden(e);
    //                break;
    //            case 8:
    //            case 46:
    //                this.setAttribute("upval", this.value);
    //                break;
    //            default:
    //                break;
    //        }
    //        if (e.keyCode === 13 || e.keyCode === 40 || e.keyCode === 38) {
    //            try { e.keyCode = 0; } catch (e) { }
    //            return false;
    //        }
    //    }
    //    catch (e) {
    //        alert(e + " | dropdownlist_onkeydown");
    //    }
    //}
    //this.oP.panel.onscroll = function (e) {
        //alert("sdfs");
        //me.ResetPanelOffset(me.ge(me.oP.elId + "_AC_Panel"));
    //}

};


//事件片理函数
function addEvent(element, type, handler) {
    //为每一个事件处理函数分派一个唯一的ID
    if (!handler.$$guid) handler.$$guid = addEvent.guid++;
    //为元素的事件类型创建一个哈希表  
    if (!element.events) element.events = {};
    //为每一个"元素/事件"对创建一个事件处理程序的哈希表  
    var handlers = element.events[type];
    if (!handlers) {
        handlers = element.events[type] = {};
        //存储存在的事件处理函数(如果有)  
        if (element["on" + type]) {
            handlers[0] = element["on" + type];
        }
    }
    //将事件处理函数存入哈希表  
    handlers[handler.$$guid] = handler;
    //指派一个全局的事件处理函数来做所有的工作  
    element["on" + type] = handleEvent;
};

//用来创建唯一的ID的计数器  
addEvent.guid = 1;

function removeEvent(element, type, handler) {
    //从哈希表中删除事件处理函数  
    if (element.events && element.events[type]) {
        delete element.events[type][handler.$$guid];
    }
};

function handleEvent(event) {
    var returnValue = true;
    //抓获事件对象(IE使用全局事件对象)  
    event = event || fixEvent(window.event);
    //取得事件处理函数的哈希表的引用  
    var handlers = this.events[event.type];
    //执行每一个处理函数  
    for (var i in handlers) {
        this.$$handleEvent = handlers[i];
        if (this.$$handleEvent(event) === false) {
            returnValue = false;
        }
    }
    return returnValue;
};

//为IE的事件对象添加一些“缺失的”函数  
function fixEvent(event) {
    //添加标准的W3C方法  
    event.preventDefault = fixEvent.preventDefault;
    event.stopPropagation = fixEvent.stopPropagation;
        return event;
};

fixEvent.preventDefault = function () {
    this.returnValue = false;
};

fixEvent.stopPropagation = function () {
    this.cancelBubble = true;
};




function dorpdownlist_doup(e) {
    e = e ? e : window.event;
    var obj = e.srcElement ? e.srcElement : e.target;
    if (window.AutoComplite_Source) {
        if (obj.id == window.AutoComplite_Source.id || obj.getAttribute("rel") == window.AutoComplite_Source.id) {
            return true;
        }


        var panel = document.getElementById(window.AutoComplite_Source.elId + "_AC_Panel");
        if (panel) {
            panel.style.display = "none";
        }
        window.AutoComplite_Source = null;
    }
}


function formatRegExp(s) 
{ 
    var pattern = new RegExp("([`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？])") 
    var rs = ""; 
    for (var i = 0; i < s.length; i++) { 
        rs = rs+s.substr(i, 1).replace(pattern, "\\$1"); 
    } 
    return rs; 
}

function addDropdownlistStyle() {
    var _path = document.getElementsByTagName('script');
    var href = "";
    for (var a = 0; a < _path.length;a++ ) {
        href = _path[a].src
        if (href) {
            if (href.indexOf("/dropdownlist.js") >= 0) {
                href = href.replace(".js", ".css");
                break;
            }
        }
    }
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = href;
    document.getElementsByTagName('head')[0].appendChild(link);
}

addEvent(document, "mouseup", dorpdownlist_doup);
addEvent(window, "load", addDropdownlistStyle);
