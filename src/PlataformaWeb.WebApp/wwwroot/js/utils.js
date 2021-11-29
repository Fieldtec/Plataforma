if (!Array.prototype.find) {
    Array.prototype.find = function (predicate) {
        if (this === null) {
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
        return undefined;
    };
}

if (!Array.prototype.findIndex) {
    Array.prototype.findIndex = function (predicate) {
        if (this === null) {
            throw new TypeError('Array.prototype.findIndex called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return i;
            }
        }
        return -1;
    };
}

if (!Array.prototype.filter) {
    Array.prototype.filter = function (fun/*, thisArg*/) {
        'use strict';

        if (this === void 0 || this === null) {
            throw new TypeError();
        }

        var t = Object(this);
        var len = t.length >>> 0;
        if (typeof fun !== 'function') {
            throw new TypeError();
        }

        var res = [];
        var thisArg = arguments.length >= 2 ? arguments[1] : void 0;
        for (var i = 0; i < len; i++) {
            if (i in t) {
                var val = t[i];

                // NOTE: Technically this should Object.defineProperty at
                //       the next index, as push can be affected by
                //       properties on Object.prototype and Array.prototype.
                //       But that method's new, and collisions should be
                //       rare, so use the more-compatible alternative.
                if (fun.call(thisArg, val, i, t)) {
                    res.push(val);
                }
            }
        }

        return res;
    };
}

if (!String.prototype.includes) {
    String.prototype.includes = function () {
        'use strict';
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}


(function ($) {

    //var super_val = $.fn.val;

    $.fn.getValue = function () {
        var value = null;

        if (this.val()) {
            if (this.hasClass("data")) {
                value = convertToDate(this.val());
            } else if (this.hasClass("numerico") || this.hasClass("inteiro")) {
                value = parseInt(this.val());
            } else if ((this.hasClass("decimal") || this.hasClass("decimal3") || this.hasClass("decimal4") || this.hasClass("money") || this.hasClass('decimalNegative'))) {
                value = converteToDouble(this.val());
            } else if (this.hasClass("boolean")) {
                value = this.prop('checked');
            } else {
                value = this.val();
            }
        }

        return value;
    }

    //$.fn.val = function () {

    //    if (!arguments.length) {
    //        return super_val.apply(this, arguments);
    //    }

    //    if (this.hasClass("data")) {
    //        arguments[0] = convertToDate(arguments[0]);
    //    }        

    //    return super_val.apply(this, arguments);
    //};


    $('[data-toggle="tooltip"]').tooltip({
        container: 'body'
    });

    var cont = 0;

    /*FUNÇÃO PARA IMAGEM DE LOADER QUANDO TIVER ALGUMA REQUISIÇÂO AJAX */
    $.load = function () {
        cont++;
        if (cont === 1) {
            $("#loadOverlay").remove();
            var markup = '<div id="loadOverlay">\n\
                            <div id="loadBox">\n\
                                <img src="' + raizAplicacao + '/images/carregando.gif" /><br />\n\
                                <b>AGUARDE...</b>\n\
                            </div>\n\
                        </div>';
            $(markup).hide().appendTo('body').fadeIn();

            var w = ($('#loadBox').width() / 2) * -1;
            var h = ($('#loadBox').height() / 2) * -1;

            $('#loadBox').css('left', "50%");
            $('#loadBox').css('margin-left', w + 'px');
            $('#loadBox').css('top', "50%");
            $('#loadBox').css('margin-top', h + 'px');
        }
    };

    $.load.hide = function () {
        cont--;
        if (cont < 0) {
            cont = 0;
        }
        if (cont === 0) {
            $('#loadOverlay').fadeOut(function () {
                $(this).remove();
            });
        }
    };

    $.getUrlAjax = function (Params, Url, Ass, callback) {
        $.ajax({
            type: 'GET',
            url: raizAplicacao + Url,
            data: Params ? JSON.stringify(Params) : {},
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: Ass,
            //processData: false,
            //cache: false,
            beforeSend: function () {

                $.load();
            },
            success: function (result) {
                $.load.hide();
                if (callback)
                    callback(result);
            },
            error: function (er) {
                console.log(er);
                $.load.hide();
                MostraAlerta("Erro ao enviar dados para o servidor", "Erro", TipoAlerta.Danger, $(this), false, false);
            }
        });
    };

    $.postUrlAjax = function (data, Url, Ass, callback) {
        $.ajax({
            type: 'POST',
            url: raizAplicacao + Url,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: Ass,
            data: JSON.stringify(data),
            beforeSend: function () {
                $.load();
            },
            success: function (result) {
                $.load.hide();
                if (result.status) {
                    if (callback)
                        callback(result.data);
                } else {
                    MostraAlerta(result.erros, "Atenção", TipoAlerta.Danger, $(this), false, false);
                }
            },
            error: function (er, x, y) {
                console.log(er);
                console.log(x);
                console.log(y);
                $.load.hide();
                MostraAlerta("Erro ao enviar dados para o servidor", "Erro", TipoAlerta.Danger, $(this), false, false);
            }
        });
    };

    $.postFormDataAjax = function (DataForm, Url, Ass, callback) {
        $.ajax({
            type: 'POST',
            url: raizAplicacao + Url,
            processData: false,
            contentType: false,
            cache: false,
            async: Ass,
            data: DataForm,
            beforeSend: function () {
                $.load();
            },
            success: function (result) {
                $.load.hide();
                if (callback)
                    callback(result);
            },
            error: function (er, x, y) {
                console.log(er);
                console.log(x);
                console.log(y);
                $.load.hide();
                MostraAlerta("Erro ao enviar dados para o servidor", "Erro", TipoAlerta.Danger, $(this), false, false);
            }
        });
    };


    /*FUNÇÃO PARA CRIAÇÃO DO ALERTA PERSONALIZADO */
    $.fn.AlertBox = function (options, callback) {

        var milisecond = (new Date()).getTime();

        var settings = $.extend({
            Type: 'alert-danger',
            TitleMessage: 'ATENÇÃO',
            BodyMessage: 'Algo inesperado aconteceu!',
            ArrayButtons: null
        }, options);

        if (settings.ArrayButtons == null) {
            var ListButton = new Array();
            var DefaultButton = new Object();
            DefaultButton.Content = 'OK';
            DefaultButton.action = function () {
                if (callback) {
                    callback();
                }
                $(this).remove();
            };
            DefaultButton.ClassButton = null;
            ListButton.push(DefaultButton);
            settings.ArrayButtons = ListButton;
        }

        var Parameters = ReturnParameters(settings.Type);
        var $this = $(this);

        $(MakeAlert(settings, milisecond, Parameters)).hide().appendTo('body').fadeIn(function () {
            $("a:first", $(".alertButtons")).focus();
        });

        var w = ($('#alertBox' + milisecond).width() / 2) * -1;
        var h = ($('#alertBox' + milisecond).height() / 2) * -1;

        $('#alertBox' + milisecond).css('left', "50%");
        $('#alertBox' + milisecond).css('margin-left', w + 'px');
        $('#alertBox' + milisecond).css('top', "50%");
        $('#alertBox' + milisecond).css('margin-top', h + 'px');
        $('.alertBox h1').css('background-color', Parameters.BackGroundH1);
        $('.alertBox h1').css('border-bottom', '1px solid ' + Parameters.BorderColor);

        var buttons = $('#alertBox' + milisecond + ' .btn'), i = 0;

        $.each(settings.ArrayButtons, function (name, obj) {
            if (i < 1) buttons.eq(i).focus();
            buttons.eq(i++).click(function () {
                obj.action();
                $('#alertOverlay' + milisecond).fadeOut(function () {
                    $(this).remove();
                });
                return false;
            });
        });
    }

    $.fn.LoadPartialView = function (Action, Controller, Callback) {
        $.load();
        $(this).load((raizAplicacao + Controller + "/" + Action), function (response, status, xhr) {
            if (status === "error") {
                location.href = raizAplicacao + "erro/" + xhr.status;
            } else {
                if (Callback) {
                    Callback();
                }
                $.load.hide();
            }
        });
    };

    /*FUNÇÕES PARA EXIBIÇÃO DE MENSAGEM DE ERRO*/
    function MakeAlert(settings, milisecond, Parameters) {
        var buttonHTML = '';
        var ClassButton = Parameters.ClassButton;

        for (var i = 0; i < settings.ArrayButtons.length; i++) {

            if (settings.ArrayButtons[i].ClassButton == null)
                ClassButton = Parameters.ClassButton;
            else
                ClassButton = settings.ArrayButtons[i].ClassButton;

            if (settings.ArrayButtons[i].IdButton == undefined)
                buttonHTML += ' <a class="btn ' + ClassButton + '" href="#">' + settings.ArrayButtons[i].Content + '</a> ';
            else
                buttonHTML += ' <a class="btn ' + ClassButton + '" href="#" id="' + settings.ArrayButtons[i].IdButton + '">' + settings.ArrayButtons[i].Content + '</a> ';

        }

        var bodyMessage = settings.BodyMessage;
        if ($.isArray(bodyMessage)) {
            var htmlBody = [];
            for (var i = 0; i < bodyMessage.length; i++) {
                htmlBody.push("<p>" + bodyMessage[i] + "</p>");
            }
            bodyMessage = htmlBody.join("");
        }


        var markup =
            '<div id="alertOverlay' + milisecond + '" class="alertOverlay">' +
            '   <div id="alertBox' + milisecond + '" class="alertBox">' +
            '       <h1><p class="' + Parameters.ClassText + '">' + settings.TitleMessage + '</p></h1>' +
            '       <div class="alertOverlayDiv"><p class="' + Parameters.ClassTex + ' scrollBar">' + bodyMessage + '</p></div>' +
            '       <div id="alertButtons' + milisecond + '" class="alertButtons" >' +
            buttonHTML +
            '       </div>' +
            '   </div>' +
            '</div>';

        return markup;
    }

    function ReturnParameters(Type) {
        var Styles = new Object();
        if (Type == 'alert-warning') {
            Styles.ClassText = 'text-white';
            Styles.BackGroundH1 = '#FFB833';
            Styles.ClassButton = 'btn-warning';
            Styles.BorderColor = '#c09853';
        } else if (Type == 'alert-info') {
            Styles.ClassText = 'text-white';
            Styles.BackGroundH1 = '#0079AD';
            Styles.ClassButton = 'btn-info';
            Styles.BorderColor = '#3a87ad';
        } else if (Type == 'alert-danger') {
            Styles.ClassText = 'text-white';
            Styles.BackGroundH1 = '#e74a3b';
            Styles.ClassButton = 'btn-danger';
            Styles.BorderColor = '#b94a48';
        } else {
            Styles.ClassText = 'text-white';
            Styles.BackGroundH1 = '#1cc88a';
            Styles.ClassButton = 'btn-success';
            Styles.BorderColor = '#468847';
        }

        return Styles;
    }


}(jQuery));

var TipoAlerta = {
    Sucesso: "success",
    Info: "info",
    Warning: "warning",
    Danger: "danger"
}

//Função chamada para Mostrar Alerta
//Parâmetros
//Msg     = String, mensagem a ser mostrada
//Titulo  = String, título da mensagem a ser mostrada
//Tipo    = String, CSS Classe do alerta
//Campo   = Elemento HTML que será focado ou esvaziado após a criação do alerta
//Esvazia = Boolean, true para o Campo ser esvaziado, false para o campo não ser esvaziado
//Foco    = Boolean, true para o Campo ser focado, false para o campo não ser focado
function MostraAlerta(Msg, Titulo, Tipo, Campo, Esvazia, Foco, Callback) {

    var ListButton = new Array();
    var Button1 = new Object();

    Button1.Content = 'OK';
    Button1.ClassButton = 'btn-danger';

    if (Tipo !== null)
        Button1.ClassButton = "btn-" + Tipo;

    //if (Tipo == "alert-success") {
    //    Button1.ClassButton = 'btn-success';
    //} else if (Tipo === "alert-info") {
    //    Button1.ClassButton = 'btn-info';
    //} else if (Tipo === "alert-danger") {
    //    Button1.ClassButton = 'btn-danger';
    //} else if (Tipo === "alert-warning") {
    //    Button1.ClassButton = 'btn-warning';
    //}

    Button1.action = function () {
        if (Foco) Campo.focus();
        if (Esvazia) Campo.val("");

        if (Callback) {
            Callback();
        }
    };

    ListButton.push(Button1);

    Campo.AlertBox({
        Type: "alert-" + Tipo,
        TitleMessage: Titulo,
        BodyMessage: Msg,
        ArrayButtons: ListButton
    });

    if (Esvazia) Campo.val("");
    $("#loadOverlay").hide();
}


//Função chamada para Mostrar Alerta de confirmação ([SIM] ou [NÃO]
//Parâmetros
//Msg       = String, mensagem a ser mostrada
//Titulo    = String, título da mensagem a ser mostrada
//Tipo      = String, CSS Classe do alerta
//Campo     = Elemento HTML que será focado ou esvaziado após a criação do alerta
//FunçãoSim = Function, função para caso o usuário pressionar [SIM]
//FuncaoNao = Function, função para caso o usuário pressionar [NÃO]
function MostraAlertaConfirma(Msg, Titulo, Tipo, Campo, FuncaoSim, FuncaoNao, msgBotaoSim, msgBotaoNao) {
    var ListButton = new Array();
    var Button1 = new Object();

    Button1.Content = msgBotaoSim || 'SIM';
    Button1.ClassButton = 'btn-success';

    Button1.action = FuncaoSim;

    var Button2 = new Object();

    Button2.Content = msgBotaoNao || 'NÃO';
    Button2.ClassButton = 'btn-danger';

    Button2.action = FuncaoNao || function () {

    };

    ListButton.push(Button1);
    ListButton.push(Button2);

    Campo.AlertBox({
        Type: "alert-" + Tipo,
        TitleMessage: Titulo,
        BodyMessage: Msg,
        ArrayButtons: ListButton
    });

}

//Função chamada para Habilitar ou Desabilitar um campo
//Parâmetros
//Desabilita = Boolean, true para desabilitar, false para habilitar
//Campo      = String, nome do ID do campo na página HTML
function HabilitaCampo(Desabilita, Campo) { $("#" + Campo).prop("disabled", Desabilita); }

//Função para setar valor em branco para um elemento HTML
//Parâmetros
//Campo  = Elemento HTML a ser setado valor vazio
function SetStringEmpty(Campo) { Campo.val("") }

//Função chamada para verificar se um Ano é bissexto
//Parâmetros
//Ano = Integer, ano a ser verificado
function IsBissexto(Ano) { if ((((Ano % 4) == 0) && ((Ano % 100) != 0)) || ((Ano % 400) == 0)) return 29; return 28; }

//Função chamada para selecionar um índice em um select
//Parâmetros
//Campo  = Elemento HTML a ser selecionado o índice
//Indice = Integer, índice a ser selecionado
function SelecionaIndice(Campo, Indice) { Campo.attr('selectedIndex', Indice); }

//Função chamada para checar um checkbox
//Parâmetros
//Check = Elemento HTML que confirmado se está checado
//Checa = Boolean, true para checkar, false para não checkar
function ChecaCheck(Check, Checa) { Check.attr('checked', Checa); }

//Função chamada para Remover acentos de uma cadeia de caracteres
//Parâmetros
//texto = String, cadeia de caracteres a ter os acentos removidos
function RemoveAcentos(texto) {
    texto = texto.replace(new RegExp('[ÁÀÂÃ]', 'gi'), 'A');
    texto = texto.replace(new RegExp('[ÉÈÊ]', 'gi'), 'E');
    texto = texto.replace(new RegExp('[ÍÌÎ]', 'gi'), 'I');
    texto = texto.replace(new RegExp('[ÓÒÔÕ]', 'gi'), 'O');
    texto = texto.replace(new RegExp('[ÚÙÛ]', 'gi'), 'U');
    texto = texto.replace(new RegExp('[Ç]', 'gi'), 'C');
    return texto;
}

//Função chamada para verificar se um valor contém apenas letras
//Parâmetros
//str = String, valor a ser verificado
function isLetter(str) {
    return str.length === 1 && str.match(/[a-z]/i);
}

//Função chamada para verificar que determinada cadeia de caracteres começa com um valor.
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };
}

