var dataTable;

$(document).ready(function () {

    loadDataTable();

});

function loadDataTable() {

    dataTable = $('#tblDataUsers').DataTable({

        "ajax": { "url": "/Admin/Account/IndexAPI", "type": "GET", "datatype": "json" },

        "columns": [

            { "data": "fullName", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "department.name", "width": "20%" },
            { "data": "emailConfirmed", "width": "20%" },
            { "data": "role", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/Admin/Account/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/Account/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"

            }
        ]

    });
}