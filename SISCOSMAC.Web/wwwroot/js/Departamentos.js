var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblDepartamentos").DataTable({
        "ajax": {
            //ejemplo con area "url": "/admin/categorias/GetAll",
            "url": "/Departamentos/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "departamentoId", "width": "5%" },
            { "data": "nombreDepartamento", "width": "50%" },
            { "data": "ordenTrabajo", "width": "20%" },
            {
                "data": "departamentoId",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href='/Departamentos/Editar/${data}' class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                            <i class='fas fa-edit'></i> Editar
                            </a>
                            
                            <a onclick=Delete("/departamentos/Delete/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                            <i class='fas fa-trash-alt'></i> Borrar
                            </a>
                            `;
                }, "width": "30%"
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

