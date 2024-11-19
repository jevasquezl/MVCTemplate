let datatable;
//$(document).ready(function () {
//    loadDataTable();
//});
$(function () {
    loadDataTable();
});
function loadDataTable()
{
    datatable = $('#tblData').DataTable
    ({
            "ajax": {
                "url": "/Admin/User/GetAll"
            },
            "columns":
                [
                    {
                        "data": "email"
                    },
                    {
                        "data": "names"
                    },
                    {
                        "data": "role"
                    },
                    {
                        "data": {
                            id: "id", LockoutEnd: "LockoutEnd"
                        },
                        "render": function (data)
                        {
                            let today = new Date().getTime();
                            let lock = new Date(data.lockoutEnd).getTime();
                            if (lock > today) {
                                return `
                                    <div class="text-center">
                                        <a onclick=lockandUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer", width:150px>
                                            <i class="bi bi-unlock-fill"></i> Desbloquear
                                        </a>
                                    </div>
                                `;
                            }
                            else {
                                return `
                                    <div class="text-center">
                                        <a onclick=lockandUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer", width:150px>
                                            <i class="bi bi-lock-fill"></i> Bloquear
                                        </a>
                                    </div>
                                `;
                            }
                        }
                    }
            ]
    });
}

function lockandUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/lockandUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message,'', { timeOut: 1000 });                
                datatable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}

