
$(function () {

    //Active Admin Panel Category
    ActivePanel();

    //CkEditor
    RefreshEditor();

    //Create Url Process
    urlControl();

    //Create Url English Process
    urlControlEn();

    //File Upload
    $(".uploader").each(function () {

        uploadStart($(this));


    })

    $(".uploaderThumbnail").each(function () {


        uploadWithThumbnailStart($(this));

        if ($(this).hasClass("editMode")) {
            initJcrop($(this));
        }

    })

    //Edit Button Index
    $(".editBtn .ajx").live("click", function () {

        var filterId = "";

        if ($('#filterId').size() > 0 || $('.filterId').size() > 0) {

            filterId = $('#filterId').val();
        }



        var url = mainPath + "radmin/" + $('#controlName').val() + "/" + $(this).attr("data-action");
        var id = $(this).parent().attr('data-id');

        console.log($('#filterId').val());
        console.log(url);

        //delete Confirm Propt
        if ($(this).hasClass("delete")) {

            console.log(url);

            jConfirm('Silme işlemi gerçekleşecektir,Onaylıyor musunuz?', 'Silme İşlemi', function (r) {
                if (r) {

                    postUrlAndParameter(url, { id: id, filterId: filterId });

                }
                return;
            });
        }
        else {

            postUrlAndParameter(url, { id: id, filterId: filterId });
        }

    });

    //Operation Result Click
    clickHide();

    //Process Index DropDown
    indexOperation();

    // Create, Edit Statu
    statuDropDownChange();

    $(".disappear").fadeOut(3000, function () { $(this).remove(); });

    if ($('.showBlock').size() > 0) {

        $('.showBlock').parent().find(".clearPhoto").css("display", "block");
    }

    $('.clearPhoto').click(function () {

        var $uploader = $(this).next();
        var $parentTab = $(this).parent();

        $parentTab.find(".preview-container").removeAttr("style");
        $parentTab.find(".preview-container img").removeAttr("style");

        if ($uploader.hasClass("uploaderThumbnail")) {
            uploadWithThumbnailStart($uploader)
        }
        else {
            uploadStart($uploader);
        }

        $(this).hide();

        if ($parentTab.find('.coordinate').size() > 0) {

            $parentTab.find('.coordinate').val("");
        }
        $parentTab.find('.photoUploadContainer').fadeOut(function () { $parentTab.find('.photoUploadContainer img').attr("src", ""); });
        $parentTab.find($uploader.attr("data-dom")).val("");


        $('form').append('<input type="hidden" name="imageclear" value="clear" />');

        $('form').submit();
    });

    if ($('.sortable').size() > 0) {
        sortingWithParentId();
    }
    if ($('#sortable').size() > 0) {
        sortingDefault();
    }

    $('.switchFull').click(function () {
        $(this).parent().parent().find(".mainImageContainer > div").addClass("fullScreenMode");
        $(this).parent().find(".switchBack").fadeIn();

    });

    $('.switchBack').click(function () {

        $(this).parent().parent().find(".mainImageContainer > div").removeClass("fullScreenMode");
        $(this).parent().find(".switchFull").fadeIn();
        $(this).hide();

    });

    $(".chosen-select").chosen({ width: "95%" });

    $(".dateTr").mask("99.99.9999");
 

})

 

function uploadWithThumbnailStart(a) {

    $uploadObj = $(a);
    var param = $(a).attr('data-param');
    var antiToken = $('input[name=__RequestVerificationToken]').val();

    $uploadObj.pluploadQueue({
        runtimes: 'html5,silverlight',
        url: mainPath + 'radmin/Upload/upload',
        max_file_size: '2mb',
        unique_names: false,
        filters: [
            { title: "Image files", extensions: "jpg,gif,png" }
        ],
        multipart_params: { 'param': param, '__RequestVerificationToken': antiToken },
        // Flash settings
        flash_swf_url: mainPath + 'Areas/radmin/Content/js/uploader/plupload.flash.swf',

        // Silverlight settings
        silverlight_xap_url: mainPath + 'Areas/radmin/Content/js/uploader/plupload.silverlight.xap',
        preinit: {
            FileUploaded: function (up, files, res) {

                var targetId = $("#" + $(this)[0].settings.container).attr("data-dom");
                var $hiddenFile = $(targetId);
                $hiddenFile.val(res.response);
                $hiddenFile.parent().find('.photoUploadContainer img').attr("src", mainPath + "Download/item/" + param + "/" + res.response);
                $hiddenFile.parent().find('.photoUploadContainer').fadeIn();
                $hiddenFile.parent().find(".clearPhoto").css("display", "block");
                initJcrop(a);
            }
        }
    });


}

