//视频列表 便于多个视频切换
var videos = ["/Files/环保小视频.mp4"];
var activityTab = 0; //视频切换值
var pop = null;
var $video = null;
var init = function () {
    pop = Popcorn("#ourvideobig");
    $video = document.getElementById("ourvideobig");
    loadedReady(); //视频加载完毕执行事件
};
$(function () {
    init();//初始化
});

var wait = 0;
//视频加载完毕执行事件
function loadedReady() {
    var rdy = pop.readyState();//获取视频状态
    if (rdy === 4)//加载完毕
    {
        $("#msg").html("视频总时间：" + pop.duration());
        //console.log("视频总时间：" + pop.duration());
        timeupdate();//动态监听播放时间
    }
    else {
        console.log("视频状态：" + rdy + "，次数=" + wait);
        //循环
        if (wait <= 10) {
            // console.log("wait:" + wait);
            setTimeout(loadedReady, 200);
            wait++;
        }
    }
}

//绑定动态监听播放时间
function timeupdate() {
    $video.addEventListener("timeupdate", function (e) {
        $("#txt_msg").val("当前播放时间：" + $video.currentTime);
        //可以用于自动切换功能
    });
}
//绑定拖动视频时间
function seeked() {
    $video.addEventListener("seeked", function (e) {
        console.log("当前视频拖动时间：" + $video.currentTime);
    });
}

//重置视频 即把视频当前时间设置为0
function reset() { pop.currentTime(0); }
//停止视频
function destroy() {
    pop.currentTime(0);
    pop.pause();//暂停
}
//全屏
function fullscreen() { $video.webkitRequestFullScreen(); }