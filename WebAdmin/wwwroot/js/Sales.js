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

   
    var row = '';
    
    items = response;
    row += '<div class="text-right">';
    row += '<button class="btn btn-warning" data-toggle="modal" data-target="#modalAgregarComments">Crear</button>';
    row += '</div>';
    row += '<table  class="table  table-condensed  table-hover wrap dt-responsive cell-border compact stripe row-border" style="width:100%">'
    row += '<thead><tr><th>By/Datetime</th><th>Title/Comment</th></tr></thead>'
    $.each(items, function (index, val) {
        
        row += '<tr>'
        row += '<td><b>' + val.commentBy + '</b><br> <p>' + val.commentDatetime + '</p></td>';
        row += '<td><b>' + val.title + '</b> <br> <p>' + val.comment + '</p></td>';
        //row += '<td>' + val.comment + '</td>';
        row += '</tr>'

    });

    row += '</table>'

    $('#modalcomment').html(row);
}