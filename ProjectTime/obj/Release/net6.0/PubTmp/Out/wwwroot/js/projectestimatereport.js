var dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {


    dataTable = $('#tblDataprojectestimatereport').DataTable({

        "ajax": { "url": "/SuperUser/Report/ProjectEstimateReportAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "12%" },
            { "data": "department.name", "width": "10%" },
            {
                "data": "dateFrom", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "15%"
            },
            {
                "data": "dateTo", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "15%"
            },
            { "data": "durationDays", "width": "15%" },
            { "data": "totalCost", "width": "12%" },
            { "data": "description", "width": "30%" }


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