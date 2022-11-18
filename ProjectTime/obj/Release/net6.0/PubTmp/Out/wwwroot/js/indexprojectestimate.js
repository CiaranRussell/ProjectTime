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

    dataTable = $('#tblDataindexprojectestimate').DataTable({

        "ajax": { "url": "/SuperUser/ProjectEstimate/IndexProjectEstimateAPI?id=" + projectId },

        "columns": [

            { "data": "project.name", "width": "10%" },
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
            { "data": "durationDays", "width": "20%" },
            { "data": "totalCost", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/SuperUser/ProjectEstimate/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/SuperUser/ProjectEstimate/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"

            }
        ]

    });
}