﻿var dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {
    

    dataTable = $('#tblDataActualVEstimate').DataTable({

        "ajax": { "url": "/SuperUser/ActualVEstimate/IndexAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "10%" },
            {
                "data": "estimateMinDate", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            { "data": "estimateMaxDate", "width": "10%" },
            { "data": "estimateDurationDays", "width": "10%" },
            { "data": "estimateTotalCost", "width": "10%" },
            { "data": "minDate", "width": "10%" },
            {
                "data": "maxDate", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            { "data": "duration", "width": "10%" },
            { "data": "totalCost", "width": "10%" },
            {
                "data": "project.id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/SuperUser/ActualVEstimate/IndexProjectTracker?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-clipboard-data"></i>Effort</a>
                        <a href="/SuperUser/ActualVEstimate/IndexProjectTrackerCost?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-clipboard-data"></i>Variance</a>
					</div>
                          `
                },
                "width": "10%"

            }
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