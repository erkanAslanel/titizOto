var library;
var path = "";


function Reklam5() { }
Reklam5.prototype.init = function () {

    path = $('body').attr("data-path");
    var modul = this.getConfig().modul.split(',');
    for (var i = 0; i < modul.length; i++)
        this.runModule(modul[i]);
}
Reklam5.prototype.getConfig = function () {
    var scripts = document.getElementById('reklam5');
    var queryString = scripts.src.replace(/^[^\?]+\??/, '');

    var Params = new Object();
    if (!queryString) return Params;

    var Pairs = queryString.split(/[;&]/);

    for (var i = 0; i < Pairs.length; i++) {
        var KeyVal = Pairs[i].split('=');

        if (!KeyVal || KeyVal.length != 2)
            continue;

        var key = unescape(KeyVal[0]);
        var val = unescape(KeyVal[1]);
        val = val.replace(/\+/g, ' ');
        Params[key] = val;
    }
    return Params;
}
Reklam5.prototype.runModule = function (module) {

    console.log(path);

    switch (module) {
        case "login":
            this.successLogin(); // login olunduğunda ,başarılıysa ana sayfaya yönlendirilir.
            break;
    }



}

function redirectMainPage(a, b) {

    setTimeout(function () {
        window.location = a;
    }, b);

}

Reklam5.prototype.successLogin = function () {

    if ($('.nSuccess').size() > 0) { 

        console.log(path);
        var url = path + "radmin/Dashboard/index";
        var second = 2000; 
        redirectMainPage(url, second);
    }

}

library = new Reklam5();
library.init();