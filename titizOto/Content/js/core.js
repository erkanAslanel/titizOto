
if (typeof titizJs == 'undefined') {
    titizJs = function () { }
}

$(function () {
    titizJs.init();
});

titizJs.init = function () {

    titizJs.mainPath = $('body').attr("data-mainpath");
    titizJs.langCode = $('body').attr("data-lang");


    $('#newsletterForm .btn').on('click', function () {

        var $button = $(this);

        var validator = $('#newsletterForm').valid();

        if (!validator) {
            return false;
        }


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
            cache: false,
            url: titizJs.mainPath + "Newsletter" + "/" + "Add",
            data: $("#newsletterForm").serialize(),
            complete: function (jqXHR) {

                var responseText = jQuery.parseJSON(jqXHR.responseText);

                if (responseText.isSuccess == "Ok") {

                    $('#newsletterForm').prepend('<div class="msg success">' + responseText.msg + '</div>');
                    $('.msg').fadeIn();

                    $button.removeClass("waiting");
                    $button.removeClass("disableBtn");
                    $button.text($button.attr("data-save"));

                    $('#newsletterForm input').val("");

                }
                else {

                    $('#newsletterForm').prepend('<div class="msg error">' + responseText.msg + '</div>');
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

    setTimeout(function () { $('.autoHide').slideUp("slow", function () { $('.autoHide').remove(); }); }, 5000);

    $(document).on("click", ".clickDisable", function () {

        var validationDom = $(this).attr("data-checkvalidation");

        if (typeof validationDom !== 'undefined' && validationDom !== false) {

            var validator = $(validationDom).valid();

            if (validator) {

                $(this).addClass('disableBtn');
                $(this).attr("disable", "disable");

            }
        }
        else {
            $(this).addClass('disableBtn');
        }


    });

    $(document).on("click", ".disableBtn", function (event) {

        event.preventDefault();
        return false;

    });


    $(document).on("click", ".msg .msgclose", function () {

        $(this).parent().slideUp("fast", function () {
            $(this).remove();
        });
    });

    if ($('#paging').size() > 0) {


        $('#paging').pagination({
            items: parseInt($('#paging').attr("data-items")),
            currentPage: parseInt($('#paging').attr("data-current")),
            itemsOnPage: parseInt($('#paging').attr("data-peritem")),
            cssStyle: 'light-theme',
            prevText: 'Önceki',
            nextText: 'Sonraki',
            hrefTextPrefix: $('#paging').attr("data-suffix")
        });
    }

    $(document).on("click", "footer .linkContainer > li a.noPointer", function (event) {

        event.preventDefault();
    });

    $(document).on("keyup", "input[data-onenter]", function (e) {

        e.preventDefault();

        if (e.keyCode == 13) {
            var clickElement = $(this).attr("data-onenter");


            $(clickElement).click();
        }
    });

    var searchTime = 0;

    $('.searchBox .keyBox > input').on("keyup", function (e) {

        var searchWord = $(this).val();

        searchTime = e.timeStamp;

        if (searchWord.length > 2) {

            $('.searchBox .searchResult .close').show();
            $('.searchBox .searchResult .loading').slideDown();
            $('.searchBox .searchResult .container').slideUp();

            setTimeout(function () {

                if (searchTime != e.timeStamp) {

                    return;
                }

                var langCode = titizJs.langCode;

                var openNewAddress = $.ajax({
                    dataType: "JSON",
                    type: "POST",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "Search" + "/" + "SearchModal",
                    data: $('.searchBox form').serialize()

                });

                openNewAddress.done(function (msg) {

                    $('.searchBox .searchResult .loading').hide();
                    $('.searchBox .searchResult .container').show();
                    $('.searchBox .searchResult .container').html(msg.html);

                    $('.searchBox .searchResult .container').slideDown();


                    if (msg.isResultCome == "yes") {

                        $('.searchBox .searchResult .container ul li a').each(function () {

                            var liContent = $(this).text();
                            $(this).html(replaceContentBold(liContent, msg.keywordList));
                        });
                    }

                });

            }, 1500);
        }

        else {

            $('.searchBox .searchResult .loading').hide();
            $('.searchBox .searchResult .container').slideUp();
        }



    });

    $(document).on("click", ".searchBox .searchResult .close", function () {



        $('.searchBox .searchResult .loading').hide();

        $('.searchBox .searchResult .container').slideUp("normal", function () {

            $('.searchBox .searchResult .close').slideUp();
        });



    });

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
                $('.sliderContainer .sliderControl .controlBlock .next').click(function (event) {
                    event.preventDefault();
                    $('.slides-navigation a.next').click();
                    $('#slider').superslides('stop');
                    $('.sliderContainer .sliderControl .controlBlock .pause').hide()
                    $('.sliderContainer .sliderControl .controlBlock .play').css('display', 'inline-block');
                });

                $('.sliderContainer .sliderControl .controlBlock .prev').click(function (event) {
                    event.preventDefault();
                    $('.slides-navigation a.prev').click();
                    $('#slider').superslides('stop');
                    $('.sliderContainer .sliderControl .controlBlock .pause').hide()
                    $('.sliderContainer .sliderControl .controlBlock .play').css('display', 'inline-block');
                });

                $('.sliderContainer .sliderControl .controlBlock .pause').click(function (event) {
                    event.preventDefault();
                    $('#slider').superslides('stop');
                    $(this).hide();
                    $('.sliderContainer .sliderControl .controlBlock .play').css('display', 'inline-block');
                });

                $('.sliderContainer .sliderControl .controlBlock .play').click(function (event) {
                    event.preventDefault();
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

            break;

        case "productList":

            productListAdd();

            break;

        case "brandSelect":

            productListAdd();

            break;

        case "openContent":

            $('.openContentContainer ul li a').on("click", function () {

                $(this).next().toggle("slow", function () {

                });

            });

            break;

        case "productDetail":

            $('.optionContainer .item select').on("change", function () {
                $(this).next().hide();

            });

            $('.rightProduct .bntContainer .addBasket').on("click", function (event) {

                event.preventDefault();

                var $actionBtn = $(this);
                var $rightProduct = $actionBtn.parent().parent();
                $rightProduct.find('.msg').remove();

                var $optionList = "";
                var optionValid = true;
                var optionValueList = [];

                if ($rightProduct.find('.optionContainer .item select').size() > 0) {

                    $optionList = $rightProduct.find('.optionContainer .item select');

                    $optionList.each(function (index) {

                        if ($(this).val() == "0") {

                            $(this).next().fadeIn("slow");
                            optionValid = false;

                        }
                        else {
                            optionValueList.push($(this).val());
                        }

                    });
                }


                if (!optionValid) {
                    return;
                }


                if ($actionBtn.hasClass("disableBtn")) {
                    return;
                }

                $actionBtn.addClass("disableBtn");

                $rightProduct.find('.payOption').fadeOut("normal", function () {
                    $rightProduct.find(".productDetailLoader").show();
                });

                var productId = $actionBtn.attr("data-productid");
                var langCode = titizJs.langCode;

                var addRequest = $.ajax({
                    dataType: "JSON",
                    type: "POST",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "ProductDetail" + "/" + "AddProduct",
                    data: { productId: productId, optionValueList: optionValueList.toString() },

                });

                addRequest.done(function (msg) {

                    $rightProduct.find('.price').before(msg.msgHtml);

                    if (msg.result == "success") {

                        $('.cartBox').remove();
                        $('.searchBox').before(msg.cartHtml);
                    }

                    $actionBtn.removeClass("disableBtn");

                    $rightProduct.find('.productDetailLoader').fadeOut("normal", function () {
                        $rightProduct.find(".payOption").show();
                    });

                    $rightProduct.find('.msg').fadeIn("slow", function () {

                        setTimeout(function () {

                            $rightProduct.find('.msg').fadeOut("slow");

                        }, 5000);

                    });
                });

                addRequest.fail(function (msg) {


                    $actionBtn.removeClass("disableBtn");

                    $rightProduct.find('.productDetailLoader').fadeOut("normal", function () {
                        $rightProduct.find(".payOption").show();
                    });


                    $rightProduct.find('.msg').fadeIn("slow", function () {

                        setTimeout(function () {

                            $rightProduct.find('.msg').fadeOut("slow");

                        }, 5000);

                    });

                });


            });

            $('.payOption').on("click", function () {

                $.fancybox.showLoading();
                var productId = $('.addBasket').attr("data-productid");
                var langCode = titizJs.langCode;
                var openOption = $.ajax({
                    dataType: "JSON",
                    type: "POST",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "ProductDetail" + "/" + "OptionList",
                    data: { productId: productId },

                });

                openOption.done(function (msg) {

                    $.fancybox.hideLoading();
                    $.magnificPopup.open({
                        items: {
                            src: msg.html
                        },
                        type: 'inline' // this is default type
                    });

                });


            });

            $('.bigPhoto').cycle();

            break;

        case "loginRegister":

            $('body').on('click', ".registerForm .submitBtn a", function () {

                var validator = $('.registerForm form').valid();

                if (validator) {

                    if ($(this).hasClass("wait")) {

                        return;
                    }

                    $(this).addClass("wait");

                    $(this).fadeOut("normal", function () {

                        $(this).parent().parent().find('.loadingForm').fadeIn();
                    });

                    var dataLang = titizJs.langCode;
                    var formData = $('.registerForm form').serialize();

                    var registerUser = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + dataLang + "/" + "LoginRegister/RegisterUser",
                        data: formData

                    });

                    registerUser.done(function (msg) {

                        $('.registerForm').html(msg.htmlText);
                        $('.field-validation-error').hide();
                        $('.field-validation-error').fadeIn();



                        if (msg.redirectPage != undefined & msg.redirectPage != "") {

                            setTimeout(function () { location.href = msg.redirectPage; }, 4000);
                        }

                    });

                }
                else {

                    $('.field-validation-error').hide();
                    $('.field-validation-error').fadeIn();
                }

            });

            $('body').on('click', ".loginForm .submitBtn a", function () {

                var validator = $('.loginForm form').valid();
                if (validator) {

                    if ($(this).hasClass("wait")) {

                        return;
                    }

                    $(this).addClass("wait");

                    $(this).fadeOut("normal", function () {

                        $(this).parent().parent().find('.loadingForm').fadeIn();
                    });

                    var dataLang = titizJs.langCode;
                    var formData = $('.loginForm form').serialize();

                    var registerUser = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + dataLang + "/" + "LoginRegister/Login",
                        data: formData

                    });

                    registerUser.done(function (msg) {

                        $('.loginForm').html(msg.htmlText);
                        $('.field-validation-error').hide();
                        $('.field-validation-error').fadeIn();



                        if (msg.redirectPage != undefined & msg.redirectPage != "") {

                            setTimeout(function () { location.href = msg.redirectPage; }, 4000);
                        }

                    });

                }
                else {

                    $('.field-validation-error').hide();
                    $('.field-validation-error').fadeIn();
                }

            });

           
            $('body').on('click', ".registerForm .aggrement a", function (e) {

                var content = $(this).parent().parent().find('.agreementContent').html();

                e.preventDefault();
                $.fancybox.showLoading();
                $.magnificPopup.open({
                    items: {
                        src: content
                    },
                    type: 'inline' // this is default type
                });

                $.fancybox.hideLoading();
            
            });

            break;

        case "accountAddress":

            // [Get] Add  Address 
            $('body').on('click', ".newAddress", function () {

                $.fancybox.showLoading();
                var userGuid = $('.addresBoxContainer').attr("data-userguid");
                var langCode = titizJs.langCode;

                var openNewAddress = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressAdd",
                    data: { userGuid: userGuid }

                });


                openNewAddress.done(function (msg) {

                    $.fancybox.hideLoading();


                    $.magnificPopup.open({
                        items: {
                            src: msg.html
                        },
                        type: 'inline',
                        closeOnBgClick: false,
                        closeBtnInside: false,
                        showCloseBtn: false,
                        callbacks: {
                            open: function () {

                                addressFormStart();
                                addressPersonalToCompany();

                                $('body').on('click', ".addAddressForm .submitContainer .close", function () {
                                    $.magnificPopup.close();
                                });


                            },
                            close: function () {

                                var closeUrl = $('.addAddressForm').attr("data-closeurl");


                                $.fancybox.showLoading();

                                setTimeout(function () {

                                    redirectPage(closeUrl);

                                }, 1000);
                            }

                        }
                    });

                });

            });

            // [Post] Add Address  
            $('body').on('click', ".addAddressForm .submitContainer .create", function () {


                $('#addForm').validate();
                var validator = $('#addForm').valid();

                if (validator) {

                    var userGuid = $('.addresBoxContainer').attr("data-userguid");
                    var langCode = titizJs.langCode;

                    var saveNewAddress = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: $("#addForm").attr("action"),
                        data: $("#addForm").serialize()

                    });

                    saveNewAddress.done(function (msg) {

                        $('.modelContainer').parent().html(msg.html);

                        addressFormStart();
                        addressPersonalToCompany();

                        setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
                    });

                }
                else {

                    return false;
                }

            });

            // [Get] Detail Address
            $('body').on('click', ".addresBox .detail", function () {

                $.fancybox.showLoading();

                var userGuid = $('.addresBoxContainer').attr("data-userguid");
                var langCode = titizJs.langCode;
                var addressId = $(this).parent().attr("data-addressid");

                var openNewAddress = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressDetail",
                    data: { userGuid: userGuid, addressId: addressId }

                });


                openNewAddress.done(function (msg) {

                    $.fancybox.hideLoading();

                    $.magnificPopup.open({
                        items: {
                            src: msg.html
                        },
                        type: 'inline',
                        closeOnBgClick: false,
                        closeBtnInside: false,
                        showCloseBtn: false,
                        callbacks: {
                            open: function () {
                                $('body').on('click', ".submitContainer .close", function () {
                                    $.magnificPopup.close();
                                });
                            }
                        }

                    });

                });

            });

            // [Get] Address Edit 
            $('body').on('click', ".addresBox .edit", function () {

                $.fancybox.showLoading();
                var userGuid = $('.addresBoxContainer').attr("data-userguid");
                var langCode = titizJs.langCode;
                var addressId = $(this).parent().attr("data-addressid");

                var openNewAddress = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressEdit",
                    data: { userGuid: userGuid, addressId: addressId }

                });


                openNewAddress.done(function (msg) {

                    $.fancybox.hideLoading();

                    $.magnificPopup.open({
                        items: {
                            src: msg.html
                        },
                        type: 'inline',
                        closeOnBgClick: false,
                        closeBtnInside: false,
                        showCloseBtn: false,
                        callbacks: {
                            open: function () {

                                addressFormStart();
                                addressPersonalToCompany();

                                $('body').on('click', ".editAddressForm .submitContainer .close", function () {
                                    $.magnificPopup.close();
                                });


                            },
                            close: function () {

                                var closeUrl = $('.editAddressForm').attr("data-closeurl");

                                $.fancybox.showLoading();

                                setTimeout(function () {

                                    redirectPage(closeUrl);

                                }, 1000);



                            }

                        }

                    });

                });

            });

            // [Post] Address Edit  
            $('body').on('click', ".editAddressForm .submitContainer .edit", function () {

                var validator = $('#editForm').valid();

                if (validator) {

                    var userGuid = $('.addresBoxContainer').attr("data-userguid");
                    var langCode = titizJs.langCode;

                    var saveNewAddress = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: $("#editForm").attr("action"),
                        data: $("#editForm").serialize()

                    });

                    saveNewAddress.done(function (msg) {

                        $('.modelContainer').parent().html(msg.html);
                        addressFormStart();
                        addressPersonalToCompany();

                        setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
                    });

                }
                else {

                    return false;
                }

            });

            // [Get] Address Delete 
            $('body').on('click', ".addresBox .delete", function () {

                $.fancybox.showLoading();
                var userGuid = $('.addresBoxContainer').attr("data-userguid");
                var langCode = titizJs.langCode;
                var addressId = $(this).parent().attr("data-addressid");

                var openNewAddress = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressDelete",
                    data: { userGuid: userGuid, addressId: addressId }

                });


                openNewAddress.done(function (msg) {

                    $.fancybox.hideLoading();

                    $.magnificPopup.open({
                        items: {
                            src: msg.html
                        },
                        type: 'inline',
                        closeOnBgClick: false,
                        closeBtnInside: false,
                        showCloseBtn: false,
                        callbacks: {
                            open: function () {

                                $('body').on('click', ".deleteAddressForm .submitContainer .close", function () {
                                    $.magnificPopup.close();
                                });


                            },
                            close: function () {

                                var closeUrl = $('.deleteAddressForm').attr("data-closeurl");

                                $.fancybox.showLoading();

                                setTimeout(function () {


                                    redirectPage(closeUrl);

                                }, 1000);



                            }

                        }

                    });

                });

            });

            // [Post] Address Delete  
            $('body').on('click', ".deleteAddressForm .submitContainer .delete", function () {


                var saveNewAddress = $.ajax({
                    dataType: "JSON",
                    type: "POST",
                    cache: false,
                    url: $("#deleteForm").attr("action"),
                    data: $("#deleteForm").serialize()

                });

                saveNewAddress.done(function (msg) {

                    $('.modelContainer').parent().html(msg.html);
                    setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
                });



            });

            addressPersonalToCompany();

            break;

        case "search":

            var searchCount = parseInt($('#searchList_Count').val());
            var keywordList = $.parseJSON($('#searchKeyWordList').val());

            if (searchCount > 0) {

                $('.searchResultList li .t1').each(function () {

                    $(this).html(replaceContentBold($(this).text(), keywordList));

                });

                $('.searchResultList li .t2').each(function () {

                    $(this).html(replaceContentBold($(this).text(), keywordList));

                });

            }

            break

        case "checkoutUnRegisterDelivery":

            addressFormStart();

            addressPersonalToCompany();

            $('body').on('click', ".checkoutForm.delivery .save", function () {

                $('.checkoutForm form').validate();

                var validator = $('.checkoutForm form').valid();

                if (validator) {

                    $.fancybox.showLoading();
                    var saveAddress = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "DeliveryUnRegister",
                        data: $(".checkoutForm form").serialize()

                    });

                    saveAddress.done(function (msg) {

                        console.log(msg);

                        if (msg.isSuccess = "yes") {

                            redirectPage(msg.redirectPage);
                        }
                        else {

                            $('.checkoutForm').html(msg.html);
                            $.fancybox.hideLoading();
                        }

                        setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
                    });

                }

            });

            break;

        case "checkoutRegisterDelivery":

            $('body').on("selectUpdate", ".checkoutForm", function () {

                var selectedAddressId = $('#selectedDeliveryAddressId').val();

                $('.addresBox').hide();
                $('.addresBox .t3[data-addressid="' + selectedAddressId + '"]').parent().parent().parent().show();


            });

            $('body').on("change", "#selectedDeliveryAddressId", function () {


                var selectedAddressId = $('#selectedDeliveryAddressId').val();

                $('.addresBox').hide();
                $('.addresBox .t3[data-addressid="' + selectedAddressId + '"]').parent().parent().parent().show();


            });

            $('body').on("refreshForm", ".checkoutForm", function (data) {

                console.log(data);


                $.fancybox.showLoading();

                var refreshDeliveryRegisterForm = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "DeliveryRegisterModal",
                    data: { pageId: data.pageId, addressId: data.addressId }

                });


                refreshDeliveryRegisterForm.done(function (msg) {

                    $('.checkoutForm').html(msg.html);

                    $(".checkoutForm").trigger({
                        type: "selectUpdate"
                    });

                    $.fancybox.hideLoading();

                });

            });

            $(".checkoutForm").trigger({
                type: "selectUpdate"
            });

            checkoutAddress();

            $('body').on("click", ".registerDeliveryForm .submit a", function () {

                var form = $(".checkoutForm  form");
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);

                $('.checkoutForm  form').validate({
                    debug: true
                });

                var validator = $('.checkoutForm form').valid();

                if (validator) {

                    $.fancybox.showLoading();
                    var langCode = titizJs.langCode;

                    var saveAddressId = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "DeliveryRegister",
                        data: $(".checkoutForm form").serialize()

                    });

                    saveAddressId.done(function (msg) {


                        if (msg.isSuccess = "yes") {

                            redirectPage(msg.redirectPage);
                        }
                        else {

                            $('.checkoutForm').html(msg.html);
                            $.fancybox.hideLoading();
                        }

                        setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);


                    });

                }

            });

            break;

        case "checkoutUnRegisterBilling":

            addressFormStart();

            addressPersonalToCompany();

            $('body').on('click', ".checkoutForm.billing .save", function () {

                $('.checkoutForm form').validate();

                var validator = $('.checkoutForm form').valid();

                if (validator) {

                    $.fancybox.showLoading();
                    var saveAddress = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "DeliveryUnRegister",
                        data: $(".checkoutForm form").serialize()

                    });

                    saveAddress.done(function (msg) {

                        if (msg.isSuccess = "yes") {

                            redirectPage(msg.redirectPage);
                        }
                        else {

                            $('.checkoutForm').html(msg.html);
                            $.fancybox.hideLoading();
                        }

                        setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
                    });

                }

            });

            break;

        case "checkoutRegisterBilling":

            $('body').on("selectUpdate", ".checkoutForm", function () {

                var selectedAddressId = $('#selectedBillingAddressId').val();

                $('.addresBox').hide();
                $('.addresBox .t3[data-addressid="' + selectedAddressId + '"]').parent().parent().parent().show();

            });

            $('body').on("change", "#selectedBillingAddressId", function () {


                var selectedAddressId = $('#selectedBillingAddressId').val();

                $('.addresBox').hide();
                $('.addresBox .t3[data-addressid="' + selectedAddressId + '"]').parent().parent().parent().show();


            });

            $('body').on("refreshForm", ".checkoutForm", function (data) {

                $.fancybox.showLoading();

                var refreshDeliveryRegisterForm = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "BillingRegisterModal",
                    data: { pageId: data.pageId, addressId: data.addressId }

                });


                refreshDeliveryRegisterForm.done(function (msg) {

                    $('.checkoutForm').html(msg.html);

                    $(".checkoutForm").trigger({
                        type: "selectUpdate"
                    });

                    $.fancybox.hideLoading();

                });

            });

            $(".checkoutForm").trigger({
                type: "selectUpdate"
            });

            checkoutAddress();

            $('body').on("click", ".checkoutForm .submit a", function () {

                var form = $(".checkoutForm  form");
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);

                $('.checkoutForm  form').validate({
                    debug: true
                });

                var validator = $('.checkoutForm form').valid();

                if (validator) {

                    $.fancybox.showLoading();

                    var langCode = titizJs.langCode;


                    var saveAddressId = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "BillingRegister",
                        data: $(".checkoutForm form").serialize()

                    });


                    saveAddressId.done(function (msg) {


                        if (msg.isSuccess = "yes") {

                            redirectPage(msg.redirectPage);
                        }
                        else {

                            $('.checkoutForm').html(msg.html);
                            $.fancybox.hideLoading();
                        }

                        setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);


                    });

                }

            });

            break;

        case "payment":

            paymentCreditAction();

            $('input[name="paymentOptionId"]').on("change", function () {

                var paymentOptionId = $(this).val();

                $.fancybox.showLoading();

                var optionChange = $.ajax({
                    dataType: "JSON",
                    type: "GET",
                    cache: false,
                    url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "PaymentRedirect",
                    data: { paymentOptionId: paymentOptionId }

                });

                optionChange.done(function (msg) {

                    $('.subContentPayment').html(msg.html);
                    paymentCreditAction();
                    $.fancybox.hideLoading();


                });
            });

            $('body').on("click", ".transfer .submit a", function () {

                var form = $(".transfer");
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);

                $('.transfer').validate({
                    debug: true
                });

                var validator = $('.transfer').valid();

                if (validator) {

                    $.fancybox.showLoading();

                    var transferPost = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "Transfer",
                        data: $('.transfer').serialize()

                    });

                    transferPost.done(function (msg) {

                        if (msg.isSuccess == "yes") {


                            redirectPage(msg.redirectPage);
                        }
                        else {

                            $('.subContentPayment').html(msg.html);
                            $.fancybox.hideLoading();

                        }


                    });

                }

            });

            $('body').on("click", ".credit .submit a", function () {

                var form = $(".credit form");
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);

                $('.credit form').validate({
                    debug: true
                });

                var validator = $('.credit form').valid();

                if (validator) {

                    $.fancybox.showLoading();

                    var creditPost = $.ajax({
                        dataType: "JSON",
                        type: "POST",
                        cache: false,
                        url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "Credit",
                        data: $('.credit form').serialize()

                    });

                    creditPost.done(function (msg) {

                        if (msg.isSuccess != undefined && msg.isSuccess == "yes") {


                            redirectPage(msg.redirectPage);
                        }
                        else {

                            $('.subContentPayment').html(msg.html);
                            $.fancybox.hideLoading();

                        }
                    });

                }

            });

            $('body').on("click", ".credit .optionTable a", function (e) {

                e.preventDefault();
                $.fancybox.showLoading();

                var amount = $(this).attr("data-amount");
                var langCode = titizJs.langCode;
                var openOption = $.ajax({
                    dataType: "JSON",
                    type: "POST",
                    cache: false,
                    url: titizJs.mainPath + langCode + "/" + "ProductDetail" + "/" + "OptionListByStr",
                    data: { amountStr: amount },

                });

                openOption.done(function (msg) {

                    $.fancybox.hideLoading();
                    $.magnificPopup.open({
                        items: {
                            src: msg.html
                        },
                        type: 'inline' // this is default type
                    });

                });

            });

            var keyupDate = 0;

            $('body').on("keyup", ".credit .cardInfo .rowElem input.creditCard", function (e) {

                keyupDate = e.timeStamp;
                var creditCard = $(this).val();
                var obj = $(this);

                if (isCreditCard(creditCard)) {

                    $.fancybox.showLoading();

                    setTimeout(function () {

                        if (keyupDate != e.timeStamp) {
                            return;
                        }

                        // Add Card Type Class
                        bindCreditCardClass(obj);

                        // get Option 
                        var getOption = $.ajax({
                            dataType: "JSON",
                            type: "GET",
                            cache: false,
                            url: titizJs.mainPath + titizJs.langCode + "/" + "Checkout" + "/" + "CreditOption",
                            data: { creditCard: creditCard, isModal: true }

                        });

                        getOption.done(function (msg) {

                            $('.cardOptionListContainer').html(msg.html);

                            $.fancybox.hideLoading();
                        });


                    }, 1000);
                }
                else {

                    // get No Option Content
                    $('.credit .cardType').hide();

                    var defaultoptionHtml = $('.hiddenCardOption').html();
                    $('.cardOptionListContainer').html(defaultoptionHtml);

                    return;
                }

            });

            break;

        case "summary":

            jQuery.validator.addMethod("isTrue", function (value, element) {

                return $(element).is(':checked'); 
            }
            , "Onaylamanız gerekmektedir.");
             
            $(".checkoutForm form").validate(); //sets up the validator
            $("#isPreSalesAgreementChecked").rules("add", "isTrue");
            $("#isAgreementChecked").rules("add", "isTrue");


            $('.approveList .item > a').on("click", function (e) {

                var content = $(this).parent().find('.agreement').html();

                e.preventDefault();
                $.fancybox.showLoading();
                $.magnificPopup.open({
                    items: {
                        src: content
                    },
                    type: 'inline' // this is default type
                });

                $.fancybox.hideLoading();

            });

             

            break;

        case "accountOrder":

            $('.approveList .item > a').on("click", function (e) {

                var content = $(this).parent().find('.agreement').html();

                e.preventDefault();
                $.fancybox.showLoading();
                $.magnificPopup.open({
                    items: {
                        src: content
                    },
                    type: 'inline' // this is default type
                });

                $.fancybox.hideLoading();

            });
            break;

        case "adminOrder":
         

            $('.orderAddress .c1 a').on("click", function (e) {
                  
                var content = $(this).parent().parent().find('.c2').html();

                e.preventDefault();
                $.fancybox.showLoading();
                $.magnificPopup.open({
                    items: {
                        src: content
                    },
                    type: 'inline' // this is default type
                });

                $.fancybox.hideLoading();


            });

            $('.approveList .item > a').on("click", function (e) {

                var content = $(this).parent().find('.agreement').html();

                e.preventDefault();
                $.fancybox.showLoading();
                $.magnificPopup.open({
                    items: {
                        src: content
                    },
                    type: 'inline' // this is default type
                });

                $.fancybox.hideLoading();

            });
          

            break;
    }
}

