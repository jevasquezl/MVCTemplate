let datatable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable()
{
    datatable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/Marca/GetAll"
            },
            "columns":
                [
                    {
                        "data": "name", "width": "20%"
                    },
                    {
                        "data": "description", "width": "40%"
                    },
                    {
                        "data": "state", 
                        "render": function (data)
                        {
                            if (data == true)
                            {
                                return "Active";
                            }
                            else {
                                return "Disable";
                            }
                        }, "width" : "20%"
                    },
                    {
                        "data": "id",
                        "render": function (data)
                        {
                        return `
                            <div class="text-center">
                                <a href="/Admin/Marca/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a onclick=Remove("/Admin/Marca/Remove/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="bi bi-trash3-fill"></i>
                                </a>
                            </div>
                        `;
                        },"width": "20%"
                    }
                ]
        }
    );
};

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
    })
}