function uploadStart(a) {

    $uploadObj = $(a);
    var param = $(a).attr('data-param');
    $hiddenFile = $($(a).attr('data-dom'));
    var antiToken = $('input[name=__RequestVerificationToken]').val();

    $uploadObj.pluploadQueue({
        runtimes: 'html5,silverlight',
        url: mainPath + 'radmin/Upload/upload',
        max_file_size: '2mb',
        unique_names: false,
        filters: [
            { title: "Image files or Pdf", extensions: "jpg,gif,png,pdf" }
        ],
        multipart_params: { 'param': param, '__RequestVerificationToken': antiToken },
        // Flash settings
        flash_swf_url: mainPath + 'Areas/radmin/Content/js/uploader/plupload.flash.swf',

        // Silverlight settings
        silverlight_xap_url: mainPath + 'Areas/radmin/Content/js/uploader/plupload.silverlight.xap',
        preinit: {
            FileUploaded: function (up, files, res) {

                var targetId = $("#" + $(this)[0].settings.container).attr("data-dom");
                var $hiddenFile = $(targetId);
                $hiddenFile.val(res.response);
                if (res.response.indexOf(".pdf") == -1) {

                    $hiddenFile.parent().find(".clearPhoto").css("display", "block");
                    $hiddenFile.parent().find('.photoUploadContainer img').attr("src", mainPath + "Download/item/" + param + "/" + res.response);
                    $hiddenFile.parent().find('.photoUploadContainer').fadeIn();
                }
            }
        }
    });

}

//CkEditor
function RefreshEditor() {

    for (var i = 0; i < parseInt($(".ckeditor").size()) ; i++) {
        var editor = CKEDITOR.replace($(".ckeditor").eq(i).attr("id"));
        CKFinder.setupCKEditor(editor, mainPath + 'Areas/radmin/Content/js/ckfinder/');
    }
}

//CategoryTreeView
function langChangeTreeview() {

    console.log("test");

    //Seçiliyi sıfırla
    var selectedNode = $("#tree").dynatree("getSelectedNodes");

    if (selectedNode != null) {
        $.map(selectedNode, function (node) {
            node.toggleSelect();
        });
    }


    var selectedLangVal = "lang" + $('.ddlLang').val();
    //var selectedLangVal = "lang" + 1;
    $(".dynatree-node").parent().show();

    $(".dynatree-node").each(function (index) {

        if (!$(this).hasClass(selectedLangVal) && !$(this).hasClass('root')) {
            $(this).parent().hide();
        }
    });
}

//Create Url Process
function urlControl() {
    if ($('.changeUrl').size() > 0 && $('.pageUrl').size() > 0) {

        if ($('.manuelUrl').is(':checked')) {

            console.log($('.manuelUrl').val())

            urlControlAction($('.pageUrl').val());
            $('.pageUrl').removeAttr("readonly");
        }

        else {

            urlControlAction($('.changeUrl').val());
            $('.changeUrl').focusout(function () {
                urlControlAction($('.changeUrl').val());
            });
        }

        $('.manuelUrl').change(function () {

            if ($(this).is(':checked')) {

                $('.changeUrl').unbind('focusout');
                $('.pageUrl').removeAttr("readonly");

                $('.pageUrl').focusout(function () {
                    urlControlAction($('.pageUrl').val());
                });

            }
            else {
                console.log("otamatik");

                $('.pageUrl').unbind('focusout');

                $(".pageUrl").attr("readonly", "readonly");

                urlControlAction($('.changeUrl').val());

                $('.changeUrl').focusout(function () {
                    urlControlAction($('.changeUrl').val());
                });
            }
        });
    }
}

