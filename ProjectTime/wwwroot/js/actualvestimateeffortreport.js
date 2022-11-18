let dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {


    dataTable = $('#tblDataactualvestimateeffortreport').DataTable({

        "ajax": { "url": "/SuperUser/Report/ActualVEstimateReportAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "10%" },
            { "data": "department.name", "width": "10%" },
            {
                "data": "dateFrom", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            {
                "data": "dateTo", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            { "data": "durationDays", "width": "15%" },
            { "data": "actualMinDate", "width": "10%" },
            { "data": "actualMaxDate", "width": "10%" },
            { "data": "actualDurationDays", "width": "15%" },
            { "data": "durationDaysVariance", "width": "25%" }


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