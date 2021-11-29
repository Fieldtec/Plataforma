(function () {

    this.apiRelatorio = this.apiRelatorio || {};

    var ns = this.apiRelatorio;

    ns.nomeRelatorio = null;

    ns.callback = null;

    ns.init = function () {

        var $modal = $('#relatorioModal'),
            $btnImprimir = $('#btnImprimirRelatorio'),
            $form = $('#formFiltrosRelatorio'),
            $titulo = $('#tituloRelatorioModal');

        $(document).on('click', '.link-relatorio', function (e) {
            e.preventDefault();
            var nomeRelatorio = $(this).data('relatorio'),
                titulo = $(this).data('titulo');

            if (!nomeRelatorio || !titulo) {
                MostraAlerta("Configuração inválida para o relatório", "Atenção", TipoAlerta.Warning, $(this), false, false);
                return;
            }

            apiRelatorio.nomeRelatorio = nomeRelatorio;
            $titulo.html(titulo);
            apiRelatorio.callback = null;
            $form.find('.modal-body').LoadPartialView(nomeRelatorio, "Relatorios", function () {
                $modal.modal('show');
            });

        });

        $btnImprimir.on('click', function () {
            var isValid = $form.ValidForm();
            if (isValid) {
                var filtros = $form.GetJsonData()
                if (apiRelatorio.callback) {
                    filtros = apiRelatorio.callback(filtros);
                }
                apiRelatorio.imprimir(filtros);
            }
        });

        $modal.on('shown.bs.modal', function () {
            $form.find('.modal-body').find('input, select, textarea').eq(0).focus();
        });

        $modal.on('hide.bs.modal', function () {
            $form.find('.modal-body').html('');
            apiRelatorio.callback = null;
            ns.nomeRelatorio = null;
        });

    }

    ns.imprimir = function (filtros, nomeRelatorio) {

        var data = {
            nomeRelatorio: nomeRelatorio || ns.nomeRelatorio,
            filtro: obterFiltosFormatadosRelatorio(filtros)
        };

        $.postUrlAjax(data, 'imprimir-relatorio', true, function (res) {
            window.open(raizAplicacao + 'reports/' + res, '_blank');
            MostraAlerta("Relatório emitido com sucesso", "Sucesso", TipoAlerta.Sucesso, $(this), false, true);
        });
    }

})();

$(document).ready(function () {
    apiRelatorio.init();
});