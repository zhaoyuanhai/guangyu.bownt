$(function () {
    var table = $("table.bowntTable");
    table.find("td").css({
        padding: "1px",
        border: "1px solid white",
        color: "white"
    });

    table = $("table", ".bowntTableContainer");
    table.find("td").css({
        padding: "1px",
        border: "1px solid white",
        color: "white"
    });

    var span = table.find("span");
    span.css("position", "initial");
});