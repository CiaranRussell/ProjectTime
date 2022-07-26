﻿let dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {


    dataTable = $('#tblDataactualvestimatevariancereport').DataTable({

        "ajax": { "url": "/SuperUser/Report/ActualVEstimateReportAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "10%" },
            { "data": "department.name", "width": "12%" },
            { "data": "durationDays", "width": "11%" },
            { "data": "actualDurationDays", "width": "11%" },
            { "data": "durationDaysVariance", "width": "13%" },
            { "data": "underOverDuration", "width": "11%" },
            { "data": "totalCost", "width": "11%" },
            { "data": "actualTotalCost", "width": "11%" },
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