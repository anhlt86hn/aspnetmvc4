﻿//banner 2 ben
function FloatTopDiv() {
    debugger;
    startLX = ((document.body.clientWidth - MainContentW) / 2) - LeftBannerW - LeftAdjust, startLY = TopAdjust + 140;
    startRX = ((document.body.clientWidth - MainContentW) / 2) + MainContentW + RightAdjust, startRY = TopAdjust + 140;
    var d = document;
    function ml(id) {
        debugger;
        var el = d.getElementById ? d.getElementById(id) : d.all ? d.all[id] : d.layers[id];
        el.sP = function (x, y) {
            this.style.left = x + 'px';
            this.style.top = y + 'px';
        };
        el.x = startRX;
        el.y = startRY;
        return el;
    }
    function m2(id) {
        debugger;
        var e2 = d.getElementById ? d.getElementById(id) : d.all ? d.all[id] : d.layers[id];
        e2.sP = function (x, y) {
            this.style.left = x + 'px';
            this.style.top = y + 'px';
        };
        e2.x = startLX;
        e2.y = startLY;
        return e2;
    }
    window.stayTopLeft = function () {
        if (document.documentElement && document.documentElement.scrollTop)
            var pY = document.documentElement.scrollTop;
        else if (document.body)
            var pY = document.body.scrollTop;
        if (document.body.scrollTop > 30) { startLY = 10; startRY = 10; } else { startLY = TopAdjust; startRY = TopAdjust; };
        ftlObj.y += (pY + startRY - ftlObj.y) / 16;
        ftlObj.sP(ftlObj.x, ftlObj.y);
        ftlObj2.y += (pY + startLY - ftlObj2.y) / 16;
        ftlObj2.sP(ftlObj2.x, ftlObj2.y);
        setTimeout("stayTopLeft()", 1);
    }
    ftlObj = ml("divAdRight");
    //stayTopLeft();     
    ftlObj2 = m2("divAdLeft");
    stayTopLeft();
}
function ShowAdDiv() {
    debugger;
    var objAdDivRight = document.getElementById("divAdRight");
    var objAdDivLeft = document.getElementById("divAdLeft");
    if (document.body.clientWidth < 1000) {
        debugger;
        objAdDivRight.style.display = "none";
        objAdDivLeft.style.display = "none";
    }
    else {
        debugger;
        objAdDivRight.style.display = "block";
        objAdDivLeft.style.display = "block";
        FloatTopDiv();
    }
}

$.ajax({
    url: '/My_Partialview/_MenuTop',
    contentType: 'application/html; charset=utf-8',
    type: 'GET',
    dateType: 'html'
})
     .success(function (result) {
         $('#menu').html(result);
     })
     .error(function (xhr, status) { alert(status); })

$.ajax({
    url: '/My_Partialview/_Logo',
    contentType: 'application/html; charset=utf-8',
    type: 'GET',
    dateType: 'html'
})
.success(function (result) {
    $('#logo').html(result);
})
.error(function (xhr, status) { alert(status); })

$.ajax({
    url: '/My_Partialview/_CopyRight',
    contentType: 'application/html; charset=utf-8',
    type: 'GET',
    dateType: 'html'
})
.success(function (result) {
    $('#foo').html(result);
})
.error(function (xhr, status) { alert(status); })

$.ajax({
    url: '/My_Partialview/_AdvLeft',
    contentType: 'application/html; charset=utf-8',
    type: 'GET',
    dateType: 'html'
})
.success(function (result) {
    $('#Adtop').html(result);
})
.error(function (xhr, status) { alert(status); })

$.ajax({
    url: '/My_Partialview/_NewsRight',
    contentType: 'application/html; charset=utf-8',
    type: 'GET',
    dateType: 'html'
})
.success(function (result) {
    $('#NewR').html(result);
})
.error(function (xhr, status) { alert(status); })

$.ajax({
    url: '/My_Partialview/_AdvRightBottom',
    contentType: 'application/html; charset=utf-8',
    type: 'GET',
    dateType: 'html'
})
.success(function (result) {
    $('#AdBot').html(result);
})
.error(function (xhr, status) { alert(status); })
