let datatable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable()
{
    datatable = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/Admin/User/GetAll"
            },
            "columns":
                [
                    {
                        "data": "email", "width": "20%"
                    },
                    {
                        "data": "names", "width": "40%"
                    },
                    {
                        "data" : "role"
                    },
                    //{
                    //    "data": "id",
                    //    "render": function (data)
                    //    {
                    //    return `
                    //        <div class="text-center">
                    //            <a href="/Admin/Category/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                    //                <i class="bi bi-pencil-square"></i>
                    //            </a>
                    //            <a onclick=Remove("/Admin/Category/Remove/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                    //                <i class="bi bi-trash3-fill"></i>
                    //            </a>
                    //        </div>
                    //    `;
                    //    },"width": "20%"
                    //}
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



