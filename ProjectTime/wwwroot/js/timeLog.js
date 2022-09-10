var dataTable;



$(document).ready(function () {

    loadDataTable();

});

{
    var url = window.location.href;
    var parts = url.split("/");
    var productId = parts[parts.length - 1];
};

function loadDataTable() {

    dataTable = $('#tblDatatimelog').DataTable({

        "ajax": { "url": "/User/TimeLog/IndexAPI?ID=" + productId},

        "columns": [

            { "data": "project.name", "width": "15%" },
            { "data": "date", "width": "15%" },
            { "data": "duration", "width": "15%" },
            { "data": "description", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/User/TimeLog/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/User/TimeLog/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"

            }
        ]

    });
}