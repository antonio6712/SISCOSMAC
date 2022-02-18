var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblOrdenes").DataTable({
        "ajax": {
            //ejemplo con area "url": "/admin/categorias/GetAll",
            "url": "/OrdenTrabajo/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "folio", "width": "5%" },
            { "data": "areaSolicitante", "width": "20%" },
            { "data": "departamentoDirigido", "width": "20%" },
            {
                "data": "solicitudId",
                "render": function (data) {
                    return `<div class="text-center">

                            
                            <a href="/OrdenTrabajo/CrearOrden/${data}" class='btn btn-primary text-white' style='cursor:pointer; width:300px; height:38px;'>
                                <i class="fas fa-plus-square"></i>Crear Orden de trabajo   
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





