(function ($) {
    $.unno = {
        bindTab:function(index) {
            $('.nav-tabs a:eq(1)').tab("show");
        },
        bindDatePicker:function(container) {
            container.find('.js-unno-datepicker').datepicker({
                format: "yyyy-mm-dd", autoclose: true });
        },
        bindEvents: function () {
            var sequence = 10000;
            var date_picker_id_sequence = 0; // date-picker plugin need each date-picker have unique id.

            var nextSequence = function () {
                sequence++;
            }

            var copyValuesToModel = function (from, to) {
                $.each(from.find(':input:not(button):not(:checkbox)'), function (k, v) {
                    var $v = $(v);
                    var postfix_array = $v.attr('name').split('_');
                    var postfix = postfix_array[postfix_array.length - 1];
                    var $t = to.find(":input:not(button):not(:checkbox)[name='" + postfix + "']");
                    if ($t.length != 0) { $t.val($v.val()).change(); }
                    $t = to.find(":checkbox[name='" + postfix + "']");
                    if ($t.length != 0) { $t[0].checked = ($v.val().toLowerCase() == "true"); }
                });
                $.each(from.find(':checkbox'), function (k, v) {
                    var $v = $(v);
                    var postfix_array = $v.attr('name').split('_');
                    var postfix = postfix_array[postfix_array.length - 1];
                    var $t = to.find(":checkbox[name='" + postfix + "']");
                    $t[0].checked = $v[0].checked;
                });
            };

            var copyValuesFromModel = function (from, to) {
                $.each(from.find(':input:not(button):not(:checkbox)'), function (k, v) {
                    var $v = $(v);
                    var $t = to.find(":input:not(button):not(:checkbox)[name$='" + $v.attr('name') + "']");
                    $t.val($v.val()).change();
                });
                $.each(from.find(':checkbox'), function (k, v) {
                    var $v = $(v);
                    var $t = to.find(":checkbox[name$='" + $v.attr('name') + "']");
                    if ($t.length != 0) { $t[0].checked = $v[0].checked; }
                    var $t = to.find(":input:not(button):not(:checkbox)[name$='" + $v.attr('name') + "']");
                    $t.val($v[0].checked?"true":"false").change();
                });
            };

            var resetTemplateControl = function (control, seq) 
            {
                control.attr('name', control.attr('name').replace("[id]", seq))
                .attr('id', control.attr('name'));
                if(control.attr('name').indexOf('[id]') == -1) {
                    control.removeClass('js-unno-ignore-validate');
                }
            };

            var resetValidationControl = function (control, seq) {
                control.attr('data-valmsg-for', control.attr('data-valmsg-for').replace("[id]", seq));
            }

            var resetTemplateContainer = function (container) {
                container.removeClass("js-unno-template");
            }

            var removeValidationError = function(container) {
                container.find('.input-validation-error').removeClass('input-validation-error').end()
                    .find('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid').empty();
            }

            var resetValidator = function() {
                $("form").removeData("validator").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(document);
            }

            var addRow = function (trigger) {
                nextSequence();
                var container = $(trigger).closest('div.js-unno');
                var template = $(container.find("tr.js-unno-template")[0]).clone();
                $.each(template.find(":input:not(button)"), function (k, v) {
                    resetTemplateControl($(v), sequence);
                });
                $.each(template.find(".field-validation-valid"), function (k, v) {
                    resetValidationControl($(v), sequence);
                });
                $.unno.bindDatePicker(template);
                resetTemplateContainer(template);
                removeValidationError(template);
                container.find('tbody').append(template);
                resetValidator();
                return template;
            };

            var deleteRow = function (trigger) {
                $(trigger).closest('tr').remove();
            };

            var detailRow = function (trigger) {
                var source = $(trigger).closest('tr');
                var container = $(trigger).closest('div.js-unno');
                var target = container.children('div.modal');
                removeValidationError(target);
                copyValuesToModel(source, target);
                target.find("[name='js-unno-save-modal']").off().on('click', function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    var controls = target.find(".modal-body :input").not(":checkbox");
                    controls.validate();
                    if (!controls.valid()) {
                        return;
                    }
                    copyValuesFromModel(target, source);
                    target.modal('hide');
                });
                target.modal({ keyboard: false, backdrop: "static" }).modal('show');
            };

            var addTab = function (trigger) {
                nextSequence();
                var container = $(trigger).closest('div.js-unno');
                var tab_template = $(container.find(".js-unno-tab .js-unno-template")[0]).clone();
                var tab_link = tab_template.find('a');
                tab_link.attr("href", tab_link.attr("href").replace("[id]", sequence));
                tab_template.removeClass("js-unno-template").show();
                container.find(".js-unno-tab li a:last").parent("li").after(tab_template);

                var content_template = $(container.find(".js-unno-tab-content .js-unno-template")[0]).clone();
                content_template.attr("id", content_template.attr("id").replace("[id]", sequence));
                $.each(content_template.find(":input:not(button)"), function (k, v) {
                    resetTemplateControl($(v), sequence);
                });
                $.each(content_template.find(".field-validation-valid"), function (k, v) {
                    resetValidationControl($(v), sequence);
                });
                $.unno.bindDatePicker(content_template);
                resetTemplateContainer(content_template);
                removeValidationError(content_template);
                container.find(".js-unno-tab-content").append(content_template);
                container.find(".js-unno-tab li a:last").tab("show");
                resetValidator();
            };

            var deleteTab = function (trigger) {
                var container = $(trigger).closest('div.js-unno');
                container.find(".js-unno-tab>.active").remove();
                container.find(".js-unno-tab-content>.active").remove();
                container.find(".js-unno-tab a:eq(1)").tab("show");
            };

            $('body').on('click', '.js-unno-add-row', function (e) {
                e.stopPropagation();
                e.preventDefault();
                var row = addRow(this);
                detailRow(row);
            }).on('click', '.js-unno-delete-row', function (e) {
                e.stopPropagation();
                e.preventDefault();
                if (!confirm("确认要删除该行？")) return;
                deleteRow(this);
            }).on('click', '.js-unno-detail-row', function (e) {
                e.stopPropagation();
                e.preventDefault();
                detailRow(this);
            }).on('click', '.js-unno-add-tab', function (e) {
                e.stopPropagation();
                e.preventDefault();
                addTab(this);
            }).on('click', '.js-unno-delete-tab', function (e) {
                e.stopPropagation();
                e.preventDefault();
                if (!confirm("确认要删除当前标签页？")) return;
                deleteTab(this);
            });
        }
    };
    $(function () {
        $.unno.bindTab();
        $.unno.bindDatePicker($('body'));
        $.unno.bindEvents();
    });
}(jQuery));

