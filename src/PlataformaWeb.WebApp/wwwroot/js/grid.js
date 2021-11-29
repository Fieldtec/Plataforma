(function ($) {
    
    function processOptions() {        
        var result;
        var $t = $(this);
        var $table = $t.data("options");

        switch (arguments[0]) {
            case 'setData':
                if ($table.rows()[0].length) {
                    $table.clear();
                }
                $table.rows.add(arguments[1]).draw();
                break;
            case 'getByIndex':
                result = $table.rows(arguments[1]).data()[0];
                break;
            case 'addData':
                $table.rows.add([data]).draw();
                break;
            case 'getSelected':
                result = getSelected($t, $table);
                break;
            case 'getData':
                result = $table.rows().data().toArray();
                break;
        }
        return result;
    }

    function getSelected(target, options) {
        var list = [];

        $.each(target.find('tbody tr td input'), function (i, item) {
            if ($(this).prop('checked')) {
                var index = $(this).parents('tr').index();
                list.push(Object.assign({}, options.rows(index).data()[0]));
            }
        });

        return list;
    }

    function create(target, options) {

        if (options.onSelected) {
            target.on('click', 'tbody tr', function () {
                var data = table.row(this).data();
                if (data) options.onSelected(data);
            });
        }

        target.on('draw.dt', function () {

            if (options.hasSelectAll) {
                target.find('thead tr th:eq(0)').html('<input type="checkbox" value="selectAll" />');
                target.on('click', 'thead tr th input[type="checkbox"]', function () {
                    target.find('tbody tr td input').prop('checked', $(this).prop("checked"));
                });
            }

            if (options.afterFetch) {
                var rows = target.data("options").rows();
                options.afterFetch(rows.nodes().toArray(), rows.data().toArray());
            }
        });

        var table = target.DataTable(options);

        target.data("options", table);

    }

    $.fn.grid = function () {
        var returnParam = false;
        var param = [];
        var args = arguments;

        var result = this.each(function () {
            var $this = $(this);
            switch (typeof (args[0])) {
                case 'string':
                    param.push(processOptions.apply($this, args));
                    returnParam = true;
                    break;
                case 'object':
                default:
                    create($this, $.extend(true, {}, $.fn.grid.defaultOptions, args[0] || {}));
            }
        });

        if (returnParam) {
            return param.length === 1 ? param[0] : param;
        }

        return result;

    }

    //$.fn.grid.getByIndex = function (index) {
    //    return table.rows(index).data()[0];
    //}

    //$.fn.grid.setData = function (list) {

    //    if (table.rows()[0].length) {
    //        table.clear();
    //    }
    //    table.rows.add(list).draw();
    //}

    //$.fn.grid.addData = function (data) {
    //    table.rows.add([data]).draw();
    //}

    //$.fn.grid.getData = function () {
    //    return table.rows().data().toArray();
    //}

    $.fn.grid.defaultOptions = {
        language: {
            url: raizAplicacao + 'lib/datatables/js/i18n.ptBR.json'
        },
        select: true,
        searching: false,
        info: false,
        paging: false
    }

}(jQuery));