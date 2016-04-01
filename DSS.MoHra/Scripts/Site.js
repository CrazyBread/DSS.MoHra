// override jquery validate plugin defaults
$.validator.setDefaults({
    highlight: function (element) {
        var elem = $(element);
        elem.closest('.form-group').addClass('has-error');
        if (elem.hasClass("select2-hidden-accessible")) {
            console.log(elem.next().addClass("has-error"));
        }
    },
    unhighlight: function (element, errorClass, validClass) {
        var elem = $(element);
        elem.closest('.form-group').removeClass('has-error');
        if (elem.hasClass("select2-offscreen")) {
            $("#s2id_" + elem.attr("id") + " ul").removeClass(errorClass);
        } else {
            elem.removeClass(errorClass);
        }
    },
    errorElement: 'span',
    errorClass: 'help-block',
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});

$(function () {
    // add asterisk for required labels
    $('input,select').each(function () {
        var req = $(this).attr('data-val-required');
        if (undefined != req) {
            var label = $('label[for="' + $(this).attr('id') + '"]');
            var text = label.text();
            if (text.length > 0) {
                label.append('<span style="color:red"> *</span>');
            }
        }
    });

    // add smart selection for drop down lists
    $('select').select2({
        language: "ru"
    });

    //MarkPopupped (data-showInPopup) support
    $('#contentWrapper').on("click", "a[data-show-in-popup=true]", null, function (e) {
        e.preventDefault();
        var href = $(this).attr('href');
        var title = $(this).data('popup-title');
        var id = $(this).data('popup-id') || 'popup';
        $('#defaultModalLabel').html(title);
        $('#defaultModalBody').load(href, function (result, status) {
            if (status != 'success')
                $('#defaultModalBody').html('<p>Извините, произошла ошибка при загрузке и отображении данных. Пожалуйста, повторите попытку позднее.</p>');
            $('#defaultModal').data('title', title).data('id', id).modal();
        });
    });
});

// Ajax handlers
var MetaAjaxFormOnCompleteHandler = function (result, state, options) {
    var options = options || {};

    // form ids (need for custom error wrappers)
    var errorMessageId = '#errorMessage';
    var errorMessageWrapperId = '#errorMessageWrapper';
    if (options.hasOwnProperty('addition')) {
        errorMessageId += '_' + options.addition;
        errorMessageWrapperId += '_' + options.addition;
    }

    if (state != 'success' || !result.responseJSON) {
        // show standart error message
        $(errorMessageId).html("Извините, не удалось получить вменяемый ответ от сервера.");
        $(errorMessageWrapperId).removeClass('hide');
        return;
    }

    var self = options.hasOwnProperty('form') ? options.form : $(this);
    var response = result.responseJSON
    var modal = self.parents('.modal').eq(0);

    // special case for forms in modal -- always close popup on success
    if (modal.length > 0 && response.Success) {
        modal.modal('hide');
    } else {
        if (response.NeedReload)
            location.reload();
        if (response.NeedRedirect)
            location.replace(response.RedirectUrl);
    }

    // work with error messages
    $(errorMessageId).empty();

    if (response.ModelErrors) {
        var fValidator = self.validate();
        if (typeof (fValidator) !== 'undefined' && Object.keys(response.ModelErrors).length > 0) {
            fValidator.showErrors(response.ModelErrors);
        }
    }
    if (response.Success) {
        // clear error message and hide wrapper
        $(errorMessageId).empty();
        $(errorMessageWrapperId).addClass('hide');
    } else {
        $(errorMessageWrapperId).removeClass('hide');
        if (!response.Success && response.Message) {
            // show custom error message
            $(errorMessageId).html(response.Message);
        }
    }
}

/*
* Localized default methods for the jQuery validation plugin.
* Locale: RU
*/
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || moment(value, "DD.MM.YYYY").isValid();
    },
    number: function (value, element) {
        value = value.toString().replace(/\s+/g, '').replace(',', '.');
        return this.optional(element) || value == parseFloat(value);
    },
    min: function (value, element, param) {
        value = value.replace(' ', '').replace(',', '.');
        return this.optional(element) || value >= param;
    },
    max: function (value, element, param) {
        value = value.replace(' ', '').replace(',', '.');
        return this.optional(element) || value <= param;
    },
    range: function (value, element, param) {
        value = value.replace(/\s+/g, '').replace(',', '.');
        return this.optional(element) || (value >= param[0] && value <= param[1]);
    },
});