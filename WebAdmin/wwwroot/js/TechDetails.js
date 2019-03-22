//listar seguimiento de detalles en TechFiles
$(document).ready(function () {
    var id = $('input[name=contractid]')[0].value;

    //    $.ajax({
    //        type: "POST",
    //        url: '../AnexosTechFiles',
    //        data: { id },
    //        success: function (response) {

                

    //        }
    //});


     $("#techanexos").DataTable({

        "ajax": {
            "url": "../AnexosTechFiles/" + id + "",
            "type": "POST",
            "datatype": "json",
            //"dataSrc": function (response) {
            //    var prueba = response
            //}
            

        },
        "columnDefs":
            [{
                //"targets": [0],
                "visible": false,
                "searchable": false,

            }],
        "columns": [
            {
                "data": "idattachfile",
                "name": "Idattachfile"
            },
            {
                data: " attachDate",
                name: " AttachDate",
                render: function (data, type, row) {
                    
                    return moment(row.attachDate).format("MM/DD/YYYY");
                }
            },
            {
                data: "fileName",
                name: "FileName"
            },
            {
                data: "fileType",
                name: "fileType"
            },
            {
                data: "fileType",
                name: "fileType",
                render: function (data, type, row) {


                    return '<a href="http://96.231.33.87:7778/' + row.stringId + '/' + row.fileName + '" target="_blank"> <img src="../../images/' + row.fileType + '.png" ) height="24" width="24" alt="Attachment Document" /></a>';
                }
            },

        ],

        "processing": true, // for show progress bar  
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "serverSide": true, // for process server side  
        stateSave: true,
        //"initComplete": function (oSettings, json) {
        //    memobusqueda(json);
        //    // alert("Datos cargados exitosamente");
        //}
    });

});