function urlControlEn() {
    if ($('.changeUrlEn').size() > 0 && $('.pageUrlEn').size() > 0) {




        if ($('.isManuelUrlEn').is(':checked')) {

            console.log("secili");

            urlControlActionEn($('.pageUrlEn').val());

            $('.pageUrlEn').removeAttr("readonly");
        }

        else {

            console.log("secili_degil");

            urlControlActionEn($('.changeUrlEn').val());

            $('.changeUrlEn').focusout(function () {
                urlControlActionEn($('.changeUrlEn').val());
            });
        }

        $('.isManuelUrlEn').change(function () {

            if ($(this).is(':checked')) {

                $('.changeUrlEn').unbind('focusout');
                $('.pageUrlEn').removeAttr("readonly");

                $('.pageUrlEn').focusout(function () {
                    urlControlActionEn($('.pageUrlEn').val());
                });

            }
            else {


                $('.pageUrlEn').unbind('focusout');

                $(".pageUrlEn").attr("readonly", "readonly");

                urlControlActionEn($('.changeUrlEn').val());

                $('.changeUrlEn').focusout(function () {
                    urlControlActionEn($('.changeUrlEn').val());
                });
            }
        });
    }
}

function urlControlAction(title) {

    var postUrl = $('.pageUrl').first().attr('data-action');

    title = $.trim(title);

    if (title != null && title != "") {

        var id = $('#primaryKey').val();
        var filterId = -1;
        if ($('#filterId').size() > 0) {
            filterId = $('#filterId').val();
        }
        console.log(postUrl);

        $.ajax({
            type: "POST",
            url: postUrl,
            data: { id: id, title: title, filterId: filterId },
            success: function (data, textStatus, jqXHR) {

                var resultArray = data.split("|");

                if (resultArray.length == 2) {

                    var isValid = resultArray[0];
                    var url = resultArray[1];

                    $('.pageUrl').first().attr("style", isValid);
                    $('.pageUrl').first().val(url);

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

                console.log(jqXHR);
            }
        })
    }

    else {
        $('.pageUrl').first().attr("style", "");
        $('.pageUrl').first().val("");
    }
}

function urlControlActionEn(title) {

    var postUrl = $('.pageUrlEn').first().attr('data-action');

    title = $.trim(title);

    if (title != null && title != "") {

        var id = $('#primaryKey').val();
        var filterId = -1;
        if ($('#filterId').size() > 0) {
            filterId = $('#filterId').val();
        }

        $.ajax({
            type: "POST",
            url: postUrl,
            data: { id: id, title: title, filterId: filterId },
            success: function (data, textStatus, jqXHR) {

                var resultArray = data.split("|");

                if (resultArray.length == 2) {

                    var isValid = resultArray[0];
                    var url = resultArray[1];

                    $('.pageUrlEn').first().attr("style", isValid);
                    $('.pageUrlEn').first().val(url);

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

                console.log(jqXHR);
            }
        })
    }

    else {
        $('.pageUrl').first().attr("style", "");
        $('.pageUrl').first().val("");
    }
}

//Active Admin Panel Category
function ActivePanel() {
    var activePanel = $('#activeController').val();

    $('.leftNav ul li a[data-controller="' + activePanel + '"]').addClass("active");
}

//Operation
function operationSuccess() {
    removeMessage();
    resetTable();

    var msg = "<div class='nNoteAbsolute nSuccess hideit' style='display:none'><p><strong>BAŞARILI: </strong>İşleminiz gerçekleşti.</p>  </div>  ";
    $('body').prepend(msg);
    $('.nNoteAbsolute').fadeIn();

    var t = setTimeout(function () {
        $('.proccess select').val("-1");
        $(".proccess div.jqTransformSelectWrapper span").text("İşlem Seçiniz");
    }, 1000)



    hideMessage();
}

function operationError() {
    removeMessage();
    var msg = "<div class='nNoteAbsolute nFailure hideit' style='display:none'><p><strong>HATA: </strong>Beklenmedik hata meydana geldi.</p></div>";
    $('body').prepend(msg);
    $('.nNoteAbsolute').fadeIn();
    hideMessage();

}

function hideMessage() {
    var t = setTimeout(function () { fadeAndRemove(); }, 4000)
}

function removeMessage() {
    $('.hideit').remove();
}

function fadeAndRemove() {

    $('.hideit').fadeOut(300, function () { $(this).remove(); });

}

function resetTable() {

    oTable = $('#indexTable').dataTable({
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "sDom": '<""f>t<"F"lp>'
    });

    $(".nosortable").find(".DataTables_sort_wrapper").attr("class", "");
    $(".nosortable").find(".DataTables_sort_icon").remove();
}

function clickHide() {
    $('.hideit').live('click', function () {

        fadeAndRemove();
    });
}

$(".proccess select").change(function () {



    var operationValue = $(this).val()
    var url = mainPath + "radmin/" + $('#controlName').val() + "/";
    var parameter = $('form').serialize();
    var isWithFilter = $("#filterId").size() > 0;
    console.log(operationValue);
    if (operationValue == "Sil") {

        jConfirm('İşaretlediğiniz kayıtlarda silme işlemi gerçekleşecektir,Onaylıyor musunuz?', 'Silme İşlemi', function (r) {
            if (r) {

                if (isWithFilter) {
                    postUrlAndParameter(url + "setDeleteAllWithFilter", parameter);
                }
                else {
                    postUrlAndParameter(url + "setDeleteAll", parameter);
                }

            }
            return false;
        });
    }

    if (operationValue == "Pasif") {

        console.log("burda");
        if (isWithFilter) {
            postUrlAndParameter(url + "setFalseAllWithFilter", parameter);
        }
        else {
            postUrlAndParameter(url + "setFalseAll", parameter);
        }


        return false;
    }
    if (operationValue == "Aktif") {

        if (isWithFilter) {
            postUrlAndParameter(url + "setTrueAllWithFilter", parameter);
        }
        else {
            postUrlAndParameter(url + "setTrueAll", parameter);
        }

        return false;
    }

    if (operationValue == "Sırala") {

        if (isWithFilter) {
            postUrlAndParameter(url + "setOrderAllWithFilter", parameter);
        }
        else {
            postUrlAndParameter(url + "setOrderAll", parameter);
        }


        return false;
    }


    return false; //prevent default browser action

});

function postUrlAndParameter(url, parameter) {




    $.ajax({
        type: "POST",
        url: url,
        data: parameter,
        dataType: "html",
        success: function (data, textStatus, jqXHR) {
            $('.table').html(data);
            operationSuccess();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            operationError();
        }
    });




}

function statuDropDownChange() {

    $('.ddlStatu').parent().parent().find('div.jqTransformSelectWrapper ul li a').click(function () {

        console.log("test");

        var selectedVal = $(this).parent().parent().parent().parent().find('div.jqTransformSelectWrapper span').text();

        console.log(selectedVal);


        if (selectedVal == "Pasif") {

            $(this).parent().parent().parent().parent().find(".statuCheck").val("false");
            return;
        }
        if (selectedVal == "Aktif") {

            $(this).parent().parent().parent().parent().find(".statuCheck").val("true");
            return;
        }


    });
}

function initJcrop(a) {

    var $parentTab = $(a).parent();

    var heightFree = false;
    var widhtFree = false;



    if ($parentTab.find('.coordinate').attr("data-height") == "0") {

        heightFree = true;
    }

    if ($parentTab.find('.coordinate').attr("data-width") == "0") {

        widhtFree = true;
    }

    if (heightFree || widhtFree) {

        if (heightFree) {

            $parentTab.find('.preview-container').width($parentTab.find('.coordinate').attr("data-width"));
            $parentTab.find('.preview-container').height(300);

            var jcrop_api,
   boundx,
   boundy,

   // Grab some information about the preview pane
   $preview = $parentTab.find('.preview-pane'),
   $pcnt = $parentTab.find('.preview-pane .preview-container'),
   $pimg = $parentTab.find('.preview-pane .preview-container img'),

   xsize = $pcnt.width(),
   ysize = $pcnt.height();

            $parentTab.find('.target').Jcrop({
                bgOpacity: 0.3,
                onChange: function (c) {

                    var $parentTab = $(a).parent();
                    var $pimg = $parentTab.find('.preview-pane .preview-container img');
                    var $pimgParent = $parentTab.find('.preview-pane .preview-container');


                    var xsize = $parentTab.find('.coordinate').attr("data-width");
                    var ysize = c.h;
                    if (parseInt(c.w) > 0) {
                        var rx = xsize / c.w;
                        var ry = ysize / c.h;

                        $pimgParent.css({ height: c.h + "px" });

                        $pimg.css({
                            width: Math.round(rx * boundx) + 'px',
                            height: Math.round(ry * boundy) + 'px',
                            marginLeft: '-' + Math.round(rx * c.x) + 'px',
                            marginTop: '-' + Math.round(ry * c.y) + 'px'
                        });
                    }

                    var coordinate = c.toString();

                    $parentTab.find('.coordinate').val(c.x.toString() + "," + c.y.toString() + "," + c.x2.toString() + "," + c.y2.toString() + "," + c.w.toString() + "," + c.h.toString());
                    console.log("onchange");
                    console.log($parentTab.find('.coordinate').val());

                },
                onSelect: function (c) {

                    console.log($(a));
                    $pimg = $parentTab.find('.preview-pane .preview-container img');
                    $pimgParent = $parentTab.find('.preview-pane .preview-container');
                    var xsize = $parentTab.find('.coordinate').attr("data-width");
                    var ysize = c.h;
                    if (parseInt(c.w) > 0) {
                        var rx = xsize / c.w;
                        var ry = ysize / c.h;

                        $pimgParent.css({ height: c.h + "px" });

                        $pimg.css({
                            width: Math.round(rx * boundx) + 'px',
                            height: Math.round(ry * boundy) + 'px',
                            marginLeft: '-' + Math.round(rx * c.x) + 'px',
                            marginTop: '-' + Math.round(ry * c.y) + 'px'
                        });
                    }

                    var coordinate = c.toString();

                    $parentTab.find('.coordinate').val(c.x.toString() + "," + c.y.toString() + "," + c.x2.toString() + "," + c.y2.toString() + "," + c.w.toString() + "," + c.h.toString());
                    console.log("onselect");
                    console.log($parentTab.find('.coordinate').val());

                },
                minSize: [xsize, 0],

                aspectRatio: 0
            }, function () {
                // Use the API to get the real image size
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];

                // Store the API in the jcrop_api variable
                jcrop_api = this;

                if ($parentTab.find('.coordinate').val() == "" || $parentTab.find('.coordinate').val() == undefined) {
                    console.log("reset");
                    jcrop_api.animateTo([0, 0, xsize, ysize]);
                }
                else {

                    var itemCoordinate = $parentTab.find('.coordinate').val();
                    var coordinateArray = itemCoordinate.split(",");
                    jcrop_api.animateTo([coordinateArray[0], coordinateArray[1], coordinateArray[2], coordinateArray[3]]);
                }


                // Move the preview into the jcrop container for css positioning
                //$preview.appendTo(jcrop_api.ui.holder);
            });

        }

        if (widhtFree) {

            $parentTab.find('.preview-container').width(200);
            $parentTab.find('.preview-container').height($parentTab.find('.coordinate').attr("data-height"));

            var jcrop_api,
   boundx,
   boundy,

   // Grab some information about the preview pane
   $preview = $parentTab.find('.preview-pane'),
   $pcnt = $parentTab.find('.preview-pane .preview-container'),
   $pimg = $parentTab.find('.preview-pane .preview-container img'),

   xsize = $pcnt.width(),
   ysize = $pcnt.height();

            $parentTab.find('.target').Jcrop({
                bgOpacity: 0.3,
                onChange: function (c) {

                    var $parentTab = $(a).parent();
                    var $pimg = $parentTab.find('.preview-pane .preview-container img');
                    var $pimgParent = $parentTab.find('.preview-pane .preview-container');


                    var xsize = c.w;
                    var ysize = $parentTab.find('.coordinate').attr("data-height");
                    if (parseInt(c.w) > 0) {
                        var rx = xsize / c.w;
                        var ry = ysize / c.h;

                        $pimgParent.css({ width: c.w + "px" });

                        $pimg.css({
                            width: Math.round(rx * boundx) + 'px',
                            height: Math.round(ry * boundy) + 'px',
                            marginLeft: '-' + Math.round(rx * c.x) + 'px',
                            marginTop: '-' + Math.round(ry * c.y) + 'px'
                        });
                    }

                    var coordinate = c.toString();

                    $parentTab.find('.coordinate').val(c.x.toString() + "," + c.y.toString() + "," + c.x2.toString() + "," + c.y2.toString() + "," + c.w.toString() + "," + c.h.toString());


                },
                onSelect: function (c) {

                    $pimg = $parentTab.find('.preview-pane .preview-container img');
                    $pimgParent = $parentTab.find('.preview-pane .preview-container');


                    var xsize = c.w;
                    var ysize = $parentTab.find('.coordinate').attr("data-height");
                    if (parseInt(c.w) > 0) {
                        var rx = xsize / c.w;
                        var ry = ysize / c.h;

                        $pimgParent.css({ width: c.w + "px" });

                        $pimg.css({
                            width: Math.round(rx * boundx) + 'px',
                            height: Math.round(ry * boundy) + 'px',
                            marginLeft: '-' + Math.round(rx * c.x) + 'px',
                            marginTop: '-' + Math.round(ry * c.y) + 'px'
                        });
                    }

                    var coordinate = c.toString();

                    $parentTab.find('.coordinate').val(c.x.toString() + "," + c.y.toString() + "," + c.x2.toString() + "," + c.y2.toString() + "," + c.w.toString() + "," + c.h.toString());
                    console.log("onchange");
                    console.log($parentTab.find('.coordinate').val());

                },
                minSize: [0, ysize],

                aspectRatio: 0
            }, function () {
                // Use the API to get the real image size
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];

                // Store the API in the jcrop_api variable
                jcrop_api = this;

                if ($parentTab.find('.coordinate').val() == "" || $parentTab.find('.coordinate').val() == undefined) {
                    console.log("reset");
                    jcrop_api.animateTo([0, 0, xsize, ysize]);
                }
                else {

                    var itemCoordinate = $parentTab.find('.coordinate').val();
                    var coordinateArray = itemCoordinate.split(",");
                    jcrop_api.animateTo([coordinateArray[0], coordinateArray[1], coordinateArray[2], coordinateArray[3]]);
                }


                // Move the preview into the jcrop container for css positioning
                //$preview.appendTo(jcrop_api.ui.holder);
            });

        }
    }

    else {

        $parentTab.find('.preview-container').width($parentTab.find('.coordinate').attr("data-width"));
        $parentTab.find('.preview-container').height($parentTab.find('.coordinate').attr("data-height"));

        var jcrop_api,
      boundx,
      boundy,

      // Grab some information about the preview pane
      $preview = $parentTab.find('.preview-pane'),
      $pcnt = $parentTab.find('.preview-pane .preview-container'),
      $pimg = $parentTab.find('.preview-pane .preview-container img'),

      xsize = $pcnt.width(),
      ysize = $pcnt.height();



        $parentTab.find('.target').Jcrop({
            bgOpacity: 0.3,
            onChange: function (c) {

                console.log($(a));
                $pimg = $parentTab.find('.preview-pane .preview-container img');
                var xsize = $parentTab.find('.coordinate').attr("data-width");
                var ysize = $parentTab.find('.coordinate').attr("data-height");
                if (parseInt(c.w) > 0) {
                    var rx = xsize / c.w;
                    var ry = ysize / c.h;

                    $pimg.css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });
                }

                var coordinate = c.toString();
                console.log(coordinate);
                $parentTab.find('.coordinate').val(c.x.toString() + "," + c.y.toString() + "," + c.x2.toString() + "," + c.y2.toString() + "," + c.w.toString() + "," + c.h.toString());

            },
            onSelect: function (c) {

                console.log($(a));
                $pimg = $parentTab.find('.preview-pane .preview-container img');
                var xsize = $parentTab.find('.coordinate').attr("data-width");
                var ysize = $parentTab.find('.coordinate').attr("data-height");
                if (parseInt(c.w) > 0) {
                    var rx = xsize / c.w;
                    var ry = ysize / c.h;

                    $pimg.css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });
                }

                var coordinate = c.toString();
                console.log(coordinate);
                $parentTab.find('.coordinate').val(c.x.toString() + "," + c.y.toString() + "," + c.x2.toString() + "," + c.y2.toString() + "," + c.w.toString() + "," + c.h.toString());

            },
            minSize: [xsize, ysize],
            aspectRatio: xsize / ysize
        }, function () {
            // Use the API to get the real image size
            var bounds = this.getBounds();
            boundx = bounds[0];
            boundy = bounds[1];

            // Store the API in the jcrop_api variable
            jcrop_api = this;

            if ($parentTab.find('.coordinate').val() == "" || $parentTab.find('.coordinate').val() == undefined) {
                console.log("reset");
                jcrop_api.animateTo([0, 0, xsize, ysize]);
            }
            else {

                var itemCoordinate = $parentTab.find('.coordinate').val();
                var coordinateArray = itemCoordinate.split(",");
                jcrop_api.animateTo([coordinateArray[0], coordinateArray[1], coordinateArray[2], coordinateArray[3]]);
            }


            // Move the preview into the jcrop container for css positioning
            //$preview.appendTo(jcrop_api.ui.holder);
        });

    }


    // Create variables (in this scope) to hold the API and image size



}

