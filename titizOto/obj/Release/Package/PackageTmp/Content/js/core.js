
if (typeof titizJs == 'undefined') {
    titizJs = function () { }
}

$(function () {
    titizJs.init();
});

titizJs.init = function () {

    titizJs.mainPath = $('body').attr("data-mainpath");



    var modul = this.getConfig().modul.split(',');
    for (var i = 0; i < modul.length; i++)
        this.runModule(modul[i]);
}

titizJs.getConfig = function () {
    var scripts = document.getElementById('titizJs');
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

titizJs.runModule = function (module) {

    switch (module) {
        case "index":

            $(document).on('init.slides', function () {
                $('.sliderContainer .sliderControl .controlBlock .next').click(function () {

                    $('.slides-navigation a.next').click();
                });

                $('.sliderContainer .sliderControl .controlBlock .prev').click(function () {

                    $('.slides-navigation a.prev').click();
                });

                $('.sliderContainer .sliderControl .controlBlock .pause').click(function () {

                    $('#slider').superslides('stop');
                    $(this).hide();
                    $('.sliderContainer .sliderControl .controlBlock .play').css('display', 'inline-block');
                });

                $('.sliderContainer .sliderControl .controlBlock .play').click(function () {
                    console.log("pause");
                    $('#slider').superslides('start');
                    $('.sliderContainer .sliderControl .controlBlock .pause').show();
                    $(this).hide();
                });
            });

            $('#slider').superslides({
                animation: 'fade',
                inherit_height_from: '#slider',
                inherit_width_from: '#slider',
                pagination: false,
                play: 6000
            });
             
            $(document).on("click",".msg", function () {

                $(this).fadeOut(function () {

                    $(this).remove();
                });
            });

            $('#newsletterForm .btn').on('click', function () {

                var $button = $(this);

                if ($(this).hasClass("waiting")) {

                    return false;
                }

                $button.addClass("waiting");
                $button.addClass("disableBtn");
                $button.text($(this).attr("data-waiting"));
                $('#newsletterForm .msg').remove();

                

                $.ajax({
                    dataType: "JSON", 
                    type: "POST",
                    cache :false,
                    url: titizJs.mainPath + "registerNewsletter", 
                    data: $("#newsletterForm").serialize(),
                    complete: function (jqXHR) {
                         
                        var responseText = jQuery.parseJSON(jqXHR.responseText);
                         
                        if (responseText.isSuccess == "Ok") {

                            $('#newsletterForm').prepend('<div class="msg success">' + responseText.msg + '</div');
                            $('.msg').fadeIn();

                            $button.removeClass("waiting");
                            $button.removeClass("disableBtn");
                            $button.text($button.attr("data-save"));

                            $('#newsletterForm input').val("");

                        }
                        else {

                            $('#newsletterForm').prepend('<div class="msg error">' + responseText.msg + '</div');
                            $('.msg').fadeIn();
                            $button.removeClass("waiting");
                            $button.removeClass("disableBtn");
                            $button.text($button.attr("data-save"));

                           
                        }



                    },
                    error: function () {

                        $('#newsletterForm').prepend('<div class="msg error">' + responseText.msg + '</div');
                        $('.msg').fadeIn();

                        $button.removeClass("waiting");
                        $button.removeClass("disableBtn");
                        $button.text($button.attr("data-save"));

                    }

                });


            });

            break;



    }
}


titizJs.empty = function () {



}


