var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblListaOrdenes").DataTable({
        "ajax": {
            //ejemplo con area "url": "/admin/categorias/GetAll",
            "url": "/OrdenTrabajo/ListaOrdenesGetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "numeroControl", "width": "5%" },
            { "data": "asignado", "width": "20%" },
            { "data": "fechaRealizacion", "width": "20%" },
            {
                "data": "ordenId",
                "render": function (data) {
                    return `<div class="text-center">

                            
                            <a href='/OrdenTrabajo/EditarOrden/${data}' class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                <i class='fas fa-edit'></i> Editar
                            </a>
                            
                            <a onclick=Delete("/OrdenTrabajo/EliminarOrden/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                                <i class='fas fa-trash-alt'></i> Borrar
                            </a>

                            <a  href="/OrdenTrabajo/PrintView?controlador=OrdenTrabajo&accion=OrdenPDF&IdSolicitud=${data}" class='btn btn-secondary text-white' style='cursor:pointer; width:200px; height:38px;'>
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