function NCok() {
    return $().crypt({
        method: "xteab64enc",
        source: new Date().getSeconds()
    });
}

function COBCok(Ob, K) {
    try {
        return JSON.parse(sjcl.decrypt(K + $().crypt({ method: "xteab64enc", source: "KEY" }), Ob));
    } catch (e) {
        throw e;
    }
}

function Enc(Ob, K) {
    return sjcl.encrypt(K + $().crypt({ method: "xteab64enc", source: "KEY" }), JSON.stringify(Ob));
}

function converteToDouble(value) {
    if (!value) {
        value = 0;
    }
    value = value ? value.replace(".", "").replace(".", "").replace(".", "").replace(",", ".") : "0";
    return parseFloat(value);
}

//Função chamada para converter uma data JSON em .NET - FORMATA EM DATA
//Parâmetros
//NETData = String, valor de data que vêm no retorno de uma chamada AJAX em um objeto JSON
//corrigir data
function ConverteNETDataJsonData(NETData) {
    var re = /-?\d+/;
    var m = re.exec(NETData);
    var novaData = new Date(parseInt(m[0]));

    //if (novaData.getFullYear() === 1969) {        
    //    return NETData;
    //}

    //var novaData = new Date(parseInt(NETData.replace("/Date(", "").replace(")/", ""), 10));
    var dd = novaData.getDate(); //yields day
    var MM = novaData.getMonth(); //yields month
    var yyyy = novaData.getFullYear(); //yields year

    if (dd < 10) {
        dd = "0" + dd;
    }

    if ((MM + 1) < 10) {
        MM = "0" + (MM + 1);
    } else {
        MM = MM + 1;
    }

    novaData = dd + "/" + MM + "/" + yyyy;
    //var novaData = yyyy + "/" + MM + "/" + dd;

    //if (novaData == '01/01/1') {
    //    novaData = '';
    //}

    if (fIsDate(novaData) == false) {
        novaData = '';
    }

    return novaData;
}


