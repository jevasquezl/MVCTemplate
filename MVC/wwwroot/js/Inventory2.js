let datatable;
//$(document).ready(function () {
//    loadDataTable();
//});
$(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Inventory/Inventory/GetAll"
            },
            "columns":
                [
                    {
                        "data": "store.name"
                    },
                    {
                        "data": "product.description"
                    },
                    {
                        "data": "product.cost", "className": "text-end",
                        "render": function (data) {
                            var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                            return d
                        }

                    },
                    {
                        "data": "quantity", "className": "text-end"
                    },
                ]
        });
}
