var dataTable;

$(document).ready(function () {

    loadDataTable();

});

function loadDataTable() {

    dataTable = $('#tblDataprojectUsers').DataTable({

        "ajax": { "url": "/Admin/ProjectUser/IndexAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "project.name", "width": "20%" },
            { "data": "applicationUser.fullName", "width": "20%" },
            { "data": "isActive", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/Admin/ProjectUser/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/ProjectUser/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"

            }
        ]

    });
}