//Função chamada para formatar uma data Javascript 
//Parâmetros
//JSData = String, valor de data que vêm no retorno de uma chamada AJAX em um objeto JSON
//corrigir data
function FormatarDataMysql(JSData) {

    if (!JSData) return "";

    if (!(JSData instanceof Date)) {
        JSData = new Date(JSData);
        if (!(JSData instanceof Date)) return "";
    }

    var dd = JSData.getDate(); //yields day
    var MM = JSData.getMonth(); //yields month
    var yyyy = JSData.getFullYear(); //yields year

    if (dd < 10) {
        dd = "0" + dd;
    }

    if ((MM + 1) < 10) {
        MM = "0" + (MM + 1);
    } else {
        MM = MM + 1;
    }

    var novaData = yyyy + '-' + MM + '-' + dd;

    return novaData;
}

//Função chamada para formatar uma data Javascript 
//Parâmetros
//JSData = String, valor de data que vêm no retorno de uma chamada AJAX em um objeto JSON
//corrigir data
function FormatarData(JSData) {

    if (!JSData) return "";

    if (!(JSData instanceof Date)) {
        JSData = new Date(JSData);
        if (!(JSData instanceof Date)) return "";
    }

    var dd = JSData.getDate(); //yields day
    var MM = JSData.getMonth(); //yields month
    var yyyy = JSData.getFullYear(); //yields year

    if (dd < 10) {
        dd = "0" + dd;
    }

    if ((MM + 1) < 10) {
        MM = "0" + (MM + 1);
    } else {
        MM = MM + 1;
    }

    var novaData = dd + "/" + MM + "/" + yyyy;
    //var novaData = yyyy + "/" + MM + "/" + dd;

    //if (novaData == '01/01/1') {
    //    novaData = '';
    //}

    if (fIsDate(novaData) == false) {
        novaData = '';
    }

    return novaData;
}

