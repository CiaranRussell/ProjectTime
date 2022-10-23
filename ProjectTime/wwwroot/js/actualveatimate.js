var dataTable;


$(document).ready(function () {

    
    loadDataTable();
});


function loadDataTable() {
    

    dataTable = $('#tblDataActualVEstimate').DataTable({

        "ajax": { "url": "/SuperUser/ActualVEstimate/IndexAPI", "type": "GET", "datatype": "json"},

        "columns": [

            { "data": "project.name", "width": "10%" },
            {
                "data": "estimateMinDate", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "15%"
            },
            { "data": "estimateMaxDate", "width": "10%" },
            { "data": "estimateDurationDays", "width": "10%" },
            { "data": "estimateTotalCost", "width": "10%" },
            {
                "data": "minDate", render: function (data) {
                return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            {
                "data": "maxDate", render: function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }, "width": "10%"
            },
            { "data": "duration", "width": "10%" },
            { "data": "totalCost", "width": "10%" },
            {
                "data": "project.id",
                "render": function (data) {
                    return `
                        <div class="w-50 btn-group" role="group">
                        <a href="/SuperUser/ActualVEstimate/IndexProjectTracker?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-clipboard-data"></i>Project Tracker</a>
					</div>
                          `
                },
                "width": "10%"

            }
        ]

    });
}