titizJs.empty = function () {



}

// Product
function productListAdd() {

    $('.addProduct').on("click", function (event) {
        event.preventDefault();
        var $actionBtn = $(this);
        var $li = $actionBtn.parent().parent();

        $li.find('.msg').remove();

        if ($actionBtn.hasClass("disableBtn")) {
            return;
        }

        $actionBtn.addClass("disableBtn");

        $li.find('.detailBtn').fadeOut("normal", function () {
            $li.find(".productLoader").show();
        });

        var productId = $actionBtn.attr("data-productid");

        var request = $.ajax({
            dataType: "JSON",
            type: "POST",
            cache: false,
            url: titizJs.mainPath + titizJs.langCode + "/" + "ProductList" + "/" + "AddProduct",
            data: { productId: productId },

        });

        request.done(function (msg) {

            if (msg.result == "success") {

                $li.find('.price').before(msg.msgHtml);

                $('.cartBox').remove();

                $('.searchBox').before(msg.cartHtml);
            }
            if (msg.result == "option") {

                var detailLink = $li.find('.detailBtn').attr("href");
                detailLink = msg.hostName + detailLink + "?action=optionSelect";

                redirectPage(detailLink);
            }
            if (msg.result == "error") {

                $li.find('.price').before(msg.msgHtml);
            }


            $actionBtn.removeClass("disableBtn");
            $li.find('.productLoader').hide();
            $li.find('.detailBtn').fadeIn("slow");

            $li.find('.msg').fadeIn("slow", function () {

                setTimeout(function () {

                    $li.find('.msg').fadeOut("slow");

                }, 5000);

            });

        });

        request.fail(function (jqXHR, textStatus) {

            $actionBtn.removeClass("disableBtn");
            $li.find(".productLoader").hide();
            $li.find('.detailBtn').fadeIn("slow");
        });

    });
}