//Função chamada para converter uma data JSON em .NET - FORMATA DATA E HORA
//Parâmetros
//NETData = String, valor de data que vêm no retorno de uma chamada AJAX em um objeto JSON
//formata em data e hora
function ConverteNETDataJsonDataHora(NETData) {
    var str, year, month, day, hour, minute, d, novaData;
    str = NETData.replace(/\D/g, "");
    d = new Date(parseInt(str));

    year = d.getFullYear();
    month = pad(d.getMonth() + 1);
    day = pad(d.getDate());
    hour = pad(d.getHours());
    minutes = pad(d.getMinutes());

    novaData = day + "-" + month + "-" + year + " " + hour + ":" + minutes;

    return novaData;
}

function ConverteNETDataJsonHora(NETData) {
    var str, hour, minutes, d, novaHora;
    str = NETData.replace(/\D/g, "");
    d = new Date(parseInt(str));

    hour = pad(d.getHours());
    minutes = pad(d.getMinutes());

    novaHora = hour + ":" + minutes;

    return novaHora;
}

function convertStringDateToUrl(data_value) {
    var dia, mes, ano;
    dia = parseInt(data_value.split("/")[0]);
    mes = parseInt(data_value.split("/")[1]);
    ano = data_value.split("/")[2];
    return ano + "-" + mes + "-" + dia;
}

function convertToDate(data_value) {
    if (!data_value)
        return null;

    var dia, mes, ano;
    dia = parseInt(data_value.split("/")[0]);
    mes = parseInt(data_value.split("/")[1]) - 1;
    ano = data_value.split("/")[2];
    return new Date(ano, mes, dia, 0, 0, 0, 0);
}

