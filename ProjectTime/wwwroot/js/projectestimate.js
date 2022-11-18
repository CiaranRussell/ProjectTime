let dataTable;


$(document).ready(function () {

    
    loadDataTable();
});


function loadDataTable() {
    

    dataTable = $('#tblDataprojectestimate').DataTable({

        "ajax": { "url": "/SuperUser/ProjectEstimate/IndexAPI", "type": "GET", "datatype": "json"},

        "columns": [

            { "data": "project.projectCode", "width": "10%" },
            { "data": "project.name", "width": "10%" },
            { "data": "project.projectStage.stage", "width": "10%" },
            {
                "data": "minDate", render: function (data) {
                return moment(data).format('DD/MM/YYYY');
                }, "width": "13%"
            },
            { "data": "maxDate", "width": "10%" },
            { "data": "durationDays", "width": "13%" },
            { "data": "totalCost", "width": "10%" },
            {
                "data": "project.id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/SuperUser/ProjectEstimate/IndexProjectEstimate?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-clipboard-data"></i>Project Estimate</a>
					</div>
                          `
                },
                "width": "20%"

            }
        ]

    });
}