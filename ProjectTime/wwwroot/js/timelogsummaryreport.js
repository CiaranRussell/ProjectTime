let dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {


    dataTable = $('#tblDatatimelogsummaryreport').DataTable({

        "ajax": { "url": "/SuperUser/Report/TimeLogSummaryReportAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.projectCode", "width": "13%" },
            { "data": "project.name", "width": "10%" },
            { "data": "project.projectStage.stage", "width": "10%" },
            { "data": "minDate", "width": "13%" },
            { "data": "maxDate", "width": "14%" },
            { "data": "duration", "width": "20%" },
            { "data": "project.description", "width": "20%" },

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
        ]

    });

    dataTable.buttons().container().appendTo($('#printbar'));
}