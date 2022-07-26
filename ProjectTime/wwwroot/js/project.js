﻿let dataTable;

$(document).ready(function () {

    loadDataTable();

});

function loadDataTable() {

    dataTable = $('#tblDataProject').DataTable({
        "ajax": {
            "url": "/Admin/Project/IndexAPI", "type": "GET", "datatype": "json"
        },
        "columns": [

            { "data": "projectCode", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "projectStage.stage", "width": "15%" },
            { "data": "description", "width": "35%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Project/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/Project/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "20%"
            }
        ]

    });
}