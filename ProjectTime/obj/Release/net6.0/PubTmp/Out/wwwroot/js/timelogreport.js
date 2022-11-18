var minDate, maxDate;

// Custom filtering function which will search data in column three between two date values
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var min = minDate.val();
        var max = maxDate.val();
        var date = moment(data[3], 'DD/MM/YYYY h:m A');

        if (
            (min === null && max === null) ||
            (min === null && date <= max) ||
            (min <= date && max === null) ||
            (min <= date && date <= max)
        ) {
            return true;
        }
        return false;
    }
);

$(document).ready(function () {
    // Create date inputs
    minDate = new DateTime($('#min'), {
        format: 'DD/MM/YYYY'
    });
    maxDate = new DateTime($('#max'), {
        format: 'DD/MM/YYYY'
    });

    // DataTables initialisation
    var table = $('#tblDatatimelogreport').DataTable({


        "ajax": { "url": "/SuperUser/Report/TimeLogReportAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "16%" },
            { "data": "projectUser.applicationUser.department.name", "width": "16%" },
            { "data": "projectUser.applicationUser.fullName", "width": "16%" },
            {
                "data": "date", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "16%"
            },
            { "data": "duration", "width": "16%" },
            { "data": "description", "width": "20%" }

        ],

        dom: 'lBfrtip',
        buttons: [
            {
                extend: 'copyHtml5',
                text: '<i class="bi bi-files"></i> Copy',
                titleAttr: 'Copy'
            },
            {
                extend: 'excelHtml5',
                text: '<i class="bi bi-file-earmark-excel"></i> Excel',
                titleAttr: 'Excel'
            },
            {
                extend: 'csvHtml5',
                text: '<i class="bi bi-filetype-csv"></i> CSV',
                titleAttr: 'CSV'
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="bi bi-file-pdf"></i> PDF',
                titleAttr: 'PDF'
            },
            {
                extend: 'print',
                text: '<i class="bi bi-printer"></i> Print',
                titleAttr: 'Print'
            }
        ],

    });

    // Refilter the table
    $('#min, #max').on('change', function () {
        table.draw();
    });

    table.buttons().container().appendTo($('#printbar'));

});