function convertToDateTime(data_value, hora_value) {
    var dia, mes, ano, hora, minuto;
    dia = parseInt(data_value.split("/")[0]);
    mes = parseInt(data_value.split("/")[1]) - 1;
    ano = data_value.split("/")[2];

    hora = parseInt(hora_value.split(':')[0]);
    minuto = parseInt(hora_value.split(':')[1]);

    return new Date(ano, mes, dia, hora, minuto, 0, 0);
}

function pad(num) {
    num = "0" + num;
    return num.slice(-2);
}

//verifica se a data é válida
function fIsDate(dateString) {

    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
        return false;

    // Parse the date parts to integers
    var parts = dateString.split("/");
    var day = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[2], 10);

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month == 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Adjust for leap years
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;

    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];

    //if (data.length == 10) {


    //    er = /(0[0-9]|[12][0-9]|3[01])[-\.\/](0[0-9]|1[012])[-\.\/][0-9]{4}/;
    //    if (er.exec(data)) {
    //        return true;
    //    } else {
    //        return false;
    //    }

    //} else {
    //    return false;
    //}
}

//Função chamada para preencher os select
//Parâmetros
//mySelect = Elemento HTML Select usado para ser preenchido
//Lista = Array que será inserido no select
function PreencheSelect(Select, Lista) {
    jQuery.each(Lista, function () {
        Select.append($('<option></option>').val(this.Value).html($.trim(this.Text)));
    });
}

//Função chamada para exibir uma data nos padrões brasileiros
//Parâmetros
//Data = Date, data a ser convertida para exibição nos padrões brasileiros
function ExibicaoData(Data) {
    return Data.getDate() + "/" + ((Data.getMonth()) + 1) + "/" + Data.getFullYear();
}


function formatarMilhar(num) {
    x = 0;
    if (num < 0) {
        num = Math.abs(num);
        x = 1;
    }
    if (isNaN(num)) num = "0";
    num = Math.floor((num * 100 + 0.5) / 100).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++) {
        num = num.substring(0, num.length - (4 * i + 3)) + "."
            + num.substring(num.length - (4 * i + 3));
    }
    ret = num;
    if (x == 1) ret = ' - ' + ret;
    return ret;
}

function formataMoeda(valor, casas, separadorDecimal, separadorMilhar) {
    if (typeof (valor) !== "string") {
        valor = String(valor).replace(",", ".");
    } else {
        valor = valor.replace(",", ".");
    }
    var n = valor;
    casas = isNaN(casas = Math.abs(casas)) ? 2 : casas;
    separadorDecimal = separadorDecimal == undefined ? "." : separadorDecimal;
    separadorMilhar = separadorMilhar == undefined ? "," : separadorMilhar;
    var s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(casas)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + separadorMilhar : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + separadorMilhar) + (casas ? separadorDecimal + Math.abs(n - i).toFixed(casas).slice(2) : "");
};


//verificaSessaoAtiva = function () {
//    var retorno = false;
//    $.ajax({
//        type: 'POST',
//        url: raizAplicacao + 'Balanca/VerificaSessao',
//        dataType: 'json',
//        async: false,
//        success: function (result) {
//            retorno = result;
//        }
//    });
//    return retorno;
//}

//Função chamada para bloquear CRTL + C e CRTL + V em toda tela
function BlockCRTL() {
    //BLOQUIANDO CRTL + V, CRTL + C E CRTL + X
    $('input[type=text]').each(function () {
        $(this).bind("cut copy paste", function (e) {
            e.preventDefault();
        });
    });
}

//BlockCRTL();


/*---------------------------------------- BLOCO DE FUNÇÕES QUE EXECUTAM SOZINHAS PARA VALIDAÇÃO E SUAS RESPECTIVAS FUNÇÕES PARA QUE SEJAM CHAMADAS PARA PARTIAL VIEWS QUE SÃO GERADAS POSTERIORMENTE --------- */

//Função criada para proibir caracteres especiais para ser chamada em casos de partial views que são renderizadas depois da pagina principal ser carregada
function proibirCaracteresEspeciais() {
    $.each($(".noSpecialcharacter"), function () {
        $(this).keyup(function () {
            var raw_text = jQuery(this).val();
            var return_text = raw_text.replace(/[^a-zA-Z0-9/.& _-]/g, '');
            jQuery(this).val(return_text);
        });
    });
}
proibirCaracteresEspeciais();

//Função para validar números inteiros criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaNumeros() {
    //Loop executado para fazer validação de campos com número inteiro
    $.each($(".validNumber"), function () {
        $(this).ValidNumber({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro")
        });;
    });
}
validaNumeros();


//Função para validar  CPF criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaCPFs() {
    //Loop executado para fazer validação de campos de entra cpf
    $.each($(".validCPF"), function () {
        $(this).ValidCPF({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro")
        });;
    });
}

validaCPFs();

//Função para validar TELEFONE criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaTelefones() {
    //Loop executado para fazer validação de campos entrada de número de telefone
    $.each($(".ValidTelephone"), function () {
        $(this).ValidTelephone({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro")
        });;
    });
}

validaTelefones();

//Função para validar EMAILS criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaEmails() {
    //Loop executado para fazer validação de campos entrada de emails
    $.each($(".ValidEmail"), function () {
        $(this).ValidEmail({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro")
        });;
    });
}

validaEmails();

//Função para validar selects criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaSelect() {
    //Loop executado para fazer validação de campos com número inteiro
    $.each($(".validSelect"), function () {
        $(this).ValidSelect({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro")
        });
    });
}
validaSelect();




