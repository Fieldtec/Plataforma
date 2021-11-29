(function ($) {

    $.fn.SubmitForm = function (options) {

        var _$this = $(this);

        _$this.on('submit', function (e) {
            var isValid = $(this).ValidForm(options);

            if (!isValid) {
                e.preventDefault();
            } else {
                $.load();
            }
        });
    }

    $.fn.SetJsonData = function (json) {
        var $fields = $(this).find('input, select, textarea');

        for (var i = 0; i < $fields.length; i++) {
            var $field = $($fields[i]),
                name = $field.attr("name");
            if (name) {
                if (json[name] !== null) {
                    if ($field.hasClass("data")) {
                        $field.val(FormatarData(json[name]));
                    } else if ($field.hasClass("numerico") || $field.hasClass("inteiro")) {
                        $field.val(json[name]);
                    } else if (($field.hasClass("decimal") || $field.hasClass("decimal3") || $field.hasClass("decimal4") || $field.hasClass("money") || $field.hasClass('decimalNegative'))) {
                        $field.val(formataMoeda(json[name], 2, ',', '.'));
                    } else {
                        $field.val(json[name]);
                    }
                } else {
                    $field.val([""]);
                }
            }                
        }
    }

    $.fn.GetJsonData = function () {
        var $fields = $(this).find("input, select, textarea"),
            request = {};

        for (var i = 0; i < $fields.length; i++) {
            var $field = $($fields[i]),
                name = $field.attr("name");

            if (name)
                request[name] = $field.getValue();

            if (name === "id" && request[name] === null)
                request[name] = 0;
        }

        return request;
    }

    $.fn.ResetForm = function () {
        $(this).validate().resetForm();
        $(this).find('input, select, textarea').removeClass('is-invalid');
        $(this).find("input, select, textarea").val("");
    }

    $.fn.ValidForm = function (options) {

        var _$this = $(this),
            settings = $.extend({}, options),
            $fields = _$this.find('input, select, textarea');

        $fields.removeClass('is-invalid');

        $fields.off('blur');

        $fields.on('blur', function (e) {
            if ($(this).val()) {
                $(this).removeClass("is-invalid");
                if ($(this).parent('.input-group'))
                    $(this).parents('.form-group').find('#' + $(this).attr('name') + '-error').remove();
                else
                    $(this).next().remove();
            }
        });

        var ElementsInvalid = new Array();

        if (!_$this.valid()) {
            var validator = _$this.validate();
            for (var i = 0; i < validator.errorList.length; i++) {
                var o = new Object({
                    Id: this.attr('id'),
                    Field: $(validator.errorList[i].element).attr('id'),
                    Message: validator.errorList[i].message
                });
                ElementsInvalid.push(o);
            }
        }

        for (var i = 0; i < ElementsInvalid.length; i++) {
            FillMessageSubmit(ElementsInvalid[i].Field, ElementsInvalid[i].Message, ElementsInvalid[i].Id);
        }

        if (ElementsInvalid.length > 0) {

            $(this).AlertBox({
                TitleMessage: 'PREENCHIMENTO INVÁLIDO!',
                BodyMessage: 'Preencha os campos em vermelho corretamente!'
            }, function () {
                if (ElementsInvalid.length > 0) {
                    var field = ElementsInvalid[0];
                    $('#' + field.Id).find("[name='" + field.Field + "']").focus();
                }
            });

            return false;
        }
        return true;
    }

    /*FUNÇÃO PARA VALIDAÇÃO PADRÃO DE CAMPO APENAS DE DATA */
    $.fn.ValidDate = function (options) {

        var settings = $.extend({
            mask: "99/99/9999",
            required: false,
            width: '80px',
            useDatepicker: false,
            readonlyDatepicker: true,
            msg: 'Data inválida!',
            maiorQueAtual: true
        }, options);


        if (options.useDatepicker) {
            this.datepicker({
                dateFormat: (options.mask == "99/99" ? "dd/mm" : options.mask == "99" ? "dd" : "dd/mm/yy"),
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                nextText: 'Próximo',
                prevText: 'Anterior'
            });
            this.attr('readonly', options.readonlyDatepicker);
        }

        //SetLengthComponent(this, settings.width);

        var PossibleFormts = new Array("99/99", "99/99/9999", "99/99/9999 99:99", "99/99/9999 99:99:99");
        if (PossibleFormts.indexOf(settings.mask) < 0) settings.mask = "99/99/9999";

        this.focus(function (e) {
            $(this).mask(settings.mask);
        });

        this.change(function (e) {
            FillMessage($(this), IsDate($(this).val(), settings.required, settings.maiorQueAtual), settings.msg);
        });
    };

    /*FUNÇÃO PARA VALIDAÇÃO PADRÃO DE CAMPO E-MAIL*/
    $.fn.ValidEmail = function (options) {

        var settings = $.extend({
            required: false,
            width: '',
            valblank: false,
            msg: 'E-mail inválido!'
        }, options);

        if (settings.valblank) this.val('');

        SetLengthComponent(this, settings.width);

        this.blur(function (e) {
            FillMessage($(this), IsEmail($(this).val(), settings.required), settings.msg);
        });
    }




    /*FUNÇÃO PARA VALIDAÇÃO PADRÃO DE CAMPOS CPF*/
    /*LIBERA É PARA CASO TENHA Q LIBERAR UM CPF PADRÃO SEM VERIFICAÇÃO, NESSE CASO SE FALSE O VALOR 999.999.999-99 NÃO É VERIFICADO*/
    $.fn.ValidCPF = function (options, bloqueiatodos) {
        var settings = $.extend({
            mask: "999.999.999-99",
            required: false,
            width: '135px',
            msg: 'CPF inválido!'
        }, options);


        SetLengthComponent(this, settings.width);
        var PossibleFormts = new Array("999.999.999-99", "999999999-99", "99999999999");
        if (PossibleFormts.indexOf(settings.mask) < 0) settings.mask = "999.999.999-99";

        this.focus(function (e) {
            $(this).mask(settings.mask);
        });

        this.blur(function (e) {
            if (bloqueiatodos == true) {
                FillMessage($(this), IsCPF($(this).val(), settings.required), settings.msg);
            } else {
                if ($(this).val() != "999.999.999-99") {
                    FillMessage($(this), IsCPF($(this).val(), settings.required), settings.msg);
                }
            }

        });
    }

    /*FUNÇÃO PARA VALIDAÇÃO PADRÃO DE CAMPOS CPF*/
    $.fn.ValidCNPJ = function (options) {
        var settings = $.extend({
            mask: "99.999.999/9999-99",
            required: false,
            width: '175px',
            msg: 'CNPJ inválido!'
        }, options);


        SetLengthComponent(this, settings.width);
        var PossibleFormts = new Array("99.999.999/9999-99", "99999999/9999-99", "999999999999-99", "99999999999999");
        if (PossibleFormts.indexOf(settings.mask) < 0) settings.mask = "99.999.999/9999-99";

        this.focus(function (e) {
            $(this).mask(settings.mask);
        });

        this.blur(function (e) {
            FillMessage($(this), IsCNPJ($(this).val(), settings.required), settings.msg);
        });
    }

    /*FUNÇÃO PARA VALIDAÇÃO PADRÃO DE CAMPOS CEP*/
    $.fn.ValidCEP = function (options) {
        var settings = $.extend({
            mask: "99.999-999",
            required: false,
            width: '100px',
            msg: 'CEP inválido!'
        }, options);


        SetLengthComponent(this, settings.width);
        var PossibleFormts = new Array("99.999-999", "99999-999", "99999999");
        if (PossibleFormts.indexOf(settings.mask) < 0) settings.mask = "99.999-999";

        this.focus(function (e) {
            $(this).mask(settings.mask);
        });

        this.blur(function (e) {
            FillMessage($(this), IsCEP($(this).val(), settings.required), settings.msg);
        });
    }

    $.fn.ValidCNPJOrCPF = function (options) {
        var _this = $(this),
            settings = $.extend({
                required: true,
                msg: 'CPF/CNPJ inválido!'
            }, options);


        var maskOptions = {
            onKeyPress: function (cpf, ev, el, op) {
                var masks = ['000.000.000-000', '00.000.000/0000-00'];
                _this.mask((cpf.length > 14) ? masks[1] : masks[0], op);
            }
        }

        _this.length > 11 ? _this.mask('00.000.000/0000-00', maskOptions) : _this.mask('000.000.000-00#', maskOptions);

        //var mask = function (val) {
        //    return val.replace(/\D/g, '').length === 14 ? '00.000.000/0000-00' : '000.000.000-009';
        //}

        //this.mask(mask, {
        //    onKeyPress: function (val, e, field, options) {
        //        field.mask(mask.apply({}, arguments), options);
        //    }
        //});

        this.change(function (e) {
            var isValid = IsCPF($(this).val(), settings.required) || IsCNPJ($(this).val(), settings.required);
            FillMessage($(this), isValid, settings.msg);
        });

    }

    /*FUNÇÃO PARA VALIDAÇÃO PADRÃO DE CAMPOS TELEFONE*/
    $.fn.ValidTelephone = function (options) {
        var settings = $.extend({
            required: false,
            msg: 'Telefone inválido!'
        }, options);

        var maskTelefone = function (val) {
            return val.replace(/\D/g, '').length === 11 ? '(00)00000-0000' : '(00)0000-00009';
        }

        this.mask(maskTelefone, {
            onKeyPress: function (val, e, field, options) {
                field.mask(maskTelefone.apply({}, arguments), options);
            }
        });

        this.change(function (e) {
            FillMessage($(this), IsTelephone($(this).val(), settings.required), settings.msg);
        });
    }

    /*FUNÇÃO PARA VALIDAR HORA*/
    $.fn.ValidTime = function (options) {
        var settings = $.extend({
            mask: "99:99",
            required: false,
            width: '80px',
            msg: 'Hora inválida!'
        }, options);


        SetLengthComponent(this, settings.width);
        var PossibleFormts = new Array("99:99");
        if (PossibleFormts.indexOf(settings.mask) < 0) settings.mask = "99:99";

        this.focus(function (e) {
            $(this).mask(settings.mask);
        });

        this.blur(function (e) {
            var res = $(this).val().split(":");
            FillMessage($(this), HourValid(res[0]), settings.msg);
            FillMessage($(this), MinuteValid(res[1]), settings.msg);
        });
    }


    /*FUNÇÃO PARA PROGRESS BAR PERSONALIZADO */
    $.fn.ProgressBar = function (options) {

        var settings = $.extend({
            ArrayClass: null,
            TimeOut: 500,
            ValueProgress: 1,
            MaxValue: 111,
            AutomaticIncrement: true,
            id: null
        }, options);



        if (settings.ArrayClass == null) settings.ArrayClass = new Array('bar', 'bar', 'bar');
        settings.id = $(this).attr('id');

        $("#" + settings.id + " .bar").removeClass(settings.ArrayClass[2]).addClass(settings.ArrayClass[0]);

        var handler = window.setInterval(function () {

            if (settings.ValueProgress == settings.MaxValue) {
                window.clearInterval(handler);
                return;
            }
            if (settings.ValueProgress == Math.round((40 * settings.MaxValue) / 100)) {
                $("#" + settings.id + " .bar").removeClass(settings.ArrayClass[0]).addClass(settings.ArrayClass[1]);
            }

            if (settings.ValueProgress == Math.round((80 * settings.MaxValue) / 100)) {
                $("#" + settings.id + " .bar").removeClass(settings.ArrayClass[1]).addClass(settings.ArrayClass[2]);
            }

            // $(this).css('width', settings.ValueProgress + "%");
            $("#" + settings.id + " .bar").css({ "width": settings.ValueProgress + "%" });

            if (settings.AutomaticIncrement) {
                settings.ValueProgress = settings.ValueProgress + 1;
                //alert(settings.ValueProgress);
            }
        }, 500);
    }

    /*FUNÇÃO PARA AJUSTAR NÚMEROS INTEIROS*/
    $.AdjustmentInteger = function (Value) {
        return Number(Value.replace(/[^0-9\.]+/g, ""));
    };
    /*--------------------------------------*/
   

    /*FUNÇÕES AUXILIARES*/
    function FillMessage(Element, Retorno, ErrorMessage) {
        if (!Retorno) {
            Element.addClass("is-invalid");
            Element.val('');
            MostraAlerta(ErrorMessage, "Atenção", TipoAlerta.Danger, Element, true, true);
        }
        else {
            Element.removeClass("is-invalid");
        }
    }

    function FillMessageSubmit(elementName, errorMessage, formId) {
        //var $field = $("#" + formId).find("[name='" + elementName + "']"),
        var $field = $("#" + formId).find("#" + elementName),
            _idField = $field.attr("id"),
            $errorDiv = $("#" + _idField + '-error');

        $field.addClass("is-invalid");
        $errorDiv.addClass("invalid-feedback");
        if ($field.parents(".input-group").length) {
            $errorDiv.insertAfter($field.parents(".input-group").find(".input-group-append"));
            $errorDiv.css("display", "inline-block");
        }
        else if ($field.parents('.selectize-control').length) {
            //if ($field.parents('.input-group').length) {

            //} else {
                $errorDiv.insertAfter($field.parents(".selectize-control"));
                $errorDiv.css("display", "inline-block");
            //}
        }

        //$("#" + Id).find("[name='" + Element + "']").after("<div class='invalid-feedback'>" + ErrorMessage + "</div>");
        //$("#" + Element).attr('title', ErrorMessage);
        //$("#" + Element).tooltip();
        //$("#" + Element).val('');
    }

    function SetLengthComponent(Element, Length) {
        if (Length.length > 0) Element.css({ 'width': Length });
    }

    function SetMaxLengthComponent(Element, Length) {
        Element.attr('maxlength', Length);
    }

    function IsNormaly(Value, Required, MinLength) {
        if (((Value == '') || (Value.length < MinLength)) && (Required)) return false;
        return true;
    }

    function IsSelect(Value, Required) {
        if ((Value == '-1') && (Required)) return false;
        return true;
    }

    /*FUNCÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO NUMÉRICO */
    function IsNumber(value, required) {
        if ((isNaN(value)) || ((required) && (value == ""))) {
            return false;
        }
        return true;
    }

    function OnlyNumber(e) {
        var key = (window.event) ? event.keyCode : e.which;
        if ((key > 47 && key < 58)) return true;
        else {
            if (key == 8 || key == 0) return true;
            else return false;
        }
    }
    /*FIM DE FUNCÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO NUMÉRICO */

    /*FUNCÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO DATA */
    function IsDate(value, required, anoMaiorQueAtual) {
        try {
            if (value != '' && value != '__/__/____') {
                $.trim(value);
                value = value.split('_').join(' ');

                var Day = null; var Month = null; var Year = null; var Hour = null; var Minute = null; var Second;

                var DayMonthYear = new Array();
                DayMonthYear = value.split("/");

                if (DayMonthYear.length == 3) {
                    Day = DayMonthYear[0];
                    Month = DayMonthYear[1];
                    Year = DayMonthYear[2];

                    return (YearValid(Year, anoMaiorQueAtual) && MonthValid(Month) && DayValid(Day, Month, Year));

                } else {
                    Day = DayMonthYear[0];
                    Month = DayMonthYear[1];
                    Year = DayMonthYear[2];

                    var HoursMinutesSeconds = DayMonthYear[2].split(":");
                    if (HoursMinutesSeconds.length > 1) {
                        Year = HoursMinutesSeconds[0].split(" ")[0];
                        Hour = HoursMinutesSeconds[0].split(" ")[1];
                        Minute = HoursMinutesSeconds[1];
                        if (HoursMinutesSeconds.length > 2) {
                            Second = HoursMinutesSeconds[2];
                            return (YearValid(Year, anoMaiorQueAtual) && MonthValid(Month) && DayValid(Day, Month, Year) && HourValid(Hour) && MinuteValid(Minute) && SecondValid(Second));
                        }
                        return (YearValid(Year, anoMaiorQueAtual) && MonthValid(Month) && DayValid(Day, Month, Year) && HourValid(Hour) && MinuteValid(Minute));
                    }
                    return (YearValid(Year, anoMaiorQueAtual) && MonthValid(Month) && DayValid(Day, Month, Year));
                }
            } else if (required) return false;
            else return true;
        } catch (exception) {
            return false;
        }
    }

    function SecondValid(Second) {
        if ((Second >= 0) && (Second <= 60)) return true;
        return false;
    }

    function MinuteValid(Minute) {
        if ((Minute >= 0) && (Minute <= 60)) return true;
        return false;
    }

    function HourValid(Hour) {
        if ((Hour >= 0) && (Hour <= 23)) return true;
        return false;
    }

    function YearValid(Year, anoMaiorQueAtual) {
        if (Year.length == 4 && Year > 1900) {
            if (Year > new Date().getFullYear())
                return anoMaiorQueAtual;
            else
                return true
        }
        return false;
    }

    function MonthValid(Month) {
        if ((Month > 0) && (Month < 13)) return true;
        return false;
    }

    function DayValid(Day, Month, Year) {
        var Months30 = new Array('04', '06', '09', '11');
        if ((Day >= 1) && (Day <= 28)) return true;

        if ((Day >= 29) && (Day <= 31)) {
            if (Month == 2) {
                if (IsLeapYear(Year)) {
                    if (Day > 29) return false; else return true;
                } else {
                    return false;
                }
            }

            if (!((Months30.indexOf(Month) >= 0) && (Day > 30))) return true;

        }
        return false;
    }

    function IsLeapYear(Year) {
        if (Year % 4 == 0 && (Year % 100 != 0 || Year % 400 == 0)) {
            return true;
        }
        return false;
    }
    /*FIM DE FUNCÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO DATA */

    /*FUNCÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO EMAIL */
    function IsEmail(emailAddress) {
        var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
        return pattern.test(emailAddress);
    };
    /*FIM DE FUNCÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO EMAIL */

    /*FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO DECIMAL */
    function IsDecimal(value, required, symbol) {
        try {
            value = value.split(symbol).join('');
            value = value.split(',').join('');
            value = value.split('.').join('');
            if (!(IsNumber(value, required))) return false;
            var number = Number(value.replace(/[^0-9\.]+/g, ""));
            if ((number == 0) && (required)) return false;
            return true;
        } catch (e) {
            return false;
        }
    }
    /*FIM FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO DECIMAL */

    /*FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO CPF */
    function IsCPF(Value, Required) {

        exp = /\.|\-/g
        Value = Value.toString().replace(exp, "");
        Value = Value.split('_').join('');

        var OtherInvalidCPFs = new Array("00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666"
            , "77777777777", "88888888888", "99999999999");

        if (OtherInvalidCPFs.indexOf(Value) >= 0) return false;

        if ((Value == '') && (!(Required))) return true;

        var DigitTyped = eval(Value.charAt(9) + Value.charAt(10));
        var sum1 = 0, sum2 = 0;
        var vlr = 11;

        //for (i = 0; i < 9; i++) {
        //    sum1 += eval(Value.charAt(i) * (vlr - 1));
        //    sum2 += eval(Value.charAt(i) * vlr);
        //    vlr--;
        //}
        //sum1 = (((sum1 * 10) % 11) == 10 ? 0 : ((sum1 * 10) % 11));
        //sum2 = (((sum2 + (2 * sum1)) * 10) % 11);

        for (i = 0; i < 9; i++) {
            sum1 += eval(Value.charAt(i) * (vlr - 1));
            vlr--;
        }

        vlr = 11;
        for (i = 0; i < 10; i++) {
            sum2 += eval(Value.charAt(i) * vlr);
            vlr--;
        }

        sum1 = ((((sum1 * 10) % 11) == 10) || (((sum1 * 10) % 11)) == 11) ? 0 : ((sum1 * 10) % 11);
        sum2 = ((((sum2 * 10) % 11) == 10) || (((sum2 * 10) % 11) == 11)) ? 0 : ((sum2 * 10) % 11);

        var DigitGenerated = (sum1 * 10) + sum2;
        if (DigitGenerated != DigitTyped)
            return false;

        return true;
    }

    /*FIM FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO CPF */

    /*FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO CNPJ */
    function IsCNPJ(Value, Required) {

        var ValidArray = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);

        var Dig1 = new Number;
        var Dig2 = new Number;

        exp = /\.|\-|\//g

        Value = Value.toString().replace(exp, "");
        Value = Value.split('_').join('');

        if ((Value == '') && (!(Required))) return true;

        var Digit = new Number(eval(Value.charAt(12) + Value.charAt(13)));

        for (i = 0; i < ValidArray.length; i++) {
            Dig1 += (i > 0 ? (Value.charAt(i - 1) * ValidArray[i]) : 0);
            Dig2 += Value.charAt(i) * ValidArray[i];
        }
        Dig1 = (((Dig1 % 11) < 2) ? 0 : (11 - (Dig1 % 11)));
        Dig2 = (((Dig2 % 11) < 2) ? 0 : (11 - (Dig2 % 11)));

        if (((Dig1 * 10) + Dig2) != Digit)
            return false;

        return true;

    }

    /*FIM FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO CNPJ */

    /*FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO CEP */
    function IsCEP(Value, Required) {
        exp = /\.|\-/g
        Value = Value.toString().replace(exp, "");
        Value = Value.split('_').join('');

        if ((Value == '') && (!(Required))) return true;

        if (Value.length != 8) return false;

        return true;
    }

    /*FIM FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO CEP */

    /*FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO TELEFONE */
    function IsTelephone(Value, Required) {
        Value = Value.replace(/\D/g, '');
        Value = Value.split('_').join('');
        if ((Value == '') && (!(Required))) return true;
        if ((Value.length != 10) && (Value.length != 11)) return false;
        return true;
    }
    /*FIM FUNÇÕES AUXILIARES PARA VALIDAÇÃO PADRÃO DE CAMPO TELEFONE */

    /*Função auxiliar para pegar o Valor de qualquer elemento*/
    function getValue(propName, objDom) {
        if (!objDom) {
            return null;
        }

        var value = objDom.val();

        /*Validações para retornar o val()*/
        if (objDom.hasClass("date")) {
            if (objDom.datepicker) {
                value = objDom.datepicker("getDate");
            }
        }

        return value;
    };

    /*--------------------------------------------------------------------------------------------*/

    /* FIM DE FUNÇÕES AUXILIARES */

    /*extendendo funções do jQuery*/
    jQuery.extend({
        isNull: function (obj) {
            if (obj == null || obj == undefined) {
                return true;
            } else if (((typeof obj) == "string" && $.trim(obj) == "") || (obj == '__/__/____')) {
                return true;
            } else if (isNaN(obj) && (typeof obj) == "number") {
                return true
            }

            return false;
        },
        newDate: function (date) {
            return new Date(date.split("/")[2], parseInt(date.split("/")[1]) - 1, date.split("/")[0]);
        },
        rgType: function (obj) {
            if ((typeof obj.Serie) == "undefined") { //Se não tiver Série é porque é ABCZ.ProdutoOT
                if ($.isNull(parseInt(obj.Rgd[5]))) {
                    return "S"; //Série
                } else if ($.trim(obj.Rgd) != "") {
                    return "D"; //RGD
                } else {
                    return "N"; //RGN apenas
                }
            }
        },
        rg: function (obj) {
            if ((typeof obj.Serie) == "undefined") { //Se não tiver Série é porque é ABCZ.ProdutoOT
                if ($.isNull(parseInt(obj.Rgd[5]))) {
                    return obj.Rgd + " " + obj.Rgn; //Série
                } else if ($.trim(obj.Rgd) != "") {
                    return obj.Rgd; //RGD
                } else {
                    return obj.Rgn; //RGN apenas
                }
            } else {
                if (obj.Serie != null && obj.Serie != "") {
                    return obj.Serie + " " + obj.Rgn;
                } else if (obj.Rgd != null && obj.Rgd != "") {
                    return obj.Rgd;
                } else {
                    return obj.Rgn;
                }
            }
        }
    });

    jQuery.fn.extend({
        disable: function () {
            $(this).attr("disabled", true);
        },
        enable: function () {
            $(this).attr("disabled", false);
        }
    });

    //CONTADOR DE CARACTER
    $.fn.contaCaracteres = function (tamanho_maximo) {
        this.each(function (e) {
            elem = $(this);
            var contador = $('<span>Restando: ' + (tamanho_maximo - elem.attr("value").length) + ' caracteres.</span>');
            elem.after(contador);
            elem.data("campo_contador", contador);

            elem.keypress(function () {
                var elem = $(this);
                var campocontador = elem.data("campo_contador");

                if (elem.attr("value").length > 0 && (elem.attr("value").length > tamanho_maximo)) {
                    elem.val(elem.val().substring(0, tamanho_maximo));
                }
                campocontador.text('Restando: ' + (tamanho_maximo - elem.attr("value").length) + ' caracteres.');
            });
        });
        return this;
    };

    $('.decimal').maskMoney({ thousands: '.', decimal: ',', allowZero: true, suffix: '' });

    $('.decimalNegative').maskMoney({ thousands: '.', decimal: ',', allowZero: true, suffix: '', allowNegative: true });

    $('.decimal3').maskMoney({ thousands: '.', decimal: ',', allowZero: true, suffix: '', precision: 3 });

    $('.money').maskMoney({ thousands: '.', decimal: ',', allowZero: true, suffix: '' });

    //$('.data').mask('00/00/0000');
    $('.data').ValidDate({
        required: true,
        useDatepicker: true
    });

    $('.inteiro').mask("99999999", { selectOnFocus: true });

    $('.telefone').ValidTelephone();

    $('.cnpjCpf').ValidCNPJOrCPF();

    //var maskTelefone = function (val) {
    //    return val.replace(/\D/g, '').length === 11 ? '(00)00000-0000' : '(00)00000-0009';
    //};

    //$('.telefone').mask(maskTelefone, {
    //    onKeyPress: function (val, e, field, options) {
    //        field.mask(maskTelefone.apply({}, arguments), options);
    //    }
    //});

    var maskCpfCnpj = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00)00000-0000' : '(00)00000-0009';
    }

}(jQuery));