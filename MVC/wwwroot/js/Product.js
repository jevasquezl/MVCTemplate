let datatable;
//$(document).ready(function () {
//    loadDataTable();
//});
$(function () {
    loadDataTable();
});
function loadDataTable()
{
    datatable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Product/GetAll"
            },
            "columns":
                [
                    {
                        "data": "serie"
                    },
                    {
                        "data": "description"
                    },
                    {
                        "data": "category.name"
                    },
                    {
                        "data": "brand.name"
                    },
                    {
                        "data": "price", "className": "text-end",
                        "render": function (data) {
                            var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                            return d
                        }
                    },
                    {
                        "data": "state",
                        "render": function (data) {
                            if (data == true) {
                                return "Active";
                            }
                            else {
                                return "Disable";
                            }
                        }
                    },
                    {
                        "data": "id",
                        "render": function (data) {
                            return `
                            <div class="text-center">
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a onclick=Remove("/Admin/Product/Remove/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="bi bi-trash3-fill"></i>
                                </a>
                            </div>
                        `;
                        }
                    }
                ]
        }
    );
}

function Remove(url) {
    Swal.fire({
        title: "Esta seguro de Eliminar",
        text: "Registo no podra ser recuperado",
        icon: "warning",
        showCloseButton: true,
        showCancelButton: true,
        dangerMode: true
    }).then((accion) => {
        if (accion.isConfirmed) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.Messages);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.Messages);
                    }
                }
            });
        }
    });
}