//Função para validar campos normais criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaNormal() {
    //Loop executado para fazer validação de campos com número inteiro
    $.each($(".validNormaly"), function () {
        $(this).ValidNormaly({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro"),
            minlength: $(this).attr("minLength"),
        });
    });
}
validaNormal();

//Função para validar campos data criada para ser chamada em paginas que o html é gerado em uma chamada ajax depois que página principal foi carregada.
function validaData() {
    //Loop executado para fazer validação de campos com número inteiro
    $.each($(".validDate"), function () {
        $(this).ValidDate({
            required: procuraRequired($(this).attr('class').split(' '), "required"),
            msg: $(this).attr("msgErro"),
            useDatepicker: $(this).attr("useDatepicker"),
            maiorQueAtual: procuraRequired($(this).attr('class').split(' '), "maiorQueHoje"),
        });
    });
}
validaData();

function proibirTagsHmtl() {

    $(document).on('keyup', '.no_tags_html', function () {
        var raw_text = $(this).val();
        var return_text = raw_text.replace(/(<([^>]+)>)/ig, "");
        $(this).val(return_text);
    });

    //$.each($(".no_tags_html"), function () {
    //    $(this).keyup(function () {
    //        var raw_text = $(this).val();
    //        var return_text = raw_text.replace(/(<([^>]+)>)/ig, "");
    //        $(this).val(return_text);
    //    });
    //});
}
proibirTagsHmtl();

function toUpper() {
    $(document).on('keyup', 'input', function () {

        if ($(this).attr("type") === 'password') return;

        var raw_text = $(this).val();
        var return_text = raw_text.toUpperCase();
        $(this).val(return_text);
    });
}
//toUpper();


//Funcão que valida Enter dos Campos
function configuraEnterInputs() {
    $('body').on('keydown', 'input:not(.no_enter), select:not(.no_enter)', function (e) {
        var self = $(this)
            , form = self.parents('form:eq(0)')
            , focusable
            , next;

        if (e.keyCode === 13 || e.which === 13) {
            //e.preventDefault();
            focusable = form.find('input:not(.no_focus),a,select,button:not(.no_focus)').filter(':visible').filter(':enabled');
            next = focusable.eq(focusable.index(this) + 1);
            if (next.length) {
                next.focus();
            } else {
                //if (!form.hasClass("no-submit")) {
                //    form.submit();
                //}
            }
            return false;
        }
    });

}
configuraEnterInputs();

//Função criada para procurar a classe required dentre as classes de um elemento
//Parâmetros
//classes = array contendo as classes do elemento
function procuraRequired(classes, classeProcurar) {
    for (var i = 0; i < classes.length; i++)
        if (classes[i] == classeProcurar) return true;
    return false;
}
/*------------------- FIM DE BLOCO --------------------------*/



//Função chamada para iniciar tabela usando o DataTable
//Parâmetros
//Tabela = Elemento HTML do tipo TABLE que será inicializado.
//Linguagem = String, a linguagem em que será mostrado os textos.

initTabela = function (Tabela, Language, ColumnDefs, Sorting, SortingDirection, iDisplayLength) {
    var Languages = new Array();

    Tabela.dataTable({
        "columnDefs": ColumnDefs,
        "aaSorting": [
            [Sorting, SortingDirection]
        ],
        "aLengthMenu": [
            [5, 10, 15, 20, 50, -1],
            [5, 10, 15, 20, 50, (Language == "" || Language == "pt-BR" ? "Todos" : Language == "en" ? "All" : "Todos")] // change per page values here
        ],

        "bDestroy": true,

        "oLanguage": {
            "sLengthMenu": " _MENU_ ",
            "sInfo": (Language == "" || Language == "pt-BR" ? "Total de" : Language == "en" ? "Total" : "Total") + " _TOTAL_ " + (Language == "" || Language == "pt-BR" ? "registros" : Language == "en" ? "records" : "registros") + ". " + (Language == "" || Language == "pt-BR" ? "Do" : Language == "en" ? "From" : "Del") + "  _START_º " + (Language == "" || Language == "pt-BR" ? "ao" : Language == "en" ? "to" : "al") + " _END_º.",
            "sInfoEmpty": (Language == "" || Language == "pt-BR" ? "Nenhum resultado encontrado." : Language == "en" ? "No results found" : "No hay resultados"),
            "sZeroRecords": (Language == "" || Language == "pt-BR" ? "Nenhum resultado encontrado." : Language == "en" ? "No results found" : "No hay resultados"),
            "sSearch": "",
            "sInfoFiltered": (Language == "" || Language == "pt-BR" ? "Nenhum resultado encontrado." : Language == "en" ? "No results found" : "No hay resultados"),
            "oPaginate": {
                "sPrevious": (Language == "" || Language == "pt-BR" ? "Anterior" : Language == "en" ? "Previous" : "Anterior"),
                "sNext": (Language == "" || Language == "pt-BR" ? "Próximo" : Language == "en" ? "Next" : "Siguiente"),
            }
        },
        "iDisplayLength": iDisplayLength,
        "bDeferRender": false
    });

    $('.dataTables_filter input').attr('placeholder', (Language == "" || Language == "pt-BR" ? "Localizar" : Language == "en" ? "Find" : "Encontrar"));
    $('.dataTables_filter input').addClass("form-control LetraFomulario ");
    $('.dataTables_length select').addClass("form-control LetraFomulario ");

}

