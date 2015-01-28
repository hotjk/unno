// jQuery Validation default does not check hidden input elements.
$.validator.setDefaults({
    onkeyup: false,
    onblur: true,
    focusCleanup: true,
    ignore: ".js-unno-ignore-validate"
});


$(function () {
    // Patch for jQuery Validation
    // jQuery unobtrusive validation does not accept hyphen for date formatting in safari/IE10.
    $.validator.methods.date = function (value, element) {
        var s = value.replace(/\-/g, '/');
        return this.optional(element) || !/Invalid|NaN/.test(new Date(s));
    };
});
