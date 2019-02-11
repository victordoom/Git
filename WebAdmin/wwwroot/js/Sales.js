$('#modalComments').on('shown.bs.modal', function () {

});

function getComments(id, action) {
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            //los datos que obtenemos con nuestra funcion se las vamos a pasar a la funcion mostarUsuario
            mostrarComments(response);
        }
    });
}

function mostrarComments(response) {
    items = response;
    var row;
    $.each(items, function (index, val) {
        row += "<tr><td>" + val.comment + "</td></tr>";

    });

    $("#mitabla").append(row);
}