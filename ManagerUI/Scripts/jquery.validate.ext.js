$(document).ready(function () {

    /* 设置默认属性 */
    $.validator.setDefaults({
        submitHandler: function (form) { form.submit(); }
    });
    // 中文字两个字节 
    jQuery.validator.addMethod("byteRangeLength", function (value, element, param) {
        var length = value.length;
        for (var i = 0; i < value.length; i++) {
            if (value.charCodeAt(i) > 127) {
                length++;
            }
        }
        return this.optional(element) || (length >= param[0] && length <= param[1]);
    });

    /* 追加自定义验证方法 */
    // 字符验证 
    jQuery.validator.addMethod("stringCheck", function (value, element) {
        return this.optional(element) || /^[\u0391-\uFFE5\w]+$/.test(value);
    }, "只能包括中文字、英文字母、数字和下划线");

    // 身份证号码验证 
    jQuery.validator.addMethod("isIdCardNo", function (value, element) {
        return this.optional(element) || isIdCardNo(value);
    }, "请正确输入您的身份证号码");

    // 字符验证 
    jQuery.validator.addMethod("userName", function (value, element) {
        return this.optional(element) || /^[\u0391-\uFFE5\w]+$/.test(value);
    }, "用户名只能包括中文字、英文字母、数字和下划线");

    // 手机号码验证 
    jQuery.validator.addMethod("isMobile", function (value, element) {
        var length = value.length;
        return this.optional(element) || (length == 11 && /^(((13[0-9]{1})|(15[0-9]{1}))+\d{8})$/.test(value));
    }, "请正确填写您的手机号码");

    // 电话号码验证 
    jQuery.validator.addMethod("isPhone", function (value, element) {
        var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
        return this.optional(element) || (tel.test(value));
    }, "请正确填写您的电话号码");

    // 邮政编码验证 
    jQuery.validator.addMethod("isZipCode", function (value, element) {
        var tel = /^[0-9]{6}$/;
        return this.optional(element) || (tel.test(value));
    }, "请正确填写您的邮政编码");
   


    // 输入框获得焦点时，样式设置 
    $('input').focus(function () {
        if ($(this).is(":text") || $(this).is(":password"))
            $(this).addClass('focus');
        if ($(this).hasClass('have_tooltip')) {
            $(this).parent().parent().removeClass('field_normal').addClass('field_focus');
        }
    });

    // 输入框失去焦点时，样式设置 
    $('input').blur(function () {
        $(this).removeClass('focus');
        if ($(this).hasClass('have_tooltip')) {
            $(this).parent().parent().removeClass('field_focus').addClass('field_normal');
        }
    });

    jQuery.extend(jQuery.validator.messages, {
        required: "必填项",
        remote: "请修正该字段",
        email: "无效邮箱地址",
        url: "无效网址",
        date: "无效日期",
        dateISO: "请输入合法的日期 (ISO).",
        number: "请输入数字",
        digits: "只能输入整数",
        creditcard: "请输入合法的信用卡号",
        equalTo: "与上次输入的不一致",
        accept: "请输入拥有合法后缀名的字符串",
        maxlength: jQuery.validator.format("请输入一个 长度最多是 {0} 的字符串"),
        minlength: jQuery.validator.format("请输入一个 长度最少是 {0} 的字符串"),
        rangelength: jQuery.validator.format("请输入 一个长度介于 {0} 和 {1} 之间的字符串"),
        range: jQuery.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
        max: jQuery.validator.format("请输入一个最大为{0} 的值"),
        min: jQuery.validator.format("请输入一个最小为{0} 的值"),
        byteRangeLength:"只允许{0}~{1}长度"
    });
});