// Search
function replaceContentBold(content, replaceWordList) {

    var wordList = replaceWordList;

    for (var i = 0; i < wordList.length; i++) {

        var regex = new RegExp('(' + wordList[i] + ')', 'gi');
        var matchedWord = content.match(regex);

        if (matchedWord != null && matchedWord != undefined) {

            for (var a = 0; a < matchedWord.length; a++) {

                content = content.replace(matchedWord[a], "<b>" + matchedWord[a] + "</b>");

            }
        }

    }


    return content;

}

// Shared
function redirectPage(url) {
    window.location.href = url;
}

// address Validation
function addressFormStart() {

    $(".addressPhone").mask("(999)999-99-99");
    $(".addressPost").mask("99999");
    $(".addressTc").mask("99999999999");
    $(".addressTax").mask("9999999999");

    var form = $(".addressForm form");
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);


    var tcErrorMessage = $('.addressTc').attr("data-required");

    if ($('.addressForm').attr("data-ispersonal") == "True") {

        $(".addressTc").rules("add", {
            required: true,
            messages: {
                required: tcErrorMessage

            }
        });

        $(".addressCompanyName").rules("remove", "required");
        $(".addresstaxOffice").rules("remove", "required");
        $(".addressTax").rules("remove", "required");

    }
    else {

        $(".addressTc").rules("remove", "required");

        var companyNameErrorMessage = $('.addressCompanyName').attr("data-required");
        $(".addressCompanyName").rules("add", {
            required: true,
            messages: {
                required: companyNameErrorMessage

            }
        });

        var taxOfficeErrorMessage = $('.addresstaxOffice').attr("data-required");
        $(".addresstaxOffice").rules("add", {
            required: true,
            messages: {
                required: taxOfficeErrorMessage

            }
        });

        var taxNoErrorMessage = $('.addressTax').attr("data-required");
        $(".addressTax").rules("add", {
            required: true,
            messages: {
                required: taxNoErrorMessage

            }
        });
    }
}

