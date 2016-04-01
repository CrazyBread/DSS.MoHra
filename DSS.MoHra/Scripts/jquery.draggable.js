/* This script was copypasted from http://habrahabr.ru/post/193004/
   Script written by Michael Rafaylik (@rafaylik). */
$.fn.draggable = function () {
    function disableSelection() {
        return false;
    }
    $(this).mousedown(function (e) {
        var drag = $(this);
        var posParentTop = drag.parent().offset().top;
        var posParentBottom = posParentTop + drag.parent().height();
        var posOld = drag.offset().top;
        var posOldCorrection = e.pageY - posOld;
        drag.addClass('dragActive');
        var mouseMove = function (e) {
            var posNew = e.pageY - posOldCorrection;
            if (posNew < posParentTop) {
                if (drag.prev().length > 0) {
                    drag.insertBefore(drag.prev().css({ 'top': -drag.height() }).animate({ 'top': 0 }, 100));
                }
                drag.offset({ 'top': posParentTop });
            } else if (posNew + drag.height() > posParentBottom) {
                if (drag.next().length > 0) {
                    drag.insertAfter(drag.next().css({ 'top': drag.height() }).animate({ 'top': 0 }, 100));
                }
                drag.offset({ 'top': posParentBottom - drag.height() });
            } else {
                drag.offset({ 'top': posNew });
                if (posOld - posNew > drag.height() - 1) {
                    drag.insertBefore(drag.prev().css({ 'top': -drag.height() }).animate({ 'top': 0 }, 100));
                    drag.css({ 'top': 0 });
                    posOld = drag.offset().top;
                    posOldCorrection = e.pageY - posOld;
                } else if (posNew - posOld > drag.height() - 1) {
                    drag.insertAfter(drag.next().css({ 'top': drag.height() }).animate({ 'top': 0 }, 100));
                    drag.css({ 'top': 0 });
                    posOld = drag.offset().top;
                    posOldCorrection = e.pageY - posOld;
                }
            }
        };
        var mouseUp = function () {
            $(document).off('mousemove', mouseMove).off('mouseup', mouseUp);
            $(document).off('mousedown', disableSelection);
            drag.animate({ 'top': 0 }, 100, function () {
                drag.removeClass('dragActive');
            });
        };
        $(document).on('mousemove', mouseMove).on('mouseup', mouseUp).on('contextmenu', mouseUp);
        $(document).on('mousedown', disableSelection);
        $(window).on('blur', mouseUp);
    });
}