function isIdCardNo(num)
{
    var factorArr = new Array(7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2,1);
    var error;
    var varArray = new Array();
    var intValue;
    var lngProduct = 0;
    var intCheckDigit;
    var intStrLen = num.length;
    var idNumber = num;   
    // initialize
    if ((intStrLen != 15) && (intStrLen != 18)) {
        //error = "输入身份证号码长度不对！";
        //alert(error);
        //frmAddUser.txtIDCard.focus();
        return false;
    }   
    // check and set value
    for(i=0;i<intStrLen;i++) {
        varArray[i] = idNumber.charAt(i);
        if ((varArray[i] < '0' || varArray[i] > '9') && (i != 17)) {
            //error = "错误的身份证号码！.";
            //alert(error);
            //frmAddUser.txtIDCard.focus();
            return false;
        } else if (i < 17) {
            varArray[i] = varArray[i]*factorArr[i];
        }
    }
    if (intStrLen == 18) {
        //check date
        var date8 = idNumber.substring(6,14);
        if (checkDate(date8) == false) {
            //error = "身份证中日期信息不正确！.";
            //alert(error);
            return false;
        }       
        // calculate the sum of the products
        for(i=0;i<17;i++) {
            lngProduct = lngProduct + varArray[i];
        }       
        // calculate the check digit
        intCheckDigit = 12 - lngProduct % 11;
        switch (intCheckDigit) {
            case 10:
                intCheckDigit = 'X';
                break;
            case 11:
                intCheckDigit = 0;
                break;
            case 12:
                intCheckDigit = 1;
                break;
        }       
        // check last digit
        if (varArray[17].toUpperCase() != intCheckDigit) {
            //error = "身份证效验位错误!...正确为： " + intCheckDigit + ".";
            //alert(error);
            return false;
        }
    }
    else{        //length is 15
        //check date
        var date6 = idNumber.substring(6,12);
        if (checkDate(date6) == false) {
            //alert("身份证日期信息有误！.");
            return false;
        }
    }
    //alert ("Correct.");
    return true;
}

function checkDate(date)
{
    return true;
}

function showBirthday(val)
{
    var birthdayValue;
    if(15==val.length)
    { //15位身份证号码
        birthdayValue = val.charAt(6)+val.charAt(7);
        if(parseInt(birthdayValue)<10)
        {
            birthdayValue = '20'+birthdayValue;
        }
        else
        {
            birthdayValue = '19'+birthdayValue;
        }
        birthdayValue=birthdayValue+'-'+val.charAt(8)+val.charAt(9)+'-'+val.charAt(10)+val.charAt(11);
        if(parseInt(val.charAt(14)/2)*2!=val.charAt(14))
            document.all.sex.value='男';
        else
            document.all.sex.value='女';
        document.all.birthday.value=birthdayValue;
    }
    if(18==val.length)
    { //18位身份证号码
        birthdayValue=val.charAt(6)+val.charAt(7)+val.charAt(8)+val.charAt(9)+'-'+val.charAt(10)+val.charAt(11)

       +'-'+val.charAt(12)+val.charAt(13);
        if(parseInt(val.charAt(16)/2)*2!=val.charAt(16))
            document.all.sex.value='男';
        else
            document.all.sex.value='女';
        if(val.charAt(17)!=IDCard(val))
        {
            document.all.idCard.style.backgroundColor='#ffc8c8';
        }
        else
        {
            document.all.idCard.style.backgroundColor='white';
        }
        document.all.birthday.value=birthdayValue;
    }
}

// 18位身份证号最后一位校验
function IDCard(Num)
{
    if (Num.length!=18)
        return false;
    var x=0;
    var y='';

    for(i=18;i>=2;i--)
        x = x + (square(2,(i-1))%11)*parseInt(Num.charAt(19-i-1));
    x%=11;
    y=12-x;
    if (x==0)
        y='1';
    if (x==1)
        y='0';
    if (x==2)
        y='X';
    return y;
}

// 求得x的y次方
function square(x,y)
{
    var i=1;
    for (j=1;j<=y;j++)
        i*=x;
    return i;
}