function addressPersonalToCompany() {

    $('body').on('change', 'input[name="addressItem.isPersonal"]', function () {

        var form = $(".addressForm form");
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);

        var isPesonal = $(this).val().toString();

        if (isPesonal == 'True') {

            $('.tcNoContainer').show();
            $('.companyContainer').hide();

            $('.addressForm').attr("data-ispersonal", "True");
        }
        else {
            $('.tcNoContainer').hide();
            $('.companyContainer').show();

            $('.addressForm').attr("data-ispersonal", "False");
        }

        if ($('.addressForm').attr("data-ispersonal") == "True") {

            var tcErrorMessage = $('.addressTc').attr("data-required");

            $(".addressTc").rules("add", {
                required: true,
                messages: {
                    required: tcErrorMessage
                }
            });

            $(".addressCompanyName").rules("remove", "required");
            $(".addresstaxOffice").rules("remove", "required");
            $(".addressTax").rules("remove", "required");

        }
        else {

            $(".addressTc").rules("remove", "required");

            var companyNameErrorMessage = $('.addressCompanyName').attr("data-required");
            $(".addressCompanyName").rules("add", {
                required: true,
                messages: {
                    required: companyNameErrorMessage

                }
            });

            var taxOfficeErrorMessage = $('.addresstaxOffice').attr("data-required");
            $(".addresstaxOffice").rules("add", {
                required: true,
                messages: {
                    required: taxOfficeErrorMessage

                }
            });

            var taxNoErrorMessage = $('.addressTax').attr("data-required");
            $(".addressTax").rules("add", {
                required: true,
                messages: {
                    required: taxNoErrorMessage

                }
            });
        }
    });
}

