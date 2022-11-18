let dataTable;

$(document).ready(function () {

    loadDataTable();

});

function loadDataTable() {

    dataTable = $('#tblDataprojectstage').DataTable({

        "ajax": { "url": "/Admin/ProjectStage/IndexAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "stage", "width": "25%" },
            { "data": "description", "width": "25%" },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/ProjectStage/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/ProjectStage/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
        ]

    });
}