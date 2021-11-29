$(function () {

    $(document).on("click", '#escolherClienteModal', function (e) {
        e.preventDefault();
        buscarClientes();
    });

    $('#tableClientes').grid({
        columns: [
            { title: "Propriedade", data: "propriedade" },
            { title: "Proprietário", data: "nome" },
            { title: "Município", data: "municipio" },
            { title: "UF", data: "uf" }
        ],
        onSelected: function (data) {                        
            $.getUrlAjax(null, 'selecionar-cliente/' + data.id, true, function (res) {
                if (res.status) {
                    window.location.reload();
                } else {
                    MostraAlerta(res.mensagem, "Atenção", TipoAlerta.Warning, $(this), false, false);
                }                
            });
        }
    });

    function buscarClientes() {
        $.getUrlAjax(null, 'buscar-clientes', true, function (res) {    

            $('#tableClientes').grid("setData", res.list);

            //$('#tableClientes').grid.setData(res.list);
            $('#clientesModal').modal("show");
        });
    }

});