initTabelaPaginaServidor = function (Tabela, Language, ColumnDefs, Sorting, SortingDirection, funcaoController, aoColumns) {
    var Languages = new Array();

    Tabela.dataTable({
        "columnDefs": ColumnDefs,
        "aaSorting": [
            [Sorting, SortingDirection]
        ],
        "aLengthMenu": [
            [5, 10, 15, 20, 50, -1],
            [5, 10, 15, 20, 50, (Language == "" || Language == "pt-BR" ? "Todos" : Language == "en" ? "All" : "Todos")] // change per page values here
        ],
        "bServerSide": true,
        "sAjaxSource": funcaoController,

        "aoColumns": aoColumns,

        "bDestroy": true,

        "oLanguage": {
            "sLengthMenu": " _MENU_ ",
            "sInfo": (Language == "" || Language == "pt-BR" ? "Total de" : Language == "en" ? "Total" : "Total") + " _TOTAL_ " + (Language == "" || Language == "pt-BR" ? "registros" : Language == "en" ? "records" : "registros") + ". " + (Language == "" || Language == "pt-BR" ? "Do" : Language == "en" ? "From" : "Del") + "  _START_º " + (Language == "" || Language == "pt-BR" ? "ao" : Language == "en" ? "to" : "al") + " _END_º",
            "sInfoEmpty": (Language == "" || Language == "pt-BR" ? "Nenhum resultado encontrado." : Language == "en" ? "No results found" : "No hay resultados"),
            "sZeroRecords": (Language == "" || Language == "pt-BR" ? "Nenhum resultado encontrado." : Language == "en" ? "No results found" : "No hay resultados"),
            "sSearch": "",
            "sInfoFiltered": (Language == "" || Language == "pt-BR" ? "Nenhum resultado encontrado." : Language == "en" ? "No results found" : "No hay resultados"),
            "oPaginate": {
                "sPrevious": (Language == "" || Language == "pt-BR" ? "Anterior" : Language == "en" ? "Previous" : "Anterior"),
                "sNext": (Language == "" || Language == "pt-BR" ? "Próximo" : Language == "en" ? "Next" : "Siguiente"),
            }
        },
        "iDisplayLength": 10,
        "bDeferRender": false
    });

    $('.dataTables_filter input').attr('placeholder', (Language == "" || Language == "pt-BR" ? "Localizar" : Language == "en" ? "Find" : "Encontrar"));
    $('.dataTables_filter input').addClass("form-control LetraFomulario ");
    $('.dataTables_length select').addClass("form-control LetraFomulario ");

}


//Função chamada para gerar o Rodapé do Relatório HTML
//Parâmetros
//Linguagem = String, a linguagem em que será mostrado os textos.
function RodapeRelatorio(Language) {
    return '<footer>' +
        ' <div class="row">' +
        '  <div class="col-lg-12">' +
        '<p style="text-align:center;">' + (Language == "" || Language == "pt-BR" ? "Planilha gerada eletrônicamente em " : Language == "en" ? "Spreadsheet generated electronically in" : "Hoja de cálculo generada electrónicamente en") + ' https://www.abczstat.com.br/Sigen </p>' +
        '<p style="text-align:center;">' + ExibicaoData(new Date()) + '</p>' +
        ' </div>' +
        ' </div>' +
        ' </footer>';
}

//Função chamada para gerar as classes do Relatório HTML
function ClassesRelatorio() {
    return '<html><head><meta charset="utf-8" /><link href="/Sigen/Content/site.css" rel="stylesheet"/>' +
        '<link href="/Sigen/Content/DataTables-1.10.2/css/dataTables.tableTools.css" rel="stylesheet"/>' +
        '<link href="/Sigen/Content/DataTables-1.10.2/css/jquery.dataTables.css" rel="stylesheet"/>';
}

function ResetarFormulario(form) {
    form.reset();
}

//Aparecer TOOLTIP nas linhas de qualquer tabela. Alguma informação.
this.ToolTipPreview = function (tabela, texto) {
    xOffset = 10;
    yOffset = 30;
    tabela.hover(function (e) {
        $("body").append("<p id='screenshot'>" + texto + "</p>");
        $("#screenshot")
            .css("top", (e.pageY - xOffset) + "px")
            .css("left", (e.pageX + yOffset) + "px")
            .fadeIn("slow");
    },
        function () {
            this.title = this.t;
            $("#screenshot").remove();
        });
    $("a.screenshot").mousemove(function (e) {
        $("#screenshot")
            .css("top", (e.pageY - xOffset) + "px")
            .css("left", (e.pageX + yOffset) + "px");
    });
};

//var KY = NCok();

(function ($) {
    $.fn.getDia = function () {
        var Data = $(this).val();
        var Dia = parseInt(Data.split("/")[0]);
        return Dia;
    }

    $.fn.getMes = function () {
        var Data = $(this).val();
        var Mes = parseInt(Data.split("/")[1]);
        return Mes;
    }

    $.fn.getAno = function () {
        var Data = $(this).val();
        var Ano = parseInt(Data.split("/")[2]);
        return Ano;
    }

    $.fn.compareDate = function (strDataCompare) {

        var dataSource = convertToDate($(this).val());
        var dataCompare = convertToDate(strDataCompare);

        if (dataSource.getTime() < dataCompare.getTime())
            return -1;
        else if (dataSource.getTime() > dataCompare.getTime())
            return 1;
        else
            return 0;

    };

    $.fn.validaHora = function () {
        var input = $(this).val();

        if (input && input.length === 5) {
            var splited = input.split(':');
            if (splited.length === 2) {
                var horas = parseInt(splited[0]);
                var minutos = parseInt(splited[1]);
                if (horas >= 0 && horas <= 23 && minutos >= 0 && minutos <= 59) {
                    return true;
                }
            }
        }

        return false;
    };

}(jQuery));


