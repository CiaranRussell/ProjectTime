let dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {


    dataTable = $('#tblDataactualvestimatesummaryreport').DataTable({

        "ajax": { "url": "/SuperUser/Report/ActualVEstimateSummaryReportAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "10%" },
            { "data": "project.projectStage.stage", "width": "10%" },
            {
                "data": "estimateMinDate", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            { "data": "estimateMaxDate", "width": "10%" },
            { "data": "estimateDurationDays", "width": "10%" },
            { "data": "estimateTotalCost", "width": "10%" },
            { "data": "minDate", "width": "10%" },
            { "data": "maxDate", "width": "10%" },
            { "data": "duration", "width": "10%" },
            { "data": "totalCost", "width": "10%" },
            { "data": "durationDaysVariance", "width": "13%" },
            { "data": "underOverDuration", "width": "11%" },
            { "data": "totalCostVariance", "width": "13%" },
            { "data": "underOverBudget", "width": "12%" },


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