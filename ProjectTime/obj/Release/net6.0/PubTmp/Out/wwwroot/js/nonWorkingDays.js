let dataTable;

$(document).ready(function () {

    loadDataTable();

});

function loadDataTable() {

    dataTable = $('#tblDataNonWorkingDays').DataTable({
        "ajax": {
            "url": "/Admin/NonWorkingDays/IndexAPI","type": "GET","datatype": "json"
        },
        "columns": [

            {"data": "date", render: function (data) {
                   return moment(data).format('DD/MM/YYYY');
                }, "width": "25%"},
            { "data": "description", "width": "25%" },
            { "data": "allowTimeLog", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/NonWorkingDays/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="/Admin/NonWorkingDays/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
        ]

    });
}