function sortingWithParentId() {
    $('.sortable').nestedSortable({
        handle: 'div',
        items: 'li',
        toleranceElement: '> div',
        protectRoot: false,
        isAllowed: function (item, parent) {

            if ($(parent).attr("id") != undefined) {

                var itemId = $(parent).attr("id").replace("list_", "");
                var selectedItemId = $(item).attr("data-parentId");

                if (itemId == selectedItemId) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {

                var selectedItemId2 = $(item).attr("data-parentId");
                if (selectedItemId2 == 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
        },
        stop: function (event, ui) { console.log("stop"); $('#sortArray').val(JSON.stringify($(this).nestedSortable('toArray'))); }

    });
    $('.submitForm').click(function (event) {
        event.preventDefault();

        $('#sortArray').val(JSON.stringify($('.sortable').nestedSortable('toArray')));

        $('form').submit();

    });


}

function sortingDefault() {

    $("#sortable").sortable();
    $("#sortable").disableSelection();



    $('.submitForm').click(function (event) {
        event.preventDefault();
        var orderList = [];
        $('.ui-state-default').each(function () {

            var dataId = $(this).attr("data-id");

            console.log(dataId);

            orderList.push({ dataId: dataId });
            console.log(JSON.stringify(orderList));

            $('#sortArray').val(JSON.stringify(orderList));
        })


        $('form').submit();

    });


}

function indexOperation() {

    $(".proccess select").change(function () {



        var operationValue = $(this).val()
        var url = mainPath + "radmin/" + $('#controlName').val() + "/";
        var parameter = $('form').serialize();
        var isWithFilter = $("#filterId").size() > 0;
        console.log(operationValue);
        if (operationValue == "Sil") {

            jConfirm('İşaretlediğiniz kayıtlarda silme işlemi gerçekleşecektir,Onaylıyor musunuz?', 'Silme İşlemi', function (r) {
                if (r) {

                    if (isWithFilter) {
                        postUrlAndParameter(url + "setDeleteAllWithFilter", parameter);
                    }
                    else {
                        postUrlAndParameter(url + "setDeleteAll", parameter);
                    }

                }
                return false;
            });
        }

        if (operationValue == "Pasif") {

            console.log("burda");
            if (isWithFilter) {
                postUrlAndParameter(url + "setFalseAllWithFilter", parameter);
            }
            else {
                postUrlAndParameter(url + "setFalseAll", parameter);
            }


            return false;
        }
        if (operationValue == "Aktif") {

            if (isWithFilter) {
                postUrlAndParameter(url + "setTrueAllWithFilter", parameter);
            }
            else {
                postUrlAndParameter(url + "setTrueAll", parameter);
            }

            return false;
        }

        if (operationValue == "Sırala") {

            if (isWithFilter) {
                postUrlAndParameter(url + "setOrderAllWithFilter", parameter);
            }
            else {
                postUrlAndParameter(url + "setOrderAll", parameter);
            }


            return false;
        }


        return false; //prevent default browser action

    });

}