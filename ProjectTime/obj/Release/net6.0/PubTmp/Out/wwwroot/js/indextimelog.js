let dataTable;


$(document).ready(function () {

    loadDataTable();
});


function loadDataTable() {
    

    dataTable = $('#tblDataindextimelog').DataTable({

        "ajax": { "url": "/User/TimeLog/IndexTimeLogAPI", "type": "GET", "datatype": "json"},

        "columns": [

            { "data": "project.projectCode", "width": "12%" },
            { "data": "project.name", "width": "12%" },
            { "data": "project.projectStage.stage", "width": "12%" },
            { "data": "minDate", "width": "15%"},
            { "data": "maxDate", "width": "15%" },
            { "data": "duration", "width": "20%" },
            {
                "data": "project.id",
                "render": function (data) {
                    return `
                        <div class="w-100 btn-group" role="group">
                        <a href="/User/TimeLog/IndexTimeLog?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-clipboard-data"></i>Time Log's</a>
					</div>
                          `
                },
                "width": "20%"

            }
        ]

    });
}