var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblSolicitudes").DataTable({
        "ajax": {
            //ejemplo con area "url": "/admin/categorias/GetAll",
            "url": "/SolicitudMantenimiento/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "folio", "width": "5%" },
            { "data": "departamentoDirigido", "width": "20%" },
            { "data": "fechaElaboracion", "width": "20%" },
            {
                "data": "solicitudId",
                "render": function (data) {
                    return `<div class="text-center">

                        <a href='/solicitudMantenimiento/EditarSolicitud/${data}' class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                            <i class='fas fa-edit'></i> Editar
                        </a>

                        <a onclick=Delete("/SolicitudMantenimiento/EliminarSolicitud/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                            <i class='fas fa-trash-alt'></i> Borrar
                        </a>
                                                        
                        <a href="/SolicitudMantenimiento/PrintView?controlador=SolicitudMantenimiento&accion=SolicitudPDF&IdSolicitud=${data}" target="_blank" class='btn btn-secondary text-white' style='cursor:pointer; width:200px; height:38px;'>
                            <i class="fas fa-plus-square"></i> Generar PDF   
                        </a>

                            `;
                }, "width": "50%"
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
