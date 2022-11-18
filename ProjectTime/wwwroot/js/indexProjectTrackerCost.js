let dataTable;

function getProjectId() {

    let url = window.location.href;
    let parts = url.split("=");
    let projectId = parts[parts.length - 1];
    return projectId
}


$(document).ready(function () {

    
    let projectId = getProjectId();
    loadDataTable(projectId);
});


function loadDataTable(projectId) {
    console.log(projectId)

    dataTable = $('#tblDataindexprojecttrackercost').DataTable({

        "ajax": { "url": "/SuperUser/ActualVEstimate/IndexProjectTrackerAPI?id=" + projectId },

        "columns": [

            { "data": "department.name", "width": "15%" },
            { "data": "durationDays", "width": "11%" },
            { "data": "actualDurationDays", "width": "11%" },
            { "data": "durationDaysVariance", "width": "11%" },
            { "data": "underOverDuration", "width": "11%" },
            { "data": "totalCost", "width": "11%" },
            { "data": "actualTotalCost", "width": "8%" },
            { "data": "totalCostVariance", "width": "11%" },
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