function checkoutAddress() {

    // [Get] Add  Address 
    $('body').on('click', ".newAddress", function () {

        $.fancybox.showLoading();
        var userGuid = $('#userguid').val();
        var langCode = titizJs.langCode;

        var openNewAddress = $.ajax({
            dataType: "JSON",
            type: "GET",
            cache: false,
            url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressAdd",
            data: { userGuid: userGuid }

        });

        openNewAddress.done(function (msg) {

            $.fancybox.hideLoading();


            $.magnificPopup.open({
                items: {
                    src: msg.html
                },
                type: 'inline',
                closeOnBgClick: false,
                closeBtnInside: false,
                showCloseBtn: false,
                callbacks: {
                    open: function () {

                        addressFormStart();
                        addressPersonalToCompany();

                        $('body').on('click', ".addAddressForm .submitContainer .close", function () {
                            $.magnificPopup.close();
                        });


                    },
                    close: function () {

                        var pageId = $('#pageId').val();
                        var addressId = 0;

                        $(".checkoutForm").trigger({
                            type: "refreshForm",
                            pageId: pageId,
                            addressId: $('#addressId').val()
                        });

                    }

                }
            });

        });

    });

    // [Post] Add Address  
    $('body').on('click', ".addAddressForm .submitContainer .create", function () {

        $('#addForm').validate();
        var validator = $('#addForm').valid();

        if (validator) {

            var userGuid = $('.addresBoxContainer').attr("data-userguid");
            var langCode = titizJs.langCode;

            var saveNewAddress = $.ajax({
                dataType: "JSON",
                type: "POST",
                cache: false,
                url: $("#addForm").attr("action"),
                data: $("#addForm").serialize()

            });

            saveNewAddress.done(function (msg) {

                console.log(msg);

                $('.modelContainer').parent().html(msg.html);

                if (msg.addressId != 0) {

                    $('#addressId').val(msg.addressId);
                }

                addressFormStart();
                addressPersonalToCompany();

                setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
            });

        }
        else {

            return false;
        }

    });

    // [Get] Detail Address
    $('body').on('click', ".addresBox .detail", function () {

        $.fancybox.showLoading();

        var userGuid = $('#userguid').val();
        var langCode = titizJs.langCode;
        var addressId = $(this).parent().attr("data-addressid");

        var openNewAddress = $.ajax({
            dataType: "JSON",
            type: "GET",
            cache: false,
            url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressDetail",
            data: { userGuid: userGuid, addressId: addressId }

        });


        openNewAddress.done(function (msg) {

            $.fancybox.hideLoading();

            $.magnificPopup.open({
                items: {
                    src: msg.html
                },
                type: 'inline',
                closeOnBgClick: false,
                closeBtnInside: false,
                showCloseBtn: false,
                callbacks: {
                    open: function () {
                        $('body').on('click', ".submitContainer .close", function () {
                            $.magnificPopup.close();
                        });
                    }
                }

            });

        });

    });

    // [Get] Address Edit 
    $('body').on('click', ".addresBox .edit", function () {

        $.fancybox.showLoading();
        var userGuid = $('#userguid').val();

        var addressId = $(this).parent().attr("data-addressid");

        $('#addressId').val(addressId);

        var openNewAddress = $.ajax({
            dataType: "JSON",
            type: "GET",
            cache: false,
            url: titizJs.mainPath + titizJs.langCode + "/" + "Account" + "/" + "AddressEdit",
            data: { userGuid: userGuid, addressId: addressId }

        });


        openNewAddress.done(function (msg) {

            $.fancybox.hideLoading();

            $.magnificPopup.open({
                items: {
                    src: msg.html
                },
                type: 'inline',
                closeOnBgClick: false,
                closeBtnInside: false,
                showCloseBtn: false,
                callbacks: {
                    open: function () {

                        addressFormStart();
                        addressPersonalToCompany();

                        $('body').on('click', ".editAddressForm .submitContainer .close", function () {
                            $.magnificPopup.close();
                        });


                    },
                    close: function () {

                        var pageId = $('#pageId').val();

                        console.log(pageId);

                        $(".checkoutForm").trigger({
                            type: "refreshForm",
                            pageId: pageId,
                            addressId: $('#addressId').val()
                        });
                    }

                }

            });

        });

    });

    // [Post] Address Edit  
    $('body').on('click', ".editAddressForm .submitContainer .edit", function () {

        var validator = $('#editForm').valid();

        if (validator) {

            var userGuid = $('.addresBoxContainer').attr("data-userguid");
            var langCode = titizJs.langCode;

            var saveNewAddress = $.ajax({
                dataType: "JSON",
                type: "POST",
                cache: false,
                url: $("#editForm").attr("action"),
                data: $("#editForm").serialize()

            });

            saveNewAddress.done(function (msg) {

                $('.modelContainer').parent().html(msg.html);
                addressFormStart();
                addressPersonalToCompany();

                setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
            });

        }
        else {

            return false;
        }

    });

    // [Get] Address Delete 
    $('body').on('click', ".addresBox .delete", function () {

        $.fancybox.showLoading();
        var userGuid = $('#userguid').val();
        var langCode = titizJs.langCode;
        var addressId = $(this).parent().attr("data-addressid");
        $('#addressId').val(addressId);

        var openNewAddress = $.ajax({
            dataType: "JSON",
            type: "GET",
            cache: false,
            url: titizJs.mainPath + langCode + "/" + "Account" + "/" + "AddressDelete",
            data: { userGuid: userGuid, addressId: addressId }

        });


        openNewAddress.done(function (msg) {

            $.fancybox.hideLoading();

            $.magnificPopup.open({
                items: {
                    src: msg.html
                },
                type: 'inline',
                closeOnBgClick: false,
                closeBtnInside: false,
                showCloseBtn: false,
                callbacks: {
                    open: function () {

                        $('body').on('click', ".deleteAddressForm .submitContainer .close", function () {
                            $.magnificPopup.close();
                        });

                    },
                    close: function () {

                        var pageId = $('#pageId').val();

                        $(".checkoutForm").trigger({
                            type: "refreshForm",
                            pageId: pageId,
                            addressId: $('#addressId').val()
                        });

                    }

                }

            });

        });

    });

    // [Post] Address Delete  
    $('body').on('click', ".deleteAddressForm .submitContainer .delete", function () {


        var saveNewAddress = $.ajax({
            dataType: "JSON",
            type: "POST",
            cache: false,
            url: $("#deleteForm").attr("action"),
            data: $("#deleteForm").serialize()

        });

        saveNewAddress.done(function (msg) {

            $('.modelContainer').parent().html(msg.html);
            setTimeout(function () { $('.autoHide').fadeOut("slow", function () { $('.autoHide').remove(); }); }, 5000);
        });



    });
}