function retornaDescricaoTipoDocumento(tipoDoc) {
    var descricao = "";

    switch (tipoDoc) {
        case "C": descricao = "CPF"; break;
        case "R": descricao = "RG"; break;
        case "H": descricao = "CNH"; break;
    }

    return descricao;
}

//FUNÇÃO PARA CALCULAR INTERVALO ENTRE DATAS
//DATA NO FORMATO dd/MM/yyyy
//RETORNO DE INTERVALOS INFORMAR UMA DAS OPÇÕES: anos, meses, semanas, dias, horas, minutos, segundos
function getDateDiff(date1, date2, interval) {

    var arrd1 = date1.split("/");
    var arrd2 = date2.split("/");

    date1 = new Date(arrd1[2], arrd1[1] - 1, arrd1[0]);
    date2 = new Date(arrd2[2], arrd2[1] - 1, arrd2[0]);


    var second = 1000,
        minute = second * 60,
        hour = minute * 60,
        day = hour * 24,
        week = day * 7;
    dateone = new Date(date1).getTime();
    datetwo = (date2) ? new Date().getTime() : new Date(date2).getTime();

    var timediff = date2.getTime() - date1.getTime();
    secdate = new Date(date2);
    firdate = new Date(date1);

    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "anos":
            return secdate.getFullYear() - firdate.getFullYear();
        case "meses":
            var meses = ((secdate.getFullYear() * 12 + secdate.getMonth()) - (firdate.getFullYear() * 12 + firdate.getMonth()));
            if (secdate.getDate() < firdate.getDate()) {
                --meses;
            }
            return meses;

        case "semanas":
            return Math.floor(timediff / week);
        case "dias":
            return Math.ceil(timediff / 1000 / 60 / 60 / 24);
        case "horas":
            return Math.floor(timediff / hour);
        case "minutos":
            return Math.floor(timediff / minute);
        case "segundos":
            return Math.floor(timediff / second);
        default:
            return undefined;
    }
}

//Função que retorno o mês atual
//Integer : Nº do mês 
function getMesDescricao(mesValue) {
    switch (mesValue) {
        case 1: return "Janeiro";
        case 2: return "Fevereiro";
        case 3: return "Março";
        case 4: return "Abril";
        case 5: return "Maio";
        case 6: return "Junho";
        case 7: return "Julho";
        case 8: return "Agosto";
        case 9: return "Setembro";
        case 10: return "Outubro";
        case 11: return "Novembro";
        case 12: return "Dezembro";
        default: return undefined;
    }
}

function formataCep(valor) {
    if (valor && valor.length === 8)
        return valor.substring(0, 5) + "-" + valor.substring(5, 8);
    return valor;
}

function RetornaSomenteNumeros(valor) {
    if (valor) return valor.replace(/[^\d]+/g, '');
    return "";
}

function RetornaNumerosELetras(valor) {
    if (valor) return valor.replace(/[^\da-zA-Z]+/g, '');
    return "";
}

function criarSelectizeObjeto(id, urlBusca, options, list) {
    return $(id).selectize($.extend(true, {
        valueField: 'id',
        labelField: 'nome',
        searchField: 'nome',
        create: false,
        load: function (query, callback) {
            if (list === null || list === undefined) {
                if (query.length < 1) return callback();
                $.ajax({
                    url: raizAplicacao + urlBusca + '?query=' + encodeURIComponent(query),
                    type: 'GET',
                    error: function (error) {
                        callback();
                    },
                    success: function (res) {
                        callback(res.list);
                    }
                });
            } else {
                if (query.length < 1) {
                    callback(list);
                } else {
                    var listFiltered = list.filter(x => x.nome.toUpperCase().includes(query.toUpperCase()));
                    callback(listFiltered);
                }
            }
        }
    }, options));
}

function obterNomePastoCurralPorCodigo(codigo) {

    if (codigo === 1)
        return 'Curral';
    else if (codigo === 2)
        return 'Pasto';

    return '';
}

function obterFiltosFormatadosRelatorio(filtros) {
    var strFiltro = [];
    var strData = [];
    var filtro = '';
    var campoData = null;
    var operator = null;

    for (var i in filtros) {

        if ((i.indexOf('dataInicial_') !== -1 || i.indexOf('dataFinal_') !== -1)) {
            if (!strData.length) {                
                campoData = i.split('_')[1];
                var dataInicial = FormatarDataMysql(filtros['dataInicial_' + campoData]);
                var dataFinal = FormatarDataMysql(filtros['dataFinal_' + campoData]);

                if (dataInicial && dataFinal) {
                    strData.push(`'${dataInicial}'`);
                    strData.push(`'${dataFinal}'`);
                    operator = 'between';
                } else if (dataInicial) {
                    strData.push(`'${dataInicial}'`);
                    operator = '>='
                } else if (dataFinal) {
                    strData.push(`'${dataFinal}'`);
                    operator = '<='
                }
            }
        } else {
            var value = filtros[i] || '';
            if (value) {
                strFiltro.push(`${i} = '${value}'`);
            }
        }
    }

    if (strData.length) {
        strFiltro.push(`${campoData} ${operator} ${strData.join(' AND ')}`);
    }

    filtro = strFiltro.join(' AND ');
    
    return filtro;
}