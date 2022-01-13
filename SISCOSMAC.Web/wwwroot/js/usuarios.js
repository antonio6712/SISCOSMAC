var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblUsuarios").DataTable({
        "ajax": {
            //ejemplo con area "url": "/admin/categorias/GetAll",
            "url": "/usuarios/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            
            { "data": "nombre", "width": "10%" },
            { "data": "aPaterno", "width": "10%" },
            { "data": "aMaterno", "width": "10%" },
            { "data": "rol", "width": "10%" },            
            { "data": "nombreDeptoPer", "width": "5%" },
            {
                "data": "usuarioId", 
                "render": function (data) {
                    return `<div class="text-center ">
                            <a href='/usuarios/Editar/${data}' class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                             <i class='fas fa-edit'></i> Editar
                            </a>
                            
                            <a onclick=Delete("/usuarios/Eliminar/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                             <i class='fas fa-trash-alt'></i> Borrar
                            </a>

                            
                            `;
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "No hay registros"
        },
        "width": "100%"
    });
}


function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, Borrar!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}