// Credit Card
function paymentCreditAction() {


    $('.credit .cardInfo .rowElem input[type="text"].creditCard').qtip({
        content: {
            attr: "title"
        },
        style: {
            classes: 'qtip-rounded qtip-bootstrap'

        }
    })

    $('.aboutCvv').each(function () {
        $(this).qtip({
            content: {
                text: $(this).next()
            },
            style: {
                classes: 'qtip-rounded qtip-bootstrap',
                width: 365,
                height: 240
            },
            position: {
                my: 'center left',  // Position my top left...
                at: 'center right' // at the bottom right of...

            }
        });
    });


    bindCreditCardClass($('.credit .cardInfo .rowElem input.creditCard'));

}

function isCreditCard(cardNo) {

    var regexObj = new RegExp("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13})$");
    return regexObj.test(cardNo)
}

function getCreditCardClass(cardNo) {
    if (isVisa(cardNo)) {

        return "visa";
    }

    if (isMaster(cardNo)) {

        return "master";
    }

    if (isAmex(cardNo)) {

        return "amex";
    }
}

function isVisa(cardNo) {
    var regexObj = new RegExp("^4[0-9]{12}(?:[0-9]{3})?$");
    return regexObj.test(cardNo)
}

function isMaster(cardNo) {
    var regexObj = new RegExp("^5[1-5][0-9]{14}$");
    return regexObj.test(cardNo)
}

function isAmex(cardNo) {
    var regexObj = new RegExp("^3[47][0-9]{13}$");
    return regexObj.test(cardNo)
}

function bindCreditCardClass(obj) {

    var cardNo = $(obj).val();

    if (isCreditCard(cardNo)) {

        $('.credit .cardType').hide();
        $('.credit .cardType').removeClass("visa");
        $('.credit .cardType').removeClass("amex");
        $('.credit .cardType').removeClass("master");
        var cardClass = getCreditCardClass(cardNo);
        $('.credit .cardType').addClass(cardClass);
        $('.credit .cardType').fadeIn();
    }
    else {

        $('.credit .cardType').hide();
    }
}