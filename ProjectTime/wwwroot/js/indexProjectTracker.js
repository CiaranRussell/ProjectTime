var dataTable;

function getProjectId() {

    var url = window.location.href;
    var parts = url.split("=");
    var projectId = parts[parts.length - 1];
    return projectId
};


$(document).ready(function () {

    
    var projectId = getProjectId();
    loadDataTable(projectId);
});


function loadDataTable(projectId) {
    console.log(projectId)

    dataTable = $('#tblDataindexprojecttracker').DataTable({

        "ajax": { "url": "/SuperUser/ActualVEstimate/IndexProjectTrackerAPI?id=" + projectId },

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
                }, "width": "15%"
            },
            { "data": "durationDays", "width": "15%" },
            { "data": "totalCost", "width": "15%" },
            { "data": "actualMinDate", "width": "15%" },
            {
                "data": "actualMaxDate", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "15%"
            },
            { "data": "actualDurationDays", "width": "25%" },
            { "data": "actualTotalCost", "width": "15%" }]

    });
}