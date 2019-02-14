$('#modalComments').on('shown.bs.modal', function () {

});

var j = 0;

var userLogeado;

function getComments(email, action) {
    $.ajax({
        type: "POST",
        url: action,
        data: { email },
        success: function (response) {

            if (response.length == 0) {
                nohaycomments()

            } else {
            //los datos que obtenemos con nuestra funcion se las vamos a pasar a la funcion mostarUsuario
            $('input[name=Idby]').val(response[0].userLogeado);
            $('input[name=User]').val(response[0].userLogeadoNombre);
            $('input[name=Idto]').val(response[0].userSelect);
            $('input[name=UserSelect]').val(response[0].userSelectNombre);
            mostrarComments(response);
            }

        }
    });
}

function nohaycomments() {
    var row = '';
    row += '<div class="text-right">';
    row += '<button class="btn btn-warning" data-toggle="modal" data-target="#modalAgregarComments">New</button>';
    row += '</div>';
    row += '<table  class="table  table-condensed  table-hover wrap dt-responsive cell-border compact stripe row-border" style="width:100%">'
    row += '<thead><tr><th>By/Datetime</th><th>Title/Comment</th></tr></thead>'
    row += '</table>'

    $('#modalcomment').html(row);
}
function mostrarComments(response) {
    j = 0;
   
    var row = '';
    
    items = response;
    row += '<div class="text-right">';
    row += '<button class="btn btn-warning" data-toggle="modal" data-target="#modalAgregarComments">New</button>';
    row += '</div>';
    row += '<table  class="table  table-condensed  table-hover wrap dt-responsive cell-border compact stripe row-border" style="width:100%">'
    row += '<thead><tr><th>By/Datetime</th><th>Title/Comment</th></tr></thead>'
    $.each(items, function (index, val) {
        
        row += '<tr>'
        row += '<td><b>' + val.nombre + '</b><br> <p>' + val.commentDatetime + '</p></td>';
        row += '<td><b>' + val.title + '</b> <br> <p>' + val.comment + '</p></td>';
        //row += '<td>' + val.comment + '</td>';
        row += '</tr>'

    });

    row += '</table>'

    $('#modalcomment').html(row);
}

function getAdministra(action) {
    $.ajax({
        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j == 0) {
                for (var i = 0; i < response.length; i++) {
                    document.getElementById('Select').options[i] = new Option(response[i].text, response[i].value);
                   // document.getElementById('SelectNuevo').options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }
        }
    });
}


function agregarComment(action) {
    var idby = $('input[name=Idby]')[0].value;
    var idto;
    var esadmin = $('input[name=EsAdmin]')[0].value;

    if (esadmin == "Admin") {
        var siadmin = $('input[name=Idto]')[0].value;
        idto = siadmin

    } else {
        var admin = document.getElementById('Select');
        var selectAdmin = admin.options[admin.selectedIndex].value;

        idto = selectAdmin
    }

    var comment = $('textarea[name=Comment]')[0].value;
    var title = $('input[name=Title]')[0].value;


    if (idto == "0") {
        $('#Select').focus();
    } else {
        if (title == "") {
            $('#Title').focus();
        } else {

            if (title == "") {
                $('#Title').focus();
            } else {

                if (comment == "") {
                    $('#Comment').focus();
                } else {


                    $.ajax({
                        type: "POST",
                        url: action,
                        data: {
                            idby, idto, comment, title
                        },
                        success: function (response) {
                            if (response = true) {
                                window.location.href = "SalesComments";
                            } else {
                                alert("The comment could not be saved");
                            }



                        }
                    });

                }
            }
        }

    }
    
    
}


