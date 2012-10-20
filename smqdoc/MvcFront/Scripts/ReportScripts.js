$(document).ready(function () {
    var textAreas = $(".LegendBlock");
    for (var i = 0; i < textAreas.length; i++) {
        $(textAreas.get(i)).height(textAreas.get(i).scrollHeight);
    }
})