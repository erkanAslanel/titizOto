(function ($) {
    var cultures = $.cultures,
        tr = cultures.tr,
        standard = tr.calendars.standard,
        culture = cultures["tr-TR"] = $.extend(true, {}, tr, {
            name: "tr-TR",
            englishName: "Turkish (Türkiye)",
            nativeName: "Türkçe (Türkiye)",
            numberFormat: {
                currency: {
                    pattern: ["-$n", "$n"],
                    symbol: "£"
                }
            },
            calendars: {
                standard: $.extend(true, {}, standard, {
                    firstDay: 1,
                    patterns: {
                        d: "dd.MM.yy",
                        D: "dd MMMM yyyy",
                        t: "HH:mm",
                        T: "HH:mm:ss",
                        f: "dd MMMM yyyy HH:mm",
                        F: "dd MMMM yyyy HH:mm:ss",
                        M: "dd MMMM",
                        Y: "MMMM yyyy"
                    }
                })
            }
        }, cultures["tr-TR"]);
    culture.calendar = culture.calendars.standard;
})(jQuery);